using nakanishiWeb.Utils;
using nakanishiWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Diagnostics;

using nakanishiWeb.General;
using nakanishiWeb.Const;

namespace nakanishiWeb
{
    public partial class Main : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;    //ページャーのコントロール用インスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_CompanyMaster companyMaster;
        public DB_BranchMaster branchMaster;
        public DB_ModelMaster modelMaster;
        public DB_MachineMaster machineMaster;
        public DB_TypeMaster typeMaster;
        public DB_Lang dbLang;
        public SortBase sortBase;

        //:::: 取得したデータ格納するリスト
        public List<Company> clientList;    //検索ボックス用
        public List<Company> endUserList;   // 〃
        public List<Branch> adminBranchList;    // 〃
        public List<Model> modelList;    // 〃
        public List<MachineType> typeList;    // 〃
        public MachineBase searchCondition;
        public List<Machine> machineList;   //テーブル表示用

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> MainPageWords;
        public string pageName = ConstData.MAIN_PAGE_NAME;
        public string common = ConstData.COMMON;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public List<string> searchInfoList;     //検索条件を格納するリスト「検索条件リスト」
        public string searchInfo;     //検索条件リストをもとに、ページに表示するための文字列「表示用文字列」
        public int limit = ConstData.MAIN_LIMIT;     //対応ページに表示するリストの上限値
        public int pagerMax = ConstData.PAGER_MAX;
        public int pagerHalf = ConstData.PAGER_HALF;
        public int selectedEndUserID;
        public string defaultSortColKey = SearchLabel.END_USER_NAME;
        public string defaultSortCol = ConstData.machineSortColumns[SearchLabel.END_USER_NAME];
        public string defaultOrderDirection = ConstData.machineFirstOrderDirection[SearchLabel.END_USER_NAME];
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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.Main, out MainPageWords);

                    if (Funcs.IsNotNullObject(Session[SearchLabel.CHOOSE_ALERT_DATE_S]))
                    {
                        Session.Remove(SearchLabel.CHOOSE_ALERT_DATE_S);
                    }
                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;
                    int userBranchID = int.Parse(Session[SearchLabel.BRANCH_ID].ToString());
                    //Detail用のソートセッションは削除
                    Common.SessionClear_ofDetailPage();

                    //:::: 表示用データの取得
                    companyMaster = new DB_CompanyMaster(this._dbObj);
                    //companyMaster.SearchClientList(out clientList, "company_name", "ASC");
                    //companyMaster.SearchAllEndUserList(out endUserList, langID);
                    branchMaster = new DB_BranchMaster(this._dbObj);
                    //branchMaster.GetAdminBranchList(out adminBranchList, langID);
                    modelMaster = new DB_ModelMaster(this._dbObj);
                    typeMaster = new DB_TypeMaster(this._dbObj);
                    machineMaster = new DB_MachineMaster(_dbObj);
                    searchCondition = new MachineBase();
                    sortBase = new SortBase();

                    //::::: 前頁からの会社IDを取得
                    if ((Funcs.IsNotNullObject(Session[SearchLabel.SELECTED_ENDUSER_ID])) && (Funcs.IsNotNullObject(Session[SearchLabel.SELECTED_ENDUSER_NAME])) && (int.Parse(Session[SearchLabel.SELECTED_ENDUSER_ID].ToString()) != ConstData.SEARCH_ALL))
                    {
                        //selectedEndUserIDがnullでなく全件検索(-1)でもなく、selectedEndUserNameもnullでない時
                        selectedEndUserID = int.Parse(Session[SearchLabel.SELECTED_ENDUSER_ID].ToString());
                        //searchCondition.endUserCode = selectedEndUserID;
                        searchCondition.enduserName_ambi = Session[SearchLabel.SELECTED_ENDUSER_NAME].ToString();
                        Session[S_ConditionLabel.ENDUSER_NAME_AMBI] = searchCondition.enduserName_ambi;
                        pager = new PagerController(machineMaster.GetSelectMachineCount_ofEndUser(selectedEndUserID, langID), limit);
                    }
                    else
                    {
                        searchCondition.endUserCode = ConstData.SEARCH_ALL;
                        pager = new PagerController(limit, limit);
                    }

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
                    if ((Funcs.IsNotNullObject(Request.Form[SearchLabel.SEARCH_BT])) && (Request.Form[SearchLabel.SEARCH_BT] == "true"))
                    {
                        //Session.Remove(SearchLabel.SELECTED_ENDUSER_ID);
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

                        //:::: DBから製品情報を取得
                        machineMaster.SearchMachines_bySearchCondition(ref searchCondition, ref pager, out machineList, ref sortBase, langID, true);

                        string[] columns = new string[]
                        {
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_12],        // エンドユーザー名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_14],        // 製品群
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_15],        // 品名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_17],        // S/N
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_56],        // 設置日
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_19],        // 稼働時間
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_10],        // 得意先名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_20],        // 担当支店営業所
                            MainPageWords[(int)LanguageTable.MainPageStrId.Main_1],           // 最終通信時間
                            MainPageWords[(int)LanguageTable.MainPageStrId.Main_2]            // 製品年齢
                        };

                        var csvString = CsvWriter.CreateMachineListCsvText(columns, machineList);
                        var fileName = CommonWords[(int)LanguageTable.CommonPageStrId.Common_48] + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

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
                        Response.Flush();
                        Response.End();
                    }

                    //:::: ページャーからの送信かをチェック
                    if ((Request.Form[SearchLabel.PAGER] != null) && (Request.Form[SearchLabel.PAGER] != ""))
                    {
                        //ページャーに現在のページナンバーとオフセットをセット
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
                        string colKey = Funcs.GetStringIdFromValue(requestString);
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

                    //:::: DBから製品情報を取得
                    machineMaster.SearchMachines_bySearchCondition(ref searchCondition, ref pager, out machineList, ref sortBase, langID, false);
                    modelMaster.SearchModel_ofSearchCondition(out modelList, ref searchCondition, langID);
                    typeMaster.SearchTypeList_ofSearchCondition(out typeList, ref searchCondition, langID);
                    //searchConditionの値から、検索条件リストに値をセット
                    Common.SetInfoList(in searchCondition, ref searchInfoList);
                    //検索条件リストから表示用文字列を作成
                    //searchInfo = Common.MakeSearchInfoString(in searchInfoList, in CommonWords);
                    //Common.MakeCompanyDataSets(out clientDatasets, in CommonWords, in clientList);
                    //Common.MakeCompanyDataSets(out enduserDatasets, in CommonWords, in endUserList);
                    //Common.MakeBranchDataSets(out mgofficeDatasets, in CommonWords, in adminBranchList);

                    //:::: フィールドに使う値をセット
                    Common.SetIntField_bySession(ref FieldsIntList);
                    Common.SetStringField_bySession(ref FieldsStringList);
                    Common.SetDateStringField_bySession(ref FieldsDateStringList);

                    //::::: マシン選択がされたかどうか
                    if ((Request.Form[SearchLabel.CHOOSE_MACHINE_ID] != null) && (Request.Form[SearchLabel.CHOOSE_MACHINE_ID] != ConstData.EMPTY))
                    {
                        //選択されていたらセッションにマシンIDを格納して詳細ページへ
                        Session[SearchLabel.CHOOSE_MACHINE_ID] = Request.Form[SearchLabel.CHOOSE_MACHINE_ID];
                        Response.Redirect("Detail.aspx?beforePage=Main", false);
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