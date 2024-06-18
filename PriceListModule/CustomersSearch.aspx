<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="CustomersSearch.aspx.vb" Inherits="CustomersSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script type="text/javascript">
<!--
// WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
<script src="../Content/js/Custom.js" type="text/javascript"></script>
<link type ="text/css" href ="../Content/css/JqueryUI.css" rel ="Stylesheet" />
<script language="javascript" type="text/javascript" >
       
   function CallWebMethod(methodType)
    {
        switch(methodType)
        {
            case "ctrycode":
                var select=document.getElementById("<%=ddlCountryCode  .ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryName   .ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                break;
                  break;
            case "ctryname":
                var select=document.getElementById("<%=ddlCountryName .ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountryCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCityCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
            case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var selectname=document.getElementById("<%=ddlCityCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
          case "salepcode":
                var select=document.getElementById("<%=ddlSalesPerson.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPersonName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "salepname":
                var select=document.getElementById("<%=ddlSalesPersonName.ClientID%>");                
                var selectname=document.getElementById("<%=ddlSalesPerson.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;                
        }
    }

    function FillCityCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

    function FillCityNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }


    function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }


</script> 

    <table width="100%" style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td class="field_heading" style="text-align: center">
                Customers List</td>
        </tr>
        <tr>
            <td class="td_cell" style="color: blue; text-align: center">
                Type few characters of code or name and click search</td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>

                     <script type="text/javascript">
                         var prm = Sys.WebForms.PageRequestManager.getInstance();
                         prm.add_beginRequest(function () {

                         });

                         prm.add_endRequest(function () {
                             MyAutoCustomer();

                         });
                                 
                       </script>
<TABLE width=800><TBODY><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=6><asp:RadioButton id="rbsearch" tabIndex=16 runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbsearch_CheckedChanged" Checked="True"></asp:RadioButton>&nbsp;<asp:RadioButton id="rbnadsearch" tabIndex=17 runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbnadsearch_CheckedChanged" wfdid="w6"></asp:RadioButton>&nbsp;

<asp:Button id="btnSearch" tabIndex=10 runat="server" Text="Search" Font-Bold="False" Width="48px" CssClass="search_button SearchButtonC"></asp:Button>&nbsp;<asp:Button 
        id="btnClear" tabIndex=11 runat="server" Text="Clear" Font-Bold="False" 
        CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=15 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
        id="btnAddNew" tabIndex=12 runat="server" Text="Add New" Font-Bold="True" 
        CssClass="field_button"></asp:Button>&nbsp;<asp:Button id="btnPrint" tabIndex=14 runat="server" Text="Report" CssClass="field_button"></asp:Button>&nbsp;
</TD></TR>
 <TR><TD style="HEIGHT: 24px" class="td_cell">Customer Code</TD><TD style="WIDTH: 230px; HEIGHT: 24px"><asp:TextBox id="txtcustomercode" tabIndex=1 runat="server" Width="211px" CssClass="field_input" MaxLength="20"></asp:TextBox></TD><TD style="HEIGHT: 24px" class="td_cell">Customer Name&nbsp;</TD>
 <TD style="HEIGHT: 24px">
 
 <asp:TextBox id="txtcustomername"  tabIndex=2 runat="server" Width="219px" CssClass="field_input MyAutoCompleteCustClass" MaxLength="100"></asp:TextBox> 
  <asp:TextBox id="txtCutomerCodeHidden"  tabIndex=52 runat="server" Width="219px" style='display:none;' CssClass="field_input MyAutoCompleteCustClassValue" MaxLength="100"></asp:TextBox> 
  <asp:TextBox id="txtCutomerNameHidden"  tabIndex=45 runat="server" Width="219px" style='display:none;' CssClass="field_input MyAutoCompleteCustClassName" MaxLength="100"></asp:TextBox>


     
     
      </TD><TD style="HEIGHT: 24px"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD><TD style="HEIGHT: 24px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="field_input" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR><TR><TD><asp:Label id="lblcategory" runat="server" Text="Category Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD style="WIDTH: 230px"><SELECT style="WIDTH: 219px" id="ddlCategory" class="field_input" tabIndex=3 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsellingtype" runat="server" Text="Selling Type Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD><SELECT style="WIDTH: 225px" id="ddlSellingType" class="field_input" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD><asp:Label id="lblothersellingtype" runat="server" Text="Other Selling Type Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD style="WIDTH: 230px"><SELECT style="WIDTH: 219px" id="ddlOtherSellingType" class="field_input" tabIndex=5 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblsellingtypecode" runat="server" Text="Ticket Selling Type Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD><SELECT style="WIDTH: 225px" id="ddlTicketSelling" class="field_input" tabIndex=6 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 80px"><asp:Label id="lblCtryCode" runat="server" Text="Country Code" ForeColor="Black" Width="76px" CssClass="field_caption" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 219px" id="ddlCountryCode" class="field_input" tabIndex=7 onchange="CallWebMethod('ctrycode');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD><TD style="WIDTH: 79px"><asp:Label id="lblCtryName" runat="server" Text="Country Name" ForeColor="Black" Width="72px" CssClass="field_caption" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 225px" id="ddlCountryName" class="field_input" tabIndex=8 onchange="CallWebMethod('ctryname');" runat="server" visible="false"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblcity" runat="server" Text="City Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD style="WIDTH: 230px"><SELECT style="WIDTH: 219px" id="ddlCitycode" class="field_input" tabIndex=9 onchange="CallWebMethod('citycode')" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD><asp:Label id="lblCityName" runat="server" Text="City Name" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD><SELECT style="WIDTH: 225px" id="ddlCityName" class="field_input" tabIndex=10 onchange="CallWebMethod('cityname')" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblspersoncode" runat="server" CssClass="td_cell" Text="Salesman Code" Visible="False"
                Width="144px"></asp:Label></td>
        <td style="width: 230px">
            <select id="ddlSalesPerson" runat="server" class="field_input" onchange="CallWebMethod('salepcode')"
                style="width: 217px" tabindex="22" visible="false">
                <option selected="selected"></option>
            </select>
        </td>
        <td>
            <asp:Label ID="lblspersonname" runat="server" CssClass="td_cell" Text="Salesman Name" Visible="False"
                Width="144px"></asp:Label></td>
        <td>
            <select id="ddlSalesPersonName" runat="server" class="field_input" onchange="CallWebMethod('salepname')"
                style="width: 223px" tabindex="23" visible="false">
                <option selected="selected"></option>
            </select>
        </td>
    </tr>
    <TR><TD><asp:Label id="lblmarket" runat="server" Text="Market Code" Width="144px" CssClass="td_cell" Visible="False"></asp:Label></TD><TD style="WIDTH: 230px"><SELECT style="WIDTH: 219px" id="ddlMarket" class="field_input" tabIndex=11 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD></TD><TD></TD></TR></TBODY></TABLE>
                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                            height: 9px" type="text" />


</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" TabIndex="13"
                    Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td style="width: 100%">
    <asp:UpdatePanel id="UpdatePanel3" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=15 runat="server" Font-Size="10px" BackColor="White" Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField SortExpression="agentcode" Visible="False" HeaderText="Customer Code"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("agentcode") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="agentcode" SortExpression="agentcode" HeaderText="Customer Code"></asp:BoundField>
<asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Customer Name"></asp:BoundField>
<asp:BoundField DataField="catcode" SortExpression="catcode" HeaderText="Category Code"></asp:BoundField>
<%--<asp:BoundField DataField="sellcode" SortExpression="sellcode" HeaderText="Selling Type"></asp:BoundField>
<asp:BoundField DataField="othsellcode" Visible="False" SortExpression="othsellcode" HeaderText="Other Selling Type"></asp:BoundField>--%>
<asp:BoundField DataField="ctrycode" SortExpression="ctrycode" HeaderText="Country">
<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="citycode" Visible="False" SortExpression="citycode" HeaderText="City"></asp:BoundField>
<%--<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market"></asp:BoundField>--%>
<asp:BoundField DataField="add1" Visible="False" SortExpression="add1" HeaderText="Address Line 1"></asp:BoundField>
<asp:BoundField DataField="add2" Visible="False" SortExpression="add2" HeaderText="Address Line 2"></asp:BoundField>
<asp:BoundField DataField="add3" Visible="False" SortExpression="add3" HeaderText="Address Line 3"></asp:BoundField>
<asp:BoundField DataField="tel1" SortExpression="tel1" HeaderText="Telephone"></asp:BoundField>
<asp:BoundField DataField="fax" SortExpression="fax" HeaderText="Fax"></asp:BoundField>
<asp:BoundField DataField="controlacctcode" Visible="False" SortExpression="controlacctcode" HeaderText="Control A/c Code"></asp:BoundField>
<asp:BoundField DataField="crdays" Visible="False" SortExpression="crdays" HeaderText="Credit Days"></asp:BoundField>
<asp:BoundField DataField="crlimit" SortExpression="crlimit" HeaderText="Credit Limit"></asp:BoundField>
<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript"  language="javascript">
        MyAutoCustomer();
    
    </script>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

