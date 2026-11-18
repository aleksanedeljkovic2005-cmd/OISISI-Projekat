using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Posetilac
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
    }
}
