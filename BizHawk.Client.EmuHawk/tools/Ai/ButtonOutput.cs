using BizHawk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizHawk.Client.EmuHawk.tools.Ai
{
	public class ButtonOutput : Neuron
	{
		public ButtonOutput(int inputsAmount, string buttonName) : base(inputsAmount)
		{
			ButtonName = buttonName;
			PressState = 0;
		}

		public string ButtonName { get; set; }
		public double PressState { get; set; }

		public double Heaviside(double value)
		{
			return Math.Pow(value, -1) * Math.Max(0, value);
		}
	}
}
