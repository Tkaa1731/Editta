using Havit.Blazor.Components.Web;
using Pobytne.Shared.Struct;

namespace Pobytne.Client.Extensions.IDirectory
{
    public interface IDirectory
    {
        string Name { get; }
        IconBase Icon { get; }
        List<IDirectory> Subdirectories { get; }
        List<IListItem> ItemsList { get; }
        Task OnSelect();
        Task AddNew();
        IListItem GetNew();
    }
}
