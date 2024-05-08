using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb.ajaxPage
{
    public partial class GetAllMGOffice : System.Web.UI.Page
    {
        public List<Branch> adminBranchList;
        public DB_BranchMaster branchMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public char split = ConstData.VALUE_SPLIT_CHAR;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                    branchMaster = new DB_BranchMaster(this._dbObj);
                    branchMaster.GetAdminBranchList(out adminBranchList, langID);
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