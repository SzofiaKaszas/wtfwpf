using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wtfwpf
{
	/// <summary>
	/// Interaction logic for DiagramWindow.xaml
	/// </summary>
	public partial class DiagramWindow : Window
	{
		private List<Measurement> data;
		private double totalDistance;
		public DiagramWindow(List<Measurement> data, double totalDistance)
		{
			InitializeComponent();
			this.data = data;
			this.totalDistance = totalDistance;


		}
	}
}
