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
    public partial class GetEndUserList : System.Web.UI.Page
    {
        public DB_CompanyMaster companyMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public List<Company> endUserList;
        public MachineBase searchCondition;
        public char split = ConstData.VALUE_SPLIT_CHAR;
        public string parentPageName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    parentPageName = Session["pageName"].ToString();
                    searchCondition = new MachineBase();
                    string parent = Request.QueryString["parent"];
                    string parentID_string = Request.QueryString["parentID"];
                    int parentID;
                    if (parentID_string != ConstData.EMPTY)
                    {
                        parentID = Funcs.GetIdFromValue(Request.QueryString["parentID"]);
                    }
                    else
                    {
                        parentID = ConstData.SEARCH_ALL;
                    }
                    int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                    if (parent == "client")
                    {
                        searchCondition.companyID = parentID;
                    }
                    else
                    {
                        searchCondition.managementOfficeID = parentID;
                    }
                    companyMaster = new DB_CompanyMaster(this._dbObj);
                    companyMaster.SearchEndUsersForSearchBox_bySearchCondition(ref searchCondition, out endUserList, langID);
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