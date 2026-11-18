using System.Text;

namespace Core.Utils
{
    public static class Serializer<T> where T : ISerializable, new()
    {
        private static readonly string DataFolder = "Data";

        private static string Putanja(string imeFajla)
        {
            return DataFolder + Path.DirectorySeparatorChar + imeFajla;
        }

        public static List<T> Ucitaj(string imeFajla)
        {
            List<T> lista = new List<T>();
            string putanja = Putanja(imeFajla);

            if (!File.Exists(putanja))
                return lista;

            foreach (string linija in File.ReadAllLines(putanja, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(linija))
                    continue;

                T objekat = new T();
                objekat.FromLine(linija);
                lista.Add(objekat);
            }

            return lista;
        }

        public static void Sacuvaj(string imeFajla, List<T> lista)
        {
            Directory.CreateDirectory(DataFolder);

            List<string> linije = new List<string>();
            foreach (T objekat in lista)
                linije.Add(objekat.ToLine());

            File.WriteAllLines(Putanja(imeFajla), linije, Encoding.UTF8);
        }
    }
}