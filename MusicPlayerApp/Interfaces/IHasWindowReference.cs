using Avalonia.Controls;

namespace MusicPlayerApp.Interfaces
{
    internal interface IHasWindowReference
    {
        Window? Window { get; set; }
    }
}
