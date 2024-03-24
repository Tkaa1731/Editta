using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
	public abstract class AReport : ACreation
	{
		public int ReportId { get; set; }
		public string ReportName { get; set; } = string.Empty;
		public int ReportChild {  get; set; }
		public int ReportAdult { get; set; }
		public int ReportQuantity { get; set; }
		public float ReportPrice {  get; set; }
		public int AccountA { get; set; }
		public int AccountS { get; set; }
	}
}
