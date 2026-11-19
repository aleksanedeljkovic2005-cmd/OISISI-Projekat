using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class AutorMeni
    {
        private readonly AutorDAO autorDAO = new AutorDAO();
        private readonly AdresaDAO adresaDAO = new AdresaDAO();
        private readonly KnjigaDAO knjigaDAO = new KnjigaDAO();
        private readonly IzdavacDAO izdavacDAO = new IzdavacDAO();
        private readonly PosetilacDAO posetilacDAO = new PosetilacDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== AUTORI =====");
                Console.WriteLine("1 - Prikaz svih autora");
                Console.WriteLine("2 - Dodavanje autora");
                Console.WriteLine("3 - Izmena autora");
                Console.WriteLine("4 - Brisanje autora");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izaberite opciju: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": Prikazi(); break;
                    case "2": Dodaj(); break;
                    case "3": Izmeni(); break;
                    case "4": Obrisi(); break;
                    case "0": radi = false; break;
                    default: Console.WriteLine("Nepostojeća opcija, pokušajte ponovo."); break;
                }
            }
        }

        private void Prikazi()
        {
            List<Autor> autori = autorDAO.UcitajSve();

            if (autori.Count == 0)
            {
                Console.WriteLine("Nema unetih autora.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"Ime",-12}{"Prezime",-15}{"Datum rođ.",-13}{"Telefon",-13}{"Email",-24}{"Iskustvo",-10}{"Adresa"}");
            Console.WriteLine(new string('-', 120));

            foreach (Autor a in autori)
            {
                Adresa? adresa = adresaDAO.NadjiPoId(a.AdresaId);
                string adresaTekst = adresa != null ? adresa.ToString() : "nepoznata";

                Console.WriteLine($"{a.Id,-4}{a.Ime,-12}{a.Prezime,-15}{a.DatumRodjenja,-13:dd.MM.yyyy}{a.Telefon,-13}{a.Email,-24}{a.GodineIskustva,-10}{adresaTekst}");
            }
        }

        private void Dodaj()
        {
            Console.WriteLine();
            Console.WriteLine("--- Dodavanje autora ---");

            Autor autor = new Autor();
            autor.Ime = Unos.Tekst("Ime: ");
            autor.Prezime = Unos.Tekst("Prezime: ");
            autor.DatumRodjenja = DateOnly.FromDateTime(Unos.Datum("Datum rođenja"));
            autor.Telefon = Unos.Tekst("Telefon: ");
            autor.Email = Unos.Email("Email: ");

            Console.WriteLine("Adresa:");
            string ulica = Unos.Tekst("  Ulica: ");
            string broj = Unos.Tekst("  Broj: ");
            string grad = Unos.Tekst("  Grad: ");
            string drzava = Unos.Tekst("  Država: ");
            Adresa adresa = new Adresa(0, ulica, broj, grad, drzava);
            adresaDAO.Dodaj(adresa);
            autor.AdresaId = adresa.Id;

            autor.BrojLicneKarte = Unos.Tekst("Broj lične karte: ");
            autor.GodineIskustva = Unos.CeoBroj("Godine iskustva: ", 0, 100);

            autorDAO.Dodaj(autor);
            Console.WriteLine($"Autor je uspešno dodat (ID: {autor.Id}).");
        }

        private void Izmeni()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID autora za izmenu (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Autor? autor = autorDAO.NadjiPoId(id);
            if (autor == null)
            {
                Console.WriteLine("Autor sa tim ID-jem ne postoji.");
                return;
            }

            Console.WriteLine("Pritisnite Enter da zadržite trenutnu vrednost.");
            autor.Ime = Unos.TekstIzmena("Ime", autor.Ime);
            autor.Prezime = Unos.TekstIzmena("Prezime", autor.Prezime);

            DateTime stariDatum = autor.DatumRodjenja.ToDateTime(TimeOnly.MinValue);
            autor.DatumRodjenja = DateOnly.FromDateTime(Unos.DatumIzmena("Datum rođenja", stariDatum));

            autor.Telefon = Unos.TekstIzmena("Telefon", autor.Telefon);
            autor.Email = Unos.EmailIzmena("Email", autor.Email);

            Adresa? adresa = adresaDAO.NadjiPoId(autor.AdresaId);
            if (adresa == null)
            {
                adresa = new Adresa();
                adresaDAO.Dodaj(adresa);
                autor.AdresaId = adresa.Id;
            }
            Console.WriteLine("Adresa:");
            adresa.Ulica = Unos.TekstIzmena("  Ulica", adresa.Ulica);
            adresa.Broj = Unos.TekstIzmena("  Broj", adresa.Broj);
            adresa.Grad = Unos.TekstIzmena("  Grad", adresa.Grad);
            adresa.Drzava = Unos.TekstIzmena("  Država", adresa.Drzava);
            adresaDAO.Izmeni(adresa);

            autor.BrojLicneKarte = Unos.TekstIzmena("Broj lične karte", autor.BrojLicneKarte);
            autor.GodineIskustva = Unos.CeoBrojIzmena("Godine iskustva", autor.GodineIskustva, 0, 100);

            autorDAO.Izmeni(autor);
            Console.WriteLine("Autor je uspešno izmenjen.");
        }

        private void Obrisi()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID autora za brisanje (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Autor? autor = autorDAO.NadjiPoId(id);
            if (autor == null)
            {
                Console.WriteLine("Autor sa tim ID-jem ne postoji.");
                return;
            }

            // Autor koji je šef izdavača ne sme da se obriše.
            Izdavac? izdavacSef = izdavacDAO.UcitajSve().FirstOrDefault(i => i.SefId == autor.Id);
            if (izdavacSef != null)
            {
                Console.WriteLine($"Autor ne može biti obrisan jer je šef izdavača \"{izdavacSef.Naziv}\".");
                Console.WriteLine("Prvo promenite šefa tog izdavača.");
                return;
            }

            string potvrda = Unos.Tekst($"Da li ste sigurni da želite da obrišete autora {autor.Ime} {autor.Prezime}? (da/ne): ").ToLower();
            if (potvrda != "da")
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            // Skidamo autora sa svih knjiga.
            int izmenjeneKnjige = 0;
            foreach (Knjiga k in knjigaDAO.UcitajSve())
            {
                if (k.AutoriIds.Remove(autor.Id))
                {
                    knjigaDAO.Izmeni(k);
                    izmenjeneKnjige++;
                }
            }

            // Skidamo autora sa svih izdavača.
            foreach (Izdavac i in izdavacDAO.UcitajSve())
            {
                if (i.AutoriIds.Remove(autor.Id))
                    izdavacDAO.Izmeni(i);
            }

            int adresaId = autor.AdresaId;
            autorDAO.Obrisi(autor.Id);

            // Brišemo adresu samo ako je niko drugi ne koristi.
            bool adresaSeKoristi = autorDAO.UcitajSve().Any(a => a.AdresaId == adresaId)
                                || posetilacDAO.UcitajSve().Any(p => p.AdresaId == adresaId);
            if (!adresaSeKoristi)
                adresaDAO.Obrisi(adresaId);

            Console.WriteLine($"Autor je obrisan (uklonjen sa {izmenjeneKnjige} knjiga).");
        }
    }
}