using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Wydawnictwo (w rozumieniu firmy)
    /// </summary>
    class Publisher
    {
        public string InstitutionId { get; set; }          // id_inst
        public string IdWithinInstitution { get; set; }    // id
        public string Title { get; set; }                   // tytul

        public Publisher()
        {

        }

        public Publisher(string id_inst, string id, string title)
        {
            this.InstitutionId = id_inst;
            this.IdWithinInstitution = id;
            this.Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
