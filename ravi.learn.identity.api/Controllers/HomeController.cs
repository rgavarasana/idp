using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ravi.learn.identity.web.Controllers
{
    public class HomeController : Controller
    {

        [Route("api/user")]
        public IActionResult Get()
        {
            return Ok(new { Name = User.FindFirst("name").Value });
        }

        //public async Task<IActionResult> Index()
        //{
        //    var disco = await DiscoveryClient.GetAsync("https://localhost:44363");
        //    var tokenClient = new TokenClient(disco.TokenEndpoint, "WebApi", "MySecret");
        //    var tokenResponse = await tokenClient.RequestClientCredentialsAsync("DemoApi");
        //    object model;
        //    if (tokenResponse.IsError)
        //    {
        //        model = "Error...Could not get access token for API";
        //    }else
        //    {
        //        var client = new HttpClient();
        //        client.SetBearerToken(tokenResponse.AccessToken);
        //        var response = await client.GetAsync("https://localhost:44301/api/values");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            model = await response.Content.ReadAsStringAsync();
        //        }else
        //        {
        //            model = "Error from Api";
        //        }

        //    }


        //    return View(model);
        //}

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        [Route("userinformation")]
        [Authorize]
        public IActionResult UserInformation()
        {
            return View();
        }



        //[Route("Spa")]
        //public IActionResult Spa()
        //{
        //    return View();
        //}

    }
}
