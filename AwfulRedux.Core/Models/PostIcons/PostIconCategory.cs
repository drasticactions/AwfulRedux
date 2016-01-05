using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulRedux.Core.Models.PostIcons
{
    public class PostIconCategory
    {
        public PostIconCategory(string category, List<PostIcon> list)
        {
            List = list;
            Category = category;
        }

        public virtual ICollection<PostIcon> List { get; private set; }

        public string Category { get; private set; }
    }
}
