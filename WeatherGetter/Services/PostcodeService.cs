using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherGetter.Models.PostcodeModels;


namespace WeatherGetter.Services
{
    class PostcodeService
    {
        string _result;
        string _postcode;

        HttpResponseMessage _response;        
        RootObject rootObject;
         

        public PostcodeService(string postcode)
        {
            this._postcode = postcode;
            rootObject = new RootObject();
        }

        public async Task<Coordinate> GetLongLatAsync()
        {
            
            //MainWindow.Client
            using (MainWindow.Client = new HttpClient())
            {
                MainWindow.Client.BaseAddress = new Uri("https://api.postcodes.io/postcodes/" + this._postcode);
                MainWindow.Client.DefaultRequestHeaders.Accept.Clear();
                MainWindow.Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));  


                try
                {
                    _response = await MainWindow.Client.GetAsync(MainWindow.Client.BaseAddress);
                    if(_response.IsSuccessStatusCode)
                    {
                        //cast result into model and then set long/lat properties which can then be used in the UI
                        _result = await _response.Content.ReadAsStringAsync();                     
         
                        rootObject = JsonConvert.DeserializeObject<RootObject>(_result);


                        var coordinate = new Coordinate(
                                Double.Parse(rootObject.result.longitude.ToString()),
                                Double.Parse(rootObject.result.latitude.ToString()));

                        return coordinate;

                    }                                      

                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
            }


            return null;



        }
    }
}
