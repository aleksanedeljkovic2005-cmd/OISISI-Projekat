using Core.Utils;

namespace Core.DAO
{
    public abstract class BaseDAO<T> where T : ISerializable, new()
    {
        private readonly string imeFajla;
        protected List<T> lista;

        protected BaseDAO(string imeFajla)
        {
            this.imeFajla = imeFajla;
            lista = Serializer<T>.Ucitaj(imeFajla);
        }

        public List<T> UcitajSve()
        {
            return new List<T>(lista);
        }

        public void SacuvajSve()
        {
            Serializer<T>.Sacuvaj(imeFajla, lista);
        }

        public int SledeciId()
        {
            return lista.Count == 0 ? 1 : lista.Max(x => x.Id) + 1;
        }

        public T? NadjiPoId(int id)
        {
            return lista.FirstOrDefault(x => x.Id == id);
        }

        public void Dodaj(T objekat)
        {
            objekat.Id = SledeciId();
            lista.Add(objekat);
            SacuvajSve();
        }

        public bool Izmeni(T objekat)
        {
            int index = lista.FindIndex(x => x.Id == objekat.Id);
            if (index == -1)
                return false;

            lista[index] = objekat;
            SacuvajSve();
            return true;
        }

        public bool Obrisi(int id)
        {
            T? objekat = NadjiPoId(id);
            if (objekat == null)
                return false;

            lista.Remove(objekat);
            SacuvajSve();
            return true;
        }
    }
}