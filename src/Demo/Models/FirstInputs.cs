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

    [Required]
    public DateTime When { get; set; } = DateTime.Now;

    [Required]
    public MatchTypes MatchType { get; set; }

    [Required]
    public bool IsAtHome { get; set; }
}

public class DropdownItem : FSDropdownBase
{
    public override string Key { get; set; } = "";

    public string Value { get; set; } = "";
}

public enum MatchTypes
{
    UNKNOWN,
    MATCH,
    CUP,
    FRENDLY
}
