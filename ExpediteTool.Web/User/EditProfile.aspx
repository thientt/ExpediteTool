<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="EditProfile.aspx.cs" Inherits="ExpediteTool.Web.EditProfile"
    Async="true" Title="Edit Profile-ExpediteTool" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <script>
        $(document).ready(function () {
            $('#txtFirstName').focus();

            $("input[type='text']").change(function () {
                var value = $(this).val();
                value = removeHtmlSpecialChars(value);
                $(this).val(value);
            });
        });

        function updateSuccess() {
            $.toaster({
                priority: 'success',
                title: 'Success',
                timer:4000,
                message: 'Your profile has been successfully updated.'
            });
        }

        function updateFail() {
            $.toaster({
                priority: 'danger',
                title: 'Fail',
                timer:7000,
                message: 'Your profile update fail'
            });
        }
    </script>
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
    <script src="../Scripts/main.js"></script>
    <h2 class="col-lg-offset-1 col-md-offset-1 col-sm-offset-1">Edit Profile Users</h2>
    <hr />
    <asp:UpdatePanel runat="server" ID="panelMain" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-1 col-md-1 col-sm-1 col-xs-1"></div>
                <div class="col-lg-6 col-md-6 col-sm-8 col-xs-10">
                    <div class="form-horizontal">
                        <asp:PlaceHolder ID="hdnID" runat="server" Visible="false">
                            <asp:TextBox runat="server" ID="txtUserId" ClientIDMode="Static" />
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <label class="col-lg-3 col-md-3 col-sm-3 control-label">First Name</label>
                            <div class="col-lg-9 col-md-9 col-sm-9">
                                <asp:TextBox runat="server" ID="txtFirstName" ClientIDMode="Static" CssClass="form-control input-sm" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-3 col-md-3 col-sm-3 control-label">Last Name</label>
                            <div class="col-lg-9 col-md-9 col-sm-9">
                                <asp:TextBox runat="server" ID="txtLastName" ClientIDMode="Static" CssClass="form-control input-sm" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-3 col-md-3 col-sm-3 control-label">Email</label>
                            <div class="col-lg-9 col-md-9 col-sm-9">
                                <asp:TextBox runat="server" TextMode="Email" ID="txtEmail" ClientIDMode="Static" CssClass="form-control input-sm" />
                            </div>
                        </div>
                        <asp:ValidationSummary runat="server" CssClass="text-danger col-md-offset-3" />
                        <div class="form-group">
                            <div class="col-lg-offset-3 col-md-offset-3 col-sm-offset-3 col-lg-9 col-md-9 col-sm-3">
                                <asp:Button runat="server" ID="btnSubmit" ClientIDMode="Static" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-3 col-xs-1"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
