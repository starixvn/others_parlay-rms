// --------------------------------------------------------------------------
// <copyright file="Pager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Pager helper.
    /// </summary>
    public sealed class Pager : IEnumerable<int>
    {
        private int _numberOfPages;
        private int _skipPages;
        private int _takePages;
        private int _currentPageIndex;
        private int _numberOfItems;
        private int _itemsPerPage;

        private Pager()
        {
        }

        private Pager(Pager pager)
        {
            _numberOfItems = pager._numberOfItems;
            _currentPageIndex = pager._currentPageIndex;
            _numberOfPages = pager._numberOfPages;
            _takePages = pager._takePages;
            _skipPages = pager._skipPages;
            _itemsPerPage = pager._itemsPerPage;
        }

        public bool IsPaged
        {
            get { return _numberOfItems > _itemsPerPage; }
        }

        public int NumberOfPages
        {
            get { return _numberOfPages; }
        }

        public bool IsUnpaged
        {
            get { return _numberOfPages == 1; }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
        }

        public int NextPageIndex
        {
            get { return _currentPageIndex + 1; }
        }

        public int LastPageIndex
        {
            get { return _numberOfPages; }
        }

        public int FirstPageIndex
        {
            get { return 1; }
        }

        public bool HasNextPage
        {
            get { return _currentPageIndex < _numberOfPages && _numberOfPages > 1; }
        }

        public bool HasPreviousPage
        {
            get { return _currentPageIndex > 1 && _numberOfPages > 1; }
        }

        public int PreviousPageIndex
        {
            get { return _currentPageIndex - 1; }
        }

        public bool IsSegmented
        {
            get { return _skipPages > 0 || _skipPages + 1 + _takePages < _numberOfPages; }
        }

        public bool IsEmpty
        {
            get { return _numberOfPages < 1; }
        }

        public bool IsFirstSegment
        {
            get { return _skipPages == 0; }
        }

        public bool IsLastSegment
        {
            get { return _skipPages + 1 + _takePages >= _numberOfPages; }
        }

        /// <summary>
        /// Creates a pager for the given number of items.
        /// </summary>
        /// <param name="numberOfItems">Number of items per page.</param>
        /// <returns>The pager object.</returns>
        public static Pager Items(int numberOfItems)
        {
            return new Pager
            {
                _numberOfItems = numberOfItems,
                _currentPageIndex = 1,
                _numberOfPages = 1,
                _skipPages = 0,
                _takePages = 1,
                _itemsPerPage = numberOfItems
            };
        }

        /// <summary>
        /// Specifies the number of items per page.
        /// </summary>
        /// <param name="itemsPerPage">Number of items per page.</param>
        /// <returns>The pager object.</returns>
        public Pager PerPage(int itemsPerPage)
        {
            int numberOfPages = (_numberOfItems + itemsPerPage - 1) / itemsPerPage;

            return new Pager(this)
            {
                _numberOfPages = numberOfPages,
                _skipPages = 0,
                _takePages = numberOfPages - _currentPageIndex + 1,
                _itemsPerPage = itemsPerPage
            };
        }

        /// <summary>
        /// Moves the pager to the given page index.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <returns>The pager object.</returns>
        public Pager Move(int pageIndex)
        {
            return new Pager(this)
            {
                _currentPageIndex = pageIndex
            };
        }

        /// <summary>
        /// Segments the pager so that it will display a maximum number of pages.
        /// </summary>
        /// <param name="maximum">Maximum number of pages.</param>
        /// <returns>The pager object.</returns>
        public Pager Segment(int maximum)
        {
            int count = Math.Min(_numberOfPages, maximum);

            return new Pager(this)
            {
                _takePages = count,
                _skipPages = Math.Min(_skipPages, _numberOfPages - count),
            };
        }

        /// <summary>
        /// Centers the segment around the current page.
        /// </summary>
        /// <returns>Pager object.</returns>
        public Pager Center()
        {
            int radius = (_takePages + 1) / 2;

            return new Pager(this)
            {
                _skipPages = Math.Min(Math.Max(_currentPageIndex - radius, 0), _numberOfPages - _takePages)
            };
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return Enumerable.Range(_skipPages + 1, _takePages).GetEnumerator();
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Enumerator object.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}