using Core.DAO;
using Core.Models;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
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
                    case "1": Console.WriteLine("Izabrali ste Posetioce."); break;
                    case "2": Console.WriteLine("Izabrali ste Autore."); break;
                    case "3": Console.WriteLine("Izabrali ste Knjige."); break;
                    case "4": Console.WriteLine("Izabrali ste Kupovine."); break;
                    case "5": Console.WriteLine("Izabrali ste Izdavače."); break;
                    case "0": radi = false;
                              Console.WriteLine("Doviđenja!"); break;
                    default: Console.WriteLine("Nepostojeća opcija, pokušajte ponovo."); break;
                }
            }
        }
    }
}
