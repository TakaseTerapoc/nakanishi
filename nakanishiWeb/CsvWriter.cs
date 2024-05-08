using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using nakanishiWeb.General;
using nakanishiWeb.DataAccess;

namespace nakanishiWeb
{
    public class CsvWriter
    {
        /// <summary>
        /// CSVテキストを作成（製品一覧）
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="machineList"></param>
        /// <returns></returns>
        public static string CreateMachineListCsvText(string[] columns, List<Machine> machineList)
        {
            var sb = new StringBuilder();

            // ヘッダーの作成
            sb.AppendLine(CreateCsvHeader(columns));

            // ボディの作成
            machineList.ForEach(a => sb.AppendLine(CreateMachineListCsvBody(a)));

            return sb.ToString();
        }

        /// <summary>
        /// CSVテキストを作成（ユーザ一覧）
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="searchEndUserList"></param>
        /// <returns></returns>
        public static string CreateClientListCsvText(string[] columns, List<Company> searchEndUserList)
        {
            var sb = new StringBuilder();

            // ヘッダーの作成
            sb.AppendLine(CreateCsvHeader(columns));

            // ボディの作成
            searchEndUserList.ForEach(a => sb.AppendLine(CreateClientListCsvBody(a)));

            return sb.ToString();
        }

        /// <summary>
        /// CSVテキストを作成（アラート一覧）
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="searchEndUserList"></param>
        /// <returns></returns>
        public static string CreateAlertListCsvText(string[] columns, List<Alert> alertList)
        {
            var sb = new StringBuilder();

            // ヘッダーの作成
            sb.AppendLine(CreateCsvHeader(columns));
            // ボディの作成
            alertList.ForEach(a => sb.AppendLine(CreateAlertListCsvBody(a)));

            return sb.ToString();
        }

        /// <summary>
        /// CSVヘッダー文字列を作成
        /// </summary>
        /// <param name="headerList"></param>
        /// <returns></returns>
        private static string CreateCsvHeader(string[] headerList)
        {
            var sb = new StringBuilder();
            foreach (var header in headerList)
            {
                sb.Append($@"""{header}"",");
            }
            // 最後のカンマを削除して返す
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// CSV定義情報文字列の作成
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string CreateMachineListCsvBody(Machine machine)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format($@"""{machine.endUserName}"","));           // エンドユーザー名
            sb.Append(string.Format($@"""{machine.modelName}"","));             // 製品群
            sb.Append(string.Format($@"""{machine.typeName}"","));              // 品名
            sb.Append(string.Format($@"""{machine.serialNumber}"","));          // S/N
            string settingDate = machine.settingDate.ToString("yyyy/MM/dd");    
            if (settingDate == "0001/01/01")
            {
                settingDate = "-";
            }
            sb.Append(string.Format($@"""{settingDate}"","));                   // 設置日
            sb.Append(string.Format($@"""{machine.operateHour}"","));           // 稼働時間
            sb.Append(string.Format($@"""{machine.companyName}"","));           // 得意先名
            sb.Append(string.Format($@"""{machine.managementOffice}"","));      // 担当支店営業所
            string lastTime = machine.lastTime.ToString("yyyy/MM/dd");
            if (lastTime == "0001/01/01")
            {
                lastTime = "-";
            }
            sb.Append(string.Format($@"""{lastTime}"","));                      // 最終通信時間

            var span = DateTime.Today - machine.settingDate;
            sb.Append(string.Format($@"""{span.Days}"","));                     // 製品年齢

            return sb.ToString();
        }

        /// <summary>
        /// CSV定義情報文字列の作成
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string CreateClientListCsvBody(Company client)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format($@"""{client.companyName}"","));            // エンドユーザー名
            sb.Append(string.Format($@"""{client.connectionCompanyName}"","));  // 得意先名
            sb.Append(string.Format($@"""{client.MGOfficeName}"","));           // 担当支店営業所
            return sb.ToString();
        }

        /// <summary>
        /// CSV定義情報文字列の作成
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string CreateAlertListCsvBody(Alert alert)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format($@"""{alert.occurTime.ToString("yyyy/MM/dd HH:mm:ss")}"","));   // 発生日時
            if (alert.isNowAlert)
            {
                sb.Append(string.Format($@"""-"","));
            }
            else
            {
                sb.Append(string.Format($@"""{alert.releaseTime.ToString("yyyy/MM/dd HH:mm:ss")}"",")); // 解除日時
            }

            sb.Append(string.Format($@"""{alert.alertLevelString}"","));                    // アラートレベル
            sb.Append(string.Format($@"""{alert.alertName}"","));                           // アラート名
            sb.Append(string.Format($@"""{alert.endUserName}"","));                         // エンドユーザー名
            sb.Append(string.Format($@"""{alert.modelName}"","));                           // 製品群
            sb.Append(string.Format($@"""{alert.typeName}"","));                            // 品名
            sb.Append(string.Format($@"""{alert.machineSerialNumber}"","));                 // S/N
            sb.Append(string.Format($@"""{alert.settingDate.ToString("yyyy/MM/dd")}"","));  // 設置日
            sb.Append(string.Format($@"""{alert.MGOfficeName}"","));                        // 担当支店営業所
            sb.Append(string.Format($@"""{alert.companyName}"","));                         // 得意先名
            return sb.ToString();
        }
    }
}