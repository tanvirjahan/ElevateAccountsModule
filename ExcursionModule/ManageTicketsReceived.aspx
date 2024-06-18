<%@ Page Title="Manage Tickets Received" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ManageTicketsReceived.aspx.vb" Inherits="ExcursionModule_ManageTicketsReceived" %>

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

 <script language="javascript" src="../TADDScript.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">

   

    function chkTextLock(e) {
        return false;
    }

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

            case "ExGrpCode":
                var select = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlExGrpName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
                hdnExcursionGroupCode.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;

                if (codeid != '[Select]') {
                    ColServices.clsServices.GetExcursionTypeCodeByGroupCode(constr, codeid, FillExTypeCode, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetExcursionTypeNameByGroupCode(constr, codeid, FillExTypeName, ErrorHandler, TimeOutHandler);
                }

                break;

            case "ExGrpName":
                var select = document.getElementById("<%=ddlExGrpName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlExGrpCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;

                var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
                hdnExcursionGroupCode.value = selectname.options[selectname.selectedIndex].text;

                if (codeid != '[Select]') {
                    ColServices.clsServices.GetExcursionTypeCodeByGroupCode(constr, codeid, FillExTypeCode, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetExcursionTypeNameByGroupCode(constr, codeid, FillExTypeName, ErrorHandler, TimeOutHandler);
                }
                break;

            case "ExTypeCode":
                var select = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                var hdnExcursionTypeName = document.getElementById("<%=hdnExcursionTypeName.ClientID%>");
                hdnExcursionTypeCode.value = select.options[select.selectedIndex].text;
                hdnExcursionTypeName.value = select.options[select.selectedIndex].value;
                break;

            case "ExTypeName":
                var select = document.getElementById("<%=ddlExTypeName.ClientID%>");
                var selectname = document.getElementById("<%=ddlExTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                var hdnExcursionTypeName = document.getElementById("<%=hdnExcursionTypeName.ClientID%>");
                hdnExcursionTypeCode.value = selectname.options[selectname.selectedIndex].text;
                hdnExcursionTypeName.value = selectname.options[selectname.selectedIndex].value;
                break;

        }
    }

    function FillExTypeCode(result) {
        var ddl = document.getElementById("<%=ddlExTypeCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillExTypeName(result) {
        var ddl = document.getElementById("<%=ddlExTypeName.ClientID%>");

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



    function FormValidation(state) {


        if (document.getElementById("<%=ddlExGrpCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlExGrpCode.ClientID%>").focus();
            alert("Please select Excursion Group Code.");
            return false;
        }

        else if (document.getElementById("<%=ddlExTypeCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlExTypeCode.ClientID%>").focus();
            alert("Please select Excursion Type Code.");
            return false;

        }


               else {

            if (state == 'New') { if (confirm('Are you sure you want to save Excursion Tickets ?') == false) return false; }
            if (state == 'EditRow') { if (confirm('Are you sure you want to update Excursion Tickets?') == false) return false; }
            if (state == 'DeleteRow') { if (confirm('Are you sure you want to Delete Excursion Tickets?') == false) return false; }

        }
    }

    function CheckTicketNo(fromticketno, toticketno) {
        txt = document.getElementById(fromticketno);
        txt1 = document.getElementById(toticketno);


        if (parseInt(txt1.value) <= parseInt(txt.value)) {
            var datearray = txt.value.split("/");
            alert("FromTicketNo should be less than ToTicketNo.");
            txt1.value = '';
            txt1.focus();
            return false;

        }
        return true;
    }

    function DateAdd(timeU, byMany, dateObj) {
        var millisecond = 1;
        var second = millisecond * 1000;
        var minute = second * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var year = day * 365;

        var newDate;
        var dVal = dateObj.valueOf();
        switch (timeU) {
            case "ms": newDate = new Date(dVal + millisecond * byMany); break;
            case "s": newDate = new Date(dVal + second * byMany); break;
            case "mi": newDate = new Date(dVal + minute * byMany); break;
            case "h": newDate = new Date(dVal + hour * byMany); break;
            case "d": newDate = new Date(dVal + day * byMany); break;
            case "y": newDate = new Date(dVal + year * byMany); break;
        }
        return newDate;
    }

    function Left(str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else
            return String(str).substring(0, n);
    }

    function Right(str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else {
            var iLen = String(str).length;
            return String(str).substring(iLen, iLen - n);
        }
    }
    
</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY>
<TR>
<TD class="field_heading" align=center colSpan=5>
<asp:Label id="lblHeading" runat="server" Text="Add New Ticket Recieved" CssClass="field_heading" Width="300px"></asp:Label></TD>
</TR>
<TR>
<TD class="td_cell" colSpan=5>
<TABLE class="td_cell">
<TBODY>
<TR>
<TD class="td_cell">Excursion Group Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD class="td_cell">  <SELECT style="WIDTH: 200px" id="ddlExGrpCode" 
class="field_input" tabIndex=1  onchange="CallWebMethod('ExGrpCode');" 
runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Excursion Group Name</TD><TD class="td_cell">  
<SELECT style="WIDTH: 300px" id="ddlExGrpName" class="field_input" tabIndex=2   
onchange="CallWebMethod('ExGrpName');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
</TR>

<TR>
<TD class="td_cell">Excursion Type Code<SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD class="td_cell"> <SELECT style="WIDTH: 200px" id="ddlExTypeCode" 
class="field_input" tabIndex=3   onchange="CallWebMethod('ExTypeCode');" 
runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="td_cell">Excursion Type Name</TD>
<TD>  <SELECT style="WIDTH: 300px" id="ddlExTypeName" class="field_input" 
tabIndex=4   onchange="CallWebMethod('ExTypeName');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
</TR>

<tr>
<TD class="td_cell">Ticket ID</TD>
<TD><INPUT id="txtAllotmentID" disabled type=text runat="server" />
</TD>
<td class="td_cell">Date Received<SPAN style="COLOR: #ff0000">*</SPAN></td>
<td><ews:DatePicker id="dpDateRecieved"  tabIndex=13 runat="server" CssClass="field_input" Width="200px" DateRegularExpression="\d{1,2}\/\d{1,2}\/\d{4}" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></td>   
</TR>
    
<TR>
<TD class="td_cell">Remarks</TD>
<TD colSpan=3>
    
    
   
<asp:TextBox runat="server" id="txtRemark"      class="field_input" 
MaxLength="1000" TextMode="MultiLine" Width="500px"  ></asp:TextBox></TD>
</TR>



<tr>
<td colspan="4">
<asp:Panel ID="Pnldategrid" runat="server" TabIndex="8">
<asp:GridView id="gv_row"   runat="server" Width="510px" Font-Size="10px" CssClass="td_cell " GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" BackColor="White">
<Columns>
<asp:TemplateField   Visible="false" HeaderText="LineNo" HeaderStyle-BackColor="#06788B"  >
<ItemTemplate  >
<asp:Label id="lblLineNo" runat="server" Text='<%# Bind("tlineno") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
                                          
                                            
<asp:TemplateField HeaderText="Select"  HeaderStyle-BackColor="#06788B" >
<ItemTemplate>
<asp:CheckBox id="chkDel" runat="server" Width="17px"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField  HeaderText="Ticket Prefix" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txtprefix"   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  

<asp:TemplateField  HeaderText="From Ticket No" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txtfromticketno"   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  

<asp:TemplateField  HeaderText="To Ticket No" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txttoticketno"   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  

<asp:TemplateField  HeaderText="Ticket Suffix" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txtsuffix"   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  


<asp:TemplateField  HeaderText="Ticket Date" HeaderStyle-BackColor="#06788B" >
<ItemTemplate>
<asp:TextBox id="txtTicketDate"  runat="server" CssClass="fiel_input" Width="100px" ValidationGroup="MKE" ></asp:TextBox>&nbsp;
<asp:ImageButton id="ImgBtnFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;
<cc1:MaskedEditValidator id="MaskEdValidDate1" runat="server" CssClass="field_error" Width="1px" Height="20px"
Display="Dynamic" ControlExtender="MskEdExtendDate1" ControlToValidate="txtTicketDate"
InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
TooltipMessage="Input a date in dd/mm/yyyy format" />
<cc1:MaskedEditExtender id="MskEdExtendDate1" runat="server" TargetControlID="txtTicketDate" AcceptNegative="Left" DisplayMoney="Left" MaskType="Date" Mask="99/99/9999" />
<cc1:CalendarExtender id="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFromDate" TargetControlID="txtTicketDate" />

</ItemTemplate>
</asp:TemplateField>  
                                            

                                             
<asp:TemplateField  HeaderText="Remarks" HeaderStyle-BackColor="#06788B">
<ItemTemplate>
<asp:TextBox id="txtRemarks"   runat="server" class="field_input" maxlength="100" ></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>  

                 
                                            
                                                 
</Columns>
<RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
<AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> 
<asp:Button ID="btnAddLines" runat="server" CssClass="btn" tabIndex="9" 
Text="Add Row" />&nbsp;&nbsp<asp:Button id="btnDeleteRow" Width="20%" TabIndex="3"  runat="server" Text="Delete Row" 
                                                                        CssClass="field_button"></asp:Button>
</asp:Panel>
</td>
</tr>


</TBODY></TABLE></TD>

</TR>




<TR><TD align=right colSpan=5>
<input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
height: 9px" type="text" />
<asp:Button id="btnResetAll" tabIndex=20  runat="server" 
Text="Reset All" CssClass="field_button"></asp:Button>
    &nbsp; &nbsp;
        
<asp:Button id="btnSave" tabIndex=12 runat="server" 
Text="Save" CssClass="field_button"></asp:Button>&nbsp;
<asp:Button id="btnExit" tabIndex=13 onclick="btnExit_Click" runat="server" 
Text="Exit" CssClass="field_button"></asp:Button>&nbsp;
<asp:Button id="btnHelp" tabIndex=20 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
</asp:UpdatePanel>

<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
<services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
</asp:ScriptManagerProxy>

<asp:HiddenField ID="hdnExcursionTypeCode"  EnableViewState="true" runat="server" />
<asp:HiddenField ID="hdnExcursionTypeName" EnableViewState="true" runat="server" />
<asp:HiddenField ID="hdnExcursionGroupCode" EnableViewState="true" runat="server" />
</asp:Content>


