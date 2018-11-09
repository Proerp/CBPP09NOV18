using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "EmployeeEditable", null, "EmployeeDeletable")
        {
        }

        public IList<Employee> SearchEmployees(string searchText)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Employee> employees = this.TotalSmartPortalEntities.Employees.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).OrderByDescending(or => or.Name).Take(20).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return employees;
        }
    }


    public class EmployeeAPIRepository : GenericAPIRepository, IEmployeeAPIRepository
    {
        public EmployeeAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetEmployeeIndexes")
        {
        }
    }
}