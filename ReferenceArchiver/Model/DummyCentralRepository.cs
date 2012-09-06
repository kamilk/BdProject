using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ReferenceArchiver.Model
{
    class DummyCentralRepository : CentralRepository
    {
        private List<Institution> _institutions;
        private List<Publisher> _publishers;
        private List<ResearchJournal> _journals;
        private List<Issue> _issues;

        public DummyCentralRepository()
        {
            _institutions = new List<Institution>();
            _institutions.Add(new Institution(1, "Politechnika Śląska"));
            _institutions.Add(new Institution(2, "Politechnika Wrocławska"));
            _institutions.Add(new Institution(3, "Politechnika Warszawska"));
            _institutions.Add(new Institution(4, "Uniwersytet Warszawski"));
            _institutions.Add(new Institution(5, "Akademia Górniczo-Hutnicza"));

            _publishers = new List<Publisher>();
            _publishers.Add(new Publisher(1, 1, "Wydawnictwo Politechniki Śląskiej"));
            _publishers.Add(new Publisher(2, 1, "Wydawnictwo Politechniki Wrocławskiej"));
            _publishers.Add(new Publisher(3, 1, "Wydawnictwo Politechniki Warszawskiej"));
            _publishers.Add(new Publisher(4, 1, "Wydawnictwo Uniwersytetu Warszawskiego"));
            _publishers.Add(new Publisher(5, 1, "Wydawnictwo AGH"));

            _journals = new List<ResearchJournal>();
            int issn = 2000;
            foreach (var publisher in _publishers)
            {
                int idWithingPublisher = 1;
                foreach (var title in new string[] { "Informatyka", "Fizyka molekularna", "Ekologia" })
                {
                    _journals.Add(new ResearchJournal()
                    {
                        InstitutionId = publisher.InstitutionId,
                        PublisherId = publisher.IdWithinInstitution,
                        IdWithinPublisher = idWithingPublisher++,
                        ISSN = string.Format("2000-{0}", issn++),
                        Title = string.Format("{0} - {1}", publisher.Title, title)
                    });
                }
            }

            _issues = new List<Issue>();
            foreach (var journal in _journals)
            {
                for (int i = 0; i < 10; i++)
                {
                    _issues.Add(new Issue(journal)
                    {
                        IdWithinJournal = i,
                        NumberWithinJournal = i,
                        NumberWithinPublisher = _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                            && issue.PublisherId == journal.PublisherId).Count() + 1,
                        Title = string.Format("Numer {0}", i),
                        YearOfPublication = 2000 + i,
                    });
                }
            }
        }

        public override IEnumerable<Institution> GetInstitutions()
        {
            return _institutions;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            return _publishers;
        }

        public override IEnumerable<Publisher> GetPublishersForInstitution(Institution institution)
        {
            if (institution.Id < 0)
                yield break;

            foreach (var publisher in _publishers)
            {
                if (publisher.InstitutionId == institution.Id)
                    yield return publisher;
            }
        }

        public override IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher)
        {
            if (publisher == null || publisher.IdWithinInstitution < 0)
                return Enumerable.Empty<ResearchJournal>();

            return _journals.Where(journal => journal.PublisherId == publisher.IdWithinInstitution
                && journal.InstitutionId == publisher.InstitutionId);
        }

        public override IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal)
        {
            if (journal == null || journal.IdWithinPublisher < 0)
                return Enumerable.Empty<Issue>();

            return _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                && issue.PublisherId == journal.PublisherId && issue.JournalId == journal.IdWithinPublisher);
        }

        public override Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number)
        {
            return _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                && issue.PublisherId == journal.PublisherId 
                && issue.JournalId == journal.IdWithinPublisher 
                && issue.NumberWithinJournal == number).FirstOrDefault();
        }

        public override Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number)
        {
            return _issues.Where(issue => issue.InstitutionId == publisher.InstitutionId
                && issue.PublisherId == publisher.IdWithinInstitution
                && issue.NumberWithinPublisher == number).FirstOrDefault();
        }
    }
}
