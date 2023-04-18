//#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public class FlightStorage : IFlightStorage
    {
        private List<Flight> _flights;
        private int _lastId;
        private readonly object _lock = new object();
        
        public FlightStorage() { 
            _flights = new List<Flight>();
            _lastId = 0;
        }

        public Flight? GetFlight(int id)
        {
            return _flights.SingleOrDefault(x => x.Id == id);
        }

        public bool AddFlight(Flight flight)
        {
            lock (_flights)
            {
                if (VerifyFlight(flight) == true)
                {
                    _flights.Add(flight);
                    return true;
                }

                return false;
            }
        }

        public void ClearFlights()
        {
            _flights.Clear();
        }

        public int GetUniqueId()
        {
            lock (_lock)
            {
                _lastId += 1;
                return _lastId;
            }
        }

        public bool CheckDuplicates(Flight flight)
        {
            lock (_flights)
            {
                return _flights.Any(f => f.Equals(flight));
            }
        }

        public void DeleteFlight(int id)
        {
            lock (_flights)
            {
                _flights.Remove(GetFlight(id));
            }
        }

        public AirportModel[] MatchAirports(string phrase)
        {
            var airportList = new List<AirportModel>();

            airportList = airportList.Concat(_flights.Select(x => x.From)).ToList();
            airportList = airportList.Concat(_flights.Select(x => x.To)).ToList();

            return airportList.Where(x => x.PartialMatch(phrase)).ToArray();
        }

        public List<Flight> SearchFlightsByOrigin(string origin)
        {
            return _flights.Where(x => x.From.PartialMatch(origin)).ToList();
        }

        public List<Flight> SearchFlightsByDestination(string destination)
        {
            return _flights.Where(x => x.To.PartialMatch(destination)).ToList();
        }

        public List<Flight> SearchFlightsByDate(DateTime date)
        {
            return _flights.Where(x => x.DepartureTime.Date == date.Date).ToList();
        }

        public bool VerifyFlight(Flight flight)
        {
            if(    flight == null
                || flight.From == null
                || flight.To == null
                || !flight.From.IsValid()
                || !flight.To.IsValid()
                || String.IsNullOrEmpty(flight.Carrier))
            {
                return false;
            }

            if (CompareAirportCodes(flight.From.Airport, flight.To.Airport))
            {
                return false;
            }

            if (DateTime.Compare(flight.DepartureTime, flight.ArrivalTime) >= 0)
            {
                return false;
            }

            return true;
        }

        private bool CompareAirportCodes(string code1, string code2)
        {
            code1 = code1.ToLower().Trim();
            code2 = code2.ToLower().Trim();

            return code1 == code2;
        }
    }
}
