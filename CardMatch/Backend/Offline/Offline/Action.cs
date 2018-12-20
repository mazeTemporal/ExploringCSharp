using System;

namespace Offline
{
  public class Action
  {
    private string description;
    private System.Action toRun;

    public Action(string descript, System.Action act)
    {
      description = descript;
      toRun = act;
    }

    public string GetDescription()
    {
      return (description);
    }

    public void Run()
    {
      toRun();
    }
  }
}
