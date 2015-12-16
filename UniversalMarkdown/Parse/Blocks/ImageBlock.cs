using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalMarkdown.Helpers;

namespace UniversalMarkdown.Parse.Elements
{
    class ImageBlock : MarkdownBlock
    {
        public string Url { get; set; }

        public ImageBlock()
            : base(MarkdownBlockType.Image)
        { }

        internal override int Parse(ref string markdown, int startingPos, int maxEndingPos)
        {
            // Find all of the link parts
            // Add one to startingPos, to account for !
            startingPos = startingPos + 1;
            int linkTextOpen = Common.IndexOf(ref markdown, '[', startingPos, maxEndingPos);
            int linkTextClose = Common.IndexOf(ref markdown, ']', linkTextOpen, maxEndingPos);
            int linkOpen = Common.IndexOf(ref markdown, '(', linkTextClose, maxEndingPos);
            int linkClose = Common.IndexOf(ref markdown, ')', linkOpen, maxEndingPos);

            // Make sure there is something to parse, and not just dead space
            linkTextOpen++;
            if (linkTextClose > linkTextOpen)
            {
                // Parse any children of this link element
                ParseInlineChildren(ref markdown, linkTextOpen, linkTextClose);
            }

            //for (int count = 0; count < Children.Count; count++)
            //{

            //}

            // Grab the link
            linkOpen++;
            Url = markdown.Substring(linkOpen, linkClose - linkOpen);

            // Return the point after the )
            return linkClose + 1;
        }

        /// <summary>
        /// Verify a match that is found in the markdown. If the match is good and the rest of the element exits the function should
        /// return true and the element will be matched. If if is a false positive return false and we will keep looking.
        /// </summary>
        /// <param name="markdown">The markdown to match</param>
        /// <param name="startingPos">Where the first trip char should be found</param>
        /// <param name="maxEndingPos">The max length to look in.</param>
        /// <param name="elementEndingPos">If found, the ending pos of the element found.</param>
        /// <returns></returns>
        public static bool VerifyMatch(ref string markdown, int startingPos, int maxEndingPos, ref int elementStartingPos, ref int elementEndingPos)
        {
            // Sanity check
            if (markdown[startingPos] == '!' && markdown[startingPos++] == '[')
            {
                int linkTextOpen = startingPos + 1;
                // Ensure we have a link
                int linkTextClose = Common.IndexOf(ref markdown, ']', linkTextOpen, maxEndingPos);
                if (linkTextClose != -1)
                {
                    int linkOpen = Common.IndexOf(ref markdown, '(', linkTextClose, maxEndingPos);
                    if (linkOpen != -1)
                    {
                        char test = markdown[maxEndingPos];
                        int linkClose = Common.IndexOf(ref markdown, ')', linkOpen, maxEndingPos);
                        if (linkClose != -1)
                        {
                            // Make sure the order is correct
                            if (linkTextOpen < linkTextClose && linkTextClose < linkOpen && linkOpen < linkClose)
                            {
                                elementStartingPos = startingPos;
                                elementEndingPos = linkClose + 1;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CanHandleBlock(ref string markdown, int nextCharPos, int endingPos)
        {
            return true;
        }

    }
}
