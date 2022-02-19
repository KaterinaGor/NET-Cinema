using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;
using PagedList;

namespace Cinema.Controllers
{
    public class ActorsActionController : Controller
    {
        private FilmsEntities db = new FilmsEntities();

        // GET: ActorsAction
        public ActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            var actor = db.Actors.OrderBy(p => p.FirstName).ToList();
            return View(actor.ToPagedList(pageNumber, pageSize));
        }

        // GET: ActorsAction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            actor = db.Actors.Include(f => f.FilmActors).FirstOrDefault(a => a.Id == id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        public FileContentResult GetImage(int id)
        {
            Actor actor = db.Actors.FirstOrDefault(a => a.Id == id);
            if (actor != null)
            {
                return File(actor.Photo, actor.ImageMimeType);
            }
            return null;
        }

        // GET: ActorsAction/Create
        [Authorize(Roles = "admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: ActorsAction/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Photo,ImageMimeType")] Actor actor, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    actor.ImageMimeType = Image.ContentType;
                    actor.Photo = new byte[Image.ContentLength];
                    Image.InputStream.Read(actor.Photo, 0, Image.ContentLength);
                }
                db.Actors.Add(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FilmIs = new SelectList(db.Films, "Id", "Title");
            return View(actor);
        }

        // GET: ActorsAction/Edit/5
        [Authorize(Roles = "admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: ActorsAction/Edit/5
        
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Photo,ImageMimeType")] Actor actor, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    actor.ImageMimeType = image.ContentType;
                    actor.Photo = new byte[image.ContentLength];
                    image.InputStream.Read(actor.Photo, 0, image.ContentLength);
                }
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: ActorsAction/Delete/5
        [Authorize(Roles = "admin")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: ActorsAction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
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
