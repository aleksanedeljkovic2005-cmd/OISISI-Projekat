using System;
using System.Collections.Generic;
using System.Text;
using Core.Utils;
using System.Globalization;

namespace Core.Models
{
    public class Kupovina : ISerializable
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; } = "";

        public Kupovina() { }

        public Kupovina(int id, int posetilacId, int knjigaId, DateTime datumKupovine, int ocena, string komentar)
        {
            Id = id;
            PosetilacId = posetilacId;
            KnjigaId = knjigaId;
            DatumKupovine = datumKupovine;
            Ocena = ocena;
            Komentar = komentar;
        }
        public string ToLine()
        {
            return string.Join("|",
                Id,
                PosetilacId,
                KnjigaId,
                DatumKupovine.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Ocena,
                Komentar);
        }

        public void FromLine(string line)
        {
            string[] delovi = line.Split('|');
            Id = int.Parse(delovi[0]);
            PosetilacId = int.Parse(delovi[1]);
            KnjigaId = int.Parse(delovi[2]);
            DatumKupovine = DateTime.ParseExact(delovi[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Ocena = int.Parse(delovi[4]);
            Komentar = delovi[5];
        }
    }
}
