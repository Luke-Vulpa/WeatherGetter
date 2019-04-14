using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherGetter.Models.LocationModels;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Resources;
using WeatherGetter.Properties;

namespace WeatherGetter.Services
{
    public class LocationService
    {
        /// <summary>
        ///  Class is used to iterate over siteList and get sites in range of postcode. That data is then used to build UI.
        /// </summary>
        public enum DistanceType { Miles, Kilometers };
        
        public Location[] ValidLocations;

        public LocationService(Coordinate coordinate)
        {
            GetSitesInRange(coordinate);        
        }


        public void GetSitesInRange(Coordinate coordinate)
        {

            // get data from sitelist.json
            Locations locations = createModel();
            Location[] filtered = new Location[500];

            int counter = 0;

            //iterate over model, checking the distance
            for (int i = 0; i < locations.Location.Count - 1; i++)
            {
                if(Distance(coordinate, locations.Location[i].Latitude, locations.Location[i].Longitude, DistanceType.Miles) < 10)
                {
                    filtered[counter] = locations.Location[i];
                    counter++;
                }
                
            }

            //instanciate array trimming null values from filter.
            this.ValidLocations = filtered.Where(loc => loc != null).ToArray();

            // returned list is used for populating buttons to gain further location information

        }

        public Locations createModel()
        {
            try
            {

                var rootObject = JsonConvert.DeserializeObject<RootObject>(Resources.sitelist2);
                var tempLocations = rootObject.Locations;

                return tempLocations;

            }
            catch(JsonException ex)
            {
                ex.ToString();
            }
            //Locations locations = siteList.Locations;

            return null;

        }

        
        //hacky remove later
        public static double DistanceBetweenLocations(Coordinate location1, Coordinate location2)
        {
            //D = sqrt((Ax - Bx)2 + (Ay - By)2)

            double distance = Math.Sqrt(
                                        Math.Pow((location1.Longitude - location2.Longitude), 2) +
                                        Math.Pow((location1.Latitude - location2.Latitude), 2));

            return distance * 69;
            // 6.35466244751442
        }

        // is this needed anymore?
        public static double Distance(Coordinate pos1, Coordinate pos2, DistanceType type)
        {
            //https://gist.github.com/jammin77/033a332542aa24889452

            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = toRadian(pos2.Latitude - pos1.Latitude);
            double dLon = toRadian(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(pos1.Latitude)) * Math.Cos(toRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        //override to deal with location data
        public static double Distance(Coordinate pos1, double pos2lat, double pos2long, DistanceType type)
        {
            //https://gist.github.com/jammin77/033a332542aa24889452

            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = toRadian(pos2lat - pos1.Latitude);
            double dLon = toRadian(pos2long - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(pos1.Latitude)) * Math.Cos(toRadian(pos2lat)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }



        private static double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

    }
}
