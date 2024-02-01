using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Authentication
{
	public class RefreshRequest
	{
		public string RefreshToken {  get; set; } = string.Empty;
		public int UserId { get; set; }
	}
}
