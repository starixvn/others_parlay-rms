namespace ParlayRMS.DataAccess
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data.DNAide;

    public class BaseDataAccess
    {
        private const int DefaultTimeOut = 600;

        public DbResult ExecSp(string name, Hashtable parameters, int timeOut)
        {
            return DbHelper.ExecuteDbResult(name, parameters, timeOut);
        }

        public DbResult ExecSp(string name, Hashtable parameters)
        {
            return ExecSp(name, parameters, DefaultTimeOut);
        }

        public DataTable ExecSpDataTable(string name, Hashtable parameters)
        {
            var result = ExecSp(name, parameters);
            if (result != null && result.ResultSet.Tables.Count > 0)
            {
                return result.ResultSet.Tables[0];
            }

            return null;
        }

        public DataSet ExecSpDataSet(string name, Hashtable parameters)
        {
            var result = ExecSp(name, parameters);
            return result.ResultSet;
        }

        public Hashtable ExecSpHashtable(string name, Hashtable parameters)
        {
            var result = ExecSp(name, parameters);
            if (result != null && result.Outputs != null && result.Outputs.Count > 0)
            {
                return result.Outputs;
            }

            return null;
        }

        public IDataReader ExecSpDataReader(string name, Hashtable parameters, int timeOut)
        {
            return DbHelper.ExecuteReader(name, parameters, timeOut);
        }

        public IDataReader ExecSpDataReader(string name, Hashtable parameters)
        {
            return ExecSpDataReader(name, parameters, DefaultTimeOut);
        }

        public IEnumerable<T> Query<T>(string name, Hashtable parameters)
        {
            return DbHelper.Query<T>(name, parameters);
        }

        public IDataReader ExecSpDataReader(string name, int timeOut)
        {
            return DbHelper.ExecuteReader(name, timeOut);
        }

        public IDataReader ExecSpDataReader(string name)
        {
            return ExecSpDataReader(name, DefaultTimeOut);
        }
    }
}
