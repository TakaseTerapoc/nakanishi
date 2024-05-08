using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nakanishiWeb.Const;
using System.Diagnostics;

namespace nakanishiWeb.DataAccess
{
    public class DBBase
    {
        private const string DB_FORMAT_TIME = "yyyy/MM/dd HH:mm:ss"; //DB登録フォーマット
        private const string DB_FORMAT_DATE = "yyyy/MM/dd";//DB登録フォーマット
        protected DataAccessObject _dbObj = null;

        // マシン情報取得時の権限による絞り込みSQL WHERE句(追記用)
        public string branchMG = "AND bm2.general_branch_id=";//支店統括
        public string branch = "AND bm2.branch_id=";//一般支店
        public string client = "AND mm.customer_id="; //顧客
        public string endUser = "AND mm.company_id="; //エンドユーザー

        //検索条件をもとに検索する際のWHERE句に使用(追記用)
        public string machineID_SQL = $"mm.machine_id=";
        public string companyID_SQL = $"mm.customer_id=";
        public string endUserCD_SQL = $"mm.company_id=";
        public string MGOfficeID_SQL = $"bm2.branch_id=";
        public string MGOffice_SQL = $"bm2.branch_name=";
        public string MGOffice_LIKE_SQL = $"bm2.branch_name LIKE";
        public string modelID_SQL = $"mm.model_id=";
        public string typeID_SQL = $"mm.type_id='";
        public string serialNumber_SQL = $"serial_number=";
        public string serialNumber_LIKE_SQL = $"serial_number LIKE";
        public string postalCode_SQL = $"bm1.post_code LIKE";
        public string address_SQL = $"bm1.place LIKE";
        public string alertNo_SQL = $"am.alert_no=";
        public string alertLv_SQL = $"am.alert_level=";
        public string langSQL = $"lr.lang_id=";
        public string partsAlert_SQL = "alert_parts=";
        public string machineAlert_SQL = "alert_machine=";
        public string clientName_LIKE_SQL = "cm.company_name LIKE";
        public string clientName_SQL = "cm.company_name=";
        public string clientKind_SQL = $"AND cm.kind={ConstData.COMPANY_KIND_CLIENT} ";
        public string enduserName_LIKE_SQL = "cm2.company_name LIKE";
        public string enduserName_SQL = "cm2.company_name=";
        public string enduserKind_SQL = $"AND cm2.kind={ConstData.COMPANY_KIND_ENDUSER} ";
        /*public string partsAlert_SQL = "ad1.alert_no IS ";
        public string machineAlert_SQL = "ad2.alert_no IS ";*/

        // マシン情報取得用のSQL文 SELECT句
        public string searchMachineSELECT_SQL = "SELECT DISTINCT mm.machine_id,mm.customer_id AS clientCD,cm.company_name AS clientName,mm.company_id AS endUserCD,cm2.company_name AS endUserName," +
            "bm1.general_branch_id AS MGOfficeID,bm2.branch_name AS MGOfficeName," +
            "delivery_date,mm.model_id,mmm.description AS model_name,mtm.type_id,lr.string AS type_name,type_iok,serial_number,bm1.post_code,bm1.place,md.actual_time," +
            "alert_machine,alert_parts," +
            "CASE WHEN((gug_data_time IS NULL) OR (iok_data_time > gug_data_time)) THEN iok_data_time " +
            "ELSE gug_data_time END AS last_time ";
            /*"(SELECT COUNT(*) FROM alert_data AS ad1 WHERE mm.machine_id=ad1.machine_id AND ad1.alert_no<10)partsAlert," +
            "(SELECT COUNT(*) FROM alert_data AS ad2 WHERE mm.machine_id = ad2.machine_id AND ad2.alert_no >= 10)machineAlert ";*/
        // マシン数取得用のSQL文 SELECT句
        public string countMachineSELECT_SQL = "SELECT COUNT(*) FROM( SELECT DISTINCT mm.machine_id ";

        // machine_masterを介した情報取得用のSQL文 FROM句（製品群・品名・製品情報・エンドユーザー・得意先 共通）
        public string searchMachineFROM_SQL = "FROM machine_master AS mm " +
            "INNER JOIN company_master AS cm ON mm.customer_id=cm.company_id AND mm.delete_flag=false " +
            "LEFT JOIN company_master AS cm2 ON mm.company_id=cm2.company_id " +
            "LEFT JOIN branch_master AS bm1 ON mm.company_id=bm1.company_id AND mm.branch_id=bm1.branch_id " +
            $"LEFT JOIN branch_master AS bm2 ON bm2.company_id={ConstData.NAKANISHI_ID} AND bm1.general_branch_id=bm2.branch_id " +
            "LEFT JOIN m_model_master AS mmm USING(model_id) " +
            "LEFT JOIN m_type_master AS mtm USING(type_id) " +
            "LEFT JOIN lang_resource AS lr ON mtm.str_id=lr.str_id " +
            "LEFT JOIN machine_data AS md ON mm.machine_id=md.machine_id " +
            $"LEFT JOIN alert_data AS ad1 ON ad1.machine_id=mm.machine_id AND ad1.alert_no<{ConstData.PARTS_ALERT_BORDER} " +
            $"LEFT JOIN alert_data AS ad2 ON ad2.machine_id=mm.machine_id AND ad2.alert_no>={ConstData.PARTS_ALERT_BORDER} ";

        //得意先検索用
        //public string searchClientSELECT_SQL = "SELECT DISTINCT cm.company_id,cm.company_name,bm1.post_code,bm1.place,bm2.branch_id AS MGOfficeID,bm2.branch_name AS MGOfficeName ";
        //public string searchClientFROM_SQL = "FROM machine_master AS mm " +
        //    "INNER JOIN company_master AS cm ON mm.customer_id=cm.company_id AND mm.delete_flag=false " +   //AND (cm.kind=1 OR cm.kind=2) 削除5/19 
        //    "LEFT JOIN branch_master AS bm1 ON bm1.branch_id=mm.branch_id AND mm.customer_id=bm1.company_id " +
        //    $"LEFT JOIN branch_master AS bm2 ON bm2.company_id={ConstData.NAKANISHI_ID} AND bm2.branch_id=bm1.general_branch_id " +
        //    $"LEFT JOIN lang_resource AS lr ON bm1.lang_id=lr.lang_id ";
        public string searchClientSELECT_SQL = "SELECT DISTINCT cm.company_id,cm.company_name,bm1.post_code,bm1.place,bm2.branch_id AS MGOfficeID,bm2.branch_name AS MGOfficeName ";
        public string searchClientFROM_SQL = "FROM machine_master AS mm, company_master AS cm, " +
                                            "branch_master AS bm1, branch_master AS bm2 " +
                                            "where mm.delete_flag=false and mm.customer_id=cm.company_id " +
                                            "and (bm1.branch_id= mm.branch_id AND mm.customer_id= bm1.company_id) " +
                                            " and(bm2.company_id= 1 AND bm2.branch_id= bm1.general_branch_id) ";

        //エンドユーザー検索用
        //public string searchEndUserSELECT_SQL = "SELECT DISTINCT cm2.company_id AS endUserID,mm.customer_id AS clientID,cm.company_name AS clientName,cm2.company_name AS endUserName,bm1.post_code,bm1.place,bm2.branch_id AS MGOfficeID,bm2.branch_name AS MGOfficeName ";
        //public string searchEndUserFROM_SQL = "FROM machine_master AS mm " +
        //    "INNER JOIN company_master AS cm ON mm.customer_id=cm.company_id AND mm.delete_flag=false " +   //AND (cm.kind=1 OR cm.kind=2)削除5/19 
        //    $"LEFT JOIN company_master AS cm2 ON mm.company_id=cm2.company_id AND cm2.kind={ConstData.COMPANY_KIND_ENDUSER} " +
        //    "LEFT JOIN branch_master AS bm1 ON mm.company_id=bm1.company_id AND bm1.branch_id=mm.branch_id " +
        //    $"LEFT JOIN branch_master AS bm2 ON bm2.company_id={ConstData.NAKANISHI_ID} AND bm2.branch_id=bm1.general_branch_id " +
        //    $"LEFT JOIN lang_resource AS lr ON bm1.lang_id=lr.lang_id ";

        public string searchEndUserSELECT_SQL = "SELECT DISTINCT cm2.company_id AS endUserID,mm.customer_id AS clientID,cm.company_name AS clientName, " +
                                                "cm2.company_name AS endUserName,bm1.post_code,bm1.place,bm2.branch_id AS MGOfficeID, bm2.branch_name AS MGOfficeName ";
        public string searchEndUserFROM_SQL = "FROM machine_master AS mm, company_master AS cm, company_master AS cm2," +
            "branch_master AS bm1, branch_master AS bm2 " +
            "where mm.delete_flag=false and mm.customer_id=cm.company_id " +
            $"and (mm.company_id=cm2.company_id AND cm2.kind={ConstData.COMPANY_KIND_ENDUSER})" +
            "and (mm.company_id=bm1.company_id AND mm.branch_id=bm1.branch_id) " +
            $"and (bm2.company_id={ConstData.NAKANISHI_ID} AND bm2.branch_id=bm1.general_branch_id) ";


        //得意先・エンドユーザー共通 総数取得用
        public string countCompanySELECT_SQL = "SELECT COUNT(DISTINCT (cm2.company_id,mm.customer_id,cm.company_name, cm2.company_name,bm1.post_code,bm1.place,bm2.branch_id, bm2.branch_name)) ";

        //ブランチ検索
        public string searchBranch_SQL = "SELECT DISTINCT bm2.branch_id,bm2.branch_name " +
            "FROM machine_master AS mm " +
            "INNER JOIN branch_master AS bm1 ON bm1.company_id=mm.company_id AND mm.delete_flag=false " +
            $"INNER JOIN branch_master AS bm2 ON bm1.general_branch_id=bm2.branch_id AND bm2.company_id={ConstData.NAKANISHI_ID} " +
            "WHERE bm2.lang_id=";

        //モデル(製品名)取得用
        //public readonly string searchModelSELECT_SQL = "SELECT DISTINCT mm.model_id,mmm.maker_id,lr.string AS modelName,cm.maker_name ";
        // 2024/01/29 修正 Yokoyama
        public readonly string searchModelSELECT_SQL = "SELECT DISTINCT mm.model_id,mmm.maker_id,lr.string AS modelName ";
        public readonly string searchModelFROM_SQL = "FROM machine_master AS mm " +
            "INNER JOIN company_master AS cm ON mm.customer_id=cm.company_id AND mm.delete_flag=false " +
            "LEFT JOIN company_master AS cm2 ON mm.company_id=cm2.company_id " +
            "LEFT JOIN branch_master AS bm1 ON mm.company_id=bm1.company_id AND mm.branch_id=bm1.branch_id " +
            $"LEFT JOIN branch_master AS bm2 ON bm2.company_id={ConstData.NAKANISHI_ID} AND bm1.general_branch_id=bm2.branch_id " +
            "LEFT JOIN m_model_master AS mmm USING(model_id) " +
            "LEFT JOIN lang_resource AS lr USING(str_id) ";

        //タイプ(品名)取得用
        public string searchTypeSELECT_SQL = "SELECT DISTINCT mtm.type_id,mtm.maker_id,mtm.model_id,lr.string AS type_name ";
        public string searchTypeFROM_SQL = "FROM machine_master AS mm " +
            "INNER JOIN company_master AS cm ON mm.customer_id=cm.company_id AND mm.delete_flag=false " +
            "LEFT JOIN company_master AS cm2 ON mm.company_id=cm2.company_id " +
            "LEFT JOIN branch_master AS bm1 ON mm.company_id=bm1.company_id AND mm.branch_id=bm1.branch_id " +
            $"LEFT JOIN branch_master AS bm2 ON bm2.company_id={ConstData.NAKANISHI_ID} AND bm1.general_branch_id=bm2.branch_id " +
                "LEFT JOIN m_type_master AS mtm USING(type_id) " +
                "LEFT JOIN lang_resource AS lr USING(str_id) ";

        //アラートデータ取得用
        public string searchAlertSELECT_SQL = "SELECT DISTINCT alert_id,ad.machine_id,alert_no,part_info,pkm.part_name,occur_time,release_time," +
            "cm.company_id AS clientID,cm.company_name AS clientName,cm2.company_id AS endUserID,cm2.company_name AS endUserName,mmm.model_id,mmm.description AS modelName," +
            "bm2.branch_id,bm2.branch_name AS MGOffice,mtm.type_id,lr.string AS typeName,mm.serial_number,mm.delivery_date,pm.total_time," +
            "lr1.string AS alert_name,am.alert_level,lr1.string AS detail,lr2.string AS cause,lr3.string AS treatString,lr4.string AS alertLevelString ";
        public string searchAlertFROM_SQL = "FROM alert_data AS ad " +
            "INNER JOIN machine_master AS mm ON ad.machine_id=mm.machine_id AND mm.delete_flag=false " +
            "LEFT JOIN alert_master AS am USING(alert_no) " +
            "LEFT JOIN parts_master AS pm USING(parts_id) " +
            "LEFT JOIN parts_kind_master AS pkm USING(parts_kind_id) " +
            "LEFT JOIN alert_level_master AS alm USING(alert_level) " +
            "INNER JOIN lang_resource AS lr1 ON am.alert_str_id=lr1.str_id " +
            "INNER JOIN lang_resource AS lr2 ON am.cause_str_id=lr2.str_id " +
            "INNER JOIN lang_resource AS lr3 ON am.treat_str_id=lr3.str_id " +
            "INNER JOIN lang_resource AS lr4 ON alm.level_str_id=lr4.str_id " +
            "LEFT JOIN m_model_master AS mmm ON am.model_id=mmm.model_id " +
            "LEFT JOIN m_type_master AS mtm USING(type_id) " +
            "LEFT JOIN company_master AS cm ON mm.customer_id=cm.company_id " +
            "LEFT JOIN company_master AS cm2 ON mm.company_id=cm2.company_id " +
            "INNER JOIN branch_master AS bm1 ON cm2.company_id=bm1.company_id and bm1.branch_id=mm.branch_id " +
            $"INNER JOIN branch_master AS bm2 ON bm1.general_branch_id=bm2.branch_id AND bm2.company_id={ConstData.NAKANISHI_ID} " +
            "LEFT JOIN lang_resource AS lr ON mtm.str_id=lr.str_id AND lr.lang_id=";
        public string countAlertSELECT_SQL = "SELECT COUNT(DISTINCT alert_id) ";
        public string searchAlertTypeSELECT_SQL = "SELECT DISTINCT alert_no,alert_name ";
        public string searchAlertLevelSELECT_SQL = "SELECT DISTINCT am.alert_level,lr4.string AS alertLevelString ";
        public string searchAlertKind_SQL = "select am.alert_no, lr.string from alert_master am " +
            "INNER JOIN lang_resource lr on am.alert_str_id = lr.str_id ";

        // ユーザ毎メール送信設定取得用
        public string searchMailSettingSELECT_SQL = "SELECT alert_no, parts_id FROM send_mail_user ";
        public string deleteMailSetting_SQL = "DELETE FROM send_mail_user ";
        public string insertMailSetting_SQL = "INSERT INTO send_mail_user (user_id, machine_id, mail_kind, alert_no, parts_id, entry_time) VALUES (";

        //パーツ情報取得用
        public string searchPartsSELECT_SQL = "SELECT parts_id,machine_id,pkm.part_name,parts_number," +
            "pkm.life_time,total_time,init_time,(pkm.life_time-(total_time+init_time))AS remainingTime," +
            "pkm.life_cycle,total_count,init_count,(pkm.life_cycle-(total_count+init_count))AS remainingCount," +
            "attach_time,mm.delivery_date,exchang_cunt ";
        public string searchPartsFROM_SQL = "FROM parts_master AS pm " +
            "INNER JOIN machine_master AS mm USING(machine_id) " +
            "INNER JOIN parts_kind_master AS pkm USING(parts_kind_id) ";
        public string searchPartsKind_SQL = "SELECT pm.parts_kind_id, lr.string, pkm.life_time,pkm.life_cycle FROM parts_master AS pm " +
            "INNER JOIN parts_kind_master pkm ON pm.parts_kind_id = pkm.parts_kind_id " +
            "INNER JOIN lang_resource lr ON pkm.str_id = lr.str_id ";

        //ログイン用
        public string loginSQL = "SELECT um.user_id,um.company_id,cm.company_name,cm.kind AS company_kind,lm.lang_code,um.branch_id,bm.general_branch_id," +
            "bm.branch_name,um.user_name,login_id,mail_s_time,mail_e_time,um.lang_id,um.kind,bm.time_diff,affiliation " +
            "FROM user_master AS um " +
            "INNER JOIN branch_master AS bm ON um.branch_id=bm.branch_id AND um.company_id=bm.company_id " +
            "INNER JOIN lang_master AS lm ON um.lang_id=lm.lang_id " +
            "INNER JOIN company_master AS cm ON um.company_id=cm.company_id ";
        public string passwordCheckSQL = "SELECT um.user_id FROM user_master AS um ";
        public string updateUserSQL = "UPDATE user_master SET ";

        //言語取得用
        public string langOptionSQL = "SELECT lang_id,name,name_jp,lang_code FROM lang_master WHERE available='1' ORDER BY lang_id";
        public string wordResourceSQL = $"SELECT str_id,string FROM lang_resource_web ";
        
        /// <summary>
        /// 絞り込んだ状態の検索総数を取得
        /// </summary>
        /// <remarks>絞り込み検索をかけるメソッドの中で使用</remarks>
        /// <returns>登録総数を表す整数値</returns>
        public int GetTotalRecordCount(string countSQL)
        {
            Debug.Print("GetTotalRecordCount : " + countSQL);

            int result = 0;
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(countSQL,connection))
            {
                connection.Open();
                result = int.Parse(command.ExecuteScalar().ToString());
            }
            connection.Close();
            return result;
        }

        /// <summary>
        /// ログイン時に発行された管理者権限を参照して、DB検索の際の絞り込み条件を追加する
        /// </summary>
        /// <param name="sql">DBに送るSQL文</param>
        public void PlusWhereWordByAdminFlag(ref string sql)
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            int userBranchID = int.Parse(hc.Session[SearchLabel.BRANCH_ID].ToString());
            int userCompanyID = int.Parse(hc.Session[SearchLabel.COMPANY_ID].ToString());
            if (DB_UserMaster.isBranchManager) { sql += $"{this.branchMG}{userBranchID} "; }
            if (DB_UserMaster.isBranch) { sql += $"{this.branch}{userBranchID} "; }
            if (DB_UserMaster.isClient) { sql += $"{this.client}{userCompanyID} "; }
            if (DB_UserMaster.isEndUser) { sql += $"{this.endUser}{userCompanyID} "; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transDepth"></param>
        /// <returns></returns>
        public int GetDataCount(string sql,bool transDepth = false)
        {
            Debug.Print("GetDataCount : " + sql);

            int result = 0;
            using(NpgsqlCommand command = new NpgsqlCommand(sql, this._dbObj.GetConnection()))
            {
                long dataNum = long.Parse(command.ExecuteScalar().ToString());
                result = (int)dataNum;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transDepth"></param>
        /// <returns></returns>
        public int ExecuteSQL(string sql,bool transDepth = false)
        {
            Debug.Print("ExecuteSQL : " + sql);

            int result = 0;
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                result = command.ExecuteNonQuery();
            }
            connection.Close();
            return result;
        }

        /// <summary>
        /// reader["int"]を整数値にして返す
        /// </summary>
        /// <param name="objectValue">reader["int"]</param>
        /// <returns>整数値</returns>
        public int GetDataReaderInt(object objectValue)
        {
            try
            {
                int intValue = -1;
                if (objectValue != null)
                {
                    int.TryParse(objectValue.ToString(), out intValue);
                }
                //int intValue = int.Parse(objectValue.ToString());
                return intValue;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// reader["long"]をロングの値(整数)で返す
        /// </summary>
        /// <param name="objectValue">reader["long"]</param>
        /// <returns>整数値</returns>
        public long GetDataReaderLong(object objectValue)
        {
            try
            {
                long longValue = -1;
                if (objectValue != null)
                {
                    long.TryParse(objectValue.ToString(), out longValue);
                }
                //long longValue = (long)objectValue;
                //long longValue = long.Parse(objectValue.ToString());
                return longValue;
            }
            catch(Exception ex)
            {
                Debug.Print($"GetDataReaderLong : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// reader["double"]を実数で返す
        /// </summary>
        /// <param name="objectValue">reader["double"]</param>
        /// <returns>実数値</returns>
        public double GetDataReaderDouble(object objectValue)
        {
            try
            {
                double doubleValue = -1;
                if (objectValue != null)
                {
                    double.TryParse(objectValue.ToString(), out doubleValue);
                }
                //double doubleValue = double.Parse(objectValue.ToString());
                return doubleValue;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 秒から時間へ変換
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public string SecondToHour(long second)
        {
            string result;
            if(second != ConstData.SEARCH_ALL)
            {
                if (second > ConstData.SEARCH_ALL)
                {
                    long min = (second % 3600) / 60;
                    long hour = second / 3600;
                    result = $"{ToDigits(hour)}:{ToDigits(min)}";
                }
                else
                {
                    long second_positive = -(second);
                    long min = (second_positive % 3600) / 60;
                    long hour = second_positive / 3600;
                    result = $"{ToDigits(hour)}:{ToDigits(min)}";
                }
            }
            else
            {
                result = ConstData.EMPTY;
            }
            return result;
        }

        /// <summary>
        /// 10以下の数値を0をつけて2桁表記にする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToDigits(long value) {
            string result;
            if (value > 10) {
                result = value.ToString();
            }
            else
            {
                result = $"0{value}";
            }
            return result;
        }

        /// <summary>
        /// データベースオブジェクトを実数値で返す
        /// </summary>
        /// <param name="ovall">オブジェクト</param>
        /// <return>実数値</return>
        public double GetDataReaderReal(object objectValue)
        {
            try
            {
                double doubleValue = float.Parse(objectValue.ToString());
                return doubleValue;
            }
            catch (Exception)
            {
                return double.MinValue;
            }
        }
        
        /// <summary>
        /// reader["string"]を文字列で返す
        /// </summary>
        /// <param name="objectValue">reader["string"]</param>
        /// <return>文字列</return>
        public string GetDataReaderString(object objectValue)
        {
            try
            {
                string stringValue = objectValue.ToString();
                return stringValue;
            }
            catch(Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// データベースオブジェクトをDateTimeで返す(local time)
        /// </summary>
        /// <param name="ovall">オブジェクト</param>
        /// <return>DateTime値</return>
        public DateTime GetDataReaderTime(object objectValue)
        {
            try
            {
                DateTime date = DateTime.MinValue;
                if (objectValue != null && objectValue is DateTime)
                {
                    date = (DateTime)objectValue;
                    date += TimeSpan.FromMinutes((double)_dbObj.serverTimeDiff);
                }
                return date;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// データベースオブジェクトをTimeSpanで返す
        /// </summary>
        /// <param name="ovall">オブジェクト</param>
        /// <return>DateTime値</return>
        public TimeSpan GetDataReaderTimeOnly(object objectValue)
        {
            try
            {
                DateTime date = DateTime.Parse(objectValue.ToString());
                return date.TimeOfDay;
            }
            catch
            {
                return TimeSpan.MinValue;
            }
        }

        /// <summary>
        /// データベースオブジェクトをDateTimeで返す(そのまま)
        /// </summary>
        /// <param name="ovall">オブジェクト</param>
        /// <return>DateTime値</return>
        public DateTime GetDataReaderDateTimeRow(object objectValue)
        {
            //DateTime day = Convert.ToDateTime(objectValue,IFormatProvider?provider);
            try
            {
                return (DateTime)objectValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 現在のローカル（ユーザ）時刻を返す
        /// </summary>
        /// <return>ローカル（ユーザ）時刻</return>
        public DateTime GetUserDateTime()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            int userTimeDiff = 1;//int.Parse(hc.Session["UserTD"].ToString());
            DateTime now = DateTime.Now;
            TimeSpan tdiff = new TimeSpan(0, _dbObj.serverTimeDiff - userTimeDiff, 0);
            now -= tdiff;
            return now;
        }

        /// <summary>
        /// 現在のGMT時刻文字列を返す
        /// </summary>
        /// <return>GMT日時文字列</return>
        public string GetGmtTimeStringNow()
        {
            DateTime now = DateTime.Now;
            TimeSpan tdiff = new TimeSpan(0, _dbObj.serverTimeDiff, 0);
            now -= tdiff;
            return now.ToString(DB_FORMAT_TIME);
        }

        /// <summary>
        /// 指定された時間をGMT時刻文字列で返す
        /// </summary>
        /// <param name="someDay">日時</param>
        /// <return>GMT日時文字列</return>
        public string GetGmtTimeString(DateTime someDay)
        {
            TimeSpan tdiff = new TimeSpan(0, _dbObj.serverTimeDiff, 0);
            someDay -= tdiff;
            return someDay.ToString(DB_FORMAT_TIME);
        }

        /// <summary>
        /// 指定された時間を文字列で返す
        /// </summary>
        /// <param name="td">日時</param>
        /// <return>日時文字列</return>
        public string GetTimeString(DateTime td)
        {
            return td.ToString(DB_FORMAT_TIME);
        }

        public bool GetDataReaderBoolean(object value)
        {
            bool result;
            try { 
                result = (bool)value;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// SQLで使用する、時差文字列を返す
        /// </summary>
        /// <return>SQL文字列</return>
        public string GetSqlTimeDiffString()
        {
            string retval = String.Format(@"+interval '{0} minutes'", _dbObj.serverTimeDiff.ToString());
            return retval;
        }

    }
}
