using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private FilmsEntities db = new FilmsEntities();

        // GET: Films
        public ActionResult Index()
        {
            var films = db.Films.OrderByDescending(f => f.Id).Take(3).ToList();
            return View(films);
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
