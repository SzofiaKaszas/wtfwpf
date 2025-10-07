using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtfwpf
{
	public class Measurement
	{
		public double Time { get; set; }
		public double Speed { get; set; }

		public Measurement(double time, double speed)
		{
			Time = time;
			Speed = speed;
		}

		public override string ToString()
		{
			return $"{Time} s, {Speed} m/s";
		}

		public string toFileString()
		{
			return $"{Time};{Speed}";
		}
	}
}
