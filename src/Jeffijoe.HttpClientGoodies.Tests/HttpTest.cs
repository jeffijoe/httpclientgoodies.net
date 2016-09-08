using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jeffijoe.HttpClientGoodies.Tests
{
    public class HttpTest
    {
        internal async Task<HttpResponseMessage> Send(HttpRequestMessage message)
        {
            using (var client = new HttpClient())
            {
                return await client.SendAsync(message);
            }
        }
    }
}
