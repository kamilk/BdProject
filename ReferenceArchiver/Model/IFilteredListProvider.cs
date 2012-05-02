using System.Collections.Generic;
using System.ComponentModel;

namespace ReferenceArchiver.Model
{
    interface IFilteredListProvider<T> : INotifyPropertyChanged
    {
        IEnumerable<T> Items { get; }
        T SelectedItem { get; set; }

        void Filter(string input);
        void AddNew(T newItem);
    }
}
