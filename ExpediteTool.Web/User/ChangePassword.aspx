<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    MasterPageFile="~/Site.Master" Inherits="ExpediteTool.Web.ChangePassword"
    Async="true" Title="Reset password-ExpediteTool" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
    <%--<h2 class="col-lg-offset-2 col-md-offset-1 hidden-xs">Enter your new password</h2>--%>
    <h2 class="col-lg-offset-2 col-md-offset-1">Change Password</h2>
    <hr />
    <asp:FormView runat="server" ID="formResetPassword" ClientIDMode="Static"
        InsertMethod="UserChangePassword" DefaultMode="Insert" Width="100%"
        ItemType="ExpediteTool.Web.Models.ChangePasswordViewModel">
        <InsertItemTemplate>
            <div class="row">
                <div class="col-lg-offset-2 col-lg-6 col-md-offset-1 col-md-8 col-sm-offset-1 col-sm-9 col-xs-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-md-4">Old Password</label>
                            <div class="col-md-8">
                                <input type="password" runat="server" class="form-control input-sm" id="txtPasswordOld" value="<%# BindItem.OldPassword %>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-4">New Password</label>
                            <div class="col-md-8">
                                <input type="password" runat="server" class="form-control input-sm" id="txtPasswordNew" value="<%# BindItem.NewPassword %>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-4">Confirm new Password</label>
                            <div class="col-md-8">
                                <input type="password" runat="server" class="form-control input-sm" id="txtPasswordConfirm" value="<%# BindItem.PasswordConfirm %>" />
                            </div>
                        </div>
                        <asp:ValidationSummary CssClass="text-danger col-md-offset-4 col-lg-offset-4" runat="server" />
                        <div class="form-group">
                            <div class="col-md-offset-4 col-md-8">
                                <asp:Button runat="server" ID="btnChangePassword"
                                    ClientIDMode="Static" Text="Submit" CssClass="btn btn-primary" CommandName="Insert" />
                            </div>
                        </div>
                    </div>
                </div>
        </InsertItemTemplate>
    </asp:FormView>
  
      <script>
        $(document).ready(function () {
            $('#txtPasswordOld').focus();
        });

        function registerSuccess() {
            $.toaster({
                priority: 'success',
                title: 'Success',
                message: 'Change passsword success'
            });
        }

        function registerFail() {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                message: 'Change password fail, Please retry'
            });
        }
    </script>
</asp:Content>
