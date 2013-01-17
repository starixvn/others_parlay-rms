namespace ParlayRMS.DataAccess.Implement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using Fanex.Data.Configuration;
    using Fanex.Data.DNAide;
    using Fanex.Logging;

    public class BaseDataAccess
    {
        private const int DEFAULT_TIME_OUT = 600;

        /// <summary>
        /// Base method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DbResult ExecSp(string name, Hashtable parameters, int timeOut)
        {
            var setting = DbSettingProviderManager.Current.GetSetting(name);
            string storeName = setting.SpName;
            return DbHelper.ExecuteDbResult(storeName, parameters, timeOut);
        }

        /// <summary>
        /// Execute Store procedure
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DbResult ExecSp(string name, Hashtable parameters)
        {
            return this.ExecSp(name, parameters, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Return DataTable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// Return DataSet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecSpDataSet(string name, Hashtable parameters)
        {
            var result = this.ExecSp(name, parameters);
            return result.ResultSet;
        }

        /// <summary>
        /// Return HashTable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// Return DataReader
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public IDataReader ExecSpDataReader(string name, Hashtable parameters, int timeOut)
        {
            return DbHelper.ExecuteReader(name, parameters, timeOut);
        }

        /// <summary>
        /// Return DataReader
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecSpDataReader(string name, Hashtable parameters)
        {
            return this.ExecSpDataReader(name, parameters, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Return list T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string name, Hashtable parameters)
        {
            return DbHelper.Query<T>(name, parameters);
        }

        /// <summary>
        /// Return DataReader
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public IDataReader ExecSpDataReader(string name, int timeOut)
        {
            return DbHelper.ExecuteReader(name, timeOut);
        }

        /// <summary>
        /// Return DataReader
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDataReader ExecSpDataReader(string name)
        {
            return this.ExecSpDataReader(name, DEFAULT_TIME_OUT);
        }

        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="category"></param>
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