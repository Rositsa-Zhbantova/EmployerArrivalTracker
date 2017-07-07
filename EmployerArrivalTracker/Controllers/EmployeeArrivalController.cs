using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace EmployerArrivalTracker.Controllers
{
    public class EmployeeArrivalController : Controller
    {
		private EmployeeDB db = new EmployeeDB();

        public ActionResult EmployeesArrival(string SortOrder, string SelectedFirstName, string SelectedLastName)
        {
			ViewBag.FirstName = String.IsNullOrEmpty(SortOrder) ? "FirstName_desc" : "";
			ViewBag.LastName = SortOrder == "LastName" ? "LastName_desc" : "LastName";

			var rowData = (from s in db.Employees select s).ToList();
			var employees = from s in rowData select s;

			if (!String.IsNullOrWhiteSpace(SelectedFirstName))
			{
				employees = employees.Where(s => s.FirstName.Trim().Equals(SelectedFirstName.Trim()));
			}
			if (!String.IsNullOrWhiteSpace(SelectedLastName))
			{
				employees = employees.Where(s => s.LastName.Trim().Equals(SelectedLastName.Trim()));
			}

			var uniqueFirstNames = from s in employees
								   group s by s.FirstName into newGroup
								   where newGroup.Key != null
								   orderby newGroup.Key
								   select new { FirstName = newGroup.Key };

			ViewBag.UniqueFirstNames = uniqueFirstNames.Select(m => new SelectListItem { Value = m.FirstName, Text = m.FirstName }).ToList();

			var uniqueLastNames = from s in employees
								   group s by s.LastName into newGroup
								   where newGroup.Key != null
								   orderby newGroup.Key
								   select new { LastName = newGroup.Key };

			ViewBag.UniqueLastNames = uniqueLastNames.Select(m => new SelectListItem { Value = m.LastName, Text = m.LastName }).ToList();

			ViewBag.SelectedFirstName = SelectedFirstName;
			ViewBag.SelectedLastName = SelectedLastName;
			switch (SortOrder)
			{
				case "FirstName_desc":
					employees = employees.OrderByDescending(s => s.FirstName);
					break;
				case "FirstName":
					employees = employees.OrderBy(s => s.FirstName);
					break;
				case "LastName_desc":
					employees = employees.OrderByDescending(s => s.LastName);
					break;
				case "LastName":
					employees = employees.OrderBy(s => s.LastName);
					break;
				default:
					employees = employees.OrderBy(s => s.FirstName);
					break;
			}

			return View(employees.ToList());
        }

		//Registration of the service
		public void RegisterService()
		{
			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			client.DefaultRequestHeaders.Add("Accept-Client", "Fourth-Monitor");

			UriBuilder urlAddress = new UriBuilder("http://localhost:51396/");

			//Url Path of the address
			urlAddress.Path = "api/clients/subscribe";

			string urlDate = "date=" + DateTime.Now.ToString("yyyy-MM-dd"); 
			string urladdress = "http://localhost:37434/api/clients/callback";
			string ulrCallBack = "callback=" + urladdress;

			//url Query of the address
			urlAddress.Query = urlDate + "&" + ulrCallBack;

			var response = client.GetAsync(urlAddress.ToString()).Result;
			if (response.IsSuccessStatusCode)
			{
				var jsonString = response.Content.ReadAsStringAsync().Result;
				dynamic model = JsonConvert.DeserializeObject(jsonString);
				db.Tokens.Add(new Token { TokenNumber = model.Token, ExpirationTime = model.Expires });
				db.SaveChanges();
			}
			
		}
	}
}