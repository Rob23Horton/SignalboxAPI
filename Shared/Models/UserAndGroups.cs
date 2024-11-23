using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Signalbox.Shared.Models;

namespace Shared.Models
{
	public class UserAndGroups : User
	{
		public List<Group> Groups { get; set; }
	}
}
