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
    public class DB_MachineMaster:DBBase
    {
        public DB_MachineMaster(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// マシンIDから1つの機種に絞って取得
        /// </summary>
        /// <remarks>主に詳細ページで使用</remarks>
        /// <param name="machineID">マシンID DB:machine_master[machine_id](serial)</param>
        /// <returns>マシンクラスのインスタンス</returns>
        public Machine SearchMachine_one(int machineID,int langID)
        {
            Machine machine = new Machine();
            string sql = this.searchMachineSELECT_SQL + this.searchMachineFROM_SQL +
                $"WHERE {this.machineID_SQL}{machineID} AND {this.langSQL}{langID}";

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchMachine_one : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        machine.machineID = GetDataReaderInt(reader["machine_id"]);
                        machine.companyID = GetDataReaderInt(reader["ClientCD"]);
                        machine.companyName = GetDataReaderString(reader["ClientName"]);
                        machine.endUserCode = GetDataReaderInt(reader["endUserCD"]);
                        machine.endUserName = GetDataReaderString(reader["endUserName"]);
                        machine.managementOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        machine.managementOffice = GetDataReaderString(reader["MGOfficeName"]);
                        machine.settingDate = GetDataReaderTime(reader["delivery_date"]);
                        machine.modelID = GetDataReaderInt(reader["model_id"]);
                        machine.modelName = GetDataReaderString(reader["model_name"]);
                        machine.typeID = GetDataReaderString(reader["type_id"]);
                        machine.typeName = GetDataReaderString(reader["type_name"]);
                        machine.typeIOK = GetDataReaderString(reader["type_iok"]);
                        machine.serialNumber = GetDataReaderString(reader["serial_number"]);
                        machine.postalCode = GetDataReaderString(reader["post_code"]);
                        machine.settingPlace = GetDataReaderString(reader["place"]);
                        machine.isPartsAlert = GetDataReaderString(reader["alert_parts"]) == "true" ? true : false;
                        machine.isMachineAlert = GetDataReaderString(reader["alert_machine"]) == "ture" ? true : false;
                       /* machine.isPartsAlert = GetDataReaderInt(reader["partsAlert"]) == ConstData.SEARCH_ALL ? true : false;
                        machine.isMachineAlert = GetDataReaderInt(reader["machineAlert"]) == ConstData.SEARCH_ALL ? true : false;*/
                        machine.actualTime = GetDataReaderLong(reader["actual_time"]);
                        machine.operateHour = this.SecondToHour(machine.actualTime);
                        machine.lastTime = GetDataReaderTime(reader["last_time"]);
                        break;
                    }
                }
            }
            connection.Close();
            return machine;
        }

        /// <summary>
        /// 検索情報をもとにDBに接続してマシン情報を取得
        /// </summary>
        /// <param name="pager">ページャー作成用インスタンス</param>
        /// <param name="searchCondition">検索条件を持ったインスタンス</param>
        /// <param name="machineList">マシン情報のリスト</param>
        /// <param name="sortColumn">並べ替えるカラム名。デフォルト⇒machine_id</param>
        /// <param name="order">ASC(昇順)かDESC(降順)か</param>
        /// <param name="langID">言語ID</param>
        public void SearchMachines_bySearchCondition(ref MachineBase searchCondition, ref PagerController pager, out List<Machine> machineList, ref SortBase sortBase, int langID, bool isAll)
        {
            machineList = new List<Machine>();   //DBから取得したマシン情報を格納するリスト
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト
            string sql = this.searchMachineSELECT_SQL + this.searchMachineFROM_SQL + "WHERE ";
            string countSQL = this.countMachineSELECT_SQL + this.searchMachineFROM_SQL + $"WHERE ";
            string orderSQL = $"ORDER BY {sortBase.sortColumn} {sortBase.orderDirection} ";
            if (!isAll)
            {
                orderSQL += $"LIMIT {pager.limit} OFFSET {pager.offset}";
            }

            int temp_nowPageNo = pager.nowPageNo;   //現在のページNoを取っておく
            int temp_limit = pager.limit;   //同様にリミットも

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if (searchCondition.machineID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.machineID_SQL}{searchCondition.machineID} "); }
            if (searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if (searchCondition.endUserCode != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.endUserCD_SQL}{searchCondition.endUserCode} "); }
            if (searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            if (searchCondition.modelID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.modelID_SQL}{searchCondition.modelID} "); }
            if (searchCondition.typeID != ConstData.SEARCH_ALL.ToString()) { whereSQLs.Add($"{this.typeID_SQL}{searchCondition.typeID}\' "); }
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
                string startDate = searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string endDate = searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            //スタートだけが入っていたとき
            }else if ((searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT)) {
                string startDate = searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string endDate = DateTime.Now.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            //エンドだけが入っていたとき
            }else if((searchCondition.settingDate_s.ToString(ConstData.TIME_STAMP_DATE_FORMAT) == ConstData.DEFAULT_DATE_FORMAT) && (searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT) != ConstData.DEFAULT_DATE_FORMAT)) {
                string startDate = new DateTime(2000,1,1,0,0,0).ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string endDate = searchCondition.settingDate_e.ToString(ConstData.TIME_STAMP_DATE_FORMAT);
                string settingDateSQL = $"delivery_date BETWEEN \'{startDate}\' AND \'{endDate}\' ";
                whereSQLs.Add(settingDateSQL); 
            }
            //パーツアラート
            if (searchCondition.partsAlert != ConstData.SEARCH_ALL) {
                string partsAlert;
                if (searchCondition.partsAlert == 1) {//パーツアラートあり
                    partsAlert = $"{this.partsAlert_SQL}true ";
                }else { //パーツアラートなし
                    partsAlert = $"{this.partsAlert_SQL}false ";
                }
                whereSQLs.Add(partsAlert);
            }
            //機械アラート
            if (searchCondition.machineAlert != ConstData.SEARCH_ALL) {
                string machineAlert;
                if (searchCondition.machineAlert == 1) {//パーツアラートあり
                    machineAlert = $"{this.machineAlert_SQL}true ";
                }else { //パーツアラートなし
                    machineAlert = $"{this.machineAlert_SQL}false ";
                }
                whereSQLs.Add(machineAlert);
            }
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
            whereSQLs.Add($"{this.langSQL}{langID} ");     //lang_idのWHERE句は必ずリストに追加

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
            countSQL += $")total";

            Debug.Print("SearchMachines_bySearchCondition : " + sql);

            //::::: DB接続
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Machine machine = new Machine();
                        machine.machineID = GetDataReaderInt(reader["machine_id"]);
                        machine.companyID = GetDataReaderInt(reader["ClientCD"]);
                        machine.companyName = GetDataReaderString(reader["ClientName"]);
                        machine.endUserCode = GetDataReaderInt(reader["endUserCD"]);
                        machine.endUserName = GetDataReaderString(reader["endUserName"]);
                        machine.managementOfficeID = GetDataReaderInt(reader["MGOfficeID"]);
                        machine.managementOffice = GetDataReaderString(reader["MGOfficeName"]);
                        machine.settingDate = GetDataReaderTime(reader["delivery_date"]);
                        machine.modelID = GetDataReaderInt(reader["model_id"]);
                        machine.modelName = GetDataReaderString(reader["model_name"]);
                        machine.typeID = GetDataReaderString(reader["type_id"]);
                        machine.typeName = GetDataReaderString(reader["type_name"]);
                        machine.typeIOK = GetDataReaderString(reader["type_iok"]);
                        machine.serialNumber = GetDataReaderString(reader["serial_number"]);
                        machine.postalCode = GetDataReaderString(reader["post_code"]);
                        machine.settingPlace = GetDataReaderString(reader["place"]);
                        machine.isPartsAlert = GetDataReaderBoolean(reader["alert_parts"]);
                        machine.isMachineAlert = GetDataReaderBoolean(reader["alert_machine"]);
                        /*machine.isPartsAlert = GetDataReaderInt(reader["partsAlert"]) == 0 ? false : true;
                        machine.isMachineAlert = GetDataReaderInt(reader["machineAlert"]) == 0 ? false : true;*/
                        machine.actualTime = GetDataReaderLong(reader["actual_time"]);
                        machine.operateHour = this.SecondToHour(machine.actualTime);
                        machine.lastTime = GetDataReaderTime(reader["last_time"]);
                        machineList.Add(machine);
                    }
                }
            }
            connection.Close();
            int totalMachineCount = this.GetTotalRecordCount(countSQL);
     //::::: ページャーがクリックされた時に持っていた情報でページャーを作り直し、ページナンバーとオフセット値は新しい数値に設定する
            pager = new PagerController(totalMachineCount,temp_limit);
            pager.nowPageNo = temp_nowPageNo;
            pager.SetOffset();
        }

        /// <summary>
        /// DBに登録されたマシン情報の総数を取得
        /// </summary>
        /// <remarks>ページャーのtotalObjectに使用</remarks>
        /// <returns>登録総数を表す整数値</returns>
        public int GetAllMachineCount(int langID)
        {
            int result = 0;
            string sql = this.countMachineSELECT_SQL + this.searchMachineFROM_SQL +
                $"WHERE {this.langSQL}{langID} ) AS total";

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("GetAllMachineCount : " + sql);

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
        /// DBから抽出したマシン情報の総数を取得
        /// </summary>
        /// <remarks>ページャーのtotalObjectに使用</remarks>
        /// <returns>登録総数を表す整数値</returns>
        public int GetSelectMachineCount_ofEndUser(int companyID,int langID)
        {
            int result = 0;
            string sql = this.countMachineSELECT_SQL + this.searchMachineFROM_SQL +
                $"WHERE {this.endUserCD_SQL}{companyID} AND {this.langSQL}{langID} ";

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);
            sql += ") AS total";

            Debug.Print("GetSelectMachineCount_ofEndUser : " + sql);

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
