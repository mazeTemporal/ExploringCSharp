using System.Web.Mvc;
using CardMatchLibrary.Models;
using CardMatchLibrary.DataAccess;
using System.Collections.Generic;

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

    public ActionResult DetectCanonical()
    {
      DataConnector.DetectCanonical();
      return Redirect("AssignCanonical");
    }

    [HttpGet]
    public ActionResult AssignCanonical()
    {
      List<ReleaseModel> releases = DataConnector.GetReleaseNeedCanonical();
      return View(releases);
    }

    [HttpPost]
    public ActionResult AssignCanonical(ReleaseModel release)
    {
      DataConnector.ReleaseAssignCanonical(release);
      return RedirectToAction("AssignCanonical");
    }

    [HttpGet]
    public ActionResult JudgeMatchable()
    {
      ReleaseModel release = DataConnector.GetReleaseNeedJudgeMatchability();
      return View(release);
    }

    [HttpPost]
    public ActionResult JudgeMatchable(ReleaseModel release)
    {
      DataConnector.ReleaseAssignMatchability(release);
      return RedirectToAction("JudgeMatchable");
    }

    [HttpGet]
    public ActionResult NeedCutout()
    {
      List<ReleaseModel> releases = DataConnector.GetReleaseNeedCutout();
      return View(releases);
    }
  }
}
