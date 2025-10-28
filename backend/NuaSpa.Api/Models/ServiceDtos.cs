namespace NuaSpa.Api.Models;

public record ServiceDto(int Id, string Name, int CategoryId, int DurationMinutes, decimal Price, string? Description);
public record CreateServiceDto(string Name, int CategoryId, int DurationMinutes, decimal Price, string? Description);
public record UpdateServiceDto(string Name, int CategoryId, int DurationMinutes, decimal Price, string? Description);
