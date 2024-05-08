using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.DataAccess
{
    public class DataAccessObject
    {
        private NpgsqlConnection connection;
        private NpgsqlCommand command;
        private NpgsqlDataReader reader;
        private NpgsqlTransaction m_Tran = null;
        public readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public string errorMsg;
        public int serverTimeDiff;//サーバー時差[min]


        /// <summary>
        /// connectionオブジェクトを返す
        /// </summary>
        /// <returns>NpgsqlConnection connection</returns>
        public NpgsqlConnection GetConnection() {
            if(this.connection == null)
            {
                this.connection = new NpgsqlConnection(this.ConnectionString);
            }
            return this.connection;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///<remarks>引数なし</remarks>
        public DataAccessObject() {}

        /// <summary>
        /// DBトランザクション処理開始
        /// </summary>
        public void BeginTransaction()
        {
            if(this.connection != null)
            {
                this.m_Tran = this.connection.BeginTransaction();
            }
        }

        /// <summary>
        /// DBトランザクションCommit
        /// </summary>
        public void Commit()
        {
            if((this.connection != null) && (this.m_Tran != null))
            {
                this.m_Tran.Commit();
                this.m_Tran.Dispose();
                this.m_Tran = null;
            }
        }

        /// <summary>
        /// DBトランザクション処理⇒巻き戻し
        /// </summary>
        public void Rollback()
        {
            if(this.connection != null && this.m_Tran != null)
            {
                this.m_Tran.Rollback();
                this.m_Tran.Dispose();
                this.m_Tran = null;
            }
        }

        /// <summary>
        /// DB接続終了
        /// </summary>
        public void DBClose()
        {
            if(this.connection != null)
            {
                this.connection.Close();
            }
        }

        /// <summary>
        /// connectionオブジェクトのメモリ開放
        /// </summary>
        public void DatabaseDisconnect()
        {
            try
            {
                if(this.connection != null)
                {
                    this.connection.Dispose();
                }
            }catch(NpgsqlException error) {
                //失敗
                this.errorMsg = $"(DB)Disconnection[{error.Message}]";
            }
            finally
            {
                this.connection = null;
            }
        }

        /// <summary>
        /// サーバー時差の取得
        /// </summary>
        /// <returns>成功:true・失敗:false</returns>
        public bool GetServerTimeDiff()
        {
            bool isSuccess = false;
            try
            {
                string sql = "SELECT time_diff FROM server_info";
                this.command = new NpgsqlCommand(sql);
                this.reader = this.command.ExecuteReader();
                while (this.reader.Read())
                {
                    serverTimeDiff = int.Parse(reader["time_diff"].ToString());
                    break;
                }
                isSuccess = true;
            }
            catch(NpgsqlException e)
            {//接続失敗
                this.errorMsg = $"[DB]GetServerTimeDiff : {e.Message}";
                this.serverTimeDiff = 540;
            }
            this.connection.Close();
            return isSuccess;
        }
    }
}
