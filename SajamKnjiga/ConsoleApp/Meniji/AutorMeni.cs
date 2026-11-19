using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class AutorMeni
    {
        private readonly AutorDAO autorDAO = new AutorDAO();
        private readonly AdresaDAO adresaDAO = new AdresaDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== AUTORI =====");
                Console.WriteLine("1 - Prikaz svih autora");
                Console.WriteLine("2 - Dodavanje autora");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izaberite opciju: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": Prikazi(); break;
                    case "2": Dodaj(); break;
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
    }
}