﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace AwfulRedux.UI.Models.Users
{
    public class User
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Username { get; set; }

        public string AvatarLink { get; set; }

        public string AvatarTitle { get; set; }

        public string AvatarHtml { get; set; }

        public DateTime DateJoined { get; set; }

        public string DateJoinedShort => DateJoined.ToString("MM/dd/yyyy");

        public string ProfileLink { get; set; }

        public string PrivateMessageLink { get; set; }

        public string PostHistoryLink { get; set; }

        public string RapSheetLink { get; set; }

        public bool CanSendPrivateMessage { get; set; }

        public string IcqContactString { get; set; }

        public string AimContactString { get; set; }

        public string YahooContactString { get; set; }

        public string HomePageString { get; set; }

        public int PostCount { get; set; }

        public DateTime LastPostDate { get; set; }

        public string LastPostDateShort => LastPostDate.ToString("MM/dd/yyyy");

        public string Location { get; set; }

        public string AboutUser { get; set; }

        public bool IsMod { get; set; }

        public bool IsAdmin { get; set; }

        public string Roles { get; set; }

        public bool IsCurrentUserPost { get; set; }

        public string PostRate { get; set; }

        public string SellerRating { get; set; }
    }
}
