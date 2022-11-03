using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class FirstInputs
{
    [Required]
    public string FirstTextInput { get; set; } = "";

    [Required]
    public DateOnly FirstDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}
