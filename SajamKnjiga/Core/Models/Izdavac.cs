using System;
using System.Collections.Generic;
using System.Text;
using Core.Utils;

namespace Core.Models
{
    public class Izdavac : ISerializable
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
        public string ToLine()
        {
            return string.Join("|",
                Id,
                Sifra,
                Naziv,
                SefId,
                string.Join(",", AutoriIds),
                string.Join(",", KnjigeIds));
        }

        public void FromLine(string line)
        {
            string[] delovi = line.Split('|');
            Id = int.Parse(delovi[0]);
            Sifra = delovi[1];
            Naziv = delovi[2];
            SefId = int.Parse(delovi[3]);
            AutoriIds = delovi[4] == "" ? new List<int>() : delovi[4].Split(',').Select(int.Parse).ToList();
            KnjigeIds = delovi[5] == "" ? new List<int>() : delovi[5].Split(',').Select(int.Parse).ToList();
        }
    }
}
