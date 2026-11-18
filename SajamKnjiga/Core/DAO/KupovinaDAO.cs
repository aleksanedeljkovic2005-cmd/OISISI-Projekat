using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DAO
{
    public class KupovinaDAO : BaseDAO<Kupovina>
    {
        public KupovinaDAO() : base("kupovine.txt") { }
    }
}
