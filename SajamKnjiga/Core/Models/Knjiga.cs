using System.Globalization;
using Core.Utils;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = "";
        public string Naziv { get; set; } = "";
        public string Zanr { get; set; } = "";
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }
        public List<int> AutoriIds { get; set; } = new List<int>();
        public int IzdavacId { get; set; }
        public List<int> KupciIds { get; set; } = new List<int>();
        public List<int> ListaZeljaIds { get; set; } = new List<int>();

        public Knjiga() { }

        public string ToLine()
        {
            return string.Join("|",
                Id,
                ISBN,
                Naziv,
                Zanr,
                GodinaIzdanja,
                Cena.ToString(CultureInfo.InvariantCulture),
                BrojStrana,
                string.Join(",", AutoriIds),
                IzdavacId,
                string.Join(",", KupciIds),
                string.Join(",", ListaZeljaIds));
        }

        public void FromLine(string line)
        {
            string[] d = line.Split('|');
            Id = int.Parse(d[0]);
            ISBN = d[1];
            Naziv = d[2];
            Zanr = d[3];
            GodinaIzdanja = int.Parse(d[4]);
            Cena = decimal.Parse(d[5], CultureInfo.InvariantCulture);
            BrojStrana = int.Parse(d[6]);
            AutoriIds = UcitajIdeve(d[7]);
            IzdavacId = int.Parse(d[8]);
            KupciIds = UcitajIdeve(d[9]);
            ListaZeljaIds = UcitajIdeve(d[10]);
        }

        private static List<int> UcitajIdeve(string tekst)
        {
            return tekst
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        public override string ToString()
        {
            return $"{Naziv} ({GodinaIzdanja})";
        }
    }
}