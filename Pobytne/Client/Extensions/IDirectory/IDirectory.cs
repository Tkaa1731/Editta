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
        Task OnSelect(); // EventCallback onSelect
        Task OnExpanded(); // EventCallback onExpanded
        void Insert(IListItem item);
        void Update(IListItem item);
        //Task LoadData(); //LoadData from DB
        IListItem GetNew();// New IListItem for form
    }
}
