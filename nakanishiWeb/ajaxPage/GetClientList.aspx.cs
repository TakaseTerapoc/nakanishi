using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb.ajaxPage
{
    public partial class GetClientList : System.Web.UI.Page
    {
        public DB_CompanyMaster companyMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public List<Company> clientList;
        public MachineBase searchCondition;
        public char split = ConstData.VALUE_SPLIT_CHAR;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try { 
                    searchCondition = new MachineBase();
                    int ID;
                    if (Request.QueryString["id"] != ConstData.EMPTY)
                    {
                        ID = Funcs.GetIdFromValue(Request.QueryString["id"]);
                    }
                    else
                    {
                        ID = ConstData.SEARCH_ALL;
                    }
                    int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                    searchCondition.managementOfficeID = ID;
                    companyMaster = new DB_CompanyMaster(this._dbObj);
                    companyMaster.SearchClient_bySearchCondition(ref searchCondition, out clientList, langID);
                }
                catch (Exception ex)
                {
                    _dbObj.errorMsg = ex.Message;
                    DB_SystemLog syslog = new DB_SystemLog(_dbObj);
                    syslog.InsertSystemLog();
                    Funcs.ExceptionEnd(ex.Message);
                }
            }
            else
            {
                Funcs.SessionTimeOutEnd();
            }
        }
    }
}