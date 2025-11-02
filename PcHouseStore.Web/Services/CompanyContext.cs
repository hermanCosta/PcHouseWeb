using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class CompanyContext
{
    public Company? CurrentCompany { get; set; }
    public bool IsAuthenticated => CurrentCompany != null;
    public long? CompanyId => CurrentCompany?.CompanyId;

    public event Action? OnCompanyChanged;

    public void SetCompany(Company company)
    {
        CurrentCompany = company;
        OnCompanyChanged?.Invoke();
    }

    public void ClearCompany()
    {
        CurrentCompany = null;
        OnCompanyChanged?.Invoke();
    }
}
