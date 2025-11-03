using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record ProductServiceResponse(
    long ProductServiceId,
    long CompanyId,
    string Name,
    string? Category,
    decimal Price,
    int Quantity,
    int MinQuantity,
    string? Note,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsLowStock);

public record CreateProductServiceRequest(
    long CompanyId,
    string Name,
    string? Category,
    decimal Price,
    int Quantity,
    int MinQuantity,
    string? Note);

public record UpdateProductServiceRequest(
    string? Name,
    string? Category,
    decimal? Price,
    int? Quantity,
    int? MinQuantity,
    string? Note);

public static class ProductServiceMapper
{
    public static ProductServiceResponse ToResponse(ProductService productService)
    {
        return new ProductServiceResponse(
            productService.ProductServiceId,
            productService.CompanyId,
            productService.Name,
            productService.Category,
            productService.Price,
            productService.Quantity,
            productService.MinQuantity,
            productService.Note,
            productService.CreatedAt,
            productService.UpdatedAt,
            productService.IsLowStock);
    }
}
