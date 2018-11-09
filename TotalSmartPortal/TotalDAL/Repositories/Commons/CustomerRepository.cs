using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }

        public IList<Customer> SearchSuppliers(string searchText)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalSmartPortalEntities.Customers.Include("EntireTerritory").Where(w => w.IsSupplier && (w.Name.Contains(searchText) || w.VATCode.Contains(searchText))).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }

        public IList<CustomerBase> GetCustomerBases(string searchText, int warehouseTaskID)
        {
            List<CustomerBase> customerBases = this.TotalSmartPortalEntities.GetCustomerBases(searchText, warehouseTaskID).ToList();

            return customerBases;
        }

        public IList<Customer> SearchCustomersByIndex(int customerCategoryID, int customerTypeID, int territoryID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalSmartPortalEntities.Customers.Where(w => w.CustomerCategoryID == customerCategoryID || w.CustomerTypeID == customerTypeID || w.TerritoryID == territoryID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }


        public IList<string> GetShippingAddress(int? customerID, string searchText)
        {
            return this.TotalSmartPortalEntities.GetShippingAddress(customerID, searchText).ToList();
        }

        public IList<string> GetAddressees(int? customerID, string searchText)
        {
            return this.TotalSmartPortalEntities.GetAddressees(customerID, searchText).ToList();
        }

        public IList<Customer> GetAllCustomers()
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = this.TotalSmartPortalEntities.Customers.Where(w => w.IsCustomer).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return customers;
        }

        public IList<Customer> GetAllSuppliers()
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Customer> suppliers = this.TotalSmartPortalEntities.Customers.Where(w => w.IsSupplier).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return suppliers;
        }

        public bool GetShowDiscount(int customerID)
        {
            if (customerID == 0 ) return false;

            bool? showDiscount = this.TotalSmartPortalEntities.GetShowDiscountByCustomer(customerID).Single();
            return showDiscount == null ? false : (bool)showDiscount;
        }

    }



    public class CustomerAPIRepository : GenericAPIRepository, ICustomerAPIRepository
    {
        public CustomerAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetCustomerIndexes")
        {
        }
    }

}

