using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }

        public Country(string code, string name, string flag)
        {
            this.Code = code;
            this.Name = name;
            this.Flag = flag;
        }
    }
}
