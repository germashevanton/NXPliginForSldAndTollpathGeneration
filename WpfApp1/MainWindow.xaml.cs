using OxyPlot;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(double diameter, double radialDepth,
            int teethNumber, int stiffness, double damping,
            double fn, string material){

            var chartCalc = new ChartCalc(diameter, radialDepth, teethNumber, stiffness, 
                damping, fn, material);
            var lobe0 = chartCalc.SLD(0);
            var lobe1 = chartCalc.SLD(1);
            var lobe2 = chartCalc.SLD(2);
            var lobe3 = chartCalc.SLD(3);
            var lobe4 = chartCalc.SLD(4);

            var viewModel = new GraphViewModel(
                lobe0.Values.ToList(),
                lobe1.Values.ToList(),
                lobe2.Values.ToList(),
                lobe3.Values.ToList(),
                lobe4.Values.ToList(),
                lobe0.Keys.ToList());


            DataContext = viewModel;

            InitializeComponent();


        }
    }
}
