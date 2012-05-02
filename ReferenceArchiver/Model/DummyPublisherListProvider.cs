using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class DummyPublisherListProvider : DummyFilterListProviderBase<Publisher>
    {
        IList<Publisher> _publishers;

        public DummyPublisherListProvider()
        {
            _publishers = new List<Publisher>();
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Śląskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Wrocławskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Politechniki Warszawskiej"));
            _publishers.Add(new Publisher("Wydawnictwo Uniwersytetu Warszawskiego"));
            _publishers.Add(new Publisher("Wydawnictwo AGH"));
            SetBaseForFilter(_publishers);
        }

        public override void AddNew(Publisher newItem)
        {
            throw new NotImplementedException();
        }
    }
}
