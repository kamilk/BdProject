using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Zeszyt
    /// </summary>
    class Issue
    {
        public int InstitutionId;
        public int PublisherId;
        public int JournalId;
        public int IdWithinJournal;

        public int NumberWithinJournal;
        public int NumberWithinPublisher;

        public string Title;
        public int YearOfPublishing;
        public bool WasVerified;
        public string Type;
        public string TypeNumber;

        public Issue(ResearchJournal parentJournal)
        {
            InstitutionId = parentJournal.InstitutionId;
            PublisherId = parentJournal.PublisherId;
            JournalId = parentJournal.IdWithinPublisher;
        }
    }
}
