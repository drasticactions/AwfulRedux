﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.UI.Models.Smilies;
using Newtonsoft.Json;
using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class SmiliesViewModel : ViewModelBase
    {
        private ObservableCollection<SmileCategory> _smileCategoryList = new ObservableCollection<SmileCategory>();

        public ObservableCollection<SmileCategory> SmileCategoryList
        {
            get { return _smileCategoryList; }
            set
            {
                Set(ref _smileCategoryList, value);
            }
        }

        public TextBox ReplyBox { get; set; }

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private WebManager _webManager;
        private SmileManager _smileManager;
        public ObservableCollection<SmileCategory> FullSmileCategoryEntities { get; set; }

        public async Task LoginUser()
        {
            var cookie = await LoginHelper.LoginDefaultUser();
            _webManager = new WebManager(cookie);
            _smileManager = new SmileManager(_webManager);
        }

        public async Task LoadSmilies()
        {
            if (_smileManager == null)
            {
                await LoginUser();
            }
            if (!SmileCategoryList.Any())
            {
                IsLoading = true;
                var result = await _smileManager.GetSmileList();
                var list = JsonConvert.DeserializeObject<List<SmileCategory>>(result.ResultJson);
                FullSmileCategoryEntities = list.ToObservableCollection();
                foreach (var item in list)
                {
                    SmileCategoryList.Add(item);
                }
                IsLoading = false;
            }
        }

        public void SelectIcon(object sender, ItemClickEventArgs e)
        {
            var smile = e.ClickedItem as Smile;
            if (smile == null) return;
            ReplyBox.Text = ReplyBox.Text.Insert(ReplyBox.Text.Length, smile.Title);
            IsOpen = false;
        }

        public void SmiliesFilterOnSuggestedQuery(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var suggestionCollection = args.Request.SearchSuggestionCollection;
            foreach (var smile in SmileCategoryList.SelectMany(smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText))))
            {
                suggestionCollection.AppendQuerySuggestion(smile.Title);
            }
        }

        public void SmiliesFilterOnSubmittedQuery(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var result = SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result == null)
            {
                return;
            }
            ReplyBox.Text = ReplyBox.Text.Insert(ReplyBox.Text.Length, result.Title);
            IsOpen = false;
        }

        public void SmiliesFilterOnChangedQuery(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText))
            {
                SmileCategoryList = FullSmileCategoryEntities;
                return;
            }
            var result = SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result != null) return;
            var searchList = FullSmileCategoryEntities.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText)));
            List<Smile> smileListEntities = searchList.ToList();
            var searchSmileCategory = new SmileCategory()
            {
                Name = "Search",
                SmileList = smileListEntities
            };
            var fakeSmileCategoryList = new List<SmileCategory> { searchSmileCategory };
            SmileCategoryList = fakeSmileCategoryList.ToObservableCollection();
        }
    }
}
