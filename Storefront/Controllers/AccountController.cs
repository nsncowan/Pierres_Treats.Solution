using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Storefront.Models;
using System.Threading.Tasks;
using Storefront.ViewModels;

public class AccountController : Controller
{
  private readonly StorefrontContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;

  public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, StorefrontContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _db = db;
  }

  public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model)
      }
      else
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IndentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }



}