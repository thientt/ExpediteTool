<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpediteLot_List.aspx.cs" Title="ExpediteLot-ExpediteTool"
    MasterPageFile="~/Site.Master" Async="true" Inherits="ExpediteTool.Web.Lots.ExpediteLot_List" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server" ID="contentMain" ClientIDMode="Static">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <link href="../Contents/expedite.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
    <script src="../Scripts/main.js"></script>
    <script src="../Scripts/expedite.js" type="text/javascript"></script>
    <%-- <script>
        reloadPage = function () {
            window.location.href = window.location.href;
        };
    </script>--%>
    <style>
        #hlClick:hover {
            cursor: pointer;
        }
    </style>

    <div class="row">
        <div class="form-horizontal">
            <div class="col-sm-12">
                <a href="#" data-toggle="modal" data-target="#popupImportExcel">Import Lots</a>
                <br />
                Click   
                <asp:LinkButton Text="here" runat="server" OnClick="btnDownloadFileTemplate"></asp:LinkButton>
                to download template file
            </div>
        </div>
    </div>

    <asp:UpdatePanel runat="server" ID="updatePanelMain" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <table class="pull-right">
                    <tr>
                        <td class="col-lg-9 col-md-9">
                            <asp:Panel runat="server" DefaultButton="btnSearchAll" class="pull-right" ID="panelSearch">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="has-feedback">
                                                <input type="text" runat="server" class="form-control" placeholder="enter keyword to search..." id="txtSearchAll" />
                                                <span class="glyphicon glyphicon-search form-control-feedback" aria-hidden="true"></span>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Button type="button" runat="server" ID="btnSearchAll" ClientIDMode="Static" Style="display: none" OnClick="btnSearchAll_Click" CssClass="btn btn-success" Text="Search" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <input type="button" class="btn btn-primary pull-right" value="Add HotLots" onclick="showPopup();" />

                            <asp:Button ID="btnExportExcelFirst" runat="server" ClientIDMode="Static"
                                CssClass="btn btn-success pull-right" OnClick="btnExportExcel_Click" OnClientClick="waitingDialog();"
                                Style="margin-right: 10px;" Text="Export to Excel" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
            </div>

            <!-- List BU-->
            <div class="row">
                <div class="col-lg-5 col-md-5 col-sm-7 col-xs-12">
                    <asp:UpdatePanel runat="server" ID="updatePanelTotalBu">
                        <ContentTemplate>
                            <asp:GridView runat="server" AllowPaging="true"
                                ShowHeaderWhenEmpty="true" ShowFooter="true"
                                ID="grdTotalBu" ClientIDMode="Static" AutoGenerateColumns="false"
                                ItemType="ExpediteTool.Model.DataTransfer.TotalBuDto"
                                OnRowDataBound="grdTotalBu_RowDataBound"
                                OnPageIndexChanging="grdTotalBu_PageIndexChanging"
                                CssClass="table table-hover table-bordered max-height">
                                <Columns>
                                    <asp:TemplateField HeaderText="BU's" HeaderStyle-CssClass="input-sm">
                                        <ItemTemplate>
                                            <span class="input-sm"><%#: Item.BUName %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Allocation (Lots)" HeaderStyle-CssClass="text-center input-sm">
                                        <ItemTemplate>
                                            <span class="text-center center-block input-sm"><%#: Item.LotAllocation %> </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual (Lots)" HeaderStyle-CssClass="text-center input-sm">
                                        <ItemTemplate>
                                            <span class="text-center center-block input-sm"><%#: Item.Actual %> </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pagination-ys" />
                                <FooterStyle CssClass="text-right" />
                                <EmptyDataTemplate>
                                    No Current Lots 
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <br />
            <!-- List Status Active and Pending -->
            <div class="row">
                <div class="col-lg-12">
                    <div role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li role="presentation" class="active text-uppercase" id="tabActivate"><a href="#statusActive" aria-controls="statusActive" role="tab" data-toggle="tab">Active</a></li>
                            <li role="presentation" id="tabPending" class="text-uppercase"><a href="#statusPending" aria-controls="statusPending" role="tab" data-toggle="tab">Pending</a></li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <!-- Tab Active -->
                            <div role="tabpanel" class="tab-pane active" id="statusActive">
                                <asp:UpdatePanel runat="server" ID="updatePanelActiveLots">
                                    <ContentTemplate>
                                        <div class="margin-tab">
                                            <asp:GridView runat="server" ID="grdActiveLots" ClientIDMode="Static" Width="100%"
                                                ItemType="ExpediteTool.Model.DataTransfer.BuDto"
                                                DataKeyNames="BUId" AutoGenerateColumns="false"
                                                OnRowDataBound="grdActiveLots_RowDataBound"
                                                ShowHeaderWhenEmpty="true" ShowHeader="false" GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgActivedShow" runat="server" OnClick="btnShowHideActivedGrid" Width="20" Height="20"
                                                                ImageUrl="~/Images/minus.png" CommandArgument="Hide" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="40">
                                                        <ItemTemplate>
                                                            <label class="text-danger text-uppercase"><%#: Item.BuId %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <label class="text-info text-uppercase"><%#: Item.BuName %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="10">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Panel runat="server" ID="panelChildActived" ClientIDMode="Static" Visible="false">
                                                                                <asp:GridView ID="grdChildActived" runat="server" DataKeyNames="ID"
                                                                                    ItemType="ExpediteTool.Model.DataTransfer.LotExpediteDto"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                                    data-buid="0"
                                                                                    OnRowDataBound="grdChildActived_RowDataBound"
                                                                                    CssClass="table table-bordered table-hover" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="LOT ID" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.LotId %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DEVICE" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Device %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REASON" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Reason %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REQUESTOR" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#:Item.Requestor %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="OWNER" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Owner %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REQUEST OUT DATE" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.RequestOutDate.ToString("MM/dd/yyyy") %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="PED" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.ScmEndDate.HasValue ? Item.ScmEndDate.Value.ToString("MM/dd/yyyy"): "" %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="COMMENT" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Comment %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="PLATFORM" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Platform %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="CURRENT OPERATION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.CurrentOperation %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-Width="170" HeaderText="LOT ACTION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <% if (ExpediteTool.Web.IdentityExtension.IsAdmin ||
                                                                                                       ExpediteTool.Web.IdentityExtension.IsContributor)
                                                                                                   {  %>
                                                                                                <asp:Button runat="server" ID="btnCloseStatus" type="button" CssClass="btn btn-link" Text="Close"
                                                                                                    CommandArgument="<%#: Item.ID %>" OnClick="btnActivedCloseStatus_Click" OnClientClick="return confirmCloseStatus();" />
                                                                                                <asp:Button runat="server" ID="btnPendingStatus" type="button" CssClass="btn btn-link" Text="Pending"
                                                                                                    CommandArgument="<%#: Item.ID %>" OnClick="btnActivedPendingStatus_Click" OnClientClick="return confirmPendingStatus();" />
                                                                                                <% } %>
                                                                                                <input id="btnUpdateComment" type="button" class="btn btn-link" value="Update Comment"
                                                                                                    data-target="#modalUpdateComment" data-toggle="modal" data-backdrop="false"
                                                                                                    onclick='<%# "setModalUpdateComment(" + "\"" + Item.ID + "\"" + "," + "\""+  Item.Comment + "\"" + ");" %>' />

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                                    <EmptyDataTemplate>
                                                                                        No Current Lots
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <!-- Tab Pending -->
                            <div role="tabpanel" class="tab-pane" id="statusPending">
                                <asp:UpdatePanel runat="server" ID="updatePanelParentPending">
                                    <ContentTemplate>
                                        <div class="margin-tab">
                                            <asp:GridView ID="grdParentPending" runat="server" DataKeyNames="BuId" Width="100%"
                                                ItemType="ExpediteTool.Model.DataTransfer.BuDto" ShowHeader="false"
                                                OnRowDataBound="grdParentPending_RowDataBound"
                                                AutoGenerateColumns="false" GridLines="None">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgPendingShow" runat="server" OnClick="btnShowHidePendingGrid" Width="20" Height="20"
                                                                ImageUrl="~/Images/minus.png" CommandArgument="Hide" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="40">
                                                        <ItemTemplate>
                                                            <label class="text-danger text-uppercase"><%#: Item.BuId %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <label class="text-info text-uppercase"><%#: Item.BuName %></label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="10">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Panel runat="server" ID="panelChildPending" ClientIDMode="Static" Visible="false">
                                                                                <asp:GridView ID="grdChildPending" runat="server" DataKeyNames="ID"
                                                                                    ItemType="ExpediteTool.Model.DataTransfer.LotExpediteDto"
                                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                                                    data-buid="0" Width="100%"
                                                                                    OnRowDataBound="grdChildPending_RowDataBound"
                                                                                    CssClass="table table-bordered table-hover">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="LOT ID" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.LotId %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DEVICE">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Device %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REASON" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Reason %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REQUESTOR" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Requestor %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="OWNER" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Owner %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="REQUEST OUT DATE" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#:Item.RequestOutDate.ToString("MM/dd/yyyy") %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="COMMENT" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Comment %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="PLATFORM" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.Platform %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="CURRENT OPERATION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <%#: Item.CurrentOperation %>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Sorting" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <div style="align-content: center" class="col-sort">
                                                                                                    <asp:ImageButton runat="server" ID="btnAsc" ClientIDMode="Static"
                                                                                                        OnClick="btnPendingAsc_Click" ToolTip="Move Up"
                                                                                                        CssClass="img-circle img-responsive" EnableTheming="true" ImageUrl="~/Images/up.png"
                                                                                                        Width="32px" Height="32px" />
                                                                                                    <asp:ImageButton runat="server" ID="btnDesc" ClientIDMode="Static"
                                                                                                        OnClick="btnPendingDesc_Click" ToolTip="Move Down"
                                                                                                        CssClass="img-circle img-responsive" EnableTheming="true" ImageUrl="~/Images/down.png"
                                                                                                        Width="32px" Height="32px" />
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-Width="190" HeaderText="LOT ACTION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <% if (ExpediteTool.Web.IdentityExtension.IsAdmin ||
                                                                                                       ExpediteTool.Web.IdentityExtension.IsContributor)
                                                                                                   {  %>
                                                                                                <asp:Button runat="server" ID="btnPendingMakeActive" ClientIDMode="Static"
                                                                                                    OnClick="btnPendingMakeActive_Click" CommandArgument="<%#: Item.ID %>"
                                                                                                    CssClass="btn btn-link" Text="Make Active" OnClientClick="return confirmActiveStatus();" />
                                                                                                <asp:Button runat="server" ID="btnPendingClose" ClientIDMode="Static"
                                                                                                    OnClick="btnPendingClose_Click" CommandArgument="<%#: Item.ID %>"
                                                                                                    CssClass="btn btn-link" Text="Close" OnClientClick="return confirmCloseStatus();" />
                                                                                                <%} %>
                                                                                                <input id="btnEdit" type="button" class="btn btn-link" value="Edit"
                                                                                                    onclick='<%# "return bindDataPopup(" + "\"" + Item.ID + "\"" + "," + "\""+ Item.LotId + "\"" + "," +"\""+ Item.Reason+ "\""+ "," + "\"" +Item.RequestOutDate.ToString("MM/dd/yyyy") + "\"" + "," + "\""+ Item.Bu.BuId + "\"" + "," + "\""+ Item.Owner +"\""+ "," +"\""+ Item.Comment+ "\"" +"," + "\""+ Item.Platform + "\""+ ","+ "\""+ Item.CurrentOperation + "\""+ ","+"\""+ Item.Device+"\"" + ");"%>' />
                                                                                                <asp:Button runat="server" ID="btnDelete" ClientIDMode="Static"
                                                                                                    CommandArgument="<%#: Item.ID %>" OnClick="btnPendingDelete_Click"
                                                                                                    CssClass="btn btn-link" Text="Delete" OnClientClick="return confirmDelete();" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <PagerStyle CssClass="pagination-ys" />
                                                                                    <EmptyDataTemplate>
                                                                                        No Current Lots
                                                                                    </EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <br />
            <!-- Add HotLots -->
            <% if (ExpediteTool.Web.IdentityExtension.IsAdmin || ExpediteTool.Web.IdentityExtension.IsContributor)
               { %>
            <input type="button" class="btn btn-primary pull-right" value="Add HotLots" onclick="showPopup();" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnExportExcelLast" ClientIDMode="Static" Text="Export to Excel" OnClientClick="waitingDialog();"
                CssClass="btn btn-success pull-right" OnClick="btnExportExcel_Click" Style="margin-right: 10px;" />
            <%} %>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Popup Add HotLots-->
    <asp:Panel runat="server" ID="panelPopup">
        <div class="modal fade" id="popupAddData">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Add HotLots</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:HiddenField runat="server" ID="hdnHoLotId" ClientIDMode="Static" />
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">LotId</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtLotId" ClientIDMode="Static"
                                        TabIndex="1"
                                        CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Reason</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox type="text" runat="server" ID="txtReason" ClientIDMode="Static"
                                        TabIndex="2" CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Request Out Date</label>
                                <div class="col-lg-8 col-md-8 col-sm-8 ui-datepicker-group">
                                    <input runat="server" type="text" id="dpkRequestOutDate" readonly="true" maxlength="10" tabindex="3"
                                        class="ui-datepicker-buttonpane form-control input-sm col-lg-12 col-md-12 col-sm-12 col-xs-12 max-width-calendar" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">BU's</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:DropDownList runat="server" ID="cboBu" ClientIDMode="Static" TabIndex="4"
                                        DataTextField="BUName" SelectMethod="GetAllBu"
                                        DataValueField="BUId" AutoPostBack="false"
                                        CssClass="form-control input-sm max-width">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Owner</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtOwner" ClientIDMode="Static"
                                        TabIndex="5" CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Comment</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtComment" ClientIDMode="Static"
                                        TabIndex="6" CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Platform</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPlatform" ClientIDMode="Static"
                                        TabIndex="7" CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Current Operation</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server"
                                        ID="txtCurrentOperation" ClientIDMode="Static"
                                        TabIndex="8" CssClass="form-control input-sm max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Device</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtDevice" ClientIDMode="Static"
                                        TabIndex="9" CssClass="form-control input-sm  max-width" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-4 col-md-4 col-sm-4">Requestor</label>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:TextBox runat="server" ID="txtRequestor" ClientIDMode="Static"
                                        TabIndex="10" CssClass="form-control input-sm max-width" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true" tabindex="11" value="Close" onclick="resetControlPopup()" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ClientIDMode="Static" ID="btnDoneAdd" TabIndex="10"
                            Text="Done Adding Lot" OnClientClick="if(!checkSave()) {return false;}" OnClick="btnDoneAdd_Click" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!--Popup Update comment -->
    <div class="modal fade" role="dialog" id="modalUpdateComment" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Update Comment</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:HiddenField runat="server" ID="hdfHotLotDataId" ClientIDMode="Static" />
                            <label class="control-label col-lg-3 col-md-3 col-sm-4">Comment</label>
                            <div class="col-lg-9 col-md-9 col-sm-8">
                                <asp:TextBox runat="server" ID="txtUpdateComment" ClientIDMode="Static" CssClass="form-control input-sm max-width" />
                            </div>
                            <label class="col-lg-offset-3 col-md-offset-3 col-sm-offset-4" style="padding-left: 15px;" id="validateLengthComment" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" UseSubmitBehavior="false" type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true" TabIndex="1" Text="Close" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSubmitUpdateComment" ClientIDMode="Static" TabIndex="0" Text="Submit"
                        OnClientClick='return checkLength("txtUpdateComment");' OnClick="btnSubmitUpdateComment_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Popup Import excel-->
    <asp:Panel runat="server" ID="panelImportExcel">
        <div class="modal fade in" id="popupImportExcel" tabindex="-1" role="dialog" aria-labelledby="modalTitle">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title" id="modalTitle">Import Lots</h3>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server">
                            <asp:FileUpload ID="fileUpload" ClientIDMode="Static" name="fileUpload" runat="server" CssClass="form-control btn-success" Style="margin-bottom: 5px;" />
                            <asp:Button runat="server" ID="btnUpload" ClientIDMode="Static" Text="Upload" OnClick="btnUpload_Click" OnClientClick="watingImportDialog();"
                                CssClass="btn btn-success" />
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="modal fade in" id="modalPopupSuccess" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="modal-title">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2>Info import</h2>
                    </div>
                </div>
                <div class="modal-body">
                    <h4 class="text-info"><span id="txtNumberRecord"></span>&nbsp;Lots imported.
                    </h4>
                </div>
                <div class="modal-footer">
                    <button data-dismiss="modal" aria-label="Close" class="btn btn-default">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalPopupDuplicate" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="modal-title">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2>Info import</h2>
                    </div>
                </div>
                <div class="modal-body">
                    <h4 class="text-info">Import failed. Lots with ID <span id="txtDuplicationLot" class="text-danger"></span>&nbsp;are existed in system and no Lot imported.</h4>
                </div>
                <div class="modal-footer">
                    <button data-dismiss="modal" aria-label="Close" class="btn btn-default">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade in" id="modalPopupFailure" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="modal-title">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2>Info import</h2>
                    </div>
                </div>
                <div class="modal-body">
                    <h4 class="text-info">Import failed.</h4>
                </div>
                <div class="modal-footer">
                    <button data-dismiss="modal" aria-label="Close" class="btn btn-default">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="pnWaitingProcess" />
</asp:Content>
