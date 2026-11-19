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
                    case "2":Dodaj(); break;
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
    }
}
