<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LotSearchResult.aspx.cs" 
    Title="Search result-ExpediteTool"
    MasterPageFile="~/Site.Master" Inherits="ExpediteTool.Web.Lots.LotSearchResult" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <style>
        td.non-col-active input[type=submit]:first-child {
            display: none;
        }

        .non-col-pending input[type=submit]:last-child {
            display: none;
        }
    </style>
    <script>
        function confirmCloseStatus() {
            return confirm('Are you sure to close lot?');
        }

        function confirmPendingStatus() {
            return confirm('Are you sure to pending this lot?');
        }

        function confirmActiveStatus() {
            return confirm('Are you sure to make active this lot?');
        }

    </script>
    <strong>Search Result for <b><i>
        <asp:Label runat="server" ID="txtSearchFor" ClientIDMode="Static" />
    </i></b></strong>
    <br />
    <asp:UpdatePanel runat="server" ID="updatePanelSearch" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">
                <asp:GridView runat="server" ID="grdSearchHotLots" ClientIDMode="Static"
                    ItemType="ExpediteTool.Model.DataTransfer.LotExpediteDto"
                    DataKeyNames="ID" AutoGenerateColumns="false"
                    OnPageIndexChanging="grdgrdSearchHotLots_PageIndexChanging"
                    OnRowDataBound="grdSearchHotLots_RowDataBound"
                    AllowPaging="true" Width="100%"
                    ShowHeaderWhenEmpty="true"
                    CssClass="table table-bordered table-hover">
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
                        <asp:TemplateField HeaderText="BU" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#: Item.Bu.BuName %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OWNER" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#: Item.Owner %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="REQUEST OUT DATE" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#: Item.RequestOutDate.ToString("MM/dd/yyyy")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="COMMENT" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#: Item.Comment %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CURRENT OPERATION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#: Item.CurrentOperation %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="200" HeaderText="LOT ACTION" HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnActiveStatus" type="button" CssClass="btn btn-link" Text="Make Active"
                                    CommandArgument="<%#: Item.ID %>" OnClick="btnActivedStatus_Click" OnClientClick="return confirmActiveStatus();" />
                                <asp:Button runat="server" ID="btnCloseStatus" type="button" CssClass="btn btn-link" Text="Close"
                                    CommandArgument="<%#: Item.ID %>" OnClick="btnCloseStatus_Click" OnClientClick="return confirmCloseStatus();" />
                                <asp:Button runat="server" ID="btnPendingStatus" type="button" CssClass="btn btn-link" Text="Pending"
                                    CommandArgument="<%#: Item.ID %>" OnClick="btnPendingStatus_Click" OnClientClick="return confirmPendingStatus();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                    <EmptyDataTemplate>
                        No Current Lots
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
