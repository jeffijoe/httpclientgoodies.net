// SkyClip
// - JsonContent.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     Json content using JSON.NET.
    /// </summary>
    public class JsonContent : StringContent
    {
        #region Constants

        /// <summary>
        ///     The media type.
        /// </summary>
        public const string MediaType = "application/json";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent"/> class.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="serializerSettings">
        /// The serializer settings.
        /// </param>
        public JsonContent(object obj, JsonSerializerSettings serializerSettings = null)
            : base(Serialize(obj, serializerSettings), Encoding.UTF8, MediaType)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="serializerSettings">
        /// The serializer settings.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string Serialize(object obj, JsonSerializerSettings serializerSettings)
        {
            return serializerSettings != null
                       ? JsonConvert.SerializeObject(obj, serializerSettings)
                       : JsonConvert.SerializeObject(obj);
        }

        #endregion
    }
}