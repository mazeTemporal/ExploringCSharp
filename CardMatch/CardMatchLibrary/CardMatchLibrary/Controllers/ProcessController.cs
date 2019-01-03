using System.Web.Mvc;
using CardMatchLibrary.Models;
using CardMatchLibrary.DataAccess;

namespace CardMatchLibrary.Controllers
{
  public class ProcessController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult CreateDatabase()
    {
      DataConnector.CreateTables();
      return Redirect("/Process/");
    }

    [HttpGet]
    public ActionResult AddSet()
    {
      return View(new CardSetModel());
    }

    [HttpPost]
    public ActionResult AddSet(CardSetModel cardSet)
    {
      DataConnector.CreateCardSet(MTGJson.ReadJson(cardSet.code));
      return RedirectToAction("AddSet");
    }

    [HttpGet]
    public ActionResult AssignFilename()
    {
      ReleaseModel release = DataConnector.GetReleaseNeedImage();
      return View(release);
    }

    [HttpPost]
    public ActionResult AssignFilename(ReleaseModel release)
    {
      DataConnector.ReleaseAssignImage(release);
      return RedirectToAction("AssignFilename");
    }
  }
}
