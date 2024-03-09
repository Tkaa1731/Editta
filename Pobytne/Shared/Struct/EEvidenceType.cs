using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
	public enum EEvidenceType
	{
		[Display(Name = "Prodej sluzeb a zbozi")]
		Basic = 0, 
		[Display(Name = "Evidence činností")]
		EvidenceCinnosti = 1,
		[Display(Name = "NRP")]
		NRP = 2,
		[Display(Name = "Poradna")]
		Poradna = 3,
		[Display(Name = "NZDM")]
		NZDM = 4
	}
}
