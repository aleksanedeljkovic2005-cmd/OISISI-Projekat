using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Izdavac
    {
        public int Id { get; set; }
        public string Sifra { get; set; } = "";
        public string Naziv { get; set; } = "";
        public int SefId { get; set; }
        public List<int> AutoriIds { get; set; } = new List<int>();
        public List<int> KnjigeIds { get; set; } = new List<int>();

        public Izdavac() { }

        public Izdavac(int id, string sifra, string naziv, int sefId)
        {
            Id = id;
            Sifra = sifra;
            Naziv = naziv;
            SefId = sefId;
        }
    }
}
