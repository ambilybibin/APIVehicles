using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Vehicles.Api.Repositories
{

    public class Vehicle
    {
        [JsonPropertyName("make")]
        public string Make { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("trim")]
        public string Trim { get; set; }

        [JsonPropertyName("colour")]
        public string Colour { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("co2_level")]
        public int CO2_Level { get; set; }

        [JsonPropertyName("transmission")]
        public string Transmission { get; set; }

        [JsonPropertyName("fuel_type")]
        public string FuelType { get; set; }

        [JsonPropertyName("engine_size")]
        public int Enginesize { get; set; }

        [JsonPropertyName("date_first_reg")]
        public string DateFirstReg { get; set; }

        [JsonPropertyName("mileage")]
        public int mileage { get; set; }
    }

    public class VehicleRepository : IVehiclesRepository
    {
        private readonly string _filePath = "Repositories/vehicles.json";

        public async Task<IEnumerable<Vehicle>> GetAllCarsList()
        {
            try
            {
                using (StreamReader r = new StreamReader(_filePath))
                {
                    string json = await r.ReadToEndAsync();
                    return JsonSerializer.Deserialize<List<Vehicle>>(json) ?? new List<Vehicle>();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading vehicles data: {ex.Message}");
                return new List<Vehicle>(); 
            }
        }
    }

}
