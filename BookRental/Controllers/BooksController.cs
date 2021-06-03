 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using BookRental.Models;
using BookRental.ViewModel;
using Newtonsoft.Json;

namespace BookRental.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IAmazonS3 _client;
		public BooksController()
		{
            _client = new AmazonS3Client();
		}

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Genre);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            var model = new BookViewModel
            {
                Book = book,
                Genres = db.Genres.ToList()
            };
            return View(model);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            var genre = db.Genres.ToList();
            var model = new BookViewModel
            {
                Genres = genre
            };
            return View(model);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(BookViewModel bookVm )
        {


            var book = new Book
            {
                Author = bookVm.Book.Author,
                Avaliability = bookVm.Book.Avaliability,
                DateAdded = DateTime.Now,
                Description = bookVm.Book.Description,
                Publisher = bookVm.Book.Publisher,
                GenreId = bookVm.Book.GenreId,
                Genre = bookVm.Book.Genre,
                ImageUrl = bookVm.Book.ImageUrl,
                ISBN = bookVm.Book.ISBN,
                Pages = bookVm.Book.Pages,
                Price = bookVm.Book.Price,
                ProductDimensions = bookVm.Book.ProductDimensions,
                PublicationDate = bookVm.Book.PublicationDate,
                Title = bookVm.Book.Title

            };

            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();

                //_client.PutObject(new PutObjectRequest
                //{
                //    BucketName = "riinvest",
                //    Key = $"gent/books/{book.ISBN}.json",
                //    ContentBody = JsonConvert.SerializeObject(book)
                //});

                return RedirectToAction("Index");
            }

            bookVm.Genres = db.Genres.ToList();
            return View(bookVm);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            var model = new BookViewModel
            {
                Book = book,
                Genres = db.Genres.ToList()
            };
            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BookViewModel bookVm)
        {

            var book = new Book
            {
                Id = bookVm.Book.Id,
                Author = bookVm.Book.Author,
                Avaliability = bookVm.Book.Avaliability,
                Publisher = bookVm.Book.Publisher,
                DateAdded = bookVm.Book.DateAdded,
                Description = bookVm.Book.Description,
                GenreId = bookVm.Book.GenreId,
                Genre = bookVm.Book.Genre,
                ImageUrl = bookVm.Book.ImageUrl,
                ISBN = bookVm.Book.ISBN,
                Pages = bookVm.Book.Pages,
                Price = bookVm.Book.Price,
                ProductDimensions = bookVm.Book.ProductDimensions,
                PublicationDate = bookVm.Book.PublicationDate,
                Title = bookVm.Book.Title

            };
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            bookVm.Genres = db.Genres.ToList();
            return View(bookVm);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            var model = new BookViewModel
            {
                Book = book,
                Genres = db.Genres.ToList()
            };
            return View(model);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
