﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.Models
{
	public class BookRent
	{
		private double x = 0;

		[Required]
		public int Id { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public int BookId { get; set; }


		public DateTime? StartDate { get; set; } = DateTime.Now;
		public DateTime? ActualEndDate { get; set; }
		public DateTime? ScheduleEndDate { get; set; }
		//public double? AdditionalCharge { get { return x; } set { this.x = AdditionalCharge.Value; } }
		public double? AdditionalCharge { get; set; } = 0;
		[Required]
		public double Price { get; set; }
		[Required]
		public string RentalDuration { get; set; }
		[Required]
		
		public StatusEnum Status { get; set; }
		public enum StatusEnum
		{
			Requested,
			Approved,
			Rejected,
			Rented,
			Closed
		}

	}
}