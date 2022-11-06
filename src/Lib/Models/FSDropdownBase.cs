namespace FlintSoft.Controls.Models;

public interface IFSDropdownBase
{
    string Key { get; set; }
}

public abstract class FSDropdownBase
{
    public abstract string Key { get; set; }
}