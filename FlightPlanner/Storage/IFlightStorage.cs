using FlightPlanner.Models;
using System;
using System.Collections.Generic;

namespace FlightPlanner.Storage
{
    public interface IFlightStorage
    {
        public Flight GetFlight(int id);
        public bool AddFlight(Flight flight);
        public void ClearFlights();
        public void DeleteFlight(int id);
        public int GetUniqueId();
        public bool CheckDuplicates(Flight flight);
        public bool VerifyFlight(Flight flight);
        public AirportModel[] MatchAirports(string phrase);
        public List<Flight> SearchFlightsByOrigin(string origin);
        public List<Flight> SearchFlightsByDestination(string destination);
        public List<Flight> SearchFlightsByDate(DateTime date);
    }
}
