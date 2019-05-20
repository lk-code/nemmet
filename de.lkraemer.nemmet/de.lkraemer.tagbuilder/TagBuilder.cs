using System.Collections.Generic;

namespace de.lkraemer.tagbuilder
{
    public class TagBuilder
    {
        #region # public accessable properties #

        /// <summary>
        /// a list of the attributes for this tag
        /// </summary>
        public Dictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        public string InnerHtml { get; private set; } = string.Empty;

        #endregion

        #region # private properties #

        /// <summary>
        /// 
        /// </summary>
        private string tagName { get; set; } = string.Empty;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        public TagBuilder(string tagName)
        {
            this.tagName = tagName;
        }

        #region # public methods #

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddAttribute(string name, string value)
        {
            this.Attributes.Add(name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        public void AddInnerHtml(string html)
        {
            this.InnerHtml += html;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            string html = "";

            html += "<" + this.tagName + "";
            html += this.GetRenderedAttributes();
            html += ">";

            html += this.InnerHtml;

            html += "</" + this.tagName + ">";

            return html;
        }

        /// <summary>
        /// returns all attributes as a rendered string
        /// </summary>
        /// <returns></returns>
        public string GetRenderedAttributes()
        {
            string attributes = string.Empty;

            foreach(var attributePair in this.Attributes)
            {
                if (!string.IsNullOrEmpty(attributePair.Value) && !string.IsNullOrWhiteSpace(attributePair.Value))
                {
                    string attributeResultScheme = " {0}=\"{1}\"";
                    string attributeResult = string.Format(attributeResultScheme, attributePair.Key, attributePair.Value);

                    attributes += attributeResult;
                }
            }

            return attributes;
        }

        #endregion
    }
}
