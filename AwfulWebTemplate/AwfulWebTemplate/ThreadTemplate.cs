#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AwfulWebTemplate
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "4.1.1.3")]
public partial class ThreadTemplate : ThreadTemplateBase
{

#line hidden

#line 1 "ThreadTemplate.cshtml"
public ThreadTemplateModel Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");


#line 4 "ThreadTemplate.cshtml"
  
int seenCount = 1;
var theme = "";
var allPosts = Model.Posts.Any(node => !node.HasSeen) && Model.Posts.Any(node => node.HasSeen);;
var otherPosts = Model.Posts.Any(node => !node.HasSeen);
switch (Model.ForumThread.ForumId)
            {
                case 29:
                    theme = "<link href=\"ms-appx-web:///Assets/Website/CSS/29.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 26:
                    theme = "<link href=\"ms-appx-web:///Assets/Website/CSS/26.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 267:
                    theme = "<link href=\"ms-appx-web:///Assets/Website/CSS/267.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 268:
                    theme = "<link href=\"ms-appx-web:///Assets/Website/CSS/268.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }


#line default
#line hidden
WriteLiteral("\r\n<head>\r\n    <meta");

WriteLiteral(" name=\"twitter:dnt\"");

WriteLiteral(" content=\"on\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1, user-scalable=no\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" href=\"ms-appx-web:///Assets/Website/CSS/winstrap.min.css\"");

WriteLiteral(" media=\"all\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" href=\"ms-appx-web:///Assets/Website/CSS/forum-thread.css\"");

WriteLiteral(" media=\"all\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" href=\"ms-appx-web:///Assets/Website/CSS/bbcode.css\"");

WriteLiteral(" media=\"all\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n");


#line 31 "ThreadTemplate.cshtml"
    

#line default
#line hidden

#line 31 "ThreadTemplate.cshtml"
     if (Model.IsDarkThemeSet) {


#line default
#line hidden
WriteLiteral("    \t<meta");

WriteLiteral(" name=\"twitter:widgets:theme\"");

WriteLiteral(" content=\"dark\"");

WriteLiteral(">\r\n");

WriteLiteral("    \t<link");

WriteLiteral(" href=\"ms-appx-web:///Assets/Website/CSS/dark.css\"");

WriteLiteral(" media=\"all\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\r\n");


#line 34 "ThreadTemplate.cshtml"
    }


#line default
#line hidden
WriteLiteral("   \t");


#line 35 "ThreadTemplate.cshtml"
      
   	WriteLiteral(theme);
   	

#line default
#line hidden
WriteLiteral("\r\n    <!--<script type=\"text/javascript\" async=\"\" src=\"JS/jquery.min.js\"></script" +
">-->\r\n    <script");

WriteLiteral(" src=\"https://code.jquery.com/jquery-2.2.3.js\"");

WriteLiteral("   integrity=\"sha256-laXWtGydpwqJ8JA+X9x2miwmaiKhn8tVmOVEigRNtP4=\"");

WriteLiteral("   crossorigin=\"anonymous\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/gifffer.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/bootstrap.min.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/widgets.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/threadTemplate.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/url.min.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" async=\"\"");

WriteLiteral(" src=\"ms-appx-web:///Assets/directionalnavigation-1.0.0.0.js\"");

WriteLiteral(@"></script>
    <script>
        var newFocusRoot = document.getElementById(""thread"");
        TVJS.DirectionalNavigation.focusRoot = newFocusRoot;
        TVJS.DirectionalNavigation.focusableSelectors.push("".btn"");
        TVJS.DirectionalNavigation.focusableSelectors.push("".article-content"");
        TVJS.DirectionalNavigation.keyCodeMap.up.push(87); // w  
        TVJS.DirectionalNavigation.keyCodeMap.down.push(83); // s  
    </script>
    <title>Forum Thread</title>
</head>

<body");

WriteLiteral(" data-thread-id=\"");


#line 57 "ThreadTemplate.cshtml"
                 Write(Model.ForumThread.ThreadId);


#line default
#line hidden
WriteLiteral("\"");

WriteLiteral(" data-thread-name=\"");


#line 57 "ThreadTemplate.cshtml"
                                                                Write(Model.ForumThread.Name);


#line default
#line hidden
WriteLiteral("\"");

WriteLiteral(" data-show-embedded-tweets=\"");


#line 57 "ThreadTemplate.cshtml"
                                                                                                                    Write(Model.EmbeddedTweets);


#line default
#line hidden
WriteLiteral("\"");

WriteLiteral(" data-show-embedded-gifv=\"");


#line 57 "ThreadTemplate.cshtml"
                                                                                                                                                                    Write(Model.EmbeddedGifv);


#line default
#line hidden
WriteLiteral("\"");

WriteLiteral(" data-show-embedded-video=\"");


#line 57 "ThreadTemplate.cshtml"
                                                                                                                                                                                                                   Write(Model.EmbeddedVideo);


#line default
#line hidden
WriteLiteral("\"");

WriteLiteral(" style=\"overflow-x: hidden;\"");

WriteLiteral(">\r\n\t<div");

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(" id=\"loggedinusername\"");

WriteLiteral(">");


#line 58 "ThreadTemplate.cshtml"
                                                Write(Model.ForumThread.LoggedInUserName);


#line default
#line hidden
WriteLiteral("</div>\r\n    <div");

WriteLiteral(" id=\"content\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"thread\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"container-fluid\"");

WriteLiteral(">\r\n            \t<div");

WriteLiteral(" class=\"row clearfix\"");

WriteLiteral(">\r\n");


#line 63 "ThreadTemplate.cshtml"
            		

#line default
#line hidden

#line 63 "ThreadTemplate.cshtml"
                     if(allPosts) {


#line default
#line hidden
WriteLiteral("            \t\t<div");

WriteLiteral(" id=\"showPosts\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" style=\"width: 100%; margin: 0;\"");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n                            <button");

WriteLiteral(" style=\"margin-left: 0; margin-right: 0; margin-top:1px; margin-bottom: 5px;\"");

WriteLiteral(" class=\"btn btn-block center-block\"");

WriteLiteral(" id=\"\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" onclick=\"window.ForumCommand(\'showPosts\', \'true\')\"");

WriteLiteral(">Show Previous Posts</button>\r\n                        </div>\r\n                  " +
"  </div>\r\n");


#line 69 "ThreadTemplate.cshtml"
            		}


#line default
#line hidden
WriteLiteral("            \t\t");


#line 70 "ThreadTemplate.cshtml"
                     foreach (var post in Model.Posts) {

            		if (seenCount > 2) {
                    	seenCount = 1;
            		}
            		string hasSeen = otherPosts && post.HasSeen ? "hiddenpost " : "";
                    hasSeen += post.HasSeen ? string.Concat("seen", seenCount) : string.Concat("postCount", seenCount);
                	seenCount++;



#line default
#line hidden
WriteLiteral("            \t\t<div");

WriteAttribute ("class", " class=\"", "\""

#line 79 "ThreadTemplate.cshtml"
, Tuple.Create<string,object,bool> ("", hasSeen

#line default
#line hidden
, false)
);
WriteLiteral(">\r\n            \t\t<div");

WriteLiteral(" class=\"col-md-4 thread-header\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" id=\"threadView\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" data-user-id=\"");


#line 82 "ThreadTemplate.cshtml"
                                          Write(post.User.Id);


#line default
#line hidden
WriteLiteral("\"");

WriteAttribute ("src", " src=\"", "\""

#line 82 "ThreadTemplate.cshtml"
                            , Tuple.Create<string,object,bool> ("", post.User.AvatarLink

#line default
#line hidden
, false)
);
WriteLiteral("\r\n                                alt=\"\"");

WriteLiteral(" class=\"av\"");

WriteLiteral(" border=\"0\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" class=\"userinfo\"");

WriteLiteral(">\r\n                                    <p");

WriteLiteral(" style=\"padding: 0;\"");

WriteLiteral(" class=\"text\"");

WriteLiteral("><span");

WriteAttribute ("class", " class=\"", "\""

#line 85 "ThreadTemplate.cshtml"
                                              , Tuple.Create<string,object,bool> ("", post.User.Roles

#line default
#line hidden
, false)
);
WriteLiteral(">");


#line 85 "ThreadTemplate.cshtml"
                                                                                                  Write(post.User.Username);


#line default
#line hidden
WriteLiteral("</span></p>\r\n                                    <p");

WriteLiteral(" class=\"text article-title\"");

WriteLiteral("><span");

WriteLiteral(" class=\"registered\"");

WriteLiteral(">");


#line 86 "ThreadTemplate.cshtml"
                                                                                      Write(post.PostDate);


#line default
#line hidden
WriteLiteral("</span></p>\r\n                                </div>\r\n                            " +
"</div>\r\n                    </div>\r\n                    <div");

WriteLiteral(" style=\"padding: 15px;\"");

WriteLiteral(" class=\"col-md-8\"");

WriteLiteral(">\r\n                    \t<div");

WriteLiteral(" class=\"article-content\"");

WriteLiteral(">\r\n                    \t\t<div");

WriteAttribute ("id", " id=\"", "\""

#line 92 "ThreadTemplate.cshtml"
, Tuple.Create<string,object,bool> ("", post.PostId

#line default
#line hidden
, false)
);
WriteLiteral(" class=\"postbody\"");

WriteLiteral(">\r\n");


#line 93 "ThreadTemplate.cshtml"
                    		

#line default
#line hidden

#line 93 "ThreadTemplate.cshtml"
                              
								WriteLiteral(post.PostHtml);
							

#line default
#line hidden
WriteLiteral("\r\n                    \t\t</div>\r\n");


#line 97 "ThreadTemplate.cshtml"
                    		

#line default
#line hidden

#line 97 "ThreadTemplate.cshtml"
                             if (Model.IsLoggedIn) {


#line default
#line hidden
WriteLiteral("                    \t\t<footer>\r\n                                    <tr");

WriteLiteral(" class=\"postbar\"");

WriteLiteral(">\r\n                                        <td");

WriteLiteral(" class=\"postlinks\"");

WriteLiteral(">\r\n                                            <ul");

WriteLiteral(" class=\"profilelinks\"");

WriteLiteral(">\r\n                                                <li>\r\n                        " +
"                            <button");

WriteLiteral(" class=\"btn\"");

WriteLiteral(" id=\"\"");

WriteLiteral(" type=\"submit\"");

WriteAttribute ("onclick", " onclick=\"", "\""
, Tuple.Create<string,object,bool> ("", "window.QuotePost(\'", true)

#line 103 "ThreadTemplate.cshtml"
                                                                                , Tuple.Create<string,object,bool> ("", post.PostId

#line default
#line hidden
, false)
, Tuple.Create<string,object,bool> ("", "\')", true)
);
WriteLiteral(">Quote</button>\r\n                                                </li>\r\n         " +
"                                       <li>\r\n                                   " +
"                 <button");

WriteLiteral(" style=\"margin-left: 7px;\"");

WriteLiteral(" class=\"btn\"");

WriteLiteral(" id=\"\"");

WriteLiteral(" type=\"submit\"");

WriteAttribute ("onclick", " onclick=\"", "\""
, Tuple.Create<string,object,bool> ("", "window.MarkAsLastRead(\'", true)

#line 106 "ThreadTemplate.cshtml"
                                                                                                               , Tuple.Create<string,object,bool> ("", post.PostIndex

#line default
#line hidden
, false)
, Tuple.Create<string,object,bool> ("", "\')", true)
);
WriteLiteral(">Last Read</button>\r\n                                                </li>\r\n");


#line 108 "ThreadTemplate.cshtml"
                                                

#line default
#line hidden

#line 108 "ThreadTemplate.cshtml"
                                                 if (post.User.IsCurrentUserPost) {


#line default
#line hidden
WriteLiteral("                                                <li>\r\n                           " +
"                         <button");

WriteLiteral(" style=\"margin-left: 7px;\"");

WriteLiteral(" class=\"btn\r\n                                                    \"");

WriteLiteral(" id=\"\"");

WriteLiteral(" type=\"submit\"");

WriteAttribute ("onclick", " onclick=\"", "\""
, Tuple.Create<string,object,bool> ("", "window.EditPost(\'", true)

#line 111 "ThreadTemplate.cshtml"
                                                             , Tuple.Create<string,object,bool> ("", post.PostId

#line default
#line hidden
, false)
, Tuple.Create<string,object,bool> ("", "\')", true)
);
WriteLiteral(">Edit</button>\r\n                                                </li>\r\n");


#line 113 "ThreadTemplate.cshtml"
                                                }


#line default
#line hidden
WriteLiteral("                                            </ul>\r\n                              " +
"          </td>\r\n                                    </tr>\r\n                    " +
"        </footer>\r\n");


#line 118 "ThreadTemplate.cshtml"
                    		}


#line default
#line hidden
WriteLiteral("                    \t</div>\r\n                    </div>\r\n            \t\t</div>\r\n");


#line 122 "ThreadTemplate.cshtml"
           	 		}


#line default
#line hidden
WriteLiteral("            \t</div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">

        window.onload = function () {
            Gifffer();

            $("".postbody a"").click(function () {
                var bool = window.OpenLink(this.href);
                return bool;
            });

            $("".standard a"").click(function () {
                var bool = window.OpenLink(this.href);
                return bool;
            });

            $("".av"").on(""click"", function () {
                ForumCommand('userProfile', $(this).attr('data-user-id'));
            });

            $(""img"").on(""dblclick"", function () {
                ForumCommand('downloadImage', this.src);
            });

            var b = RegExp(""^"" + $(""#loggedinusername"").text().replace(/([.*+?^${}()|\[\]\/\\])/g,""\\$1"") + ""\\s+posted:$"");
            $("".bbc-block h4"").filter(function () {
                return b.test($(this).text());
            }).map(function() {
                return $(this).closest("".bbc-block"")[0];
            }).addClass(""userquoted"");
            ForumCommand('scrollToDivStart', $(""#scrolltopoststring"").text());
        };

        var reloadPage = function () {
            return new Promise(function (resolve, reject) {
                if (true) {
                    ForumCommand('reloadPage', null);
                    resolve();
                } else {
                    reject();
                }
            });
        };
    </script>
</body>
</html>
");

}
}

// NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
// in order to customize it or share it between multiple templates, and specify the template's base 
// class via the @inherits directive.
public abstract class ThreadTemplateBase
{

		// This field is OPTIONAL, but used by the default implementation of Generate, Write, WriteAttribute and WriteLiteral
		//
		System.IO.TextWriter __razor_writer;

		// This method is OPTIONAL
		//
		/// <summary>Executes the template and returns the output as a string.</summary>
		/// <returns>The template output.</returns>
		public string GenerateString ()
		{
			using (var sw = new System.IO.StringWriter ()) {
				Generate (sw);
				return sw.ToString ();
			}
		}

		// This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of __razor_writer
		// and provide another means of invoking Execute.
		//
		/// <summary>Executes the template, writing to the provided text writer.</summary>
		/// <param name="writer">The TextWriter to which to write the template output.</param>
		public void Generate (System.IO.TextWriter writer)
		{
			__razor_writer = writer;
			Execute ();
			__razor_writer = null;
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the template output without HTML escaping it.</summary>
		/// <param name="value">The literal value.</param>
		protected void WriteLiteral (string value)
		{
			__razor_writer.Write (value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the TextWriter without HTML escaping it.</summary>
		/// <param name="writer">The TextWriter to which to write the literal.</param>
		/// <param name="value">The literal value.</param>
		protected static void WriteLiteralTo (System.IO.TextWriter writer, string value)
		{
			writer.Write (value);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a value to the template output, HTML escaping it if necessary.</summary>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected void Write (object value)
		{
			WriteTo (__razor_writer, value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes an object value to the TextWriter, HTML escaping it if necessary.</summary>
		/// <param name="writer">The TextWriter to which to write the value.</param>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected static void WriteTo (System.IO.TextWriter writer, object value)
		{
			if (value == null)
				return;

			var write = value as Action<System.IO.TextWriter>;
			if (write != null) {
				write (writer);
				return;
			}

			//NOTE: a more sophisticated implementation would write safe and pre-escaped values directly to the
			//instead of double-escaping. See System.Web.IHtmlString in ASP.NET 4.0 for an example of this.
			writer.Write(System.Net.WebUtility.HtmlEncode (value.ToString ()));
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to the template output.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		protected void WriteAttribute (string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			WriteAttributeTo (__razor_writer, name, prefix, suffix, values);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to a TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which to write the attribute.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		///<remarks>Used by Razor helpers to write attributes.</remarks>
		protected static void WriteAttributeTo (System.IO.TextWriter writer, string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			// this is based on System.Web.WebPages.WebPageExecutingBase
			// Copyright (c) Microsoft Open Technologies, Inc.
			// Licensed under the Apache License, Version 2.0
			if (values.Length == 0) {
				// Explicitly empty attribute, so write the prefix and suffix
				writer.Write (prefix);
				writer.Write (suffix);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			for (int i = 0; i < values.Length; i++) {
				Tuple<string,object,bool> attrVal = values [i];
				string attPrefix = attrVal.Item1;
				object value = attrVal.Item2;
				bool isLiteral = attrVal.Item3;

				if (value == null) {
					// Nothing to write
					continue;
				}

				// The special cases here are that the value we're writing might already be a string, or that the
				// value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
				// of the string 'true'. If the value is the bool 'false' we don't want to write anything.
				//
				// Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
				string stringValue;
				bool? boolValue = value as bool?;
				if (boolValue == true) {
					stringValue = name;
				} else if (boolValue == false) {
					continue;
				} else {
					stringValue = value as string;
				}

				if (first) {
					writer.Write (prefix);
					first = false;
				} else {
					writer.Write (attPrefix);
				}

				if (isLiteral) {
					writer.Write (stringValue ?? value);
				} else {
					WriteTo (writer, stringValue ?? value);
				}
				wroteSomething = true;
			}
			if (wroteSomething) {
				writer.Write (suffix);
			}
		}
		// This method is REQUIRED. The generated Razor subclass will override it with the generated code.
		//
		///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
		///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
		public abstract void Execute ();

}
}
#pragma warning restore 1591
