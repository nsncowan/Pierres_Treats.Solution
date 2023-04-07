using Microsoft.AspNetCore.Mvc;
using Storefront.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

[Authorize]
public class TreatController : Controller
{
  private readonly StorefrontContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public TreatController(UserManager<ApplicationUser> userManager, StorefrontContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Treat treat)
  {
    _db.Treats.Add(treat);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }
  
  
  
  
  
  public ActionResult Index()
  {
    List<Treat> model = _db.Treats
                                  .Include(treat => treat.FlavorTreatJoinEntities)
                                  .ToList();
    return View(model);
  }






}