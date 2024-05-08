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
    public partial class GetMachineModelList : System.Web.UI.Page
    {
        public DB_ModelMaster modelMaster;
        public MachineBase searchCondition;
        public DataAccessObject _dbObj = new DataAccessObject();
        public Dictionary<int,string> typeIokList;
        public List<Model> modelList;
        public char split = ConstData.VALUE_SPLIT_CHAR;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    modelMaster = new DB_ModelMaster(this._dbObj);
                    searchCondition = new MachineBase();
                    int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                    string companyID = Funcs.GetStringIdFromValue(Request.QueryString[SearchLabel.COMPANY_ID]);
                    string parent = Request.QueryString["parent"];
                    if (parent == "client")
                    {
                        searchCondition.companyID = Funcs.GetIntFromStringValue(companyID);
                    }
                    else if (parent == "enduser")
                    {
                        searchCondition.endUserCode = Funcs.GetIntFromStringValue(companyID);
                    }
                    else
                    {
                        searchCondition.managementOfficeID = Funcs.GetIntFromStringValue(companyID);
                    }
                    modelMaster.SearchModel_ofSearchCondition(out modelList, ref searchCondition, langID);
                }
                catch(Exception ex)
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