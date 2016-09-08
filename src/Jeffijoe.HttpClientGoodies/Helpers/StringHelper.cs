// SkyClip
// - StringHelper.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     The string helper.
    /// </summary>
    public static class StringHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the string to base64.
        /// </summary>
        /// <param name="src">
        /// The source.
        /// </param>
        /// <returns>
        /// The base64d string.
        /// </returns>
        public static string ToBase64(this string src)
        {
            var bytes = Encoding.UTF8.GetBytes(src);
            return Convert.ToBase64String(bytes);
        }

        #endregion
    }
}