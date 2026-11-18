using System.Globalization;
using Core.Utils;

namespace Core.Models
{
    public class Autor : ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateOnly DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public string Telefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojLicneKarte { get; set; } = "";
        public int GodineIskustva { get; set; }
        public List<int> KnjigeIds { get; set; } = new List<int>();

        public Autor() { }

        public string ToLine()
        {
            return string.Join("|",
                Id,
                Ime,
                Prezime,
                DatumRodjenja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                AdresaId,
                Telefon,
                Email,
                BrojLicneKarte,
                GodineIskustva,
                string.Join(",", KnjigeIds));
        }

        public void FromLine(string line)
        {
            string[] d = line.Split('|');
            Id = int.Parse(d[0]);
            Ime = d[1];
            Prezime = d[2];
            DatumRodjenja = DateOnly.ParseExact(d[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            AdresaId = int.Parse(d[4]);
            Telefon = d[5];
            Email = d[6];
            BrojLicneKarte = d[7];
            GodineIskustva = int.Parse(d[8]);
            KnjigeIds = d[9]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        public override string ToString()
        {
            return $"{Ime} {Prezime}";
        }
    }
}