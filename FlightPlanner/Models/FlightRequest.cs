namespace FlightPlanner.Models
{
    public class FlightRequest
    {
        public AirportModel From { get; set; }
        public AirportModel To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
