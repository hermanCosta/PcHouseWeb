using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record FaultResponse(
    long FaultId,
    string? Code,
    string Description);

public record CreateFaultRequest(
    string? Code,
    string Description);

public record UpdateFaultRequest(
    string? Code,
    string Description);

public static class FaultMapper
{
    public static FaultResponse ToResponse(Fault fault)
    {
        return new FaultResponse(
            fault.FaultId,
            fault.Code,
            fault.Description);
    }
}
