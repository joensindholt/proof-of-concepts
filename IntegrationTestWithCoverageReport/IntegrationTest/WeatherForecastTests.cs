using Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public WeatherForecastTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanGetWeatherForecasts()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/WeatherForecast");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var forecasts = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
            forecasts.Should().HaveCount(5);
        }
    }
}
