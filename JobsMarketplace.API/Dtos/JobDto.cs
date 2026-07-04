namespace JobsMarketplace.API.Dtos;

public record JobDto
{
    // Use nullable int for Id to allow for cases where the job is not yet created and thus has no Id assigned.
    public int? Id { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime DueDate { get; init; }

    public decimal Budget { get; init; }

    public string Description { get; init; } = string.Empty;

    public int? AcceptedById { get; init; }
}