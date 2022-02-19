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
    public class FilmActorsActionController : Controller
    {
        private FilmsEntities db = new FilmsEntities();

        // GET: FilmActorsAction
        public ActionResult Index()
        {
            var filmActors = db.FilmActors.Include(f => f.Actor).Include(f => f.Film);
            return View(filmActors.ToList());
        }

        // GET: FilmActorsAction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmActor filmActor = db.FilmActors.Find(id);
            if (filmActor == null)
            {
                return HttpNotFound();
            }
            return View(filmActor);
        }

        // GET: FilmActorsAction/Create
        public ActionResult Create()
        {
            ViewBag.ActorId = new SelectList(db.Actors, "Id", "FirstName");
            ViewBag.FilmId = new SelectList(db.Films, "Id", "Title");
            return View();
        }

        // POST: FilmActorsAction/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FilmId,ActorId")] FilmActor filmActor)
        {
            if (ModelState.IsValid)
            {
                db.FilmActors.Add(filmActor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActorId = new SelectList(db.Actors, "Id", "FirstName", filmActor.ActorId);
            ViewBag.FilmId = new SelectList(db.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActorsAction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmActor filmActor = db.FilmActors.Find(id);
            if (filmActor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActorId = new SelectList(db.Actors, "Id", "FirstName", filmActor.ActorId);
            ViewBag.FilmId = new SelectList(db.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // POST: FilmActorsAction/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FilmId,ActorId")] FilmActor filmActor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filmActor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActorId = new SelectList(db.Actors, "Id", "FirstName", filmActor.ActorId);
            ViewBag.FilmId = new SelectList(db.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActorsAction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilmActor filmActor = db.FilmActors.Find(id);
            if (filmActor == null)
            {
                return HttpNotFound();
            }
            return View(filmActor);
        }

        // POST: FilmActorsAction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilmActor filmActor = db.FilmActors.Find(id);
            db.FilmActors.Remove(filmActor);
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
