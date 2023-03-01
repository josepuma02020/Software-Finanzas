using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public class DomainValidation
    {
        public DomainValidation()
        {
            Fallos = new Dictionary<string, List<string>>();
            Fallos.Clear();
        }

        public Dictionary<string, List<string>> Fallos { get; private set; }
        public bool EsValido => Fallos.Count == 0;

        public DomainValidation AddFallo(string key, string error)
        {
            if (Fallos.ContainsKey(key))
            {
                Fallos[key].Add(error);
                return this;
            }
            Fallos.Add(key, new List<string>()
            {
                error
            });
            return this;
        }
    }
}
