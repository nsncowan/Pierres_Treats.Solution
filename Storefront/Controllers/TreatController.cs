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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Storefront.Controllers
{
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

    [AllowAnonymous]
    public ActionResult Index()
    {
      List<Treat> model = _db.Treats
                                    // .Include(treat => treat.FlavorTreatJoinEntities)
                                    .ToList();
      return View(model);
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
      return RedirectToAction("Index"/* , "Home" */);
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
                            .Include(treat => treat.FlavorTreatJoinEntities)
                            .ThenInclude(join => join.Flavor)
                            .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
    Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
    _db.Treats.Remove(thisTreat);
    _db.SaveChanges();
    return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(flavorTreat => (flavorTreat.TreatId == treat.TreatId && flavorTreat.FlavorId == flavorId));
      #nullable disable
      if(joinEntity == null && flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() {FlavorId = flavorId, TreatId = treat.TreatId});
        _db.SaveChanges();
      }
      return RedirectToAction("details", new {id = treat.TreatId});
    }
  }
}