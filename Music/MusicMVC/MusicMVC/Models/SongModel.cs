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

    [ListNotEmpty]
    public List<GroupModel> Groups;

    public List<string> Sources;

    [Required]
    public bool SongHasLyrics;

    public string Lyrics;
  }
}
