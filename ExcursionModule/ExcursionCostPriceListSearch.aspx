<%@ Page Title="" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="ExcursionCostPriceListSearch.aspx.vb" Inherits="TransportModule_TransferPriceList" %>

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

            case "sellcd":
                var select = document.getElementById("<%=ddlSellcd.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

            case "sellnm":
                var select = document.getElementById("<%=ddlSellingName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSellcd.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;


            case "ddlexccode":
                var select = document.getElementById("<%=ddlexccode.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexcname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

            case "ddlexcname":
               
                var select = document.getElementById("<%=ddlexcname.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexccode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                codeid = select.options[select.selectedIndex].value;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

            case "groupcd":
                var select = document.getElementById("<%=ddlgpcd.ClientID%>");
                var selectname = document.getElementById("<%=ddlgpnm.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "groupnm":
                var select = document.getElementById("<%=ddlgpnm.ClientID%>");
                var selectname = document.getElementById("<%=ddlgpcd.ClientID%>");
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


    function ddlgrpcode_onclick() {

    }

    </script>

<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;border-bottom: gray 2px solid;" id="TABLE1" >
<tr>
  <td align="center" class="field_heading">
  <asp:Label id="lblheading" runat="server" Text="Excursion Cost Price List" CssClass="field_heading" __designer:wfdid="w30"></asp:Label> </td>
</tr>
<tr>
  <td align="center" class="td_cell" style="color: blue;">Type few characters of code or name and click search</td>
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
            <asp:Button id="btnSearch" tabIndex=12 onclick="btnSearch_Click" runat="server" Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
            <asp:Button id="btnClear" tabIndex=13 runat="server" Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
            <asp:Button id="btnHelp" tabIndex=14 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;
            <asp:Button id="btnAddNew" tabIndex=15 onclick="btnAddNew_Click" runat="server" Text="Add New" Font-Bold="False" Width="75px" CssClass="btn"></asp:Button>&nbsp;
            <asp:Button id="btnPrint" tabIndex=16 runat="server" Text="Report" CssClass="btn" Visible="False"></asp:Button></TD></TR>
            
     <tr><td style="width: 510px" class="td_cell" align=left>
        <span  class="td_cell">PL Code </SPAN></TD>
        <td style="width: 200px">
            <asp:TextBox id="TxtPLCD" TabIndex=17 runat="server" Width="196px" CssClass="txtbox"></asp:TextBox></TD>
        <td style="width: 156px" class="td_cell">&nbsp;</TD>
        <td>
            &nbsp;</TD></TR>
                
      <tr><td style="width: 510px" class="td_cell" align=left>Group Code&nbsp;</TD>
        <td style="width: 200px">
            <SELECT style="width: 200px" id="ddlgpcd" class="drpdown" tabIndex=2 
                onchange="CallWebMethod('groupcd');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <td style="width: 156px" class="td_cell">Group&nbsp; Name</td >
        <td><SELECT style="WIDTH: 200px" id="ddlgpnm" class="drpdown" tabIndex=3 
                onchange="CallWebMethod('groupnm');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>

          <tr>
              <td  align="left" class="td_cell" style="width: 510px">
                  <asp:Label ID ="lblexccode" runat ="server" visible ="false">   Excursion Code </asp:Label></td>
              <td style="width: 200px" >
                  <select id="ddlexccode" runat="server" class="drpdown" name="D1" visible ="false"
                      onchange="CallWebMethod('ddlexccode');" style="width: 200px" tabindex="2">
                      <option selected=""></option>
                  </select>
              </td>
              <td class="td_cell" style="width: 156px">
               <asp:Label ID ="lblexcname" runat ="server" visible ="false">   Excursion Name </asp:Label></td>
              <td >
                  <select id="ddlexcname" runat="server" class="drpdown" name="D2" visible ="false"
                      onchange="CallWebMethod('ddlexcname');" style="WIDTH: 200px" tabindex="3">
                      <option selected=""></option>
                  </select>
              </td>
          </tr>

    <tr>
        <td align="left" class="td_cell" style="width: 510px">
            Party&nbsp; Code&nbsp;</td>
        <td style="width: 200px">
            <select ID="ddlSellcd" runat="server" class="drpdown" 
                onchange="CallWebMethod('sellcd');" style="WIDTH: 200px" tabindex="3">
                <option selected=""></option>
            </select>
        </td>
        <td class="td_cell" style="width: 156px">
            Party&nbsp; Name</td>
        <td>
            <select ID="ddlSellingName" runat="server" class="drpdown" 
                onchange="CallWebMethod('sellnm');" style="WIDTH: 200px" tabindex="5">
                <option selected=""></option>
            </select>
        </td>
          </TR>
        
        <tr>
         <td style="width: 510px" class="td_cell" align=left><span class="td_cell">From Date</span></TD>
        <%-- <td style="WIDTH: 203px"><ews:DatePicker id="dpFromdate" tabIndex=6 runat="server" Width="180px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>--%>
         <td style="width: 200px">
             <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" 
                 Width="150px">
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
         <td align="left" class="td_cell" style="width: 686px">
             <span class="td_cell">To &nbsp;Date</span>
         </td>
         <%--<td><ews:DatePicker id="dpToDate" tabIndex=7 runat="server" Width="180px" CssClass="field_input" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
          <td>
              <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="100px">
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

    <tr>
        <td align="left" class="td_cell" style="width: 510px">
            &nbsp;</td>
        <td style="width: 200px">
            &nbsp;</TD>
        <td style="width: 156px" class="td_cell">&nbsp;</td >
        <td>&nbsp;</TD></TR>

   <tr><td style="width: 686px" class="td_cell" align=left>Order By</TD>
        <td><asp:DropDownList id="ddlOrderBy" runat="server" Width="200px" CssClass="drpdown" AutoPostBack="True">
                <asp:ListItem Value="0">Plistcode Desc</asp:ListItem>
                <asp:ListItem Value="1">Plistcode Asc</asp:ListItem>
                <asp:ListItem Value="2">Group Name Desc</asp:ListItem>
                <asp:ListItem Value="3">Group Name Asc</asp:ListItem>
                <asp:ListItem Value="4">Party Name Desc</asp:ListItem>
                <asp:ListItem Value="5">Party Name Asc</asp:ListItem>
            </asp:DropDownList>
        </TD>

        <td style="width: 686px" class="td_cell" align=left>Approved</TD>
        <td><asp:DropDownList id="ddlapprove" runat="server" Width="200px" CssClass="drpdown" AutoPostBack="True">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Approved</asp:ListItem>
                <asp:ListItem Value="2">Pending</asp:ListItem>
                
            </asp:DropDownList>
        </TD>

   <td class="td_cell" >&nbsp;</TD><td>
       &nbsp;</td></td></TR>
    </tbody></table>

    <asp:Panel id="PnlOtherServiceCost" runat="server" Height="100px" Visible="False">
    <table style="width: 817px"><tbody>
     
     </TBODY></TABLE></asp:Panel> 
 </contenttemplate>
 </asp:UpdatePanel> </td>
        </tr>
     <tr>
        <td >&nbsp;
          <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel" Width="140px" TabIndex="10" />&nbsp;
           <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px; height: 9px" type="text" />
          </td>
        </tr>
     <tr>
        <td >
  <asp:UpdatePanel id="UpdatePanel2" runat="server">
  <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=11 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="PList Code"><EditItemTemplate>
</EditItemTemplate>
<ItemTemplate>
<asp:Label ID="lblocplistcode" runat="server" Text='<%# Bind("eplistcode") %>'></asp:Label>         
</ItemTemplate>
</asp:TemplateField>



<asp:BoundField DataField="eplistcode" SortExpression="eplistcode" HeaderText="PList Code"></asp:BoundField>

<asp:BoundField DataField="gpcode" SortExpression="gpcode" HeaderText="Group Code"></asp:BoundField>
<asp:BoundField DataField="gpname" SortExpression="gpname" HeaderText="Group Name"></asp:BoundField>

<asp:BoundField DataField="partycode" SortExpression="partyname" HeaderText="Party Code"></asp:BoundField>
<asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Party Name"></asp:BoundField>
<asp:BoundField DataField="approved" SortExpression="approved" HeaderText="Approved "></asp:BoundField>
<asp:BoundField DataField="frmdate" SortExpression="frmdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" DataField="adddate" SortExpression="adddate" HeaderText="Add Date"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="Add User"></asp:BoundField>
<asp:BoundField HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" DataField="moddate" SortExpression="moddate" HeaderText="Modified Date"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="Modified User"></asp:BoundField>
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

