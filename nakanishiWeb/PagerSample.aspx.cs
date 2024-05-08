using nakanishiWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace nakanishiWeb
{
    public partial class PagerSample : System.Web.UI.Page
    {
        public PagerController pager;
        public Dictionary<int, string> sampleDict = new Dictionary<int, string>();
        public string[] names = { "Apple","Bakery","Citrus","Discover","Elmo","Fablic","Gate","HushTag","Illust","Journey","King","Lemon","Memory","Number","Octopas","Pearl","Queen","Rider","Skin","T-shirt","UFO","VioTechnology","Weather","Xray","Yesterday","Zoo"};
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sampleDict.Count == 0)
            {
                sampleDict = new Dictionary<int, string>();
                for (int i = 0; i < names.Length; i++)
                {
                    sampleDict.Add(i + 1, names[i]);
                }
            }
            pager = new PagerController(sampleDict.Count,5);
            if (Request.Form["clickedPager"] != null) {
                pager.SetNowPageNo(int.Parse(Request.Form["clickedPager"]));
                pager.SetOffset();
            }
        }
    }
}