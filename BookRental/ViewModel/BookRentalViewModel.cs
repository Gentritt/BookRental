using BookRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.ViewModel
{
	public class BookRentalViewModel
	{

		//Book Details
		public int Id { get; set; }
		public int BookId { get; set; }
		public int ISBN { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		[DataType(DataType.ImageUrl)]
		public string ImageUrl { get; set; }
		[Range(0, 1000)]
		[DisplayName("Availability")]
		public int Avaliability { get; set; }
		[DataType(DataType.Currency)]
		public double Price { get; set; }
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Date Added")]

		public DateTime? DateAdded { get; set; }
		public int GenreId { get; set; }
		public Genre Genre { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Publication Date")]
		public DateTime PublicationDate { get; set; }
		public int Pages { get; set; }
		[Display(Name = "Product Dimensions")]
		public string ProductDimensions { get; set; }
		public string Publisher { get; set; }

		//Rental Details
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Start Date")]
		public DateTime? StartDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Actual End date")]
		public DateTime? ActualEndDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Scheduled End Date")]
		public DateTime? ScheduleEndDate { get; set; }
		[Display(Name = "Additional Charge")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "AddCharge must be numeric")]
		public double? AdditionalCharge { get; set; }

		[Display(Name = "Price")]
		public double RentalPrice { get; set; }
		
		public string Status { get; set; }
		public string RentalDuration { get; set; }
		
		public double rentalPriceOneMonth { get; set; }
		public double rentalPriceSixMonth { get; set; }

		//User Details
		public string UserId { get; set; }
		public string Email { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
		[Display(Name = "Birth date")]
		public DateTime Birthdate { get; set; }
		[Display(Name ="First Name")]
		public string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		public string Name { get { return FirstName + " " + LastName; } }


	}
}