﻿using FlintSoft.Controls.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class FirstInputs
{
    [Required]
    public string FirstTextInput { get; set; } = "";

    [Required]
    public DateTimeOffset FirstDate { get; set; } = DateTime.Now;

    [Required]
    public DropdownItem SelectedItem { get; set; } = default!;

    [Required]
    public DateTimeOffset When { get; set; } = DateTime.Now;

    [Required]
    public MatchTypes MatchType { get; set; }

    [Required]
    public bool IsAtHome { get; set; }

    [Required]
    public string Country { get; set; } = "";

    [Required]
    public string Country2 { get; set; } = "";

    [Required]
    public string City { get; set; } = "";

    [Required]
    public string Firstname { get; set; } = "";

    [Required]
    public string Lastname { get; set; } = "";

    [Required]
    public double DblValue { get; set; }
}

public class DropdownItem : FSDropdownBase<string>
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
