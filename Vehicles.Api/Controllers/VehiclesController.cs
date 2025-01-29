using Microsoft.AspNetCore.Mvc;
using Vehicles.Api.Repositories;

namespace Vehicles.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly ILogger<VehiclesController> _logger;
        private readonly IVehiclesRepository _repo;

        public VehiclesController(ILogger<VehiclesController> logger, IVehiclesRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        /// <summary>
        /// Get list of all cars available
        /// </summary>

        [HttpGet("Cars")]
        public async Task<IActionResult> GetAllCarsList()
        {
            _logger.LogInformation("Received request for /Vehicles/Cars endpoint");

            try
            {
                var cars = await _repo.GetAllCarsList(); //get all cars from the json
                if (cars == null || !cars.Any())  // No cars are found
                {
                    return NotFound("No cars found.");
                }

                return new JsonResult(cars);  // return list of cars
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching the cars list: {ex.Message}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        /// <summary>
        /// Get list of all cars available by make
        /// <param name="make"></param>
        /// </summary>
        [HttpGet("Cars/Make")]
        public async Task<IActionResult> GetAllCarsListByMake([FromQuery] string make)
        {
            try
            {
                var cars = await _repo.GetAllCarsList();//get all cars from the json
                if (!string.IsNullOrEmpty(make))
                {
                    cars = cars.Where(car => car.Make.Equals(make, StringComparison.OrdinalIgnoreCase)).ToList();//get all cars for the make
                }
                if (cars == null || !cars.Any())
                {
                    return NotFound("No cars found for this make.");
                }

                return new JsonResult(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching cars by make: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Get list of all cars available by model
        /// <param name="model"></param>
        /// </summary>
        [HttpGet("Cars/Model")]
        public async Task<IActionResult> GetAllCarsListByModel([FromQuery] string model)
        {
            try
            {
                if (string.IsNullOrEmpty(model))
                {
                    return BadRequest("Model is required.");
                }

                var cars = await _repo.GetAllCarsList();
                if (!string.IsNullOrEmpty(model))
                {
                    cars = cars.Where(car => car.Make.Equals(model, StringComparison.OrdinalIgnoreCase)).ToList();//get all cars for the model
                }
                if (cars == null || !cars.Any())
                {
                    return NotFound("No cars found for this model.");
                }

                return new JsonResult(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching cars by model: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Get list of all cars available between a range of mileage
        /// <param name="minMileage"></param>
        /// <param name="maxMileage"></param>
        /// </summary>
        [HttpGet("Search/Mileage")]
        public async Task<IActionResult> GetCarsByMileageRange([FromQuery] int? minMileage, [FromQuery] int? maxMileage)
        {
            if (!minMileage.HasValue || !maxMileage.HasValue)
            {
                return BadRequest("Both minMileage and maxMileage query parameters are required.");
            }

            if (minMileage > maxMileage)
            {
                return BadRequest("minMileage cannot be greater than maxMileage.");
            }

            try
            {
                var cars = await _repo.GetAllCarsList();
                var filteredCars = cars.Where(car => car.mileage >= minMileage && car.mileage <= maxMileage).ToList();//get all cars within the min and max mileage provided
                if (filteredCars == null || !filteredCars.Any())
                {
                    return NotFound("No cars found in this mileage range.");
                }
                return new JsonResult(filteredCars);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching cars by mileage range: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }


    }
}