using HttpMock;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jeffijoe.HttpClientGoodies.Tests
{
    public class JsonContentTests : HttpTest
    {
        [Fact]
        public async Task CanDeserializeCorrectly()
        {
            var mock = HttpMockRepository.At("http://localhost:9191");
            mock.Stub(x => x.Get("/data"))
                .Return("{ \"Name\": \"Jeff\" }")
                .OK();

            var message = new RequestBuilder()
                 .Uri("http://localhost:9191/data")
                 .Method(HttpMethod.Get)
                 .ToHttpRequestMessage();

            var response = await this.Send(message);
            var data = await response.Content.ReadAsJsonAsync<Data>();
            data.Name.ShouldBe("Jeff");
        }

        [Fact]
        public async Task CanSerializeCorrectly()
        {
            var content = new JsonContent(new Data { Name = "Jeff" });
            var data = await content.ReadAsJsonAsync<Data>();
            data.Name.ShouldBe("Jeff");
        }

        class Data
        {
            public string Name { get; set; }
        }
    }

    
}
