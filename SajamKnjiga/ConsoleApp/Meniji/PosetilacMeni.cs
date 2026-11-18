using System;
using System.Collections.Generic;
using System.Text;
using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class PosetilacMeni
    {
        private readonly PosetilacDAO posetilacDAO = new PosetilacDAO();
        private readonly AdresaDAO adresaDAO = new AdresaDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== POSETIOCI =====");
                Console.WriteLine("1 - Prikaz svih posetilaca");
                Console.WriteLine("2 - Dodavanje posetioca");
                Console.WriteLine("3 - Izmena posetioca");
                Console.WriteLine("4 - Brisanje posetioca");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izaberite opciju: ");

                string? izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":Prikazi();break;
                    case "2":Console.WriteLine("Još nije urađeno.");break;
                    case "3":Console.WriteLine("Još nije urađeno.");break;
                    case "4":Console.WriteLine("Još nije urađeno.");break;
                    case "0":radi = false;break;
                    default:Console.WriteLine("Nepostojeća opcija, pokušajte ponovo.");break;
                }
            }
        }

        private void Prikazi()
        {
            List<Posetilac> posetioci = posetilacDAO.UcitajSve();

            if (posetioci.Count == 0)
            {
                Console.WriteLine("Nema registrovanih posetilaca.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"Br. karte",-12}{"Ime",-12}{"Prezime",-14}{"Status",-8}{"Email",-22}{"Adresa"}");
            Console.WriteLine(new string('-', 100));

            foreach (Posetilac p in posetioci)
            {
                Adresa? adresa = adresaDAO.NadjiPoId(p.AdresaId);
                string adresaTekst = adresa != null ? adresa.ToString() : "nepoznata";

                Console.WriteLine($"{p.Id,-4}{p.BrojClanskeKarte,-12}{p.Ime,-12}{p.Prezime,-14}{p.Status,-8}{p.Email,-22}{adresaTekst}");
            }
        }
    }
}
