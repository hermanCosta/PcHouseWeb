using PcHouseStore.Web.Models;

namespace PcHouseStore.Web.Services;

public class CompanyContext
{
    public CompanyResponse? CurrentCompany { get; set; }
    public bool IsAuthenticated => CurrentCompany != null;
    public long? CompanyId => CurrentCompany?.CompanyId;

    public event Action? OnCompanyChanged;

    public void SetCompany(CompanyResponse company)
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
