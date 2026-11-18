using Core.Models;

namespace Core.DAO
{
    public class AutorDAO : BaseDAO<Autor>
    {
        public AutorDAO() : base("autori.txt") { }
    }
}