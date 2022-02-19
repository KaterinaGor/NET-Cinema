using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;
using PagedList;

namespace Cinema.Controllers
{
    public class FilmsActionController : Controller
    {
        private FilmsEntities db = new FilmsEntities();

        // GET: FilmsAction
        [Authorize]
        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            var filmes = db.Films.OrderBy(p => p.Title).ToList();
            return View(filmes.ToPagedList(pageNumber, pageSize));
        }

        // GET: FilmsAction/Details/5
        [Authorize]
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

        public FileContentResult GetImage(int id)
        {
            Film film = db.Films.FirstOrDefault(g => g.Id == id);
            if (film != null)
            {
                return File(film.Cover, film.ImageMimeType);
            }
            return null;
        }


        // GET: FilmsAction/Create
        [Authorize(Roles = "admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: FilmsAction/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Year,Description,Cover,ImageMimeType,Country")] Film film, HttpPostedFileBase Image)  
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {

                    film.ImageMimeType = Image.ContentType;
                    film.Cover = new byte[Image.ContentLength];
                    Image.InputStream.Read(film.Cover, 0, Image.ContentLength);

                }
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(film);
        }

        // GET: FilmsAction/Edit/5
        [Authorize(Roles = "admin")]

        public ActionResult Edit(int? id)
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

        // POST: FilmsAction/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Year,Description,Cover,ImageMimeType,Country")] Film film, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    film.ImageMimeType = Image.ContentType;
                    film.Cover = new byte[Image.ContentLength];
                    Image.InputStream.Read(film.Cover, 0, Image.ContentLength);
                }
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        // GET: FilmsAction/Delete/5
        [Authorize(Roles = "admin")]

        public ActionResult Delete(int? id)
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

        // POST: FilmsAction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
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
