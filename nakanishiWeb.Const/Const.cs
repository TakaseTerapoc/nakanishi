using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.Const
{
    public class ConstData
    {
        public const int NAKANISHI_ID = 1;
        public const int USER_KIND_ADMIN = 0;
        public const int USER_KIND_ORDINARY = 1;
        public const int COMPANY_KIND_ADMIN = 0;
        public const int COMPANY_KIND_CLIENT = 1;
        public const int COMPANY_KIND_MAKER = 2;
        public const int COMPANY_KIND_ENDUSER = 3;
        public const int ADMIN_BRANCH = 0;
        public const int MAIN_OFFICE = 0;
        public const int SEARCH_ALL = -1;
        public const int ADMIN_USER = 0;
        public const int PARTS_ALERT_BORDER = 10;
        public const string EMPTY = "";
        public const int SELECT_ALL = -1;
        public const int SELECT_ALERT_TRUE = 1;
        public const int SELECT_ALERT_FALSE = 0;
        public const int DEFAULT_USER_ID = -1;
        public const string DATE_FORMAT = "yyyy/MM/dd";
        public const string DEFAULT_DATE = "0001/01/01";
        public const string TIME_STAMP_DATE_FORMAT = "yyyy-MM-dd 00:00:00";
        public const string TIME_STAMP_DATE_END_FORMAT = "yyyy-MM-dd 23:59:59";
        public const string TIME_STAMP_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DEFAULT_DATE_FORMAT = "0001-01-01 00:00:00";
        public const int KIND_MACHINE_ALERT = 0;
        public const int KIND_PARTS_ALERT = 1;

        //アラートの緊急度 = DB alert_master.alert_level
        public const int EMERGENCY = 1; //key => 16
        public const int WORNING = 2; //key => 17
        public const int BEWARE = 3; //key => 18
        public const int ATTENTION = 5; //key => 19

        //アラート緊急度に対応するDBstringのキー
        public const int EMERGENCY_KEY = 16;
        public const int WORNING_KEY = 17;
        public const int BEWARE_KEY = 18;
        public const int ATTENTION_KEY = 19;

        //文字列リソースを格納するDictionaryのKEY(pageName)に使用
        public const string COMMON = "common";//Dictionary用
        public const string LOGIN_PAGE_NAME = "login";
        public const string MAIN_PAGE_NAME = "main";
        public const string CLIENT_PAGE_NAME = "client";
        public const string PRODUCT_PAGE_NAME = "product";
        public const string ALERT_PAGE_NAME = "alert";
        public const string DETAIL_PAGE_NAME = "detail";
        public const string HISTORY_PAGE_NAME = "history";
        public const string USER_EDIT_PAGE_NAME = "useredit";
        public const string ALERT_MAIL_SETTING_PAGE_NAME = "alertmailsetting";
        public const string SETTING_COMPLETE_PAGE_NAME = "SettingComplete";

        //以下３つは検索ボックスの値を取得するのに使用
        public const char VALUE_SPLIT_CHAR= ',';
        public const int VALUES_ID_INDEX = 0;
        public const int VALUES_NAME_INDEX = 1;

        //各ページのテーブルの行 DBのLIMIT句でも使用
        public const int MAIN_LIMIT = 15;
        public const int CLIENT_LIMIT = 15;
        public const int PRODUCT_LIMIT = 15;
        public const int ALERT_LIMIT = 15;
        public const int DETAIL_LIMIT = 5;
        public const int HISTORY_LIMIT = 15;
        //ページャー関連の数
        public const int PAGER_MAX = 7;
        public const int PAGER_HALF = 3;
        public const int SHORT_PAGER_MAX = 5;
        public const int SHORT_PAGER_HALF = 2;

        //並べ替え用のディクショナリー : MachineMaster
        public static Dictionary<string, string> machineSortColumns =
            new Dictionary<string, string>()
            {
                { SearchLabel.CLIENT_NAME,"cm.company_name "},
                { SearchLabel.END_USER_NAME,"cm2.company_name"},
                { SearchLabel.ALERT_LEVEL,"alert_level"},
                { SearchLabel.ALERT_NAME,"alert_name"},
                { SearchLabel.OCCUR_TIME,"occur_time"},
                { SearchLabel.RELEASE_TIME,"release_time"},
                { SearchLabel.DELIVERY_DATE,"delivery_date"},
                { SearchLabel.MODEL_NAME,"mmm.description"},
                { SearchLabel.TYPE_NAME,"lr.string"},
                { SearchLabel.SERIAL_NUMBER,"serial_number"},
                { SearchLabel.OPERATING_TIME,"actual_time"},
                { SearchLabel.MANAGEMENT_OFFICE,"bm2.branch_name"},
                { SearchLabel.MACHINE_ALERT,"alert_machine"},
                { SearchLabel.PARTS_ALERT,"alert_parts"},
                { SearchLabel.PARTS_NAME,"part_name"},
                { SearchLabel.LAST_TIME,"last_time"},
            };
        public static Dictionary<string, string> machineFirstOrderDirection =
            new Dictionary<string, string>()
            {
                { SearchLabel.CLIENT_NAME,"ASC"},
                { SearchLabel.END_USER_NAME,"ASC"},
                { SearchLabel.ALERT_LEVEL,"ASC"},
                { SearchLabel.ALERT_NAME,"ASC"},
                { SearchLabel.OCCUR_TIME,"DESC"},
                { SearchLabel.RELEASE_TIME,"DESC"},
                { SearchLabel.DELIVERY_DATE,"DESC"},
                { SearchLabel.MODEL_NAME,"ASC"},
                { SearchLabel.TYPE_NAME,"ASC"},
                { SearchLabel.SERIAL_NUMBER,"ASC"},
                { SearchLabel.OPERATING_TIME,"DESC"},
                { SearchLabel.MANAGEMENT_OFFICE,"ASC"},
                { SearchLabel.MACHINE_ALERT,"DESC"},
                { SearchLabel.PARTS_ALERT,"DESC"},
                { SearchLabel.PARTS_NAME,"ASC"},
                { SearchLabel.LAST_TIME,"ASC"},
            };

        //並べ替え用のディクショナリー : CompanyMaster
        public static Dictionary<string, string> companySortColumns =
            new Dictionary<string, string>()
            {
                { SearchLabel.CLIENT_NAME,"clientName"},
                { SearchLabel.END_USER_NAME,"endUserName"},
                { SearchLabel.MANAGEMENT_OFFICE,"MGOfficeName"}
            };
        public static Dictionary<string, string> companyFirstOrderDirection =
            new Dictionary<string, string>()
            {
                { SearchLabel.CLIENT_NAME,"ASC"},
                { SearchLabel.END_USER_NAME,"ASC"},
                { SearchLabel.MANAGEMENT_OFFICE,"ASC"}
            };

        //並べ替え用のディクショナリー : AlertMaster
        public static Dictionary<string,string> alertSortColmuns =
            new Dictionary<string, string>()
            {
                { SearchLabel.OCCUR_TIME,"occur_time"},
                { SearchLabel.RELEASE_TIME,"release_time"},
                { SearchLabel.ALERT_LEVEL,"alert_level"},
                { SearchLabel.ALERT_NAME,"alert_name"}
            };
        public static Dictionary<string,string> alertFirstOrderDirection =
            new Dictionary<string, string>()
            {
                { SearchLabel.OCCUR_TIME,"DESC"},
                { SearchLabel.RELEASE_TIME,"DESC"},
                { SearchLabel.ALERT_LEVEL,"DESC"},
                { SearchLabel.ALERT_NAME,"ASC"}
            };

        //並べ替え用のディクショナリー : PartsMaster
        public static Dictionary<string, string> partsSortColmuns =
            new Dictionary<string, string>()
            {
                { SearchLabel.PARTS_NAME,"part_name"},
                { SearchLabel.EX_GUIDELINE,"pkm.life_time"},
               // { "exchangeGuidLine_C","life_cycle"},
                { SearchLabel.OPERATING_TIME,"(total_time + init_time)"},
                { SearchLabel.REMAINING_OPERATE,"remainingTime"},
                { SearchLabel.LAST_EX_DATE,"attach_time"},
                { SearchLabel.DELIVERY_DATE,"mm.delivery_date"},
                { SearchLabel.EX_COUNT,"exchang_cunt"}
            };
        public static Dictionary<string, string> partsFirstOrderDirection =
            new Dictionary<string, string>()
            {
                { SearchLabel.PARTS_NAME,"ASC"},
                { SearchLabel.EX_GUIDELINE,"ASC"},
               // { "exchangeGuidLine_C","life_cycle"},
                { SearchLabel.OPERATING_TIME,"DESC"},
                { SearchLabel.REMAINING_OPERATE,"ASC"},
                { SearchLabel.LAST_EX_DATE,"DESC"},
                { SearchLabel.DELIVERY_DATE,"DESC"},
                { SearchLabel.EX_COUNT,"DESC"}
            };

        //並べ替え用のディクショナリー : PartsMaster・AlertMaster
        public static Dictionary<string, string> historySortColmuns =
            new Dictionary<string, string>()
            {
                { SearchLabel.ALERT_LEVEL,"alert_level"},
                { SearchLabel.ALERT_NAME,"alert_name"},
                { SearchLabel.OCCUR_TIME,"occur_time"},
                { SearchLabel.RELEASE_TIME,"release_time"},
                { SearchLabel.PARTS_NAME,"part_name"}
            };
        public static Dictionary<string, string> historyFirstOrderDirection =
            new Dictionary<string, string>()
            {
                { SearchLabel.ALERT_LEVEL,"ASC"},
                { SearchLabel.ALERT_NAME,"ASC"},
                { SearchLabel.OCCUR_TIME,"DESC"},
                { SearchLabel.RELEASE_TIME,"DESC"},
                { SearchLabel.PARTS_NAME,"ASC"}
            };
    }

    /// <summary>
    /// Requestから取り出すときのキー
    /// </summary>
    public class SearchLabel
    {
        public const string CLIENT_CODE = "searchClientCD";
        public const string CLIENT_NAME = "clientName";
        public const string CLIENT_NAME_AMBI = "clientName_ambi";
        public const string END_USER_CODE = "endUserCD";
        public const string END_USER_NAME = "endUserName";
        public const string ENDUSER_NAME_AMBI = "endUserName_ambi";
        public const string OPERATING_TIME = "operatingTime";
        public const string MANAGEMENT_OFFICE = "MGOffice";
        public const string MANAGEMENT_OFFICE_ID = "MGOfficeID";
        public const string MGO_AMBI = "MGOffice_Ambigous";
        public const string CONTRACT_DATE = "contractDate";
        public const string MACHINE_ID = "machineID";
        public const string MAKER_ID = "makerId";
        public const string MODEL_ID = "modelID";
        public const string TYPE_ID = "typeID";
        public const string SERIAL_NUMBER = "serialNumber";
        public const string MACHINE_NAME = "machineName";
        public const string MAKER_NAME = "makerName";
        public const string MODEL_NAME = "modelName";
        public const string TYPE_NAME = "typeName";
        public const string SETTING_DATE_S = "settingDate_start";
        public const string SETTING_DATE_E = "settingDate_end";
        public const string ALERT_S = "alert_start";
        public const string ALERT_E = "alert_end";
        public const string PARTS_ALERT = "partsAlert";
        public const string MACHINE_ALERT = "machineAlert";
        public const string DAY_OR_MONTH = "DayOrMonth";
        public const string ALERT_NO = "alertNo";
        public const string TITLE_NO = "titleNo";
        public const string ALERT_NAME = "alertName";
        public const string OCCUR_TIME = "occurTime";
        public const string RELEASE_TIME = "releaseTime";
        public const string DELIVERY_DATE = "deliveryDate";
        public const string PARTS_NAME = "partsName";
        public const string ALERT_LEVEL = "alertLevel";
        public const string EX_GUIDELINE = "exchangeGuidline";
        public const string REMAINING_OPERATE = "remainingOperate";
        public const string EX_COUNT = "exchangeCount";
        public const string LAST_EX_DATE = "lastExchangeDate";
        public const string EXCHANGE = "exchange";
        public const string LAST_TIME = "lastTime";

        //:::: ↓↓ユーザー情報に使用↓↓
        public const string USER_ID = "userID";
        public const string USER_PASSWORD = "password";
        public const string USER_NAME = "userName";
        public const string USER_KIND = "userKind";
        public const string USER_TIME_DIFF = "userTimeDiff";
        public const string LANGUAGE = "langID";
        public const string LANG_CODE = "langCode";
        public const string COMPANY_ID = "companyID";
        public const string COMPANY_NAME = "companyName";
        public const string COMPANY_KIND = "companyKind";
        public const string BRANCH_ID = "branchID";
        public const string BRANCH_NAME = "branchName";
        public const string AFFILIATION = "affiliation";
        public const string LOGIN_ID = "loginID";
        public const string MAIL_START_TIME = "mailStartTime";
        public const string MAIL_END_TIME = "mailEndTime";

        //他ページャーや選択された機器などのラベル
        public const string PAGER = "clickedPager";
        public const string CHOOSE_MACHINE_ID = "chooseMachineID";
        public const string SELECTED_ENDUSER_ID = "selectedEndUserID";
        public const string SELECTED_ENDUSER_NAME = "selectedEndUserName";
        public const string CHOOSE_MACHINE = "chooseMachine";
        public const string SEARCH_BT = "searchBT";
        public const string BEFORE_PAGE = "beforePage";
        public const string SORT_CHANGE = "changeSort";
        public const string PAGE_NO_TEMP = "tempPageNo";
        public const string PAGE_OFFSET_TEMP = "tempPageOffset";
        public const string HISTORY_PAGE_NO_TEMP = "HistoryTempPageNo";
        public const string HISTORY_PAGE_OFFSET_TEMP = "HistoryTempPageOffset";
        public const string CHOOSE_ALERT_DATE_S = "chooseAlertDate";
        public const string CHOOSE_ALERT_DATE_E = "chooseAlertDateEnd";
        public const string DOWNLOAD_CSV_BT = "downloadCSVBT";
        public const string USER_SETTING_EDIT_BT = "editSet";
    }

    /// <summary>
    /// 検索条件を保存するためのSessionに使うキー
    /// </summary>
    public class S_ConditionLabel
    {
        public const string MACHINE_ID = "searchCondition_machineID";
        public const string CLIENT_CODE = "searchCondition_clientCD";
        public const string CLIENT_NAME_AMBI = "searchCondition_clientName_ambigous";
        public const string END_USER_CODE = "searchCondition_endUserCD";
        public const string ENDUSER_NAME_AMBI = "searchCondition_enduserName_ambigous";
        public const string MANAGEMENT_OFFICE_ID = "searchCondition_mgOfficeID";
        public const string MGO_AMBI = "searchCondition_MGOffice";
        public const string MODEL_ID = "searchCondition_modelID";
        public const string TYPE_ID = "searchCondition_typeID";
        public const string SERIAL_NUMBER = "searchCondition_serialNo";
        public const string POSTAL_CODE = "searchCondition_postalCD";
        public const string ADDRESS = "searchCondition_address";
        public const string OPERATING_TIME = "searchCondition_OPtime";
        public const string NICKNAME = "searchCondition_nickName";
        public const string SETTING_DATE_S = "searchCondition_settingDate_S";
        public const string SETTING_DATE_E = "searchCondition_settingDate_E";
        public const string ALERT_S = "searchCondition_alert_start";
        public const string ALERT_E = "searchCondition_alert_end";
        public const string MAIL = "searchCondition_mail";
        public const string PARTS_ALERT = "searchCondition_partsAlert";
        public const string MACHINE_ALERT = "searchCondition_machineAlert";
        public const string DAY_OR_MONTH = "searchCondition_dayOrMonth";
        public const string ALERT_NO = "searchCondition_alertNo";
        public const string ALERT_NAME = "searchCondition_alertName";
        public const string ALERT_LEVEL = "searchCondition_alertLevel";
        public const string ALERT_LEVEL_STRING = "searchCondition_alertLevelStr";
        public const string SORT_COL = "searchCondition_sortColumn";
        public const string ORDER_DIRECTION = "searchCondition_orderDirection";
        public const string COLMUN_KEY = "searchCondition_sortColumnsKey";
        public const string PARTS_SORT_COL = "searchCondition_PartsSortColumn";
        public const string PARTS_ORDER_DIRECTION = "searchCondition_PartsOrderDirection";
        public const string PARTS_COLMUN_KEY = "searchCondition_PartsSortColumnsKey";
        public const string ALERT_SORT_COL = "searchCondition_AlertSortCol";
        public const string ALERT_COL_KEY = "searchCondition_AlertSortColKey";
        public const string ALERT_ORDER_DIRECTION = "searchCondition_AlertOrderDirection";
        public const string HISTORY_SORT_COL = "searchCondition_HistorySortCol";
        public const string HISTORY_COL_KEY = "searchCondition_HistorySortColKey";
        public const string HISTORY_ORDER_DIRECTION = "searchCondition_HistoryOrderDirection";

        //infoList用
        public const string INFO_LIST = "infoList";
    }

    /// <summary>
    /// Detailページに表示されるグラフデータ用Sessionに使用
    /// </summary>
    public class GraphData_Label
    {
        public const string GRAPH_ALERT_S = "glaphData_Alert_S";
        public const string GRAPH_ALERT_E = "glaphData_Alert_E";
    }

    /// <summary>
    /// 多言語対応用
    /// とりあえず日本語と英語のID
    /// </summary>
    public class LangConst
    {
        public const int JAPANESE_CODE = 1;
        public const int ENGLISH_CODE = 2;
    }
}
