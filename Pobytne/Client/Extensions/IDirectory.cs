//using Havit.Blazor.Components.Web.Bootstrap;
//using Havit.Blazor.Components.Web;
//using Pobytne.Shared.Procedural;

//namespace Pobytne.Client.Extensions
//{
//    public interface IDirectory
//    {
//        string Name { get; }
//        IconBase Icon { get; }
//        IDirectory[] Subdirectories { get; }
//        void OnSelect();
//        void AddNew();
//    }
//    public class LicenseDir : IDirectory // Add/update module
//    {
//        public LicenseDir(License license, PobytneService<Module> service) 
//        {
//            License = license;
//            _service = service;
//            Modules = Array.Empty<ModuleDir>();
//        }
//        private PobytneService<Module> _service { get;}
//        public License License { get; set; }
//        public ModuleDir[] Modules { get; set; }// 
//        public UserDir Users { get; set; }
//        public IDirectory[] Subdirectories
//        {
//            get
//            {
//                var result = Modules as IDirectory[];
//                result.Append(Users);
//                return result;
//            }
//        }

//        public string Name { get { return $"{License.NameOfOrganization}|{License.LicenseNumber}"; } }//$"{License.NameOfOrganization}|{License.LicenseNumber}";
//        public IconBase Icon { get { return BootstrapIcon.Folder; } }

//        public void AddNew()
//        {

//        }
//        public async void OnSelect()
//        {
//            if (Modules.Count() <=0)
//            {
//                var modules = await _service.GetAllAsync($"api/Module/ModulesList?licenseNumber={License.LicenseNumber}");
//                Modules = Array.Empty<ModuleDir>();
//                foreach (Module m in modules)
//                {
//                    Modules.Append(new ModuleDir(m));// TOFO: yjistit kde je ta trida kteroudam do scope
//                }
//            }
//        }
//    }
//    public class ModuleDir : IDirectory // add/update permition for user
//    {
//        public ModuleDir(Module module, PobytneService<User> service)
//        {
//            Module = module;
//            UsersOfModule = Array.Empty<User>(); ;
//            _service = service;
//        }

//        public Module Module { get; set; }
//        public User[] UsersOfModule { get; set; }
//        private PobytneService<User> _service { get; set; }

//        public string Name { get { return "JaJsemModul"; } }//Module.ModuleName;
//        public IconBase Icon { get { return BootstrapIcon.FolderPlus; } }
//        public IDirectory[] Subdirectories { get { return Array.Empty<IDirectory>(); } }
//        public void AddNew()
//        {

//        }
//        public void OnSelect() 
//        {
//        }

//    }
//    public class UserDir : IDirectory// add/update user
//    {
//        public User[] Users { get; set; }

//        public string Name { get { return "Users"; } }
//        public IconBase Icon { get { return BootstrapIcon.People; } }
//        public IDirectory[] Subdirectories { get { return Array.Empty<IDirectory>(); } }
//        public void AddNew()
//        {

//        }
//        public void OnSelect() { }
//    }
//}
