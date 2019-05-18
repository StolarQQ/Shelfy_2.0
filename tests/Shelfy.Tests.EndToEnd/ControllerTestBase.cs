using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shelfy.API;
using Xunit;

namespace Shelfy.Tests.EndToEnd
{
    public abstract class ControllerTestsBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected HttpClient Client { get; }

        protected ControllerTestsBase(WebApplicationFactory<Startup> fixture)
        {
            Client = fixture.CreateClient();
            Client.BaseAddress = new Uri("https://localhost");
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}