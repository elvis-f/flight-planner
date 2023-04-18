using System;
using System.Linq;

namespace FlightPlanner.Models
{
    public class AirportModel
    {
        public AirportModel(string country, string city, string airport) { 
            Country = country;
            City = city;
            Airport = airport;
        }

        public String Country { get; set; }
        public String City { get; set; }
        public String Airport { get; set; }

        public bool Equals(AirportModel other)
        {
            return Airport.ToLower() == other.Airport.ToLower();
        }

        public bool IsValid()
        {
            return !(String.IsNullOrEmpty(Country) 
                || String.IsNullOrEmpty(City)
                || String.IsNullOrEmpty(Airport));
        }

        public bool PartialMatch(string phrase)
        {
            var strings = this.GetType()
                .GetProperties()
                .Where(x => x.PropertyType == typeof(string))
                .Select(x => ((string)x.GetValue(this)).ToLower().Trim());

            Console.WriteLine($"{string.Join('|', strings)} : {phrase}");

            return strings
                .Any(x => x.Contains(phrase.ToLower().Trim()));
        }
    }
}
