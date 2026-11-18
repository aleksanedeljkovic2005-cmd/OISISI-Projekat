using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DAO
{
    public class PosetilacDAO : BaseDAO<Posetilac>
    {
        public PosetilacDAO() : base("posetioci.txt") { }
    }
}
