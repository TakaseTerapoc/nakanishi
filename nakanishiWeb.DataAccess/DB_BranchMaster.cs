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
    public class DB_BranchMaster:DBBase
    {
        public DB_BranchMaster(DataAccessObject dbObj)
        {
            this._dbObj = dbObj;
        }

        /// <summary>
        /// 管理会社ブランチの名前とＩＤをデータベースから取得しリストに格納
        /// </summary>
        /// <param name="adminBranchList">データ格納用リスト</param>
        public void GetAdminBranchList(out List<Branch> adminBranchList,int langID) {
            adminBranchList = new List<Branch>();
            string sql = this.searchBranch_SQL+$"{langID} ";
            this.PlusWhereWordByAdminFlag(ref sql);

            Debug.Print("GetAdminBranchList : " + sql);

            NpgsqlConnection connection = this._dbObj.GetConnection();
            using (NpgsqlCommand command = new NpgsqlCommand(sql,connection))
            {
                connection.Open();
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Branch branch = new Branch();
                            branch.branchID = GetDataReaderInt(reader["branch_id"]);
                            branch.branchName = GetDataReaderString(reader["branch_name"]);
                            adminBranchList.Add(branch);
                        }
                    }
                }
            }
            connection.Close();
        }
    }
}
