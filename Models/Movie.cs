using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MVC_Lab3.Models
{
    public class Movie
    {

        //[Key]
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
        public string Synopsis { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }

        /// <summary>
        /// For each movie passed into the method, 
        /// the movie's XMLattributes are transformed and assigned to actual movie object values.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>Movie Object</returns>
        public Movie XmlToMovie(XElement movie)
        {
            Movie mov = new Movie();
            mov.Title = movie.Element("Title").Value;
            mov.OriginalTitle = movie.Element("OriginalTitle").Value;
            mov.ReleaseYear = (int)movie.Element("ReleaseYear");
            mov.Rating = double.Parse(movie.Element("Rating").Value.Replace(',', '.'));
            mov.Synopsis = movie.Element("Synopsis").Value;
            mov.Genre = string.Join(", ", movie.Element("Genres").Elements("Genre").Select(x => x.Value).ToArray());
            mov.Actors = string.Join(",", movie.Element("Actors").Elements("Actor").Select(x => x.Value).ToArray());
            return mov;

        }
    }
}