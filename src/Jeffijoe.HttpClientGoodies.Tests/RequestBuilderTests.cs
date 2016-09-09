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
using HttpMock;
using System.Threading.Tasks;
using System.Net.Http;

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
            
        /// <summary>
        /// Verifies that we can send a request without passing in a client.
        /// </summary>
        [Fact]
        public async Task CanSendWithNoClientPassed()
        {
            var mock = HttpMockRepository.At("http://localhost:9191");
            mock.Stub(x => x.Get("/data"))
                .Return("{ \"Name\": \"Jeff\" }")
                .OK();

            var resp = await new RequestBuilder()
                .Uri("http://localhost:9191/data")
                .SendAsync();

            Data data = await resp.Content.ReadAsJsonAsync<Data>();
            data.Name.ShouldBe("Jeff");
        }

        /// <summary>
        /// Verifies that we can send a request when passing in a client.
        /// </summary>
        [Fact]
        public async Task CanSendWithClientPassed()
        {
            var mock = HttpMockRepository.At("http://localhost:9191");
            mock.Stub(x => x.Get("/data"))
                .Return("{ \"Name\": \"Jeff\" }")
                .OK();

            using (var client = new HttpClient())
            {
                Data data = await new RequestBuilder()
                    .Uri("http://localhost:9191/data")
                    .SendAsync(client)
                    .AsJson<Data>();

                data.Name.ShouldBe("Jeff");
            }    
        }

        [Fact]
        public void StaticMethodHelpers()
        {
            const string expected = "http://localhost:9191/data";
            var builder = RequestBuilder.Get(expected);
            var msg = builder.ToHttpRequestMessage();
            msg.Method.ShouldBe(HttpMethod.Get);
            msg.RequestUri.ShouldBe(new Uri(expected));
        }

        private class Data
        {
            public string Name { get; set; }
        }

        #endregion
    }
}