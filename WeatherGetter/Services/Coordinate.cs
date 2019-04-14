namespace WeatherGetter.Services
{
    public class Coordinate
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }



        public Coordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }   

    }
}
