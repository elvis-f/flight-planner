using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlightPlanner.Storage;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private readonly IFlightStorage _flightStorage;
        private string customFormat = "yyyy-MM-dd HH:mm";

        public AdminApiController(IFlightStorage flightStorage)
        {
            _flightStorage = flightStorage;
        }

        [HttpGet("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightStorage.GetFlight(id);

            return flight == null ? new NotFoundObjectResult("Requested flight couldn't be found! :(") : new OkObjectResult(flight.Carrier);
        }

        [HttpPut("flights")]
        public IActionResult PutFlight(FlightRequest request)
        {
            var departureResult = DateTime.TryParse(request.DepartureTime, out var departureTime);
            var arrivalResult = DateTime.TryParse(request.ArrivalTime, out var arrivalTime);

            if(departureResult == false
                || arrivalResult == false
                || request == null
                || request.From == null
                || request.To == null
                || request.Carrier == null)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            var flightId = _flightStorage.GetUniqueId();

            var flight = new Flight(
                flightId,
                request.From,
                request.To,
                request.Carrier,
                departureTime,
                arrivalTime
            );

            var isFlightValid = _flightStorage.VerifyFlight(flight);

            if (!isFlightValid)
            {
                return new BadRequestObjectResult("Bad request!");
            }

            var isFlightUnique = !_flightStorage.CheckDuplicates(flight);

            if (!isFlightUnique)
            {
                return Conflict("This flight already exists!");
            }

            _flightStorage.AddFlight(flight);

            var flightResponse = new FlightResponse(
                flight.Id,
                flight.From,
                flight.To,
                flight.Carrier,
                flight.DepartureTime.ToString(customFormat),
                flight.ArrivalTime.ToString(customFormat)
            );

            //Console.WriteLine($"From: {request.From.Airport}, To: {request.To.Airport}, Carrier: {request.Carrier}, Times: {request.DepartureTime}-{request.ArrivalTime}");
            //Console.WriteLine("Created a new flight!");
            return Created($"/flights/{flightId}", flightResponse);
        }

        [HttpDelete("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            _flightStorage.DeleteFlight(id);

            return new OkObjectResult("Removed flight (if it exists!)");
        }
    }
}
