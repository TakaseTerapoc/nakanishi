using Npgsql;
using nakanishiWeb.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace nakanishiWeb.DataAccess
{
    public class DB_CompanyMaster:DBBase
    {
        public DB_CompanyMaster(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// 検索ボックスに使用
        /// クライアントの名前とＩＤをデータベースから取得しリストに格納
        /// </summary>
        /// <param name="companyList">データ格納用リスト</param>
        public void SearchClientList(out List<Company> companyList,string sortColumn,string order) {
            companyList = new List<Company>();
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            int langID = int.Parse(hc.Session[SearchLabel.LANGUAGE].ToString());
            string sql = this.searchClientSELECT_SQL + this.searchClientFROM_SQL;
            string orderSQL = $"ORDER BY {sortColumn} {order} ";
            
            this.PlusWhereWordByAdminFlag(ref sql);
            sql += orderSQL;

            Debug.Print("SearchClientList : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["company_id"]);
                        company.companyName = GetDataReaderString(reader["company_name"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        companyList.Add(company);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 会社の名前の一部からＩＤとクライアント名をデータベースから取得しリストに格納
        /// </summary>
        /// <remarks>あいまい検索用</remarks>
        /// <param name="companyList">データ格納用リスト</param>
        /// <param name="companyName">会社名</param>
        public void SearchClientListFromName(string companyName,out List<Company> companyList,string sortColumn,string order) {
            companyList = new List<Company>();
            string sql = this.searchClientSELECT_SQL + this.searchClientFROM_SQL 
                +$"AND (kind={ConstData.COMPANY_KIND_CLIENT} OR kind=1) AND company_name LIKE '%{companyName}%' ";
            string orderSQL = $"ORDER BY {sortColumn} {order} ";

            this.PlusWhereWordByAdminFlag(ref sql);
            sql += orderSQL;

            Debug.Print("SearchClientListFromName : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["company_id"]);
                        company.companyName = GetDataReaderString(reader["company_name"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        companyList.Add(company);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// エンドユーザーの名前とＩＤをデータベースから取得しリストに格納
        /// </summary>
        /// <param name="companyList">データ格納用リスト</param>
        public void SearchAllEndUserList(out List<Company> ebdUserList,int langID) {
            ebdUserList = new List<Company>();
            string sql = this.searchEndUserSELECT_SQL + this.searchEndUserFROM_SQL;
            this.PlusWhereWordByAdminFlag(ref sql);
            sql += " order by clientID";

            Debug.Print("SearchAllEndUserList : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["endUserID"]);
                        company.companyName = GetDataReaderString(reader["endUserName"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        company.connectionCompanyName = GetDataReaderString(reader["clientName"]);
                        ebdUserList.Add(company);
                    }
                }
            }
            connection.Close();
        }
        /// <summary>
        /// エンドユーザーの名前の一部からＩＤと会社名をデータベースから取得しリストに格納
        /// </summary>
        /// <remarks>あいまい検索用</remarks>
        /// <param name="endUserList">データ格納用リスト</param>
        /// <param name="companyName">会社名</param>
        public void SearchEndUserList_byName(string companyName,out List<Company> endUserList) {
            endUserList = new List<Company>();
            string sql = this.searchClientSELECT_SQL + this.searchClientFROM_SQL +
                $"AND kind={ConstData.COMPANY_KIND_ENDUSER} AND company_name LIKE '%{companyName}%' ";
            string orderSQL = $"ORDER BY company_id ASC ";
            this.PlusWhereWordByAdminFlag(ref sql);
            sql += orderSQL;

            Debug.Print("SearchEndUserList_byName : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["company_id"]);
                        company.companyName = GetDataReaderString(reader["company_name"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        endUserList.Add(company);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 検索ボックス用のクライアントリスト取得(オーバーロード)メソッド
        /// </summary>
        /// <param name="searchCondition">検索条件インスタンス</param>
        /// <param name="endUserList">エンドユーザー格納リスト</param>
        /// <param name="langID">言語ID</param>
        public void SearchClient_bySearchCondition(ref MachineBase searchCondition,out List<Company>endUserList,int langID)
        {
            endUserList = new List<Company>();   //DBから取得したマシン情報を格納するリスト
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト
            string sql = this.searchClientSELECT_SQL + this.searchClientFROM_SQL + "AND ";
            string countSQL = this.countCompanySELECT_SQL + this.searchClientFROM_SQL + $"AND ";
            string orderSQL = $"ORDER BY cm.company_name ASC ";

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if(searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            //whereSQLs.Add($"{this.langSQL}{langID} ");     //lang_idのWHERE句は必ずリストに追加

            //::::: リストに追加されたWHERE句のSQL文のみサーチ用SQLに追加
            for(int i=0; i<whereSQLs.Count; i++)
            {
                sql += whereSQLs[i];
                countSQL += whereSQLs[i];
                if(i != whereSQLs.Count - 1)//最後の要素でなければ
                {
                    sql += "AND ";
                    countSQL += "AND ";
                }
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);
            this.PlusWhereWordByAdminFlag(ref countSQL);
            
            //::::: WHERE句リストを全て追加したら、ORDER BY句も追加
            sql += orderSQL;

            Debug.Print("SearchClient_bySearchCondition : " + sql);

            //::::: DB接続
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["company_id"]);
                        company.companyName = GetDataReaderString(reader["company_name"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        endUserList.Add(company);
                    }
                }
            }
            connection.Close();
        }
        /// <summary>
        /// 検索ボックス用のエンドユーザーリスト取得メソッド
        /// </summary>
        /// <param name="searchCondition">検索条件インスタンス</param>
        /// <param name="endUserList">エンドユーザー格納リスト</param>
        /// <param name="langID">言語ID</param>
        public void SearchEndUsersForSearchBox_bySearchCondition(ref MachineBase searchCondition,out List<Company>endUserList,int langID)
        {
            endUserList = new List<Company>();   //DBから取得したマシン情報を格納するリスト
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト
            string sql = this.searchEndUserSELECT_SQL + this.searchEndUserFROM_SQL + "AND ";
            string countSQL = this.countCompanySELECT_SQL + this.searchEndUserFROM_SQL + $"AND ";
            string orderSQL = $"ORDER BY cm.company_name ASC ";
            string kindSQL = $"cm2.kind={ConstData.COMPANY_KIND_ENDUSER} ";

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if(searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if(searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            //whereSQLs.Add($"{this.langSQL}{langID} ");     //lang_idのWHERE句は必ずリストに追加
            whereSQLs.Add(kindSQL);     //kindのWHERE句も必ずリストに追加

            //::::: リストに追加されたWHERE句のSQL文のみサーチ用SQLに追加
            for(int i=0; i<whereSQLs.Count; i++)
            {
                sql += whereSQLs[i];
                countSQL += whereSQLs[i];
                if(i != whereSQLs.Count - 1)//最後の要素でなければ
                {
                    sql += "AND ";
                    countSQL += "AND ";
                }
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);
            this.PlusWhereWordByAdminFlag(ref countSQL);
            
            //::::: WHERE句リストを全て追加したら、ORDER BY句も追加
            sql += orderSQL;

            Debug.Print("SearchEndUsersForSearchBox_bySearchCondition : " + sql);

            //::::: DB接続
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["endUserID"]);
                        company.companyName = GetDataReaderString(reader["endUserName"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        company.connectionCompanyName = GetDataReaderString(reader["clientName"]);
                        endUserList.Add(company);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 検索条件に沿って会社(エンドユーザー)情報を取得
        /// </summary>
        /// <param name="searchCondition">検索条件を持つインスタンス</param>
        /// <param name="pager">ページャー用(リミット・オフセットの情報を使用)</param>
        /// <param name="endUserList">会社情報を格納するリスト</param>
        /// <param name="langID">言語ID</param>
        public void SearchEndUsers_bySearchCondition(ref MachineBase searchCondition, ref PagerController pager, out List<Company> endUserList, ref SortBase sortBase, int langID, bool isAll)
        {
            endUserList = new List<Company>();   //DBから取得したマシン情報を格納するリスト
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト
            string sql = this.searchEndUserSELECT_SQL + this.searchEndUserFROM_SQL + "AND ";
            string countSQL = this.countCompanySELECT_SQL + this.searchEndUserFROM_SQL + $"AND ";
            string orderSQL = $"ORDER BY {sortBase.sortColumn} {sortBase.orderDirection} ";
            if (!isAll)
            {
                orderSQL += $"LIMIT { pager.limit} OFFSET { pager.offset}";
            }

            string kindSQL = $"cm2.kind={ConstData.COMPANY_KIND_ENDUSER} ";
            int temp_nowPageNo = pager.nowPageNo;   //現在のページNoを取っておく
            int temp_limit = pager.limit;   //同様にリミットも

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if(searchCondition.endUserCode != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.endUserCD_SQL}{searchCondition.endUserCode} "); }
            if(searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if(searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            //担当支店営業所
            if ((searchCondition.mgo_ambi != ConstData.SEARCH_ALL.ToString()) && (searchCondition.mgo_ambi != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.mgo_ambi.Contains("%")) {
                    whereSQLs.Add($"{this.MGOffice_LIKE_SQL}\'{searchCondition.mgo_ambi}\' "); 
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.MGOffice_SQL}\'{searchCondition.mgo_ambi}\' ");
                }
            }
            if ((searchCondition.companyName_ambi != ConstData.SEARCH_ALL.ToString()) && (searchCondition.companyName_ambi != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.companyName_ambi.Contains("%")) {
                    whereSQLs.Add($"{this.clientName_LIKE_SQL}\'{searchCondition.companyName_ambi}\' {this.clientKind_SQL} ");
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.clientName_SQL}\'{searchCondition.companyName_ambi}\' {this.clientKind_SQL} ");
                }
            }
            if ((searchCondition.enduserName_ambi != ConstData.SEARCH_ALL.ToString()) && (searchCondition.enduserName_ambi != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.enduserName_ambi.Contains("%")) {
                    whereSQLs.Add($"{this.enduserName_LIKE_SQL}\'{searchCondition.enduserName_ambi}\' {this.enduserKind_SQL} ");
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.enduserName_SQL}\'{searchCondition.enduserName_ambi}\' {this.enduserKind_SQL} ");
                }
            }
            //whereSQLs.Add($"{this.langSQL}{langID}");     //lang_idのWHERE句は必ずリストに追加
            whereSQLs.Add(kindSQL);     //kindのWHERE句も必ずリストに追加

            //::::: リストに追加されたWHERE句のSQL文のみサーチ用SQLに追加
            for(int i=0; i<whereSQLs.Count; i++)
            {
                sql += whereSQLs[i];
                countSQL += whereSQLs[i];
                if(i != whereSQLs.Count - 1)//最後の要素でなければ
                {
                    sql += "AND ";
                    countSQL += "AND ";
                }
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);
            this.PlusWhereWordByAdminFlag(ref countSQL);
            
            //::::: WHERE句リストを全て追加したら、ORDER BY句も追加
            sql += orderSQL;
            Debug.Print("SearchEndUsers_bySearchCondition : " + sql);

            //::::: DB接続
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.companyID = GetDataReaderInt(reader["endUserID"]);
                        company.companyName = GetDataReaderString(reader["endUserName"]);
                        company.postalCode = GetDataReaderString(reader["post_code"]);
                        company.address = GetDataReaderString(reader["place"]);
                        company.MGOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        company.MGOfficeName = GetDataReaderString(reader["MGOfficeName"]);
                        company.connectionCompanyName = GetDataReaderString(reader["clientName"]);
                        endUserList.Add(company);
                    }
                }
            }
            connection.Close();
            int totalMachineCount = this.GetTotalRecordCount(countSQL);
     //::::: ページャーがクリックされた時に持っていた情報でページャーを作り直し、ページナンバーとオフセット値は新しい数値に設定する
            pager = new PagerController(totalMachineCount,temp_limit);
            pager.SetNowPageNo(temp_nowPageNo);
            pager.SetOffset();
        }

        /// <summary>
        /// DBに登録されたエンドユーザー情報の総数を取得
        /// </summary>
        /// <remarks>ページャーのtotalObjectに使用</remarks>
        /// <returns>登録総数を表す整数値</returns>
        public int GetAllEndUserCount(int langID)
        {
            int result = 0;
            string sql = this.countCompanySELECT_SQL + this.searchEndUserFROM_SQL +
                $"AND cm.kind={ConstData.COMPANY_KIND_ENDUSER} ";

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("GetAllEndUserCount : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                result = int.Parse(command.ExecuteScalar().ToString());
            }
            connection.Close();
            return result;
        }
    }
}
