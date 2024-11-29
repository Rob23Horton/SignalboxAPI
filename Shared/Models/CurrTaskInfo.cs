using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
	public class CurrTaskInfo
	{
		public int CurrTaskInfoId { get; set; }
		public int TaskCode { get; set; }
		public bool IsOwner { get; set; }
		public int Owner { get; set; }
		public bool Completed { get; set; }
	}
}
