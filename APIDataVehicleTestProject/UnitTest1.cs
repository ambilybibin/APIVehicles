using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;
using Vehicles.Api.Repositories;
namespace APIDataVehicleTestProject
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UnitTest1()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Fact]
        public async void TestGetAllVehicles()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("Vehicles/Cars");
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<List<Vehicle>>(contentString);

            Assert.NotNull(cars);
            Assert.NotEmpty(cars);
        }

        [Fact]
        public async Task GetCarsByMileageRange_ReturnsFilteredCars_WhenValidMileageRangeIsProvided()
        {
            var client = _factory.CreateClient();
            var minMileage = 50000;
            var maxMileage = 160000;
            var requestUri = $"Vehicles/Search/Mileage?minMileage={minMileage}&maxMileage={maxMileage}";

            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("FIAT", responseBody);
            Assert.Contains("BMW", responseBody);
        }

        [Fact]
        public async Task GetCarsByMileageRange_ReturnsBadRequest_WhenMinMileageIsGreaterThanMaxMileage()
        {
            var client = _factory.CreateClient();
            var minMileage = 50000;
            var maxMileage = 3000;
            var requestUri = $"Vehicles/Search/Mileage?minMileage={minMileage}&maxMileage={maxMileage}";

            var response = await client.GetAsync(requestUri);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetCarsByMileageRange_ReturnsBadRequest_WhenMileageRangeIsMissing()
        {
            var client = _factory.CreateClient();
            var requestUri = "Vehicles/Search/Mileage";

            var response = await client.GetAsync(requestUri);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task GetAllCarsListByModel_ReturnsSuccessWithCorrectData()
        {
            var client = _factory.CreateClient();
            var requestUri = "/Vehicles/Cars/Make?make=Toyota";
            var response = await client.GetAsync(requestUri);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentString = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<List<Vehicle>>(contentString);
            Assert.NotNull(cars);
            Assert.All(cars, car => Assert.Equal("Toyota", car.Make, ignoreCase: true));
        }

        [Fact]
        public async Task GetAllCarsListByModel_ShouldReturnCorrectCars_IgnoringCase()
        {
            var client = _factory.CreateClient();
            var requestUri = "/Vehicles/Cars/Make?make=toyota";

            var response = await client.GetAsync(requestUri);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentString = await response.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<List<Vehicle>>(contentString);
            Assert.NotNull(cars);
            Assert.All(cars, car => Assert.Equal("Toyota", car.Make, ignoreCase: true));
        }

        [Fact]
        public async Task GetAllCarsListByModel_ShouldReturnAllCars_WhenNoModelProvided()
        {
            var client = _factory.CreateClient();
            var requestUri = "/Vehicles/Cars/Model";

            var response = await client.GetAsync(requestUri);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

