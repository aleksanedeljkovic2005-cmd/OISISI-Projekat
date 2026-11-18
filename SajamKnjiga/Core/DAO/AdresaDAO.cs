
using Core.Models;

namespace Core.DAO
{
    public class AdresaDAO : BaseDAO<Adresa>
    {
        public AdresaDAO() : base("adrese.txt") { }
    }
}