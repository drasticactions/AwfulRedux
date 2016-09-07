﻿using System.Collections.Generic;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Polls;
using AwfulRedux.UI.Models.Posts;
using PropertyChanged;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AwfulRedux.UI.Models.Threads
{
    [ImplementPropertyChanged]
    public class Thread
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string ImageIconUrl { get; set; } = "";

        public string ImageIconLocation { get; set; }

        public string StoreImageIconUrl { get; set; }

        public string StoreImageIconLocation { get; set; }

        public string Author { get; set; }

        public int ReplyCount { get; set; }

        public int ViewCount { get; set; }

        public int Rating { get; set; }

        public string RatingImage { get; set; }

        public string RatingImageUrl { get; set; }

        public string KilledBy { get; set; }

        public bool IsSticky { get; set; }

        public bool IsNotified { get; set; }

        public bool IsLocked { get; set; }

        public bool IsAnnouncement { get; set; }

        public bool HasBeenViewed { get; set; }

        public bool CanMarkAsUnread { get; set; }

        public int RepliesSinceLastOpened { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int ScrollToPost { get; set; }

        public string ScrollToPostString { get; set; }

        public string LoggedInUserName { get; set; }

        [PrimaryKey]
        public int Id { get; set; }

        public long ThreadId { get; set; }

        [ForeignKey(typeof(Forum))]
        public int ForumId { get; set; }

        [ManyToOne]
        public virtual Forum ForumEntity { get; set; }

        public bool HasSeen { get; set; }

        public bool IsBookmark { get; set; }

        public string Html { get; set; }

        [Ignore]
        public PollGroup Poll { get; set; }

        [Ignore]
        public List<Post> Posts { get; set; }

        public bool IsPrivateMessage { get; set; }

        public Thread Clone()
        {
            return MemberwiseClone() as Thread;
        }
    }

}
