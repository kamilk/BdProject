using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class DummyCentralRepository : CentralRepository
    {
        private List<Institution> _institutions;
        IList<Publisher> _publishers;

        public DummyCentralRepository()
        {
            _institutions = new List<Institution>();
            _institutions.Add(new Institution("Politechnika Śląska"));
            _institutions.Add(new Institution("Politechnika Wrocławska"));
            _institutions.Add(new Institution("Politechnika Warszawska"));
            _institutions.Add(new Institution("Uniwersytet Warszawski"));
            _institutions.Add(new Institution("Akademia Górniczo-Hutnicza"));

            _publishers = new List<Publisher>();
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Śląskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Wrocławskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Warszawskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Uniwersytetu Warszawskiego"));
            _publishers.Add(new Publisher("Wydawnictwo AGH"));
        }

        public override IEnumerable<Institution> GetInstitutions()
        {
            return _institutions;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            return _publishers;
        }

        public override IEnumerable<Publisher> GetPublishersAssignedToInstitution()
        {
            throw new NotImplementedException();
        }
    }
}
