using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
namespace VehicleParkingSystem.Models
{
	public class RecaptchaResponse
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("error-codes")]
		public List<string> ErrorCodes { get; set; }
	}
}