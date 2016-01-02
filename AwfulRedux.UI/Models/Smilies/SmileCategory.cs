using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.UI.Models.Smilies
{
    public class SmileCategory
    {
        public SmileCategory()
        {
            SmileList = new List<Smile>();
        }

        public virtual ICollection<Smile> SmileList { get; set; }

        public string Name { get; set; }
    }

    public class Smile
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }
    }
}
