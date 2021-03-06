﻿using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Employee
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Employee(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetEmployeeIndexes();

            this.EmployeeEditable();
            this.EmployeeDeletable();
        }


        private void GetEmployeeIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      EmployeeID, Code, Name, Title, Birthday, Telephone, Address, Remarks " + "\r\n";
            queryString = queryString + "       FROM        Employees " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetEmployeeIndexes", queryString);
        }

        private void EmployeeEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM Employees WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default employee, because it is related to Customers
            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("EmployeeEditable", queryArray);
        }

        private void EmployeeDeletable()
        {
            string[] queryArray = new string[1];
            queryArray[0] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM Employees WHERE EmployeeID = @EntityID "; //DON'T ALLOW TO DELETE 

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM Employees WHERE EmployeeID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM GoodsIssueDetails WHERE EmployeeID = @EntityID ";
            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("EmployeeDeletable", queryArray);
        }
    }
}
