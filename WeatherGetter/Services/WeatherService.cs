using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WeatherGetter.Models.WeatherServiceModels;

namespace WeatherGetter.Services
{
    class WeatherService
    {
        private string _id;
        HttpResponseMessage _response;
        public SiteRep SiteRep;

        public WeatherService(string Id)
        {
            this._id = Id;
            SiteRep = new SiteRep();


        }

       public async Task<SiteRep> getWeatherModelDataAsync()
       {

            using (MainWindow.Client = new HttpClient())
            {
                try
                {
                    MainWindow.Client.BaseAddress = new Uri("http://datapoint.metoffice.gov.uk/public/data/val/wxfcs/all/json/" + this._id + "?res=daily&key=f34c0500-6d46-4a3d-a1cd-0e156caec504");
                    MainWindow.Client.DefaultRequestHeaders.Accept.Clear();
                    MainWindow.Client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    _response = await MainWindow.Client.GetAsync(MainWindow.Client.BaseAddress);

                    if (_response.IsSuccessStatusCode)
                    {
                        
                        var _result = await _response.Content.ReadAsStringAsync();


                        RootObject siteTemp = JsonConvert.DeserializeObject<RootObject>(_result);

                        siteTemp.SiteRep.Dv.Location.Period[0].Rep[0].W = 
                            decodeWeatherType(siteTemp.SiteRep.Dv.Location.Period[0].Rep[0].W);
                        siteTemp.SiteRep.Dv.Location.Period[0].Rep[1].W = 
                            decodeWeatherType(siteTemp.SiteRep.Dv.Location.Period[0].Rep[1].W);


                        this.SiteRep = siteTemp.SiteRep;
                        
                        return SiteRep;                       

                        

                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                return null;
            }
       }

       private string decodeWeatherType(string weatherType)
       {
            
            switch (weatherType)
            {
                case "NA":
                    return "Not available";
                case "0":
                    return "Clear night";
                case "1":
                    return "Sunny day";
                case "2":
                    return "Partly cloudy(night)";
                case "3":
                    return "Partly cloudy(day)";
                case "4":
                    return "Not used";
                case "5":
                    return "Mist";
                case "6":
                    return "Fog";
                case "7":
                    return "Cloudy";
                case "8":
                    return "Overcast";
                case "9":
                    return "Light rain shower(night)";
                case "10":
                    return "Light rain shower(day)";
                case "11":
                    return "Drizzle";
                case "12":
                    return "Light rain";
                case "13":
                    return "Heavy rain shower(night)";
                case "14":
                    return "Heavy rain shower(day)";
                case "15":
                    return "Heavy rain";
                case "16":
                    return "Sleet shower(night)";
                case "17":
                    return "Sleet shower(day)";
                case "18":
                    return "Sleet";
                case "19":
                    return "Hail shower(night)";
                case "20":
                    return "Hail shower(day)";
                case "21":
                    return "Hail";
                case "22":
                    return "Light snow shower(night)";
                case "23":
                    return "Light snow shower(day)";
                case "24":
                    return "Light snow";
                case "25":
                    return "Heavy snow shower(night)";
                case "26":
                    return "Heavy snow shower(day)";
                case "27":
                    return "Heavy snow";
                case "28":
                    return "Thunder shower(night)";
                case "29":
                    return "Thunder shower(day)";
                case "30":
                    return "Thunder";
            }

            return "NA";

       }





    }
}
