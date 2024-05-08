using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace nakanishiWeb.General
{
    public class Common
    {

        /// <summary>
        /// 検索ボックスのselect要素にselectedをつけるために各ページ.aspxがコピーして使用
        /// </summary>
        public static Dictionary<string, int> fieldIntValues = new Dictionary<string, int>()
        {
            { S_ConditionLabel.MANAGEMENT_OFFICE_ID,ConstData.SEARCH_ALL},
            { S_ConditionLabel.MODEL_ID,ConstData.SEARCH_ALL},
            { S_ConditionLabel.MACHINE_ALERT,ConstData.SEARCH_ALL},
            { S_ConditionLabel.PARTS_ALERT,ConstData.SEARCH_ALL},
            {S_ConditionLabel.ALERT_NO,ConstData.SEARCH_ALL },
            {S_ConditionLabel.ALERT_LEVEL,ConstData.SEARCH_ALL }
        };
        public static Dictionary<string, string> fieldStringValues = new Dictionary<string, string>()
        {
            { S_ConditionLabel.CLIENT_NAME_AMBI,ConstData.EMPTY },
            { S_ConditionLabel.ENDUSER_NAME_AMBI,ConstData.EMPTY },
            { S_ConditionLabel.TYPE_ID,ConstData.EMPTY },
            { S_ConditionLabel.SERIAL_NUMBER,ConstData.EMPTY},
            {S_ConditionLabel.MGO_AMBI,ConstData.EMPTY }
        };
        public static Dictionary<string, string> fieldDateStringValues = new Dictionary<string, string>()
        {
            { S_ConditionLabel.SETTING_DATE_S,ConstData.EMPTY },
            { S_ConditionLabel.SETTING_DATE_E,ConstData.EMPTY },
            { S_ConditionLabel.ALERT_S,ConstData.EMPTY },
            { S_ConditionLabel.ALERT_E,ConstData.EMPTY }
        };
        public static List<string> FintKeys = fieldIntValues.Keys.ToList(); //0:mgOffice, 1:modelID, 2:machineAlert, 3:partsAlert
        public static List<string> FstringKeys = fieldStringValues.Keys.ToList();    //0:clientName, 1:endUser, 2:typeID, 3:S/N, 4:MGOffice
        public static List<string> FdatestringKeys = fieldDateStringValues.Keys.ToList();    //0:settingS, 1:settingE, 2:alertS, 3:alertE

        /// <summary>
        /// int型のフィールド値をセッションからセットする
        /// </summary>
        /// <param name="fieldIntValue"><string,int>のDictionary</param>
        public static void SetIntField_bySession(ref Dictionary<string, int> fieldIntValue)
        {
            List<string> keys = new List<string>(fieldIntValue.Keys);
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            foreach (string key in keys)
            {
                if ((hc.Session[key] != null) && (hc.Session[key].ToString() != ConstData.EMPTY))
                {
                    fieldIntValue[key] = int.Parse(hc.Session[key].ToString());
                }
                else { continue; }
            }
        }

        /// <summary>
        /// string型のフィールド値をセッションからセットする
        /// </summary>
        /// <param name="fieldStringValue"></param>
        public static void SetStringField_bySession(ref Dictionary<string, string> fieldStringValue)
        {
            List<string> keys = new List<string>(fieldStringValue.Keys);
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            foreach (string key in keys)
            {
                if ((hc.Session[key] != null) && (hc.Session[key].ToString() != ConstData.EMPTY))
                {
                    if (hc.Session[key].ToString() != ConstData.SEARCH_ALL.ToString())
                    {
                        fieldStringValue[key] = hc.Session[key].ToString();
                        if ((key == S_ConditionLabel.CLIENT_NAME_AMBI) || (key == S_ConditionLabel.ENDUSER_NAME_AMBI) 
                            || (key == S_ConditionLabel.SERIAL_NUMBER) || (key == S_ConditionLabel.MGO_AMBI))
                        {
                            fieldStringValue[key] = fieldStringValue[key].Replace("%", "?");
                        }
                    }
                    else
                    {
                        fieldStringValue[key] = ConstData.EMPTY;
                    }
                }
                else { continue; }
            }
        }

        /// <summary>
        /// 日付(string型)のフィールド値をセッションからセットする
        /// </summary>
        /// <param name="fieldDateStringValue"></param>
        public static void SetDateStringField_bySession(ref Dictionary<string, string> fieldDateStringValue)
        {
            List<string> keys = new List<string>(fieldDateStringValue.Keys);
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            foreach (string key in keys)
            {
                if ((hc.Session[key] != null) && (hc.Session[key].ToString() != ConstData.EMPTY))
                {
                    DateTime day = (DateTime)hc.Session[key];
                    if (day.ToString("yyyy/MM/dd") != ConstData.DEFAULT_DATE) 
                    {
                        fieldDateStringValue[key] = day.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        fieldDateStringValue[key] = ConstData.EMPTY;
                    }
                }
                else { continue; }
            }
        }

        /// <summary>
        /// JSに渡す用のブランチリスト文字列の作成(担当支店営業所)
        /// </summary>
        /// <param name="datasets"></param>
        /// <param name="branchList"></param>
        public static void MakeBranchDataSets(out string datasets, in Dictionary<int, string> commonWords, in List<Branch>branchList)
        {
            datasets = commonWords[(int)LanguageTable.CommonPageStrId.Common_29]+" *";
            for(int i=0; i < branchList.Count; i++)
            {
                datasets += branchList[i].branchName;
                if (i != branchList.Count-1) { datasets += "*"; }
            }
        }

        /// <summary>
        /// JSに渡す用のカンパニーリスト文字列の作成
        /// </summary>
        /// <param name="datasets"></param>
        /// <param name="companyList"></param>
        public static void MakeCompanyDataSets(out string datasets, in Dictionary<int, string> commonWords, in List<Company>companyList)
        {
            datasets = commonWords[(int)LanguageTable.CommonPageStrId.Common_29]+" *";
            for(int i=0; i < companyList.Count; i++)
            {
                datasets += companyList[i].companyName;
                if (i != companyList.Count-1) { datasets += "*"; }
            }
        }

        /// <summary>
        /// DateTimeインスタンスの値を文字列で返す
        /// </summary>
        /// <param name="someday">文字列にしたいDateTimeインスタンス</param>
        /// <returns>引数を文字列<にしたもの/returns>
        public static string GetStringDate(DateTime someday)
        {
            string result = $"{someday.Year}-{AddZero(someday.Month)}-{AddZero(someday.Day)}";
            return result;
        }

        public static string AddZero(int value)
        {
            string result;
            if (value < 10) {
                result = $"0{value}";
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// searchCondition用のセッションクリア
        /// </summary>
        public static void SessionClear_ofSearchCondition()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session.Remove(SearchLabel.CLIENT_NAME);
            hc.Session.Remove(S_ConditionLabel.CLIENT_CODE);
            hc.Session.Remove(S_ConditionLabel.CLIENT_NAME_AMBI);
            hc.Session.Remove(SearchLabel.END_USER_NAME);
            hc.Session.Remove(S_ConditionLabel.END_USER_CODE);
            hc.Session.Remove(S_ConditionLabel.ENDUSER_NAME_AMBI);
            hc.Session.Remove(S_ConditionLabel.OPERATING_TIME);
            hc.Session.Remove(S_ConditionLabel.MANAGEMENT_OFFICE_ID);
            hc.Session.Remove(SearchLabel.MANAGEMENT_OFFICE);
            hc.Session.Remove(S_ConditionLabel.MGO_AMBI);
            hc.Session.Remove(S_ConditionLabel.MODEL_ID);
            hc.Session.Remove(SearchLabel.MODEL_NAME);
            hc.Session.Remove(S_ConditionLabel.TYPE_ID);
            hc.Session.Remove(SearchLabel.TYPE_NAME);
            hc.Session.Remove(S_ConditionLabel.SERIAL_NUMBER);
            hc.Session.Remove(S_ConditionLabel.NICKNAME);
            hc.Session.Remove(S_ConditionLabel.ALERT_S);
            hc.Session.Remove(S_ConditionLabel.ALERT_E);
            hc.Session.Remove(S_ConditionLabel.SETTING_DATE_S);
            hc.Session.Remove(S_ConditionLabel.SETTING_DATE_E);
            hc.Session.Remove(S_ConditionLabel.PARTS_ALERT);
            hc.Session.Remove(S_ConditionLabel.MACHINE_ALERT);
            hc.Session.Remove(S_ConditionLabel.ALERT_NO);
            hc.Session.Remove(S_ConditionLabel.ALERT_NAME);
            hc.Session.Remove(S_ConditionLabel.ALERT_LEVEL);
            hc.Session.Remove(SearchLabel.CHOOSE_ALERT_DATE_S);
            hc.Session.Remove(SearchLabel.PAGE_NO_TEMP);
            hc.Session.Remove(SearchLabel.PAGE_OFFSET_TEMP);
            hc.Session.Remove(SearchLabel.HISTORY_PAGE_NO_TEMP);
            hc.Session.Remove(SearchLabel.HISTORY_PAGE_OFFSET_TEMP);
            hc.Session.Remove(S_ConditionLabel.SORT_COL);
            hc.Session.Remove(S_ConditionLabel.ORDER_DIRECTION);
            hc.Session.Remove(S_ConditionLabel.COLMUN_KEY);
            hc.Session.Remove(GraphData_Label.GRAPH_ALERT_S);
            hc.Session.Remove(GraphData_Label.GRAPH_ALERT_E);
            hc.Session.Remove(S_ConditionLabel.ALERT_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.ALERT_COL_KEY);
            hc.Session.Remove(S_ConditionLabel.ALERT_ORDER_DIRECTION);
            hc.Session.Remove(S_ConditionLabel.PARTS_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.PARTS_COLMUN_KEY);
            hc.Session.Remove(S_ConditionLabel.PARTS_ORDER_DIRECTION);
            hc.Session.Remove(S_ConditionLabel.HISTORY_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.HISTORY_COL_KEY);
            hc.Session.Remove(S_ConditionLabel.HISTORY_ORDER_DIRECTION);
        }

        public static void SesionClear_ofHistoryPage()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session.Remove(S_ConditionLabel.HISTORY_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.HISTORY_COL_KEY);
            hc.Session.Remove(S_ConditionLabel.HISTORY_ORDER_DIRECTION);
            hc.Session.Remove(SearchLabel.HISTORY_PAGE_NO_TEMP);
            hc.Session.Remove(SearchLabel.HISTORY_PAGE_OFFSET_TEMP);
        }

        public static void SessionClear_ofDetailPage()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session.Remove(S_ConditionLabel.PARTS_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.PARTS_COLMUN_KEY);
            hc.Session.Remove(S_ConditionLabel.PARTS_ORDER_DIRECTION);
            hc.Session.Remove(S_ConditionLabel.ALERT_SORT_COL);
            hc.Session.Remove(S_ConditionLabel.ALERT_COL_KEY);
            hc.Session.Remove(S_ConditionLabel.ALERT_ORDER_DIRECTION);
        }

        /// <summary>
        /// endUser用のセッションクリア
        /// </summary>
        public static void SessionClear_ofSelectedEndUser()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session.Remove(SearchLabel.SELECTED_ENDUSER_ID);
            hc.Session.Remove(SearchLabel.SELECTED_ENDUSER_NAME);
        }

        /// <summary>
        /// 検索条件リストから、表示用文字列を作成する
        /// </summary>
        /// <remarks>セッションにも格納 : Session[S_ConditionLabel.INFO_LIST]</remarks>
        /// <param name="searchInfoList">検索条件がまとめられたリスト</param>
        /// <param name="commonWords">ワードリソース</param>
        /// <returns>検索条件の表示用文字列</returns>
        public static string MakeSearchInfoString(in List<string> searchInfoList, in Dictionary<int, string> commonWords)
        {
            string searchInfo = "";
            if (searchInfoList == null) //検索条件リストがNULLの時
            {
                searchInfo = commonWords[(int)LanguageTable.CommonPageStrId.Common_28] + $"【{commonWords[(int)LanguageTable.CommonPageStrId.Common_47]}】";   //「現在の検索条件 : 【なし】」
            }
            else
            {
                if (searchInfoList.Count() == 0)//検索条件リストが空っぽの時
                {
                    searchInfo = commonWords[(int)LanguageTable.CommonPageStrId.Common_28] + $"【{commonWords[(int)LanguageTable.CommonPageStrId.Common_47]}】";   //「現在の検索条件 : 【なし】」
                }
                else//検索条件リストに中身があった時
                {
                    searchInfo = commonWords[(int)LanguageTable.CommonPageStrId.Common_28];
                    for (int i = 0; i < searchInfoList.Count(); i++)
                    {
                        searchInfo += $"<p class=\"inline marker bold\">{searchInfoList[i]}</p>";
                    }
                }
            }
            // System.Web.HttpContext hc = System.Web.HttpContext.Current;
            // hc.Session[S_ConditionLabel.INFO_LIST] = searchInfo;
            return searchInfo;
        }

        /// <summary>
        /// サーチコンディションの情報から、検索条件リストを作成
        /// </summary>
        /// <param name="searchCondition">MachineBaseインスタンス</param>
        /// <param name="searchInfoList">検索条件をページに表示するためのリスト</param>
        public static void SetInfoList(in MachineBase searchCondition,ref List<string>searchInfoList)
        {
            if(searchInfoList == null) { searchInfoList = new List<string>(); }
            int searchAll = ConstData.SEARCH_ALL;

    /*アラート発生（期間）*/
            //両方入っているとき
            if((Funcs.IsNotNullObject(searchCondition.alertStart)) && (searchCondition.alertStart.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE) &&
                (Funcs.IsNotNullObject(searchCondition.alertEnd)) && (searchCondition.alertEnd.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE)) {
                searchInfoList.Add($"アラート発生期間 : {searchCondition.alertStart.ToString(ConstData.DATE_FORMAT)} ～ {searchCondition.alertEnd.ToString(ConstData.DATE_FORMAT)}");
            }
            //スタートだけ入っていた
            else if ((Funcs.IsNotNullObject(searchCondition.alertStart)) && (searchCondition.alertStart.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE))
            { 
                searchInfoList.Add($"アラート発生期間 :  {searchCondition.alertStart.ToString(ConstData.DATE_FORMAT)} ～ ");
            }
            else//終了日時だけ入っていたとき
            {
                if((Funcs.IsNotNullObject(searchCondition.alertEnd)) && (searchCondition.alertEnd.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE)) { 
                    searchInfoList.Add($"アラート発生期間 :  ～ {searchCondition.alertEnd.ToString(ConstData.DATE_FORMAT)}");
                }
            }

    /*アラート内容*/
            if((Funcs.IsNotNullObject(searchCondition.alertNo)) && (searchCondition.alertNo != searchAll)) {
                searchInfoList.Add($"アラート種類 : {searchCondition.alertName}");
            }
    /*アラートLv*/
            if((Funcs.IsNotNullObject(searchCondition.alertLevel)) && (searchCondition.alertLevel != searchAll)) {
                searchInfoList.Add($"アラートレベル : {searchCondition.alertLevelString}");
            }

    /*エンドユーザ名*/
            if(searchCondition.endUserCode != searchAll) { 
                searchInfoList.Add($"エンドユーザー名 : {searchCondition.endUserName}");
            }
    /*あいまいEU名*/
            if(searchCondition.enduserName_ambi != searchAll.ToString()) {
                string enduserName;
                if (searchCondition.enduserName_ambi.Contains("%"))
                {
                    enduserName = searchCondition.enduserName_ambi.Replace("%", "");
                    searchInfoList.Add($"エンドユーザー名 :「{enduserName}」を含む");
                }
                else
                {
                    enduserName = searchCondition.enduserName_ambi;
                    searchInfoList.Add($"エンドユーザー名 : {enduserName}");
                }
            }


    /*製品群*/
            if(searchCondition.modelID != searchAll) {
                searchInfoList.Add($"製品群 : {searchCondition.modelName}");
            }

    /*品名*/
            if(searchCondition.typeID != searchAll.ToString()) { 
                searchInfoList.Add($"品名 : {searchCondition.typeName}");
            }

    /*S/N*/
            if(searchCondition.serialNumber != searchAll.ToString()) { 
                string serialNumber;
                if (searchCondition.serialNumber.Contains("%"))
                {
                    serialNumber = searchCondition.serialNumber.Replace("%","");
                    searchInfoList.Add($"シリアルナンバー :「{serialNumber}」を含む");
                }
                else
                {
                    serialNumber = searchCondition.serialNumber;
                    searchInfoList.Add($"シリアルナンバー : {serialNumber}");
                }
            }

    /*稼働時間*/
            if(searchCondition.operatingTime != searchAll) { 
                searchInfoList.Add($"稼働時間 : {searchCondition.operatingTime}時間以上");
            }

    /*設置年月（期間）*/
            //両方入っているとき
            if((Funcs.IsNotNullObject(searchCondition.settingDate_s)) && (searchCondition.settingDate_s.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE) &&
                (Funcs.IsNotNullObject(searchCondition.settingDate_e)) && (searchCondition.settingDate_e.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE)) {
                searchInfoList.Add($"設置期間 : {searchCondition.settingDate_s.ToString(ConstData.DATE_FORMAT)} ～ {searchCondition.settingDate_e.ToString(ConstData.DATE_FORMAT)}");
            }
            //スタートだけ入っていた
            else if ((Funcs.IsNotNullObject(searchCondition.settingDate_s)) && (searchCondition.settingDate_s.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE))
            { 
                searchInfoList.Add($"設置期間 :  {searchCondition.settingDate_s.ToString(ConstData.DATE_FORMAT)} ～ ");
            }
            else//終了日時だけ入っていたとき
            {
                if((Funcs.IsNotNullObject(searchCondition.settingDate_e)) && (searchCondition.settingDate_e.ToString(ConstData.DATE_FORMAT) != ConstData.DEFAULT_DATE)) { 
                    searchInfoList.Add($"設置期間 :  ～ {searchCondition.settingDate_e.ToString(ConstData.DATE_FORMAT)}");
                }
            }

    /*得意先名*/
            if(searchCondition.companyID != searchAll) {
                searchInfoList.Add($"得意先名 : {searchCondition.companyName}");
            }
    /*あいまい得意先名*/
            if(searchCondition.companyName_ambi != searchAll.ToString()) {
                string clientName;
                if (searchCondition.companyName_ambi.Contains("%"))
                {
                    clientName = searchCondition.companyName_ambi.Replace("%","");
                    searchInfoList.Add($"得意先名 :「{clientName}」を含む");
                }
                else
                {
                    clientName = searchCondition.companyName_ambi;
                    searchInfoList.Add($"得意先名 : {clientName}");
                }
            }

    /*担当支店営業所*/
            if(searchCondition.managementOfficeID != searchAll) {
                searchInfoList.Add($"担当支店営業所 : {searchCondition.managementOffice}");
            }
    /*あいまい担当支店名*/
            if(searchCondition.mgo_ambi != searchAll.ToString()) {
                string mgofficeName;
                if (searchCondition.mgo_ambi.Contains("%"))
                {
                    mgofficeName = searchCondition.mgo_ambi.Replace("%","");
                    searchInfoList.Add($"担当支店営業所 :「{mgofficeName}」を含む");
                }
                else
                {
                    mgofficeName = searchCondition.mgo_ambi;
                    searchInfoList.Add($"担当支店営業所 : {mgofficeName}");
                }
            }


    /*機械アラート有り無し*/
            if (searchCondition.machineAlert != searchAll) {
                switch (searchCondition.machineAlert){
                    case 0:
                        searchInfoList.Add($"機械アラート : なし");
                        break;
                    case 1:
                        searchInfoList.Add($"機械アラート : あり");
                        break;
                }
            }

    /*部品交換アラートありなし*/
            if (searchCondition.partsAlert != searchAll) {
                switch (searchCondition.partsAlert){
                    case 0:
                        searchInfoList.Add($"部品交換アラート : なし");
                        break;
                    case 1:
                        searchInfoList.Add($"部品交換アラート : あり");
                        break;
                }
            }
        }

        /// <summary>
        /// 検索ボックスからの情報をチェックしてセッションに値を格納する。
        /// </summary>
        /// <remarks>全てのケースでsearchCondのプロパティに値をセットしてから、SessionにsearchCondのプロパティを格納。
        /// </remarks>
        /// <param name="searchCond">MachineBaseインスタンス</param>
        public static void GetRefiningCondition(Dictionary<int, string> commonWords, ref MachineBase searchCond)
        {
            //各リクエストフォームの処理内容はclientNameのコメントを参照してください。
            System.Web.HttpContext hc = System.Web.HttpContext.Current;

 //:::: clientName[得意先名] == DB:companyName
 //+++ 処理内容はclientCDとclientNameがベース +++
            //まずNullでないかチェック
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.CLIENT_NAME]))
            {
                string clientName = hc.Request.Form[SearchLabel.CLIENT_NAME];//リクエストフォームの内容を受け取る
                //この時点でのclientName=>「id,name」もしくは空
                //不正な値が入っていないかチェック
                if (Utils.Validate.CheckInputData(clientName))
                {
                    //空文字だった時。(SELECT-OPTIONの項目は"SELECT_ALL"(-1)と比較)
                    if (clientName == ConstData.EMPTY)
                    {
                        //空文字の時は名前・IDそれぞれデフォルト値をセット
                        searchCond.companyName = ConstData.SEARCH_ALL.ToString();//全検索用の値をセット
                        searchCond.companyID = ConstData.SEARCH_ALL;//全検索用の値をセット
                    }
                    else//空文字ではなかった時
                    {
                        //clientNameをidとnameに分ける
                        int id = Funcs.GetIdFromValue(clientName);
                        string name = Funcs.GetNameFromValue(clientName);
                        //分けた値をそれぞれセット
                        searchCond.companyName = name;
                        searchCond.companyID = id;
                        searchCond.companyName_ambi = ConstData.SEARCH_ALL.ToString();
                        hc.Session[S_ConditionLabel.CLIENT_NAME_AMBI] = searchCond.companyName_ambi;
                    }
                }
            }
            else//NULLだった時
            {
                searchCond.companyName = ConstData.SEARCH_ALL.ToString();//全検索用の値をセット
            }
            hc.Session[SearchLabel.CLIENT_NAME] = searchCond.companyName;
            //内部検索用
            hc.Session[S_ConditionLabel.CLIENT_CODE] = searchCond.companyID;

            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.CLIENT_NAME_AMBI]))//NULLでないか
            {
                string clientName_ambi = hc.Request.Form[SearchLabel.CLIENT_NAME_AMBI];
                if (clientName_ambi != ConstData.EMPTY)//空文字でないとき
                {
                    if (Validate.CheckInputData(clientName_ambi)) { 
                        string clientName = clientName_ambi.Replace("?","%");//半角
                        if (clientName == commonWords[(int)LanguageTable.CommonPageStrId.Common_29]) { clientName = ConstData.SEARCH_ALL.ToString(); }
                        searchCond.companyName_ambi = clientName;
                        searchCond.companyID = ConstData.SEARCH_ALL;
                        searchCond.companyName = ConstData.SEARCH_ALL.ToString();
                    }
                }
                                
                else //空文字だった時
                {
                    searchCond.companyName_ambi = ConstData.SEARCH_ALL.ToString();
                    searchCond.companyID = ConstData.SEARCH_ALL;
                    searchCond.companyName = ConstData.SEARCH_ALL.ToString();
                }
            }
            else { //NULLの時
            
                    searchCond.companyName_ambi = ConstData.SEARCH_ALL.ToString();
                    searchCond.companyID = ConstData.SEARCH_ALL;
                    searchCond.companyName = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.CLIENT_NAME_AMBI] = searchCond.companyName_ambi;
            hc.Session[S_ConditionLabel.CLIENT_CODE] = searchCond.companyID;
            hc.Session[SearchLabel.CLIENT_NAME] = searchCond.companyName;

 //:::: endUserName[エンドユーザー名] == DB:factoryName
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.END_USER_NAME]))
            {
                string endUserName = hc.Request.Form[SearchLabel.END_USER_NAME];
                if (Utils.Validate.CheckInputData(endUserName))
                {
                    if (endUserName == ConstData.EMPTY)
                    {
                        searchCond.endUserName = ConstData.SEARCH_ALL.ToString();
                        searchCond.endUserCode = ConstData.SEARCH_ALL;
                        hc.Session.Remove(SearchLabel.SELECTED_ENDUSER_NAME);
                    }
                    else
                    {
                        searchCond.endUserName = Funcs.GetNameFromValue(endUserName);
                        int id = Funcs.GetIdFromValue(endUserName);
                        searchCond.endUserCode = id;
                        hc.Session[SearchLabel.SELECTED_ENDUSER_NAME] = Funcs.GetNameFromValue(endUserName);
                    }
                }
            }
            else
            {
                searchCond.endUserName = ConstData.SEARCH_ALL.ToString();
                searchCond.endUserCode = ConstData.SEARCH_ALL;
                hc.Session.Remove(SearchLabel.SELECTED_ENDUSER_NAME);
            }
            hc.Session[SearchLabel.END_USER_NAME] = searchCond.endUserName;
            hc.Session[S_ConditionLabel.END_USER_CODE] = searchCond.endUserCode;
            hc.Session[SearchLabel.SELECTED_ENDUSER_ID] = searchCond.endUserCode;

         //:::: ambigous Search
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.ENDUSER_NAME_AMBI]))//NULLでないか
            {
                string enduserName_ambi = hc.Request.Form[SearchLabel.ENDUSER_NAME_AMBI].Replace("？", "?");
                if (enduserName_ambi != ConstData.EMPTY)//空文字でないとき
                {
                    if (Utils.Validate.CheckInputData(enduserName_ambi))
                    {
                        string enduserName = enduserName_ambi.Replace("?", "%");
                        if (enduserName == commonWords[(int)LanguageTable.CommonPageStrId.Common_29]) { enduserName = ConstData.SEARCH_ALL.ToString(); }
                        searchCond.enduserName_ambi = enduserName;
                        searchCond.endUserCode = ConstData.SEARCH_ALL;
                        searchCond.endUserName = ConstData.SEARCH_ALL.ToString();
                    }
                }
                                
                else //空文字だった時
                {
                    searchCond.enduserName_ambi = ConstData.SEARCH_ALL.ToString();
                    searchCond.endUserCode= ConstData.SEARCH_ALL;
                    searchCond.endUserName = ConstData.SEARCH_ALL.ToString();
                }
            }
            else { //NULLの時
                searchCond.enduserName_ambi = ConstData.SEARCH_ALL.ToString();
                searchCond.endUserCode= ConstData.SEARCH_ALL;
                searchCond.endUserName = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.ENDUSER_NAME_AMBI] = searchCond.enduserName_ambi;
            hc.Session[S_ConditionLabel.END_USER_CODE] = searchCond.endUserCode;
            hc.Session[SearchLabel.END_USER_NAME] = searchCond.endUserName;

 //:::: oparatingTime[稼働時間]
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.OPERATING_TIME]))
            {
                string operatingTime = hc.Request.Form[SearchLabel.OPERATING_TIME];
                if (Utils.Validate.CheckInputData(operatingTime))
                {
                    if (operatingTime == ConstData.EMPTY)
                    {
                        searchCond.operatingTime = ConstData.SEARCH_ALL;
                    }
                    else
                    {
                        searchCond.operatingTime = int.Parse(operatingTime);
                    }
                }
            }
            else
            {
                searchCond.operatingTime = ConstData.SEARCH_ALL;
            }
            hc.Session[S_ConditionLabel.OPERATING_TIME] = searchCond.operatingTime;

 //:::: managementOffice[担当支店営業所]
            //リクエストフォームの確認(Nullでなければ)
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.MANAGEMENT_OFFICE]))
            {
                string managementOffice = hc.Request.Form[SearchLabel.MANAGEMENT_OFFICE];
                //不正文字列の確認
                if (Utils.Validate.CheckInputData(managementOffice))
                {
                    //リクエストフォームのvalueを確認
                    if (managementOffice != ConstData.EMPTY)//空文字でない時
                    { 
                        int branchID = Funcs.GetIdFromValue(managementOffice);
                        searchCond.managementOfficeID = branchID;
                        string name = Funcs.GetNameFromValue(managementOffice);
                        searchCond.managementOffice = name;
                    }
                    else//空文字であった時
                    {
                        searchCond.managementOffice = ConstData.SEARCH_ALL.ToString();
                        searchCond.managementOfficeID = ConstData.SEARCH_ALL;
                    }
                }
            }
            else//リクエストフォームの値がNull
            {
                searchCond.managementOffice = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[SearchLabel.MANAGEMENT_OFFICE] = searchCond.managementOffice;
            hc.Session[S_ConditionLabel.MANAGEMENT_OFFICE_ID] = searchCond.managementOfficeID;

         //:::: MGOffice ambigous Search
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.MGO_AMBI]))//NULLでないか
            {
                string mgo_ambi = hc.Request.Form[SearchLabel.MGO_AMBI];
                if (mgo_ambi != ConstData.EMPTY)//空文字でないとき
                {
                    if (Utils.Validate.CheckInputData(mgo_ambi))
                    {
                        string mgo = mgo_ambi.Replace("?", "%");
                        if (mgo == commonWords[(int)LanguageTable.CommonPageStrId.Common_29]) { mgo = ConstData.SEARCH_ALL.ToString(); }
                        searchCond.mgo_ambi = mgo;
                    }
                }
                else //空文字だった時
                {
                    searchCond.mgo_ambi = ConstData.SEARCH_ALL.ToString();
                }
            }
            else { //NULLの時
                searchCond.mgo_ambi = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.MGO_AMBI] = searchCond.mgo_ambi;

 //:::: modelID[製品群]
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.MODEL_ID]))
            {
                string modelID = hc.Request.Form[SearchLabel.MODEL_ID];
                if (Utils.Validate.CheckInputData(modelID))
                {
                    if (modelID == ConstData.EMPTY)
                    {
                        searchCond.modelID = ConstData.SEARCH_ALL;
                    }
                    else
                    {
                        int id = Funcs.GetIdFromValue(modelID);
                        string name = Funcs.GetNameFromValue(modelID);
                        searchCond.modelName = name;
                        searchCond.modelID = id;
                    }
                }
            }
            else
            {
                searchCond.modelID = ConstData.SEARCH_ALL;
            }
            hc.Session[S_ConditionLabel.MODEL_ID] = searchCond.modelID;
            hc.Session[SearchLabel.MODEL_NAME] = searchCond.modelName;

 //:::: typeName[品名]
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.TYPE_ID]))
            {
                string typeID = hc.Request.Form[SearchLabel.TYPE_ID];
                if (Utils.Validate.CheckInputData(typeID))
                {
                    if (typeID == ConstData.EMPTY)
                    {
                        searchCond.typeID = ConstData.SEARCH_ALL.ToString();
                    }
                    else
                    {
                        string id = Funcs.GetStringIdFromValue(typeID);
                        searchCond.typeID = id;
                        string name = Funcs.GetNameFromValue(typeID);
                        searchCond.typeName = name;
                    }
                }
            }
            else
            {
                searchCond.typeID = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.TYPE_ID] = searchCond.typeID;
            hc.Session[SearchLabel.TYPE_NAME] = searchCond.typeName;

 //:::: serialNumber[シリアルナンバー]
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.SERIAL_NUMBER]))
            {
                string serialNumber = hc.Request.Form[SearchLabel.SERIAL_NUMBER];
                if (serialNumber != ConstData.EMPTY)//空文字でないとき
                {
                    if (Utils.Validate.CheckInputData(serialNumber))
                    {
                        string SN = serialNumber.Replace("?", "%");
                        if (SN == commonWords[(int)LanguageTable.CommonPageStrId.Common_29]) { SN = ConstData.SEARCH_ALL.ToString(); }
                        searchCond.serialNumber = SN;
                    }
                }
                else //空文字だった時
                {
                    searchCond.serialNumber = ConstData.SEARCH_ALL.ToString();
                }
                if (Utils.Validate.CheckInputData(serialNumber))
                {
                }
            }
            else
            {
                searchCond.serialNumber = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.SERIAL_NUMBER] = searchCond.serialNumber;

 //:::: machineName[顧客呼称]
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.MACHINE_NAME]))
            {
                string machineName = hc.Request.Form[SearchLabel.MACHINE_NAME];
                if (Utils.Validate.CheckInputData(machineName))
                {
                    if (machineName == ConstData.EMPTY)
                    {
                        searchCond.machineName = ConstData.SEARCH_ALL.ToString();
                    }
                    else
                    {
                        searchCond.machineName = machineName;
                    }
                }
            }
            else
            {
                searchCond.machineName = ConstData.SEARCH_ALL.ToString();
            }
            hc.Session[S_ConditionLabel.NICKNAME] = searchCond.machineName;

 //:::: alert発生期間
            //start
            if ((Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.ALERT_S])) && (hc.Request.Form[SearchLabel.ALERT_S] != ConstData.EMPTY) && (hc.Request.Form[SearchLabel.ALERT_S] != ConstData.DEFAULT_DATE))
            {
                string alertDate_s = hc.Request.Form[SearchLabel.ALERT_S];
                if (Utils.Validate.CheckInputData(alertDate_s))
                {
                        searchCond.alertStart= DateFromValue(alertDate_s);
                }
                else
                {
                    searchCond.alertStart = DateTime.MinValue;
                }
            }
            else
            {
                searchCond.alertStart = DateTime.MinValue;
            }
            hc.Session[S_ConditionLabel.ALERT_S] = searchCond.alertStart;

            //end
            if ((Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.ALERT_E])) && (hc.Request.Form[SearchLabel.ALERT_E] != ConstData.EMPTY) && (hc.Request.Form[SearchLabel.ALERT_E] != ConstData.DEFAULT_DATE))
            {
                string alertDate_e = hc.Request.Form[SearchLabel.ALERT_E];
                if (Utils.Validate.CheckInputData(alertDate_e))
                {
                    searchCond.alertEnd= DateFromValue(alertDate_e);
                }
                else
                {
                    searchCond.alertEnd = DateTime.MinValue;
                }
            }
            else
            {
                searchCond.alertEnd = DateTime.MinValue;
            }
            hc.Session[S_ConditionLabel.ALERT_E] = searchCond.alertEnd;

 //:::: deliveryDate
            //start
            if ((Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.SETTING_DATE_S])) && (hc.Request.Form[SearchLabel.SETTING_DATE_S] != ConstData.EMPTY) && (hc.Request.Form[SearchLabel.SETTING_DATE_S] != ConstData.DEFAULT_DATE))
            {
                string deliveryDate_s = hc.Request.Form[SearchLabel.SETTING_DATE_S];
                if (Utils.Validate.CheckInputData(deliveryDate_s))
                {
                    //検索開始の日時
                    searchCond.settingDate_s = DateFromValue(deliveryDate_s);
                }
                else
                {
                    searchCond.settingDate_s = DateTime.MinValue;
                }
            }
            else
            {
                searchCond.settingDate_s = DateTime.MinValue;
            }
            hc.Session[S_ConditionLabel.SETTING_DATE_S] = searchCond.settingDate_s;

            //end
            if ((Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.SETTING_DATE_E])) && (hc.Request.Form[SearchLabel.SETTING_DATE_E] != ConstData.EMPTY) && (hc.Request.Form[SearchLabel.SETTING_DATE_E] != ConstData.DEFAULT_DATE))
            {
                string deliveryDate_e = hc.Request.Form[SearchLabel.SETTING_DATE_E];
                if (Utils.Validate.CheckInputData(deliveryDate_e))
                {
                    //検索終了の日時
                    searchCond.settingDate_e = DateFromValue(deliveryDate_e);
                }
                else
                {
                    searchCond.settingDate_e = DateTime.MinValue;
                }
            }
            else
            {
                searchCond.settingDate_e = DateTime.MinValue;
            }
            hc.Session[S_ConditionLabel.SETTING_DATE_E] = searchCond.settingDate_e;

 //:::: partsAlert
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.PARTS_ALERT]))
            {
                if (hc.Request.Form[SearchLabel.PARTS_ALERT] != ConstData.EMPTY)
                {
                     searchCond.partsAlert = int.Parse(hc.Request.Form[SearchLabel.PARTS_ALERT]);
                }
                else
                {
                    searchCond.partsAlert = ConstData.SEARCH_ALL;
                }
            }
            hc.Session[S_ConditionLabel.PARTS_ALERT] = searchCond.partsAlert;

 //:::: machineAlert
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.MACHINE_ALERT]))
            {
                if (hc.Request.Form[SearchLabel.MACHINE_ALERT] != ConstData.EMPTY)
                {
                    searchCond.machineAlert = int.Parse(hc.Request.Form[SearchLabel.MACHINE_ALERT]);
                }
                else {
                    searchCond.machineAlert = ConstData.SEARCH_ALL;
                }
            }
            hc.Session[S_ConditionLabel.MACHINE_ALERT] = searchCond.machineAlert;

 //:::: alertNo
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.ALERT_NO]))
            {
                string alertNo = hc.Request.Form[SearchLabel.ALERT_NO];
                if (Utils.Validate.CheckInputData(alertNo))
                {
                    if (alertNo == ConstData.EMPTY)
                    {
                        searchCond.alertNo = ConstData.SEARCH_ALL;
                    }
                    else
                    {
                        int id = Funcs.GetIdFromValue(alertNo);
                        searchCond.alertNo = id;
                        string name = Funcs.GetNameFromValue(alertNo);
                        searchCond.alertName = name;
                        hc.Session[S_ConditionLabel.ALERT_NAME] = searchCond.alertName;
                    }
                }
            }
            else
            {
                searchCond.alertNo = ConstData.SEARCH_ALL;
            }
            hc.Session[S_ConditionLabel.ALERT_NO] = searchCond.alertNo;

 //:::: alertLv
            if (Funcs.IsNotNullObject(hc.Request.Form[SearchLabel.ALERT_LEVEL]))
            {
                string alertNo = hc.Request.Form[SearchLabel.ALERT_LEVEL];
                if (Utils.Validate.CheckInputData(alertNo))
                {
                    if (alertNo == ConstData.EMPTY)
                    {
                        searchCond.alertLevel = ConstData.SEARCH_ALL;
                    }
                    else
                    {
                        int id = Funcs.GetIdFromValue(alertNo);
                        searchCond.alertLevel = id;
                        string name = Funcs.GetNameFromValue(alertNo);
                        searchCond.alertLevelString = name;
                        hc.Session[S_ConditionLabel.ALERT_LEVEL_STRING] = searchCond.alertLevelString;
                    }
                }
            }
            else
            {
                searchCond.alertLevel = ConstData.SEARCH_ALL;
            }
            hc.Session[S_ConditionLabel.ALERT_LEVEL] = searchCond.alertLevel;
        }

        /// <summary>
        /// input type=dateに入力された値をDateTime型にして返す
        /// </summary>
        /// <remarks>"yyyy-MM-dd"を"yyyy/MM/dd"に変更。できなければ本日の日付で返す</remarks>
        /// <param name="dateString">入力された値の文字列</param>
        /// <returns>DateTime</returns>
        public static DateTime DateFromValue(string dateString)
        {
            DateTime someDay;
            if(dateString != ConstData.EMPTY)
            {
                dateString.Replace("-", "/");
            }
            else
            {
                dateString = DateTime.Now.ToString("yyyy/MM/dd");
            }
            someDay = DateTime.Parse(dateString);
            return someDay;
        }
    }
}