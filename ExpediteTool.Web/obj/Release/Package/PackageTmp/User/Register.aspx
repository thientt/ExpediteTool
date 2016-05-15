<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Title="Register-ExpediteTool"
    CodeBehind="Register.aspx.cs" Inherits="ExpediteTool.Web.Register" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <style>
        #pnWaitingProcess {
            display: block;
            background-image: url('../Images/ajax-loader.gif');
            background-position: center center;
            background-repeat: no-repeat;
        }
        /* hide the close x on the loading screen */
        .loadingScreenWindow .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
    <h2 class="col-lg-offset-1 hidden-xs">Create a new account</h2>
    <h2 class="col-xs-offset-1 visible-xs">Register</h2>
    <hr />
    <asp:UpdatePanel runat="server" ID="panelMain" ClientIDMode="Static">
        <ContentTemplate>
            <asp:FormView runat="server" ID="formRegisterUser" ClientIDMode="Static"
                ItemType="ExpediteTool.Web.Models.RegisterViewModel" Width="100%"
                DefaultMode="Insert" InsertMethod="RegisterUser">
                <InsertItemTemplate>
                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-1 col-xs-1"></div>
                        <div class="col-lg-6 col-md-6 col-sm-10 col-xs-10">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-4 col-lg-4 control-label">User Name</label>
                                    <div class="col-md-8 col-lg-8">
                                        <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" ViewStateMode="Enabled"
                                            Text="<%# BindItem.UserName %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 control-label">Password</label>
                                    <div class="col-lg-8 col-md-8">
                                        <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" ClientIDMode="Static"
                                            Text="<%# BindItem.Password %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 control-label">Confirm password</label>
                                    <div class="col-lg-8 col-md-8">
                                        <asp:TextBox runat="server" TextMode="Password" ID="txtConfirmPassword" ClientIDMode="Static"
                                            Text="<%# BindItem.ConfirmPassword %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 control-label">First Name</label>
                                    <div class="col-lg-8 col-md-8">
                                        <asp:TextBox runat="server" ID="txtFirstName" ClientIDMode="Static"
                                            Text="<%# BindItem.Firstname %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 control-label">Last Name</label>
                                    <div class="col-lg-8 col-md-8">
                                        <asp:TextBox runat="server" ID="txtLastName" ClientIDMode="Static"
                                            Text="<%# BindItem.Lastname %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-4 col-md-4 control-label">Email</label>
                                    <div class="col-lg-8 col-md-8">
                                        <asp:TextBox runat="server" TextMode="Email" ID="txtEmail" ClientIDMode="Static"
                                            Text="<%# BindItem.Email %>" CssClass="form-control input-sm" />
                                    </div>
                                </div>
                                <asp:ValidationSummary runat="server" CssClass="text-danger col-lg-offset-4 col-md-offset-4" />
                                <div class="form-group">
                                    <div class="col-md-offset-4 col-md-8">
                                        <asp:Button runat="server" Text="Register" OnClientClick="waitingDialog();" CommandName="Insert" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-1 col-xs-1"></div>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
        </ContentTemplate>

    </asp:UpdatePanel>

    <div id="pnWaitingProcess" />

    <script>
        $(document).ready(function () {
            $('#txtUserName').focus();
            $('#pnWaitingProcess').dialog(
           {
               autoOpen: false,
               dialogClass: "loadingScreenWindow",
               closeOnEscape: false,
               draggable: false,
               width: 260,
               minHeight: 30,
               modal: true,
               buttons: {},
               resizable: false,
               open: function () {
                   // scrollbar fix for IE
                   $('body').css('overflow', 'hidden');
               },
               close: function () {
                   // reset overflow
                   $('body').css('overflow', 'auto');
               }
           });
        });

        function waitingDialog() {
            $("#pnWaitingProcess").dialog('option', 'title', 'Creating account...');
            $("#pnWaitingProcess").dialog('open');
        }
        function closeWaitingDialog() {
            $("#pnWaitingProcess").dialog('close');
        }
        function registerSuccess() {
            $.toaster({
                priority: 'success',
                title: 'Success',
                message: 'Register acocunt success, click <u><a href="Login.aspx">Login</a></u> the login'
            });
        }
        function registerFail() {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                message: 'Register acocunt fail, Please retry register'
            });
        }
    </script>
</asp:Content>
