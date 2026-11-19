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
        private readonly KupovinaDAO kupovinaDAO = new KupovinaDAO();


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
                    case "2":Dodaj(); break;
                    case "3":Izmeni();break;
                    case "4":Obrisi();break;
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
        private void Dodaj()
        {
            Console.WriteLine();
            Console.WriteLine("--- Dodavanje posetioca ---");

            string ime = Unos.Tekst("Ime: ");
            string prezime = Unos.Tekst("Prezime: ");
            DateTime datumRodjenja = Unos.Datum("Datum rođenja");
            string telefon = Unos.Tekst("Telefon: ");
            string email = Unos.Email("Email: ");

            Console.WriteLine("Adresa:");
            string ulica = Unos.Tekst("  Ulica: ");
            string broj = Unos.Tekst("  Broj: ");
            string grad = Unos.Tekst("  Grad: ");
            string drzava = Unos.Tekst("  Država: ");
            Adresa adresa = new Adresa(0, ulica, broj, grad, drzava);
            adresaDAO.Dodaj(adresa);

            string brojKarte = Unos.Tekst("Broj članske karte (npr. SK-1-2023): ");
            int godinaClanstva = Unos.CeoBroj("Godina članstva: ", 1900, DateTime.Now.Year);

            StatusPosetioca status;
            while (true)
            {
                string s = Unos.Tekst("Status (R - redovan, V - VIP): ").ToUpper();
                if (s == "R" || s == "V")
                {
                    status = Enum.Parse<StatusPosetioca>(s);
                    break;
                }
                Console.WriteLine("Unesite R ili V.");
            }

            Posetilac posetilac = new Posetilac(0, ime, prezime, datumRodjenja, adresa.Id,
                                                telefon, email, brojKarte, godinaClanstva, status);
            posetilacDAO.Dodaj(posetilac);

            Console.WriteLine($"Posetilac je uspešno dodat (ID: {posetilac.Id}).");
        }
        private void Izmeni()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID posetioca za izmenu (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Posetilac? p = posetilacDAO.NadjiPoId(id);
            if (p == null)
            {
                Console.WriteLine("Posetilac sa tim ID-jem ne postoji.");
                return;
            }

            Console.WriteLine("Pritisnite Enter da zadržite trenutnu vrednost.");
            p.Ime = Unos.TekstIzmena("Ime", p.Ime);
            p.Prezime = Unos.TekstIzmena("Prezime", p.Prezime);
            p.DatumRodjenja = Unos.DatumIzmena("Datum rođenja", p.DatumRodjenja);
            p.Telefon = Unos.TekstIzmena("Telefon", p.Telefon);
            p.Email = Unos.EmailIzmena("Email", p.Email);

            Adresa? adresa = adresaDAO.NadjiPoId(p.AdresaId);
            if (adresa == null)
            {
                adresa = new Adresa();
                adresaDAO.Dodaj(adresa);
                p.AdresaId = adresa.Id;
            }
            Console.WriteLine("Adresa:");
            adresa.Ulica = Unos.TekstIzmena("  Ulica", adresa.Ulica);
            adresa.Broj = Unos.TekstIzmena("  Broj", adresa.Broj);
            adresa.Grad = Unos.TekstIzmena("  Grad", adresa.Grad);
            adresa.Drzava = Unos.TekstIzmena("  Država", adresa.Drzava);
            adresaDAO.Izmeni(adresa);

            p.BrojClanskeKarte = Unos.TekstIzmena("Broj članske karte", p.BrojClanskeKarte);
            p.GodinaClanstva = Unos.CeoBrojIzmena("Godina članstva", p.GodinaClanstva, 1900, DateTime.Now.Year);

            while (true)
            {
                string s = Unos.TekstIzmena("Status (R/V)", p.Status.ToString()).ToUpper();
                if (s == "R" || s == "V")
                {
                    p.Status = Enum.Parse<StatusPosetioca>(s);
                    break;
                }
                Console.WriteLine("Unesite R ili V.");
            }

            posetilacDAO.Izmeni(p);
            Console.WriteLine("Posetilac je uspešno izmenjen.");
        }
        private void Obrisi()
        {
            Prikazi();
            int id = Unos.CeoBroj("Unesite ID posetioca za brisanje (0 - odustani): ", 0, int.MaxValue);
            if (id == 0)
                return;

            Posetilac? p = posetilacDAO.NadjiPoId(id);
            if (p == null)
            {
                Console.WriteLine("Posetilac sa tim ID-jem ne postoji.");
                return;
            }

            string potvrda = Unos.Tekst($"Da li ste sigurni da želite da obrišete posetioca {p.Ime} {p.Prezime}? (da/ne): ").ToLower();
            if (potvrda != "da")
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            List<Kupovina> njegoveKupovine = kupovinaDAO.UcitajSve()
                .Where(k => k.PosetilacId == p.Id)
                .ToList();
            foreach (Kupovina k in njegoveKupovine)
                kupovinaDAO.Obrisi(k.Id);

            
            int adresaId = p.AdresaId;
            posetilacDAO.Obrisi(p.Id);
            bool adresaSeKoristi = posetilacDAO.UcitajSve().Any(x => x.AdresaId == adresaId);
            if (!adresaSeKoristi)
                adresaDAO.Obrisi(adresaId);

            Console.WriteLine($"Posetilac je obrisan (obrisano kupovina: {njegoveKupovine.Count}).");
        }
    }
}
