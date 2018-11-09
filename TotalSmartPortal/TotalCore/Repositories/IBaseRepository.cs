using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

using TotalBase.Enums;
using TotalModel.Models;

namespace TotalCore.Repositories
{
    public interface IBaseRepository
    {
        TotalSmartPortalEntities TotalSmartPortalEntities { get; }

        bool IsDirty();

        Dictionary<string, object> RepositoryBag { get; set; }

        int GetModuleID(GlobalEnums.NmvnTaskID nmvnTaskID, int userID, ref int moduleDetailID);

        ICollection<TElement> ExecuteFunction<TElement>(string functionName, params ObjectParameter[] parameters);
        int ExecuteFunction(string functionName, params ObjectParameter[] parameters);
        int ExecuteStoreCommand(string commandText, params Object[] parameters);


        T GetEntity<T>(params Expression<Func<T, object>>[] includes) where T : class;
        T GetEntity<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class;
        T GetEntity<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        T GetEntity<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;


        ICollection<T> GetEntities<T>(params Expression<Func<T, object>>[] includes) where T : class;
        ICollection<T> GetEntities<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class;
        ICollection<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;
        ICollection<T> GetEntities<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;


        String GetSystemInfos();
        String GetSystemInfos(bool secureEncoding);

        bool SystemInfoValidate();
        bool VersionValidate(bool bothKeyID);

        bool AutoUpdates(bool restoreProcedures);
    }
}
