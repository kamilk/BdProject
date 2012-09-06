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
        public int Id { get; set; }         // id
        public string Name { get; set; }    // nazwa

        public Institution()
        {
            this.Id = -1;
        }

        public Institution(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
