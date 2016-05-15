<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs" Inherits="ExpediteTool.Web.RecoverPassword" MasterPageFile="~/Site.Master" Title="RecoverPassword" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
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
    <h2 class="col-lg-offset-1 hidden-xs">Recover Password</h2>
    <hr />
    <asp:UpdatePanel runat="server" ID="panelMain" ClientIDMode="Static">
        <ContentTemplate>
            <asp:FormView runat="server" ID="formRecoverPassword" ClientIDMode="Static"
                ItemType="ExpediteTool.Web.Models.RecoverPasswordViewModel" Width="100%"
                DefaultMode="Insert" InsertMethod="Recover">
                <InsertItemTemplate>
                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-1 col-xs-1"></div>
                        <div class="col-lg-6 col-md-6 col-sm-10 col-xs-10">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-3 col-lg-3 control-label">Email</label>
                                    <div class="col-md-9 col-lg-9">
                                        <asp:TextBox runat="server" ID="txtEmail" ClientIDMode="Static" ViewStateMode="Enabled"
                                            Text="<%# BindItem.Email %>" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-9">
                                        <asp:Button runat="server" Text="Recover" OnClientClick="waitingDialog();" CommandName="Insert" CssClass="btn btn-primary" />
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
            $('#txtEmail').focus();
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
            $("#pnWaitingProcess").dialog('option', 'title', 'Password Recovery...');
            $("#pnWaitingProcess").dialog('open');
        }
        function closeWaitingDialog() {
            $("#pnWaitingProcess").dialog('close');
        }
        function recoverSuccess() {
            $.toaster({
                priority: 'success',
                title: 'Success',
                message: 'Recover acocunt success, Please check email.'
            });
        }
        function recoverFail() {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                message: 'Recover password fail, Please retry recover.'
            });
        }
    </script>
</asp:Content>
