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
    public partial class ClientList : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;      //ページャーのコントロール用インスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_Lang dbLang;
        public DB_CompanyMaster companyMaster;
        public DB_BranchMaster branchMaster;
        public SortBase sortBase;

        //:::: 取得したデータ格納するリスト
        public List<Model> modelList;
        public List<Company> clientList;   //検索ボックスに表示するための、顧客全件のリスト
        public List<Company> endUserList;    //検索ボックス用エンドユーザーリスト
        public List<Company> searchEndUserList;     //テーブル表示に使用する、会社のリスト。絞り込み結果などはこちらに格納。
        public List<Branch> adminBranchList;    //自社の支店や工場など
        public MachineBase searchCondition;

        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<int, string> CommonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<int, string> ClientListPageWords;
        public string pageName = ConstData.CLIENT_PAGE_NAME;
        public string common = ConstData.COMMON;
        public char split = ConstData.VALUE_SPLIT_CHAR;
        public List<string> searchInfoList;
        public string searchInfo;
        public int limit = ConstData.CLIENT_LIMIT;
        public int pagerMax = ConstData.PAGER_MAX;
        public int pagerHalf = ConstData.PAGER_HALF;
        public int selectedEndUserID;
        public string defaultSortColKey = SearchLabel.END_USER_NAME;
        public string defaultSortCol = ConstData.companySortColumns[SearchLabel.END_USER_NAME];
        public string defaultOrderDirection = ConstData.companyFirstOrderDirection[SearchLabel.END_USER_NAME];
        public string clientDatasets;
        public string enduserDatasets;
        public string mgofficeDatasets;

        //検索BOXのselected用
        public Dictionary<string, string> FieldsStringList = new Dictionary<string, string>(Common.fieldStringValues);
        public List<string> FstringKeys = Common.FstringKeys;    //0:clientName, 1:endUser, 2:typeID, 3:S/N

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
                    dbLang.GetWordResorce(langID, (int)LanguageTable.PageId.ClientList, out ClientListPageWords);

                    int userTimeDiff = int.Parse(Session[SearchLabel.USER_TIME_DIFF].ToString());
                    this._dbObj.serverTimeDiff = userTimeDiff;
                    Session["pageName"] = pageName;
                    int userBranchID = int.Parse(Session[SearchLabel.BRANCH_ID].ToString());

                    //:::: 表示用データの取得
                    companyMaster = new DB_CompanyMaster(_dbObj);
                    companyMaster.SearchClientList(out clientList, "company_name", defaultOrderDirection);
                    companyMaster.SearchAllEndUserList(out endUserList, langID);
                    branchMaster = new DB_BranchMaster(_dbObj);
                    branchMaster.GetAdminBranchList(out adminBranchList, langID);
                    searchEndUserList = new List<Company>();
                    pager = new PagerController(companyMaster.GetAllEndUserCount(langID), limit);
                    searchCondition = new MachineBase();
                    sortBase = new SortBase();
                    FstringKeys = FieldsStringList.Keys.ToList();

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

                        // DBからエンドユーザー情報を取得
                        companyMaster.SearchEndUsers_bySearchCondition(ref searchCondition, ref pager, out searchEndUserList, ref sortBase, langID, true);
                        string[] columns = new string[]
                        {
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_12],    // エンドユーザー名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_10],    // 得意先名
                            CommonWords[(int)LanguageTable.CommonPageStrId.Common_20]     // 担当支店営業所
                        };

                        var csvString = CsvWriter.CreateClientListCsvText(columns, searchEndUserList);
                        var fileName = ClientListPageWords[(int)LanguageTable.ClientListPageStrId.ClientList_0] + "_"
                                     + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

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
                        string colKey = Funcs.GetNameFromValue(requestString);
                        if (colKey != ConstData.EMPTY)
                        {
                            sortBase.columnsKey = Funcs.GetStringIdFromValue(requestString);
                            sortBase.sortColumn = ConstData.companySortColumns[sortBase.columnsKey];
                            string orderD = Funcs.GetNameFromValue(requestString);
                            if (orderD == "default")
                            {
                                sortBase.orderDirection = ConstData.companyFirstOrderDirection[sortBase.columnsKey];
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

                    //DBからエンドユーザー情報を取得
                    companyMaster.SearchEndUsers_bySearchCondition(ref searchCondition, ref pager, out searchEndUserList, ref sortBase, langID, false);
                    //searchConditionの値から、検索条件リストに値をセット
                    Common.SetInfoList(in searchCondition, ref searchInfoList);
                    //::::: 表示用検索条件を文字列に
                    searchInfo = Common.MakeSearchInfoString(in searchInfoList, in CommonWords);
                    Common.MakeCompanyDataSets(out clientDatasets, in CommonWords, in clientList);
                    Common.MakeCompanyDataSets(out enduserDatasets, in CommonWords, in endUserList);
                    Common.MakeBranchDataSets(out mgofficeDatasets, in CommonWords, in adminBranchList);

                    //:::: フィールドに使う値をセット
                    Common.SetStringField_bySession(ref FieldsStringList);

                    //::::: エンドユーザー選択がされたかどうか
                    if ((Request.Form[SearchLabel.COMPANY_ID] != null) && (Request.Form[SearchLabel.COMPANY_ID] != ConstData.EMPTY))
                    {
                        //選択されていたらセッションにエンドユーザーIDを格納して詳細ページへ
                        int selectedCompanyID = Funcs.GetIdFromValue(Request.Form[SearchLabel.COMPANY_ID]);
                        Session["selectedEndUserName"] = Funcs.GetNameFromValue(Request.Form[SearchLabel.COMPANY_ID]);
                        Session[SearchLabel.SELECTED_ENDUSER_ID] = selectedCompanyID;
                        //ページャー情報とソート情報を含む検索条件はクリア
                        Common.SessionClear_ofSearchCondition();
                        Response.Redirect($"Main.aspx", false);
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