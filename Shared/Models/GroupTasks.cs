using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalbox.Shared.Models
{
	public class GroupTasks : Group
	{
		public List<UserTask> Tasks { get; set; }
	}
}
