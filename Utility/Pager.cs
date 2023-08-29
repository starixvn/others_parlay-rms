namespace ParlayRMS.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Pager : IEnumerable<int>
    {
        private int _skipPages;
        private int _takePages;
        private int _numberOfItems;
        private int _itemsPerPage;

        private Pager()
        {
        }

        private Pager(Pager pager)
        {
            _numberOfItems = pager._numberOfItems;
            CurrentPageIndex = pager.CurrentPageIndex;
            NumberOfPages = pager.NumberOfPages;
            _takePages = pager._takePages;
            _skipPages = pager._skipPages;
            _itemsPerPage = pager._itemsPerPage;
        }

        public bool IsPaged => _numberOfItems > _itemsPerPage;

        public int NumberOfPages { get; private set; }

        public bool IsUnpaged => NumberOfPages == 1;

        public int CurrentPageIndex { get; private set; }

        public int NextPageIndex => CurrentPageIndex + 1;

        public int LastPageIndex => NumberOfPages;

        public int FirstPageIndex => 1;

        public bool HasNextPage => CurrentPageIndex < NumberOfPages && NumberOfPages > 1;

        public bool HasPreviousPage => CurrentPageIndex > 1 && NumberOfPages > 1;

        public int PreviousPageIndex => CurrentPageIndex - 1;

        public bool IsSegmented => _skipPages > 0 || _skipPages + 1 + _takePages < NumberOfPages;

        public bool IsEmpty => NumberOfPages < 1;

        public bool IsFirstSegment => _skipPages == 0;

        public bool IsLastSegment => _skipPages + 1 + _takePages >= NumberOfPages;

        public static Pager Items(int numberOfItems)
        {
            return new Pager
            {
                _numberOfItems = numberOfItems,
                CurrentPageIndex = 1,
                NumberOfPages = 1,
                _skipPages = 0,
                _takePages = 1,
                _itemsPerPage = numberOfItems
            };
        }

        public Pager PerPage(int itemsPerPage)
        {
            var numberOfPages = (_numberOfItems + itemsPerPage - 1) / itemsPerPage;

            return new Pager(this)
            {
                NumberOfPages = numberOfPages,
                _skipPages = 0,
                _takePages = numberOfPages - CurrentPageIndex + 1,
                _itemsPerPage = itemsPerPage
            };
        }

        public Pager Move(int pageIndex)
        {
            return new Pager(this)
            {
                CurrentPageIndex = pageIndex
            };
        }

        public Pager Segment(int maximum)
        {
            var count = Math.Min(NumberOfPages, maximum);

            return new Pager(this)
            {
                _takePages = count,
                _skipPages = Math.Min(_skipPages, NumberOfPages - count),
            };
        }

        public Pager Center()
        {
            var radius = (_takePages + 1) / 2;

            return new Pager(this)
            {
                _skipPages = Math.Min(Math.Max(CurrentPageIndex - radius, 0), NumberOfPages - _takePages)
            };
        }

        public IEnumerator<int> GetEnumerator()
        {
            return Enumerable.Range(_skipPages + 1, _takePages).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
