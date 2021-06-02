using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookRental.Controllers.API
{
    
    public class UsersController : ApiController
    {
        private ApplicationDbContext db;
		public UsersController()
		{
            db = ApplicationDbContext.Create();
		}
		//To REtrieve Email or (name and birthday)
		public IHttpActionResult Get(string type, string query = null)
		{
			if (type.Equals("email") && query != null)
			{
				//This will retrieve the customer from database based on email match.
				var customerQuery = db.Users.Where(u => u.Email.ToLower().Contains(query.ToLower()));

				return Ok(customerQuery.ToList());
			}
			if (type.Equals("name") && query != null)
			{
				//This will retrieve the customer from database based on email match.
				var customerQuery = from u in db.Users
									where u.Email.Contains(query)
									select new { u.Firstname, u.Lastname, u.Birthdate,u.RentalCount };

				return Ok(customerQuery.ToList()[0].Firstname + " " + customerQuery.ToList()[0].Lastname + " ;"+ customerQuery.ToList()[0].Birthdate + " ;" + customerQuery.ToList()[0].RentalCount);
			}
			return BadRequest();
			
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
