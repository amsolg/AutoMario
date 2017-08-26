using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizHawk.Client.EmuHawk.tools.Ai.Super_Mario_World
{
	public class GameState
	{
		public int[,] Q { get; set; }
		public int[,] R { get; set; }
	}
}
