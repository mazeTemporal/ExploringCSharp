using System.Web.Http;
using CardMatchLibrary.DataAccess;

namespace CardMatchLibrary.Controllers
{
  public class ProcessApiController : ApiController
  {
    [Route("api/Process/CreateTables")]
    [HttpGet]
    public string CreateTables()
    {
      DataConnector.CreateTables();
      return ("Tables Created");
    }

    [Route("api/Process/ImportSet/{setCode}")]
    [HttpGet]
    public string ImportSet(string setCode)
    {
      DataConnector.CreateCardSet(MTGJson.ReadJson(setCode));
      return ("Set Inserted");
    }
  }
}
