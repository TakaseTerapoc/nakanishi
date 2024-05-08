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
    public class DB_SendMailUser : DBBase
    {
		public DB_SendMailUser(DataAccessObject dbObj)
        {
			this._dbObj = dbObj;
		}

        /// <summary>
        /// メール送信設定取得
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="machineID"></param>
        /// <param name="mailKind"></param>
        /// <param name="alertIDList"></param>
        /// <param name="partsIDList"></param>
        /// <returns></returns>
		public bool GetUserMailRegisterdList(int userID, int machineID, int mailKind, out List<int> alertIDList, out List<int> partsIDList)
        {
            bool bret = false;
            alertIDList = new List<int>();
            partsIDList = new List<int>();

            string SQL = this.searchMailSettingSELECT_SQL + $" WHERE user_id={userID} AND machine_id={machineID} AND mail_kind={mailKind}";
            Debug.Print("GetUserMailRegisterdList : " + SQL);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            {
                using (NpgsqlCommand command = new NpgsqlCommand(SQL, connection))
                {
                    connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var partsId = GetDataReaderInt(reader["parts_id"]);
                            var alertNo = GetDataReaderInt(reader["alert_no"]);
                            if (partsId == -1)
                            {
                                // 登録済みアラート
                                alertIDList.Add(alertNo);
                            }
                            else
                            {
                                // 登録済みパーツ
                                partsIDList.Add(partsId);
                            }
                        }
                    }
                }
            }
            connection.Close();
            return bret;
        }

        /// <summary>
        /// ユーザーと機器に紐づく設定の削除
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="machineID"></param>
        /// <param name="mailKind"></param>
        /// <returns></returns>
        public bool DeleteUserMailSetting(int userID, int machineID, int mailKind)
        {
            bool bret = false;
            string sql = this.deleteMailSetting_SQL +
                $"WHERE user_id = {userID} AND machine_id = {machineID} AND mail_kind = {mailKind}";

            Debug.Print("DeleteUserMailSetting : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                var result = command.ExecuteNonQuery();
                if (result >= 0)
                {
                    bret = true;
                }
            }
            connection.Close();
            return bret;

        }

        /// <summary>
        /// ユーザーと機器に紐づく設定の削除
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="machineID"></param>
        /// <param name="mailKind"></param>
        /// <returns></returns>
        public bool InsertUserMailSetting(int userID, int machineID, int mailKind, List<AlertKind> alertKinds, List<PartsKind> partsKinds)
        {
            bool bret = false;

            // (user_id, machine_id, mail_kind, alert_no, parts_id, entry_time)

            string sql = "";
            foreach (var alert in alertKinds)
            {
                var temp = this.insertMailSetting_SQL +
                    $"{userID}, {machineID}, {mailKind}, {alert.alertNo}, -1, \'{this.GetGmtTimeStringNow()}\');";
                Debug.Print(temp);

                sql += temp;
            }

            foreach (var parts in partsKinds)
            {
                int alertId = 3;
                if(parts.lifeCycle > 0)
                {
                    alertId = 4;
                }
                var temp = this.insertMailSetting_SQL +
                    $"{userID}, {machineID}, {mailKind}, {alertId}, {parts.partsKindId}, \'{this.GetGmtTimeStringNow()}\');";
                Debug.Print(temp);

                sql += temp;
            }

            if (string.IsNullOrEmpty(sql) == false)
            {
                Debug.Print("InsertUserMailSetting : " + sql);

                NpgsqlConnection connection = this._dbObj.GetConnection();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    if (result >= 0)
                    {
                        bret = true;
                    }
                }
                connection.Close();
            }
            else
            {
                bret = true;
            }
            return bret;

        }
    }
}
