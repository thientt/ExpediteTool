using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.Concretes;
using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Utilities;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebForm.ModelBinder;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web.Lots
{
    public partial class ExpediteLot_List_HOME : PageBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Bind data to gridview PENDING
                BindGridViewPending();
                //Bind data to gridview AVTIVED
                BindGridViewAvtived();
                //Bind data to grid BU
                GetTotalBUsWithActived();
            }
        }

        #region Func
        /// <summary>
        /// Gets the data bu.
        /// </summary>
        private void GetTotalBUsWithActived()
        {
            var items = BuRepository.GetTotalBuWithActived();
            totalActual = items.Sum(x => x.Actual);
            totalAllocation = items.Sum(x => x.LotAllocation);
            grdTotalBu.PageSize = ExpediteTool.Web.Constants.ConfigManager.PAGE_SIZE;
            grdTotalBu.PagerSettings.PageButtonCount = ExpediteTool.Web.Constants.ConfigManager.PAGE_BUTTON_COUNT;
            grdTotalBu.DataSource = items;
            grdTotalBu.DataBind();
        }

        // This method is used to bind gridview from database
        /// <summary>
        /// Binds the gridview.
        /// </summary>
        private void BindGridViewPending()
        {
            var items = BuRepository.FindAll();
            grdParentPending.DataSource = items;
            grdParentPending.DataBind();
        }

        /// <summary>
        /// Binds the grid view avtived.
        /// </summary>
        private void BindGridViewAvtived()
        {
            var items = BuRepository.FindAll();
            grdActiveLots.DataSource = items;
            grdActiveLots.DataBind();
        }

        /// <summary>
        /// Binds the grid view child.
        /// </summary>
        /// <param name="buId">The bu identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="gvChild">The gv child.</param>
        private void BindGridViewChild(int buId, StatusType status, GridView gvChild)
        {
            gvChild.Attributes["data-buid"] = buId.ToString();
            gvChild.DataSource = LotExpediteRepository.GetHotLotDataByBuAndStatus(buId, status);
            gvChild.DataBind();
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

        /// <summary>
        /// Updates the pending.
        /// </summary>
        /// <param name="buid">The buid.</param>
        private void UpdatePending(int buid)
        {
            GridView grdParentPending = (GridView)this.updatePanelParentPending.FindControl("grdParentPending");
            //this a panel only one control
            ControlCollection webControl = (ControlCollection)grdParentPending.Controls[0].Controls;
            foreach (Control item in webControl)
            {
                if (item is GridViewRow)
                {
                    var temp = (GridViewRow)item;
                    if (temp.RowType == DataControlRowType.DataRow)
                    {
                        GridView gvChildPending = (GridView)temp.FindControl("grdChildPending");
                        if (gvChildPending.HasAttributes)
                        {
                            string buIDPending = gvChildPending.Attributes["data-buid"];
                            if (buid.ToString().Equals(buIDPending))
                            {
                                BindGridViewChild(buid, StatusType.PENDING, gvChildPending);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the actived.
        /// </summary>
        /// <param name="buid">The buid.</param>
        private void UpdateActived(int buid)
        {
            GridView grdParentActived = (GridView)this.updatePanelActiveLots.FindControl("grdActiveLots");
            //this a panel only one control
            ControlCollection controlCollection = (ControlCollection)grdParentActived.Controls[0].Controls;
            foreach (Control item in controlCollection)
            {
                if (item is GridViewRow)
                {
                    var temp = (GridViewRow)item;
                    if (temp.RowType == DataControlRowType.DataRow)
                    {
                        GridView gvChildActived = (GridView)temp.FindControl("grdChildActived");
                        if (gvChildActived.HasAttributes)
                        {
                            string buIDActived = gvChildActived.Attributes["data-buid"];
                            if (buid.ToString().Equals(buIDActived))
                            {
                                BindGridViewChild(buid, StatusType.ACTIVED, gvChildActived);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all bu.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BuDto> GetAllBu()
        {
            return BuRepository.FindAll();
        }

        /// <summary>
        /// Gets all user.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UsersInfoDto> GetAllUser()
        {
            return UserRepository.FindAll();
        }
        #endregion

        #region Properties
        [Inject]
        public ILotExpediteRepository LotExpediteRepository
        {
            get;
            set;
        }

        [Inject]
        public IBuRepository BuRepository { get; set; }

        [Inject]
        public IUserRepository UserRepository { get; set; }

        [Inject]
        public ILogService LogService { get; set; }
        #endregion

        #region Event
        private int totalAllocation = 0;
        private int totalActual = 0;

        /// <summary>
        /// Handles the RowDataBound event of the grdTotalBu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdTotalBu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;

                e.Row.Cells[1].Text = totalAllocation.ToString();
                e.Row.Cells[1].Font.Bold = true;

                e.Row.Cells[2].Text = totalActual.ToString();
                e.Row.Cells[2].Font.Bold = true;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the grdTotalBu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void grdTotalBu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTotalBu.PageIndex = e.NewPageIndex;
            GetTotalBUsWithActived();
        }

        /// <summary>
        /// Handles the RowDataBound event of the grdChildPending control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdChildPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gv = (GridView)e.Row.Parent.Parent;
                if (gv.HasAttributes)
                {
                    //Hide or show Hyperlink Make-Actived
                    int buid = int.Parse(gv.Attributes["data-buid"]);
                }
            }
        }

        /// <summary>
        /// BTNs the show hide pending grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnShowHidePendingGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                Control control = row.FindControl("panelChildPending");
                if (control != null)
                    control.Visible = true;

                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/Images/minus.png";
                int buid = 0;
                int.TryParse(grdParentPending.DataKeys[row.RowIndex].Value.ToString(), out buid);
                GridView gvChildPending = row.FindControl("grdChildPending") as GridView;
                if (buid > 0 && gvChildPending != null)
                    BindGridViewChild(buid, StatusType.PENDING, gvChildPending);
            }
            else
            {
                Control control = row.FindControl("panelChildPending");
                if (control != null)
                    control.Visible = false;

                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/Images/plus.png";
            }
        }

        /// <summary>
        /// BTNs the show hide actived grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnShowHideActivedGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                Control control = row.FindControl("panelChildActived");
                if (control != null)
                    control.Visible = true;

                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/Images/minus.png";
                int buid = 0;
                int.TryParse(grdParentPending.DataKeys[row.RowIndex].Value.ToString(), out buid);
                GridView gvChildActived = row.FindControl("grdChildActived") as GridView;
                if (buid > 0 && gvChildActived != null)
                    BindGridViewChild(buid, StatusType.ACTIVED, gvChildActived);
            }
            else
            {
                Control control = row.FindControl("panelChildActived");
                if (control != null)
                    control.Visible = false;

                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/Images/plus.png";
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the grdParentPending control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdParentPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Control control = e.Row.FindControl("panelChildPending");
                if (control != null)
                    control.Visible = true;

                int buid = 0;
                int.TryParse(grdParentPending.DataKeys[e.Row.RowIndex].Value.ToString(), out buid);
                GridView gvChildPending = e.Row.FindControl("grdChildPending") as GridView;
                if (buid > 0 && gvChildPending != null)
                    BindGridViewChild(buid, StatusType.PENDING, gvChildPending);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the grdActiveLots control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdActiveLots_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Control control = e.Row.FindControl("panelChildActived");
                if (control != null)
                    control.Visible = true;

                int buid = 0;
                int.TryParse(grdActiveLots.DataKeys[e.Row.RowIndex].Value.ToString(), out buid);
                GridView gvChildActived = e.Row.FindControl("grdChildActived") as GridView;
                if (buid > 0 && gvChildActived != null)
                    BindGridViewChild(buid, StatusType.ACTIVED, gvChildActived);
            }
        }
        #endregion
    }
}