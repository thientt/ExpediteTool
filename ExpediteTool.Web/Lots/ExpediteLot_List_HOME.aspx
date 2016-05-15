<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpediteLot_List_HOME.aspx.cs" Title="Home-ExpediteTool"
    MasterPageFile="~/Site.Master" Async="true" Inherits="ExpediteTool.Web.Lots.ExpediteLot_List_HOME" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server" ID="contentMain" ClientIDMode="Static">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script src="../Scripts/jquery.toaster.js"></script>
    <script src="../Scripts/main.js"></script>
    <style>
        table#grdTotalBu tbody tr td {
            padding: 0px !important;
            font-size: 18px;
        }

            table#grdTotalBu tbody tr td span {
                font-size: 13px;
            }

        /* hide the close x on the loading screen */
        .loadingScreenWindow .ui-dialog-titlebar-close {
            display: none;
        }
    </style>

    <asp:UpdatePanel runat="server" ID="updatePanelMain" UpdateMode="Conditional">
        <ContentTemplate>
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
                                            <span class="input-sm"><%#: Item.BUName %>
                                            </span>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
