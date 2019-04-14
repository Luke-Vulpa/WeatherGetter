using NUnit.Framework;
using WeatherGetter.Services;

namespace Tests
{
    public class LocationServiceTests
    {



        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DistanceBetweenLocationsTest()
        {
            var sutAirport = new Coordinate(50.7354, -3.4201);
            var sutHome = new Coordinate(50.731525, -3.512115);
            /*
            "latitude": "50.7354",
            "longitude": "-3.4201",
            "name": "Exeter International Airport",  

            "latitude": "50.731525",
            "longitude": "-3.512115",
            "name": " home ",

            */

            //Arrange



            double result = LocationService.DistanceBetweenLocations(sutAirport, sutHome);
            Assert.Pass(result.ToString());
        }

        [Test]
        public void DistanceTest()
        {
            var sutAirport = new Coordinate(50.7354, -3.4201);
            var sutHome = new Coordinate(50.731525, -3.512115);

            var result = LocationService.Distance(sutAirport, sutHome, LocationService.DistanceType.Miles);
            Assert.Pass(result.ToString());


        }

        [Test]
        public void IterateOverSiteList()
        {
            var file = WeatherGetter.Properties.Resources.sitelist;
            var sut = new LocationService();
            sut.createModel();
            
        }
    }
}