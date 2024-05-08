using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakanishiWeb.Utils
{
    public class Validate
    {
        /// <summary>
		/// 文頭にスペースあり
		/// </summary>
        public static readonly int inTopSpace = 0x0001;

        /// <summary>
        /// 文末にスペースあり
        /// </summary>
        public static readonly int inBottomSpace = 0x0002;

        /// <summary>
        /// 使用不可文字あり
        /// </summary>
        public static readonly int inErrorSymbol = 0x0004;

        /// <summary>
        /// 半角カナあり
        /// </summary>
        public static readonly int inHankaku = 0x0008;

        /// <summary>
        /// ローマ字あり
        /// </summary>
        public static readonly int inRoman = 0x0010;

        /// <summary>
        /// 数値あり
        /// </summary>
        public static readonly int inNumeric = 0x0020;

        /// <summary>
        /// 文字あり
        /// </summary>
        public static readonly int inSymbol = 0x0040;

        /// <summary>
        /// 全角あり
        /// </summary>
        public static readonly int inFullSize = 0x000F;

        /// <summary>
        /// 入力エラー
        /// </summary>
        public static readonly int inputError = 0x000F;
        
        /// <summary>
        /// パスワード最低文字数
        /// </summary>
        public static readonly int minPassword = 6;

        /// <summary>
        /// 入力文字列のチェック
        /// </summary>
        /// <param name="value">入力文字列</param>
        /// <param name="checkFlag">チェックフラグ</param>
        /// <returns>真偽値</returns>
        public static bool CheckInputData(string value/*, ref int checkFlag*/)
        {
            bool result;
            if((value == null) || (value == ""))
            {
                result = true;
            }
            //::::: 不正な文字が1つでも含まれていたら【false】にする
            if ((IsTopSpace(value)) || (IsBottomSpace(value)) || (IsErrorSymbol(value)) || (IsHankana(value)) || (IsDoubleQuotation(value)))
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 正規表現での文字列チェック(入力チェックのベース)
        /// </summary>
        /// <remarks>例えばスペースなどが先頭に含まれていると【true】</remarks>
        /// <param name="value">入力された値</param>
        /// <param name="pattern">チェックする文字列パターン</param>
        /// <returns>真偽値</returns>
        public static bool IsContain(string value , string pattern)
        {
            bool result = false;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.Match match = regex.Match(value);
            if (match.Success)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 正しい数値かをチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>true:number</returns>
        public static bool IsNumber(string value)
        {
            return IsContain(value,"[0-9.+-]+");
        }

        /// <summary>
        /// 自然数チェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsNotNatureNumber(string value)
        {
            return IsContain(value, "[^0-9]+");
        }

        /// <summary>
        /// 整数チェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsNotNatureNumberSign(string value)
        {
            bool result;
            string temp = value.Trim();
            if(temp.Length > 1)
            {
                if ((temp.StartsWith("0")) || (temp.StartsWith("-0")))
                {
                    result = true;
                }
            }
            result = !(IsContain(value,"^(-)?\\d+$"));
            return result;
        }

        /// <summary>
        /// アスキー文字が入力されていないか
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsNotAscii(string value)
        {
            return IsContain(value, "[^-~]");
        }

        /// <summary>
        /// アルファベットのみの入力かをチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsRoman(string value)
        {
            return IsContain(value,"[a-zA-Z]+");
        }

        /// <summary>
        /// 文頭スペースチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsTopSpace(string value)
        {
            return IsContain(value,"^\\s");
        }

        /// <summary>
        /// 文末スペースチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsBottomSpace(string value)
        {
            return IsContain(value, "\\s$");
        }

        /// <summary>
        /// 使用可文字チェック
        /// </summary>
        /// <param name="value"></param>
        /// <returns>真偽値</returns>
        public static bool IsSymbol(string value)
        {
            //\\?
            return IsContain(value, "[_@:\\$\\!\\=\\(\\)\\{\\}\\[\\]\\~\\/\\^\\;\\\\]+");
        }

        /// <summary>
        /// 使用不可文字チェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsErrorSymbol(string value)
        {
            return IsContain(value, "[#&%'\"\\*\\<\\>]+");
        }

        /// <summary>
        /// 半角カタカナチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsHankana(string value)
        {
            return IsContain(value, "[ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝｧｨｩｪｫｯｬｭｮｰ｡｢｣､･ﾞﾟ]+");
        }

        /// <summary>
        /// 全角文字チェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsFullSize(string value)
        {
            return IsContain(value, "[^\x00-\x7F]+");
        }

        /// <summary>
        /// ダブルクォーテーションチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsDoubleQuotation(string value)
        {
            bool result;
            if((value == null) || (value == ""))
            {
                result = false;
            }
            result = IsContain(value,"[\"]+");
            return result;
        }

        /// <summary>
        /// メールフォーマットチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsMailAddress(string value)
        {
            return IsContain(value, "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
        }

        /// <summary>
        /// メールフォーマットチェック
        /// </summary>
        /// <param name="value">入力された値</param>
        /// <returns>真偽値</returns>
        public static bool IsMailAddressNotAllowChar(string value)
        {
            return IsContain(value,"[^0-9A-Za-z-@_.]");
        }
    }
}
