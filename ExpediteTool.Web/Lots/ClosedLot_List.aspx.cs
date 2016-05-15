using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.Concretes;
using ExpediteTool.Model.DataTransfer;
using Ninject;
using System;
using System.Collections.Generic;

namespace ExpediteTool.Web.Lots
{
    public partial class Closed_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grdClosedHotLots.PageSize = ExpediteTool.Web.Constants.ConfigManager.PAGE_SIZE;
                grdClosedHotLots.PagerSettings.PageButtonCount = ExpediteTool.Web.Constants.ConfigManager.PAGE_BUTTON_COUNT;

                //txtSearchAll.Attributes.Add("onKeyPress",
                // "doClick('" + btnSearchAll.ClientID + "',event)");
            }
        }

        public List<LotExpediteDto> GetClosedHotLots(int startRowIndex, int pageSize)
        {
            LotExpediteRepository service = new LotExpediteRepository();
            return service.GetHotLotDataByStatus(StatusType.CLOSED, startRowIndex, pageSize);
        }

        public int GetCountClosedHotLots()
        {
            LotExpediteRepository service = new LotExpediteRepository();
            return service.GetCountClosedHotLots();
        }

        [Inject]
        public static ILotExpediteRepository LotExpediteRep { get; set; }

        protected void grdClosedHotLots_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            grdClosedHotLots.DataBind();
        }

        protected void btnSearchAll_Click(object sender, EventArgs e)
        {
            string textSearch = String.Format("LotSearchResult.aspx?search={0}&type={1}", txtSearchAll.Value, "2");
            Response.Redirect(textSearch, true);
        }
    }
}