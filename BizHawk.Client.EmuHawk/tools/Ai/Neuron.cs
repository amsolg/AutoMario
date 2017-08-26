using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizHawk.Client.EmuHawk.tools.Ai
{
	public class Neuron
	{
		//the number of inputs into the neuron
		public int InputAmount { get; set; }
		
		//the weights for each input
		public double[] Weights { get; set; }

		public Neuron(int inputsAmount)
		{
			Random r = new Random();
			//we need an additional weight for the bias hence the +1
			for (int i = 0; i < InputAmount + 1; ++i)
			{
				//set up the weights with an initial random value
				Weights[i] = r.NextDouble() * 2 - 1;
			}
		}
	}
}
