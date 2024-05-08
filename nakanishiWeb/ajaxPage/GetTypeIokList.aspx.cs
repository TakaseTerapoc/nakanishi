using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb.ajaxPage
{
    public partial class GetTypeIokList : System.Web.UI.Page
    {
        public DB_MachineMaster machineMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public Dictionary<int, string> typeIokList;
        public char split = ConstData.VALUE_SPLIT_CHAR;
        protected void Page_Load(object sender, EventArgs e)
        {
            string modelID = Funcs.GetStringIdFromValue(Request.QueryString[SearchLabel.MODEL_ID]);
            machineMaster = new DB_MachineMaster(this._dbObj);
            if(modelID != ConstData.EMPTY)
            {
                machineMaster.GetTypeIokList_connectModelID(out typeIokList, int.Parse(modelID));
            }
            else
            {
                machineMaster.GetTypeIokList_All(out typeIokList);
            }
        }
    }
}