﻿using BookRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.ViewModel
{
	public class UserViewModel
	{

		[Required]
		public string Id { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		public ICollection<MembershipType> MembershipTypes { get; set; }
		[Required]
		public int MembershipTypeId { get; set; }
		[Required]
		public string Firstname { get; set; }
		[Required]
		public string LastName { get; set; }
		public bool Disable { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString ="{0:MM dd yyyy}")]
		public DateTime Birthdate { get; set; }
		public int RentalCount { get; set; }

 	}
}
