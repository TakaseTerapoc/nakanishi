using nakanishiWeb.Const;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.DataAccess
{
    //言語構造定義
    public class LanguageData
    {
        public int id;
        public string name;
        public string code;

        /// <summary>
        /// コンストラクタ DB:lang_master
        /// </summary>
        /// <param name="id">DB:lang_id</param>
        /// <param name="name">DB:lang_name</param>
        /// <param name="code">DB:lang_code</param>
        public LanguageData(int id,string name,string code) {
            this.id = id;
            this.name = name;
            this.code = code;
        }
    }

    ////ユーザー定義
    //public class UserData
    //{
    //    public int userID;
    //    public int companyID;
    //    public int factoryID;
    //    public int kind;
    //    public string userName;
    //    public string loginID;
    //    public string password;
    //    public string affiliation;
    //    public string position;
    //    public string mailAddress;
    //    public string phoneNo;
    //    public int langID;

    //    /// <summary>
    //    /// コンストラクタ DB:user_master
    //    /// </summary>
    //    /// <param name="userID">ユーザーID DB:user_id</param>
    //    /// <param name="companyID">所属会社ID DB:company_id</param>
    //    /// <param name="factoryID">工場ID DB:factory_id</param>
    //    /// <param name="kind">管理者か一般か DB:kind</param>
    //    /// <param name="userName">ユーザー名 DB:user_name</param>
    //    /// <param name="loginID">ログインID DB:login_id</param>
    //    /// <param name="password">パスワード DB:password</param>
    //    /// <param name="affiliation">所属部署 DB:affiliation</param>
    //    /// <param name="position">役職 DB:position</param>
    //    /// <param name="mailAddress">メールアドレス DB:mail_address</param>
    //    /// <param name="phoneNo">電話番号 DB:tel</param>
    //    /// <param name="langID">母国語ID DB:lang_id</param>
    //    public UserData(int userID,int companyID,int factoryID,int kind,string userName,string loginID,string password,string affiliation,string position,string mailAddress,string phoneNo,int langID)
    //    {
    //        this.userID = userID;
    //        this.companyID = companyID;
    //        this.factoryID = factoryID;
    //        this.kind = kind;
    //        this.userName = userName;
    //        this.loginID = loginID;
    //        this.password = password;
    //        this.affiliation = affiliation;
    //        this.position = position;
    //        this.mailAddress = mailAddress;
    //        this.phoneNo = phoneNo;
    //        this.langID = langID;
    //    }
    //}

    //ログイン情報
    public class LoginData
    {
        public int userID;              // ユーザーID
        public int companyID;           // 会社ID
        public string companyName;      // 所属会社名
        public int companyKind;         // 所属会社の権限
        public int branchID;            // 支店ID
        public string branchName;       // 所属支店名
        public string affiliation;      // 部署名
        public int generalBranchID;     //
        public string userName;         // ユーザー名
        public string loginID;          // ログインID
        public int kind;                // ユーザー権限【０:管理者・１:一般】
        public int timeDiff;            // ユーザー時差[min]
        public int langID;              // 言語ID
        public string langCode;         // 言語コード
        public TimeSpan mailStartTime;  // メール開始時刻
        public TimeSpan mailEndTime;    // メール終了時刻
    }

    //ブランチ情報定義（仮）
    public class Branch
    {
        public int branchID;
        public string branchName;

        public Branch() { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ブランチID</param>
        /// <param name="name">ブランチ名</param>
        public Branch(int id, string name)
        {
            this.branchID = id; ;
            this.branchName = name;
        }
    }

    //会社情報定義（サンプル）
    public class Company
    {
        public int companyID;
        public string companyName;
        public string postalCode;
        public string address;
        public int MGOfficeID;
        public string MGOfficeName;
        public int connectionCompanyID; //endUserの時⇒clientID
        public string connectionCompanyName; //endUserの時⇒clientName

        public Company() { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="companyID">会社ID</param>
        /// <param name="name">会社名</param>
        public Company(int companyID,string name)
        {
            this.companyID = companyID;
            this.companyName = name;
        }
    }

    //マシン基本定義（検索条件に使用: searchCondition or searchCond）
    //デフォルトstring=>"" , int=>-1
    public class MachineBase
    {
        public int machineID = ConstData.SEARCH_ALL;      //マシンID
        public string machineName = ConstData.EMPTY;         //マシン名称

        public int companyID = ConstData.SEARCH_ALL;      //所有している会社のID
        public string companyName = ConstData.EMPTY;         //所有している会社の名称
        public string companyName_ambi = ConstData.SEARCH_ALL.ToString();   //あいまい検索用

        public int endUserCode =  ConstData.SEARCH_ALL;       //設置されている工場のID
        public string endUserName = ConstData.EMPTY;         //設置されている工場の名称
        public string enduserName_ambi = ConstData.SEARCH_ALL.ToString();

        public int comIDofMGOffice = ConstData.NAKANISHI_ID;      //担当支店営業所のcompanyID
        public int managementOfficeID = ConstData.SEARCH_ALL;   //担当支店営業所のbranchID
        public string managementOffice = ConstData.EMPTY;        //担当支店営業所名
        public string mgo_ambi = ConstData.SEARCH_ALL.ToString();   //あいまい検索用

        public int makerID = ConstData.SEARCH_ALL;        //メーカーID
        public string makerName = ConstData.EMPTY;//メーカ名称

        public int modelID = ConstData.SEARCH_ALL;        //マシンモデルID
        public string modelName = ConstData.EMPTY;//マシンモデル名称


        public string typeID = ConstData.SEARCH_ALL.ToString();//品名ID
        public string typeName = ConstData.EMPTY;//マシンタイプ名称

        public string typeIOK = ConstData.EMPTY;//型式
        public string serialNumber = ConstData.SEARCH_ALL.ToString();//シリアルナンバー
        public string IPAddress = ConstData.EMPTY;
        public string gatewayID = ConstData.EMPTY;
        public int operatingTime = ConstData.SEARCH_ALL;
        public string postalCode = ConstData.SEARCH_ALL.ToString();
        public string address = ConstData.SEARCH_ALL.ToString();
        public string mail = ConstData.EMPTY;
        public DateTime contractDate;           //契約日
        public DateTime settingDate_s;           //納品日
        public DateTime settingDate_e;           //納品日
        public DateTime alertStart; //アラート　期間検索用　スタート値
        public DateTime alertEnd; //アラート　期間検索用　エンド値
        public int partsAlert = ConstData.SEARCH_ALL;         //部品交換アラート [-1:全て・1:あり・0なし]
        public int machineAlert = ConstData.SEARCH_ALL;       //機械アラート[-1:全て・1:あり・0なし]
        public int alertNo = ConstData.SEARCH_ALL;    //アラート種別のID
        public string alertName = ConstData.EMPTY;    //アラート種別
        public int alertLevel = ConstData.SEARCH_ALL;
        public string alertLevelString = ConstData.EMPTY;

        public DateTime lastTime;   //最終通信時間

        /// <summary>
        /// Sessionにある値から検索条件をセットする
        /// </summary>
        /// <remarks>Sessionの中身がNullでなければ自身のプロパティにSessionの値をセット</remarks>
        public void SetSearchCondition_Self()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            if(hc.Session[S_ConditionLabel.CLIENT_NAME_AMBI] != null)
            {
                this.companyName_ambi = hc.Session[S_ConditionLabel.CLIENT_NAME_AMBI].ToString();
            }
            if(hc.Session[S_ConditionLabel.ENDUSER_NAME_AMBI] != null)
            {
                this.enduserName_ambi = hc.Session[S_ConditionLabel.ENDUSER_NAME_AMBI].ToString();
            }
            if(hc.Session[S_ConditionLabel.MACHINE_ID] != null) {
                this.machineID = int.Parse(hc.Session[S_ConditionLabel.MACHINE_ID].ToString()) ;
            }
            if(hc.Session[S_ConditionLabel.CLIENT_CODE] != null) {
                this.companyID = int.Parse(hc.Session[S_ConditionLabel.CLIENT_CODE].ToString()) ;
                this.companyName = hc.Session[SearchLabel.CLIENT_NAME].ToString();
            }
            if(hc.Session[S_ConditionLabel.END_USER_CODE] != null) {
                this.endUserCode = int.Parse(hc.Session[S_ConditionLabel.END_USER_CODE].ToString()) ;
                this.endUserName = hc.Session[SearchLabel.END_USER_NAME].ToString();
            }
            if(hc.Session[S_ConditionLabel.MANAGEMENT_OFFICE_ID] != null) {
                this.managementOfficeID = int.Parse(hc.Session[S_ConditionLabel.MANAGEMENT_OFFICE_ID].ToString());
                this.managementOffice = hc.Session[SearchLabel.MANAGEMENT_OFFICE].ToString();
            }
            if(hc.Session[S_ConditionLabel.MGO_AMBI] != null) {
                this.mgo_ambi = hc.Session[S_ConditionLabel.MGO_AMBI].ToString();
            }
            if(hc.Session[S_ConditionLabel.MODEL_ID] != null) {
                this.modelID =  int.Parse(hc.Session[S_ConditionLabel.MODEL_ID].ToString()) ;
                this.modelName = hc.Session[SearchLabel.MODEL_NAME].ToString();
            }
            if (hc.Session[S_ConditionLabel.TYPE_ID] != null){
                this.typeID = hc.Session[S_ConditionLabel.TYPE_ID].ToString();
                this.typeName = hc.Session[SearchLabel.TYPE_NAME].ToString();
            }
            if (hc.Session[S_ConditionLabel.SERIAL_NUMBER] != null){
                this.serialNumber = hc.Session[S_ConditionLabel.SERIAL_NUMBER].ToString();
            }
            if(hc.Session[S_ConditionLabel.PARTS_ALERT] != null) {
                this.partsAlert = int.Parse(hc.Session[S_ConditionLabel.PARTS_ALERT].ToString());
            }
            if(hc.Session[S_ConditionLabel.MACHINE_ALERT] != null) {
                this.machineAlert = int.Parse(hc.Session[S_ConditionLabel.MACHINE_ALERT].ToString());
            }
            if((hc.Session[S_ConditionLabel.ALERT_NO] != null) && (int.Parse(hc.Session[S_ConditionLabel.ALERT_NO].ToString()) != ConstData.SEARCH_ALL))
            {
                this.alertNo = int.Parse(hc.Session[S_ConditionLabel.ALERT_NO].ToString());
                this.alertName = hc.Session[S_ConditionLabel.ALERT_NAME].ToString();
            }
            if((hc.Session[S_ConditionLabel.ALERT_LEVEL] != null) && (int.Parse(hc.Session[S_ConditionLabel.ALERT_LEVEL].ToString()) != ConstData.SEARCH_ALL))
            {
                this.alertLevel = int.Parse(hc.Session[S_ConditionLabel.ALERT_LEVEL].ToString());
                this.alertLevelString = hc.Session[S_ConditionLabel.ALERT_LEVEL_STRING].ToString();
            }
            if((hc.Session[S_ConditionLabel.ALERT_S] != null) && (hc.Session[S_ConditionLabel.ALERT_S].ToString() != ConstData.EMPTY))
            {
                this.alertStart = (DateTime)hc.Session[S_ConditionLabel.ALERT_S];
            }
            if(hc.Session[S_ConditionLabel.ALERT_E] != null)
            {
                this.alertEnd = (DateTime)hc.Session[S_ConditionLabel.ALERT_E];
            }
            if(hc.Session[S_ConditionLabel.SETTING_DATE_S] != null)
            {
                this.settingDate_s = (DateTime)hc.Session[S_ConditionLabel.SETTING_DATE_S];
            }
            if(hc.Session[S_ConditionLabel.SETTING_DATE_E] != null)
            {
                this.settingDate_e = (DateTime)hc.Session[S_ConditionLabel.SETTING_DATE_E];
            }
         }
    }

    //マシン定義
    public class Machine
    {
        public int machineID;   //マシンID
        public string machineName;    //マシン名
        public int companyID;   //所有企業ID
        public string companyName;  //所有企業名
        public int endUserCode;     //所有企業の本社・支店・工場などのID
        public string endUserName;     //所有企業の本社・支店・工場などの名称
        public int managementOfficeID;     //担当支店営業所ID
        public string managementOffice;     //担当支店営業所
        public DateTime settingDate;    //納品日
        public int modelID;     //マシンモデルID
        public string modelName;    //マシンモデル名(例：洗浄機器)
        public string typeID;   //タイプID(AKTとかSVDとか)
        public string typeName;     //品名(例：試験機)
        public string typeIOK;      //型式(例：SVC-ｄ)
        public string serialNumber;     //シリアルナンバー
        public string postalCode;   //郵便番号
        public string settingPlace;     //所有企業の本社・支店・工場などの住所
        public long actualTime;  //マシンの通電時間
        public string operateHour;  //マシンの通電時間
        public bool isMachineAlert = false;   //
        public bool isPartsAlert = false; //
        public DateTime lastTime;   //最終通信時間

        public Machine() { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="machineID">マシンID</param>
        /// <param name="modelID">モデルID</param>
        /// <param name="companyName">会社名 DB:company_master[company_name]</param>
        /// <param name="settingPlace">設置工場名 DB:factory_master[factory_name]</param>
        /// <param name="date">設置日 DB:machine_master[delivery_date]</param>
        /// <param name="type">機器種類 DB:type_master[type_name]</param>
        /// <param name="sNumber">シリアルナンバー DB:machine_master[serial_number]</param>
        public Machine(int machineID,int modelID,string companyName,string settingPlace,string date,string type,string sNumber)
        {
            this.machineID = machineID;
            this.modelID = modelID;
            this.companyName = companyName;
            this.settingPlace = settingPlace;
            this.settingDate = DateTime.Parse(date);
            this.typeName = type;
            this.serialNumber = sNumber;
        }
    }

    //アラート定義
    public class Alert
    {
        public int alertID;
        public int alertNo;
        public string partInfo;
        public string partsName;
        public string alertName;
        public int alertLevel;
        public string alertLevelString;
        public DateTime occurTime;
        public DateTime releaseTime;
        public int machineID;
        public int companyID;
        public string companyName;
        public int endUserID;
        public string endUserName;
        public int modelID;
        public string modelName;
        public string typeID;
        public string typeName;
        public string machineSerialNumber;
        public int totalTimeOrCount;
        public DateTime settingDate;
        public int MGOfficeID;
        public string MGOfficeName;
        public string detailString;
        public string causeString;
        public string maintenanceString;
        public bool isNowAlert = false;


        public Alert() { }
    }

    public class AlertKind
    {
        public int alertNo;
        public string alertName;
    }

    //「型式」定義
    public class Model
    {
        public int makerID;
        public int modelID;
        public string modelName;

        public Model() { }

        public Model(int makerID,int modelID,string modelName)
        {
            this.makerID = makerID;
            this.modelID = modelID;
            this.modelName = modelName;
        }
    }

    //「品名」定義
    public class MachineType
    {
        public int makerID;
        public int modelID;
        public string typeID;
        public string typeName;

        public MachineType(int makerID,int modelID,string typeID,string name)
        {
            this.makerID = makerID;
            this.modelID = modelID;
            this.typeID = typeID;
            this.typeName = name;
        }
    }

    //システムログinsert用
    public class SystemLogData
    {
        public DateTime entryTime; //処理時刻(GMT)
        public int kind;                      //処理種別
        public string contents;         //内容
    }

    //並べ替え用クラス
    public class SortBase
    {
        public string sortColumn;　//ソート項目
        public string orderDirection;　//ASC or DESC
        public string columnsKey;   //Constのカラム名Dictionaryから値をとるためのキー

        public SortBase() { }

        /// <summary>
        /// Sessionの内容を自身のプロパティにセットする
        /// </summary>
        public void SetHandoverValue_Self()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            this.sortColumn = hc.Session[S_ConditionLabel.SORT_COL].ToString();
            this.orderDirection = hc.Session[S_ConditionLabel.ORDER_DIRECTION].ToString();
            this.columnsKey = hc.Session[S_ConditionLabel.COLMUN_KEY].ToString();
        }

        /// <summary>
        /// プロパティの値をSessionに格納する
        /// </summary>
        public void SessionValueSet()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session[S_ConditionLabel.SORT_COL] = this.sortColumn;
            hc.Session[S_ConditionLabel.ORDER_DIRECTION] = this.orderDirection;
            hc.Session[S_ConditionLabel.COLMUN_KEY] = this.columnsKey;
        }

        /// <summary>
        /// デフォルト値を各ページからセット
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortColKey"></param>
        /// <param name="orderDirection"></param>
        public void SetDefaultValue(string sortCol, string sortColKey, string orderDirection)
        {
            this.sortColumn = sortCol;
            this.columnsKey = sortColKey;
            this.orderDirection = orderDirection;
        }

    }

    //「部品」定義
    public class Parts
    {
        public int partsID;     //パーツ固有ID
        public int machineID;   //このパーツが使われているマシンID
        public string partsName;    //パーツ名
        public int partsNo;
        public long exchangeGuideline_time;   //推奨交換目安
        public long exchangeGuideline_count;   //推奨交換目安
        public string exchangeGuideTimeString;   //推奨交換目安を日時にしたもの
        public string exchangeGuideCountString;   //推奨交換目安回数を文字列にしたもの
        public long DBtotalTime;
        public long totalTime;  //累計稼働
        public long initTime;
        public long DBtotalCount;
        public long totalCount;  //累計稼働
        public long initCount;
        public string operateHourString;
        public string operateCountString;
        public long remainingOperate_time;  //exchangeGuideline - ( totalOperate + initOperate )
        public long remainingOperate_count;  //exchangeGuideline - ( totalOperate + initOperate )
        public string remainingOperateHourString;
        public string remainingOperateCountString;
        public DateTime lastExchangeDate;   //前回交換日
        public int exchangeCount;   //トータル交換回数
        public bool isExchangeAlert = false; //交換アラートが出ているかどうか = remainingOperateTime < totalTime : true

        public Parts() { }
    }

    public class PartsKind
    {
        public int partsKindId;
        public string partsName;
        public long lifeTime;
        public long lifeCycle;
    }

    public class WorkHistory
    {
        public DateTime occurTime;
        public DateTime endTime;
        public int workNo;
        public int workStatus;
        public int pitch;
        public int tooolLength;
        public double toolDiameter;

        public WorkHistory() { }

        public  double GetTotalWorkTime(DateTime specDay)
        {
            double result = 0.0;
            DateTime startDate = this.occurTime;
            DateTime endDate = this.endTime;
            if(this.occurTime <= specDay)
            {
                startDate = specDay;
            }
            if(this.endTime > specDay.AddDays(1))
            {
                endDate = specDay.AddDays(1);
            }
            if((this.occurTime > specDay.AddDays(1)) || (this.endTime < specDay))
            {
                result = 0.0;
            }
            TimeSpan diff = endDate - startDate;
            result = diff.TotalMinutes;
            return result;
        }
    }

    //ページャー操作クラス
    public class PagerController
    {
        public int totalObject;//検索結果の総数
        public int totalPageCount;//全部で何ページになるか
        public int limit;//1ページに何件表示するか
        public int nowPageNo;//今見ているいるページのインデックス
        public int offset;//いくつ目のデータから表示するか。SQL文のOFFSET句にも使用
        public const int DEFAULT_PAGE_NO = 1;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="total">検索結果総数</param>
        /// <param name="limit">１ページに表示する項目数</param>
        public PagerController(int total,int limit) {
            this.totalObject = total;
            this.totalPageCount = (int)Math.Ceiling((double)this.totalObject / limit);
            this.limit = limit;
            this.nowPageNo = 1;
            this.offset = 0;
        }

        /// <summary>
        /// Sessionの値を自身のプロパティにセットする
        /// </summary>
        public void SetHandoverValue_Self()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            //SessionのページナンバーとオフセットがNULLでなければ、pagerにセット
            if (hc.Session[SearchLabel.PAGE_NO_TEMP] != null ) { this.nowPageNo = int.Parse(hc.Session[SearchLabel.PAGE_NO_TEMP].ToString()); }
            if (hc.Session[SearchLabel.PAGE_OFFSET_TEMP] != null) { this.offset = int.Parse(hc.Session[SearchLabel.PAGE_OFFSET_TEMP].ToString()); }
        }

        /// <summary>
        /// プロパティの値をSessionに格納する
        /// </summary>
        public void SessionValueSet()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session[SearchLabel.PAGE_NO_TEMP] = this.nowPageNo;
            hc.Session[SearchLabel.PAGE_OFFSET_TEMP] = this.offset;
        }

        /// <summary>
        /// 現ページインデックス用のSetter
        /// </summary>
        /// <param name="now">現在見ているページのインデックス</param>
        public void SetNowPageNo(int now) {
            this.nowPageNo = now;
        }

        /// <summary>
        /// １ページに表示する項目数をセット
        /// </summary>
        /// <param name="limit">１ページに表示したい項目数</param>
        public void SetLimit(int limit)
        {
            this.limit = limit;
            this.totalPageCount = (int)Math.Ceiling((double)this.totalObject / limit);
        }

        /// <summary>
        ///ページ総数を計算してセット
        /// </summary>
        public void SetTotalPageCount()
        {
            /// 検索総数を１ページに表示する数で割って、割り切れない分は１ページとしてカウントする
            this.totalPageCount = (int)Math.Ceiling((double)this.totalObject / this.limit);
        }

        /// <summary>
        /// 何個目の項目から見たいかをセット
        /// <remarks>SetNowPageNとセットで使用。oインスタンスの現在ページインデックスと、１ページごとの項目数から自動でセット</remarks>
        /// </summary>
        public void SetOffset()
        {
            this.offset = (this.nowPageNo-1)*this.limit;
        }

        /// <summary>
        /// ページNoをデフォルトの値に戻して、オフセットの再計算をする
        /// </summary>
        public void SetDefaultPageNoAndOffset()
        {
            this.nowPageNo = DEFAULT_PAGE_NO;
            this.SetOffset();
        }

        /// <summary>
        ///「〇件～〇件まで」のスタートの方を取得
        /// </summary>
        /// <remarks>
        /// 例）現在ページ⇒1、1ページごとの項目数⇒10
        /// 「1件～10件まで」と表示する。
        /// 式）(1-1)＊10＋1 = 1
        /// </remarks>
        /// <returns>スタート値（例の場合、1）</returns>
        public int GetStartObjectCount() {
            int start = (this.nowPageNo - 1) * this.limit + 1;
            if (this.totalObject == 0)
            {
                start = 0;
            }
            return start;
        }

        /// <summary>
        ///「〇件～〇件まで」のエンドの方を取得
        /// </summary>
        /// <remarks>
        /// 例）現在ページ⇒2、1ページごとの項目数⇒10、トータルの項目数⇒25
        /// 「11件～20件まで」と表示する。
        /// 式）「25」か「2＊10」の小さいほう
        /// </remarks>
        /// <returns>エンド値（例の場合、20）</returns>
        public int GetEndObjectCount() {
            int end = Math.Min(this.totalObject,this.nowPageNo*this.limit);
            return end;
        }
    }
}
