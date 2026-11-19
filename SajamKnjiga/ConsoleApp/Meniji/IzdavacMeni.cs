using System;
using System.Collections.Generic;
using System.Text;
using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class IzdavacMeni
    {
        private readonly IzdavacDAO izdavacDAO = new IzdavacDAO();
        private readonly AutorDAO autorDAO = new AutorDAO();
        private readonly KnjigaDAO knjigaDAO = new KnjigaDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== IZDAVAČI =====");
                Console.WriteLine("1 - Prikaz svih izdavača");
                Console.WriteLine("2 - Dodavanje izdavača");
                Console.WriteLine("3 - Izmena izdavača");
                Console.WriteLine("4 - Brisanje izdavača");
                Console.WriteLine("0 - Nazad");
                Console.Write("Izaberite opciju: ");

                switch (Console.ReadLine())
                {
                    case "1": Prikazi(); break;
                    case "2": Console.WriteLine("Još nije urađeno."); break;
                    case "3": Console.WriteLine("Još nije urađeno."); break;
                    case "4": Console.WriteLine("Još nije urađeno."); break;
                    case "0": radi = false; break;
                    default: Console.WriteLine("Nepostojeća opcija, pokušajte ponovo."); break;
                }
            }
        }

        private void Prikazi()
        {
            List<Izdavac> izdavaci = izdavacDAO.UcitajSve();

            if (izdavaci.Count == 0)
            {
                Console.WriteLine("Nema registrovanih izdavača.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"Šifra",-10}{"Naziv",-20}{"Šef",-24}{"Autora",-8}{"Knjiga"}");
            Console.WriteLine(new string('-', 75));

            foreach (Izdavac i in izdavaci)
            {
                Autor? sef = autorDAO.NadjiPoId(i.SefId);
                string sefTekst = sef != null ? $"{sef.Ime} {sef.Prezime}" : "-";

                Console.WriteLine($"{i.Id,-4}{i.Sifra,-10}{i.Naziv,-20}{sefTekst,-24}{i.AutoriIds.Count,-8}{i.KnjigeIds.Count}");
            }
        }
    }
}
