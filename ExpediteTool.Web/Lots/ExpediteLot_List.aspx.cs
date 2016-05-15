using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.Concretes;
using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Utilities;
using ExpediteTool.Web.Infractructures;
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
    public partial class ExpediteLot_List : PageBase
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
                txtRequestor.Text = User.Identity.Name;
                //Bind data to gridview PENDING
                BindGridViewPending();
                //Bind data to gridview AVTIVED
                BindGridViewAvtived();
                //Bind data to grid BU
                GetTotalBUsWithActived();
            }
            btnDoneAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnDoneAdd, null) + ";");
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

        private LotExpediteDto GetLotFromUI()
        {
            var item = ModelBinder.BindModel<AddHotLotsDataViewModel>(this.panelPopup);
            LotExpediteDto hotLotData = new LotExpediteDto();

            string requestOutDate = dpkRequestOutDate.Value;
            if (!String.IsNullOrEmpty(requestOutDate))
                hotLotData.RequestOutDate = DateTime.ParseExact(requestOutDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            hotLotData.BUId = item.cboBu;
            hotLotData.CurrentOperation = item.txtCurrentOperation;
            hotLotData.Device = item.txtDevice;
            hotLotData.IsDeleted = false;
            hotLotData.LotId = item.txtLotId;
            hotLotData.Owner = item.txtOwner;
            hotLotData.Comment = item.txtComment;
            hotLotData.Platform = item.txtPlatform;
            hotLotData.Reason = item.txtReason;
            hotLotData.Requestor = item.txtRequestor;
            hotLotData.Status = StatusType.PENDING;

            return hotLotData;
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
                LotExpediteDto item = (LotExpediteDto)e.Row.DataItem;
                if (item != null && (item.Requestor != User.Identity.Name))
                {
                    e.Row.Cells[9].CssClass = "non-col-del";//hide column Delete
                }

                if (gv.HasAttributes)
                {
                    //Hide or show Hyperlink Make-Actived
                    int buid = int.Parse(gv.Attributes["data-buid"]);
                    bool check = BuRepository.CheckAllocationAndActual(buid);
                    if (!check)
                    {
                        if (item != null && (item.Requestor != User.Identity.Name))
                            e.Row.Cells[9].CssClass = "non-col-act non-col-del";//Hide hyperlink Make-Actived and hyperlink Delete

                        else
                            e.Row.Cells[9].CssClass = "non-col-act";//Hide column Make-Actived

                        e.Row.Cells[9].Width = 80;
                    }
                }
                int pageSize = gv.PageSize;
                int pageIndex = gv.PageIndex;
                int linePos = (pageSize * pageIndex) + (e.Row.RowIndex + 1);
                int count = ((List<ExpediteTool.Model.DataTransfer.LotExpediteDto>)gv.DataSource).Count;
                if (pageIndex == 0 && e.Row.RowIndex == 0 && linePos == count)
                {
                    e.Row.Cells[8].CssClass = "non-col-up non-col-down";
                }
                else
                {
                    if (pageIndex == 0 && e.Row.RowIndex == 0)
                        e.Row.Cells[8].CssClass = "non-col-up";
                    if (linePos == count)
                        e.Row.Cells[8].CssClass = "non-col-down";
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the grdChildActived control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdChildActived_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!ExpediteTool.Web.IdentityExtension.IsAdmin &&
                   !ExpediteTool.Web.IdentityExtension.IsContributor)//if is user UserNormal then execute
                {
                    LotExpediteDto item = (LotExpediteDto)e.Row.DataItem;
                    if (item.Requestor != User.Identity.Name)
                        e.Row.Cells[9].CssClass = "non-col-comment";//hide hyperlink UpdateComment
                    else
                        e.Row.Cells[9].CssClass = "show-col-comment";//show hyperlink UpdateComment
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
        /// Handles the Click event of the btnDesc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void btnPendingDesc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            GridView grdChildPending = (GridView)row.Parent.Parent;//Parent1: ChildTable->Parent2: GridView

            int index = row.RowIndex;
            int idSelf = (int)grdChildPending.DataKeys[index].Value;

            ActionResult resultSort = LotExpediteRepository.MoveSorting(idSelf, MoveType.DOWN);
            if (grdChildPending.Attributes["data-buid"] != null && resultSort == ActionResult.SUCCESS)
            {
                int buid = 0;
                int.TryParse(grdChildPending.Attributes["data-buid"], out buid);

                if (buid > 0)
                    BindGridViewChild(buid, StatusType.PENDING, grdChildPending);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAsc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void btnPendingAsc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            GridView grdChildPending = (GridView)row.Parent.Parent;//Parent1: ChildTable->Parent2: GridView

            int index = row.RowIndex;
            int idSelf = (int)grdChildPending.DataKeys[index].Value;

            ActionResult resultSort = LotExpediteRepository.MoveSorting(idSelf, MoveType.UP);
            if (grdChildPending.Attributes["data-buid"] != null && resultSort == ActionResult.SUCCESS)
            {
                int buid = 0;
                int.TryParse(grdChildPending.Attributes["data-buid"], out buid);
                if (buid > 0)
                    BindGridViewChild(buid, StatusType.PENDING, grdChildPending);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDoneAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDoneAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LotExpediteDto hotLotData = GetLotFromUI();

                int idHotLot = 0;
                int.TryParse(hdnHoLotId.Value, out idHotLot);
                ActionResult resultSave = ActionResult.FAIL;

                if (idHotLot > 0)
                {
                    hotLotData.ID = idHotLot;
                    //Update HotLot
                    resultSave = LotExpediteRepository.Update(hotLotData, User.Identity.Name);
                }
                else
                    //Add HotLot
                    resultSave = LotExpediteRepository.Add(hotLotData, User.Identity.Name);

                if (resultSave == ActionResult.SUCCESS)//save success
                {
                    //Update gridview Pending
                    BindGridViewPending();
                    //Message save sucsess
                    if (idHotLot > 0)
                        ScriptManager.RegisterStartupScript(this.panelPopup, GetType(), "updateDataSuccess", "updateDataSuccess()", true);
                    else
                        ScriptManager.RegisterStartupScript(this.panelPopup, GetType(), "addDataSuccess", "addDataSuccess()", true);
                }
                else//save fail
                {
                    //Message save fail
                    if (idHotLot > 0)
                        ScriptManager.RegisterStartupScript(this.panelPopup, GetType(), "updateDataFail", "updateDataFail()", true);
                    else
                        ScriptManager.RegisterStartupScript(this.panelPopup, GetType(), "addDataFail", "addDataFail()", true);
                }
            }
            catch
            {
                //Message save fail
                ScriptManager.RegisterStartupScript(this.panelPopup, GetType(), "addDataFail", "addDataFail()", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCloseStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnActivedCloseStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            ActionResult actionResult = UpdateStatus(id, StatusType.CLOSED);
            if (actionResult == ActionResult.SUCCESS)
            {
                //1.Update gridView
                GridView gvChild = (GridView)btn.Parent.Parent.Parent.Parent;
                if (gvChild != null)
                {
                    int buid = 0;
                    int.TryParse(gvChild.Attributes["data-buid"], out buid);
                    if (buid > 0)
                    {
                        BindGridViewChild(buid, StatusType.ACTIVED, gvChild);
                        UpdatePending(buid);
                    }
                }
                //update TotalBu
                GetTotalBUsWithActived();
            }
            else
            {
                //Message to user
            }
        }

        /// <summary>
        /// Handles the Click event of the btnActivedPendingStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnActivedPendingStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            ActionResult actionResult = UpdateStatus(id, StatusType.PENDING);
            if (actionResult == ActionResult.SUCCESS)
            {
                //1.Update gridView
                GridView gvChild = (GridView)btn.Parent.Parent.Parent.Parent;
                if (gvChild != null)
                {
                    int buid = 0;
                    int.TryParse(gvChild.Attributes["data-buid"], out buid);
                    if (buid > 0)
                    {
                        BindGridViewChild(buid, StatusType.ACTIVED, gvChild);
                        UpdatePending(buid);
                    }
                }
                //update TotalBu
                GetTotalBUsWithActived();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPendingMakeActive control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPendingMakeActive_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = 0;
            int.TryParse(btn.CommandArgument, out id);
            GridView gvChild = (GridView)btn.Parent.Parent.Parent.Parent;
            int buid = 0;
            int.TryParse(gvChild.Attributes["data-buid"], out buid);

            ActionResult actionResult = UpdateStatus(id, StatusType.ACTIVED);
            if (actionResult == ActionResult.SUCCESS)
            {
                if (buid > 0)
                {
                    BindGridViewChild(buid, StatusType.PENDING, gvChild);
                    UpdateActived(buid);
                }
                GetTotalBUsWithActived();
            }
            else
            {
                //Message to user
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPendingClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnPendingClose_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridView gvChild = (GridView)btn.Parent.Parent.Parent.Parent;
            int buid = 0;
            int.TryParse(gvChild.Attributes["data-buid"], out buid);

            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            ActionResult actionResult = UpdateStatus(id, StatusType.CLOSED);
            if (actionResult == ActionResult.SUCCESS)
            {
                if (buid > 0)
                    BindGridViewChild(buid, StatusType.PENDING, gvChild);
            }
            else
            {
                //Message to user
            }
        }

        /// <summary>
        /// Change request: 03_Aug_2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPendingDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridView gvChild = (GridView)btn.Parent.Parent.Parent.Parent;
            int buid = 0;
            int.TryParse(gvChild.Attributes["data-buid"], out buid);

            int id = 0;
            int.TryParse(btn.CommandArgument, out id);

            ActionResult actionResult = LotExpediteRepository.Delete(id, User.Identity.Name);
            if (actionResult == ActionResult.SUCCESS)
            {
                if (buid > 0)
                    BindGridViewChild(buid, StatusType.PENDING, gvChild);
            }
            else
            {
                //Message to user
            }
        }

        /// <summary>
        /// Handles the Click event of the btnExportExcel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string pathRoot = Server.MapPath("~");
                LogService.Info("Start Excel");
                ExportExcel export = new ExportExcel(pathRoot, LogService);
                LogService.Info("Step1");
                export.HotLotsList = LotExpediteRepository.FindAll();
                LogService.Info("Step2");
                export.BuList = BuRepository.FindAll().Select(x => x.BuName).ToArray();
                LogService.Info("Step3");
                export.CreateHeader();
                LogService.Info("Step4");
                export.CreateTitle();
                LogService.Info("Step5");
                export.CreateContent();
                LogService.Info("Step6");
                string filePath = export.SaveFile();
                LogService.Info("Step7");

                ScriptManager.RegisterClientScriptBlock(updatePanelMain, typeof(string), "getfile", "getfile('" + filePath + "');", true);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "closeWaitingDialog", "closeWaitingDialog();", true);
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

        /// <summary>
        /// Handles the Click event of the btnSearchAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSearchAll_Click(object sender, EventArgs e)
        {
            string textSearch = String.Format("LotSearchResult.aspx?search={0}&type={1}", txtSearchAll.Value, "1");
            Response.Redirect(textSearch, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmitUpdateComment_Click(object sender, EventArgs e)
        {
            int hotLotId = 0;
            int.TryParse(hdfHotLotDataId.Value, out hotLotId);
            if (hotLotId == 0)
                return;

            string comment = txtUpdateComment.Text;
            int BuID = 0;
            //Update comment
            ActionResult result = LotExpediteRepository.UpdateComment(id: hotLotId, comment: comment, userName: User.Identity.Name, BuID: out BuID);

            if (result == ActionResult.SUCCESS) //if successfuly then update gridview
            {
                ControlCollection controlCollection = (ControlCollection)grdActiveLots.Controls[0].Controls;
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
                                if (BuID.ToString().Equals(buIDActived))
                                {
                                    BindGridViewChild(BuID, StatusType.ACTIVED, gvChildActived);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else //else then message fail for user
            {

            }

        }

        /// <summary>
        /// BTNs the download file template.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnDownloadFileTemplate(object sender, EventArgs e)
        {
            // **************************************************
            string strFileName =
                string.Format("{0}.xlsm", "LotData_Template");

            string strRootRelativePathName =
                string.Format("~/Templates/{0}", strFileName);

            string strPathName =
                Server.MapPath(strRootRelativePathName);

            if (System.IO.File.Exists(strPathName) == false)
            {
                return;
            }
            // **************************************************

            System.IO.Stream oStream = null;

            try
            {
                // Open the file
                oStream =
                    new System.IO.FileStream
                        (path: strPathName,
                        mode: System.IO.FileMode.Open,
                        share: System.IO.FileShare.Read,
                        access: System.IO.FileAccess.Read);

                // **************************************************
                Response.Buffer = false;

                // Setting the unknown [ContentType]
                // will display the saving dialog for the user
                Response.ContentType = "application/octet-stream";

                // With setting the file name,
                // in the saving dialog, user will see
                // the [strFileName] name instead of [download]!
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);

                long lngFileLength = oStream.Length;

                // Notify user (client) the total file length
                Response.AddHeader("Content-Length", lngFileLength.ToString());
                // **************************************************

                // Total bytes that should be read
                long lngDataToRead = lngFileLength;

                // Read the bytes of file
                while (lngDataToRead > 0)
                {
                    // The below code is just for testing! So we commented it!
                    //System.Threading.Thread.Sleep(200);

                    // Verify that the client is connected or not?
                    if (Response.IsClientConnected)
                    {
                        // 8KB
                        int intBufferSize = 8 * 1024;

                        // Create buffer for reading [intBufferSize] bytes from file
                        byte[] bytBuffers =
                            new System.Byte[intBufferSize];

                        // Read the data and put it in the buffer.
                        int intTheBytesThatReallyHasBeenReadFromTheStream =
                            oStream.Read(buffer: bytBuffers, offset: 0, count: intBufferSize);

                        // Write the data from buffer to the current output stream.
                        Response.OutputStream.Write
                            (buffer: bytBuffers, offset: 0,
                            count: intTheBytesThatReallyHasBeenReadFromTheStream);

                        // Flush (Send) the data to output
                        // (Don't buffer in server's RAM!)
                        Response.Flush();

                        lngDataToRead = lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream;
                    }
                    else
                    {
                        // Prevent infinite loop if user disconnected!
                        lngDataToRead = -1;
                    }
                }
            }
            catch { }
            finally
            {
                if (oStream != null)
                {
                    //Close the file.
                    oStream.Close();
                    oStream.Dispose();
                    oStream = null;
                }
                Response.Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strRootRelativePathName = string.Format("~/Templates");
            string strPathName = Server.MapPath(strRootRelativePathName);
            string savedFileName = Path.Combine(strPathName, Guid.NewGuid().ToString() + ".xlsm");
            BulkLoader bulkLoader = null;
            IEnumerable<LotExpediteDto> duplicateLot = null;
            SaveBulkType saveBulk = SaveBulkType.FAILURE;
            ActionResult result = ActionResult.FAIL;
            try
            {
                if (fileUpload.HasFile)
                {
                    HttpPostedFile file = fileUpload.PostedFile;
                    file.SaveAs(savedFileName); // Save the file

                    bulkLoader = new BulkLoader(savedFileName);
                    if (bulkLoader.Lots != null && bulkLoader.Lots.Count > 0)
                    {
                        //Get  all Lot in DB
                        var allItems = LotExpediteRepository.FindAll();
                        if (allItems != null)
                        {
                            duplicateLot = allItems.Where(x => bulkLoader.Lots.Any(y => y.LotId == x.LotId));
                            if (duplicateLot != null && duplicateLot.Count() > 0)
                            {
                                saveBulk = SaveBulkType.DUPLICATE;
                            }
                            else
                            {
                                result = LotExpediteRepository.AddBulk(bulkLoader.Lots, User.Identity.Name);
                                if (result == ActionResult.SUCCESS)
                                    saveBulk = SaveBulkType.SUCCESS;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bulkLoader = null;
                LogService.Error(ex.Message, ex);
                saveBulk = SaveBulkType.FAILURE;
            }
            finally
            {
                System.IO.File.Delete(savedFileName);
            }
            if (bulkLoader != null && bulkLoader.Lots.Count == 0)
            {
                //Case: no record found
                ScriptManager.RegisterStartupScript(this, GetType(), "closeImportDialog", "closeImportDialog(0,0);", true);
            }
            else
            {
                switch (saveBulk)
                {
                    case SaveBulkType.SUCCESS:
                        BindGridViewPending();
                        int countRecordInserted = bulkLoader.Lots.Count;
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeImportDialog", "closeImportDialog(0," + countRecordInserted + ");", true);
                        break;
                    case SaveBulkType.DUPLICATE:
                        string lotId = "";
                        foreach (var item in duplicateLot)
                        {
                            if (String.IsNullOrEmpty(lotId))
                            {
                                lotId += item.LotId;
                            }
                            else
                            {
                                lotId += ", " + item.LotId;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeImportDialog", "closeImportDialog(2," + "'" + lotId + "'" + ");", true);
                        break;
                    case SaveBulkType.FAILURE:
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeImportDialog", "closeImportDialog(1,0);", true);
                        break;
                }
            }
        }
        #endregion
    }
}