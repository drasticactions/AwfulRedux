﻿using System;
using AwfulRedux.UI.Models.Posts;

namespace AwfulWebTemplate
{
	public class PrivateMessageTemplateModel
	{
		public Post PMPost { get; set; }

		public bool IsLoggedIn { get; set; }

		public bool IsDarkThemeSet { get; set; }
	}
}

