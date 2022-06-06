using System.Text.Json.Serialization;

namespace RelatedKeyword.Models.Chart
{
    public class StackedColumnDataModel
    {
        public string Title { get; set; }
        public List<string> xAxisCategories { get; set; }
        public string xAxisCategoriesJsonData { get; set; }
        public string SeriesJsonData { get; set; }
        public List<StackedColumnSeriesModel> SeriesData { get; set; }
    }

    public class StackedColumnSeriesModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("data")]
        public List<int> Data { get; set; }
    }
}
