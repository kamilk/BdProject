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
            _institutions.Add(new Institution("Politechnika Śląska", 1));
            _institutions.Add(new Institution("Politechnika Wrocławska", 2));
            _institutions.Add(new Institution("Politechnika Warszawska", 3));
            _institutions.Add(new Institution("Uniwersytet Warszawski", 4));
            _institutions.Add(new Institution("Akademia Górniczo-Hutnicza", 5));

            _publishers = new List<Publisher>();
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Śląskiej", 1, 1));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Wrocławskiej", 2, 1));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Warszawskiej", 3, 1));
            _publishers.Add(new Publisher("Wydawnictwo Uniwersytetu Warszawskiego", 4, 1));
            _publishers.Add(new Publisher("Wydawnictwo AGH", 5, 1));

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
