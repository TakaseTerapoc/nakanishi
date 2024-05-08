using nakanishiWeb.Const;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.DataAccess
{
    public class DB_AlertMaster:DBBase
    {
        public DB_AlertMaster(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// 検索ボックスに使用。IDとアラート名のみ取得する
        /// </summary>
        /// <param name="alertTypeList"></param>
        /// <param name="langID"></param>
        /// <param name="alertLimitNo">マシンアラートか部品交換アラートか</param>
        public void SearchAlertType(out List<Alert> alertTypeList,int langID,int alertLimitNo)
        {
            alertTypeList = new List<Alert>();
            string sign = alertLimitNo == 0 ? ">=" : "<" ;
            string sql = this.searchAlertTypeSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} WHERE alert_no{sign}{ConstData.PARTS_ALERT_BORDER} ";
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchAlertType : " + sql);
            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Alert alert = new Alert();
                        alert.alertID = GetDataReaderInt(reader["alert_no"]);
                        alert.alertName = GetDataReaderString(reader["alert_name"]);
                        alertTypeList.Add(alert);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// アラートのレベルを表す文言を取得する(AlertListの検索ボックス用)
        /// </summary>
        /// <param name="alertLevelList">格納用リスト</param>
        /// <param name="langID">言語￥ID</param>
        /// <param name="alertLimitNo">部品交換アラートか機械アラートかを表す数値</param>
        public void SearchAlertLevel(out List<Alert> alertLevelList,int langID,int alertLimitNo)
        {
            alertLevelList = new List<Alert>();
            string sign = alertLimitNo == 0 ? ">=" : "<" ;
            string sql = this.searchAlertLevelSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} WHERE alert_no{sign}{ConstData.PARTS_ALERT_BORDER} AND lr4.lang_id={langID} ";
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchAlertLevel : " + sql);
            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Alert alert = new Alert();
                        alert.alertLevel = GetDataReaderInt(reader["alert_level"]);
                        alert.alertLevelString = GetDataReaderString(reader["alertLevelString"]);
                        alertLevelList.Add(alert);
                    }
                }
            }
            connection.Close();
        }


        /// <summary>
        /// DBから部品交換アラートを検索
        /// </summary>
        /// <param name="partsAlertList">部品交換アラートリスト</param>
        /// <param name="langID">言語ID</param>
        /// <param name="pager">ページャー</param>
        /// <param name="machineID">マシンID(全件検索⇒-1)</param>
        /// <param name="date">検索する指定の日付け</param>
        public void SearchPartsAlert(out List<Alert> partsAlertList,int langID,ref PagerController pager,int machineID,DateTime date)
        {
            partsAlertList = new List<Alert>();
            string where = $"WHERE am.alert_no<{ConstData.PARTS_ALERT_BORDER} ";
            //date が "0001-01-01 00:00:00" でなければ
            if (date.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) {
                string specifyDateStart = date.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string specifyDateEnd = date.ToString(ConstData.TIME_STAMP_DATE_END_FORMAT);
                where += $"AND occur_time BETWEEN \'{specifyDateStart}\' AND \'{specifyDateEnd}\' " +
                    $"AND release_time IS NULL OR release_time BETWEEN \'{specifyDateStart}\' AND \'{specifyDateEnd}\' ";
            }
            if (machineID != ConstData.SEARCH_ALL) { where += $"AND mm.machine_id={machineID} "; }
            string sql = this.searchAlertSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} " + where;
            string countSQL = this.countAlertSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} " + where;
            int temp_nowPageNo = pager.nowPageNo;   //現在のページNoを取っておく
            int temp_limit = pager.limit;   //同様にリミットも

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);
            Debug.Print("SearchPartsAlert : " + sql);

            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Alert alert = new Alert();
                        alert.alertID = GetDataReaderInt(reader["alert_id"]);
                        alert.alertNo = GetDataReaderInt(reader["alert_no"]);
                        alert.partInfo = GetDataReaderString(reader["part_info"]);
                        alert.machineID = GetDataReaderInt(reader["machine_id"]);
                        alert.alertName = GetDataReaderString(reader["alert_name"]);
                        alert.occurTime = GetDataReaderTime(reader["occur_time"]);
                        //release_timeがNULLの時は、現在時刻をセット
                        alert.releaseTime = reader["release_time"] == null ? DateTime.Now :
                            GetDataReaderTime(reader["release_time"]);
                        alert.companyID = GetDataReaderInt(reader["clientID"]);
                        alert.companyName = GetDataReaderString(reader["clientName"]);
                        alert.endUserID = GetDataReaderInt(reader["endUserID"]);
                        alert.endUserName = GetDataReaderString(reader["endUserName"]);
                        alert.modelID = GetDataReaderInt(reader["model_id"]);
                        alert.modelName = GetDataReaderString(reader["modelName"]);
                        alert.MGOfficeID = GetDataReaderInt(reader["branch_id"]);
                        alert.MGOfficeName = GetDataReaderString(reader["MGOffice"]);
                        alert.typeID = GetDataReaderString(reader["type_id"]);
                        alert.typeName = GetDataReaderString(reader["typeName"]);
                        alert.machineSerialNumber = GetDataReaderString(reader["serial_number"]);
                        alert.settingDate = GetDataReaderTime(reader["delivery_date"]);
                        alert.detailString = GetDataReaderString(reader["detail"]);
                        alert.causeString = GetDataReaderString(reader["cause"]);
                        alert.maintenanceString = GetDataReaderString(reader["treatString"]);
                        partsAlertList.Add(alert);
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
        /// DBから機械アラートを検索して取得
        /// </summary>
        /// <param name="machineAlertList">機械アラートリスト</param>
        /// <param name="langID">言語ID</param>
        /// <param name="pager">ページャー</param>
        /// <param name="machineID">マシンID(全件検索⇒-1)</param>
        /// <param name="date">検索する指定の日付</param>
        public void SearchMachineAlert(out List<Alert> machineAlertList,int langID,ref PagerController pager,int machineID,DateTime date)
        {
            machineAlertList = new List<Alert>();
            string where = $"WHERE am.alert_no>{ConstData.PARTS_ALERT_BORDER} AND lr1.lang_id={langID} AND lr2.lang_id={langID} AND lr3.lang_id={langID} AND lr4.lang_id={langID} ";
            if (date.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) {
                string specifyDateStart = this.GetGmtTimeString(new DateTime(date.Year,date.Month,date.Day,0,0,0));
                string specifyDateEnd = this.GetGmtTimeString(new DateTime(date.Year,date.Month,date.Day,23,59,59));
                where += $"AND occur_time BETWEEN \'{specifyDateStart}\' AND \'{specifyDateEnd}\' " +
                    $"AND release_time IS NULL OR release_time BETWEEN \'{specifyDateStart}\' AND \'{specifyDateEnd}\' ";
            }
            if (machineID != ConstData.SEARCH_ALL) { where += $"AND mm.machine_id={machineID} "; }
            string order = "ORDER BY occur_time ASC ";
            string limit = $"LIMIT {pager.limit} OFFSET {pager.offset}";
            string sql = this.searchAlertSELECT_SQL + this.searchAlertFROM_SQL+$"{langID} " + where + order;
            string countSQL = this.countAlertSELECT_SQL + this.searchAlertFROM_SQL+$"{langID} " + where;
            int temp_nowPageNo = ConstData.SEARCH_ALL;
            int temp_limit = ConstData.SEARCH_ALL;
            if (pager.limit != ConstData.SEARCH_ALL)
            {
                temp_nowPageNo = pager.nowPageNo;   //現在のページNoを取っておく
                temp_limit = pager.limit;   //同様にリミットも
                sql += limit;
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchMachineAlert : " + sql);

            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Alert alert = new Alert();
                        alert.alertID = GetDataReaderInt(reader["alert_id"]);
                        alert.alertNo = GetDataReaderInt(reader["alert_no"]);
                        alert.partInfo = GetDataReaderString(reader["part_info"]);
                        alert.partsName = GetDataReaderString(reader["part_name"]);
                        alert.alertLevel = GetDataReaderInt(reader["alert_level"]);
                        alert.alertLevelString = GetDataReaderString(reader["alertLevelString"]);
                        alert.machineID = GetDataReaderInt(reader["machine_id"]);
                        alert.alertName = GetDataReaderString(reader["alert_name"]);
                        alert.occurTime = GetDataReaderTime(reader["occur_time"]);
                        //release_timeがNULLの時は、現在時刻をセット
                        alert.releaseTime = reader["release_time"] == null ? DateTime.Now :
                            GetDataReaderTime(reader["release_time"]);
                        alert.companyID = GetDataReaderInt(reader["clientID"]);
                        alert.companyName = GetDataReaderString(reader["clientName"]);
                        alert.endUserID = GetDataReaderInt(reader["endUserID"]);
                        alert.endUserName = GetDataReaderString(reader["endUserName"]);
                        alert.modelID = GetDataReaderInt(reader["model_id"]);
                        alert.modelName = GetDataReaderString(reader["modelName"]);
                        alert.MGOfficeID = GetDataReaderInt(reader["branch_id"]);
                        alert.MGOfficeName = GetDataReaderString(reader["MGOffice"]);
                        alert.typeID = GetDataReaderString(reader["type_id"]);
                        alert.typeName = GetDataReaderString(reader["typeName"]);
                        alert.machineSerialNumber = GetDataReaderString(reader["serial_number"]);
                        alert.settingDate = GetDataReaderTime(reader["delivery_date"]);
                        alert.detailString = GetDataReaderString(reader["detail"]);
                        alert.causeString = GetDataReaderString(reader["cause"]);
                        alert.maintenanceString = GetDataReaderString(reader["treatString"]);
                        machineAlertList.Add(alert);
                    }
                }
            }
            connection.Close();
            int totalRecordCount = this.GetTotalRecordCount(countSQL);
            //::::: 取っておいたリミットが-1でなければ、クリックされた時の情報でページャーを作り直し、ページナンバーとオフセット値を新しい数値に設定する
            if (temp_limit != ConstData.SEARCH_ALL)
            {
                pager = new PagerController(totalRecordCount,temp_limit);
                pager.SetNowPageNo(temp_nowPageNo);
                pager.SetOffset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertList">DBからの値を格納するリスト</param>
        /// <param name="langID">言語ID</param>
        /// <param name="pager">ページ情報を持つインスタンス</param>
        /// <param name="sortBase">並べ替え条件を持つインスタンス</param>
        /// <param name="searchCondition">検索条件を持つインスタンス</param>
        /// <param name="alertKindNo">マシンアラートか部品交換アラートか</param>
        public void SearchAlert_bySearchCondition(out List<Alert> alertList, int langID, ref PagerController pager, ref SortBase sortBase, in MachineBase searchCondition, int alertKindNo, bool isAll = false)
        {
            alertList = new List<Alert>();
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト
            string sql = this.searchAlertSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} WHERE ";
            string countSQL = this.countAlertSELECT_SQL + this.searchAlertFROM_SQL+$"{langID} WHERE ";
            string orderSQL = $"ORDER BY {sortBase.sortColumn} {sortBase.orderDirection} ";
            string sign = alertKindNo == ConstData.KIND_MACHINE_ALERT ? ">=" : "<" ;
            string alertNo_SQL = $"am.alert_no{sign}{ConstData.PARTS_ALERT_BORDER} ";
            string langID_SQL = $"lr1.lang_id={langID} AND lr2.lang_id={langID} AND lr3.lang_id={langID} AND lr4.lang_id={langID} ";
            int temp_nowPageNo = pager != null ? pager.nowPageNo : 1;   //現在のページNoを取っておく
            int temp_limit = pager != null ? pager.limit : 1;   //同様にリミットも
            int temp_offset = pager != null ? pager.offset : 1;   //同様にオフセットも
            string limitSQL = $"LIMIT {temp_limit} OFFSET {temp_offset} ";

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if (searchCondition.machineID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.machineID_SQL}{searchCondition.machineID} "); }
            if (searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if (searchCondition.endUserCode != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.endUserCD_SQL}{searchCondition.endUserCode} "); }
            if (searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            if (searchCondition.modelID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.modelID_SQL}{searchCondition.modelID} "); }
            if (searchCondition.typeID != ConstData.SEARCH_ALL.ToString()) { whereSQLs.Add($"{this.typeID_SQL}{searchCondition.typeID}\' "); }
            if(searchCondition.alertNo != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.alertNo_SQL}{searchCondition.alertNo} "); }
            if(searchCondition.alertLevel != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.alertLv_SQL}{searchCondition.alertLevel} "); }
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
            //シリアルナンバー
            if ((searchCondition.serialNumber != ConstData.SEARCH_ALL.ToString()) && (searchCondition.serialNumber != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.serialNumber.Contains("%")) {
                    whereSQLs.Add($"{this.serialNumber_LIKE_SQL}\'{searchCondition.serialNumber}\' "); 
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.serialNumber_SQL}\'{searchCondition.serialNumber}\' ");
                }
            }
            //設置年月スタートとエンド両方が入っていたとき
            if((searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT)) {
                string startDate = this.GetGmtTimeString(searchCondition.settingDate_s);
                string endDate = this.GetGmtTimeString(searchCondition.settingDate_e);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            //設置期間スタートだけが入っていたとき
            }else if ((searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT)) {
                string startDate = this.GetGmtTimeString(searchCondition.settingDate_s);
                string endDate = this.GetGmtTimeString(DateTime.Now);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            //設置期間エンドだけが入っていたとき
            }else if((searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT)) {
                string startDate = DateTime.MinValue.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string endDate = this.GetGmtTimeString(searchCondition.settingDate_e);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            }
            //アラート期間スタートとエンド両方が入っていたとき
            if((searchCondition.alertStart.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.alertEnd.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT)) {
                string specifyDateStart = this.GetAlertStartDate(searchCondition.alertStart);
                string specifyDateEnd = this.GetAlertEndDate(searchCondition.alertEnd);
                whereSQLs.Add($"occur_time>=(\'{specifyDateStart}\') AND occur_time<=(\'{specifyDateEnd}\')  ");
                //アラート期間スタートだけ入っていたとき
            }
            else if((searchCondition.alertStart.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.alertEnd.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT)) {
                string specifyDateStart = this.GetAlertStartDate(searchCondition.alertStart);
                whereSQLs.Add($"occur_time>=(\'{specifyDateStart}\') ");
            //アラート期間エンドだけが入っていたとき
            }else if ((searchCondition.alertEnd.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.alertStart.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT)) { 
                string specifyDateEnd = this.GetAlertEndDate(searchCondition.alertEnd);
                whereSQLs.Add($"occur_time<=(\'{specifyDateEnd}\') ");
            }
            if (alertKindNo != ConstData.SEARCH_ALL) { whereSQLs.Add(alertNo_SQL); }
            //あいまい検索
            if ((searchCondition.companyName_ambi != ConstData.SEARCH_ALL.ToString()) && (searchCondition.companyName_ambi != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.companyName_ambi.Contains("%")) {
                    whereSQLs.Add($"{this.clientName_LIKE_SQL}\'{searchCondition.companyName_ambi}\' {this.clientKind_SQL}");
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.clientName_SQL}\'{searchCondition.companyName_ambi}\' {this.clientKind_SQL}");
                }
            }
            if ((searchCondition.enduserName_ambi != ConstData.SEARCH_ALL.ToString()) && (searchCondition.enduserName_ambi != ConstData.EMPTY)) {
                //LIKE検索の時
                if (searchCondition.enduserName_ambi.Contains("%")) {
                    whereSQLs.Add($"{this.enduserName_LIKE_SQL}\'{searchCondition.enduserName_ambi}\' {this.enduserKind_SQL}");
                }
                //一致検索のとき
                else
                {
                    whereSQLs.Add($"{this.enduserName_SQL}\'{searchCondition.enduserName_ambi}\' {this.enduserKind_SQL}");
                }
            }
            whereSQLs.Add(langID_SQL);     //alert_noのWHERE句は必ずリストに追加

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
            if (pager != null && !isAll) { sql += limitSQL; }
            Debug.Print("SearchMachineAlert(2) : " + sql);

            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Alert alert = new Alert();
                        alert.alertID = GetDataReaderInt(reader["alert_id"]);
                        alert.alertNo = GetDataReaderInt(reader["alert_no"]);
                        alert.partInfo = GetDataReaderString(reader["part_info"]);
                        alert.partsName = GetDataReaderString(reader["part_name"]);
                        alert.alertLevel = GetDataReaderInt(reader["alert_level"]);
                        alert.alertLevelString = GetDataReaderString(reader["alertLevelString"]);
                        alert.machineID = GetDataReaderInt(reader["machine_id"]);
                        alert.alertName = GetDataReaderString(reader["alert_name"]);
                        alert.occurTime = GetDataReaderTime(reader["occur_time"]);
                        //release_timeがNULLの時は、現在時刻をセット
                        if ((reader["release_time"] == null) || (reader["release_time"].ToString() == ConstData.EMPTY))
                        {
                            alert.releaseTime = DateTime.Now;
                            alert.isNowAlert = true;
                        }
                        else
                        {
                            alert.releaseTime = GetDataReaderTime(reader["release_time"]);
                        }
                        alert.companyID = GetDataReaderInt(reader["clientID"]);
                        alert.companyName = GetDataReaderString(reader["clientName"]);
                        alert.endUserID = GetDataReaderInt(reader["endUserID"]);
                        alert.endUserName = GetDataReaderString(reader["endUserName"]);
                        alert.modelID = GetDataReaderInt(reader["model_id"]);
                        alert.modelName = GetDataReaderString(reader["modelName"]);
                        alert.MGOfficeID = GetDataReaderInt(reader["branch_id"]);
                        alert.MGOfficeName = GetDataReaderString(reader["MGOffice"]);
                        alert.typeID = GetDataReaderString(reader["type_id"]);
                        alert.typeName = GetDataReaderString(reader["typeName"]);
                        alert.machineSerialNumber = GetDataReaderString(reader["serial_number"]);
                        alert.settingDate = GetDataReaderTime(reader["delivery_date"]);
                        alert.detailString = GetDataReaderString(reader["detail"]);
                        alert.causeString = GetDataReaderString(reader["cause"]);
                        alert.maintenanceString = GetDataReaderString(reader["treatString"]);
                        alertList.Add(alert);
                    }
                }
            }
            connection.Close();
            int totalRecordCount = this.GetTotalRecordCount(countSQL);
     //::::: ページャーがクリックされた時に持っていた情報でページャーを作り直し、ページナンバーとオフセット値は新しい数値に設定する
            pager = new PagerController(totalRecordCount,temp_limit);
            pager.nowPageNo = temp_nowPageNo;
            pager.SetOffset();
        }

        /// <summary>
        /// DBに登録されたマシン情報の総数を取得
        /// </summary>
        /// <remarks>ページャーのtotalObjectに使用</remarks>
        /// <returns>登録総数を表す整数値</returns>
        public int GetAllAlertCount(int langID,int machineID)
        {
            int result = 0;
            string sql = this.countAlertSELECT_SQL + this.searchAlertFROM_SQL + $"{langID} ";
            if (machineID != ConstData.SEARCH_ALL)
            {
                sql += $"WHERE ad.machine_id={machineID} AND lr1.lang_id={langID} AND lr2.lang_id={langID} AND lr3.lang_id={langID} AND lr4.lang_id={langID} ";
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("GetAllAlertCount : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                result = int.Parse(command.ExecuteScalar().ToString());
            }
            connection.Close();
            return result;
        }

        /// <summary>
        /// DBから緊急度を表す文言を取得するためのキー
        /// </summary>
        /// <remarks>アラートレベルを参照</remarks>
        /// <param name="alertLevel"></param>
        /// <returns></returns>
        public int GetKey(int alertLevel)
        {
            int result;
            if (alertLevel == ConstData.EMERGENCY) { result = ConstData.EMERGENCY_KEY; }
            else if (alertLevel == ConstData.WORNING) { result = ConstData.WORNING_KEY; }
            else if (alertLevel == ConstData.BEWARE) { result = ConstData.BEWARE_KEY; }
            else if (alertLevel == ConstData.ATTENTION) { result = ConstData.ATTENTION_KEY; }
            else { result = ConstData.ATTENTION_KEY; }
            return result;
        }

        /// <summary>
        /// 条件検索に使用
        /// </summary>
        /// <param name="start_date">期間指定のスタートの日時</param>
        /// <returns>GMTタイムのDateTime文字列</returns>
        public string GetAlertStartDate(DateTime start_date)
        {
            return this.GetGmtTimeString(new DateTime(start_date.Year,start_date.Month,start_date.Day,0,0,0));
        }

        /// <summary>
        /// 条件検索に使用
        /// </summary>
        /// <param name="end_date">期間指定のエンドの日時</param>
        /// <returns>GMTタイムのDateTime文字列</returns>
        public string GetAlertEndDate(DateTime end_date)
        {
            return this.GetGmtTimeString(new DateTime(end_date.Year,end_date.Month,end_date.Day,23,59,59));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertKindList"></param>
        /// <param name="langID"></param>
        /// <param name="machineID"></param>
        public void SearchAlertKindList(out List<AlertKind> alertKindList, int langID)
        {
            alertKindList = new List<AlertKind>();
            string sql = this.searchAlertKind_SQL + $"where lr.lang_id={langID} AND am.alert_no > 4 ";

            Debug.Print("SearchAlertKindList : " + sql);
            //:::: DB
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlertKind kind = new AlertKind();
                        kind.alertNo = GetDataReaderInt(reader["alert_no"]);
                        kind.alertName = GetDataReaderString(reader["string"]);
                        alertKindList.Add(kind);
                    }
                }
            }
            connection.Close();
        }
    }
}
