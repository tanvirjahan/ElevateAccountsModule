<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ShuPriceList2.aspx.vb" Inherits="ShuPriceList2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript" >
    function chkTextLock(evt) {
        return false;
    }
    function chkTextLock1(evt) {
        if (evt.keyCode = 9) {
            return true;
        }
        else {
            return false;
        }
        return false;

    }
    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
        if (charCode != 47 && (charCode > 44 && charCode < 58)) {
            return true;
        }
        return false;
    }


    function checkNumber1(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if ((charCode > 47 && charCode < 58)) {
            //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
            return true;
        }
        return false;
    }

    function checkNumber2(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if ((charCode > 46 && charCode < 58)) {

            return true;
        }
        return false;
    }


    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
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



            <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
   <TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid">
     <TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4>
          <asp:Label id="lblHeading" runat="server" Text="Add New Shuttle price List" CssClass="field_heading" Width="744px"></asp:Label></TD></TR>
     
     <tr><td style="width: 201px" class="td_cell" align=left>
        <SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px">
            <INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD>
            <TD style="WIDTH: 190px" class="td_cell" align=left></TD>
            <TD></TD></TR><TR><TD style="WIDTH: 201px; HEIGHT: 3px" class="td_cell" align=left>
            <SPAN style="FONT-FAMILY: Arial">Supplier &nbsp;Type Code&nbsp;<SPAN style="COLOR: #ff0000">* </SPAN></SPAN></TD>
            <td style="WIDTH: 122px; HEIGHT: 3px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px; HEIGHT: 3px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier &nbsp;Type Name</SPAN></TD>
            <td style="HEIGHT: 3px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
            
     <%-- <tr><td style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Code </SPAN><SPAN style="COLOR: #ff0000">*</SPAN></TD>
          <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSupplierCode" class="field_input" tabIndex=4 onchange="CallWebMethod('PartyCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
          <td style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Name</SPAN></TD><TD><SELECT style="WIDTH: 300px" id="ddlSupplierName" class="field_input" tabIndex=5 onchange="CallWebMethod('PartyName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
          
      <tr><td style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Agent Code&nbsp; <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD>
          <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSupplierAgentCode" class="field_input" tabIndex=6 onchange="CallWebMethod('SupplierAgentCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
          <td style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Supplier Agent Name</SPAN></TD>
          <td><SELECT style="WIDTH: 300px" id="ddlSupplierAgentName" class="field_input" tabIndex=7 onchange="CallWebMethod('SupplierAgentName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
          --%>
         <%-- <TR><TD style="WIDTH: 201px" class="td_cell" align=left>Market Code <SPAN style="COLOR: #ff0000">*</SPAN></TD>
          <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=8 onchange="CallWebMethod('marketcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
          <TD style="WIDTH: 190px" class="td_cell" align=left>Market Name</TD>
          <TD><SELECT style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=9 onchange="CallWebMethod('marketname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>--%>
    
   <%-- <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Code</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD>
        <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Name</SPAN></TD>
        <TD><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=11 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     --%>
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Currency Code</SPAN> </SPAN></TD>
        <TD style="WIDTH: 122px"><asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" CssClass="field_input" Width="194px"></asp:TextBox></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD>
        <TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR>
        
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN> <SPAN style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></SPAN></TD>
        <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=14 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN></TD>
        <TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=15 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    
  
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Transfer Types</SPAN> <SPAN style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></SPAN></TD>
        <TD style="WIDTH: 122px">
            <asp:DropDownList ID="ddlServerType" runat="server" CssClass="fiel_input" 
                Enabled="False" tabindex="3" Width="200px">
            </asp:DropDownList>
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left>&nbsp;</TD>
        <TD>&nbsp;</TD></TR>

    
  
    <tr><TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD>
        <TD style="WIDTH: 183px; HEIGHT: 36px" align=left colSpan=3>
            <TEXTAREA style="WIDTH: 607px; HEIGHT: 29px" id="txtRemark" class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA></TD>
                 <caption>
                     <input id="txtconnection" runat="server" 
                         style="visibility: hidden; width: 12px; height: 9px" type="text" />
        </caption>
         </TR>
                
                
   <%--<tr><TD style="WIDTH: 201px" class="td_cell" align=left colSpan=2>
   <asp:Panel ID="pnlMarket" runat="server" Width="400px">
                                <table style="WIDTH: 360px; HEIGHT: 189px">
                                    <tbody>
                                        <tr>
                                            <td align="left" class="td_cell" style="WIDTH: 391px; height: 16px;">
                                                Market</td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="WIDTH: 391px" valign="top">
                                                <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Auto">
                                                    <asp:GridView ID="gv_Market" runat="server" AutoGenerateColumns="False" 
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                        CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" Width="360px">
                                                        <RowStyle CssClass="grdRowstyle" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server"    />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Market Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblcode" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle Width="400px" />
                                                                <ItemStyle Width="400px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="plgrpname" HeaderText="Market Name">
                                                                <HeaderStyle Width="1000px" />
                                                                <ItemStyle Width="1000px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle CssClass="grdfooter" />
                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                        <HeaderStyle CssClass="grdheader" />
                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="WIDTH: 391px">
                                                &nbsp;<asp:Button ID="btnSelectAll" runat="server" CssClass="btn" tabIndex="20" 
                                                    Text="Select All" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnUnselectAll" runat="server" CssClass="btn" tabIndex="21" 
                                                    Text="Unselect All" />
                                                &nbsp;<input id="txtrowcnt" runat="server"   type="text" style="width: 10px ; visibility: hidden; " />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
   </TD>--%>
   <TR><TD style="WIDTH: 201px" class="td_cell" align=left><%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                    <INPUT id="ChkActive" tabIndex=19 type=checkbox CHECKED runat="server" />Active</TD><TD style="WIDTH: 183px" align=left colSpan=3>
   
    <%--<asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Visible="False" />--%></TD></TR><%--<TR><TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=4>
    &nbsp;<asp:Button id="btnGenerate" tabIndex=20 onclick="btnGenerate_Click" runat="server" Text="Generate" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnCancel" tabIndex=21 onclick="btnCancel_Click" 
            runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=22 onclick="btnhelp_Click" runat="server" 
            Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR>--%>
            
        <%--  </TBODY></TABLE>--%>
        <TR><TD style="HEIGHT: 22px" class="td_cell" align=left colSpan=4>
          <TABLE style="WIDTH: 864px"><TBODY><TR>
            <TD style="WIDTH: 449px" class="td_cell" valign="top">
                <strong>Enter Dates</strong></TD>
            <td style="WIDTH: 351px">
                &nbsp;</td>
            <td style="WIDTH: 351px">
                &nbsp;<strong>Market</strong></td>
            <td class="td_cell" style="WIDTH: 449px" valign="top">
                &nbsp;</td>
            <td class="td_cell" style="WIDTH: 449px" valign="top">
                <strong>Selling Formula &amp; Currency Conversion</strong></td>
            </TR>
            <tr>
                <td style="WIDTH: 300px" valign="top">
                  <asp:Panel ID="Panel4" runat="server" Height="210px" ScrollBars="Auto" >
                    <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                        tabIndex="12" Width="1px">
                        <FooterStyle CssClass="grdfooter" />
                        <Columns>
                            <asp:BoundField DataField="SrNo" HeaderText="Sr No" Visible="False">
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="From Date">
                                <ItemTemplate>
                                    <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
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
                                    <br />
                                    <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                                        ControlExtender="MeFromDate" ControlToValidate="txtfromDate" 
                                        CssClass="field_error" Display="Dynamic" 
                                        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                        ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                                        InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date">
                                <ItemTemplate>
                                    <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
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
                                    <br />
                                    <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                                        ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                                        Display="Dynamic" EmptyValueBlurredText="Date is required" 
                                        EmptyValueMessage="Date is required" ErrorMessage="MeToDate" 
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" />
                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="grdheader" />
                        <AlternatingRowStyle CssClass="grdAternaterow" />
                    </asp:GridView></asp:Panel>
                    <br />
                    <asp:Button ID="btnAddLines" runat="server" CssClass="btn" 
                        onclick="btnAddLines_Click" Text="Add Row" />

                    <br />
                </td>
                <td style="WIDTH: 351px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 351px" valign="top">
                    <asp:Panel ID="Panel2" runat="server" Height="210px" ScrollBars="Auto" 
                        Width="369px">
                        <asp:GridView ID="gv_Market" runat="server" AutoGenerateColumns="False" 
                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" Width="360px">
                            <RowStyle CssClass="grdRowstyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Market Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcode" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="400px" />
                                    <ItemStyle Width="400px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="plgrpname" HeaderText="Market Name">
                                    <HeaderStyle Width="1000px" />
                                    <ItemStyle Width="1000px" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle CssClass="grdfooter" />
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                            <HeaderStyle CssClass="grdheader" />
                            <AlternatingRowStyle CssClass="grdAternaterow" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    <asp:Panel ID="Panel3" runat="server" Height="210px" ScrollBars="Auto">
                        <asp:GridView ID="grdSelling" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                            tabIndex="12" Width="325px" DataKeyNames="convrate">
                            <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="sellcode" HeaderText="Sell Code">
                                    <ItemStyle Width="400px" />
                                    <HeaderStyle Width="400px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="currcode" HeaderText="SellCurrency">
                                    <ItemStyle Width="1000px" />
                                    <HeaderStyle Width="1000px" />
                                </asp:BoundField>
                               <%-- <asp:TemplateField HeaderText="FormulaFrom">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlFormulaFrom" runat="server" AutoPostBack="True" 
                                            CssClass="drpdown" OnSelectedIndexChanged="ddlFormulaFrom_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">Category</asp:ListItem>
                                            <asp:ListItem>Supplier</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FormulaCode">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlFormulaCD" runat="server" CssClass="drpdown" 
                                            Width="124px">
                                            <asp:ListItem>[Select]</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                                <%--<asp:TemplateField HeaderText="Conv.Rate" >
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("convrate") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtconv" runat="server" CssClass="field_input" 
                                            Text='<%# Bind("convrate") %>' Width="75px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                             <%--   <div id="div1" style="display:block"></div>--%>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle" />
                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="grdheader" />
                            <AlternatingRowStyle CssClass="grdAternaterow" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <%--<TR><TD style="WIDTH: 449px">
                &nbsp;</TD>
                <td style="WIDTH: 351px">
                    &nbsp;</td>
                <td style="WIDTH: 351px">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    <asp:Button ID="btnChkDate" runat="server" CssClass="btn" 
                        Text="Check Duplicate Dates" Visible="False" Width="162px" />
                </td>
            </TR>--%>
            <tr>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 351px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 351px" valign="top">
                    &nbsp;&nbsp;&nbsp;
                </td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
                <td style="WIDTH: 449px" valign="top">
                    &nbsp;</td>
            </tr>
            </TBODY></TABLE> </TD> </TR> 

        <tr>
                <td style="WIDTH: 449px" class="td_cell" valign="top"><strong>Enter Selling Rates</strong></td></tr> 
                <tr>

                    <td align="left" class="td_cell" colSpan="4" style="HEIGHT: 22px">
                        <asp:Panel ID="Panel1" runat="server"   
                            ScrollBars="Auto" Width="900px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td >
                                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                                CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                                tabIndex="20" >
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                                                <Columns>
                                                <asp:TemplateField><EditItemTemplate>
                                                <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                <INPUT id="ChkSelect" type=checkbox name="ChkSelect" runat="server" />
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:BoundField DataField="Sr No" HeaderText="Sr No" ></asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Code" HeaderText="Service Type Code">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Name"  HeaderText="Service Type Name">
                                                       
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" 
                                                    HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" Font-Bold="True" ForeColor="white" 
                                                    HorizontalAlign="Left" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr> 
                <td>                     <asp:Button ID="btnPreviousRates" runat="server" CssClass="btn" 
                        onclick="btnPreviousRates_Click" Text="Copy Rates to next line" Width="196px" />
                                  </td>
                </tr>
                <tr>
                    <td align="left" class="td_cell" colSpan="4" style="HEIGHT: 22px">
                        <table style="WIDTH: 951px">
                            <tbody>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Verdana" 
                                            Font-Size="8pt" Visible="False" Width="734px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="WIDTH: 133px; HEIGHT: 23px">
                                    <asp:HiddenField ID="hdnMarkUp" runat="server" />
                                    </td>
                                    <td style=" HEIGHT: 23px " colspan="2" >
                                    <%--<asp:CheckBox ID="chkConsdierForMarkUp" runat="server" Font-Bold="False" 
                                            Text="Consider this supplier for markup " />--%>
                                    </td>
                                    <td align="right" style="HEIGHT: 23px">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="WIDTH: 133px; HEIGHT: 23px">
                                        <table align="left" class="td_cell" 
                                            style="BORDER-RIGHT: blue 1px solid; BORDER-TOP: blue 1px solid; BORDER-LEFT: blue 1px solid; WIDTH: 356px; BORDER-BOTTOM: blue 1px solid">
                                            <tbody>
                                                <tr>
                                                    <td style="WIDTH: 100px">
                                                        -1 Incl</td>
                                                    <td style="WIDTH: 100px">
                                                        -2 N/Incl</td>
                                                    <td style="WIDTH: 100px">
                                                        -3 Free</td>
                                                    <td style="WIDTH: 100px">
                                                        -4 N/A</td>
                                                    <td style="WIDTH: 100px">
                                                        -5 On Request</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" 
                                            Text="Approve/Unapprove" />
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnClearFormula" runat="server" CssClass="field_button" 
                                            onclick="BtnClearFormula_Click" tabIndex="23" Text="Clear Prices" />
                                        &nbsp;
                                       
                                    </td>
                                </tr>
                                <tr >
                                
                                <td align="right">&nbsp;</td>
                                <td>
                                    <%--<asp:CheckBox ID="chkrecalsprice" runat="server" Font-Bold="False" 
                                        Text="Recalculate Selling Price" />--%>
                                    </td>
                                <td> <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                            onclick="btnSave_Click" tabIndex="21" Text="Save" />
                                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                            onclick="btnCancel_Click" tabIndex="22" Text="Return to Search" />
                                        &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" Height="20px" 
                                            onclick="btnhelp_Click" tabIndex="23" Text="Help" />
                                        &nbsp;<asp:Button ID="btnselling" runat="server" CssClass="btn" tabIndex="18" 
                                                Text="View Selling" Visible="false" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <caption>
                    <select ID="ddlGroupCode" runat="server" class="field_input" 
                        style="WIDTH: 200px" tabindex="10" visible="false">
                        <option selected=""></option>
                    </select>
                </caption>
         
</contenttemplate>
</asp:UpdatePanel>

</asp:Content>

