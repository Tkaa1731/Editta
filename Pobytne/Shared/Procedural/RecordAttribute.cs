using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
	[Serializable]
	[Table("S_ZaznamyVlastnosti")]
	public class RecordAttribute : ACreation, ICloneable
	{
		public int Id {  get; set; }
		public int ModuleId { get; set; } = -1;
		public ERecordType Type { get; set; }
        [Required(ErrorMessage = "Vyplňte název vlastnosti")]
		[MaxLength(50)]
		public string Name { get; set; } = string.Empty;
		[MaxLength(12)]
		public string AccountA { get; set; } = string.Empty;
		[MaxLength(12)]
        public string AccountS { get; set; } = string.Empty;
		[MaxLength(12)]
        public string CentreNumber { get; set; } = string.Empty;
		[MaxLength(12)]
        public string OrderNumber { get; set; } = string.Empty;
		[MaxLength(12)]
        public string ProjectNumber { get; set; } = string.Empty;
        public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
