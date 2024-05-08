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
    public partial class GetTypeList : System.Web.UI.Page
    {
        public DB_TypeMaster typeMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public List<MachineType> typeList;
        public char split = ConstData.VALUE_SPLIT_CHAR;
        public MachineBase searchCondition;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                searchCondition = new MachineBase();
                int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                string parent = Request.QueryString["parent"];
                string companyID_string = Request.QueryString[SearchLabel.COMPANY_ID];
                int companyID;
                string modelID_string = Request.QueryString[SearchLabel.MODEL_ID];
                int modelID;
                if (Funcs.IsNotNullObject(companyID_string))
                {
                    companyID = int.Parse(companyID_string);
                }
                else
                {
                    companyID = ConstData.SEARCH_ALL;
                }
                if (Funcs.IsNotNullObject(modelID_string))
                {
                    modelID = Funcs.GetIdFromValue(modelID_string);
                }
                else
                {
                    modelID = ConstData.SEARCH_ALL;
                }
                searchCondition.modelID = modelID;
                typeMaster = new DB_TypeMaster(this._dbObj);
                switch (parent)
                {
                    case "enduser":
                        searchCondition.endUserCode = companyID;
                        break;
                    case "client":
                        searchCondition.companyID = companyID;
                        break;
                    case "MGOffice":
                        searchCondition.managementOfficeID = companyID;
                        break;
                    default:
                        break;
                }
                typeMaster.SearchTypeList_ofSearchCondition(out typeList, ref searchCondition, langID);
            }
            catch (Exception ex)
            {
                _dbObj.errorMsg = ex.Message;
                DB_SystemLog syslog = new DB_SystemLog(_dbObj);
                syslog.InsertSystemLog();
                Funcs.ExceptionEnd(ex.Message);
            }
        }
    }
}