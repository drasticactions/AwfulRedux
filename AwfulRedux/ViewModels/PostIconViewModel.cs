using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.PostIcons;
using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class PostIconViewModel : ViewModelBase
    {
        private ObservableCollection<PostIcon> _postIconEntities = new ObservableCollection<PostIcon>();

        public ObservableCollection<PostIcon> PostIconEntities
        {
            get { return _postIconEntities; }
            set
            {
                Set(ref _postIconEntities, value);
            }
        }

        private readonly PostIconManager _postIconManager = new PostIconManager(Views.Shell.Instance.ViewModel.WebManager);

        private PostIcon _postIcon = default(PostIcon);

        public PostIcon PostIcon
        {
            get { return _postIcon; }
            set
            {
                Set(ref _postIcon, value);
            }
        }

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        public void SelectIcon(object sender, ItemClickEventArgs e)
        {
            var smile = e.ClickedItem as PostIcon;
            if (smile == null) return;
            PostIcon = smile;
            IsOpen = false;
        }

        public async Task Initialize(int forumId)
        {
            if ((PostIconEntities == null || !PostIconEntities.Any()))
            {
                var test = await _postIconManager.GetPostIcons(forumId);
                PostIconEntities = test.First().List.ToObservableCollection();
            }
            else if (forumId == 0)
            {
                var test = await _postIconManager.GetPmPostIcons();
                PostIconEntities = test.First().List.ToObservableCollection();
            }
        }
    }
}
