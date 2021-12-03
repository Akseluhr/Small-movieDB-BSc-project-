using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab3Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Lab3 : ILab3
    {
        static List<JObject> _movies;
        static List<JObject> _actors;
        static List<JObject> _genres;

        public Lab3()
        {
            /// API call and json-convertion
            using (WebClient webClient = new WebClient())
            {
                string jsonMovies = webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vbW92aWVzLmpzb24=")));
                _movies = JsonConvert.DeserializeObject<List<JObject>>(jsonMovies);
                string jsonActors = webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vYWN0b3JzLmpzb24=")));
                _actors = JsonConvert.DeserializeObject<List<JObject>>(jsonActors);
                string jsonGenres = webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vZ2VucmUuanNvbg==")));
                _genres = JsonConvert.DeserializeObject<List<JObject>>(jsonGenres);

            }
        }

        /// <summary>
        /// Creates an XML structure of all Movies with sub-method
        /// </summary>
        /// <returns>XML structure of all movies</returns>
        public XElement GetAllMovies()
        {
            XElement allMovies = new XElement("Movies");

            foreach (var movie in _movies)
            {
                MovieToXml(movie, allMovies);
            }
            return allMovies;
        }

        /// <summary>
        /// Fetches top 10 movies based on raiting and creates XML-structure of it
        /// </summary>
        /// <returns>XML stucture of top 10 movies</returns>
        public XElement GetTopTenMovies()
        {
            XElement topMovies = new XElement("Movies");

            var topTenMovies = (from m in _movies
                                 orderby m["Rating"] descending
                                 select m).Take(10);

            foreach (JObject movie in topTenMovies)
            {
                MovieToXml(movie, topMovies);
            }

            return topMovies;

        }

        /// <summary>
        /// Creates XML-structure of a single movie.
        /// Substitutes actorID and genreID with actual actor name(s) and actual genres
        /// All elements are added to the XML structure.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="mov"></param>
        public void MovieToXml(JObject movie, XElement mov)
        {
           // string tempActor = "";
            XElement aMovie = new XElement("Movie");

            foreach (var m in movie)
            {
                if(m.Key != "Actors" && m.Key != "Genre")
                {
                    XElement element = new XElement(m.Key, m.Value.ToString());
                    aMovie.Add(element);
                }
                else if (m.Key == "Actors")
                {
                    XElement actors = new XElement("Actors");

                    foreach (var aID in m.Value)
                    {
                        foreach (var a in _actors)
                        {
                            if (a["ID"].Value<string>() == aID.Value<string>())
                            {
                                //   tempActor = (string)a["Name"];
                                XElement element = new XElement("Actor", (string)a["Name"]);
                                actors.Add(element);
                                // movie["Actors"] += tempActor + ", ";
                                // tempActor = "";
                            }
                        }
                    }
                    aMovie.Add(actors);
                }
                else if (m.Key == "Genre")
                {
                    XElement genre = new XElement("Genres");

                    foreach (var gID in m.Value)
                    {
                        foreach (var g in _genres)
                        {
                            if (g["ID"].Value<string>() == gID.Value<string>())
                            {
                                XElement element = new XElement("Genre", (string)g["Name"]);
                                genre.Add(element);
                            }
                        }
                    }
                    aMovie.Add(genre);
                }
            }
            mov.Add(aMovie);

        }

        public XElement MoviesToXml()
        {
            throw new NotImplementedException();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
