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


[Authorize]
public class FlavorController : Controller
{
  private readonly StorefrontContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public FlavorController(UserManager<ApplicationUser> userManager, StorefrontContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Flavor flavor)
  {
    _db.Flavors.Add(flavor);
    _db.SaveChanges();
    return RedirectToAction("Index", "Home");
  }

  public ActionResult Index()
  {
    List<Flavor> model = _db.Flavors
                                  .Include(flavor => flavor.FlavorTreatJoinEntities)
                                  .ToList();
    return View(model);
  }

  public ActionResult Details(int id)
  {
    Flavor thisFlavor = _db.Flavors
                          .Include(flavor => flavor.FlavorTreatJoinEntities)
                          .ThenInclude(join => join.Treat)
                          .FirstOrDefault(flavor => flavor.FlavorId == id);
    return View(thisFlavor);
  }

  public ActionResult Edit(int id)
  {
    Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
    return View(thisFlavor);
  }

  [HttpPost]
  public ActionResult Edit(Flavor flavor)
  {
    _db.Flavors.Update(flavor);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult Delete(int id)
  {
    Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
    return View(thisFlavor);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
  Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
  _db.Flavors.Remove(thisFlavor);
  _db.SaveChanges();
  return RedirectToAction("Index");
  }

  public ActionResult AddTreat(int id)
  {
    Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
    ViewBag.TreatSelect = new SelectList(_db.Treats, "TreatId", "Name");
    return View(thisFlavor);
  }

  [HttpPost]
  public ActionResult AddTreat(Flavor flavor, int treatId)
  {
    #nullable enable
    FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(flavorTreat => (flavorTreat.TreatId == treatId && flavorTreat.FlavorId == flavor.FlavorId));
    #nullable disable
    if(joinEntity == null && treatId != 0)
    {
      _db.FlavorTreats.Add(new FlavorTreat() {FlavorId = flavor.FlavorId, TreatId = treatId});
      _db.SaveChanges();
    }
    return RedirectToAction("Details", new {id = flavor.FlavorId});
  }




}