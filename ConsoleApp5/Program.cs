using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp5
{
    public class City
    {
        public City()
        {

        }
        public City(string Name, int Code)
        {
            this.Name = Name;
            this.Code = Code;
        }

        public string Name { get; set; }
        public int Code { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", Code, Name);
        }
    }

    class WeatherCity
    {
        public WeatherCity()
        {

        }

        public WeatherCity(XmlNode xmlNode)
        {
            //Type wt = typeof(WeatherCity);

            //foreach (PropertyInfo property in wt.GetProperties())
            //{
            //    property.Name
            //}
            Title = xmlNode.SelectSingleNode("title").InnerText;
            Description = xmlNode.SelectSingleNode("description").InnerText;
        }

        public string Title { get; set; }
        public string Description { get; set; }   
        
        public string GetInfo()
        {
           return string.Format($"{Title} \n{Description}\n\n");            
        }
    }

    public static class WeatherSettings
    {
        private static List<WeatherCity> list = new List<WeatherCity>();

        public static void GetWeatherCity(int code)
        {
            string link = string.Format("http://informer.gismeteo.by/rss/{0}.xml", code);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(link);

            foreach (XmlNode item in xmlDocument.SelectNodes("/rss/channel/item"))
                list.Add(new WeatherCity(item));            
        }

        public static void ShowWeatherCity()
        {
            foreach (var item in list)
                Console.WriteLine(item.GetInfo());
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            List<City> cities = new List<City>();
            string root = "cities.xml";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(File.Open(root, FileMode.Open));


            foreach (XmlNode item in xmlDocument.DocumentElement.ChildNodes)
            {
                City city = new City();
                foreach (XmlNode element in item.ChildNodes)
                {
                    if (element.Name == "name")
                        city.Name = element.InnerText;

                    if (element.Name == "code")
                        city.Code = int.Parse(element.InnerText);
                }
                cities.Add(city);
            }

            foreach (var city in cities)
            {
                Console.WriteLine(city);
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Введите код города: ");
            int newCode = int.Parse(Console.ReadLine());

            Console.WriteLine();
            WeatherSettings.GetWeatherCity(newCode);
            WeatherSettings.ShowWeatherCity();
        }
    }
}
