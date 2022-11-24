namespace FlintSoft.Controls.Models;

public interface IFSDropdownBase
{
    string Key { get; set; }
}

public abstract class FSDropdownBase<T>
{
    public abstract T Key { get; set; }
}