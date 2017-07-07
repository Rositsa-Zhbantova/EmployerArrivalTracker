using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EmployerArrivalTracker
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			RegisterService();
		}

		private void RegisterService()
		{
			//////HttpClient client = new HttpClient();

			//////// Set accepted media type
			//////client.DefaultRequestHeaders.Accept.Clear();
			//////client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			//////client.DefaultRequestHeaders.Add("Accept-Client", "Fourth-Monitor");

			////////UriBuilder za da si buildna urla

			//////var response = client.GetAsync("http://localhost:51396/....").Result;
			//////if (response.IsSuccessStatusCode)
			//////{
			//////	var jsonString = response.Content.ReadAsStringAsync().Result;
			//////	dynamic model = JsonConvert.DeserializeObject(jsonString);
			//////	string token = model.Token;// tova da se zapazi w sesiata - user ili w application session
			//////	DateTime expires = model.Expires;
			//////}
			//response.Content - here is the token : and expires data
			//да преместя кода да се изпълнява с бутон 'subsscribe'
			//да използвам await 
			//da  suzdam nov action method, koito da izpulnqwa post query
		}
	}
}
