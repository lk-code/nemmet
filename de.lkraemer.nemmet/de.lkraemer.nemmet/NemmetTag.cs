/**
 * MIT License
 * 
 * Copyright (c) 2019 lk-code
 * see more at https://github.com/lk-code/nemmet
 * 
 * based on the nemmet project of deanebarker ast https://github.com/deanebarker/Nemmet
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using de.lkraemer.tagbuilder;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace de.lkraemer.nemmet
{
    public class NemmetTag
    {
        public string Name { get; set; }
        public List<string> Classes { get; set; }
        public string Id { get; set; }
        public string InnerHtml { get; set; }
        public List<NemmetTag> Children { get; set; }
        public NemmetTag Parent { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        // Regex and delimiter constants
        private const char CHILD_OPERATOR = '>';
        private const char SIBLING_OPERATOR = '+';
        private const char CLIMBUP_OPERATOR = '^';
        private const string CONTENT_PATTERN = @"{([^}]*)}";
        private const string ATTRIBUTES_PATTERN = @"\[([^\]]*)\]";
        private const string ID_CLASS_PATTERN = @"[#\.][^{#\.]*";
        private const string NONWORD_PATTERN = @"\W";
        private const string SPACE = " ";
        private const char OPEN_CURLEY_BRACE = '{';
        private const char CLOSING_CURLEY_BRACE = '}';
        private const char OPEN_PARAN = '(';
        private const char CLOSE_PARAN = ')';

        /// <summary>
        /// this returns a List<NemmetTag> because there could be multiple top-level tags ("tag1+tag2+tag3"). We can't assume this HTML fragment will be well-formed.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<NemmetTag> Parse(string code)
        {
            // The top tag on this stack represents the tag to which new tags will be added as children. We put an empty placeholder in there to initialize it.
            var tagStack = new Stack<NemmetTag>();
            tagStack.Push(new NemmetTag("root"));

            // We'll keep track if we're in a brace or not to determine whether something is an actual operator or just text content
            var inBraces = false;

            // Iterate through each character
            // We add the sibling operator to the end to make sure the last tag gets added (remember, we only process the buffer when we encouter an operator, so we need to make sure there's a final operator on the end)
            var buffer = new StringBuilder();
            foreach (var character in string.Concat(code, SIBLING_OPERATOR))
            {
                // Toggle the braces indicator so we know if something is an operator of just content
                if (character == OPEN_CURLEY_BRACE || character == CLOSING_CURLEY_BRACE)
                {
                    inBraces = !inBraces;
                }

                // Is this an operator that is NOT contained in a brace?
                if (string.Concat(SIBLING_OPERATOR, CHILD_OPERATOR, CLIMBUP_OPERATOR, OPEN_PARAN).Contains(character) && !inBraces)
                {
                    // We have encountered an operator, which means whatever is in the buffer represents a single tag
                    // We need to...
                    // 1. Create this tag
                    // 2. Evaluate the operator to determine where the NEXT tag will go by manipulating the tagStack)

                    // If there's anything in the buffer, process it as a new child of the context tag
                    // (If you're climbing up more than one level at a time ("^^") there might not be anything in the buffer.)
                    NemmetTag tag = null;
                    if (buffer.Length > 0)
                    {
                        tag = new NemmetTag(buffer.ToString());

                        // If the name is empty, then name this based on the defaults
                        // (I can't do this inside of NemmetTag, because it requires access to the prior tag...)
                        if (string.IsNullOrWhiteSpace(tag.Name))
                        {
                            tag.Name = NemmetParsingOptions.DefaultTagMap.ContainsKey(tagStack.Peek().Name) ? NemmetParsingOptions.DefaultTagMap[tagStack.Peek().Name] : NemmetParsingOptions.DefaultTag;
                        }

                        // Add this to the top tag on the stack
                        tagStack.Peek().Children.Add(tag);

                        // We empty the buffer so we can start accumulating the next tag
                        buffer.Clear();
                    }

                    // Now, what do we do with the NEXT tag?

                    // The next tag should be added to the same tag as the last one.
                    if (character == SIBLING_OPERATOR)
                    {
                        // Do nothing. This is just for clarity.
                    }

                    // Climb up. Remove the top tag, to reveal its parent underneath.
                    if (character == CLIMBUP_OPERATOR)
                    {
                        tagStack.Pop();
                    }

                    // Descend. Add this tag to the top of stack.
                    if (character == CHILD_OPERATOR)
                    {
                        tagStack.Push(tag);
                    }
                }
                else
                {
                    buffer.Append(character);
                }
            }

            // The base tag in the stack was just a placeholder, remember. We want to return the top-level children of that.
            return tagStack.Last().Children;
        }

        public NemmetTag(string token)
        {
            Classes = new List<string>();
            Children = new List<NemmetTag>();
            Attributes = new Dictionary<string, string>();

            // The incoming text string should represent THIS TAG ONLY.  The string should NOT have any operators in it. It should be the configuration this tag only.

            // The name is everything before the first non-word character
            Name = token.GetBefore(NONWORD_PATTERN);
            if (NemmetParsingOptions.AlwaysLowerCaseTagName)
            {
                Name = Name.ToLowerInvariant();
            }

            // Tag content
            foreach (Match subtoken in Regex.Matches(token, CONTENT_PATTERN))
            {
                InnerHtml = subtoken.Groups[1].Value;
                token = token.Remove(subtoken.Value);
            }

            // Tag attributes
            foreach (Match subtoken in Regex.Matches(token, ATTRIBUTES_PATTERN))
            {
                foreach (var attribute in subtoken.Groups[1].Value.SplitOnAny())
                {
                    var key = attribute.GetBefore("=");
                    var value = attribute.GetAfter("=");
                    Attributes.Add(key, value);
                }
                token = token.Remove(subtoken.Value);
            }

            // Tag ID and class
            foreach (Match subtoken in Regex.Matches(token, ID_CLASS_PATTERN))
            {
                if (subtoken.Value.StartsWith("#"))
                {
                    Id = subtoken.Value.TrimStart("#".ToCharArray());
                }

                if (subtoken.Value.StartsWith("."))
                {
                    Classes.Add(subtoken.Value.TrimStart(".".ToCharArray()));
                }
            }
        }

        /// <summary>
        /// this returns a CSS representation of the tag ("div.class"), which is handy for identification
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var name = Name;
            if (Classes.Any())
            {
                name = string.Concat(name, ".", Classes.JoinOn("."));
            }
            return name;
        }

        /// <summary>
        /// render this tag to a html string
        /// </summary>
        /// <returns>the html content as string</returns>
        public string ToHtml()
        {
            // If the tag has no name, then it's not a tag -- it's just text content ("{content}")
            if (string.IsNullOrWhiteSpace(Name))
            {
                return InnerHtml;
            }

            TagBuilder tagBuilder = new TagBuilder(Name);

            // Add the ID
            if (!string.IsNullOrWhiteSpace(Id))
            {
                tagBuilder.AddAttribute("id", Id);
            }

            // Add the classes
            if (Classes.Any())
            {
                tagBuilder.AddAttribute("class", Classes.JoinOn(SPACE));
            }

            // Add the attributes
            foreach (var attribute in Attributes)
            {
                tagBuilder.AddAttribute(attribute.Key, attribute.Value);
            }

            // Render
            tagBuilder.AddInnerContent(InnerHtml);

            // Recurse through the children
            foreach (var tag in Children)
            {
                tagBuilder.AddInnerContent(tag.ToHtml());
            }

            string html = tagBuilder.RenderAsString();

            return html;
        }
    }
}
