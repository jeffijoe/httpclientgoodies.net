// SkyClip
// - JsonContentException.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     Thrown when there are issues deserializing JSON content.
    /// </summary>
    public class JsonContentException : InvalidOperationException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public JsonContentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public JsonContentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}