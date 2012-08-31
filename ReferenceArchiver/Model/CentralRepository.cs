using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    abstract class CentralRepository
    {
        public abstract IEnumerable<Institution> GetInstitutions();
        public abstract IEnumerable<Publisher> GetPublishers();
        public abstract IEnumerable<Publisher> GetPublishersForInstitution(Institution institution);
        public abstract IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher);
        public abstract IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal);
        public abstract Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number);
        public abstract Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number);

        private static CentralRepository _instance;
        public static CentralRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DummyCentralRepository();
                return _instance;
            }
        }
    }
}
