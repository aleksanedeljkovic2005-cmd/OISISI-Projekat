using Core.Models;

namespace Core.DAO
{
    public class KnjigaDAO : BaseDAO<Knjiga>
    {
        public KnjigaDAO() : base("knjige.txt") { }
    }
}