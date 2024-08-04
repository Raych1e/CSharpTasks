using C_Tasks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace C_Tasks.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(ResultModel model)
        {
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetResult(RequesModel model)
        {
            if (model.RequestEnter == null) 
            {
                return BadRequest("Строка не может быть пустой!");
            }
            HttpClient client = new HttpClient();
            string currentUrl = $"{Request.Scheme.ToString()}://{Request.Host.ToString()}/GetResult?SortMethod={model.RequestSortMethod}&enter={model.RequestEnter}";
            var result = await client.GetAsync(currentUrl);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                var body = JsonConvert.DeserializeObject<ResultModel>(response);

                return RedirectToAction("Index", body);
            }
            else
            {
                return BadRequest(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
