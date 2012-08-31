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
        public int? YearOfPublication;
        public bool WasVerified;
        public IssueType Type = IssueType.Symposium;
        public string TypeNumber;

        public Issue()
        {
            InstitutionId = -1;
            PublisherId = -1;
            JournalId = -1;
            IdWithinJournal = -1;
        }

        public Issue(ResearchJournal parentJournal)
        {
            InstitutionId = parentJournal.InstitutionId;
            PublisherId = parentJournal.PublisherId;
            JournalId = parentJournal.IdWithinPublisher;
            IdWithinJournal = -1;
        }
    }
}
