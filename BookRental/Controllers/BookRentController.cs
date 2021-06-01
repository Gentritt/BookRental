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

				if(book.RentalDuration == "6")
				{
					rentalprice = Convert.ToDouble(booktoRent.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateSixMonth);
				}
				else
				{
					rentalprice = Convert.ToDouble(booktoRent.Price) * Convert.ToDouble(chargerate.ToList()[0].ChargeRateOneMonth);
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
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
		}
	}
}