using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Seria
    /// </summary>
    class ResearchJournal
    {
        public string InstitutionId { get; set; }      // id_inst
        public string PublisherId { get; set; }        // id_wyd
        public string IdWithinPublisher { get; set; }  // id
        public string Title { get; set; }              // tytul
        public string ISSN { get; set; }               // issn

        public ResearchJournal()
        { }

        public ResearchJournal(string id_inst, string id_wyd, string id, string title, string issn)
        {
            this.IdWithinPublisher = id_inst;
            this.InstitutionId = id_wyd;
            this.PublisherId = id;
            this.Title = title;
            this.ISSN = issn;
        }
    }
}
