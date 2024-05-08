using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;

namespace nakanishiWeb
{
    public partial class UserEditComplete : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public string pageName = ConstData.USER_EDIT_PAGE_NAME;

        protected void Page_Load(object sender, EventArgs e)
        {
            //:::: セッションのチェック
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    // 言語テーブル取得
                    int langID = Session[SearchLabel.LANGUAGE] != null ? int.Parse(Session[SearchLabel.LANGUAGE].ToString()) : LangConst.JAPANESE_CODE;
                    dbLang = new DB_Lang(_dbObj);
                    dbLang.GetWordResorceCommon(langID, out CommonWords);

                }
                catch (System.Threading.ThreadAbortException)
                {
                    // 
                }
                catch (Exception ex)
                {
                    _dbObj.errorMsg = ex.Message;
                    DB_SystemLog syslog = new DB_SystemLog(_dbObj);
                    syslog.InsertSystemLog();
                    Funcs.ExceptionEnd($"{pageName} : {ex.Message}");
                }
            }
            else
            {
                Funcs.SessionTimeOutEnd();
            }
        }
    }
}
