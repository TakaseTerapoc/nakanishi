using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class AlertMailSetting : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_MachineMaster machineMaster;
        public DB_AlertMaster alertMaster;
        public DB_PartsMaster partsMaster;
        public DB_SendMailUser mailUserMaster;

        //:::: 取得したデータ格納するインスタンス
        public Machine machine;
        public int machineID;

        //:::: 取得したデータ格納するリスト
        public List<AlertKind> alertKindList;
        public List<PartsKind> partsKindList;
        public List<int> registeredAlertNoList;
        public List<int> registeredPartsIdList;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> AlertListWords;
        public Dictionary<int, string> HistoryPageWords;
        public Dictionary<int, string> DetailPageWords;
        public string pageName = ConstData.HISTORY_PAGE_NAME;
        public string common = ConstData.COMMON;
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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.AlertList, out AlertListWords);
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.History, out HistoryPageWords);
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.Detail, out DetailPageWords);

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;

                    //:::: 表示用データの取得
                    machineMaster = new DB_MachineMaster(this._dbObj);
                    machineID = int.Parse(Session[SearchLabel.CHOOSE_MACHINE_ID].ToString());
                    machine = (Machine)Session[SearchLabel.CHOOSE_MACHINE];
                    alertMaster = new DB_AlertMaster(this._dbObj);
                    partsMaster = new DB_PartsMaster(this._dbObj);
                    mailUserMaster = new DB_SendMailUser(this._dbObj);

                    //Detail用のソートセッションは削除
                    Common.SessionClear_ofDetailPage();

                    // 機器のアラート種類を取得
                    alertMaster.SearchAlertKindList(out alertKindList, langID);
                    partsMaster.SearchPartsKindList(out partsKindList, machineID, langID);
                    var userID = (int)Session[SearchLabel.USER_ID];

                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.USER_SETTING_EDIT_BT])) && (Request.Form[SearchLabel.USER_SETTING_EDIT_BT] == "true"))
                    {
                        List<AlertKind> newAlertKindList = new List<AlertKind>();
                        if (Request.Form["alertCheck[]"] != null)
                        {
                            string[] alertCheckedList = Request.Form["alertCheck[]"].ToString().Split(',');
                            for (int i = 0; i < alertKindList.Count; ++i)
                            {
                                if (alertCheckedList.Contains(i.ToString()))
                                {
                                    newAlertKindList.Add(alertKindList[i]);
                                }
                            }
                        }
                        List<PartsKind> newPartsKindList = new List<PartsKind>();
                        if (Request.Form["partsCheck[]"] != null)
                        {
                            string[] partsCheckedList = Request.Form["partsCheck[]"].ToString().Split(',');
                            for (int i = 0; i < partsKindList.Count; ++i)
                            {
                                if (partsCheckedList.Contains(i.ToString()))
                                {
                                    newPartsKindList.Add(partsKindList[i]);
                                }
                            }
                        }

                        // 登録済みをいったん削除
                        mailUserMaster.DeleteUserMailSetting(userID, machineID, 0);
                        if (newAlertKindList.Count > 0 || newPartsKindList.Count > 0)
                        {
                            // チェック項目をすべて登録
                            mailUserMaster.InsertUserMailSetting(userID, machineID, 0, newAlertKindList, newPartsKindList);
                        }

                        Response.Redirect("SettingComplete.aspx", false);
                    }

                    // 登録済みアラート取得
                    mailUserMaster.GetUserMailRegisterdList(userID, machineID, 0, out registeredAlertNoList, out registeredPartsIdList);
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