using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wtfwpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//mérés lista
		List<Measurement> measurements = new List<Measurement>();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void RefreshList()
		{
			listBox.Items.Clear();

			//rendezés idő szerint majd kiírás
			foreach (var m in measurements.OrderBy(m => m.Time))
			{
				listBox.Items.Add(m.ToString());
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			//ellenőrzések
			if(!double.TryParse(txtTime.Text, out double t) || !double.TryParse(txtSpeed.Text, out double s) || t < 0 || s < 0)
			{
				MessageBox.Show("Hibás adat bevitel!");
				return;
			}

			//ha már van ilyen időpontú mérés akkor azt ne adjuk hozzá
			if (measurements.Any(x => x.Time == t))
			{
				MessageBox.Show("Ilyen időpontú mérés már van!");
				return;
			}

			//új mérés hozzáadása
			measurements.Add(new Measurement(t, s));
			RefreshList();

			//mezők ürítésa
			txtTime.Clear();
			txtSpeed.Clear();
		}
	}
}