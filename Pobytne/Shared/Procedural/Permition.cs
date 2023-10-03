using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Opravneni")]
    public class Permition : ICreation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string PermitionString { get; set; } = string.Empty;
        [Editable(false)]
        public string ModuleName { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;

        public bool CheckPemition(string quaery)
        {
            return true;
        }
    }
}
//NazevOpravneni.Add(DataUtil.EKlicDleNabidky.RoleUzivatele, "role uživatele")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Aplikace, "nastavení aplikace")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Licence, "seznam licencí")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.LoginUser, "seznam uživatelů aplikace")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Modul, "seznam modulů")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Opravneni, "seznam oprávnění")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.ZaznamyVlastnosti, "seznam účetní spefikace")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.TypPlatby, "typy plateb")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Zaznamy, "seznam záznamů")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.PohybyEvidence, "sklad zboží")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.PohybyPokladna, "vedlejší pokladna")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Uzivatele, "seznam klientů")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Evidence, "evidence záznamů")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Permanentky, "permanentky")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.SouhrnyEvidence, "souhrny evidence")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.SouhrnyPokladny, "souhrny druhů plateb")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.SouhrnyPermanentky, "souhrny permanentky")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.SouhrnyDohodaZprava, "souhrny dohody - zprávy")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.SouhrnyEvidenceSklad, "souhrny stavu zásob")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.Dohody, "dohody vpp")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.DohodyOsoby, "dohody - osoby")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.DohodyPrilohy, "dohody - přílohy")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.DohodyObdobi, "dohody dle období")
//                NazevOpravneni.Add(DataUtil.EKlicDleNabidky.DohodyZpravy, "dohody - zprávy")
