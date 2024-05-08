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
    public class DB_TypeMaster:DBBase
    {
        public DB_TypeMaster(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// 検索ボックス用「型式」を全件取得する
        /// </summary>
        /// <param name="typeList">情報格納用リスト</param>
        /// <param name="langID">言語ID</param>
        public void SearchAllTypeList(out List<MachineType> typeList,int langID)
        {
            string sql = this.searchTypeSELECT_SQL + this.searchTypeFROM_SQL;
            typeList = new List<MachineType>();
            string WHERE =  $"WHERE lr.lang_id={langID}";
            sql += WHERE;

            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("SearchAllTypeList : " + sql);

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
                        string typeID = GetDataReaderString(reader["type_id"]);
                        string name = GetDataReaderString(reader["type_name"]);
                        MachineType type = new MachineType(makerID, modelID, typeID, name);
                        typeList.Add(type);
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// 検索ボックス用「型式」を条件付きで取得する
        /// </summary>
        /// <param name="typeList">情報格納用リスト</param>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="langID">言語ＩＤ</param>
        public void SearchTypeList_ofSearchCondition(out List<MachineType>typeList,ref MachineBase searchCondition,int langID)
        {
            typeList = new List<MachineType>();
            List<string> whereSQLs = new List<string>();    //searchConditionに沿って、追加する検索条件を格納するリスト

            //::::: searchConditionの各プロパティ用のWHERE句SQL
            string sql = this.searchTypeSELECT_SQL + this.searchTypeFROM_SQL + "WHERE ";

            //::::: searchConditionの各プロパティがデフォルトかどうか(デフォルトでなければWHERE句リストに追加)
            if(searchCondition.companyID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.companyID_SQL}{searchCondition.companyID} "); }
            if(searchCondition.endUserCode != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.endUserCD_SQL}{searchCondition.endUserCode} "); }
            if(searchCondition.managementOfficeID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.MGOfficeID_SQL}{searchCondition.managementOfficeID} "); }
            if (searchCondition.modelID != ConstData.SEARCH_ALL) { whereSQLs.Add($"{this.modelID_SQL}{searchCondition.modelID} "); }
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

            Debug.Print("SearchTypeList_ofSearchCondition : " + sql);

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
                        int mID = GetDataReaderInt(reader["model_id"]);
                        string typeID = GetDataReaderString(reader["type_id"]);
                        string name = GetDataReaderString(reader["type_name"]);
                        MachineType type = new MachineType(makerID, mID, typeID, name);
                        typeList.Add(type);
                    }
                }
            }
            connection.Close();
        }
    }
}
