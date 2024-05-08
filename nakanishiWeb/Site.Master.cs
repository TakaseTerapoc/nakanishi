using nakanishiWeb.Utils;
using nakanishiWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nakanishiWeb.General;
using nakanishiWeb.Const;
using System.Diagnostics;

namespace nakanishiWeb
{
    public partial class Site : System.Web.UI.MasterPage
    {
        // DB
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;

        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)

        public string userName;
        public string companyName;
        public string affiliation;
        public string common = ConstData.COMMON;
        public string langCode = "ja";
        public bool isEnduser;
        public bool isClient;
        public bool isBranch;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 言語テーブル取得
            int langID = Session[SearchLabel.LANGUAGE] != null ? int.Parse(Session[SearchLabel.LANGUAGE].ToString()) : LangConst.JAPANESE_CODE;
            dbLang = new DB_Lang(_dbObj);
            dbLang.GetWordResorceCommon(langID, out CommonWords);

            isEnduser = DB_UserMaster.isEndUser;
            isClient = DB_UserMaster.isClient;
            isBranch = DB_UserMaster.isBranch;

            if (Funcs.IsNotNullObject(Session[SearchLabel.USER_NAME]))
            {
                userName = Session[SearchLabel.USER_NAME].ToString();
                companyName = Session[SearchLabel.COMPANY_NAME].ToString();
                affiliation = Session[SearchLabel.AFFILIATION].ToString();
                langCode = Session[SearchLabel.LANG_CODE].ToString();
            }
            if ((Funcs.IsNotNullObject(Request.Form["pageChange"])) && (Request.Form["pageChange"] != ConstData.EMPTY))
            {
                string value = Request.Form["pageChange"];
                Common.SessionClear_ofSelectedEndUser();
                Common.SessionClear_ofSearchCondition();
                switch (value)
                {
                    case "main":
                        Response.Redirect("Main.aspx",false);
                        break;
                    case "endUser":
                        Response.Redirect("ClientList.aspx",false);
                        break;
                    case "userEdit":
                        Response.Redirect("UserEdit.aspx", false);
                        break;
                    case "0":
                        Response.Redirect("AlertList.aspx?value=0",false);
                        break;
                    case "1":
                        Response.Redirect("AlertList.aspx?value=1",false);
                        break;
                    default:
                        Response.Redirect("Main.aspx",false);
                        break;
                }
            }
        }
    }
}