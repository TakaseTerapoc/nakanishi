using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using nakanishiWeb.Const;
using nakanishiWeb.General;
using nakanishiWeb.Utils;
using nakanishiWeb.DataAccess;

namespace nakanishiWeb
{
    public partial class SettingComplete : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_MachineMaster machineMaster;

        //:::: 取得したデータ格納するインスタンス
        public Machine machine;
        public int machineID;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> HistoryPageWords;
        public Dictionary<int, string> DetailPageWords;

        public string pageName = ConstData.SETTING_COMPLETE_PAGE_NAME;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public int limit = ConstData.HISTORY_LIMIT;
        public int pagerMax = ConstData.PAGER_MAX;
        public int pagerHalf = ConstData.PAGER_HALF;

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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.History, out HistoryPageWords);
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.Detail, out DetailPageWords);

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;

                    //:::: 表示用データの取得
                    machineMaster = new DB_MachineMaster(this._dbObj);
                    machineID = int.Parse(Session[SearchLabel.CHOOSE_MACHINE_ID].ToString());
                    machine = (Machine)Session[SearchLabel.CHOOSE_MACHINE];
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

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (_dbObj != null)
            {
                _dbObj.DatabaseDisconnect();
                _dbObj.DBClose();
            }
        }
    }
}