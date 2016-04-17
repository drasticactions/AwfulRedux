using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.UI.Models.Threads;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class SearchViewModel : ForumThreadListBaseViewModel
    {
        private SearchPageScrollingCollection _ForumPageScrollingCollection = default(SearchPageScrollingCollection);

        public SearchPageScrollingCollection ForumPageScrollingCollection
        {
            get { return _ForumPageScrollingCollection; }
            set { Set(ref _ForumPageScrollingCollection, value); }
        }
    }
}
