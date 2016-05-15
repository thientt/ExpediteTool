<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" 
    MasterPageFile="~/Site.Master" Inherits="ExpediteTool.Web.Login" Debug="true" Title="Login-ExpediteTool" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.js"></script>
    <style>
        .panel-group-margin {
            margin-bottom: -10px;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#txtUserName').focus();
        });
    </script>
    <asp:UpdatePanel runat="server" ID="panelLogin" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="panel panel-primary" style="max-width: 455px;">
                        <div class="panel-group panel-group-margin">
                            <div class="panel-heading">
                                <div class="panel-title">
                                    <h3><strong><b>Login</b></strong></h3>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">User Name </label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-user" />
                                                </div>
                                                <asp:TextBox runat="server" ID="txtUserName" ClientIDMode="Static" CssClass="form-control text-left" placeholder="Enter user name" />
                                            </div>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName" CssClass="text-danger" ErrorMessage="The user name field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Password </label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-eye-open"></span>
                                                </div>
                                                <asp:TextBox TextMode="Password" runat="server" CssClass="form-control" ID="txtPassword" ClientIDMode="Static" placeholder="Enter password" />
                                            </div>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                                        </div>
                                    </div>
                                    <br />
                                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="FailureText" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="col-lg-offset-4 col-md-offset-4 input-group col-md-8">
                                        <asp:Button ID="btnLogin" CssClass="btn btn-primary pull-right"
                                            runat="server" Text="Log in" OnClick="btnLogin_Click" />
                                    </div>
                                    <p>
                                        <asp:HyperLink runat="server" NavigateUrl="~/User/Register.aspx" ID="HyperLink1" ClientIDMode="Static" ViewStateMode="Enabled">Register</asp:HyperLink>&nbsp;&nbsp;|&nbsp;
                                        <asp:HyperLink runat="server" NavigateUrl="~/User/RecoverPassword.aspx" ID="hplRecoverPassword" ClientIDMode="Static" ViewStateMode="Enabled">Forgot Password?</asp:HyperLink>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
