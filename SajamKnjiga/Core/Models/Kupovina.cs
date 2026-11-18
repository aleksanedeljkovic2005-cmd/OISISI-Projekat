using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Kupovina
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
    }
}
