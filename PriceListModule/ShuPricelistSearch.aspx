<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="ShuPricelistSearch.aspx.vb" Inherits="ShuPricelistSearch" %>
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

            case "sptypecode":
                var select = document.getElementById("<%=ddlSPType.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

//                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCodes, ErrorHandler, TimeOutHandler);
//                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierNames, ErrorHandler, TimeOutHandler);

                break;
            case "sptypename":
                var select = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSPType.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

//                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCodes, ErrorHandler, TimeOutHandler);
//                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierNames, ErrorHandler, TimeOutHandler);
                break;




           

            case "marketcode":
//                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
//                var data1 = hddata.value
//                if (data1 == "1") {
                    
                    var select = document.getElementById("<%=ddlMarketCode.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlMarketName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
//                }
            case "marketname":
//                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
//                var data1 = hddata.value
//                if (data1 == "1") {
                   
                    var select = document.getElementById("<%=ddlMarketName.ClientID%>");
                    var codeid = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlMarketCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
//                }

            case "GroupCode":
                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
                var data1 = hddata.value
                if (data1 == "1") {
                    
                    var select = document.getElementById("<%=ddlGroupCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlGroupName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                }
            case "GroupName":
                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
                var data1 = hddata.value
                if (data1 == "1") {
                  
                    var select = document.getElementById("<%=ddlGroupName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlGroupCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                }

            case "SeasCode":
                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
                var data1 = hddata.value
                if (data1 == "1") {
                    
                    var select = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                }
            case "SeasName":
                var hddata = document.getElementById("<%=hdpanelvalue.ClientID%>")
                var data1 = hddata.value
                if (data1 == "1") {
                   

                    var select = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                    var selectname = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    break;
                }

        }
    }


    


//    function FillGroupCodesGroup(result) {
//        var ddl = document.getElementById("<%=ddlGroupCode.ClientID%>");
//        RemoveAll(ddl)
//        for (var i = 0; i < result.length; i++) {
//            var option = new Option(result[i].ListText, result[i].ListValue);
//            ddl.options.add(option);
//        }
//        ddl.value = "[Select]";
//    }

//    function FillGroupNamesGroup(result) {
//        var ddl = document.getElementById("<%=ddlGroupName.ClientID%>");
//        RemoveAll(ddl)
//        for (var i = 0; i < result.length; i++) {
//            var option = new Option(result[i].ListText, result[i].ListValue);
//            ddl.options.add(option);
//        }
//        ddl.value = "[Select]";
//    }





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
  <td align="center" class="field_heading"  >Shuttle Price List   </td>
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
            
     <tr><td style="width: 141px" class="td_cell" align=left>
        <span  class="td_cell">PL Code </SPAN></TD>
        <td style="width: 200px">
            <asp:TextBox id="TxtPLCD" TabIndex=1 runat="server" Width="194px" CssClass="txtbox"></asp:TextBox></TD>
        <td style="width: 156px" class="td_cell">Order By</TD>
        <td><asp:DropDownList id="ddlOrderBy" runat="server" Width="120px" CssClass="drpdown" AutoPostBack="True">
                <asp:ListItem Value="0">Plist code Desc</asp:ListItem>
                <asp:ListItem Value="1">Plist code Asc</asp:ListItem>
                <asp:ListItem Value="2">Supplier</asp:ListItem>
                <asp:ListItem Value="3">Market</asp:ListItem>
                <asp:ListItem Value="4">Group</asp:ListItem>
                <asp:ListItem Value="5">Sub Season</asp:ListItem></asp:DropDownList></TD></TR>
                
    <tr><td style="width: 141px" class="td_cell" align=left>Supplier &nbsp;Type Code&nbsp;</TD>
        <td style="width: 200px">
            <SELECT style="width: 200px" id="ddlSPType" class="drpdown" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <td style="width: 156px" class="td_cell">Supplier &nbsp;Type Name</td >
        <td><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="drpdown" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
  
    <tr>
        <td align="left" class="td_cell" style="width: 141px">
            Approved /Unapproved</td>
        <td style="width: 200px">
            <asp:DropDownList ID="DDLstatus" runat="server" CssClass="drpdown" Width="198px">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Unapprove</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:DropDownList></td>
        <td align="left" class="td_cell" style="width: 156px" >
            Transfer Type</td>
        <td>
            <asp:DropDownList ID="ddlServerType" runat="server" CssClass="fiel_input" 
                tabindex="3" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
          <tr>
              <td align="left" class="td_cell" style="width: 141px">
                  <span class="td_cell">Market Code</span>
              </td>
              <td style="width: 200px">
                  <select id="ddlMarketCode" runat="server" class="drpdown" name="D1" 
                      onchange="CallWebMethod('marketcode');" style="width: 200px" tabindex="8">
                      <option selected=""></option>
                  </select>
              </td>
              <td align="left" class="td_cell" style="width: 156px">
                  <span class="td_cell">Market &nbsp;Name</span>
              </td>
              <td>
                  <select id="ddlMarketName" runat="server" class="drpdown" name="D2" 
                      onchange="CallWebMethod('marketname');" style="width: 300px" tabindex="9">
                      <option selected=""></option>
                  </select>
              </td>
          </tr>
          <tr>
              <td align="left" class="td_cell" style="width: 141px">
                  <span class="td_cell">From Date</span>
              </td>
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
              <td align="left" class="td_cell" style="width: 156px">
                  <span class="td_cell">To &nbsp;Date</span>
              </td>
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
          </tr>
    </tbody></table>

    <asp:Panel id="PnlOtherServiceCost" runat="server" Height="150px" Visible="False">
    <table style="width: 817px"><tbody>
     <%--<tr><td style="width: 674px" class="td_cell" align=left>Supplier Agent Code</TD><td style="width: 203px">
         <SELECT style="width: 200px" id="ddlSupplierAgentCode" class="drpdown" tabIndex=6 onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
         <td style="width: 788px; WIDT: 686px" class="td_cell" align=left>Supplier Agent Name</TD><TD style="HEIGHT: 22px">
         <SELECT style="WIDTH: 300px" id="ddlSupplierAgentName" class="drpdown" tabIndex=7 onchange="CallWebMethod('SupplierAgentName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>--%>
         
     <tr><td style="WIDTH: 674px" class="td_cell" align=left><SPAN class="td_cell">Other Service Group Code</SPAN></TD>
         <td style="WIDTH: 203px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="drpdown" tabIndex=10 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
         <td style="WIDTH: 788px; WIDT: 686px" class="td_cell" align=left><SPAN class="td_cell">Other Service Group Name</SPAN></TD>
         <td><SELECT style="WIDTH: 300px" id="ddlGroupName" class="drpdown" tabIndex=11 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     
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
          <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px; height: 9px" type="text" /></td>
        </tr>
     <tr>
        <td >
  <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=21 runat="server" Font-Size="10px" Width="800px" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="PList Code"><EditItemTemplate>
                            
</EditItemTemplate>
<ItemTemplate>
                    <asp:Label ID="lblocplistcode" runat="server" Text='<%# Bind("tplistcode") %>'></asp:Label>
                            
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="tplistcode" SortExpression="tplistcode" HeaderText="TPList Code"></asp:BoundField>
<%--<asp:BoundField DataField="partyname" SortExpression="partyname" HeaderText="Suppplier">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>--%>
<%--<asp:BoundField DataField="supagentname" SortExpression="supagentname" HeaderText="Agent"></asp:BoundField>--%>
<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market"></asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency"></asp:BoundField>
<asp:BoundField DataField="subseascode" SortExpression="subseascode" HeaderText="Sub Season"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="frmdate" SortExpression="frmdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="approve" SortExpression="approve" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField DataField="transfertype" SortExpression="transfertype" HeaderText="Transfer Type"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>

<asp:TemplateField Visible="False" HeaderText="Approve"><EditItemTemplate>                                      
</EditItemTemplate>
<ItemTemplate>
                <asp:Label id="lblapprove" runat="server" Text='<%# Bind("approve") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

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
                              <asp:HiddenField ID="hdpanelvalue" runat="server" />
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

