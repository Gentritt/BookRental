using BookRental.Models;
using BookRental.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookRental.Controllers
{
    public class BookRentController : Controller
    {
        private ApplicationDbContext db;
		public BookRentController()
		{
            db = ApplicationDbContext.Create();
		}
        public ActionResult Index()
        {
			string userid = User.Identity.GetUserId();

			var model = from br in db.BookRents
						join b in db.Books
						on br.BookId equals b.Id
						join u in db.Users on br.UserId equals u.Id
						//Joining all the tables, and then returnin the bookrentalviewmodel object to the view.

						select new BookRentalViewModel
						{
							BookId = b.Id,
							RentalPrice = br.Price,
							Price = b.Price,
							Pages = b.Pages,
							FirstName = u.Firstname,
							LastName = u.Lastname,
							Birthdate = u.Birthdate,
							ScheduleEndDate = br.ScheduleEndDate,
							Author = b.Author,
							Avaliability = b.Avaliability,
							DateAdded = b.DateAdded,
							Description = b.Description,
							Email = u.Email,
							GenreId = b.GenreId,
							Genre = db.Genres.Where(g => g.Id.Equals(b.GenreId)).FirstOrDefault(),
							ISBN = b.ISBN,
							ImageUrl = b.ImageUrl,
							ProductDimensions = b.ProductDimensions,
							PublicationDate = b.PublicationDate,
							Publisher = b.Publisher,
							RentalDuration = br.RentalDuration,
							Status = br.Status.ToString(),
							Title = b.Title,
							UserId = u.Id,
							Id = br.Id,
							StartDate = br.StartDate
						};
			if (!User.IsInRole("Admin"))
			{
				model = model.Where(u => u.UserId.Equals(userid));
			}

            return View(model.ToList()); 
        }

		public ActionResult Create(string title = null,int isbn = 0)
		{
			if(title != null && isbn !=0)
			{
				BookRentalViewModel model = new BookRentalViewModel
				{
					Title = title,
					ISBN = isbn,
				};
			}
			return View(new BookRentalViewModel());
		}

		//Post Action
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(BookRentalViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = model.Email;

				var userDetails = from u in db.Users
								  where u.Email.Equals(email)
								  select new { u.Id }; //gets the user details based on email;

				var isbn = model.ISBN;

				Book bookselected = db.Books.Where(b => b.ISBN.Equals(isbn)).FirstOrDefault(); // Gets the selected book
				var rentalduration = model.RentalDuration;

				var chargerate = from u in db.Users
								 join m in db.MembershipTypes
								 on u.MembershipTypeId equals m.Id
								 where u.Email.Equals(email)
								 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth }; //Gets the charge rate based on membership

				//charge rate for one month calc;
				var onemonth = Convert.ToDouble(bookselected.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateOneMonth) / 100;
				//charge rate for six month calc;
				var sixmonth = Convert.ToDouble(bookselected.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateSixMonth) / 100;

				double rentalprice = 0;
				if(model.RentalDuration == "6")
				{
					rentalprice = sixmonth;
				}
				else
				{
					rentalprice = onemonth;
				}
				BookRent modeltoAdd = new BookRent
				{
					BookId = bookselected.Id,
					Price = rentalprice,
					ScheduleEndDate = model.ScheduleEndDate,
					RentalDuration = rentalduration,
					Status = BookRent.StatusEnum.Requested,
					UserId = userDetails.ToList()[0].Id
				};
				bookselected.Avaliability -= 1;
				db.BookRents.Add(modeltoAdd);
				db.SaveChanges();
				return RedirectToAction("Index");


			}
			return View();
		}
		[HttpPost]
		public ActionResult Reserve(BookRentalViewModel book)
		{

			var userid = User.Identity.GetUserId();
			Book booktoRent = db.Books.Find(book.BookId);

			double rentalprice = 0;

			if (userid != null)
			{
				var chargerate = from u in db.Users
								 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
								 where u.Id.Equals(userid)
								 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };



				var sixmonth = Convert.ToDouble(booktoRent.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateSixMonth);
				var onemonth = Convert.ToDouble(booktoRent.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateOneMonth);
				if (book.RentalDuration == "6")
				{
					rentalprice = sixmonth;
				}
				else
				{
					rentalprice = onemonth;
				}

				BookRent bookRent = new BookRent
				{
					BookId = booktoRent.Id,
					UserId = userid,
					RentalDuration = book.RentalDuration,
					Price = rentalprice,
					Status = BookRent.StatusEnum.Requested
				};

				db.BookRents.Add(bookRent);
				var bookInDb = db.Books.SingleOrDefault(c => c.Id == book.BookId);
				bookInDb.Avaliability -= 1;
				db.SaveChanges();
				return RedirectToAction("Index", "BookRent");
			}

			return View();
		}

		public ActionResult Details(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}

			BookRent bookRent = db.BookRents.Find(id);
			var model = getVM(bookRent);
			if(model == null)
			{
				return HttpNotFound();
			}
			return View(model);
		}
		public ActionResult Decline(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			BookRent rent = db.BookRents.Find(id);
			var model = getVM(rent);
			if(model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Decline(BookRentalViewModel book)
		{

			if(book.Id == 0)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			}
			if (ModelState.IsValid) { 
			BookRent bookRent = db.BookRents.Find(book.Id);
			bookRent.Status = BookRent.StatusEnum.Rejected;
			Book bookindb = db.Books.Find(bookRent.BookId);
			bookindb.Avaliability += 1;
			db.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public ActionResult Approve(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			BookRent bookRent = db.BookRents.Find(id);
			var model = getVM(bookRent);
			if (model == null)
				return HttpNotFound();
			return View("Approve", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Approve(BookRentalViewModel model)
		{
			if (model == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			if (ModelState.IsValid)
			{
				BookRent bookRent = db.BookRents.Find(model.Id);

				bookRent.Status = BookRent.StatusEnum.Approved;

				db.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		public ActionResult PickUp(int? id)
		{

			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			BookRent bookRent = db.BookRents.Find(id);
			var model = getVM(bookRent);
			if (model == null)
				return HttpNotFound();
			return View("PickUp",model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult PickUp(BookRentalViewModel model)
		{
			if(model == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			if (ModelState.IsValid)
			{
				BookRent bookRent = db.BookRents.Find(model.Id);
				bookRent.Status = BookRent.StatusEnum.Rented;
				bookRent.StartDate = DateTime.Now;
				if(bookRent.RentalDuration == "6")
				{
					bookRent.ScheduleEndDate = DateTime.Now.AddMonths(Convert.ToInt32("6"));
				}
				else
				{
					bookRent.ScheduleEndDate = DateTime.Now.AddMonths(Convert.ToInt32("1"));
				}
				db.SaveChanges();

			}

			return RedirectToAction("Index");
		}


		public ActionResult Return(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

			var bookrent = db.BookRents.Find(id);
			var model = getVM(bookrent);
			if (model == null)
				return HttpNotFound();

			return View("Return",model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Return(BookRentalViewModel model)
		{
			//double addCharge = 5;
			if (model == null)
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			if (ModelState.IsValid)
			{
				BookRent bookRent = db.BookRents.Find(model.Id);
				bookRent.Status = BookRent.StatusEnum.Closed;

				bookRent.AdditionalCharge = model.AdditionalCharge;
				bookRent.Price += (Double)bookRent.AdditionalCharge;
				Book BookInDb = db.Books.Find(bookRent.BookId);
				BookInDb.Avaliability += 1;

				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View("Return", model);
		
			
		}
		private BookRentalViewModel getVM(BookRent book)
		{
			Book selectedBook = db.Books.Where(b => b.Id == book.BookId).FirstOrDefault();
			var userDetails = from u in db.Users
							  where u.Id.Equals(book.UserId)
							  select new { u.Id, u.Firstname, u.Lastname,u.Birthdate, u.Email };

			BookRentalViewModel model = new BookRentalViewModel
			{
				Id = book.Id,
				BookId = book.BookId,
				RentalPrice = book.Price,
				Pages = selectedBook.Pages,
				FirstName = userDetails.ToList()[0].Firstname,
				LastName = userDetails.ToList()[0].Lastname,
				Birthdate = userDetails.ToList()[0].Birthdate,
				ScheduleEndDate = book.ScheduleEndDate,
				Author = selectedBook.Author,
				StartDate = book.StartDate,
				Avaliability = selectedBook.Avaliability,
				DateAdded = selectedBook.DateAdded,
				Description = selectedBook.Description,
				Email = userDetails.ToList()[0].Email,
				GenreId = selectedBook.GenreId,
				Genre = db.Genres.FirstOrDefault(g => g.Id.Equals(selectedBook.GenreId)),
				ISBN = selectedBook.ISBN,
				ImageUrl = selectedBook.ImageUrl,
				ProductDimensions = selectedBook.ProductDimensions,
				PublicationDate = selectedBook.PublicationDate,
				Publisher = selectedBook.Publisher,
				RentalDuration = book.RentalDuration,
				Status = book.Status.ToString(),
				Title = selectedBook.Title,
				UserId = userDetails.ToList()[0].Id,
				AdditionalCharge = book.AdditionalCharge
			};
			return model;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
		}
	}
}