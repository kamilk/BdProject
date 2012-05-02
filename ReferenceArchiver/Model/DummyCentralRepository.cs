using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class DummyCentralRepository : CentralRepository
    {
        private List<Institution> _institutions;
        private List<Publisher> _publishers;

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
        }

        public override IEnumerable<Institution> GetInstitutions()
        {
            return _institutions;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            return _publishers;
        }

        public override IEnumerable<Publisher> GetPublishersAssignedToInstitution(Institution institution)
        {
            if (institution.Id < 0)
                yield break;

            foreach (var publisher in _publishers)
            {
                if (publisher.InstitutionId == institution.Id)
                    yield return publisher;
            }
        }
    }
}
