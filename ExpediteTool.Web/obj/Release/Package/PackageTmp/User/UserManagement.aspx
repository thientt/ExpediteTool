<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" 
    Inherits="ExpediteTool.Web.Account.UserManagement"
    MasterPageFile="~/Site.Master" Async="true" Title="User Management-ExpediteTool" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>

    <style>
        table tbody th a.sortasc {
            display: block;
            padding: 0 4px 0 15px;
            background: #554477;
        }

        table tbody th a.sortdesc {
            display: block;
            padding: 0 4px 0 15px;
            background: #590034;
        }
    </style>

    <asp:UpdatePanel runat="server" ID="updatePanelUserManagement">
        <ContentTemplate>
            <asp:GridView runat="server" ID="grdListUserInfo" ClientIDMode="Static" Width="100%"
                AllowPaging="true" AllowSorting="true" ShowHeaderWhenEmpty="true"
                AutoGenerateColumns="false" CssClass="table table-bordered table-hover"
                CurrentSortField="UserName" CurrentSortDirection="ASC"
                OnPageIndexChanging="grdUserInfo_PageIndexChanging"
                OnSorting="grdUserInfo_Sorting"
                OnRowDataBound="grdUserInfo_DataBound"
                OnRowCreated="grdListUserInfo_RowCreated"
                ItemType="ExpediteTool.Model.DataTransfer.UsersInfoDto">
                <Columns>
                    <asp:TemplateField HeaderText="UserID" HeaderStyle-CssClass="text-center text-uppercase"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Item.UserId %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="User name" HeaderStyle-CssClass="text-uppercase text-center" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="UserName" DataField="UserName" />
                    <asp:TemplateField HeaderText="name" HeaderStyle-CssClass="text-uppercase text-center" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#: Item.FullName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-CssClass="text-center text-uppercase" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#: Item.Email %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role" HeaderStyle-CssClass="text-center text-uppercase"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#: Item.Role.RoleName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Status" HeaderStyle-CssClass="text-uppercase text-center" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="Status" DataField="Status" />
                    <asp:BoundField HeaderText="Registration Date"
                        HeaderStyle-CssClass="text-uppercase text-center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                        SortExpression="RegistrationDate" DataField="RegistrationDate" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:TemplateField HeaderText="LastLogin"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-uppercase text-center">
                        <ItemTemplate>
                            <%#: Item.LastLogin.ToString("MM/dd/yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="290">
                        <ItemTemplate>
                            <input type="button" id="btnEdit"
                                class="btn-link" value="Edit" data-target="#modalPopEditProfile" data-toggle="modal" data-backdrop="false"
                                onclick='<%# "setValue(" + "\"" + Item.UserId + "\"" + "," + "\""+  Item.FullName + "\"" + "," + Item.RoleId + "," + "\""+ Item.Status+ "\"" + ");" %>' />
                            <asp:Button runat="server" ID="btnChangeStatus" ClientIDMode="Static"
                                OnClick="btnChangeStatus_Click" CommandArgument='<%#: Item.UserId %>'
                                OnClientClick='<%# "return confirmChangeStatus("+"\"" + Item.UserName.ToUpper() + "\"" + "," + "\""+  Item.ShowStatus.ToUpper() + "\""+ ")" %>'
                                CssClass="btn-link" Text='<%#: Item.ShowStatus%>' />
                           <%-- <asp:Button runat="server" ID="btnChangePassword" ClientIDMode="Static"
                                CssClass="btn btn-link" Text="Reset Password"
                                OnClick="btnChangePassword_Click" CommandArgument='<%#: Item.UserId %>'
                                OnClientClick='<%# "return confirmChangePassword("+ "\"" + Item.UserName.ToUpper() + "\""+ ");" %>' />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Current Lots
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" role="dialog" id="modalPopEditProfile">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Edit Profile</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1"></div>
                        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-10">
                            <div class="form-horizontal">
                                <asp:HiddenField runat="server" ID="txtUserId" ClientIDMode="Static" />
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 col-sm-4 control-label">Full Name</label>
                                    <div class="col-lg-8 col-md-8 col-sm-8">
                                        <asp:TextBox runat="server" ID="txtFullName" ClientIDMode="Static" ReadOnly="true" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 col-sm-4 control-label">Role</label>
                                    <div class="col-lg-8 col-md-8 col-sm-8">
                                        <asp:DropDownList runat="server" ID="cboRole" ClientIDMode="Static"
                                            DataTextField="RoleName" SelectMethod="GetRoleUser"
                                            DataValueField="RoleId" AutoPostBack="false"
                                            CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 col-sm-4 control-label">Status</label>
                                    <div class="col-lg-8 col-md-8 col-sm-8">
                                        <asp:DropDownList runat="server" ID="cboStatus" ClientIDMode="Static"
                                            DataValueField="UserStatusId" DataTextField="UserStatusName"
                                            AutoPostBack="false" SelectMethod="GetUserStatus"
                                            CssClass="form-control input-sm">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <asp:ValidationSummary runat="server" CssClass="text-danger col-md-offset-3" />
                                <div class="form-group">
                                    <div class="col-lg-offset-4 col-md-offset-4 col-sm-offset-4 col-lg-8 col-md-8 col-sm-8">
                                        <asp:Button runat="server" ID="btnSubmit" ClientIDMode="Static" Text="Submit"
                                            CssClass="btn btn-primary" OnClick="btnSubmit_Click" OnClientClick="hidePopupEditProfile();" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-3 col-xs-1"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $('#modalPopEditProfile').on('shown.bs.modal', function () {
                $('#txtFullName').focus();
            });
        });

        function confirmChangePassword(user) {
            return confirm('Are you sure reset password default of user ' + user + "?");
        }

        function confirmChangeStatus(user, status) {
            return confirm('Are you sure change status of user ' + user + ' to ' + status + "?");
        }

        function showPopupEditProfile(fname, indexStatus, indexRole) {
            $('#modalPopEditProfile').modal('show');
            $('#txtFullName'); val(fname);
            $('#cboStatus').val(indexStatus);
            $('#cboRole').val(indexRole);
        }

        function hidePopupEditProfile() {
            $('#modalPopEditProfile').modal('hide');
        }

        function updateStatusFail() {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                message: 'Update status of user fail',
                time: 10000,
            });
        }

        function updateStatusSuccess() {
            $.toaster({
                priority: 'success',
                title: 'Success',
                message: 'Update status of user success',
                time: 10000,
            });
        }

        function resetPassFail(user) {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                message: 'Reset password of ' + user + ' fail',
                time: 10000,
            });
        }

        function resetPassSuccess(user) {
            $.toaster({
                priority: 'success',
                title: 'Success',
                message: 'Password of ' + user + '  changed ',
                time: 10000,
            });
        }

        function setValue(userId, fullName, roleId, status) {
            $('#txtUserId').val(userId);
            $('#txtFullName').val(fullName);
            $('#cboRole').val(roleId);
            if (status == 'Activated')
                $('#cboStatus').val(0);
            if (status == 'De_activated')
                $('#cboStatus').val(1);
            if (status == 'Locked')
                $('#cboStatus').val(2);
        }
    </script>
</asp:Content>
