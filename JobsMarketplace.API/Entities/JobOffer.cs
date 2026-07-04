namespace JobsMarketplace.API.Entities
{
    public class JobOffer
    {
        public int Id { get; set; }

        // Foreign Key to Job
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;

        // Foreign Key to Contractor
        public int ContractorId { get; set; }
        public Contractor Contractor { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Message { get; set; } = string.Empty;

        // Status: "Pending", "Accepted", "Rejected"
        public string Status { get; set; } = "Pending";
    }
}
