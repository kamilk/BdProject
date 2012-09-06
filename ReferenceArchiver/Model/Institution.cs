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
        public string Id { get; set; }      // id
        public string Name { get; set; }    // nazwa

        public Institution()
        {

        }

        public Institution(string id, string name)
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
