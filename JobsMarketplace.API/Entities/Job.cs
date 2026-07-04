namespace JobsMarketplace.API.Entities
{
    public class Job
    {
        private DateTime _startDate;
        private DateTime _dueDate;
        private decimal _budget;

        private Job() { }

        public Job(DateTime startDate, DateTime dueDate, decimal budget, string description)
        {
            StartDate = startDate;
            DueDate = dueDate;
            Budget = budget;
            Description = description ?? string.Empty;
        }

        public Job(int id, DateTime startDate, DateTime dueDate, decimal budget, string description)
            : this(startDate, dueDate, budget, description)
        {
            Id = id;
        }

        public int Id { get; set; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                ValidateDates();
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                ValidateDates();
            }
        }

        public decimal Budget
        {
            get => _budget;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Budget must be non-negative.");
                _budget = value;
            }
        }

        public string Description { get; set; } = string.Empty;

        // Who's doing the job? (The Contractor)    
        public int? AcceptedById { get; set; }

        public Contractor AcceptedBy { get; set; }

        private void ValidateDates()
        {
            if (_startDate != default && _dueDate != default && _startDate > _dueDate)
                throw new ArgumentException("StartDate must be on or before DueDate.");
        }
    }
}
