﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSmtp.Interfaces
{
	public interface IRichMailMessage
	{
		string To { get; }
		string From { get; }
		string Subject { get; }
		string Boundary { get; }
		string ContentType { get; }
	}
}
