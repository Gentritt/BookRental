using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.Models
{
	public class MembershipType
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string  Name { get; set; }

		[Required]
		[DataType(DataType.Currency)]
		public byte SignUpFee { get; set; }
		
		[Display(Name="Rental Rate")]
		[Required]
		public byte ChargeRateOneMonth { get; set; }
		[Required]
		public byte ChargeRateSixMonth { get; set; }

	}
}