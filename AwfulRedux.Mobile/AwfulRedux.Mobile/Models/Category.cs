using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;

namespace AwfulRedux.Mobile.Models
{
    public class Category : ObservableCollection<Forum>
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public int Order { get; set; }
    }
}
