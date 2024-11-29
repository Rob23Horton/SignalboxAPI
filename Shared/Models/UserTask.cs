using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalbox.Shared.Models
{
	public class UserTask
	{
		public int TaskId { get; set; }
		public string Name { get; set; }
		public int GroupCode {  get; set; }
		public int TaskTypeCode { get; set; }
		public string Description { get; set; }
	}
}
