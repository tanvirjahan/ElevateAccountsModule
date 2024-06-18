<%@ Page Title="" Language="VB" MasterPageFile="~/UserAdminMaster.master" AutoEventWireup="false" CodeFile="PasswordChangelog.aspx.vb" Inherits="UserAdminModule_PasswordChangelog" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<table style="width: 936px">
        <tr>
            <td class="field_heading" style="text-align: center">
             Password Change Log</td>
        </tr>
        <tr>
            <td style="width: 100px">
                <table class="td_cell" style="width: 936px">
                   
                     <tr>
                        <td style="width: 67px">
                            User code</td>
                        <td style="width: 55px">
                            <asp:TextBox ID="txtUsercode" runat="server" CssClass="txtbox"></asp:TextBox></td>
                             <td style="width: 100px">
                            User Name</td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtUserName" runat="server" Width="304px" CssClass="txtbox"></asp:TextBox></td>

                       
                    </tr>
                     <tr>
                        <td style="width: 67px">
                            From Date</td>
                        <td style="width: 55px">
                             <ews:DatePicker ID="dpFromDate" runat="server" DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="23" 
                                    Width="200px" /></td>
                             <td style="width: 100px">
                            To Date</td>
                        <td style="width: 100px">
                             <ews:DatePicker ID="dpToDate" runat="server" DateFormatString="dd/MM/yyyy" 
                                    RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" tabIndex="23" 
                                    Width="200px" />
                            </td>

                       
                    </tr>

                  
                    <tr>
                        <td style="width: 67px">
                        </td>
                        <td style="width: 55px">
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 250px">
                            <asp:Button ID="btnSearch" runat="server" CssClass="search_button" Font-Bold="False"
                                TabIndex="3" Text="Search" Width="48px" />&nbsp;<asp:Button ID="btnClear" runat="server"
                                    CssClass="search_button" Font-Bold="False" TabIndex="4" Text="Clear" Width="39px" />&nbsp;
                                    <asp:Button
                                        ID="btnHelp" runat="server" CssClass="search_button" 
                                        TabIndex="8" Text="Help" Width="35px" />&nbsp;
                <asp:Button ID="btnAddNew" runat="server" CssClass="btn" Font-Bold="True"
                    TabIndex="5" Text="Add New"   Visible="False" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnExportToExcel"
                        runat="server" CssClass="btn" TabIndex="6" Text="Export To Excel" 
                    Visible="False" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="btn" TabIndex="7"
                            Text="Report" Visible="False" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
        <asp:GridView id="gv_SearchResult" tabIndex="8" runat="server" Font-Size="10px" CssClass="grdstyle" Width="950px" 
        GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
         AllowSorting="True" AllowPaging="True">
        <FooterStyle CssClass="grdfooter"></FooterStyle>
          <Columns>         
            
            <asp:BoundField DataField="UserCode" SortExpression="UserCode" HeaderText="User Code"></asp:BoundField>
            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User Name"></asp:BoundField>
            <asp:BoundField DataField="adddate" SortExpression="adddate" HeaderText="Date"></asp:BoundField>
           
          </Columns>

        <RowStyle CssClass="grdRowstyle"></RowStyle>

        <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

        <HeaderStyle CssClass="grdheader" HorizontalAlign ="Left"  ForeColor="White" ></HeaderStyle>

        <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
        <EmptyDataTemplate >
        Records not found, Please redefine search criteria
        </EmptyDataTemplate>
        </asp:GridView>  
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>

</asp:Content>

