using TotalBase.Enums;

namespace TotalCore.Services
{
    public interface IBaseService
    {
        int UserID { get; set; }
        int LocationID { get; }

        GlobalEnums.NmvnTaskID NmvnTaskID { get; }
        int GetModuleID(ref int moduleDetailID);

        GlobalEnums.AccessLevel GetAccessLevel();
        GlobalEnums.AccessLevel GetAccessLevel(int? organizationalUnitID);

        bool GetApprovalPermitted();
        bool GetApprovalPermitted(int? organizationalUnitID);

        bool GetUnApprovalPermitted();
        bool GetUnApprovalPermitted(int? organizationalUnitID);

        bool GetVoidablePermitted();
        bool GetVoidablePermitted(int? organizationalUnitID);

        bool GetUnVoidablePermitted();
        bool GetUnVoidablePermitted(int? organizationalUnitID);
        
        bool GetShowDiscount();

        bool GetShowListedPrice(int? priceCategoryID);
        bool GetShowListedGrossPrice(int? priceCategoryID);

    }
}
