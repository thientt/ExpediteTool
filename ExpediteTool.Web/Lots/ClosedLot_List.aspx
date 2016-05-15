<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClosedLot_List.aspx.cs" Title="ClosedLot-ExpediteTool"
    MasterPageFile="~/Site.Master" Inherits="ExpediteTool.Web.Lots.Closed_List" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <link href="../Contents/Site.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {

            var text = $('#txtSearchAll').val();
            if (jQuery.trim(text) != '') {
                $('#btnSearchAll').removeAttr('disabled');
            }
            else {
                $('#btnSearchAll').attr('disabled', 'disabled');
            }

            $('#txtSearchAll').keyup(function () {
                if ($(this).val() != '') {
                    $('#btnSearchAll').removeAttr('disabled');
                }
                else {
                    $('#btnSearchAll').attr('disabled', 'disabled');
                }
            });
        });
    </script>
    <div class="row">
        <table class="pull-right" style="margin-bottom: 5px;">
            <tr>
                <td>
                    <asp:Panel runat="server" ID="panelSearch" DefaultButton="btnSearchAll" class="pull-right">
                        <table>
                            <tr>
                                <td>
                                    <div class="has-feedback">
                                        <span class="glyphicon glyphicon-search form-control-feedback" aria-hidden="true"></span>
                                        <input type="text" runat="server" id="txtSearchAll" clientidmode="Static" placeholder="enter keyword to search..." class="form-control" />
                                    </div>
                                </td>
                                <td>
                                    <asp:Button type="button" runat="server" ID="btnSearchAll" Enabled="false" Style="display: none" ClientIDMode="Static" OnClick="btnSearchAll_Click" CssClass="btn btn-success" Text="Search" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatePanelClosedHotLots" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">
                <asp:GridView runat="server" ID="grdClosedHotLots" ClientIDMode="Static"
                    DataSourceID="objectDataSourceClosedHotLots"
                    ItemType="ExpediteTool.Model.DataTransfer.LotExpediteDto"
                    DataKeyNames="ID" AutoGenerateColumns="false"
                    AllowPaging="true" Width="100%"
                    ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="grdClosedHotLots_PageIndexChanging"
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
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                    <EmptyDataTemplate>
                        Data Empty
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource runat="server" ID="objectDataSourceClosedHotLots"
                    TypeName="ExpediteTool.Web.Lots.Closed_List"
                    SelectMethod="GetClosedHotLots"
                    SelectCountMethod="GetCountClosedHotLots"
                    StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="pageSize"
                    EnablePaging="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
