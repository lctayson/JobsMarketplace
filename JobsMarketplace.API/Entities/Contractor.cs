namespace JobsMarketplace.API.Entities
{
    public class Contractor
    {
        private string _name = string.Empty;
        private double _rating;

        private Contractor() { }

        public Contractor(string name, double rating = 0.0)
        {
            Name = name;
            Rating = rating;
        }

        public Contractor(int id, string name, double rating = 0.0)
        {
            Id = id;
            Name = name;
            Rating = rating;
        }

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("BusinessName is required.", nameof(value)) : value;
        }

        // Rating on a 0.0 - 5.0 scale
        public double Rating
        {
            get => _rating;
            set
            {
                if (double.IsNaN(value) || value < 0.0 || value > 5.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 0.0 and 5.0.");
                }

                _rating = value;
            }
        }
    }
}
