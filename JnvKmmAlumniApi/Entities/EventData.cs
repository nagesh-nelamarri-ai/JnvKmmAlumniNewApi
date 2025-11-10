namespace JnvKmmAlumniApi.Entities
{
    public class EventData
    {
        public int? Id { get; set; } // Primary key
        public string Title { get; set; } = string.Empty;
        //public string Subtitle { get; set; } = string.Empty;

        public string? FilePath { get; set; }
        public string? FileName { get; set; }

        public IFormFile File { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? EventDateTime { get; set; } = DateTime.MinValue;
        public string Location { get; set; } = string.Empty;
    }
}
