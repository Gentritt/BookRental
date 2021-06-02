using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookRental.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private static ApplicationDbContext _context;

		public GenreController()
		{
            _context = new ApplicationDbContext();
		}
        // GET: Genre
        public ActionResult Index()
        {
            var genres = _context.Genres.ToList();
            return View(genres);
        }

        public ActionResult Create()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
		{

			if (ModelState.IsValid)
			{
                _context.Genres.Add(genre);
                _context.SaveChanges();

                return RedirectToAction("Index");
			}
            return View();
		}

        public ActionResult Details(int id)
		{
            if(id == 0)
			{
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
            Genre genre = _context.Genres.Find(id);
            return View(genre);
		}
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Genre genre = _context.Genres.Find(id);
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Edit(Genre genre)
		{
			if (ModelState.IsValid)
			{
				var genreinDb = _context.Genres.SingleOrDefault(g => g.Id == genre.Id);
				genreinDb.Name = genre.Name;
				//_context.Entry(genre).State = EntityState.Modified;
				_context.SaveChanges();
                return RedirectToAction("Index");
			}
			return View();
		}
        public ActionResult Delete(int? id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Genre genre = _context.Genres.Find(id);
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
        
                var genre = _context.Genres.Find(id);
                _context.Genres.Remove(genre);

                //_context.Entry(genre).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        protected override void Dispose(bool disposing)
		{
            _context.Dispose();
		}
	}
}