using Microsoft.Win32;
using System.IO;
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
			if (!double.TryParse(txtTime.Text, out double t) || !double.TryParse(txtSpeed.Text, out double s) || t < 0 || s < 0)
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

		private void Mentes_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Szövegfájl (*.txt)|*.txt";

			if (saveFileDialog.ShowDialog() == true)
			{
				using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
				{
					foreach (var m in measurements)
					{
						sw.WriteLine(m.toFileString());
					}
					MessageBox.Show("Sikeres mentés!");
				}
			}
		}

		private void Betoltes_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Szövegfájl (*.txt)|*.txt";
			if (openFileDialog.ShowDialog() == true)
			{
				measurements.Clear();

				var lines = File.ReadAllLines(openFileDialog.FileName, Encoding.UTF8);
				foreach (var line in lines)
				{
					var parts = line.Split(';');
					if (parts.Length == 2 && double.TryParse(parts[0], out double t) && double.TryParse(parts[1], out double s))
					{
						measurements.Add(new Measurement(t, s));
					}
				}

				RefreshList();
				MessageBox.Show("Sikeres betöltés!");
			}
		}

		private void Kilepes_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (listBox.SelectedIndex == -1)
			{
				MessageBox.Show("Nincs kiválasztva mérés!");
				return;
			}

			var sorted = measurements.OrderBy(m => m.Time).ToList();
			var selected = sorted[listBox.SelectedIndex];

			measurements.Remove(selected);

			/*
			foreach (var m in measurements)
			{
				if(m.ToString() == listBox.SelectedItem.ToString())
				{
					measurements.Remove(m);
					break;
				}
			}*/

			RefreshList();
		}

		private void btnDiagram_Click(object sender, RoutedEventArgs e)
		{
			if (measurements.Count < 2)
			{
				MessageBox.Show("Nincs elég mérés a diagramhoz!");
				return;
			}

			//össztáv számítása (trapéz módszer)
			var sorted = measurements.OrderBy(m => m.Time).ToList();
			double total = 0;
			for (int i = 0; i < sorted.Count - 1; i++)
			{
				//idő különbség
				double dt = sorted[i + 1].Time - sorted[i].Time;

				//átlagsebesség
				double avg = (sorted[i + 1].Speed + sorted[i].Speed) / 2;

				//részterület hozzáadása
				total += dt * avg;
			}

			DiagramWindow diagramWindow = new DiagramWindow(sorted, total);
			diagramWindow.ShowDialog();
		}
	}
}