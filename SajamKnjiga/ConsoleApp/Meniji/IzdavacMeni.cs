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
        private int IzaberiSefa(int trenutniSefId)
        {
            List<Autor> kandidati = autorDAO.UcitajSve()
                .Where(a => a.GodineIskustva >= 5)
                .ToList();

            if (kandidati.Count == 0)
            {
                Console.WriteLine("Nema autora sa najmanje 5 godina iskustva - šef ne može biti postavljen.");
                return 0;
            }

            Console.WriteLine("Mogući šefovi (autori sa >= 5 godina iskustva):");
            foreach (Autor a in kandidati)
                Console.WriteLine($"  {a.Id} - {a.Ime} {a.Prezime} ({a.GodineIskustva} god.)");

            while (true)
            {
                int id = trenutniSefId == 0
                    ? Unos.CeoBroj("ID šefa: ", 1, int.MaxValue)
                    : Unos.CeoBrojIzmena("ID šefa", trenutniSefId, 1, int.MaxValue);

                if (kandidati.Any(a => a.Id == id))
                    return id;

                Console.WriteLine("Izabrani autor ne postoji ili nema 5 godina iskustva.");
            }
        }

        private void Dodaj()
        {
            Console.WriteLine();
            Console.WriteLine("--- Dodavanje izdavača ---");

            string sifra = Unos.Tekst("Šifra (npr. IZD-03): ");
            if (izdavacDAO.UcitajSve().Any(i => i.Sifra.ToLower() == sifra.ToLower()))
            {
                Console.WriteLine("Izdavač sa tom šifrom već postoji.");
                return;
            }

            string naziv = Unos.Tekst("Naziv: ");
            int sefId = IzaberiSefa(0);

            Izdavac izdavac = new Izdavac(0, sifra, naziv, sefId);
            if (sefId != 0)
                izdavac.AutoriIds.Add(sefId);

            izdavacDAO.Dodaj(izdavac);
            Console.WriteLine($"Izdavač je uspešno dodat (ID: {izdavac.Id}).");
        }
        private void Izmeni()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID izdavača za izmenu (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Izdavac? izdavac = izdavacDAO.NadjiPoId(id);
            if (izdavac == null)
            {
                Console.WriteLine("Izdavač sa tim ID-jem ne postoji.");
                return;
            }

            Console.WriteLine("Pritisnite Enter da zadržite trenutnu vrednost.");

            string novaSifra = Unos.TekstIzmena("Šifra", izdavac.Sifra);
            if (izdavacDAO.UcitajSve().Any(i => i.Id != izdavac.Id && i.Sifra.ToLower() == novaSifra.ToLower()))
            {
                Console.WriteLine("Drugi izdavač već ima tu šifru. Izmena otkazana.");
                return;
            }
            izdavac.Sifra = novaSifra;
            izdavac.Naziv = Unos.TekstIzmena("Naziv", izdavac.Naziv);

            int noviSefId = IzaberiSefa(izdavac.SefId);
            if (noviSefId != 0)
            {
                izdavac.SefId = noviSefId;
                if (!izdavac.AutoriIds.Contains(noviSefId))
                    izdavac.AutoriIds.Add(noviSefId);
            }

            izdavacDAO.Izmeni(izdavac);
            Console.WriteLine("Izdavač je uspešno izmenjen.");
        }
        private void Obrisi()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID izdavača za brisanje (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Izdavac? izdavac = izdavacDAO.NadjiPoId(id);
            if (izdavac == null)
            {
                Console.WriteLine("Izdavač sa tim ID-jem ne postoji.");
                return;
            }

            if (Unos.Tekst($"Da li ste sigurni da želite da obrišete izdavača {izdavac.Naziv}? (da/ne): ").ToLower() != "da")
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            // knjige ovog izdavaca ostaju bez izdavaca
            List<Knjiga> njegoveKnjige = knjigaDAO.UcitajSve()
                .Where(k => k.IzdavacId == izdavac.Id)
                .ToList();
            foreach (Knjiga k in njegoveKnjige)
            {
                k.IzdavacId = 0;
                knjigaDAO.Izmeni(k);
            }

            izdavacDAO.Obrisi(izdavac.Id);
            Console.WriteLine($"Izdavač je obrisan (knjiga bez izdavača: {njegoveKnjige.Count}).");
        }
    }
}
