// SkyClip
// - HttpResponseException.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;
using System.Net.Http;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     Thrown when a response is non-successful.
    /// </summary>
    public class HttpResponseException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseException"/> class.
        /// </summary>
        /// <param name="responseMessage">
        /// The response message.
        /// </param>
        public HttpResponseException(HttpResponseMessage responseMessage)
            : base("A non-successful response was returned.")
        {
            this.ResponseMessage = responseMessage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the response message.
        /// </summary>
        /// <value>
        ///     The response message.
        /// </value>
        public HttpResponseMessage ResponseMessage { get; private set; }

        #endregion
    }
}