namespace JnvKmmAlumniApi.Entities
{
    public class Members
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string Batch { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public IFormFile ProfilePhoto { get; set; } = null!;
        public string? Comments { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastTs { get; set; }
        public bool IsActive { get; set; } = true;

        // Computed property, not mapped to the DB.
        public string FullName => $"{Name} {Surname}".Trim();
        public string Location { get; set; } = string.Empty;
        public string FilePath { get; internal set; } = string.Empty;
        public string FileName { get; internal set; } = string.Empty;
        public int RoleId { get; set; } = 3;
    }
}
