namespace NuaSpa.Api.Models;

public record ServiceCategoryDto(int Id, string Name);
public record CreateServiceCategoryDto(string Name);
public record UpdateServiceCategoryDto(string Name);
