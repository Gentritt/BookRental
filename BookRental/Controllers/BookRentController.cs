using BookRental.Models;
using BookRental.ViewModel;
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
            return View();
        }

		public ActionResult Create(string title = null,string isbn = null)
		{
			if(title != null && isbn != null)
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
		}
	}
}