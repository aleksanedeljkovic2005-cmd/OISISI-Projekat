using System;
using System.Collections.Generic;
using System.Text;
using Core.Utils;
using System.Globalization;

namespace Core.Models
{
    public class Posetilac :ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public DateTime DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public string Telefon { get; set; } = "";
        public string Email { get; set; } = "";
        public string BrojClanskeKarte { get; set; } = "";
        public int GodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
        public double ProsecnaOcena { get; set; }
        public List<int> KupovineIds { get; set; } = new List<int>();
        public List<int> ListaZeljaIds { get; set; } = new List<int>();

        public Posetilac() { }

        public Posetilac(int id, string ime, string prezime, DateTime datumRodjenja, int adresaId,
                         string telefon, string email, string brojClanskeKarte,
                         int godinaClanstva, StatusPosetioca status)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            AdresaId = adresaId;
            Telefon = telefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            GodinaClanstva = godinaClanstva;
            Status = status;
        }
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
                BrojClanskeKarte,
                GodinaClanstva,
                Status.ToString(),
                ProsecnaOcena.ToString(CultureInfo.InvariantCulture),
                string.Join(",", KupovineIds),
                string.Join(",", ListaZeljaIds));
        }

        public void FromLine(string line)
        {
            string[] delovi = line.Split('|');
            Id = int.Parse(delovi[0]);
            Ime = delovi[1];
            Prezime = delovi[2];
            DatumRodjenja = DateTime.ParseExact(delovi[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            AdresaId = int.Parse(delovi[4]);
            Telefon = delovi[5];
            Email = delovi[6];
            BrojClanskeKarte = delovi[7];
            GodinaClanstva = int.Parse(delovi[8]);
            Status = Enum.Parse<StatusPosetioca>(delovi[9]);
            ProsecnaOcena = double.Parse(delovi[10], CultureInfo.InvariantCulture);
            KupovineIds = delovi[11] == "" ? new List<int>() : delovi[11].Split(',').Select(int.Parse).ToList();
            ListaZeljaIds = delovi[12] == "" ? new List<int>() : delovi[12].Split(',').Select(int.Parse).ToList();
        }
    }
}
