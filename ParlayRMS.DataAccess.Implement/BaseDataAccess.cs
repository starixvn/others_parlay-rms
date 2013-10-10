// --------------------------------------------------------------------------
// <copyright file="BaseDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using Fanex.Data.DNAide;
    using Fanex.Logging;

    /// <summary>
    /// Base Data Access.
    /// </summary>
    public class BaseDataAccess
    {
        private const int DefaultTimeOut = 600;

        /// <summary>
        /// Base Executing SP method.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <param name="timeOut">Time out value.</param>
        /// <returns>The result returned from executing SP.</returns>
        public DbResult ExecSp(string name, Hashtable parameters, int timeOut)
        {
            ////var setting = DbSettingProviderManager.Current.GetSetting(name);
            ////string storeName = setting.SpName;
            return DbHelper.ExecuteDbResult(name, parameters, timeOut);
        }

        /// <summary>
        /// Executes Store procedure.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The result returned from executing SP.</returns>
        public DbResult ExecSp(string name, Hashtable parameters)
        {
            return this.ExecSp(name, parameters, DefaultTimeOut);
        }

        /// <summary>
        /// Executes SP and return a DataTable.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The DataTable returned from executing SP.</returns>
        public DataTable ExecSpDataTable(string name, Hashtable parameters)
        {
            var result = this.ExecSp(name, parameters);
            if (result != null && result.ResultSet.Tables.Count > 0)
            {
                return result.ResultSet.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// Executes SP and return a DataSet.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The DataSet returned from executing SP.</returns>
        public DataSet ExecSpDataSet(string name, Hashtable parameters)
        {
            var result = this.ExecSp(name, parameters);
            return result.ResultSet;
        }

        /// <summary>
        /// Executes SP and return a HashTable.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The HashTable returned from executing SP.</returns>
        public Hashtable ExecSpHashtable(string name, Hashtable parameters)
        {
            var result = this.ExecSp(name, parameters);
            if (result != null && result.Outputs != null && result.Outputs.Count > 0)
            {
                return result.Outputs;
            }

            return null;
        }

        /// <summary>
        /// Executes SP and return a DataReader.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <param name="timeOut">Time out value.</param>
        /// <returns>The DataReader returned from executing SP.</returns>
        public IDataReader ExecSpDataReader(string name, Hashtable parameters, int timeOut)
        {
            return DbHelper.ExecuteReader(name, parameters, timeOut);
        }

        /// <summary>
        /// Executes SP and return a DataReader.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The DataReader returned from executing SP.</returns>
        public IDataReader ExecSpDataReader(string name, Hashtable parameters)
        {
            return this.ExecSpDataReader(name, parameters, DefaultTimeOut);
        }

        /// <summary>
        /// Executes SP and return a list T type objects.
        /// </summary>
        /// <typeparam name="T">Type of expected objects.</typeparam>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>List of objects returned from executing SP.</returns>
        public IEnumerable<T> Query<T>(string name, Hashtable parameters)
        {
            return DbHelper.Query<T>(name, parameters);
        }

        /// <summary>
        /// Executes SP and return a DataReader.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <param name="timeOut">Time out value.</param>
        /// <returns>The DataReader returned from executing SP.</returns>
        public IDataReader ExecSpDataReader(string name, int timeOut)
        {
            return DbHelper.ExecuteReader(name, timeOut);
        }

        /// <summary>
        /// Executes SP and return a DataReader.
        /// </summary>
        /// <param name="name">Name of the stored procedure.</param>
        /// <returns>The DataReader returned from executing SP.</returns>
        public IDataReader ExecSpDataReader(string name)
        {
            return this.ExecSpDataReader(name, DefaultTimeOut);
        }

        /// <summary>
        /// Writes Log the occurred exception.
        /// </summary>
        /// <param name="ex">Occurred exception.</param>
        /// <param name="category">Category of the exception.</param>
        protected void ProcessException(Exception ex, string category = "PARLAY_RMS")
        {
            LogService.Error(ex.Message, category, ex);

            if (ConfigurationManager.AppSettings["IsDebug"].Equals(bool.TrueString))
            {
                throw ex;
            }
        }
    }
}