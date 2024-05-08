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
    public class DB_UserMaster:DBBase
    {
		public DB_UserMaster(DataAccessObject dbObj)
        {
			this._dbObj = dbObj;
		}

        /// <summary>
        /// ログインユーザーの権限管理。各ページから参照。
        /// </summary>
        public static bool isMainOffice; //全ての情報を閲覧可能
        public static bool isBranchManager; //統括する支店に紐づく情報を閲覧可能
        public static bool isBranch; //自身の支店に紐づく情報を閲覧可能
        public static bool isClient; //自身の企業に紐づく情報のみ閲覧可能
        public static bool isEndUser; //自身の所有するマシン情報のみ閲覧可能
        public static bool isAdminUser;
        public static bool isOrdinnaryUser;

		/// <summary>
		/// ログインユーザ登録済みかどうかの確認
		/// </summary>
		/// <param name="userID">ユーザID</param>
		/// <param name="password">パスワード</param>
		/// <param name="adminFg">管理者フラグ</param>
		/// <param name="mainteFg">メンテナンスフラグ</param>
		/// <return>true=成功</return>
		public  bool IsLoginUser(string userID,string hash_password, out LoginData loginData,bool adminFlag = false,bool maintenanceFlag = false)
        {
			bool bret = false;
            loginData = new LoginData();
//            string SQL = this.loginSQL + $"WHERE um.login_id=\'{userID}\' AND um.password=\'{password}\'";
            string SQL = this.loginSQL + $"WHERE um.login_id=\'{userID}\' AND um.passwordhash=\'{hash_password}\'";
            if (adminFlag)
            {
                SQL += " AND um.kind=0";
            }
            if (maintenanceFlag)
            {
                SQL += " AND um.company_id=1";
            }

            Debug.Print("IsLoginUser : " + SQL);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(SQL, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loginData.userID = GetDataReaderInt(reader["user_id"]);
                        loginData.companyID = GetDataReaderInt(reader["company_id"]);
                        loginData.companyName = GetDataReaderString(reader["company_name"]);
                        loginData.companyKind = GetDataReaderInt(reader["company_kind"]);
                        loginData.branchID = GetDataReaderInt(reader["branch_id"]);
                        loginData.branchName = GetDataReaderString(reader["branch_name"]);
                        loginData.affiliation = GetDataReaderString(reader["affiliation"]);
                        loginData.generalBranchID = GetDataReaderInt(reader["general_branch_id"]);
                        loginData.userName = GetDataReaderString(reader["user_name"]);
                        loginData.kind = GetDataReaderInt(reader["kind"]);
                        loginData.timeDiff = GetDataReaderInt(reader["time_diff"]);
                        loginData.langID = GetDataReaderInt(reader["lang_id"]);
                        loginData.langCode = GetDataReaderString(reader["lang_code"]);
                        loginData.loginID = GetDataReaderString(reader["login_id"]);
                        loginData.mailStartTime = GetDataReaderTimeOnly(reader["mail_s_time"]);
                        loginData.mailEndTime = GetDataReaderTimeOnly(reader["mail_e_time"]);
                        bret = true;
                        break;
                    }
                }
            }
            connection.Close();

         //::::: ユーザーの権限情報設定
            if(loginData.kind == ConstData.USER_KIND_ADMIN)
            {
                isAdminUser = true;
                isOrdinnaryUser = false;
            }
            else
            {
                isOrdinnaryUser = true;
                isAdminUser = false;
            }
         //::::: ユーザーが所属する会社の権限チェック
            if (loginData.companyKind == ConstData.COMPANY_KIND_ADMIN)//まず中西製作所かどうか
            {
                if (loginData.branchID == ConstData.ADMIN_BRANCH)//本社かどうか
                {
                    isMainOffice = true; //本社のフラグ
                    isBranchManager = false;
                    isBranch = false;
                    isClient = false;
                    isEndUser = false;
                }
                else//本社以外
                {
                    if(loginData.branchID == loginData.generalBranchID)//支店統括かどうか
                    {
                        isMainOffice = false;
                        isBranchManager = true; //支店統括のフラグ
                        isBranch = false;
                        isClient = false;
                        isEndUser = false;
                    }
                    else//一般支店
                    {
                        isMainOffice = false;
                        isBranchManager = false;
                        isBranch = true; //一般支店のフラグ
                        isClient = false;
                        isEndUser = false;
                    }
                }
            }
            else if(loginData.companyKind == ConstData.COMPANY_KIND_CLIENT)//顧客
            {
                    isMainOffice = false;
                    isBranchManager = false;
                    isBranch = false;
                    isClient = true; //顧客のフラグ
                    isEndUser = false;
            }
            else if (loginData.companyKind == ConstData.COMPANY_KIND_ENDUSER)//エンドユーザー
            {
                    isMainOffice = false;
                    isBranchManager = false;
                    isBranch = false;
                    isClient = false;
                    isEndUser = true; //エンドユーザーのフラグ
            }
			return bret;
        }

        /// <summary>
        /// パスワードチェック
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="hash_password"></param>
        /// <returns></returns>
        public bool CheckPassword(string loginId, string hash_password)
        {
            bool bret = false;
            string sql = this.passwordCheckSQL + $"WHERE um.login_id=\'{loginId}\' AND um.passwordhash=\'{hash_password}\'";

            Debug.Print("CheckPassword : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bret = true;
                        break;
                    }
                }
            }
            connection.Close();
            return bret;
        }

        /// <summary>
        /// ユーザー情報更新
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newHashPassword"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        public bool UpdateUserSetting(string userId, string newHashPassword, TimeSpan sTime, TimeSpan eTime)
        {
            bool bret = false;
            string sql = this.updateUserSQL;
            sql += $"mail_s_time=\'{sTime.ToString(@"hh\:mm\:ss")}\', ";
            sql += $"mail_e_time=\'{eTime.ToString(@"hh\:mm\:ss")}\', ";
            sql += $"passwordhash=\'{newHashPassword}\', ";
            sql += $"update_time=\'{this.GetGmtTimeStringNow()}\' ";
            sql += $"WHERE user_id=\'{userId}\'";

            Debug.Print("UpdateUserSetting : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                connection.Open();
                var result = command.ExecuteNonQuery();
                if(result >= 0)
                {
                    bret = true;
                }
            }
            connection.Close();
            return bret;
        }
    }
}
