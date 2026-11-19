using System.Globalization;
using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class KnjigaMeni
    {
        private readonly KnjigaDAO knjigaDAO = new KnjigaDAO();
        private readonly AutorDAO autorDAO = new AutorDAO();
        private readonly IzdavacDAO izdavacDAO = new IzdavacDAO();
        private readonly KupovinaDAO kupovinaDAO = new KupovinaDAO();
        private readonly PosetilacDAO posetilacDAO = new PosetilacDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== KNJIGE =====");
                Console.WriteLine("1 - Prikaz svih knjiga");
                Console.WriteLine("2 - Dodavanje knjige");
                Console.WriteLine("3 - Izmena knjige");
                Console.WriteLine("4 - Brisanje knjige");
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
            List<Knjiga> knjige = knjigaDAO.UcitajSve();

            if (knjige.Count == 0)
            {
                Console.WriteLine("Nema unetih knjiga.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"ISBN",-14}{"Naziv",-24}{"Žanr",-12}{"God.",-6}{"Cena",-10}{"Str.",-6}{"Izdavač",-16}{"Autori"}");
            Console.WriteLine(new string('-', 120));

            foreach (Knjiga k in knjige)
            {
                Izdavac? izdavac = izdavacDAO.NadjiPoId(k.IzdavacId);
                string izdavacTekst = izdavac != null ? izdavac.Naziv : "-";

                List<string> imenaAutora = new List<string>();
                foreach (int autorId in k.AutoriIds)
                {
                    Autor? a = autorDAO.NadjiPoId(autorId);
                    if (a != null)
                        imenaAutora.Add(a.ToString());
                }
                string autoriTekst = imenaAutora.Count > 0 ? string.Join(", ", imenaAutora) : "-";

                Console.WriteLine($"{k.Id,-4}{k.ISBN,-14}{k.Naziv,-24}{k.Zanr,-12}{k.GodinaIzdanja,-6}{k.Cena,-10:0.00}{k.BrojStrana,-6}{izdavacTekst,-16}{autoriTekst}");
            }
        }

        private void Dodaj()
        {
            Console.WriteLine();
            Console.WriteLine("--- Dodavanje knjige ---");

            Knjiga knjiga = new Knjiga();

            while (true)
            {
                knjiga.ISBN = Unos.Tekst("ISBN: ");
                if (!knjigaDAO.UcitajSve().Any(k => k.ISBN == knjiga.ISBN))
                    break;
                Console.WriteLine("Knjiga sa tim ISBN-om već postoji.");
            }

            knjiga.Naziv = Unos.Tekst("Naziv: ");
            knjiga.Zanr = Unos.Tekst("Žanr: ");
            knjiga.GodinaIzdanja = Unos.CeoBroj("Godina izdanja: ", 1000, DateTime.Now.Year);
            knjiga.Cena = UnesiCenu("Cena: ");
            knjiga.BrojStrana = Unos.CeoBroj("Broj strana: ", 1, 10000);
            knjiga.AutoriIds = IzaberiAutore();
            knjiga.IzdavacId = IzaberiIzdavaca();

            knjigaDAO.Dodaj(knjiga);

            // Upisujemo knjigu i kod autora i kod izdavača.
            foreach (int autorId in knjiga.AutoriIds)
            {
                Autor? a = autorDAO.NadjiPoId(autorId);
                if (a != null && !a.KnjigeIds.Contains(knjiga.Id))
                {
                    a.KnjigeIds.Add(knjiga.Id);
                    autorDAO.Izmeni(a);
                }
            }

            Izdavac? izdavac = izdavacDAO.NadjiPoId(knjiga.IzdavacId);
            if (izdavac != null && !izdavac.KnjigeIds.Contains(knjiga.Id))
            {
                izdavac.KnjigeIds.Add(knjiga.Id);
                izdavacDAO.Izmeni(izdavac);
            }

            Console.WriteLine($"Knjiga je uspešno dodata (ID: {knjiga.Id}).");
        }

        private void Izmeni()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID knjige za izmenu (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Knjiga? knjiga = knjigaDAO.NadjiPoId(id);
            if (knjiga == null)
            {
                Console.WriteLine("Knjiga sa tim ID-jem ne postoji.");
                return;
            }

            Console.WriteLine("Pritisnite Enter da zadržite trenutnu vrednost.");

            while (true)
            {
                string isbn = Unos.TekstIzmena("ISBN", knjiga.ISBN);
                if (!knjigaDAO.UcitajSve().Any(k => k.ISBN == isbn && k.Id != knjiga.Id))
                {
                    knjiga.ISBN = isbn;
                    break;
                }
                Console.WriteLine("Druga knjiga već ima taj ISBN.");
            }

            knjiga.Naziv = Unos.TekstIzmena("Naziv", knjiga.Naziv);
            knjiga.Zanr = Unos.TekstIzmena("Žanr", knjiga.Zanr);
            knjiga.GodinaIzdanja = Unos.CeoBrojIzmena("Godina izdanja", knjiga.GodinaIzdanja, 1000, DateTime.Now.Year);
            knjiga.Cena = UnesiCenuIzmena("Cena", knjiga.Cena);
            knjiga.BrojStrana = Unos.CeoBrojIzmena("Broj strana", knjiga.BrojStrana, 1, 10000);

            knjigaDAO.Izmeni(knjiga);
            Console.WriteLine("Knjiga je uspešno izmenjena.");
        }

        private void Obrisi()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID knjige za brisanje (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Knjiga? knjiga = knjigaDAO.NadjiPoId(id);
            if (knjiga == null)
            {
                Console.WriteLine("Knjiga sa tim ID-jem ne postoji.");
                return;
            }

            // Knjiga koju je neko kupio ne sme da se obriše.
            if (kupovinaDAO.UcitajSve().Any(k => k.KnjigaId == knjiga.Id))
            {
                Console.WriteLine("Knjiga ne može biti obrisana jer postoje kupovine te knjige.");
                return;
            }

            string potvrda = Unos.Tekst($"Da li ste sigurni da želite da obrišete knjigu \"{knjiga.Naziv}\"? (da/ne): ").ToLower();
            if (potvrda != "da")
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            // Skidamo knjigu sa autora.
            foreach (Autor a in autorDAO.UcitajSve())
            {
                if (a.KnjigeIds.Remove(knjiga.Id))
                    autorDAO.Izmeni(a);
            }

            // Skidamo knjigu sa izdavača.
            foreach (Izdavac i in izdavacDAO.UcitajSve())
            {
                if (i.KnjigeIds.Remove(knjiga.Id))
                    izdavacDAO.Izmeni(i);
            }

            // Skidamo knjigu sa listi želja posetilaca.
            foreach (Posetilac p in posetilacDAO.UcitajSve())
            {
                if (p.ListaZeljaIds.Remove(knjiga.Id))
                    posetilacDAO.Izmeni(p);
            }

            knjigaDAO.Obrisi(knjiga.Id);
            Console.WriteLine("Knjiga je obrisana.");
        }

        private List<int> IzaberiAutore()
        {
            List<Autor> autori = autorDAO.UcitajSve();
            if (autori.Count == 0)
            {
                Console.WriteLine("Nema unetih autora. Knjiga će biti dodata bez autora.");
                return new List<int>();
            }

            Console.WriteLine("Autori:");
            foreach (Autor a in autori)
                Console.WriteLine($"  {a.Id} - {a}");

            while (true)
            {
                Console.Write("Unesite ID-jeve autora odvojene zarezom (npr. 1,2): ");
                string unos = Console.ReadLine() ?? "";
                string[] delovi = unos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                List<int> ids = new List<int>();
                bool ispravno = delovi.Length > 0;

                foreach (string deo in delovi)
                {
                    if (int.TryParse(deo, out int autorId) && autorDAO.NadjiPoId(autorId) != null)
                    {
                        if (!ids.Contains(autorId))
                            ids.Add(autorId);
                    }
                    else
                    {
                        Console.WriteLine($"Autor sa ID-jem \"{deo}\" ne postoji.");
                        ispravno = false;
                    }
                }

                if (ispravno)
                    return ids;

                Console.WriteLine("Pokušajte ponovo.");
            }
        }

        private int IzaberiIzdavaca()
        {
            List<Izdavac> izdavaci = izdavacDAO.UcitajSve();
            if (izdavaci.Count == 0)
            {
                Console.WriteLine("Nema unetih izdavača. Knjiga će privremeno biti bez izdavača (ID 0).");
                return 0;
            }

            Console.WriteLine("Izdavači:");
            foreach (Izdavac i in izdavaci)
                Console.WriteLine($"  {i.Id} - {i.Naziv}");

            while (true)
            {
                int izdavacId = Unos.CeoBroj("Unesite ID izdavača: ", 1, int.MaxValue);
                if (izdavacDAO.NadjiPoId(izdavacId) != null)
                    return izdavacId;
                Console.WriteLine("Izdavač sa tim ID-jem ne postoji.");
            }
        }

        private static decimal UnesiCenu(string poruka)
        {
            while (true)
            {
                Console.Write(poruka);
                string unos = (Console.ReadLine() ?? "").Replace(',', '.');
                if (decimal.TryParse(unos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal cena) && cena >= 0)
                    return cena;
                Console.WriteLine("Unesite ispravnu cenu (npr. 1200 ili 999.99).");
            }
        }

        private static decimal UnesiCenuIzmena(string poruka, decimal stara)
        {
            while (true)
            {
                Console.Write($"{poruka} [{stara:0.00}]: ");
                string unos = (Console.ReadLine() ?? "").Replace(',', '.');
                if (string.IsNullOrWhiteSpace(unos))
                    return stara;
                if (decimal.TryParse(unos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal cena) && cena >= 0)
                    return cena;
                Console.WriteLine("Unesite ispravnu cenu (npr. 1200 ili 999.99).");
            }
        }
    }
}