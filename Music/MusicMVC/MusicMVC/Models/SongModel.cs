using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicMVC.Validation;

namespace MusicMVC.Models
{
  public class SongModel
  {
    public int SongId;

    [Required(ErrorMessage = "Song must have a name")]
    public string Name;

    [Display(Name = "Contributing Groups")]
    [ListNotEmpty]
    public List<GroupModel> Groups;

    [DataType(DataType.Url)]
    public List<string> Sources;

    [Display(Name = "Has Lyrics")]
    [Required]
    public bool SongHasLyrics;

    public string Lyrics;
  }
}
