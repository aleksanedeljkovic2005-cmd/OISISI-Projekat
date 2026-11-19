using System;
using System.Collections.Generic;
using System.Text;
using Core.DAO;
using Core.Models;

namespace ConsoleApp.Meniji
{
    public class KupovinaMeni
    {
        private readonly KupovinaDAO kupovinaDAO = new KupovinaDAO();
        private readonly PosetilacDAO posetilacDAO = new PosetilacDAO();
        private readonly KnjigaDAO knjigaDAO = new KnjigaDAO();

        public void Pokreni()
        {
            bool radi = true;

            while (radi)
            {
                Console.WriteLine();
                Console.WriteLine("===== KUPOVINE =====");
                Console.WriteLine("1 - Prikaz svih kupovina");
                Console.WriteLine("2 - Dodavanje kupovine");
                Console.WriteLine("3 - Izmena kupovine");
                Console.WriteLine("4 - Brisanje kupovine");
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
            List<Kupovina> kupovine = kupovinaDAO.UcitajSve();

            if (kupovine.Count == 0)
            {
                Console.WriteLine("Nema evidentiranih kupovina.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-4}{"Posetilac",-22}{"Knjiga",-26}{"Datum",-12}{"Ocena",-7}{"Komentar"}");
            Console.WriteLine(new string('-', 95));

            foreach (Kupovina k in kupovine)
            {
                Posetilac? p = posetilacDAO.NadjiPoId(k.PosetilacId);
                Knjiga? knjiga = knjigaDAO.NadjiPoId(k.KnjigaId);
                string posetilacTekst = p != null ? $"{p.Ime} {p.Prezime}" : "nepoznat";
                string knjigaTekst = knjiga != null ? knjiga.Naziv : "nepoznata";

                Console.WriteLine($"{k.Id,-4}{posetilacTekst,-22}{knjigaTekst,-26}{k.DatumKupovine:dd.MM.yyyy}  {k.Ocena,-7}{k.Komentar}");
            }
        }

        private void AzurirajProsek(Posetilac p)
        {
            List<int> ocene = kupovinaDAO.UcitajSve()
                .Where(k => k.PosetilacId == p.Id)
                .Select(k => k.Ocena)
                .ToList();

            p.ProsecnaOcena = ocene.Count == 0 ? 0 : Math.Round(ocene.Average(), 2);
            posetilacDAO.Izmeni(p);
        }
    }
}
