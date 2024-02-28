using Havit.Blazor.Components.Web;
using Pobytne.Shared.Struct;

namespace Pobytne.Client.Extensions.IDirectory
{
    public interface IDirectory
    {
        int Id { get; }
        string Name { get; }
        IconBase Icon { get; }
        List<IDirectory> SubDirectories { get; }
        List<IListItem> ItemsList { get; }
        Task OnSelect();
        Task OnExpanded();
        Task AddNew();
        IListItem GetNew();
    }
}
