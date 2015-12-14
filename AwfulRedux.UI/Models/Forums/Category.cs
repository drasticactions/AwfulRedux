using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using static SQLiteNetExtensions.Attributes.CascadeOperation;

namespace AwfulRedux.UI.Models.Forums
{
    public class Category
    {
        public Category()
        {
            ForumList = new List<Forum>();
        }

        public string Name { get; set; }

        public string Location { get; set; }

        [PrimaryKey]
        public int Id { get; set; }

        public int Order { get; set; }

        /// <summary>
        ///     The forums that belong to that category (Ex. GBS, FYAD)
        /// </summary>
        [OneToMany(CascadeOperations = All)]

        public List<Forum> ForumList { get; set; }
    }
}
