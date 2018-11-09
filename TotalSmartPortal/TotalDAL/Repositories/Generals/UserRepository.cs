using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Generals;


namespace TotalDAL.Repositories.Generals
{
    public class UserRepository : IUserRepository //GenericWithDetailRepository<UserReference, UserReferenceDetail>, 
    {
        //public UserRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        //    : base(totalSmartPortalEntities, "UserReferenceEditable", "UserReferenceApproved", null, "UserReferenceVoidable")
        //{
        //}
    }








    public class UserAPIRepository : GenericAPIRepository, IUserAPIRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public UserAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetUserIndexes")
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("ActiveOption", this.RepositoryBag.ContainsKey("ActiveOption") && this.RepositoryBag["ActiveOption"] != null ? this.RepositoryBag["ActiveOption"] : false), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("ActiveOption");

            return objectParameters;
        }

        public IList<TaskIndex> GetTaskIndexes()
        {
            return this.TotalSmartPortalEntities.GetTaskIndexes().ToList();
        }

        public int UserRegister(int? userID, int? organizationalUnitID, int? sameOUAccessLevel, int? sameLocationAccessLevel, int? otherOUAccessLevel)
        {
            return this.totalSmartPortalEntities.UserRegister(userID, organizationalUnitID, sameOUAccessLevel, sameLocationAccessLevel, otherOUAccessLevel);
        }

        public int UserUnregister(int? userID, string userName, string organizationalUnitName)
        {
            return this.totalSmartPortalEntities.UserUnregister(userID, userName, organizationalUnitName);
        }

        public int UserToggleVoid(int? userID, bool? inActive)
        {
            return this.totalSmartPortalEntities.UserToggleVoid(userID, inActive);
        }

        public bool UserEditable(int? userID)
        {
            return this.totalSmartPortalEntities.UserEditable(userID).FirstOrDefault() == null;
        }

        public IList<UserAccessControl> GetUserAccessControls(int? userID, int? nmvnTaskID)
        {
            return this.TotalSmartPortalEntities.GetUserAccessControls(userID, nmvnTaskID).ToList();
        }

        public int SaveUserAccessControls(int? accessControlID, int? accessLevel, bool? approvalPermitted, bool? unApprovalPermitted, bool? voidablePermitted, bool? unVoidablePermitted, bool? showDiscount)
        {
            return this.TotalSmartPortalEntities.SaveUserAccessControls(accessControlID, accessLevel, approvalPermitted, unApprovalPermitted, voidablePermitted, unVoidablePermitted, showDiscount);
        }


        public IList<UserReportControl> GetUserReportControls(int? userID)
        {
            return this.TotalSmartPortalEntities.GetUserReportControls(userID).ToList();
        }

        public int SaveUserReportControls(int? reportControlID, bool? enabled)
        {
            return this.TotalSmartPortalEntities.SaveUserReportControls(reportControlID, enabled);
        }

        public IList<LocationOrganizationalUnit> GetLocationOrganizationalUnits(int? nothing)
        {
            return this.TotalSmartPortalEntities.GetLocationOrganizationalUnits(nothing).ToList();
        }
        

    }

}
