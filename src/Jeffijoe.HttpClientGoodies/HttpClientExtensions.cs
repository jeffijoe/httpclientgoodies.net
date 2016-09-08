// SkyClip
// - HttpClientExtensions.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     HTTP client extensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Reads the content as JSON asynchronously.
        /// </summary>
        /// <typeparam name="T">
        /// Type to deserialize to.
        /// </typeparam>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="serializerSettings">
        /// The serializer settings.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Attempted to deserialize JSON content, but content was null.
        /// </exception>
        public static async Task<T> ReadAsJsonAsync<T>(
            this HttpContent content, 
            JsonSerializerSettings serializerSettings = null) where T : class
        {
            var str = await content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(str))
            {
                throw new JsonContentException("Attempted to deserialize JSON content, but content was null.");
            }

            if (str.StartsWith("<"))
            {
                throw new JsonContentException(
                    "Attempted to deserialize JSON content, but it looks XML-ish.\r\nContent:\r\n" + str);
            }

            return serializerSettings == null
                       ? JsonConvert.DeserializeObject<T>(str)
                       : JsonConvert.DeserializeObject<T>(str, serializerSettings);
        }

        /// <summary>
        /// Throws if unsuccessful.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// Thrown if unsuccessful.
        /// </exception>
        public static HttpResponseMessage ThrowIfUnsuccessful(this HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                return message;
            }

            throw new HttpResponseException(message);
        }

        #endregion
    }
}