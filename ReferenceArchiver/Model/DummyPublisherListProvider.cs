using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class DummyPublisherListProvider : DummyFilterListProviderBase<Publisher>
    {
        public DummyPublisherListProvider()
        {
            SetBaseForFilter(CentralRepository.Instance.GetPublishers());
        }

        public override void AddNew(Publisher newItem)
        {
            throw new NotImplementedException();
        }
    }
}
