namespace ParlayRMS.DataAccess.Implement.Members
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Interface.Members;
    using ParlayRMS.DTO.Members;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;
    using ParlayRMS.DTO;

    public class MembersDataAccess : BaseDataAccess, IMembersDataAccess
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="username"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<RMS_CustomerSel> RMS_CustomerSel(string username, int level)
        {
            var result = new List<RMS_CustomerSel>();

            try
            {
                using (var db = new ObjectDb("RMS_CustomerSel"))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        db.AddInParameter("username", DbType.String, username);
                    }

                    if (level != -1)
                    {
                        db.AddInParameter("level", DbType.Int32, level);
                    }

                    int counter = 1;
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_CustomerSel
                            {
                                No = counter++,
                                Custid = reader.ToInt32("CustId"),
                                Username = reader.ToString("UserName"),
                                Level = reader.ToInt32("Level")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="level"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_CustomerIns(string userName, int level, int adminId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("username", userName);
                param.Add("level", level);
                param.Add("ip", ip);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_CustomerIns", param);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="level"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_CustomerUpd(int custId, int level, int adminId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("custid", custId);
                param.Add("level", level);
                param.Add("ip", ip);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_CustomerUpd", param);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="following"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("custid", custId);
                param.Add("monitored", following);
                param.Add("ip", ip);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_CustomerUpdStatus", param);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public bool RMS_CustomerCheckExisting(int custId)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("custid", custId);
                param.Add("isexists", 0);

                var r = ExecSp("RMS_CustomerCheckExisting", param);
                if (r.IsValid())
                {
                    return DbConverter.ToBoolean(r.Outputs["isexists"]);
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return false;
        }
    }
}