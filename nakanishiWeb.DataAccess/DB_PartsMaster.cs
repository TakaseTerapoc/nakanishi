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
    public class DB_PartsMaster:DBBase
    {
        public DB_PartsMaster(DataAccessObject dbObj) {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// マシンIDに紐づくパーツを全て取得
        /// </summary>
        /// <param name="partsList">パーツ格納用リスト</param>
        /// <param name="langID">言語ID</param>
        /// <param name="machineID">マシンID</param>
        /// <param name="sortBase">ソート用インスタンス</param>
        public void SearchParts_ofMachine(out List<Parts>partsList ,int langID,int machineID,ref SortBase sortBase)
        {
            partsList = new List<Parts>();
            string where = $"WHERE machine_id={machineID} AND pm.delete_flag=false  ";
            string order = $"ORDER BY {sortBase.sortColumn} {sortBase.orderDirection} NULLS LAST ";
            string sql = this.searchPartsSELECT_SQL + this.searchPartsFROM_SQL + where + order;

            Debug.Print("SearchParts_ofMachine : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection)) 
            {
                connection.Open();
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Parts parts = new Parts();
                        parts.machineID = GetDataReaderInt(reader["machine_id"]);
                        parts.partsID = GetDataReaderInt(reader["parts_id"]);
                        parts.partsName = GetDataReaderString(reader["part_name"]);
                        parts.partsNo = GetDataReaderInt(reader["parts_number"]);
                        parts.exchangeGuideline_time = GetDataReaderLong(reader[$"life_time"]);
                        parts.exchangeGuideline_count = GetDataReaderLong(reader[$"life_cycle"]);
                        if(parts.exchangeGuideline_time != ConstData.SEARCH_ALL)
                        {
                            parts.DBtotalTime = GetDataReaderLong(reader[$"total_time"]);
                            parts.initTime = GetDataReaderLong(reader[$"init_time"]);
                            parts.totalTime = parts.DBtotalTime + parts.initTime;
                            parts.remainingOperate_time = parts.exchangeGuideline_time - parts.totalTime;
                        }
                        else
                        {
                            parts.DBtotalTime = ConstData.SEARCH_ALL;
                            parts.initTime = ConstData.SEARCH_ALL;
                            parts.totalTime = ConstData.SEARCH_ALL;
                            parts.remainingOperate_time = ConstData.SEARCH_ALL;
                        }
                        parts.exchangeGuideTimeString = this.SecondToHour(parts.exchangeGuideline_time);
                        parts.operateHourString = this.SecondToHour(parts.totalTime);
                        parts.remainingOperateHourString = this.SecondToHour(parts.remainingOperate_time);

                        if(parts.exchangeGuideline_count != ConstData.SEARCH_ALL)
                        {
                            parts.DBtotalCount = GetDataReaderLong(reader[$"total_count"]);
                            parts.initCount = GetDataReaderLong(reader["init_count"]);
                            parts.totalCount = parts.DBtotalCount + parts.initCount;
                            parts.remainingOperate_count = parts.exchangeGuideline_count - parts.totalCount;
                            parts.operateCountString = parts.totalCount.ToString();
                            parts.exchangeGuideCountString = parts.exchangeGuideline_count.ToString();
                            parts.remainingOperateCountString = parts.remainingOperate_count.ToString();
                        }
                        else
                        {
                            parts.DBtotalCount = ConstData.SEARCH_ALL;
                            parts.initCount = ConstData.SEARCH_ALL;
                            parts.totalCount = ConstData.SEARCH_ALL;
                            parts.remainingOperate_count = ConstData.SEARCH_ALL;
                            parts.operateCountString = ConstData.EMPTY;
                            parts.exchangeGuideCountString = ConstData.EMPTY;
                            parts.remainingOperateCountString = ConstData.EMPTY;
                        }

                        parts.lastExchangeDate = GetDataReaderTime(reader["attach_time"]);
                        parts.exchangeCount = GetDataReaderInt(reader["exchang_cunt"]);
                        //現在アラート発生中かどうか(交換時間)
                        if((parts.exchangeGuideline_time != ConstData.SEARCH_ALL) && (parts.totalTime > parts.exchangeGuideline_time)) 
                        { 
                            parts.isExchangeAlert = true;
                            parts.remainingOperateHourString += " OVER";
                        }
                        //現在アラート発生中かどうか(交換回数)
                        if((parts.exchangeGuideline_count != ConstData.SEARCH_ALL) && (parts.totalCount > parts.exchangeGuideline_count)) 
                        { 
                            parts.isExchangeAlert = true;
                            //回数に関しては、多言語化のためページ.aspx.cs側で「回OVER」を追加
                        }
                        partsList.Add(parts);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// パーツIDを指定して、パーツ情報を１つだけ取得する
        /// </summary>
        /// <param name="partsID">パーツID</param>
        /// <param name="machineID">マシンID</param>
        /// <param name="parts">パーツ情報を格納するインスタンス</param>
        public void SearchPartsOne(int partsID,int machineID,out Parts parts)
        {
            string where = $"WHERE machine_id={machineID} AND parts_id={partsID} AND pm.delete_flag=false ";
            string sql = this.searchPartsSELECT_SQL + this.searchPartsFROM_SQL + where;
            parts = new Parts();

            Debug.Print("SearchPartsOne : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection)) 
            {
                connection.Open();
                using(NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        parts.machineID = GetDataReaderInt(reader["machine_id"]);
                        parts.partsID = GetDataReaderInt(reader["parts_id"]);
                        parts.partsName = GetDataReaderString(reader["part_name"]);
                        parts.partsNo = GetDataReaderInt(reader["parts_number"]);
                        parts.exchangeGuideline_time = GetDataReaderLong(reader[$"life_time"]);
                        parts.exchangeGuideline_count = GetDataReaderLong(reader[$"life_cycle"]);
                        if(parts.exchangeGuideline_time != ConstData.SEARCH_ALL)
                        {
                            parts.DBtotalTime = GetDataReaderLong(reader[$"total_time"]);
                            parts.initTime = GetDataReaderLong(reader[$"init_time"]);
                            parts.totalTime = parts.DBtotalTime + parts.initTime;
                            parts.remainingOperate_time = GetDataReaderLong(reader["remainingTime"]);
                        }
                        else
                        {
                            parts.DBtotalTime = ConstData.SEARCH_ALL;
                            parts.initTime = ConstData.SEARCH_ALL;
                            parts.totalTime = ConstData.SEARCH_ALL;
                            parts.remainingOperate_time = ConstData.SEARCH_ALL;
                        }
                        if(parts.exchangeGuideline_count != ConstData.SEARCH_ALL)
                        {
                            parts.DBtotalCount = GetDataReaderLong(reader[$"total_count"]);
                            parts.initCount = GetDataReaderLong(reader["init_count"]);
                            parts.totalCount = parts.DBtotalCount + parts.initCount;
                            parts.remainingOperate_count = GetDataReaderLong(reader["remainingTime"]);
                        }
                        else
                        {
                            parts.DBtotalCount = ConstData.SEARCH_ALL;
                            parts.initCount = ConstData.SEARCH_ALL;
                            parts.totalCount = ConstData.SEARCH_ALL;
                            parts.remainingOperate_count = ConstData.SEARCH_ALL;
                        }
                        parts.lastExchangeDate = GetDataReaderTime(reader["attach_time"]);
                        parts.operateHourString = this.SecondToHour(parts.totalTime);
                        parts.remainingOperateHourString = this.SecondToHour(parts.remainingOperate_time);
                        parts.operateCountString = parts.totalCount == ConstData.SEARCH_ALL ? "-" : parts.totalCount.ToString();
                        parts.remainingOperateCountString = parts.remainingOperate_count == ConstData.SEARCH_ALL ? "-" : parts.remainingOperate_count.ToString();
                        parts.exchangeGuideTimeString = this.SecondToHour(parts.exchangeGuideline_time);
                        parts.exchangeCount = GetDataReaderInt(reader["exchang_cunt"]);
                        parts.exchangeGuideCountString = parts.exchangeGuideline_count == ConstData.SEARCH_ALL ? "-" : parts.exchangeGuideline_count.ToString();
                        if((parts.exchangeGuideline_time != ConstData.SEARCH_ALL) && (parts.totalTime > parts.exchangeGuideline_time)) { parts.isExchangeAlert = true; }
                        if((parts.exchangeGuideline_count != ConstData.SEARCH_ALL) && (parts.totalCount > parts.exchangeGuideline_count)) { parts.isExchangeAlert = true; }
                        break;
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// パーツIDを指定して、パーツ情報を１つだけ取得する
        /// </summary>
        /// <param name="partsID">パーツID</param>
        /// <param name="machineID">マシンID</param>
        /// <param name="parts">パーツ情報を格納するインスタンス</param>
        public void SearchPartsKindList(out List<PartsKind> partsKindList, int machineID, int langID)
        {
            partsKindList = new List<PartsKind>();
            string sql = this.searchPartsKind_SQL + $"where pm.machine_id={machineID} AND lr.lang_id={langID}";

            Debug.Print("SearchPartsKindList : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PartsKind kind = new PartsKind();
                        kind.partsKindId = GetDataReaderInt(reader["parts_kind_id"]);
                        kind.partsName = GetDataReaderString(reader["string"]);
                        kind.lifeTime = GetDataReaderLong(reader["life_time"]);
                        kind.lifeCycle = GetDataReaderLong(reader["life_cycle"]);
                        partsKindList.Add(kind);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// SearchOneで取得したパーツ情報を使用して、そのパーツの交換回数とinitTime・initCountを更新する
        /// </summary>
        /// <remarks>アラートメンテナンス時期情報の「交換完了」ボタンが押されたときに使用</remarks>
        /// <param name="parts">交換回数を変更するパーツインスタンス</param>
        public void UpdateExchangeCount(in Parts parts)
        {
            //Debug.Print($"partsID:{parts.partsID} , initTime:{parts.initTime} , totalTime:{parts.totalTime}");
            //init_time・init_countは現在のinit_からtotalを引いた数値を、attach_timeは現在時刻を、交換回数は現在の交換回数に+1した数値をセット
            string sql = $"UPDATE parts_master SET init_time={(parts.DBtotalTime*(-1))},init_count={(parts.DBtotalCount*(-1))}," +
                $"attach_time=\'{this.GetGmtTimeStringNow()}\',exchang_cunt={parts.exchangeCount+1}," +
                $"update_time=\'{this.GetGmtTimeStringNow()}\' " +
                $"WHERE parts_id={parts.partsID}";

            Debug.Print("UpdateExchangeCount : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            connection.Open();
            using (NpgsqlTransaction tran = connection.BeginTransaction())
            {
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    tran.Commit();
                }
            }
            connection.Close();
        }

        /// <summary>
        /// UpdateExchangeCountで変更された内容をDBにインサートする
        /// </summary>
        /// <param name="parts">変更があったパーツ情報を持つインスタンス</param>
        public void InsertExchangeInformation(in Parts parts)
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            int userID = int.Parse(hc.Session[SearchLabel.USER_ID].ToString());
            string sql = $"INSERT INTO parts_exchang_history VALUES(" +
                $"{parts.machineID},{parts.partsID},{parts.partsNo},{parts.totalTime},{parts.totalCount}," +
                $"\'{this.GetGmtTimeStringNow()}\',{userID})";

            Debug.Print("InsertExchangeInformation : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
