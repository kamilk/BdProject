using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Publisher
    {
        public int IdWithinInstitution { get; set; }
        public int InstitutionId { get; set; }
        public string Title { get; set; }

        public Publisher()
        {
            this.InstitutionId = -1;
            this.IdWithinInstitution = -1;
        }

        public Publisher(string title, int institutionId, int id)
        {
            this.Title = title;
            this.InstitutionId = institutionId;
            this.IdWithinInstitution = id;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
