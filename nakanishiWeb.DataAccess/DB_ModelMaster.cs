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
    public class DB_ModelMaster:DBBase
    {
        public DB_ModelMaster(DataAccessObject dbObj) {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// 検索ボックス用「製品群」の一覧取得
        /// </summary>
        /// <param name="modelList">情報格納用リスト</param>
        /// <param name="langID">言語ID</param>
        public void SearchModelList_All(out List<Model> modelList,int langID)
        {
            modelList = new List<Model>();
            string sql = this.searchModelSELECT_SQL + searchModelFROM_SQL + $"WHERE {this.langSQL}{langID}";
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchModelList_All : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int makerID = GetDataReaderInt(reader["maker_id"]);
                        int modelID = GetDataReaderInt(reader["model_id"]);
                        string name = GetDataReaderString(reader["modelName"]);
                        Model model = new Model(makerID, modelID, name);
                        modelList.Add(model);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 検索ボックス用「製品群」リストを条件付きで取得する
        /// </summary>
        /// <param name="modelList">情報格納用リスト</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="langID">言語ID</param>
        public void SearchModel_ofSearchCondition(out List<Model>modelList,ref MachineBase searchCondition,int langID)
        {
            modelList = new List<Model>();
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト

            //::::: searchConditionの各プロパティ用のWHERE句SQL
            string sql = this.searchModelSELECT_SQL + this.searchModelFROM_SQL + "WHERE ";

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if(searchCondition.endUserCode != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.endUserCD_SQL}{searchCondition.endUserCode} "); }
            if(searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if(searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            whereSQLs.Add($"{this.langSQL}{langID}");     //lang_idのWHERE句は必ずリストに追加

            //::::: リストに追加されたWHERE句のSQL文のみサーチ用SQLに追加
            for(int i=0; i<whereSQLs.Count; i++)
            {
                sql += whereSQLs[i];
                if(i != whereSQLs.Count - 1)//最後の要素でなければ
                {
                    sql += "AND ";
                }
            }

            //ユーザー権限によるWHERE句の追加
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchModel_ofSearchCondition : " + sql);

            //::::: DB接続
            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int makerID = GetDataReaderInt(reader["maker_id"]);
                        int modelID = GetDataReaderInt(reader["model_id"]);
                        string name = GetDataReaderString(reader["modelName"]);
                        Model model = new Model(makerID, modelID, name);
                        modelList.Add(model);
                    }
                }
            }
            connection.Close();
        }
    }
}
