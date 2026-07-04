namespace JobsMarketplace.API.Dtos;

public record CreateJobOfferDto(int JobId, int ContractorId, decimal Amount, string Message);

public record JobOfferDto(int Id, int JobId, int ContractorId, decimal Amount, string Message, string Status);