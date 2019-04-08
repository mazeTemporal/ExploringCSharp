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

    [Display(Name = "Group Members")]
    public List<GroupModel> Subgroups;

    [Display(Name = "Member Of")]
    public List<GroupModel> Supergroups;

    public List<SongModel> Songs;

    [Display(Name = "Official Sites")]
    [DataType(DataType.Url)]
    public List<string> OfficialSites;

    [Display(Name = "Last Check Date")]
    [DataType(DataType.Date)]
    [Required]
    public string LastCheckDate;
  }
}
