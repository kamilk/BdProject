using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ReferenceArchiver.Model
{
    class DummyInstitutionProvider : DummyFilterListProviderBase<Institution>
    {
        private List<Institution> _allItems = new List<Institution>();

        public override void AddNew(Institution newItem)
        {
            throw new NotImplementedException();
        }

        public DummyInstitutionProvider()
        {
            _allItems.Add(new Institution("Politechnika Śląska"));
            _allItems.Add(new Institution("Politechnika Wrocławska"));
            _allItems.Add(new Institution("Politechnika Warszawska"));
            _allItems.Add(new Institution("Uniwersytet Warszawski"));
            _allItems.Add(new Institution("Akademia Górniczo-Hutnicza"));

            SetBaseForFilter(_allItems);
        }
    }
}
