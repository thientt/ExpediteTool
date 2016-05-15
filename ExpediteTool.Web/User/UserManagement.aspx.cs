using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Web.Constants;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web.Account
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserManagement : PageBase
    {
        #region Func Event
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsRoleAdmin(UserRepository))
            {
                string url = Request.QueryString["ReturnUrl"];
                if (!String.IsNullOrEmpty(url))
                    Response.Redirect(url, true);
                else
                    Response.Redirect("~/Lots/ExpediteLot_List.aspx");
            }
            if (!IsPostBack)
            {
                grdListUserInfo.DataSource = GetUserInfo();
                grdListUserInfo.PageSize = ConfigManager.PAGE_SIZE;
                grdListUserInfo.PagerSettings.PageButtonCount = ConfigManager.PAGE_BUTTON_COUNT;
                grdListUserInfo.DataBind();
            }
            btnSubmit.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the grdUserInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void grdUserInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdListUserInfo.PageIndex = e.NewPageIndex;
            GetUserInfo();
        }

        /// <summary>
        /// Handles the DataBound event of the grdUserInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdUserInfo_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var item = (UsersInfoDto)e.Row.DataItem;
                switch (item.Status)
                {
                    case UserStatus.Activated:
                        e.Row.CssClass = "text-success";
                        break;
                    case UserStatus.De_activated:
                        e.Row.CssClass = "text-primary";
                        break;
                    case UserStatus.Locked:
                        e.Row.CssClass = "text-danger";
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Sorting event of the grdUserInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void grdUserInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection sortDirec = SortDirection.Ascending;
            string sortField = String.Empty;
            SortGridView(grdListUserInfo, e, out sortDirec, out sortField);
            switch (e.SortExpression)
            {
                case "UserName":
                    if (sortDirec == SortDirection.Ascending)
                        grdListUserInfo.DataSource = GetUserInfo().OrderBy(x => x.UserName).ToList();
                    else
                        grdListUserInfo.DataSource = GetUserInfo().OrderByDescending(x => x.UserName).ToList();
                    break;
                case "Status":
                    if (sortDirec == SortDirection.Ascending)
                        grdListUserInfo.DataSource = GetUserInfo().OrderBy(x => x.Status).ToList();
                    else
                        grdListUserInfo.DataSource = GetUserInfo().OrderByDescending(x => x.Status).ToList();
                    break;
                case "RegistrationDate":
                    if (sortDirec == SortDirection.Ascending)
                        grdListUserInfo.DataSource = GetUserInfo().OrderBy(x => x.RegistrationDate).ToList();
                    else
                        grdListUserInfo.DataSource = GetUserInfo().OrderByDescending(x => x.RegistrationDate).ToList();
                    break;
            }
            grdListUserInfo.DataBind();
        }

        /// <summary>
        /// Handles the RowCreated event of the grdListUserInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdListUserInfo_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (grdListUserInfo.Attributes["CurrentSortField"] != null && grdListUserInfo.Attributes["CurrentSortDirection"] != null)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (cell.HasControls())
                        {
                            LinkButton sortLinkButton = null;
                            if (cell.Controls[0] is LinkButton)
                            {
                                sortLinkButton = (LinkButton)cell.Controls[0];
                                if (sortLinkButton != null && grdListUserInfo.Attributes["CurrentSortField"] == sortLinkButton.CommandArgument)
                                {
                                    // Create the sorting image based on the sort direction.
                                    Image sortImage = new Image();
                                    if (grdListUserInfo.Attributes["CurrentSortDirection"] == "DESC")
                                    {
                                        sortImage.ImageUrl = "~/Images/arrowup.gif";
                                        sortImage.AlternateText = "Ascending Order";
                                    }
                                    else
                                    {
                                        sortImage.ImageUrl = "~/Images/arrowdown.gif";
                                        sortImage.AlternateText = "Descending Order";
                                    }

                                    cell.Controls.Add(new LiteralControl("&nbsp;"));
                                    cell.Controls.Add(sortImage);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int idUser = 0;
            int.TryParse(txtUserId.Value, out idUser);
            var result = UserRepository.AdminUpdate(idUser, (byte)cboStatus.SelectedIndex, (byte)cboRole.SelectedIndex + 1);
            if (result == Model.ActionResult.SUCCESS)
            {
                grdListUserInfo.DataSource = GetUserInfo();
                grdListUserInfo.DataBind();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnChangeStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            Button btnChange = sender as Button;
            int userId = 0;
            int.TryParse(btnChange.CommandArgument, out userId);
            byte status = 0;
            switch (btnChange.Text)
            {
                case "Activate":
                    status = 0;
                    break;
                case "De-Activate":
                    status = 1;
                    break;
                case "Lock":
                    status = 2;
                    break;
            }
            ActionResult result = UserRepository.AdminUpdate(userId, status, 0);
            if (result != ActionResult.SUCCESS)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "updateStatusFail", "updateStatusFail();", true);
            }
            else
            {
                grdListUserInfo.DataSource = GetUserInfo();
                grdListUserInfo.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "updateStatusSuccess", "updateStatusSuccess();", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Button btnChange = sender as Button;
            int userId = 0;
            int.TryParse(btnChange.CommandArgument, out userId);
            string userNameResetPassworded = String.Empty;
            string passDefault = Constants.ConfigManager.PASS_DEFAULT;
            ActionResult result = UserRepository.ResetPassword(userId, passDefault, out userNameResetPassworded);
            if (result != ActionResult.SUCCESS)
                ScriptManager.RegisterStartupScript(this, GetType(), "resetPassFail", "resetPassFail(" + "\"" + userNameResetPassworded + "\"" + ");", true);
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "resetPassSuccess", "resetPassSuccess(" + "\"" + userNameResetPassworded + "\"" + ");", true);
        }
        #endregion

        #region Func User
        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UsersInfoDto> GetUserInfo()
        {
            IEnumerable<UsersInfoDto> usersInfos = UserRepository.FindAll();
            return usersInfos;
        }

        /// <summary>
        /// Gets the role user.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleUserDto> GetRoleUser()
        {
            return RoleRepository.FindAll();
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserStatusViewModel> GetUserStatus()
        {
            return new List<UserStatusViewModel>()
            {
                new UserStatusViewModel{UserStatusId =0, UserStatusName="Activated"},
                new UserStatusViewModel{UserStatusId =1, UserStatusName="De_activated"},
                new UserStatusViewModel{UserStatusId=2, UserStatusName="Locked"}
            };
        }

        /// <summary>
        /// Sorts the grid view.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="sortField">The sort field.</param>
        private void SortGridView(GridView gridView, GridViewSortEventArgs e, out SortDirection sortDirection, out string sortField)
        {
            sortField = e.SortExpression;
            sortDirection = e.SortDirection;

            if (gridView.Attributes["CurrentSortField"] != null && gridView.Attributes["CurrentSortDirection"] != null)
            {
                if (sortField == gridView.Attributes["CurrentSortField"])
                {
                    if (gridView.Attributes["CurrentSortDirection"] == "ASC")
                        sortDirection = SortDirection.Descending;
                    else
                        sortDirection = SortDirection.Ascending;
                }
                gridView.Attributes["CurrentSortField"] = sortField;
                gridView.Attributes["CurrentSortDirection"] = (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        /// <value>
        /// The user repository.
        /// </value>
        [Inject]
        public IUserRepository UserRepository
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the role repository.
        /// </summary>
        /// <value>
        /// The role repository.
        /// </value>
        [Inject]
        public IRoleRepository RoleRepository
        {
            get;
            set;
        }
        #endregion
    }
}