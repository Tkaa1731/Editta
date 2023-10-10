using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Procedural;
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
        void AddNew(IListItem newItem);
        IListItem GetNew();
    }
}
