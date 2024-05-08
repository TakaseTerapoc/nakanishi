using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.General;
using nakanishiWeb.Utils;

namespace nakanishiWeb
{
    public partial class UserEdit : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_UserMaster DBuserMaster;

        //:::: データ取得用のインスタンス
        public PagerController pager;      //ページャーのコントロール用インスタンス

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> UserEditPageWords;
        public string pageName = ConstData.USER_EDIT_PAGE_NAME;
        public string userName;
        public string loginID;
        public TimeSpan mailStartTime;
        public TimeSpan mailEndTime;

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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.UserEdit, out UserEditPageWords);

                    if (Funcs.IsNotNullObject(Session[SearchLabel.USER_NAME]))
                    {
                        userName = Session[SearchLabel.USER_NAME].ToString();
                        loginID = Session[SearchLabel.LOGIN_ID].ToString();
                        mailStartTime = (TimeSpan)Session[SearchLabel.MAIL_START_TIME];
                        mailEndTime = (TimeSpan)Session[SearchLabel.MAIL_END_TIME];
                    }

                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.USER_SETTING_EDIT_BT])) && (Request.Form[SearchLabel.USER_SETTING_EDIT_BT] == "true"))
                    {
                        DBuserMaster = new DB_UserMaster(_dbObj);

                        string password = Request.Form[SearchLabel.USER_PASSWORD];
                        var hashpass = Funcs.HashPassword(password);
                        if (DBuserMaster.CheckPassword(loginID, hashpass))
                        {
                            string newHashPassword = hashpass;

                            var newPassword = Request.Form["newPassword1"].ToString();
                            if(string.IsNullOrEmpty(newPassword) == false)
                            {
                                // 変更後のパスワードをハッシュ化
                                newHashPassword = Funcs.HashPassword(newPassword);
                            }

                            var userId = Session[SearchLabel.USER_ID].ToString();
                            var sHour = int.Parse(Request.Form["s_timeH"]);
                            var sMin = int.Parse(Request.Form["s_timeM"]);
                            var eHour = int.Parse(Request.Form["e_timeH"]);
                            var eMin = int.Parse(Request.Form["e_timeM"]);
                            mailStartTime = new TimeSpan(sHour, sMin, 0);
                            mailEndTime = new TimeSpan(eHour, eMin, 0);

                            if (mailStartTime > mailEndTime)
                            {
                                var errMsg = UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_7];
                                Response.Write($"<script language=JavaScript> alert('{errMsg}') </script>");
                            }
                            else
                            {
                                if (DBuserMaster.UpdateUserSetting(userId, newHashPassword, mailStartTime, mailEndTime))
                                {
                                    Session[SearchLabel.MAIL_START_TIME] = mailStartTime;
                                    Session[SearchLabel.MAIL_END_TIME] = mailEndTime;

                                    Response.Redirect("UserEditComplete.aspx", false);
                                }
                            }
                        }
                        else
                        {
                            var errMsg = UserEditPageWords[(int)LanguageTable.UserEditPageStrId.UserEdit_8];
                            Response.Write($"<script language=JavaScript> alert('{errMsg}') </script>");
                        }
                    }
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

        protected void Page_UnLoad()
        {
            if (_dbObj != null)
            {
                _dbObj.DatabaseDisconnect();
                _dbObj.DBClose();
            }
        }
    }
}