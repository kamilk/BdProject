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
        public int IdWithinPublisher { get; set; }
        public int PublisherId { get; set; }
        public int InstitutionId { get; set; }
        public string Title { get; set; }
        public string ISSN { get; set; }
    }
}
