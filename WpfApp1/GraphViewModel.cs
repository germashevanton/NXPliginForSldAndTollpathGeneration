using OxyPlot;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp1
{
    public class GraphViewModel : BindableBase
    {
        public GraphViewModel(List<double> lobe0, List<double> lobe1, List<double> lobe2, List<double> lobe3, List<double> lobe4, List<double> blim)
        {
            Title = "Stability Lobe Diagram";
            Points0 = new List<DataPoint>();
            for (int i = 0; i < lobe0.Count; i++)
            {
                Points0.Add(new DataPoint(lobe0[i], blim[i]));
            }
            YMax = blim.Min() * 10;
            Points1 = new List<DataPoint>();
            for (int i = 0; i < lobe0.Count; i++)
            {
                Points1.Add(new DataPoint(lobe1[i], blim[i]));
            }
            Points2 = new List<DataPoint>();
            for (int i = 0; i < lobe0.Count; i++)
            {
                Points2.Add(new DataPoint(lobe2[i], blim[i]));
            }
            Points3 = new List<DataPoint>();
            for (int i = 0; i < lobe0.Count; i++)
            {
                Points3.Add(new DataPoint(lobe3[i], blim[i]));
            }
            Points4 = new List<DataPoint>();
            for (int i = 0; i < lobe0.Count; i++)
            {
                Points4.Add(new DataPoint(lobe4[i], blim[i]));
            }

        }

        public string Title { get; private set; }
        public double YMax { get; private set; }


        public IList<DataPoint> Points0 { get; private set; }
        public IList<DataPoint> Points1 { get; private set; }
        public IList<DataPoint> Points2 { get; private set; }
        public IList<DataPoint> Points3 { get; private set; }
        public IList<DataPoint> Points4 { get; private set; }

    }
}
