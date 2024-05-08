using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb.suggestPage
{
    public partial class CompanySuggest : System.Web.UI.Page
    {
        public DB_CompanyMaster companyMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public List<Company> companyList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    string queyWord = Request.QueryString["word"];
                    companyMaster = new DB_CompanyMaster(this._dbObj);
                    if ((queyWord != ConstData.EMPTY) && (Utils.Validate.CheckInputData(queyWord)))
                    {
                        companyMaster.SearchClientListFromName(queyWord, out companyList, "company_id", "ASC");
                    }
                    else if (queyWord == ConstData.EMPTY)
                    {
                        companyList = new List<Company>();
                    }
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

        public void PrintJsonData()
        {
            if (Funcs.IsNotNullObject(companyList))
            {
                System.Web.HttpContext hc = System.Web.HttpContext.Current;
                string json = "[";
                for (int i=0; i<companyList.Count; i++)
                {
                    if(i != companyList.Count - 1)//最後の要素でなければ
                    {
                        json += $"\"{companyList[i].companyName}\",";
                    }
                    else
                    {
                        json += $"\"{companyList[i].companyName}\"";
                    }
                }
                json += "]";
                hc.Response.Write(json);
            }
        }
    }
}