using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AwfulRedux.UI.Models.Forums
{
    public class Forum
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int CurrentPage { get; set; }

        public bool IsSubforum { get; set; }

        public int TotalPages { get; set; }

        public int ForumId { get; set; }

        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(Category))]
        public int ForumCategoryEntityId { get; set; }

        [ForeignKey(typeof(Forum))]
        public int ParentForumId { get; set; }

        [ManyToOne]
        public Forum ParentForum { get; set; }

        [ManyToOne]
        public Category ForumCategory { get; set; }

        public bool IsBookmarks { get; set; }
    }
}
