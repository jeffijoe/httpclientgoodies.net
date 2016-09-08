// SkyClip
// - RequestBuilder.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     Class that builds <see cref="HttpRequestMessage" />s fluidly.
    /// </summary>
    /// <author>Jeff Hansen</author>
    // ReSharper disable ParameterHidesMember
    public class RequestBuilder
    {
        #region Fields

        /// <summary>
        ///     The message.
        /// </summary>
        private readonly HttpRequestMessage message;

        /// <summary>
        ///     The URI builder.
        /// </summary>
        private readonly Dictionary<string, object> query;

        /// <summary>
        ///     The URI segments.
        /// </summary>
        private readonly Dictionary<string, object> uriSegments;

        /// <summary>
        ///     The authentication header.
        /// </summary>
        private AuthenticationHeaderValue authentication;

        /// <summary>
        ///     The base URI.
        /// </summary>
        private Uri baseUri;
        
        /// <summary>
        ///     The resource URI.
        /// </summary>
        private string resourceUri;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBuilder"/> class.
        /// </summary>
        /// <param name="message">
        /// The message to modify, if any.
        /// </param>
        public RequestBuilder(HttpRequestMessage message = null)
        {
            this.message = message ?? new HttpRequestMessage();
            this.query = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            this.uriSegments = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the message that will be modified with a new URI when <see cref="ToHttpRequestMessage" /> is called.
        /// </summary>
        public HttpRequestMessage RequestMessage
        {
            get
            {
                return this.message;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the header. Modifies the <see cref="RequestMessage"/> directly.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder AddHeader(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value != null)
            {
                this.RequestMessage.Headers.Add(key, value.ToString());
            }

            return this;
        }

        /// <summary>
        /// Sets the full request URI.
        /// </summary>
        /// <param name="uri">The URI to use.</param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder Uri(string uri)
        {
            return this.Uri(new System.Uri(uri));
        }

        /// <summary>
        /// Sets the full request URI.
        /// </summary>
        /// <param name="uri">The URI to use.</param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder Uri(Uri uri)
        {
            this.RequestMessage.RequestUri = uri;
            return this;
        }

        /// <summary>
        /// Adds the headers. Modifies the <see cref="RequestMessage"/> directly.
        /// </summary>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder AddHeaders(IEnumerable<KeyValuePair<string, object>> headers)
        {
            if (headers == null)
            {
                return this;
            }

            foreach (var keyValuePair in headers)
            {
                this.AddHeader(keyValuePair.Key, keyValuePair.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds the key-value pairs to the query string.
        /// </summary>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder AddQueries(IEnumerable<KeyValuePair<string, object>> queries)
        {
            if (queries == null)
            {
                return this;
            }

            foreach (var keyValuePair in queries)
            {
                this.AddQuery(keyValuePair.Key, keyValuePair.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds the value to the query string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder AddQuery(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value != null)
            {
                this.query[key] = value;
            }

            return this;
        }

        /// <summary>
        /// Adds the URI segment. E.g. /api/tasks/{id}, you would then add the id segment, e.g. AddUriSegment("id", 123).
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder AddUriSegment(string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return this;
            }

            this.uriSegments.Add(key, value);
            return this;
        }

        /// <summary>
        /// Authentications the specified scheme.
        /// </summary>
        /// <param name="scheme">
        /// The scheme.
        /// </param>
        /// <param name="parameter">
        /// The value.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder Authentication(string scheme, string parameter)
        {
            this.authentication = new AuthenticationHeaderValue(scheme, parameter);
            return this;
        }

        /// <summary>
        /// Sets the base URI. Will be combined with the resource URI from <see cref="ResourceUri"/>.
        /// </summary>
        /// <param name="baseUri">
        /// The base URI.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder BaseUri(Uri baseUri)
        {
            this.baseUri = baseUri;
            return this;
        }

        /// <summary>
        /// Sets the base URI. Will be combined with the resource URI from <see cref="ResourceUri"/>.
        /// </summary>
        /// <param name="baseUri">
        /// The base URI.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder BaseUri(string baseUri)
        {
            this.baseUri = new Uri(baseUri);
            return this;
        }

        /// <summary>
        /// Sets up a Basic Authentication header, with base64 and everything.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder BasicAuthentication(string username, string password)
        {
            var encoded = string.Format("{0}:{1}", username, password).ToBase64();
            return this.Authentication("Basic", encoded);
        }

        /// <summary>
        /// Sets the specified content as content. Modifies the <see cref="RequestMessage"/> directly.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder Content(HttpContent content)
        {
            this.RequestMessage.Content = content;
            return this;
        }

        /// <summary>
        /// Sets the Content to a <see cref="JsonContent"/>, with the specified settings. Modifies the
        ///     <see cref="RequestMessage"/> directly.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object to deserialize.
        /// </typeparam>
        /// <param name="objToSerialize">
        /// The object to serialize.
        /// </param>
        /// <param name="serializerSettings">
        /// The serializer settings.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder JsonContent<T>(T objToSerialize, JsonSerializerSettings serializerSettings = null)
        {
            this.RequestMessage.Content = new JsonContent(objToSerialize, serializerSettings);
            return this;
        }

        /// <summary>
        /// Sets the given method on the request. Modifies the <see cref="RequestMessage"/> directly.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <returns>
        /// The <see cref="RequestBuilder"/>.
        /// </returns>
        public RequestBuilder Method(HttpMethod method)
        {
            this.RequestMessage.Method = method;
            return this;
        }

        /// <summary>
        /// Sets the given resource URI.
        /// </summary>
        /// <param name="resourceUri">
        /// The resource URI.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder ResourceUri(string resourceUri)
        {
            this.resourceUri = resourceUri;
            return this;
        }

        /// <summary>
        ///     Modifies and returns the underlying <see cref="HttpRequestMessage" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="HttpRequestMessage" />.
        /// </returns>
        /// <exception cref="System.ArgumentException">No URI configured on the request.</exception>
        public HttpRequestMessage ToHttpRequestMessage()
        {
            // Fall back to message's uri.
            Uri uri = null;
            if (this.baseUri == null && this.resourceUri == null)
            {
                uri = this.message.RequestUri;
                if (uri == null)
                {
                    throw new ArgumentException("No URI configured on the request.");
                }
            }

            if (uri == null)
            {
                // If no base URL has been set, just use the Resource URI and assume it's complete.
                uri = this.baseUri == null ? new Uri(this.resourceUri) : new Uri(this.baseUri, this.resourceUri);
            }

            // Add URL segments.
            if (this.uriSegments.Any())
            {
                foreach (var uriSegment in this.uriSegments)
                {
                    var strUri = uri.ToString();
                    strUri = strUri.Replace(
                        string.Format("{{{0}}}", System.Uri.EscapeDataString(uriSegment.Key)),
                        System.Uri.EscapeDataString(uriSegment.Value.ToString()));
                    uri = new Uri(strUri);
                }
            }

            // Add query string.
            if (this.query.Any())
            {
                var queryStringBuilder = new StringBuilder();
                queryStringBuilder.Append('?');
                var queryList = this.query.ToList();
                for (int i = 0; i < queryList.Count; i++)
                {
                    var q = queryList[i];
                    queryStringBuilder.Append(System.Uri.EscapeDataString(q.Key));
                    queryStringBuilder.Append('=');
                    queryStringBuilder.Append(System.Uri.EscapeDataString(q.Value.ToString()));

                    // If this is not the last query string segment, add the delimiter.
                    if (i != queryList.Count - 1)
                    {
                        queryStringBuilder.Append('&');
                    }
                }

                var combined = UrlHelper.Combine(uri.ToString(), queryStringBuilder.ToString());
                uri = new Uri(combined, UriKind.RelativeOrAbsolute);
            }

            // Add authentication header value.
            if (this.authentication != null)
            {
                this.message.Headers.Authorization = this.authentication;
            }

            this.message.RequestUri = uri;
            return this.message;
        }

        #endregion
    }

    // ReSharper restore ParameterHidesMember
}