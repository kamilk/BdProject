using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Instytucja
    /// </summary>
    class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Institution()
        {
            this.Id = -1;
        }

        public Institution(string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
