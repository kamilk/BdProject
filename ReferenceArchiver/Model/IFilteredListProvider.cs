using System.Collections.Generic;
using System.ComponentModel;

namespace BdGui2.Model
{
    interface IFilteredListProvider<T> : INotifyPropertyChanged
    {
        ICollection<T> Items { get; }

        void Filter(string input);
        void AddNew(T newItem);
    }
}
