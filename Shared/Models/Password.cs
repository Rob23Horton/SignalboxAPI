using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalbox.Shared.Models
{
	public class Password
	{
		public int UserCode { get; set; }
		public bool Valid { get; set; }
		public string Reason { get; set; }
	}
}
