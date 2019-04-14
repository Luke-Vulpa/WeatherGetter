using System;

namespace WeatherGetter.Models.WeatherServiceModels
{
    public class Param
    {
        public string name { get; set; }
        public string units { get; set; }

    }

    public class Wx
    {
        public Param[] Param { get; set; }
    }

    public class Rep
    {
        public string D { get; set; }
        public string Gn { get; set; }
        public string Hn { get; set; }
        public string PPd { get; set; }
        public string S { get; set; }
        public string V { get; set; }
        public string Dm { get; set; }
        public string FDm { get; set; }
        public string W { get; set; }
        public string U { get; set; }
        public string Gm { get; set; }
        public string Hm { get; set; }
        public string PPn { get; set; }
        public string Nm { get; set; }
        public string FNm { get; set; }
    }

    public class Period
    {
        public string type { get; set; }
        public string value { get; set; }
        public Rep[] Rep { get; set; }
    }

    public class Location
    {
        public string i { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string continent { get; set; }
        public string elevation { get; set; }
        public Period[] Period { get; set; }
    }

    public class Dv
    {
        public DateTime DataDate { get; set; }
        public string type { get; set; }
        public Location Location { get; set; }
    }

    public class SiteRep
    {
        public Wx Wx { get; set; }
        public Dv Dv { get; set; }
    }

    public class RootObject
    {
        public SiteRep SiteRep { get; set; }
    }
}
