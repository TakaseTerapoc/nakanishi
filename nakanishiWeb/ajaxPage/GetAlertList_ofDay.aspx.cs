using nakanishiWeb.Const;
using nakanishiWeb.DataAccess;
using nakanishiWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb.ajaxPage
{
    public partial class GetAlertList : System.Web.UI.Page
    {
        public DB_AlertMaster alertMaster;
        public DataAccessObject _dbObj = new DataAccessObject();
        public List<Alert> machineAlertList;

        public string graphData;
        public string sStartDate;
        public string DayOrMonth;
        public int alertNo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Funcs.IsSessionAlive())
            {
                try
                {
                    int selectMachineID = int.Parse(Session[SearchLabel.CHOOSE_MACHINE_ID].ToString());
                    int langID = int.Parse(Session[SearchLabel.LANGUAGE].ToString());
                    alertMaster = new DB_AlertMaster(this._dbObj);
                    if ((Funcs.IsNotNullObject(Request.QueryString[SearchLabel.ALERT_NO])) && (Request.Form[SearchLabel.ALERT_NO] != ConstData.EMPTY))
                    {
                        if (Session[S_ConditionLabel.DAY_OR_MONTH] != null)
                        {
                            DayOrMonth = Session[S_ConditionLabel.DAY_OR_MONTH].ToString();
                        }
                        else
                        {
                            DayOrMonth = "day";
                        }
                        alertNo = int.Parse(Request.QueryString[SearchLabel.ALERT_NO]);
                        PagerController pager = new PagerController(10, 10);
                        //alertMaster.SearchMachineAlert(out machineAlertList,langID,ref pager,selectMachineID);
                        switch (DayOrMonth)
                        {
                            case "day":
                                //     MakeDayGraphData(in machineAlertList);
                                break;
                            case "month":
                                //  MakeMonthGraphData();
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _dbObj.errorMsg = ex.Message;
                    DB_SystemLog syslog = new DB_SystemLog(_dbObj);
                    syslog.InsertSystemLog();
                    Funcs.ExceptionEnd(ex.Message);
                }
            }
            else
            {
                Funcs.SessionTimeOutEnd();
            }
        }

        /// <summary>
        /// 月別グラフを描画するためのデータ成型メソッド
        /// </summary>
        public void MakeMonthGraphData() { }

        /// <summary>
        /// 日別グラフを描画するためのデータ成型メソッド
        /// </summary>
        public void MakeDayGraphData(in List<Alert> alertList)
        {
            if (Funcs.IsNotNullObject(alertList))
            {
                sStartDate = alertList[0].occurTime.ToString("yyyy/MM/dd");
            }
            graphData = "[";
            Dictionary<int, int> t_tool = new Dictionary<int, int>();
            int count = 1;
            int gPOS = 0;
            for(int i=0; i < alertList.Count; i++)
            {
                if(i != 0) { graphData += ",";  }
                gPOS = count;
                count++;
                bool workFlag = false;
                if(alertList[i].releaseTime < alertList[i].occurTime)
                {
                    alertList[i].releaseTime = DateTime.Now;
                    workFlag = true;
                }
                //double realTime = partsList[i].GetTotalWorkTime(DateTime.Now);
                graphData += $"{{key: {1},";
                graphData += $"name: '',";
                graphData += $"start: new Date(\'{alertList[i].occurTime.ToString("yyyy/MM/dd HH:mm:ss")}\'),";
                graphData += $"end: new Date(\'{alertList[i].releaseTime.ToString("yyyy/MM/dd HH:mm:ss")}\'),";
                graphData += $"lavel: '',";
                graphData += $"flag: '{workFlag}'";
                graphData += "}";
            }
            graphData += "];";
        }
    }
}