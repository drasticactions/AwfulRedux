#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AwfulWebTemplate
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "2.6.0.0")]
public partial class PrivateMessageTemplate : PrivateMessageTemplateBase
{

#line hidden

#line 1 "PrivateMessageTemplate.cshtml"
public PrivateMessageTemplateModel Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta");

WriteLiteral(" name=\"twitter:dnt\"");

WriteLiteral(" content=\"on\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1, user-scalable=no\"");

WriteLiteral(">\r\n    <link");

WriteLiteral(" href=\"ms-appx-web:///Assets/Website/CSS/winstrap.css\"");

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


#line 10 "PrivateMessageTemplate.cshtml"
    

#line default
#line hidden

#line 10 "PrivateMessageTemplate.cshtml"
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


#line 13 "PrivateMessageTemplate.cshtml"
    }


#line default
#line hidden
WriteLiteral("    <!--<script type=\"text/javascript\" async=\"\" src=\"JS/jquery.min.js\"></script>-" +
"->\r\n    <script");

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

WriteLiteral("></script>\r\n    <title>Forum Thread</title>\r\n</head>\r\n\r\n<body");

WriteLiteral(" data-show-embedded-tweets=\"true\"");

WriteLiteral(" data-show-embedded-gifv=\"true\"");

WriteLiteral(" data-show-embedded-video=\"true\"");

WriteLiteral(" style=\"overflow-x: hidden;\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" id=\"content\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"thread\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"container-fluid\"");

WriteLiteral(">\r\n            \t<div");

WriteLiteral(" class=\"row clearfix\"");

WriteLiteral(">\r\n            \t\t<div");

WriteLiteral(" style=\"overflow: auto;\"");

WriteLiteral(" class=\"postCount1\"");

WriteLiteral(">\r\n            \t\t<div");

WriteLiteral(" class=\"col-md-4\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" id=\"threadView\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" data-user-id=\"");


#line 33 "PrivateMessageTemplate.cshtml"
                                          Write(Model.PMPost.User.Id);


#line default
#line hidden
WriteLiteral("\"");

WriteAttribute ("src", " src=\"", "\""

#line 33 "PrivateMessageTemplate.cshtml"
                                    , Tuple.Create<string,object,bool> ("", Model.PMPost.User.AvatarLink

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

#line 36 "PrivateMessageTemplate.cshtml"
                                              , Tuple.Create<string,object,bool> ("", Model.PMPost.User.Roles

#line default
#line hidden
, false)
);
WriteLiteral(">");


#line 36 "PrivateMessageTemplate.cshtml"
                                                                                                          Write(Model.PMPost.User.Username);


#line default
#line hidden
WriteLiteral("</span></p>\r\n                                    <p");

WriteLiteral(" class=\"text article-title\"");

WriteLiteral("><span");

WriteLiteral(" class=\"registered\"");

WriteLiteral(">");


#line 37 "PrivateMessageTemplate.cshtml"
                                                                                      Write(Model.PMPost.User.DateJoinedShort);


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

#line 43 "PrivateMessageTemplate.cshtml"
, Tuple.Create<string,object,bool> ("", Model.PMPost.PostId

#line default
#line hidden
, false)
);
WriteLiteral(" class=\"postbody\"");

WriteLiteral(">\r\n");


#line 44 "PrivateMessageTemplate.cshtml"
                    		

#line default
#line hidden

#line 44 "PrivateMessageTemplate.cshtml"
                              
								WriteLiteral(@Model.PMPost.PostHtml);
							

#line default
#line hidden
WriteLiteral("\r\n                    \t\t</div>\r\n                    \t</div>\r\n                    " +
"</div>\r\n            \t\t</div>\r\n            \t</div>\r\n            </div>\r\n        <" +
"/div>\r\n    </div>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n\r\n            window.onload = function () {\r\n                Gifffer();\r\n\r\n   " +
"             $(\".postbody a\").click(function () {\r\n                    var bool " +
"= window.OpenLink(this.href);\r\n                    return bool;\r\n               " +
" });\r\n\r\n                $(\".standard a\").click(function () {\r\n                  " +
"  var bool = window.OpenLink(this.href);\r\n                    return bool;\r\n    " +
"            });\r\n\r\n                $(\".av\").on(\"click\", function () {\r\n         " +
"           ForumCommand(\'userProfile\', $(this).attr(\'data-user-id\'));\r\n         " +
"       });\r\n\r\n                $(\"img\").on(\"dblclick\", function () {\r\n           " +
"         ForumCommand(\'downloadImage\', this.src);\r\n                });\r\n\r\n      " +
"          var b = RegExp(\"^\" + $(\"#loggedinusername\").text().replace(/([.*+?^${}" +
"()|\\[\\]\\/\\\\])/g,\"\\\\$1\") + \"\\\\s+posted:$\");\r\n                $(\".bbc-block h4\").f" +
"ilter(function () {\r\n                    return b.test($(this).text());\r\n       " +
"         }).map(function() {\r\n                    return $(this).closest(\".bbc-b" +
"lock\")[0];\r\n                }).addClass(\"userquoted\");\r\n                ForumCom" +
"mand(\'scrollToDivStart\', $(\"#scrolltopoststring\").text());\r\n            };\r\n\r\n  " +
"          var reloadPage = function () {\r\n                return new Promise(fun" +
"ction (resolve, reject) {\r\n                    if (true) {\r\n                    " +
"    ForumCommand(\'reloadPage\', null);\r\n                        resolve();\r\n     " +
"               } else {\r\n                        reject();\r\n                    " +
"}\r\n                });\r\n            };\r\n    </script>\r\n</body>\r\n</html>\r\n\r\n");

}
}

// NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
// in order to customize it or share it between multiple templates, and specify the template's base 
// class via the @inherits directive.
public abstract class PrivateMessageTemplateBase
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
