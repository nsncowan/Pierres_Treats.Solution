using Microsoft.AspNetCore.Mvc;
using Storefront.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Storefront.Controllers
{
  public class HomeController : Controller
  {
    private readonly StorefrontContext _db;
    private readonly UserManager<ApplicationUser> _userManager;


  public HomeController(UserManager<ApplicationUser> userManager, StorefrontContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
      return View();
    }
  }
}