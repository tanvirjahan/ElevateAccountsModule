<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"
    CodeFile="VisaCategorySearch.aspx.vb" Inherits="Visa_Category_Types_Search" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table>
        <tr>
            <td>
                <table id="TABLE1" style="border-right: gray 2px solid; border-top: gray 2px solid;
                    border-left: gray 2px solid; width: 918px; border-bottom: gray 2px solid;">
                    <tr>
                        <td class="td_cell" colspan="1">
                            <table>
                                <tr align="center">
                                    <td align="center" class="field_heading">
                                        <asp:Label runat="server" ID="Lblselltypes" Text="Visa Category List"></asp:Label>
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="td_cell" style="color: blue; text-align: center;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="text-align: center" class="td_cell" align="center" colspan="7">
                                                                &nbsp; &nbsp;&nbsp;
                                                                <asp:RadioButton ID="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell"
                                                                    __designer:wfdid="w51" Checked="True" GroupName="GrSearch" AutoPostBack="True">
                                                                </asp:RadioButton>&nbsp;
                                                                <asp:RadioButton ID="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black"
                                                                    CssClass="td_cell" __designer:wfdid="w52" GroupName="GrSearch" AutoPostBack="True">
                                                                </asp:RadioButton>&nbsp;&nbsp;
                                                                <asp:Button ID="btnSearch" TabIndex="5" runat="server" Text="Search" Font-Bold="False"
                                                                    CssClass="search_button" __designer:wfdid="w56"></asp:Button>&nbsp;
                                                                <asp:Button ID="btnClear" TabIndex="6" OnClick="btnClear_Click1" runat="server" Text="Clear"
                                                                    Font-Bold="False" CssClass="search_button" __designer:wfdid="w57"></asp:Button>&nbsp;
                                                                <asp:Button ID="btnHelp" TabIndex="10" OnClick="btnHelp_Click" runat="server" Text="Help"
                                                                    __designer:dtid="1688858450198528" CssClass="search_button" __designer:wfdid="w19">
                                                                </asp:Button>&nbsp;<asp:Button ID="btnAddNew" TabIndex="7" runat="server" Text="Add New"
                                                                    Font-Bold="False" __designer:dtid="6192449487634451" CssClass="btn" __designer:wfdid="w5">
                                                                </asp:Button>&nbsp;<asp:Button ID="btnPrint" TabIndex="9" runat="server" Text="Report"
                                                                    __designer:dtid="6192449487634453" CssClass="btn" __designer:wfdid="w6" Height="19px">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_cell" colspan="4">
                                                                <asp:Panel ID="PnlCustSect" runat="server" __designer:wfdid="w55">
                                                                    <table>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label1" runat="server" Text="Category Code" Width="77px" __designer:wfdid="w68"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCategoryCode" TabIndex="1" runat="server" Width="183px" CssClass="txtbox"
                                                                                        __designer:wfdid="w26" MaxLength="20"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label2" runat="server" Text="Category Name" Width="80px" __designer:wfdid="w68"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCategoryName" TabIndex="2" runat="server" Width="244px" CssClass="txtbox"
                                                                                        __designer:wfdid="w27"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"
                                                                                        __designer:wfdid="w7"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown"
                                                                                        __designer:wfdid="w8" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 1022px;" valign="bottom">
                                        &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="8"
                                            Text="Export To Excel" />
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gv_SearchResult" TabIndex="10" runat="server" Font-Size="10px"
                                        Width="902px" CssClass="grdstyle" __designer:wfdid="w61" GridLines="Vertical"
                                        CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False"
                                        AllowSorting="True" AllowPaging="True">
                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                        <Columns>
                                            <asp:TemplateField Visible="False" HeaderText="Category Code">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("visacategorycode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblothsellcode" runat="server" Text='<%# Bind("visacategorycode") %>'
                                                        __designer:wfdid="w1"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="visacategorycode" SortExpression="visacategorycode" HeaderText="Visa Category Code">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="visacategoryname" SortExpression="visacategoryname" HeaderText="Visa Category Name">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>                                            
                                            <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                                DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                                DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                            </asp:ButtonField>
                                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                            </asp:ButtonField>
                                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                            </asp:ButtonField>
                                        </Columns>
                                        <RowStyle CssClass="grdRowstyle"></RowStyle>
                                        <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                        <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                    </asp:GridView>
                                    <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                        Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="905px" __designer:wfdid="w62"
                                        Visible="False" CssClass="lblmsg"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="z-index: 100; left: 0px; position: absolute; top: 0px">
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
