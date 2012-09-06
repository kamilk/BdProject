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
        public int InstitutionId { get; set; }      // id_inst
        public int PublisherId { get; set; }        // id_wyd
        public int IdWithinPublisher { get; set; }  // id
        public string Title { get; set; }           // tytul
        public string ISSN { get; set; }            // issn

        public ResearchJournal( int id_inst, int id_wyd, int id, string title, string issn )
        {
            this.IdWithinPublisher = id_inst;
            this.InstitutionId = id_wyd;
            this.PublisherId = id;
            this.Title = title;
            this.ISSN = issn;
        }
    }
}
