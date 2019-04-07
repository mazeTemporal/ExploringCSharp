using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicMVC.Validation;

namespace MusicMVC.Models
{
  public class GroupModel
  {
    public int GroupId;

    [ListNotEmpty]
    public List<string> Names;

    public List<GroupModel> Subgroups;

    public List<GroupModel> Supergroups;

    public List<SongModel> Songs;

    [Url]
    public List<string> OfficialSites;

    [Required]
    public string LastCheckDate;
  }
}
