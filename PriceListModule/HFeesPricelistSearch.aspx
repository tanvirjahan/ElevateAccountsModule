<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="HFeesPricelistSearch.aspx.vb" Inherits="PriceListModule_HFeesPricelistSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
        <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type="text/javascript" >
    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }
    function checkCharacter(e) {
        if (event.keyCode == 32 || event.keyCode == 46)
            return;
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }

    }

    function CallWebMethod(methodType) {
        switch (methodType) {


            case "othsellcode":
                var select = document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlOtherSellingTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "othsellname":
                var select = document.getElementById("<%=ddlOtherSellingTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

          

            case "SeasCode":
                var select = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "SeasName":
                var select = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

        }
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

<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;border-bottom: gray 2px solid;" id="TABLE1" >
<tr>
  <td align="center" class="field_heading">
  <asp:Label id="lblheading" runat="server" Text="Other Services Price List" CssClass="field_heading" __designer:wfdid="w30"></asp:Label> </td>
</tr>
<tr>
  <td align="center" class="td_cell" style="color: blue;  ">Type few characters of code or name and click search</td>
</tr>

<tr>
  <td>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
      <contenttemplate>

      <table style="width: 816px">
      <tbody>
      <tr><td class="td_cell" align=center colSpan=4>
            <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" Checked="True" AutoPostBack="True"></asp:RadioButton> 
            <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True"></asp:RadioButton> 
            <asp:Button id="btnSearch" tabIndex=16 onclick="btnSearch_Click" runat="server" Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
            <asp:Button id="btnClear" tabIndex=17 runat="server" Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
            <asp:Button id="btnHelp" tabIndex=21 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;
            <asp:Button id="btnAddNew" tabIndex=18 onclick="btnAddNew_Click" runat="server" Text="Add New" Font-Bold="False" Width="75px" CssClass="btn"></asp:Button>&nbsp;
            <asp:Button id="btnPrint" tabIndex=20 runat="server" Text="Report" CssClass="btn" Visible="False"></asp:Button></TD></TR>
            
     <tr><td style="width: 510px" class="td_cell" align=left>
        <span  class="td_cell">PL Code </SPAN></TD>
        <td style="width: 200px">
            <asp:TextBox id="TxtPLCD" TabIndex=1 runat="server" Width="194px" CssClass="txtbox"></asp:TextBox></TD>
        <td style="width: 156px" class="td_cell">Order By</TD>
        <td><asp:DropDownList id="ddlOrderBy" runat="server" Width="120px" CssClass="drpdown" AutoPostBack="True">
                <asp:ListItem Value="0">Plist code Desc</asp:ListItem>
                <asp:ListItem Value="1">Plist code Asc</asp:ListItem>             
               
                <asp:ListItem Value="3">Group</asp:ListItem>
                <asp:ListItem Value="4">Sub Season</asp:ListItem></asp:DropDownList></TD></TR>
                
    
  
    <tr>
        <td align="left" class="td_cell" style="width: 510px">
            Approved /Unapproved</td>
        <td style="width: 200px">
            <asp:DropDownList ID="DDLstatus" runat="server" CssClass="drpdown" Width="198px">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Unapprove</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:DropDownList></td>
        <td align="left" class="td_cell" style="width: 156px" >
        </td>
        <td>
        </td>
    </tr>
       <tr><td class="td_cell" align=left style="width: 510px">
           <SPAN class="td_cell">Other Selling Type Code</SPAN></TD>
            <td style="width: 203px"><SELECT style="width: 200px" id="ddlOtherSellingTypeCode" class="drpdown" tabIndex=8 onchange="CallWebMethod('othsellcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td style="width: 788px; WIDT: 686px" class="td_cell" align=left><SPAN class="td_cell">Other Selling Type Name</SPAN></TD>
            <td style="height: 22px"><SELECT style="width: 300px" id="ddlOtherSellingTypeName" class="drpdown" tabIndex=9 onchange="CallWebMethod('othsellname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     
     <tr><td  class="td_cell" align=left style="width: 510px"><SPAN class="td_cell">From Date</SPAN></TD>
        <%-- <td style="WIDTH: 203px"><ews:DatePicker id="dpFromdate" tabIndex=14 runat="server" Width="180px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>--%>
        <td style="width: 200px">
                  <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px">
                  </asp:TextBox>
                  <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                      PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtfromDate">
                  </cc1:CalendarExtender>
                  <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                      MaskType="Date" TargetControlID="txtfromDate">
                  </cc1:MaskedEditExtender>
                  <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                      ImageUrl="~/Images/Calendar_scheduleHS.png" />
                  <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                      ControlExtender="MeFromDate" ControlToValidate="txtfromDate" 
                      CssClass="field_error" Display="Dynamic" 
                      EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                      ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                      InvalidValueMessage="Invalid Date" 
                      TooltipMessage="Input a Date in Date/Month/Year">
                  </cc1:MaskedEditValidator>
              </td>
         <td style="WIDT: 686px" class="td_cell" align=left><SPAN class="td_cell">To &nbsp;Date</SPAN></TD>
         <%--<td><ews:DatePicker id="dpToDate" tabIndex=15 runat="server" Width="180px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
          <td>
                  <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
                  </asp:TextBox>
                  <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                      PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                  </cc1:CalendarExtender>
                  <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                      MaskType="Date" TargetControlID="txtToDate">
                  </cc1:MaskedEditExtender>
                  <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                      ImageUrl="~/Images/Calendar_scheduleHS.png" />
                  <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                      ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                      Display="Dynamic" EmptyValueBlurredText="Date is required" 
                      EmptyValueMessage="Date is required" ErrorMessage="MeToDate" 
                      InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                      TooltipMessage="Input a Date in Date/Month/Year">
                  </cc1:MaskedEditValidator>
              </td>
   </TD></TR>
    </tbody></table>

    <asp:Panel id="PnlOtherServiceCost" runat="server" Height="34px" Visible="False" 
              Width="864px">
    <table style="width:817px"><tbody>
         
     <%--<tr><td style="width: 674px" class="td_cell" align=left>
           <SPAN class="td_cell">Market Code</SPAN></TD>
            <td style="width: 203px"><SELECT style="width: 200px" id="ddlMarketCode" class="drpdown" tabIndex=8 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td style="width: 788px; WIDT: 686px" class="td_cell" align=left><SPAN class="td_cell">Market &nbsp;Name</SPAN></TD>
            <td style="height: 22px"><SELECT style="width: 300px" id="ddlMarketName" class="drpdown" tabIndex=9 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     --%>
     
     <tr><td style="WIDTH: 674px" class="td_cell" align=left><SPAN class="td_cell">Sub Season Code</SPAN></TD>
         <td style="WIDTH: 203px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="drpdown" tabIndex=12 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
         <td style="WIDTH: 788px; WIDT: 686px" class="td_cell" align=left><SPAN class="td_cell">Sub Season Name</SPAN></TD>
         <td style="HEIGHT: 22px"><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="drpdown" tabIndex=13 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    
     </TBODY></TABLE></asp:Panel> 
 </contenttemplate>
 </asp:UpdatePanel> </td>
        </tr>
     <tr>
        <td >&nbsp;
          <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel" Width="126px" TabIndex="19" />&nbsp;
           <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px; height: 9px" type="text" />
          </td>
        </tr>
     <tr>
        <td >
  <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=21 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="PList Code"><EditItemTemplate>
                            
</EditItemTemplate>
<ItemTemplate>
                    <asp:Label ID="lblocplistcode" runat="server" Text='<%# Bind("oplistcode") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField Visible="False" HeaderText="Approve"><EditItemTemplate>                                      
</EditItemTemplate>
<ItemTemplate>
                <asp:Label id="lblapprove" runat="server" Text='<%# Bind("approve") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="oplistcode" SortExpression="oplistcode" HeaderText="OPList Code"></asp:BoundField>


<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group"></asp:BoundField>
<asp:BoundField DataField="othsellname" SortExpression="othsellname" HeaderText="Other Selling Type"></asp:BoundField>

<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency"></asp:BoundField>
<asp:BoundField DataField="subseascode" SortExpression="subseascode" HeaderText="Sub Season"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="frmdate" SortExpression="frmdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="approve" SortExpression="approve" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
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
<ControlStyle ForeColor="Blue"></ControlStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg" 
                            Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
                      <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>


</asp:Content>

