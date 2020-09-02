using System.Collections.ObjectModel;

namespace Test_Project1.Syncfusion_ViewModels
{
    public class PieChart
    {
        public string Month { get; set; }

        public double Target { get; set; }

        public PieChart(string xValue, double yValue)
        {
            Month = xValue;
            Target = yValue;
        }
    }

    public class PieChartViewModel
    {
        public ObservableCollection<PieChart> Data { get; set; }

        public PieChartViewModel()
        {
            Data = new ObservableCollection<PieChart>()
            {
                new PieChart("Jan", 50),
                new PieChart("Feb", 70),
                new PieChart("Mar", 65),
                new PieChart("Apr", 57),
                new PieChart("May", 48),
            };
        }

    }

}
