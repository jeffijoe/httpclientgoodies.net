// SkyClip
// - UrlHelper.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System.Linq;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     URL Helper.
    /// </summary>
    public static class UrlHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Combines URI parts, taking care of trailing and starting slashes.
        ///     See http://stackoverflow.com/a/6704287
        /// </summary>
        /// <param name="uriParts">
        /// The URI parts to combine.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Combine(params string[] uriParts)
        {
            var uri = string.Empty;
            if (uriParts != null && uriParts.Any())
            {
                uriParts = uriParts.Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();
                char[] trimChars = { '\\', '/' };
                uri = (uriParts[0] ?? string.Empty).TrimEnd(trimChars);
                for (var i = 1; i < uriParts.Count(); i++)
                {
                    uri = string.Format(
                        "{0}/{1}", 
                        uri.TrimEnd(trimChars), 
                        (uriParts[i] ?? string.Empty).TrimStart(trimChars));
                }
            }

            return uri;
        }

        #endregion
    }
}