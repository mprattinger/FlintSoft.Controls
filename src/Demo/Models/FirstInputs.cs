using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class FirstInputs
{
    [Required]
    public string FirstTextInput { get; set; } = "";

    [Required]
    public DateTime FirstDate { get; set; } = DateTime.Now;
}
