using FlintSoft.Controls.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class FirstInputs
{
    [Required]
    public string FirstTextInput { get; set; } = "";

    [Required]
    public DateTime FirstDate { get; set; } = DateTime.Now;

    [Required]
    public DropdownItem SelectedItem { get; set; } = default!;
}

public class DropdownItem : FSDropdownBase
{
    public override string Key { get; set; } = "";

    public string Value { get; set; } = "";
}
