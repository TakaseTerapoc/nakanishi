using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.General;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb
{
    public partial class AlertList : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_CompanyMaster companyMaster;
        public DB_BranchMaster branchMaster;
        public DB_ModelMaster modelMaster;
        public DB_TypeMaster typeMaster;
        public DB_AlertMaster alertMaster;
        public SortBase sortBase;

        //:::: 取得したデータ格納するリスト
        public List<Company> clientList;
        public List<Branch> adminBranchList;
        public List<Company> endUserList;
        public List<Model> modelList;
        public List<MachineType> typeList;
        public List<Alert> alertTypeList; //検索条件に使用
        public List<Alert> alertLevelList;
        public List<Alert> alertList;
        public MachineBase searchCondition;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> AlertListPageWords; //このページ用の文言リスト

        public string pageName = ConstData.ALERT_PAGE_NAME;
        public string common = ConstData.COMMON;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public List<string> searchInfoList;
        public string searchInfo;
        public int limit = ConstData.ALERT_LIMIT;
        public int pagerMax = ConstData.PAGER_MAX;
        public int pagerHalf = ConstData.PAGER_HALF;
        public int headerCount = 9;
        public int titleNumber;
        public string defaultSortColKey = SearchLabel.OCCUR_TIME;
        public string defaultSortCol = ConstData.historySortColmuns[SearchLabel.OCCUR_TIME];
        public string defaultOrderDirection = ConstData.machineFirstOrderDirection[SearchLabel.OCCUR_TIME];
        public string clientDatasets;
        public string enduserDatasets;
        public string mgofficeDatasets;

        //検索BOXのselected用
        public Dictionary<string, int> FieldsIntList = new Dictionary<string, int>(Common.fieldIntValues);
        public Dictionary<string, string> FieldsStringList = new Dictionary<string, string>(Common.fieldStringValues);
        public Dictionary<string, string> FieldsDateStringList = new Dictionary<string, string>(Common.fieldDateStringValues);
        public List<string> FintKeys = Common.FintKeys; //0:mgOffice, 1:modelID, 2:machineAlert, 3:partsAlert
        public List<string> FstringKeys = Common.FstringKeys;    //0:clientName, 1:endUser, 2:typeID, 3:S/N
        public List<string> FdatestringKeys = Common.FdatestringKeys;    //0:settingS, 1:settingE, 2:alertS, 3:alertE

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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.AlertList, out AlertListPageWords);

                    //Detail用のソートセッションは削除
                    Common.SessionClear_ofDetailPage();

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;
                    int userBranchID = int.Parse(Session[SearchLabel.BRANCH_ID].ToString());

                    //:::: QueryStringがあれば続行なければMainへ(サイドメニューから遷移していればNullにはならない)
                    if (Funcs.IsNotNullObject(Request.QueryString["value"]))
                    {
                        titleNumber = int.Parse(Request.QueryString["value"]);
                        Session[SearchLabel.TITLE_NO] = titleNumber;
                    }
                    else
                    {
                        if (Funcs.IsNotNullObject(Session[SearchLabel.TITLE_NO]))
                        {
                            titleNumber = int.Parse(Session[SearchLabel.TITLE_NO].ToString());
                        }
                        else
                        {
                            Response.Redirect("Main.aspx", false);
                        }
                    }

                    //:::: 表示用データの取得
                    companyMaster = new DB_CompanyMaster(this._dbObj);
                    companyMaster.SearchClientList(out clientList, "company_id", "ASC");
                    companyMaster.SearchAllEndUserList(out endUserList, langID);
                    branchMaster = new DB_BranchMaster(this._dbObj);
                    branchMaster.GetAdminBranchList(out adminBranchList, langID);
                    modelMaster = new DB_ModelMaster(this._dbObj);
                    modelMaster.SearchModelList_All(out modelList, langID);
                    typeMaster = new DB_TypeMaster(this._dbObj);
                    typeMaster.SearchAllTypeList(out typeList, langID);
                    alertMaster = new DB_AlertMaster(_dbObj);
                    alertMaster.SearchAlertType(out alertTypeList, langID, titleNumber);
                    alertMaster.SearchAlertLevel(out alertLevelList, langID, titleNumber);
                    searchInfo = CommonWords[(int)LanguageTable.CommonPageStrId.Common_28];
                    pager = new PagerController(alertMaster.GetAllAlertCount(langID, -1), limit);
                    searchCondition = new MachineBase();
                    sortBase = new SortBase();

                    //:::: 引き継いだソート情報があればセット。なければデフォルト
                    if (Funcs.IsNotNullObject(Session[S_ConditionLabel.SORT_COL]))
                    {
                        sortBase.SetHandoverValue_Self();
                    }
                    else
                    {
                        sortBase.SetDefaultValue(defaultSortCol, defaultSortColKey, defaultOrderDirection);
                    }

                    //:::: 引き継いだページャー情報をセット
                    pager.SetHandoverValue_Self();

                    //::::: 検索ボックスからの送信かをチェック
                    if ((Request.Form[SearchLabel.SEARCH_BT] != null) && (Request.Form[SearchLabel.SEARCH_BT] == "true"))
                    {
                        //ページャーの値をdefault
                        pager.SetDefaultPageNoAndOffset();
                        //並べ替えの項目をデフォルトに戻す
                        sortBase.SetDefaultValue(defaultSortCol, defaultSortColKey, defaultOrderDirection);

                        //検索ボックスからの値をサーチコンディションにセット
                        Common.GetRefiningCondition(CommonWords, ref searchCondition);
                    }

                    // CSVダウンロードボタン押下？
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.DOWNLOAD_CSV_BT])) && (Request.Form[SearchLabel.DOWNLOAD_CSV_BT] == "true"))
                    {
                        //検索条件をセッションから取得してセット
                        searchCondition.SetSearchCondition_Self();

                        //::::: DBから製品情報を取得
                        if (titleNumber == ConstData.KIND_MACHINE_ALERT) { alertMaster.SearchAlert_bySearchCondition(out alertList, langID, ref pager, ref sortBase, in searchCondition, titleNumber, true); }
                        if (titleNumber == ConstData.KIND_PARTS_ALERT) { alertMaster.SearchAlert_bySearchCondition(out alertList, langID, ref pager, ref sortBase, in searchCondition, titleNumber, true); }

                        string[] columns = new string[]
                        {
                            AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_8],  // 発生日時
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_53],              // 解除日時
                            AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_7],  // アラートレベル
                            AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_4],  // アラート名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_12],              // エンドユーザー名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_14],              // 製品群
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_15],              // 品名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_17],              // S/N
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_56],              // 設置日
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_20],              // 担当支店営業所
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_10]               // 得意先名
                        };
                        var csvString = CsvWriter.CreateAlertListCsvText(columns, alertList);
                        var fileName = "";

                        if (titleNumber == ConstData.KIND_MACHINE_ALERT)
                        {
                            fileName = AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_1] + AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_0] + "_"
                                     + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        }
                        else
                        {
                            fileName = AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_2] + AlertListPageWords[(int)LanguageTable.AlertListPageStrId.AlertList_0] + "_"
                                     + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                        }

                        // 言語で切り替え
                        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                        if (langID == 1)   // 日本語？
                        {
                            encoding = System.Text.Encoding.GetEncoding("Shift-JIS");
                        }
                        // Fileメソッドでダウンロードするために文字列をbyteデータに変換
                        var csvData = encoding.GetBytes(csvString);

                        // CSVファイルをダウンロード
                        Response.Buffer = false;
                        Response.AddHeader("Content-Disposition", "inline;filename=" + fileName);
                        Response.ContentType = "application/octet-stream";
                        Response.BinaryWrite(csvData);
                        Response.End();
                    }

                    //:::: ページャーへ値のセット ＆ ページャーからの送信かをチェック
                    if ((Request.Form[SearchLabel.PAGER] != null) && (Request.Form[SearchLabel.PAGER] != ""))
                    {
                        pager.SetNowPageNo(int.Parse(Request.Form[SearchLabel.PAGER]));
                        pager.SetOffset();
                        //並べ替え情報を引き継ぐ
                        sortBase.SetHandoverValue_Self();
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
                            sortBase.sortColumn = ConstData.machineSortColumns[sortBase.columnsKey];
                            string orderD = Funcs.GetNameFromValue(requestString);
                            if (orderD == "default")
                            {
                                sortBase.orderDirection = ConstData.machineFirstOrderDirection[sortBase.columnsKey];
                            }
                            else
                            {
                                sortBase.orderDirection = orderD;
                            }
                        }
                    }

                    //検索条件をセッションから取得してセット
                    searchCondition.SetSearchCondition_Self();

                    //:::: ソート項目とページャー情報をセッションにをセット
                    sortBase.SessionValueSet();
                    pager.SessionValueSet();

                    //::::: DBから製品情報を取得
                    if (titleNumber == ConstData.KIND_MACHINE_ALERT) { alertMaster.SearchAlert_bySearchCondition(out alertList, langID, ref pager, ref sortBase, in searchCondition, titleNumber); }
                    if (titleNumber == ConstData.KIND_PARTS_ALERT) { alertMaster.SearchAlert_bySearchCondition(out alertList, langID, ref pager, ref sortBase, in searchCondition, titleNumber); }
                    //searchConditionの値から、検索条件リストに値をセット
                    Common.SetInfoList(in searchCondition, ref searchInfoList);
                    //検索条件リストから表示用文字列を作成
                    searchInfo = Common.MakeSearchInfoString(in searchInfoList, in CommonWords);
                    Common.MakeCompanyDataSets(out clientDatasets, in CommonWords, in clientList);
                    Common.MakeCompanyDataSets(out enduserDatasets, in CommonWords, in endUserList);
                    Common.MakeBranchDataSets(out mgofficeDatasets, in CommonWords, in adminBranchList);

                    //:::: フィールドに使う値をセット
                    Common.SetIntField_bySession(ref FieldsIntList);
                    Common.SetStringField_bySession(ref FieldsStringList);
                    Common.SetDateStringField_bySession(ref FieldsDateStringList);

                    //::::: マシン選択がされたかどうか
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.CHOOSE_MACHINE_ID])) && (Request.Form[SearchLabel.CHOOSE_MACHINE_ID] != ConstData.EMPTY))
                    {
                        //選択されていたらセッションにマシンIDを格納して詳細ページへ
                        string values = Request.Form[SearchLabel.CHOOSE_MACHINE_ID];
                        Session[SearchLabel.CHOOSE_MACHINE_ID] = Funcs.GetIdFromValue(values);
                        Session[SearchLabel.CHOOSE_ALERT_DATE_S] = Funcs.GetNameFromValue(values);
                        Response.Redirect("Detail.aspx?beforePage=AlertList", false);
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
            if(_dbObj != null)
            {
                _dbObj.DatabaseDisconnect();
                _dbObj.DBClose();
            }
        }
    }
}