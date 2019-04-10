using DataLibrary.Models;
using DataLibrary.DataAccess;

namespace DataLibrary.BusinessLogic
{
  public static class GroupProcessor
  {
    public static int CreateGroup(string CanonicalName)
    {
      //!!! this should be a transaction

      // create the empty group
      string sql = @"INSERT INTO Group; SELECT CAST(SCOPE_IDENTITY() as int)";
      GroupModel group = new GroupModel();
      group.GroupId = SQLiteDataAccess.InsertGetId(sql, group);

      // create the group's name
      sql = @"INSERT INTO GroupName ( GroupId, GroupName ) 
        VALUES ( @GroupId, @GroupName )";
      GroupNameModel groupName = new GroupNameModel
        { GroupId = group.GroupId, GroupName = CanonicalName };
      group.CanonicalNameId = SQLiteDataAccess.InsertGetId(sql, groupName);

      // set name as group's canonical name
      sql = @"UPDATE Group SET CanonicalNameId = @CanonicalNameId WHERE GroupId = @GroupId";
      int rowsAffected = SQLiteDataAccess.SaveData(sql, group);

      return (rowsAffected);
    }
  }
}
