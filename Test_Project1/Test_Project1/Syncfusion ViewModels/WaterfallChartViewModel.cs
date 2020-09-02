using System.Collections.ObjectModel;

namespace Test_Project1.Syncfusion_ViewModels
{
    public class WaterfallModel
    {
        public string Category { get; set; }

        public double Value { get; set; }

        public bool IsSummary { get; set; }

        public WaterfallModel(string category, double value, bool isSummary)
        {
            Category = category;
            Value = value;
            IsSummary = isSummary;
        }
    }
    public class WaterfallChartViewModel
    {
        public ObservableCollection<WaterfallModel> Data { get; set; }

        public WaterfallChartViewModel()
        {
            Data = new ObservableCollection<WaterfallModel>()
            {
                new WaterfallModel("Sales", 145, false),
                new WaterfallModel("Staff", -65, false),
                new WaterfallModel("Balance", 58, true),
                new WaterfallModel("Others", 12, false),
                new WaterfallModel("Tax", -30, false),
                new WaterfallModel("Profit", 45, true),
                new WaterfallModel("Inventory", -12, false),
                new WaterfallModel("Marketing", -25, false),
                new WaterfallModel("Net Income", 25, true),
            };
        }
    }
}