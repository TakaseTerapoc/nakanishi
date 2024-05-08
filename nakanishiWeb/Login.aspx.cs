using Microsoft.Web.WebPages.OAuth;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Const;
using nakanishiWeb.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nakanishiWeb.General;

namespace nakanishiWeb
{
    public partial class Login : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_UserMaster DBuserMaster;
        public DB_Lang dbLang;

        //:::: 取得したデータ格納するリスト
        public LoginData loginData;
        public List<LanguageData> langList;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> LoginPageWords;
        public string pageName = ConstData.LOGIN_PAGE_NAME;
        public int langID;
        public string errorMsg="";
        public bool isLangChange = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session["pageName"] = pageName;
            //:::: 表示用データの取得
            dbLang = new DB_Lang(_dbObj);

           //:::: 言語変更かどうかチェックしてフラグと取得するリストに反映
            isLangChange = Request.Form["langChange"] == "on" ? true : false;
            langID = Request.Form[SearchLabel.LANGUAGE]== null ? LangConst.JAPANESE_CODE : int.Parse(Request.Form["motherTongue"]);
            dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.Login , out LoginPageWords);
            dbLang.GetLangList(out langList);

            //:::: 言語の変更でなければ DBでユーザーチェック
            if (!isLangChange)
            {
                if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.USER_ID])) || (Funcs.IsNotNullObject(Request.Form[SearchLabel.USER_PASSWORD])))
                {
                    DBuserMaster = new DB_UserMaster(_dbObj);
                    string userID = Request.Form[SearchLabel.USER_ID];
                    string password = Request.Form[SearchLabel.USER_PASSWORD];
                    //:::: 入力値に不正な値が入っていなければ、DBに接続
                    if ((Utils.Validate.CheckInputData(userID)) && (Utils.Validate.CheckInputData(password)))
                    {
                        var hashpass = Funcs.HashPassword(password);
                        //:::: ユーザーが登録されていたら、セッションに各値をセット
                        if (DBuserMaster.IsLoginUser(userID, hashpass, out loginData))
                        {
                            Session[SearchLabel.USER_ID] = loginData.userID;
                            Session[SearchLabel.COMPANY_ID] = loginData.companyID;
                            Session[SearchLabel.COMPANY_NAME] = loginData.companyName;
                            Session[SearchLabel.BRANCH_ID] = loginData.branchID;
                            Session[SearchLabel.BRANCH_NAME] = loginData.branchName;
                            Session[SearchLabel.AFFILIATION] = loginData.affiliation;
                            Session[SearchLabel.USER_TIME_DIFF] = loginData.timeDiff;
                            Session[SearchLabel.USER_NAME] = loginData.userName;
                            Session[SearchLabel.USER_KIND] = loginData.kind;
                            Session[SearchLabel.LANGUAGE] = loginData.langID;
                            Session[SearchLabel.LANG_CODE] = loginData.langCode;
                            Session[SearchLabel.COMPANY_KIND] = loginData.companyKind;
                            Session[SearchLabel.LOGIN_ID] = loginData.loginID;
                            Session[SearchLabel.MAIL_START_TIME] = loginData.mailStartTime;
                            Session[SearchLabel.MAIL_END_TIME] = loginData.mailEndTime;
                        }
                        //:::: セッションにユーザーIDがなかったらエラー、あればMainへ遷移。
                        if ((Session[SearchLabel.USER_ID] == null) || (int.Parse(Session[SearchLabel.USER_ID].ToString()) == ConstData.DEFAULT_USER_ID))
                        {
                            errorMsg = LoginPageWords[(int)LanguageTable.LoginPageStrId.Login_4];
                        }
                        else
                        {
                            Response.Redirect("Main.aspx", false);
                        }
                    }
                    else
                    {
                        //errorMsg = "使用できない文字が含まれています";
                        errorMsg = LoginPageWords[(int)LanguageTable.LoginPageStrId.Login_4];
                    }
                }
            }
        }

        public void Page_Unload()
        {
            if(_dbObj != null)
            {
                _dbObj.DatabaseDisconnect();
                _dbObj.DBClose();
            }
        }

    }
}