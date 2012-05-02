using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ReferenceArchiver.Model
{
    class DummyInstitutionListProvider : DummyFilterListProviderBase<Institution>
    {
        private List<Institution> _allItems = new List<Institution>();

        public override void AddNew(Institution newItem)
        {
            throw new NotImplementedException();
        }

        public DummyInstitutionListProvider()
        {
            SetBaseForFilter(CentralRepository.Instance.GetInstitutions());
        }
    }
}
