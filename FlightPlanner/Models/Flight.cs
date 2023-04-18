using FlightPlanner.Storage;
using System;

namespace FlightPlanner.Models
{
    public class Flight
    {
        public Flight(int id, AirportModel from, AirportModel to, string carrier, DateTime departureTime, DateTime arrivalTime)
        {
            Id = id;
            From = from;
            To = to;
            Carrier = carrier;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }
        public int Id { get; set; }
        public AirportModel From { get; set; }
        public AirportModel To { get; set; }
        public string Carrier { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public bool Equals(Flight other)
        {
            // check airports
            var from = From.Equals(other.From);
            var to = To.Equals(other.To);

            // check carrier
            var carrier = Carrier.Equals(other.Carrier);

            // check times
            var departure = DateTime.Compare(DepartureTime, other.DepartureTime) == 0;
            var arrival = DateTime.Compare(ArrivalTime, other.ArrivalTime) == 0;

            // if all of them are true, then flights are equal
            return from && to && carrier && departure && arrival;
        }
    }
}
