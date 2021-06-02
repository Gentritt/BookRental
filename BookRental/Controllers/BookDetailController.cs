using BookRental.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BookRental.ViewModel;

namespace BookRental.Controllers
{
    public class BookDetailController : Controller
    {
        // GET: BookDetail
        private ApplicationDbContext db;
		public BookDetailController()
		{
            db = ApplicationDbContext.Create();
		}
        public ActionResult Index(int id)
        {
			var userid = User.Identity.GetUserId();
			var user = db.Users.FirstOrDefault(u => u.Id == userid);

			var bookmodel = db.Books.Include(b => b.Genre).SingleOrDefault(b => b.Id == id);

			var rentalprice = 0.0;
			var rentalPriceOneMonth = 0.0;
			var rentalPriceSixMonth = 0.0;
			var rentalCount = 0;
			
			if(user != null && !User.IsInRole("Admin"))
			{
				var chargeRate = from u in db.Users
								 join m in db.MembershipTypes
								 on u.MembershipTypeId equals m.Id
								 where u.Id.Equals(userid)
								 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth,u.RentalCount };  //this will join users and based on their memberships we 
																							 // we retrieving their charge rate for one month and six month.
				rentalPriceOneMonth = Convert.ToDouble(bookmodel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth)/ 100;
				rentalPriceSixMonth = Convert.ToDouble(bookmodel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
				rentalCount = Convert.ToInt32(chargeRate.ToList()[0].RentalCount);

			}

			BookRentalViewModel model = new BookRentalViewModel
			{
				BookId = bookmodel.Id,
				ISBN = bookmodel.ISBN,
				Author = bookmodel.Author,
				Avaliability = bookmodel.Avaliability,
				DateAdded = bookmodel.DateAdded,
				Description = bookmodel.Description,
				Genre = db.Genres.FirstOrDefault(g => g.Id.Equals(bookmodel.GenreId)),
				GenreId = bookmodel.GenreId,
				ImageUrl = bookmodel.ImageUrl,
				Pages = bookmodel.Pages,
				Price = bookmodel.Price,
				PublicationDate = bookmodel.PublicationDate,
				ProductDimensions = bookmodel.ProductDimensions,
				Title = bookmodel.Title,
				UserId = userid,
				RentalPrice = rentalprice,
				rentalPriceOneMonth = rentalPriceOneMonth,
				rentalPriceSixMonth = rentalPriceSixMonth,
				RentalCount = rentalCount,
				Publisher = bookmodel.Publisher,
			};
            return View(model);
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