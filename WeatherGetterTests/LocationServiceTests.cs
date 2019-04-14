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
        public void Distance_LessThan10()
        {
            var sutAirport = new Coordinate(50.7354, -3.4201);
            var sutHome = new Coordinate(50.731525, -3.512115);

            var result = LocationService.Distance(sutAirport, sutHome.Latitude, sutHome.Longitude, LocationService.DistanceType.Miles);
            //Assert.Pass(result < 10);

            Assert.LessOrEqual(result, 10);


        }

 
    }
}