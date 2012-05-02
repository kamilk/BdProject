using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BdGui2.Model
{
    class DummyInstitutionProvider : IFilteredListProvider<Institution>
    {
        private List<Institution> _allItems = new List<Institution>();
        
        private ICollection<Institution> _items;
        public ICollection<Institution> Items
        {
            get 
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Filter(string input)
        {
            var newSelection = new List<Institution>();
            foreach (var institution in _allItems)
            {
                if (institution.ToString().StartsWith(input, StringComparison.CurrentCultureIgnoreCase))
                    newSelection.Add(institution);
            }
            Items = newSelection;
        }

        public void AddNew(Institution newItem)
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

            _items = new List<Institution>(_allItems);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
