using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DAO
{
    public class IzdavacDAO : BaseDAO<Izdavac>
    {
        public IzdavacDAO() : base("izdavaci.txt") { }
    }
}
