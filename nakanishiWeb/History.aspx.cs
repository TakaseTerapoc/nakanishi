using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.General;
using nakanishiWeb.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb
{
    public partial class History : System.Web.UI.Page
    {

        //:::: データ取得用のインスタンス
        public PagerController pager;
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_MachineMaster machineMaster;
        public DB_AlertMaster alertMaster;
        public MachineBase searchCondition;

        //:::: 取得したデータ格納するインスタンス
        public Machine machine;
        public int machineID;

        //:::: 取得したデータ格納するリスト
        public List<Alert> alertList;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> HistoryPageWords;
        public string pageName = ConstData.HISTORY_PAGE_NAME;
        public string common = ConstData.COMMON;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public int limit = ConstData.HISTORY_LIMIT;
        public int pagerMax = ConstData.PAGER_MAX;
        public int pagerHalf = ConstData.PAGER_HALF;
        public SortBase sortBase;
        public string defaultSortColKey = SearchLabel.OCCUR_TIME;
        public string defaultSortCol = ConstData.historySortColmuns[SearchLabel.OCCUR_TIME];
        public string defaultOrderDirection = ConstData.historyFirstOrderDirection[SearchLabel.OCCUR_TIME];

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

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;

                    //:::: 表示用データの取得
                    machineMaster = new DB_MachineMaster(this._dbObj);
                    machineID = int.Parse(Session[SearchLabel.CHOOSE_MACHINE_ID].ToString());
                    machine = (Machine)Session[SearchLabel.CHOOSE_MACHINE];
                    alertMaster = new DB_AlertMaster(this._dbObj);
                    sortBase = new SortBase();
                    //ソートのデフォルトをセット(アラート発生日時の昇順)
                    sortBase.SetDefaultValue(defaultSortCol, defaultSortColKey, defaultOrderDirection);
                    searchCondition = new MachineBase();
                    searchCondition.machineID = machineID;
                    //Detail用のソートセッションは削除
                    Common.SessionClear_ofDetailPage();

                    //:::: ページャーへ値のセット ＆ ページャーからの送信かをチェック
                    pager = new PagerController(alertMaster.GetAllAlertCount(langID, machineID), limit);

                    //:::: 引き継いだページャー情報をセット
                    if (Session[SearchLabel.HISTORY_PAGE_NO_TEMP] != null ) { pager.nowPageNo = int.Parse(Session[SearchLabel.HISTORY_PAGE_NO_TEMP].ToString()); }
                    if (Session[SearchLabel.HISTORY_PAGE_OFFSET_TEMP] != null) { pager.offset = int.Parse(Session[SearchLabel.HISTORY_PAGE_OFFSET_TEMP].ToString()); }

                    //:::: ページャーからの送信かをチェック
                    if ((Request.Form[SearchLabel.PAGER] != null) && (Request.Form[SearchLabel.PAGER] != ""))
                    {
                        pager.SetNowPageNo(int.Parse(Request.Form[SearchLabel.PAGER]));
                        pager.SetOffset();
                        Session[SearchLabel.HISTORY_PAGE_NO_TEMP] = pager.nowPageNo;
                        Session[SearchLabel.HISTORY_PAGE_OFFSET_TEMP] = pager.offset;
                        //ページャーが押された時は、並べ替え情報を引き継ぐ
                        if (Funcs.IsNotNullObject(Session[S_ConditionLabel.SORT_COL]))
                        {
                            sortBase.sortColumn = Session[S_ConditionLabel.HISTORY_SORT_COL].ToString();
                            sortBase.columnsKey = Session[S_ConditionLabel.HISTORY_COL_KEY].ToString();
                            sortBase.orderDirection = Session[S_ConditionLabel.HISTORY_ORDER_DIRECTION].ToString();
                        }
                    }

                    //:::: 並べ替えの指定があったかどうか
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.SORT_CHANGE])) && (Request.Form[SearchLabel.SORT_CHANGE] != ConstData.EMPTY))
                    {
                        //並び替え時はページャーの値をdefault
                        pager.SetDefaultPageNoAndOffset();
                        string requestString = Request.Form[SearchLabel.SORT_CHANGE];
                        string colKey = Funcs.GetNameFromValue(requestString);
                        if (colKey != ConstData.EMPTY)
                        {
                            sortBase.columnsKey = Funcs.GetStringIdFromValue(requestString);
                            sortBase.sortColumn = ConstData.historySortColmuns[sortBase.columnsKey];
                            string orderD = Funcs.GetNameFromValue(requestString);
                            if (orderD == "default")
                            {
                                sortBase.orderDirection = ConstData.historyFirstOrderDirection[sortBase.columnsKey];
                            }
                            else
                            {
                                sortBase.orderDirection = orderD;
                            }
                        }
                    }
                    //:::: ソート項目とページャー情報をセッションにをセット
                    Session[S_ConditionLabel.HISTORY_SORT_COL] = sortBase.sortColumn;
                    Session[S_ConditionLabel.HISTORY_COL_KEY] = sortBase.columnsKey;
                    Session[S_ConditionLabel.HISTORY_ORDER_DIRECTION] = sortBase.orderDirection;
                    Session[SearchLabel.HISTORY_PAGE_NO_TEMP] = pager.nowPageNo;
                    Session[SearchLabel.HISTORY_PAGE_OFFSET_TEMP] = pager.offset;

                    alertMaster.SearchAlert_bySearchCondition(out alertList, langID, ref pager, ref sortBase, in searchCondition, ConstData.SEARCH_ALL);
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

        protected void Page_UnLoad(object sender,EventArgs e)
        {
            if(_dbObj != null)
            {
                _dbObj.DatabaseDisconnect();
                _dbObj.DBClose();
            }
        }
    }
}