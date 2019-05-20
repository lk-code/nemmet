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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace de.lkraemer.nemmet
{
    /// <summary>
    /// 
    /// </summary>
    static class NemmetTagExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string[] SplitOn(this string text, string delim = " ")
        {
            if (delim.Length == 1)
            {
                return text.SplitOnAny(delim);
            }

            return Regex.Split(text, delim);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string[] SplitOnAny(this string text, string delim = " ")
        {
            return text.Split(delim.ToCharArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string GetBefore(this string text, string delim)
        {
            return text.SplitOn(delim).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string GetAfter(this string text, string delim)
        {
            return text.SplitOn(delim).Last();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string GetBeforeAny(this string text, string delim)
        {
            return text.SplitOnAny(delim).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string GetAfterAny(this string text, string delim)
        {
            return text.SplitOnAny(delim).Last();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public static string JoinOn(this IEnumerable<string> collection, string delim = null)
        {
            return string.Join(delim ?? string.Empty, collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Remove(this string text, string target)
        {
            return text.Replace(target, string.Empty);
        }
    }
}
