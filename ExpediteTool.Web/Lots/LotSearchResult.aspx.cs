using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ninject;
using Ninject.Web;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model;
using ExpediteTool.Model.DataTransfer;

namespace ExpediteTool.Web.Lots
{
    public partial class LotSearchResult : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        #region Function
        /// <summary>
        /// 
        /// </summary>
        private void BindGridView()
        {
            string search = Request.QueryString["search"];
            string type = Request.QueryString["type"];
            if (!String.IsNullOrEmpty(search))
            {
                txtSearchFor.Text = HttpUtility.HtmlEncode(search);
                Search(type, search);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void Search(string type, string textSearch)
        {
            string text = textSearch.Trim().TrimStart().TrimEnd();

            var items = new List<ExpediteTool.Model.DataTransfer.LotExpediteDto>();
            switch (type)
            {
                case "1"://search Active and pending hotlots
                    items = LotExpediteRepository.SearchAll(text).Where(x => x.Status == StatusType.ACTIVED ||
                                                                        x.Status == StatusType.PENDING).ToList();
                    break;
                case "2"://search Closed hotlots
                    items = LotExpediteRepository.SearchAll(text).Where(x => x.Status == StatusType.CLOSED).ToList();
                    break;
            }
            grdSearchHotLots.PageSize = ExpediteTool.Web.Constants.ConfigManager.PAGE_SIZE;
            grdSearchHotLots.PagerSettings.PageButtonCount = ExpediteTool.Web.Constants.ConfigManager.PAGE_BUTTON_COUNT;
            grdSearchHotLots.DataSource = items;
            grdSearchHotLots.DataBind();
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="idHotLot">The identifier hot lot.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private ActionResult UpdateStatus(int idHotLot, StatusType status)
        {
            return LotExpediteRepository.UpdateStatus(idHotLot, status, User.Identity.Name);
        }

        #endregion end function

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdgrdSearchHotLots_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            BindGridView();
            grdSearchHotLots.PageIndex = e.NewPageIndex;
            grdSearchHotLots.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the btnCloseStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnActivedStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            if (id > 0)
            {
                ActionResult actionResult = UpdateStatus(id, StatusType.ACTIVED);
                if (actionResult == ActionResult.SUCCESS)
                    Response.Redirect("ExpediteLot_List.aspx", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPendingStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            if (id > 0)
            {
                ActionResult actionResult = UpdateStatus(id, StatusType.PENDING);
                if (actionResult == ActionResult.SUCCESS)
                    Response.Redirect("ExpediteLot_List.aspx", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPendingClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCloseStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int id = 0;
            int.TryParse(btn.CommandArgument, out id);
            if (id > 0)
            {
                ActionResult actionResult = UpdateStatus(id, StatusType.CLOSED);
                if (actionResult == ActionResult.SUCCESS)
                    Response.Redirect("ExpediteLot_List.aspx", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSearchHotLots_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Request.QueryString["type"] == "2" ||
                    !ExpediteTool.Web.IdentityExtension.IsAdmin)
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[9].Width = 0;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ExpediteTool.Web.IdentityExtension.IsAdmin)
                {
                    LotExpediteDto obj = e.Row.DataItem as LotExpediteDto;
                    switch (obj.Status)
                    {
                        case StatusType.ACTIVED://if it has status ACTIVED then hide it
                            e.Row.Cells[9].Width = 200;
                            e.Row.Cells[9].CssClass = "non-col-active";
                            break;
                        case StatusType.PENDING:
                            e.Row.Cells[9].Width = 200;
                            bool check = BuRepository.CheckAllocationAndActual(obj.Bu.BuId);
                            if (!check)// (make-actived >= allocation)
                                e.Row.Cells[9].CssClass = "non-col-pending non-col-active";
                            else
                                e.Row.Cells[9].CssClass = "non-col-pending";
                            break;
                        case StatusType.CLOSED:
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[9].Width = 0;
                            break;
                    }
                }
                else
                {
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[9].Width = 0;
                }
            }
        }
        #endregion end Events

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public ILotExpediteRepository LotExpediteRepository
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public IBuRepository BuRepository { get; set; }
        #endregion
    }
}