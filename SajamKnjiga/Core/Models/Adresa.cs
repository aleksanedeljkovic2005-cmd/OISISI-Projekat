using Core.Utils;

namespace Core.Models
{
    public class Adresa : ISerializable
    {
        public int Id { get; set; }
        public string Ulica { get; set; } = "";
        public string Broj { get; set; } = "";
        public string Grad { get; set; } = "";
        public string Drzava { get; set; } = "";

        public Adresa() { }

        public Adresa(int id, string ulica, string broj, string grad, string drzava)
        {
            Id = id;
            Ulica = ulica;
            Broj = broj;
            Grad = grad;
            Drzava = drzava;
        }

        public string ToLine()
        {
            return string.Join("|", Id, Ulica, Broj, Grad, Drzava);
        }

        public void FromLine(string line)
        {
            string[] d = line.Split('|');
            Id = int.Parse(d[0]);
            Ulica = d[1];
            Broj = d[2];
            Grad = d[3];
            Drzava = d[4];
        }

        public override string ToString()
        {
            return $"{Ulica} {Broj}, {Grad}, {Drzava}";
        }
    }
}