using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly IFlightStorage _flightStorage;
        private string customFormat = "yyyy-MM-dd HH:mm";

        public CustomerApiController(IFlightStorage flightStorage)
        {
            _flightStorage = flightStorage;
        }

        [HttpGet("airports")]
        public IActionResult SearchAirports(string search)
        {
            var resultList = _flightStorage.MatchAirports(search);

            if (resultList.Length > 0)
            {
                return new OkObjectResult(resultList);
            }else
            {
                return new NotFoundObjectResult("Sorry no results!");
            }
        }

        [HttpPost("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            Console.WriteLine($"{request.From} -> {request.To} | {request.DepartureDate}");

            if(String.IsNullOrEmpty(request.From)
                || String.IsNullOrEmpty(request.To)
                || String.IsNullOrEmpty(request.DepartureDate)
                || request.From.ToLower().Trim() == request.To.ToLower().Trim())
            {
                return new BadRequestObjectResult("Bad request!");
            }

            var flightsByOrigin = _flightStorage.SearchFlightsByOrigin(request.From);
            var flightsByDestination = _flightStorage.SearchFlightsByDestination(request.To);
            var flightsByDate = _flightStorage.SearchFlightsByDate(DateTime.Parse(request.DepartureDate));

            var results = flightsByOrigin.Intersect(flightsByDestination).Intersect(flightsByDate).ToList();

            return new OkObjectResult(new PageResult
            {
                Page = 0,
                TotalItems = results.Count,
                Items = results.ToArray(),
            });
        }

        [HttpGet("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _flightStorage.GetFlight(id);

            if(flight != null)
            {
                var flightResult = new FlightResponse
                (
                    flight.Id,
                    flight.From,
                    flight.To,
                    flight.Carrier,
                    flight.DepartureTime.ToString(customFormat),
                    flight.ArrivalTime.ToString(customFormat)
                );

                return new OkObjectResult(flightResult);
            }
            else
            {
                return new NotFoundObjectResult("Not found: (");
            }
        }
    }
}
