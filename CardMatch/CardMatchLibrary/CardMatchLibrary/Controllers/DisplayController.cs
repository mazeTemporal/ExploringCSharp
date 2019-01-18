using System.Web.Mvc;
using CardMatchLibrary.Models;
using CardMatchLibrary.DataAccess;
using System.Collections.Generic;

namespace CardMatchLibrary.Controllers
{
  public class DisplayController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public ActionResult Lands()
    {
      List<ReleaseModel> bases = DataConnector.GetBases();
      return View(bases);
    }

    [HttpGet]
    public ActionResult SelectLand(int baseId)
    {
      List<MatchModel> matches = DataConnector.GetMatchesWithBase(baseId);
      return View(matches);
    }
  }
}
