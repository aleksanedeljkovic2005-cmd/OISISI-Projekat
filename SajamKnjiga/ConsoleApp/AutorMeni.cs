using Core.DAO;
using Core.Models;

namespace ConsoleApp
{
    public static class AutorMeni
    {
        private static readonly AutorDAO autorDAO = new AutorDAO();

        public static void Pokreni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== AUTORI =====");
                Console.WriteLine("1 - Prikaz svih autora");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izaberite opciju: ");
                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": PrikaziSve(); break;
                    case "0": return;
                    default: Console.WriteLine("Nepostojeća opcija."); break;
                }
            }
        }

        private static void PrikaziSve()
        {
            List<Autor> autori = autorDAO.UcitajSve();
            if (autori.Count == 0)
            {
                Console.WriteLine("Nema unetih autora.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"Ime",-12}{"Prezime",-15}{"Datum rođ.",-13}{"Telefon",-13}{"Email",-24}{"Iskustvo",8}");
            Console.WriteLine(new string('-', 89));

            foreach (Autor a in autori)
            {
                Console.WriteLine($"{a.Id,-4}{a.Ime,-12}{a.Prezime,-15}{a.DatumRodjenja,-13:dd.MM.yyyy}{a.Telefon,-13}{a.Email,-24}{a.GodineIskustva,8}");
            }
        }
    }
}