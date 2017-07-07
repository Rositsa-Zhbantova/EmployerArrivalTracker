using EmployerArrivalTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace EmployerArrivalTracker.Controllers
{
	public class ClientsController : ApiController
	{
		[HttpPost]
		[Route("api/clients/callback")]
		public IHttpActionResult Post(List<EmployeeData> data)
		{
			string token = string.Empty;

			IEnumerable<string> headerValues;
			if (Request.Headers.TryGetValues("X-Fourth-Token", out headerValues))
			{
				token = headerValues.FirstOrDefault();
			}

			//Validate token
			string expirationDate = GetExpirationTime(token);
			DateTime expirationDateTime = DateTime.ParseExact(expirationDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			if ( expirationDate != null && expirationDateTime < DateTime.Now )
			{
				return Unauthorized();
			}

			//Save EmployeeId and Time into DB
			foreach (EmployeeData item in data)
			{
				var db = new EmployeeDB();
				db.Arrivals.Add(new Arrival { EmployeeID = item.EmployeeId, Time = item.When });
				db.SaveChanges();
			}
			
			return Ok();
		}

		private string GetExpirationTime(string token)
		{
			var db = new EmployeeDB();
			var result = db.Tokens.Where(x => x.TokenNumber == token).FirstOrDefault();
			if (result != null)
			{
				return result.ExpirationTime;
			}
			else
			{
				return String.Empty;
			}
		}

	}
}

