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
        public int InstitutionId { get; set; }          // id_inst
        public int IdWithinInstitution { get; set; }    // id
        public string Title { get; set; }               // tytul

        public Publisher()
        {
            this.InstitutionId = -1;
            this.IdWithinInstitution = -1;
        }

        public Publisher(int id_inst, int id, string title)
        {
            this.IdWithinInstitution = id_inst;
            this.InstitutionId = id;
            this.Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
