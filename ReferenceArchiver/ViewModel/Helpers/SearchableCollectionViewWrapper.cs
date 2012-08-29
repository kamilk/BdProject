using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.ViewModel.Helpers
{
	class SearchableCollectionViewWrapper<T> where T : class
	{
        private Func<T, string> _selector;
        private Predicate<T> _extraCondition;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollectionView CollectionView { get; private set; }

        public string SearchedString { get; set; }

        public SearchableCollectionViewWrapper(ICollectionView collectionView, Func<T, string> selector, Predicate<T> extraCondition = null)
        {
            if (!collectionView.CanFilter)
                throw new ArgumentException("collectionView must be filterable");

            CollectionView = collectionView;
            _selector = selector;
            _extraCondition = extraCondition;

            CollectionView.Filter += Filter;
        }

        private bool Filter(object obj)
        {
            var typedObject = obj as T;
            if (typedObject == null)
                return false;

            bool stringMatch;
            if (SearchedString == null || SearchedString.Length == 0)
                stringMatch = true;
            else
            {
                string testedString = _selector(typedObject);
                if (testedString == null || testedString.Length == 0)
                    return false;
                stringMatch = testedString.StartsWith(SearchedString, StringComparison.CurrentCultureIgnoreCase);
            }

            bool customMatch = _extraCondition == null ? true : _extraCondition(typedObject);

            return stringMatch && customMatch;
        }
    }
}
