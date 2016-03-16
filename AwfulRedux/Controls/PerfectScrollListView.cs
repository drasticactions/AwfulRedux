using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AwfulRedux.Controls
{
    public class PerfectScrollListView : ListView
    {

        public PerfectScrollListView()
        {
            this.SizeChanged += PerfectScrollListView_SizeChanged;
        }

        private void PerfectScrollListView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {

            if (ItemsPanelRoot != null)
                ItemsPanelRoot.Width = e.NewSize.Width;
        }
    }
}
