using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.Model
{
    abstract class DummyFilterListProviderBase<T> : IFilteredListProvider<T>
    {
        private List<T> _allItems = new List<T>();
        private string _filterCriterion;

        private ICollection<T> _items = new List<T>();
        public ICollection<T> Items
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

        public void Filter(string input)
        {
            _filterCriterion = input;
            if (input == null || input.Length == 0)
                Items = _allItems;
            else
            {
                var newSelection = new List<T>();
                foreach (var item in _allItems)
                {
                    if (item.ToString().StartsWith(input, StringComparison.CurrentCultureIgnoreCase))
                        newSelection.Add(item);
                }
                Items = newSelection;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void AddNew(T newItem);

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetBaseForFilter(ICollection<T> allItems)
        {
            _allItems = new List<T>(allItems);
            Filter(_filterCriterion);
        }
    }
}
