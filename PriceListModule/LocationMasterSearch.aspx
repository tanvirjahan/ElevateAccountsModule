<%@ Page Title="" Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="LocationMasterSearch.aspx.vb" Inherits="PriceListModule_LocationMasterSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
  <script language="javascript" type="text/javascript">

    function CallWebMethod(methodType)
    {
        switch(methodType) {
                   
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
        }
    }

    function FillCityCodes(result) {
        var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {

            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillCityNames(result) {
        var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

</script>
<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
<tr>
            <td style="width: 917px">
                <table>
                 <tr>
                     <td class="field_heading" style="width: 896px; " align="center">
                Location Master List</td>
                 </tr>
                  <tr>
                        <td class="td_cell" style="width: 896px; color: blue; " align="center">
                Type few characters of code or name and click search 
                        </td>
                    </tr>
                      <tr>
                        <td>
                        <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 828px"><TBODY><TR><TD align=center colSpan=6>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" Checked="True" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp; 
 <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton>&nbsp;
 <asp:Button id="btnSearch" tabIndex=1 runat="server" Text="Search" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=2 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;
        <asp:Button id="btnhelp" tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;
    <asp:Button id="btnAddNew" tabIndex=4 runat="server" Text="Add New" 
        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp; 
    <asp:Button id="btnPrint" tabIndex=5 runat="server" Text="Report" 
        CssClass="btn"></asp:Button></TD></TR>

        <TR>
        <TD style="WIDTH: 80px"><asp:Label id="Label2" runat="server" Text="Area Code" ForeColor="Black" Width="84px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 100px"><asp:TextBox id="TxtAreaCode" tabIndex=6 runat="server" 
                Width="212px" CssClass="txtbox" MaxLength="20"></asp:TextBox></TD>
        <TD style="WIDTH: 79px"><asp:Label id="Label1" runat="server" Text="Area Name" ForeColor="Black" Width="84px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 100px"><asp:TextBox id="TxtAreaName" tabIndex=7 runat="server" 
                Width="300px"></asp:TextBox></TD>
        <TD style="WIDTH: 178px"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD>
        <TD style="WIDTH: 178px"><asp:DropDownList id="ddlOrderBy" runat="server" 
                Width="104px" CssClass="drpdown" AutoPostBack="True" 
                OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" TabIndex="8"></asp:DropDownList></TD>
        </TR>

        <TR>
        <TD style="WIDTH: 80px"><asp:Label id="lblCityCode" runat="server" Text="City Code  " ForeColor="Black" Width="48px" CssClass="field_caption" Visible="False"></asp:Label>
        </TD>
        <TD style="WIDTH: 100px">
            <SELECT style="WIDTH: 219px" id="ddlCityCode" class="drpdown" tabIndex=13 
                onchange="CallWebMethod('citycode');" runat="server" visible="false" 
                onserverchange="ddlCityCode_ServerChange"> <OPTION selected></OPTION></SELECT> </TD>
                <TD style="WIDTH: 79px"><asp:Label id="lblCityName" runat="server" Text="City Name" ForeColor="Black" Width="60px" CssClass="field_caption" Visible="False"></asp:Label></TD>
                <TD style="WIDTH: 100px">
            <SELECT style="WIDTH: 306px" id="ddlCityName" class="drpdown" tabIndex=14 
                onchange="CallWebMethod('cityname');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT>
                 </TD><TD style="WIDTH: 178px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 178px"></TD></TR>
     

      </TBODY></TABLE>
</contenttemplate>                
                </asp:UpdatePanel></td>                        
                    </tr>
                     <tr>
                        <td>
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="15"
                                            Text="Export To Excel" />
                                        </td>
                    </tr>

<tr>
<td style="width: 896px">
<asp:UpdatePanel id="UpdatePanel2" runat="server">
<contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=16 runat="server" Font-Size="10px" Width="902px" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Area Code"><EditItemTemplate>
<asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("areacode") %>'></asp:TextBox>
                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblAreaCode" runat="server" Text='<%# Bind("areacode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="areacode" SortExpression="areacode" HeaderText="Area Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="areaname" SortExpression="areaname" HeaderText="Area Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField HtmlEncode="False" DataField="citycode" SortExpression="citycode" HeaderText="City Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="cityname" SortExpression="cityname" HeaderText="City Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>
<asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                </table>
            </td>
</tr>

</table>

<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
<Services>
<asp:ServiceReference Path="~/clsServices.asmx" />
</Services>
</asp:ScriptManagerProxy>

</asp:Content>

