// SkyClip
// - RequestBuilderTests.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;
using System.Linq;

using Shouldly;

using Xunit;
using Xunit.Abstractions;

namespace Jeffijoe.HttpClientGoodies.Tests
{
    /// <summary>
    ///     Tests for the <see cref="RequestBuilder" /> class.
    /// </summary>
    public class RequestBuilderTests
    {
        #region Fields

        /// <summary>
        ///     The output helper.
        /// </summary>
        private readonly ITestOutputHelper outputHelper;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBuilderTests"/> class.
        /// </summary>
        /// <param name="outputHelper">
        /// The output helper.
        /// </param>
        public RequestBuilderTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Determines whether this instance [can add headers].
        /// </summary>
        [Fact]
        public void CanAddHeaders()
        {
            var builder = new RequestBuilder()
                .ResourceUri("http://google.com/test")
                .AddHeader("Authorization", "Bearer stuff");

            var message = builder.ToHttpRequestMessage();
            message.Headers.Any(x => x.Key == "Authorization").ShouldBe(true);
        }

        /// <summary>
        ///     The content is set properly.
        /// </summary>
        [Fact]
        public void ContentIsSetProperly()
        {
            var builder = new RequestBuilder()
                .ResourceUri("http://google.com/test")
                .JsonContent(new { hello = "world" });

            var message = builder.ToHttpRequestMessage();
            message.Content.ShouldNotBe(null);
            message.Content.ShouldBeOfType<JsonContent>();
        }

        /// <summary>
        ///     Verify that the builder modifies the URL correctly.
        /// </summary>
        [Fact]
        public void ModifiesUriCorrectly()
        {
            var builder = new RequestBuilder()
                .AddUriSegment("id", "hello")
                .BaseUri(new Uri("http://google.com"))
                .ResourceUri("api/{id}/world")
                .AddQuery("count", 1337)
                .AddQuery("q", "crack is wack & hello there");

            var message = builder.ToHttpRequestMessage();
            var uri = message.RequestUri.ToString();

            uri.ShouldBe("http://google.com/api/hello/world/?count=1337&q=crack is wack %26 hello there");

            this.outputHelper.WriteLine(uri);
        }

        /// <summary>
        ///     Verify that the builder adds the correct auth header.
        /// </summary>
        [Fact]
        public void BasicAuthSetsAuthHeader()
        {
            var req = new RequestBuilder()
                .Uri("http://google.com")
                .BasicAuthentication("test", "test")
                .ToHttpRequestMessage();

            req.RequestUri.ShouldBe(new Uri("http://google.com"));
            req.Headers.Authorization.Parameter.ShouldBe("dGVzdDp0ZXN0");
        }

        #endregion
    }
}