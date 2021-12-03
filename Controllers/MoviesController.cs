using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Lab3.Context;
using MVC_Lab3.Models;
using PagedList;

namespace MVC_Lab3.Controllers
{
    public class MoviesController : Controller
    {
        private MovieContext db = new MovieContext();
       // Lab3ServiceReference.Lab3Client service = new Lab3ServiceReference.Lab3Client();

        // GET: Movies
        /// <summary>
        /// Fetches all movies, ordering them by raiting and presenting them on the website with pagening.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {
            var movies = db.Movies.AsQueryable();
            movies = movies.OrderByDescending(m => m.Rating);
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            return View(movies.ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// Queries the movie DB with the use input, which is either title, original tile, release year or genres
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Search(string search, int? page)
        {
            var movies = db.Movies;
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            //if (search == "Top Ten".ToLower().Trim())
            //{
            //    var FiltreradeTop10 = db.
            //}
            //else
            //{
                var FiltreradeFilmer = from m in movies
                                       where m.Title == search || m.OriginalTitle == search || m.ReleaseYear.ToString() == search || m.Genre == search
                                       select m;
                // IPagedList<Movie> toPagedList = movies.ToPagedList(pageNumber ?? 1, 5); // FiltreradeFilmer.ToPagedList(pageNumber ?? 1, 5);
                FiltreradeFilmer = FiltreradeFilmer.OrderBy(m => m.Title);
                return View("Index", FiltreradeFilmer.ToPagedList(pageNumber, pageSize));

        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,OriginalTitle,ReleaseYear,Rating,Synopsis,Genre,Actors")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,Title,OriginalTitle,ReleaseYear,Rating,Synopsis,Genre,Actors")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
