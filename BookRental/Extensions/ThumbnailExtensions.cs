using BookRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookRental.Extensions
{
	public static class ThumbnailExtensions
	{

		public static IEnumerable<ThumbnailModel> GetBookThumbnail(this List<ThumbnailModel> thumbnails, ApplicationDbContext db = null)
		{
			//Gets Books from db.Books table in database and adds them as thumbnails;
			try
			{
				if (db == null) db = ApplicationDbContext.Create();
				thumbnails = (from b in db.Books
							  select new ThumbnailModel
							  {
								  BookId  = b.Id,
								  Title = b.Title,
								  Description = b.Description,
								  ImageUrl = b.ImageUrl,
								  Link = "/BookDetail/Index"+b.Id
							  }).ToList();


			}
			catch (Exception ex)
			{

				throw;
			}

			return thumbnails.OrderBy(b => b.Title);
		}
	}
}