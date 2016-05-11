// Taken from https://github.com/mattwhetton/Codenutz.XF.InfiniteListView/blob/master/Codenutz.XF.Controls.InfiniteListView/Codenutz.XF.InfiniteListView.cs

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Controls
{
    /// <summary>
    /// A simple listview that exposes a bindable command to allow infinite loading behaviour.
    /// </summary>
    public class InfiniteListView : ListView
    {
        /// <summary>
        /// Respresents the command that is fired to ask the view model to load additional data bound collection.
        /// </summary>
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteListView, Command>(bp => bp.LoadMoreCommand, default(Command));

        /// <summary>
        /// Gets or sets the command binding that is called whenever the listview is getting near the bottomn of the list, and therefore requiress more data to be loaded.
        /// </summary>
        public Command LoadMoreCommand
        {
            get { return (Command)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="InfiniteListView" />
        /// </summary>
        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }


        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }

    }
}
