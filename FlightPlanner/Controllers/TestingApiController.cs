using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingApiController : ControllerBase
    {
        private IFlightStorage _flightStorage;

        public TestingApiController(IFlightStorage flightStorage)
        {
            _flightStorage = flightStorage;
        }

        [HttpPost("clear")]
        public IActionResult Clear()
        {
            _flightStorage.ClearFlights();

            return new OkObjectResult("Flights cleared!");
        }
    }
}
