using System.Text.Json.Serialization;

namespace RelatedKeyword.Models.Chart
{
    public class PieChartModel
    {
        public string Title { get; set; }
        public string SeriesJsonData { get; set; }
        public List<PieChartSeriesDataModel> SeriesData { get; set; }
    }

    public class PieChartSeriesDataModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("y")]
        public decimal Y { get; set; }
    }
}
