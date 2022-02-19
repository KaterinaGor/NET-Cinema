using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;
using PagedList.Mvc;
using PagedList;

namespace Cinema.Controllers
{
    public class FilmsPagesController : Controller
    {
        private FilmsEntities db = new FilmsEntities();
        
        // GET: FilmsPages
        public ActionResult Index(int? page)
          
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            var films = db.Films.OrderByDescending(f => f.Id).ToList();
            return View(films.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: FilmsPages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        [HttpPost]
        public ActionResult FilmSearch(string FilmCountry)
        {
            var allFilms = db.Films.Where(b => b.Country.Contains(FilmCountry)).OrderByDescending(t => t.Year).ToList();
            if (allFilms.Count <= 0)
            {
                return RedirectToAction("Index", "FilmsPages");
            }
            return View(allFilms);
        }

        public FileContentResult GetImage(int id)
        {
            Film film = db.Films.FirstOrDefault(a => a.Id == id);
            if (film != null)
            {
                return File(film.Cover, film.ImageMimeType);
            }
            return null;
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
