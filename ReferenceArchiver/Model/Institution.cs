using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdGui2.Model
{
    class Institution
    {
        public string Name { get; set; }

        public Institution()
        {
        }

        public Institution(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
