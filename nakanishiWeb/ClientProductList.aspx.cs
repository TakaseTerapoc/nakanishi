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
    public partial class ClientProductList : System.Web.UI.Page
    {
        //:::: データ取得用のインスタンス
        public PagerController pager;    //ページャーのコントロール用インスタンス
        public DataAccessObject _dbObj = new DataAccessObject();
        public DB_BranchMaster branchMaster;
        public DB_ModelMaster modelMaster;
        public DB_TypeMaster typeMaster;
        public DB_CompanyMaster companyMaster;
        public DB_MachineMaster machineMaster;
        public DB_Lang dbLang;

        //:::: 取得したデータ格納するリスト
        public List<Branch> adminBranchList;
        public List<Branch> clientBranchList;
        public List<Machine> machineList;
        public List<Model> modelList;
        public List<MachineType> typeList;
        public MachineBase searchCondition;
        public Dictionary<int, string> typeIokList;


        //:::: 表示用の文字と文言リスト・リスト取得の上限
        public Dictionary<string, string> commonWordList = Common.commonWords;   //共通の文言リスト(DB_Lang)
        public Dictionary<string, string> wordList = Common.productPageWords;
        public string pageName = ConstData.PRODUCT_PAGE_NAME;
        public string common = ConstData.COMMON;
        public List<string> searchInfoList;
        public string searchInfo;
        public Company selectCompany;
        public int limit = ConstData.PRODUCT_LIMIT;
        public char split = ConstData.VALUE_SPLIT_CHAR; //"," valueを表示用と検索用に分けるために使用
        public int factories = 10;

        //MachineDBできるまで
        public int headerCount = 14;
        public string[] machineNames = { "マシンA","マシンB","マシンC","マシンD","マシンE","マシンF","マシンG","マシンH","マシンI","マシンJ","マシンK","マシンL","マシンM","マシンN","マシンO","マシンP","マシンQ","マシンR","マシンS","マシンT","マシンU","マシンV","マシンW","マシンX","マシンY","マシンZ"};
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //:::: セッションのチェック
            Funcs.IsSessionAlive();

            //:::: 前頁からの会社IDを取得
            int companyID;
            if(Request.QueryString[SearchLabel.COMPANY_ID] != null)
            {
                companyID = int.Parse(Request.QueryString[SearchLabel.COMPANY_ID]);
            }
            else
            {
                companyID = ConstData.NAKANISHI_ID;
            }

            //:::: 表示用データの取得
            companyMaster = new DB_CompanyMaster(this._dbObj);
            companyMaster.GetCompanyOne(out selectCompany,companyID);
            branchMaster = new DB_BranchMaster(this._dbObj);
            branchMaster.GetAdminBranchList(out adminBranchList);
            branchMaster.GetSelectClientBranchList(out clientBranchList,companyID);
            modelMaster = new DB_ModelMaster(this._dbObj);
            modelMaster.GetModelList(out modelList);
            typeMaster = new DB_TypeMaster(this._dbObj);
            typeMaster.GetAllTypeList(out typeList);
            machineMaster = new DB_MachineMaster(_dbObj);
            machineMaster.GetTypeIokList_All(out typeIokList);

            //:::: DB入力までの処理
            machineList = new List<Machine>();
            for(int i=0; i < machineNames.Length; i++)
            {
                Machine machine = new Machine(i+1,1,"SAMPLE","sample","20XX/00/00","sample",$"Serial_{i}");
                machine.machineName = machineNames[i];
                machineList.Add(machine);
            }

            //:::: ページャーへ値のセット ＆ ページャーからの送信かをチェック
            pager = new PagerController(machineList.Count(),limit);
            if((Request.Form[SearchLabel.PAGER] != null) && (Request.Form[SearchLabel.PAGER] != ""))
            {
                pager.SetNowPageNo(int.Parse(Request.Form[SearchLabel.PAGER]));
                pager.SetOffset();
            }

            //::::: 検索ボックスからの送信かをチェック
            searchInfo = commonWordList[$"{common}_28"];
            if((Request.Form[SearchLabel.SEARCH_BT] != null) && (Request.Form[SearchLabel.SEARCH_BT] == "true"))
            {
                searchCondition = new MachineBase();
                Common.GetRefiningCondition(ref searchCondition,out searchInfoList);
            }
            if((!Funcs.IsNotNullObject(searchInfoList)) || (searchInfoList.Count() == 0))//検索条件リストがNULLか空っぽの時
            {
                string key = $"{common}_47";
                searchInfo += $"【{commonWordList[key]}】";
            }
            else//検索条件リストに中身があった時
            {
                for(int i = 0; i < searchInfoList.Count(); i++)
                {
                        searchInfo += $"<p class=\"inline marker bold\">{searchInfoList[i]}</p>";
                }
            }
        }


        protected void Page_UnLoad()
        {
            _dbObj.DBClose();
        }
    }
}