using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        protected Page page;
        public ViewModelBase(Page page)
        {
            this.page = page;
        }
    }
}
