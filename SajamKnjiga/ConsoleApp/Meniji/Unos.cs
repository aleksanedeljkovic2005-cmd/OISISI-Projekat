using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ConsoleApp.Meniji
{
    public static class Unos
    {
        public static string Tekst(string poruka)
        {
            while (true)
            {
                Console.Write(poruka);
                string? unos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(unos) && !unos.Contains('|'))
                    return unos.Trim();
                Console.WriteLine("Unos ne sme biti prazan niti sadržati znak |.");
            }
        }

        public static int CeoBroj(string poruka, int min, int max)
        {
            while (true)
            {
                Console.Write(poruka);
                if (int.TryParse(Console.ReadLine(), out int broj) && broj >= min && broj <= max)
                    return broj;
                Console.WriteLine($"Unesite ceo broj od {min} do {max}.");
            }
        }

        public static DateTime Datum(string poruka)
        {
            while (true)
            {
                Console.Write(poruka + " (dd.MM.yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum))
                    return datum;
                Console.WriteLine("Neispravan datum. Primer: 25.01.1999");
            }
        }

        public static string Email(string poruka)
        {
            while (true)
            {
                string email = Tekst(poruka);
                if (email.Contains('@') && email.Contains('.'))
                    return email;
                Console.WriteLine("Neispravan email.");
            }
        }
    }
}
