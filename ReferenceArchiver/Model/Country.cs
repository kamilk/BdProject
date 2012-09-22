using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Country
    {
        public string Code;
        public string Name;
        public string Flag;

        public Country(string code, string name, string flag)
        {
            this.Code = code;
            this.Name = name;
            this.Flag = flag;
        }
    }
}
