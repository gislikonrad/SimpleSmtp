using SimpleSmtp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSmtp.Interfaces
{
	public interface IHaveAContentType
	{
		ContentType ContentType { get; }
	}
}
