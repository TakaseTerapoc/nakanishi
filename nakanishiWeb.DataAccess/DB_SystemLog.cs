using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.DataAccess
{
    public class DB_SystemLog : DBBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbObj">データアクセス用インスタンス</param>
        public DB_SystemLog(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// システムログを追加する
        /// </summary>
        /// <param name="msg">追加するログ</param>
        /// <returns>成功⇒true</returns>
        public bool InsertSystemLog(string msg)
        {
            bool result = false;
            string SQL = $"INSERT INTO system_log (entry_time,kind,string) VALUES (\'{GetGmtTimeStringNow()}\',\'2\',\'[Web]{this._dbObj.errorMsg}\')";
            if(ExecuteSQL(SQL) != 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// システムログの追加
        /// </summary>
        /// <remarks>引数なし</remarks>
        /// <returns>成功⇒true</returns>
        public bool InsertSystemLog()
        {
            bool result = false;
            string SQL = $"INSERT INTO system_log (entry_time,kind,string) VALUES (\'{GetGmtTimeStringNow()}\',\'2\',\'[Web]{this._dbObj.errorMsg}\')";
            if (this.ExecuteSQL(SQL) != 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// システムログの一覧を取得
        /// </summary>
        /// <param name="sortBase">ソートベースインスタンス</param>
        /// <param name="logList">システムログ用のリスト</param>
        /// <returns>成功⇒true</returns>
       /* public bool GetSystemLogList(ref SortBase sortBase,out List<SystemLogData> logList)
        {
            bool result = false;
            logList = new List<SystemLogData>();
            try
            {
                string SQL = "SELECT COUNT(*) FROM system_log";
                sortBase.totalCount = GetDataCount(SQL);
                SQL = $"SELECT entry_time,kind,string FROM system_log ORDER BY {sortBase.sortColumn} {sortBase.orderDirection} OFFSET {sortBase.offset} LIMIT {sortBase.limit}";
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(SQL,connection))
            {
                connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemLogData syslog = new SystemLogData();
                            syslog.entryTime = (DateTime)reader["entry_time"];
                            syslog.kind = (int)reader["kind"];
                            syslog.contents = reader["string"].ToString();
                            logList.Add(syslog);
                        }
                        result = true;
                    }
                }
                connection.Close();
            }
            catch(Exception e)
            {
                this._dbObj.errorMsg = $"GetSystemLogList : [{e.Message}]";
                InsertSystemLog();
            }
            finally
            {
                this._dbObj.DBClose();
            }
            return result;
        }*/
    }
}
