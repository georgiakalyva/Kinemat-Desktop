using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinemat.IO.Helpers
{
	public static class StringHelpers
	{
		public static string RemoveFirstSlash(this string source)
		{
			if (source[0] != '/')
				return source;

			return source.Remove(0, 1);
		}
	}
}
