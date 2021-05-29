using BookRental.Models;
using BookRental.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookRental.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db;
		public UserController()
		{
            db = ApplicationDbContext.Create();

		}
        // GET: User
        public ActionResult Index()
        {

			var user = from u in db.Users
					   join m in db.MembershipTypes on u.MembershipTypeId
					   equals m.Id
					   select new UserViewModel
					   {
						   Id = u.Id,
						   Firstname = u.Firstname,
						   LastName = u.Lastname,
						   Email = u.Email,
						   MembershipTypeId = u.MembershipTypeId,
						   Birthdate = u.Birthdate,
						   //will retrieve the memmbershiptype that belongs to the membershiptypeID
						   MembershipTypes = (ICollection<MembershipType>)db.MembershipTypes.ToList().Where(n => n.Id.Equals(u.MembershipTypeId)),
						   Disable = u.Disable,

					   };
			var userlist = user.ToList();
            return View(userlist);
        }

		public ActionResult Edit(string id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			ApplicationUser user = db.Users.Find(id);
			if(user == null)
			{
				return HttpNotFound();
			}
			UserViewModel viewModel = new UserViewModel
			{
				Firstname = user.Firstname,
				LastName = user.Lastname,
				Birthdate = user.Birthdate,
				Email = user.Email,
				Id = user.Id,
				MembershipTypeId = user.MembershipTypeId,
				MembershipTypes = db.MembershipTypes.ToList(),
				Disable = user.Disable,
			};
			return View(viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(UserViewModel user)
		{
			if (!ModelState.IsValid)
			{
				UserViewModel model = new UserViewModel
				{
					Firstname = user.Firstname,
					LastName = user.LastName,
					Birthdate = user.Birthdate,
					Email = user.Email,
					Id = user.Id,
					MembershipTypeId = user.MembershipTypeId,
					MembershipTypes = db.MembershipTypes.ToList(),
					Disable = user.Disable,
				};
				return View("Edit", model);
			}
			else
			{	
				//This will retrieve the database object and then update the database object with the user objects.
				var userinDb = db.Users.Single(u => u.Id.Equals(user.Id));
				userinDb.Firstname = user.Firstname;
				userinDb.Lastname = user.LastName;
				userinDb.Birthdate = user.Birthdate;
				userinDb.Email = user.Email;
				userinDb.MembershipTypeId = user.MembershipTypeId;
				userinDb.Disable = user.Disable;
			}
			db.SaveChanges();
			return RedirectToAction("Index", "User");

		}
		public ActionResult Details(string id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			ApplicationUser user = db.Users.Find(id);
			UserViewModel model = new UserViewModel
			{
				Firstname = user.Firstname,
				LastName = user.Lastname,
				Birthdate = user.Birthdate,
				Email = user.Email,
				Id = user.Id,
				MembershipTypeId = user.MembershipTypeId,
				MembershipTypes = db.MembershipTypes.ToList(),
				Disable = user.Disable,
			};
			return View(model);
		}
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			ApplicationUser user = db.Users.Find(id);
			UserViewModel model = new UserViewModel
			{
				Firstname = user.Firstname,
				LastName = user.Lastname,
				Birthdate = user.Birthdate,
				Email = user.Email,
				Id = user.Id,
				MembershipTypeId = user.MembershipTypeId,
				MembershipTypes = db.MembershipTypes.ToList(),
				Disable = user.Disable,
			};
			return View(model);
		}
		//Delete Post method
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			var userindb = db.Users.Find(id);
			if(id == null)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
			}
			userindb.Disable = true;
			db.SaveChanges();
			return RedirectToAction("Index");

		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{

			}
		}
	}
}