using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalbox.Shared.Models
{
	public class Task
	{
		public int TaskId { get; set; }
		public string Name { get; set; }
		public int TypeCode { get; set; }
		public string TypeDescrition { get; set; }
	}
}
