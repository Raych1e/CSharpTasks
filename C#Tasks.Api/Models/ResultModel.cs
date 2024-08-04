namespace C_Tasks.Api.Models
{
    public class ResultModel
    {
        public string Result { get; set; } = string.Empty;
        
        public List<string>? Unique {  get; set; }

        public string LongSubstring { get; set; } = string.Empty;

        public string SortedString { get; set; } = string.Empty;

        public string ResultWithoutSymbol { get; set; } = string.Empty;
    }
}
