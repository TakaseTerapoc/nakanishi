using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using nakanishiWeb.Const;

namespace nakanishiWeb.Utils
{
    public class TimeFormat
    {
        public const int TYPE_DATE = 1;
        public const int TYPE_TIME = 2;
        public const int TYPE_TIME2 = 3;
        public const int TYPE_MONTH = 4;
        public const int TYPE_HOUR = 5;
        public const int TYPE_DATETIME = 0;

        public const string FORMAT_DATE = "yyyy/MM/dd";
        public const string FORMAT_TIME = "HH:mm:ss";
        public const string FORMAT_DATETIME = "yyyy/MM/dd HH:mm:ss";

        public const string FORMAT_MONTH = "yyyy/MM";
        public const string FORMAT_HOUR = "yyyy/MM/dd HH:00";
    }

    public class Funcs
    {
        /// <summary>
        /// リクエストフォームから数値型のIDだけを切り出して返す
        /// </summary>
        /// <remarks>リクエストフォームから送信された値=>「id,name」</remarks>
        /// <param name="value">リクエストフォームからの値</param>
        /// <returns>整数値型のID</returns>
        public static int GetIdFromValue(string value)
        {
            int result;
            string[] splitValues = value.Split(ConstData.VALUE_SPLIT_CHAR);
            result = int.Parse(splitValues[ConstData.VALUES_ID_INDEX]);
            return result;
        }

        /// <summary>
        /// 異常終了
        /// </summary>
        public static void ExceptionEnd(string msg)
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            hc.Session["ErrorMessage"] = msg;
            hc.Response.Redirect("/Http500Form.aspx");
        }
        
        /// <summary>
        /// stringをintにして返す
        /// </summary>
        /// <param name="value">intに変換したい文字列</param>
        /// <returns>引数をintに変換したもの</returns>
        public static int GetIntFromStringValue(string value)
        {
            int result;
            if(value != ConstData.EMPTY)
            {
                result = int.Parse(value);
            }
            else
            {
                result = ConstData.SEARCH_ALL;
            }
            return result;
        }

        /// <summary>
        /// リクエストフォームから文字列型のIDだけを切り出して返す
        /// </summary>
        /// <remarks>リクエストフォームから送信された値=>「id,name」</remarks>
        /// <param name="value">リクエストフォームからの値</param>
        /// <returns>文字列型のID</returns>
        public static string GetStringIdFromValue(string value)
        {
            string result;
            string[] splitValues = value.Split(ConstData.VALUE_SPLIT_CHAR);
            result = splitValues[ConstData.VALUES_ID_INDEX];
            return result;
        }

        /// <summary>
        /// リクエストフォームからnameだけを切り出して返す
        /// </summary>
        /// <remarks>リクエストフォームから送信された値=>「id,name」</remarks>
        /// <param name="value">リクエストフォームからの値</param>
        /// <returns>文字列型のname</returns>
        public static string GetNameFromValue(string value)
        {
            string result;
            string[] splitValues = value.Split(ConstData.VALUE_SPLIT_CHAR);
            result = splitValues[ConstData.VALUES_NAME_INDEX];
            return result;
        }
        /// <summary>
        /// セッションに格納されているユーザーがタイムアウトになっていないか
        /// </summary>
        public static bool IsSessionAlive() {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            bool result = true;
            if(hc.Session["userID"] == null)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Sessionが時間切れになったときの処理
        /// </summary>
        public static void SessionTimeOutEnd()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            string url = hc.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
            hc.Response.Redirect($"{url}/SessionTimeOut.aspx");
        }

        /// <summary>
        /// オブジェクトのNULL判定
        /// </summary>
        /// <param name="obj">NULL判定したいオブジェクト</param>
        /// <returns>NOTNULL⇒true・NULL⇒false</returns>
        public static bool IsNotNullObject(object obj)
        {
            if(obj != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 管理会社かどうかの判定
        /// </summary>
        /// <returns>管理会社⇒true・他⇒false</returns>
        public static bool IsAdminCompany()
        {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            try
            {
                if(hc.Session["CompanyAdmin"] != null)
                {
                    string companyID = hc.Session["companyAdmin"].ToString();
                    if (companyID.Equals("true"))
                    {
                        return true;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.Print($"Funcs.IsAdminCompany:[{e.Message}]");
            }
            return false;
        }

        /// <summary>
        /// 文字列から整数値へ
        /// </summary>
        /// <remarks>パースできない場合は「-1」を返却</remarks>
        /// <param name="stringValue">文字列</param>
        /// <returns>整数値</returns>
        public static int ToInteger(string stringValue) {
            int result = -1;
            try
            {
                result = int.Parse(stringValue);
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        /// <summary>
        /// 整数値から文字列へ
        /// <remarks>変換できなければ「-」を返却</remarks>
        /// </summary>
        /// <param name="value">整数値</param>
        /// <returns>文字列</returns>
        public static string IntToString(int value)
        {
            string result = "-";
            if(value >= 0)
            {
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// ロング値から文字列へ
        /// </summary>
        /// <remarks>変換できなければ「-」を返却</remarks>
        /// <param name="value">ロング値</param>
        /// <returns>文字列</returns>
        public static string LongToString(long value)
        {
            string result = "-";
            if(value >= 0L)
            {
                result = result.ToString();
            }
            return result;
        }

        /// <summary>
        /// パスワードハッシュ化
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
