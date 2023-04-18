namespace FlightPlanner.Models
{
    public class FlightResponse
    {
        public FlightResponse(
            int id,
            AirportModel from,
            AirportModel to,
            string carrier,
            string departureTime,
            string arrivalTime
        )
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
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
