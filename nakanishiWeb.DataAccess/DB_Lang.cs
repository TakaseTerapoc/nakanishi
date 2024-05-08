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
    public class DB_Lang : DBBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>引数1</remarks>
        ///<params name="dbObject">DataAccessObject</params>
        public DB_Lang(DataAccessObject dbObject)
        {
            this._dbObj = dbObject;
        }

        /// <summary>
        /// 言語セレクト用のデータ(日本語・英語など)をデータベースから取得
        /// </summary>
        /// <param name="langList">取得した言語データを格納するリスト</param>
        public void GetLangList(out List<LanguageData> langList) {
            langList = new List<LanguageData>();

            Debug.Print("GetLangList : " + this.langOptionSQL);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(this.langOptionSQL,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = GetDataReaderInt(reader["lang_id"].ToString());
                        string name = GetDataReaderString(reader["name"]);
                        string code = GetDataReaderString(reader["lang_code"]);
                        LanguageData lang = new LanguageData(id, name, code);
                        langList.Add(lang);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 共通の単語のディクショナリーリストを作る
        /// </summary>
        /// <param name="langID">言語ID</param>
        public void GetWordResorceCommon(int langID, out Dictionary<int, string> commonWords)
        {
            commonWords = new Dictionary<int, string>();
            string SQL = this.wordResourceSQL + $"WHERE lang_id={langID} AND page_id={(int)LanguageTable.PageId.Common} ORDER BY str_id ASC";

            Debug.Print("GetWordResorceCommon : " + SQL);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(SQL, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    int id = 0;
                    string value;
                    if ((commonWords == null) || (commonWords.Count == 0))
                    {
                        while (reader.Read())
                        {
                            id = GetDataReaderInt(reader["str_id"]);
                            value = GetDataReaderString(reader["string"]);
                            commonWords.Add(id, value);
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            id = GetDataReaderInt(reader["str_id"]);
                            if (commonWords[id] != null)
                            {
                                commonWords[id] = reader["string"].ToString();
                            }
                        }
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// ページごとの単語のディクショナリーリストを作る
        /// </summary>
        /// <param name="langID">言語ID</param>
        /// <param name="pageID">ページID</param>
        /// <param name="wordDict">ディクショナリーリスト</param>
        public void GetWordResorce(int langID, int pageID, out Dictionary<int, string> wordDict)
        {
            wordDict = new Dictionary<int, string>();
            string SQL = this.wordResourceSQL + $"WHERE lang_id={langID} AND page_id={pageID} ORDER BY str_id ASC";

            Debug.Print("GetWordResorce : " + SQL);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(SQL, connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    int id = 0;
                    string value;
                    if ((wordDict == null) || (wordDict.Count == 0))
                    {
                        while (reader.Read())
                        {
                            id = GetDataReaderInt(reader["str_id"]);
                            value = reader["string"].ToString();
                            wordDict.Add(id, value);
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            id = GetDataReaderInt(reader["str_id"]);
                            if (wordDict[id] != null)
                            {
                                wordDict[id] = reader["string"].ToString();
                            }
                        }
                    }
                }
            }
            connection.Close();
        }
    }
}
