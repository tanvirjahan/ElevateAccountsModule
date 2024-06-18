<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="ProfitabilityDetails.aspx.vb" Inherits="AccountsModule_ProfitabilityDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:UpdatePanel ID="upnlMain" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td style="text-align: center" class=" field_input" colspan="5">
                        <asp:Label ID="lblHeading" runat="server" Text="Profitability Details" ForeColor="White"
                            Width="100%" CssClass="field_heading"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:GridView ID="gv_SearchResult" TabIndex="10" runat="server" Font-Size="10px"
                            Width="100%" CssClass="grdstyle" __designer:wfdid="w50" GridLines="Vertical"
                            CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False"
                            AllowSorting="True" AllowPaging="True" PageSize="50">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("acc_tran_id") %>'></asp:Label>     
                                        <asp:Label ID="lblreqid" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>     
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField headerText="Market">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmarket" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>                                             
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:BoundField DataField="acc_tran_id" HeaderText="Invoice No">
                                    <ItemStyle HorizontalAlign="Left" Font-Size="12px"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="acc_tran_date" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <ItemStyle HorizontalAlign="Left" Font-Size="12px"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="requestid" HeaderText="File No">
                                    <ItemStyle HorizontalAlign="Left" Font-Size="12px"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="agentname" HeaderText="Customer Name">
                                    <ItemStyle HorizontalAlign="left" Font-Size="12px" />
                                    <HeaderStyle HorizontalAlign="Center" Font-Size="12px"></HeaderStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sale Value">
                                    <HeaderStyle HorizontalAlign="Right" Font-Size="12px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Font-Size="12px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" CssClass="field_input" Font-Size="12px" Text='<%# Bind("credit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Value">
                                    <HeaderStyle HorizontalAlign="Right" Font-Size="12px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Font-Size="12px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" CssClass="field_input" Font-Size="12px" Text='<%# Bind("debit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Profit">
                                    <HeaderStyle HorizontalAlign="Right" Font-Size="12px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Font-Size="12px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProfit" runat="server" Font-Size="12px" CssClass="field_input"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Percentage">
                                    <HeaderStyle HorizontalAlign="Right" Font-Size="12px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Font-Size="12px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPercentage" runat="server" Font-Size="12px" CssClass="field_input"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPLreport" runat="server" Font-Size="12px" Text="P&L" CommandName="plreport" 
                                        CommandArgument ='<%# Ctype(Container,GridViewRow).RowIndex %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="width: 525px">
                        Total:                         
                    </td>
                    <td>
                        <asp:TextBox ID="txtSaleVale" runat="server" CssClass="fiel_input" 
                            Enabled="false" Width="75px" style="text-align:right"></asp:TextBox>&nbsp;<asp:TextBox ID="txtCostValue" style="text-align:right" runat="server" CssClass="fiel_input" Enabled="false" Width="75px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtProfit" style="text-align:right" runat="server" CssClass="fiel_input" Enabled="false" Width="75px"></asp:TextBox>&nbsp;<asp:TextBox ID="txtPercentage" style="text-align:right" runat="server" CssClass="fiel_input" Enabled="false" Width="75px"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td style="text-align: center" class=" field_input" colspan="5">
                        <asp:Button ID="btnPrint" runat="server" Text="Load Report" CssClass="field_button" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
