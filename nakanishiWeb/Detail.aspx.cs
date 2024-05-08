using nakanishiWeb.Utils;
using nakanishiWeb.DataAccess;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nakanishiWeb.Const;
using nakanishiWeb.General;
using System.Diagnostics;

namespace nakanishiWeb
{
    public partial class Detail : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;    //ページャーのコントロール用インスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_MachineMaster machineMaster;
        public DB_AlertMaster alertMaster;
        public DB_PartsMaster partsMaster;
        public DB_Lang dbLang;

        //:::: 取得したデータ格納するインスタンス
        public Machine machine;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> DetailPageWords;
        public Dictionary<int, string> HistoryPageWords;
        public string pageName = ConstData.DETAIL_PAGE_NAME;
        public string common = ConstData.COMMON;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public string graphData="\'\'";
        public string sStartDate;
        public string DayOrMonth;
        public DateTime searchDateS;
        public DateTime searchDateE;
        public string searchDateString; //viewの表示用
        public int year;
        public int month;
        public int alertNo;
        public string beforePage;
        public string subTitle;
        MachineBase searchCondition;
        //アラートテーブル用↓↓
        public List<Alert> machineAlertList;
        public SortBase sortBase_Alert;
        public string defaultSortColKey_Alert = SearchLabel.OCCUR_TIME;
        public string defaultSortCol_Alert = ConstData.alertSortColmuns[SearchLabel.OCCUR_TIME];
        public string defaultOrderDirection_Alert = ConstData.alertFirstOrderDirection[SearchLabel.OCCUR_TIME];
        //パーツテーブル用↓↓
        public List<Parts> partsList;
        public SortBase sortBase_Parts;
        public string defaultSortColKey_Parts = SearchLabel.PARTS_NAME;
        public string defaultSortCol_Parts = ConstData.partsSortColmuns[SearchLabel.PARTS_NAME];
        public string defaultOrderDirection_Parts = ConstData.partsFirstOrderDirection[SearchLabel.PARTS_NAME];

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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.Detail, out DetailPageWords);
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.History, out HistoryPageWords);

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    //アラート日に指定があったら 
                    if (Funcs.IsNotNullObject(Session[SearchLabel.CHOOSE_ALERT_DATE_S]))
                    {
                        DateTime date = DateTime.Parse(Session[SearchLabel.CHOOSE_ALERT_DATE_S].ToString());
                        searchDateS = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        searchDateE = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    }
                    else
                    {
                        DateTime today = DateTime.Now;
                        searchDateS = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                        searchDateE = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
                    }
                    Session["pageName"] = pageName;
                    Session[GraphData_Label.GRAPH_ALERT_S] = searchDateS;
                    Session[GraphData_Label.GRAPH_ALERT_E] = searchDateE;

                    //:::: DetailPageに来た際に、他のページからの遷移であればクエリパラメーターにそのページ名がある
                    if (Funcs.IsNotNullObject(Request.QueryString[SearchLabel.BEFORE_PAGE]))
                    {
                        //クエリパラメーターに値があれば、変数とセッションにそのページ名を入れておく
                        beforePage = Request.QueryString[SearchLabel.BEFORE_PAGE].ToString();
                        Session[SearchLabel.BEFORE_PAGE] = beforePage;
                    }
                    else //HistoryPageから来た時
                    {
                        //クエリパラメーターがない場合はセッションから取得する
                        beforePage = Session[SearchLabel.BEFORE_PAGE].ToString();
                        //HistoryPageのページャー用セッションは削除する
                        Common.SesionClear_ofHistoryPage();
                    }

                    //日別・月別の選択がされてないデフォルトの状態なら「日別」に設定
                    if (Session[SearchLabel.DAY_OR_MONTH] == null)
                    {
                        DayOrMonth = "day";
                        subTitle = searchDateS.ToString("yyyy/MM/dd") + DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_30];
                    }
                    else
                    {
                        DayOrMonth = Session[SearchLabel.DAY_OR_MONTH].ToString();
                    }

                    //:::: 表示用データの取得
                    machineMaster = new DB_MachineMaster(this._dbObj);
                    alertMaster = new DB_AlertMaster(this._dbObj);
                    partsMaster = new DB_PartsMaster(this._dbObj);
                    searchCondition = new MachineBase();
                    int machineID = Session[SearchLabel.CHOOSE_MACHINE_ID] == null ? 0 : int.Parse(Session[SearchLabel.CHOOSE_MACHINE_ID].ToString());
                    machine = machineMaster.SearchMachine_one(machineID, langID);
                    searchCondition.machineID = machineID;
                    searchCondition.alertStart = searchDateS;
                    searchCondition.alertEnd = searchDateE;
                    Session[SearchLabel.CHOOSE_MACHINE] = machine;
                    //wordList.Add($"{pageName}_{wordList.Count}", Common.historyPageWords["history_1"]);
                    sortBase_Alert = new SortBase();
                    sortBase_Parts = new SortBase();

                    if (Funcs.IsNotNullObject(Session[S_ConditionLabel.ALERT_SORT_COL]))
                    {
                        sortBase_Alert.sortColumn = Session[S_ConditionLabel.ALERT_SORT_COL].ToString();
                        sortBase_Alert.columnsKey = Session[S_ConditionLabel.ALERT_COL_KEY].ToString();
                        sortBase_Alert.orderDirection = Session[S_ConditionLabel.ALERT_ORDER_DIRECTION].ToString();
                    }
                    else
                    {
                        sortBase_Alert.SetDefaultValue(defaultSortCol_Alert, defaultSortColKey_Alert, defaultOrderDirection_Alert);
                    }
                    if (Funcs.IsNotNullObject(Session[S_ConditionLabel.PARTS_SORT_COL]))
                    {
                        sortBase_Parts.sortColumn = Session[S_ConditionLabel.PARTS_SORT_COL].ToString();
                        sortBase_Parts.columnsKey = Session[S_ConditionLabel.PARTS_COLMUN_KEY].ToString();
                        sortBase_Parts.orderDirection = Session[S_ConditionLabel.PARTS_ORDER_DIRECTION].ToString();
                    }
                    else
                    {
                        sortBase_Parts.SetDefaultValue(defaultSortCol_Parts,defaultSortColKey_Parts,defaultOrderDirection_Parts);
                    }

                    //:::: 部品交換
                    if (Funcs.IsNotNullObject(Request.Form[SearchLabel.EXCHANGE]) && (Request.Form[SearchLabel.EXCHANGE]) != ConstData.EMPTY)
                    {
                        //partsIDからPartsをSearchOne()
                        int partsID = int.Parse(Request.Form[SearchLabel.EXCHANGE].ToString());
                        Parts parts;
                        partsMaster.SearchPartsOne(partsID, machineID, out parts);
                        //パーツマスターの情報アップデート
                        partsMaster.UpdateExchangeCount(in parts);
                        //部品交換履歴テーブルにインサート
                        partsMaster.InsertExchangeInformation(in parts);
                    }

                    //:::: 並べ替えの指定があったかどうか
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.SORT_CHANGE])) && (Request.Form[SearchLabel.SORT_CHANGE] != ConstData.EMPTY))
                    {
                        string requestString = Request.Form[SearchLabel.SORT_CHANGE];
                        string colKey = Funcs.GetStringIdFromValue(requestString);  //ASC or DESC
                        string tableKind = Request.Form["tableKind"];
                        if (colKey != ConstData.EMPTY)
                        {//ソートカラム名が空文字でなければ
                            string columnsKey;
                            string orderDirection;
                            if (tableKind == "alert") //アラートテーブル
                            {
                                columnsKey = Funcs.GetStringIdFromValue(requestString);
                                Session[S_ConditionLabel.ALERT_COL_KEY] = columnsKey;
                                Session[S_ConditionLabel.ALERT_SORT_COL] = ConstData.alertSortColmuns[columnsKey];
                                orderDirection = Funcs.GetNameFromValue(requestString);
                                if (orderDirection == "default"){
                                    Session[S_ConditionLabel.ALERT_ORDER_DIRECTION] = ConstData.alertFirstOrderDirection[columnsKey];
                                }else{
                                    Session[S_ConditionLabel.ALERT_ORDER_DIRECTION] = orderDirection;
                                }
                            }
                            else//パーツテーブル
                            {
                                columnsKey = Funcs.GetStringIdFromValue(requestString);
                                Session[S_ConditionLabel.PARTS_COLMUN_KEY] = columnsKey;
                                Session[S_ConditionLabel.PARTS_SORT_COL] = ConstData.partsSortColmuns[columnsKey];
                                orderDirection = Funcs.GetNameFromValue(requestString);
                                if (orderDirection == "default"){
                                    Session[S_ConditionLabel.PARTS_ORDER_DIRECTION] = ConstData.partsFirstOrderDirection[columnsKey];
                                }else{
                                    Session[S_ConditionLabel.PARTS_ORDER_DIRECTION] = orderDirection;
                                }
                            }
                        }
                    }

                    //:::: 日時の変更かどうか
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.DAY_OR_MONTH])) && (Request.Form[SearchLabel.DAY_OR_MONTH] != ConstData.EMPTY))
                    {
                        string[] values = Request.Form[SearchLabel.DAY_OR_MONTH].Split(split);
                        if (values[values.Length - 1] == "day")    //日別検索
                        {
                            Session[S_ConditionLabel.DAY_OR_MONTH] = "day";
                            DayOrMonth = "day";
                            searchDateS = new DateTime(int.Parse(values[0]), int.Parse(values[1]) + 1, int.Parse(values[2]), 0, 0, 0);
                            searchDateE = new DateTime(int.Parse(values[0]), int.Parse(values[1]) + 1, int.Parse(values[2]), 23, 59, 59);
                            subTitle = searchDateS.ToString("yyyy/MM/dd") + DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_30];
                        }
                        else  //月別検索
                        {
                            Session[S_ConditionLabel.DAY_OR_MONTH] = "month";
                            DayOrMonth = "month";
                            year = int.Parse(values[0]);
                            month = int.Parse(values[1]);
                            subTitle = $"{year}/{month}{DetailPageWords[(int)LanguageTable.DetailPageStrId.Detail_30]}";
                        }
                    }

                    //:::: 日にち変更ボタン(「前の日」)
                    if (Funcs.IsNotNullObject(Request.Form["before"]))
                    {
                        DateTime date = DateTime.Parse(Session[SearchLabel.CHOOSE_ALERT_DATE_S].ToString());
                        searchDateS = date.AddDays(-1);
                        searchDateE = date.AddDays(-1);
                    }
                    //:::: 日にち変更ボタン(「次の日」)
                    if (Funcs.IsNotNullObject(Request.Form["next"]))
                    {
                        DateTime date = DateTime.Parse(Session[SearchLabel.CHOOSE_ALERT_DATE_S].ToString());
                        searchDateS = date.AddDays(1);
                        searchDateE = date.AddDays(1);
                    }
                    Session[SearchLabel.CHOOSE_ALERT_DATE_S] = searchDateS;
                    Session[SearchLabel.CHOOSE_ALERT_DATE_E] = searchDateE;

                    //検索条件をセッションから取得してセット
                    searchCondition.SetSearchCondition_Self();
                    searchCondition.alertStart = searchDateS;
                    searchCondition.alertEnd = searchDateE;
                    searchDateString = Common.GetStringDate(searchDateS);

                    //::::: DBから製品情報を取得
                    alertMaster.SearchAlert_bySearchCondition(out machineAlertList, langID, ref pager, ref sortBase_Alert, in searchCondition, ConstData.KIND_MACHINE_ALERT);
                    partsMaster.SearchParts_ofMachine(out partsList, langID, machineID, ref sortBase_Parts);
                    string time = CommonWords[(int)LanguageTable.CommonPageStrId.Common_52];
                    string count = CommonWords[(int)LanguageTable.CommonPageStrId.Common_51];
                    //回数と時間を表示用にメイキング
                    for (int i = 0; i < partsList.Count; i++)
                    {
                        if (partsList[i].exchangeGuideTimeString != ConstData.EMPTY) { partsList[i].exchangeGuideTimeString = $"[{time}]{partsList[i].exchangeGuideTimeString}"; }
                        if (partsList[i].operateHourString != ConstData.EMPTY) { partsList[i].operateHourString = $"[{time}]{partsList[i].operateHourString}"; }
                        if (partsList[i].remainingOperateHourString != ConstData.EMPTY) { partsList[i].remainingOperateHourString = $"[{time}]{partsList[i].remainingOperateHourString}"; }
                        if (partsList[i].exchangeGuideCountString != ConstData.EMPTY) { partsList[i].exchangeGuideCountString = $"{partsList[i].exchangeGuideCountString}{count}"; }
                        if (partsList[i].operateCountString != ConstData.EMPTY) { partsList[i].operateCountString = $"{partsList[i].operateCountString}{count}";}
                        //残り時間に関しては、DB_PartsMasterにて「OVER」の表示を追加
                        if (partsList[i].remainingOperateCountString != ConstData.EMPTY) {
                            if (partsList[i].remainingOperateCountString.Contains("-")){
                                partsList[i].remainingOperateCountString = $"{partsList[i].remainingOperateCountString.Replace("-", "")}{count} OVER";
                            }else{
                                partsList[i].remainingOperateCountString = $"{partsList[i].remainingOperateCountString}{count}";
                            }
                        }
                    }
                    //回数と時間のメイキングここまで
                    MakeDayGraphData(in machineAlertList);

                    //:::: テーブルからの送信かどうか
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.ALERT_NO])) && (Request.Form[SearchLabel.ALERT_NO] != ConstData.EMPTY))
                    {
                        alertNo = int.Parse(Request.Form[SearchLabel.ALERT_NO]);
                        switch (DayOrMonth)
                        {
                            case "day":
                                if (Funcs.IsNotNullObject(machineAlertList))
                                {
                                    MakeDayGraphData(in machineAlertList);
                                }
                                else
                                {
                                    graphData = "\'\'";
                                }
                                break;
                            case "month":
                                MakeMonthGraphData();
                                break;
                        }
                    }
                    else
                    {
                        //:::: グラフ用の文字列形成
                        if (Funcs.IsNotNullObject(machineAlertList))
                        {
                            MakeDayGraphData(in machineAlertList);
                        }
                        else
                        {
                            graphData = "\'\'";
                        }
                    }
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

        /// <summary>
        /// SortBaseの値をセッションもしくはデフォルトにセットして、新しい値をセッションに格納する
        /// </summary>
        /// <param name="kind">sortBaseアラート用かパーツ用かの文字列</param>
        /// <param name="sortBase">sortBaseインスタンス</param>
        public void SetSortPropaties(string kind,ref SortBase sortBase) {
            switch (kind)
            {
                case "parts":
                    sortBase.sortColumn = Funcs.IsNotNullObject(Session[S_ConditionLabel.PARTS_SORT_COL]) ? Session[S_ConditionLabel.PARTS_SORT_COL].ToString() : defaultSortCol_Parts;
                    sortBase.columnsKey = Funcs.IsNotNullObject(Session[S_ConditionLabel.PARTS_COLMUN_KEY]) ? Session[S_ConditionLabel.PARTS_COLMUN_KEY].ToString() : defaultSortColKey_Parts;
                    sortBase.orderDirection = Funcs.IsNotNullObject(Session[S_ConditionLabel.PARTS_ORDER_DIRECTION]) ? Session[S_ConditionLabel.PARTS_ORDER_DIRECTION].ToString() : defaultOrderDirection_Parts;
                    Session[S_ConditionLabel.PARTS_SORT_COL] = sortBase.sortColumn;
                    Session[S_ConditionLabel.PARTS_COLMUN_KEY] = sortBase.columnsKey;
                    Session[S_ConditionLabel.PARTS_ORDER_DIRECTION] = sortBase.orderDirection;
                    break;
                case "alert":
                    sortBase.sortColumn = Funcs.IsNotNullObject(Session[S_ConditionLabel.ALERT_SORT_COL]) ? Session[S_ConditionLabel.ALERT_SORT_COL].ToString() : defaultSortCol_Alert;
                    sortBase.columnsKey = Funcs.IsNotNullObject(Session[S_ConditionLabel.ALERT_COL_KEY]) ? Session[S_ConditionLabel.ALERT_COL_KEY].ToString() : defaultSortColKey_Alert;
                    sortBase.orderDirection = Funcs.IsNotNullObject(Session[S_ConditionLabel.ALERT_ORDER_DIRECTION]) ? Session[S_ConditionLabel.ALERT_ORDER_DIRECTION].ToString() : defaultOrderDirection_Alert;
                    Session[S_ConditionLabel.ALERT_SORT_COL] = sortBase.sortColumn;
                    Session[S_ConditionLabel.ALERT_COL_KEY] = sortBase.columnsKey;
                    Session[S_ConditionLabel.ALERT_ORDER_DIRECTION] = sortBase.orderDirection;
                    sortBase.SessionValueSet();
                    break;
            }
        }

        /// <summary>
        /// 月別グラフを描画するためのデータ成型メソッド
        /// </summary>
        public void MakeMonthGraphData() { }

        /// <summary>
        /// 日別グラフを描画するためのデータ成型メソッド
        /// </summary>
        public void MakeDayGraphData(in List<Alert> machineAlertList)
        {
            if (machineAlertList.Count != 0)
            {
                graphData = "[";
                Dictionary<int, int> t_tool = new Dictionary<int, int>();
                int count = 1;
                int gPOS = 0;
                for (int i = 0; i < machineAlertList.Count; i++)
                {
                    if (i != 0) { graphData += ","; }
                    gPOS = count;
                    count++;
                    bool workFlag = false;
                    if (machineAlertList[i].isNowAlert)
                    {
                        machineAlertList[i].releaseTime = DateTime.Now;
                        workFlag = true;
                    }
                    graphData += $"{{key: {1},";
                    graphData += $"name: '',";
                    graphData += $"start: new Date(\'{machineAlertList[i].occurTime.ToString("yyyy/MM/dd HH:mm:ss")}\'),";
                    graphData += $"end: new Date(\'{machineAlertList[i].releaseTime.ToString("yyyy/MM/dd HH:mm:ss")}\'),";
                    graphData += $"lavel: '',";
                    graphData += $"flag: '{workFlag}',";
                    graphData += $"id: \'rect_{machineAlertList[i].alertID}\'";
                    graphData += "}";
                }
                graphData += "];";
            }
            else
            {
                graphData = $"[{{key: {1},name:'',start: {0},end: {0}}}]";
            }
            sStartDate = searchDateS.ToString("yyyy/MM/dd");
        }

    }
}