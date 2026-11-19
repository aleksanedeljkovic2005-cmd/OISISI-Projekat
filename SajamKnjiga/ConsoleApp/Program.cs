using Core.DAO;
using Core.Models;
using ConsoleApp.Meniji;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            bool radi = true;
            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== SAJAM KNJIGA =====");
                Console.WriteLine("1 - Posetioci");
                Console.WriteLine("2 - Autori");
                Console.WriteLine("3 - Knjige");
                Console.WriteLine("4 - Kupovine");
                Console.WriteLine("5 - Izdavači");
                Console.WriteLine("0 - Izlaz");
                Console.Write("Izaberite opciju: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": new PosetilacMeni().Pokreni(); break;
                    case "2": new AutorMeni().Pokreni(); break;
                    case "3": new KnjigaMeni().Pokreni(); break;
                    case "4": new KupovinaMeni().Pokreni(); break;
                    case "5": new IzdavacMeni().Pokreni(); break;
                    case "0": radi = false;
                              Console.WriteLine("Doviđenja!"); break;
                    default: Console.WriteLine("Nepostojeća opcija, pokušajte ponovo."); break;
                }
            }
        }
    }
}
