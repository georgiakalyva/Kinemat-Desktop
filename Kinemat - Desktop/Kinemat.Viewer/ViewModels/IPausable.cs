using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinemat.Viewer.ViewModels
{
	internal interface IPausable
	{
		bool IsPaused { get; set; }
		void Pause();
		void Resume();
	}
}
