<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptComplimentReportSearch.aspx.vb" Inherits="rptComplimentReportSearch" MasterPageFile="~/AccountsMaster.master"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID ="Main" runat="server" >

    <script language ="javascript" type ="text/javascript" >

    function CallWebMethod(methodType) {

        switch (methodType) {
            case "plgrpcode":
                var select = document.getElementById("<%=ddlmarketcode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlmarketname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var selectname2 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                selectname2.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                select1.value = selectname2.options[selectname2.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes1, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName1, ErrorHandler, TimeOutHandler);

                break;
            case "plgrpname":
                var select = document.getElementById("<%=ddlmarketname.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlmarketcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                select1.value = selectname1.options[selectname1.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerCodeAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerCodes1, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetCustomerNameAllListnew(constr, '', '', '', '', '', '', codeid, FillCustomerName1, ErrorHandler, TimeOutHandler);

                break;
            case "plgrpcodeto":
                var select = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "plgrpnameto":
                var select = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "agentcode":
                var select = document.getElementById("<%=ddlcustomercode.ClientID%>");
                var selectname = document.getElementById("<%=ddlcustomername.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname2 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                selectname2.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                select1.value = selectname2.options[selectname2.selectedIndex].text;
                break;
            case "agentname":
                var select = document.getElementById("<%=ddlcustomername.ClientID%>");
                var selectname = document.getElementById("<%=ddlcustomercode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                select1.value = selectname1.options[selectname1.selectedIndex].text;

                break;
            case "agentcodeto":
                var select = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "agentnameto":
                var select = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

        }
    }

    //-------------function for dependency dropdown-----for customer----------------------------------------------------
    function FillCustomerCodes(result) {

        var ddl = document.getElementById("<%=ddlcustomercode .ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);

        }
        ddl.value = "[Select]";


    }

    function FillCustomerName(result) {
        var ddl = document.getElementById("<%=ddlcustomername.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);

        }
        ddl.value = "[Select]";

    }

    function FillCustomerCodes1(result) {
        var ddl1 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");

        RemoveAll(ddl1)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);

            ddl1.options.add(option);

        }

        ddl1.value = "[Select]";

    }

    function FillCustomerName1(result) {

        var ddl1 = document.getElementById("<%=ddlcustomernameto.ClientID%>");

        RemoveAll(ddl1)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);

            ddl1.options.add(option);

        }

        ddl1.value = "[Select]";
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
    //------------------------------------------------------------------------

    //--------Function for diasabled terue/false-----------------------------
    function rbevent(rb1, rb2, Opt, Group) {
        var rb2 = document.getElementById(rb2);
        rb1.checked = true;
        rb2.checked = false;
        switch (Group) {
            case "Market":
                var ddlm1 = document.getElementById("<%=ddlmarketcode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlmarketname.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlmarketcodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlmarketnameto.ClientID%>");
                var lbl1 = document.getElementById("<%=lblmarketcode.ClientID%>");
                var lbl2 = document.getElementById("<%=lblmarketcodeto.ClientID%>");
                var lbl3 = document.getElementById("<%=lblmarketname.ClientID%>");
                var lbl4 = document.getElementById("<%=lblmarketnameto.ClientID%>");

                break;
            case "Customer":
                var ddlm1 = document.getElementById("<%=ddlcustomercode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlcustomername.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlcustomercodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlcustomernameto.ClientID%>");
                var lbl1 = document.getElementById("<%=lblcustomercode.ClientID%>");
                var lbl2 = document.getElementById("<%=lblcustomercodeto.ClientID%>");
                var lbl3 = document.getElementById("<%=lblcustomername.ClientID%>");
                var lbl4 = document.getElementById("<%=lblcustomernameto.ClientID%>");

                break;
           
        }

        if (Opt == 'A') {
            ddlm1.value = '[Select]';
            ddlm2.value = '[Select]';
            ddlm3.value = '[Select]';
            ddlm4.value = '[Select]';

            ddlm1.disabled = true;
            ddlm2.disabled = true;
            ddlm3.disabled = true;
            ddlm4.disabled = true;
//            ddlm1.style.visibility = "hidden";
//            ddlm2.style.visibility = "hidden";
//            ddlm3.style.visibility = "hidden";
//            ddlm4.style.visibility = "hidden";

//            lbl1.style.visibility = "hidden";
//            lbl2.style.visibility = "hidden";
//            lbl3.style.visibility = "hidden";
//            lbl4.style.visibility = "hidden";

        }
        else {
            ddlm1.disabled = false;
            ddlm2.disabled = false;
            ddlm3.disabled = false;
            ddlm4.disabled = false;
//            ddlm1.style.visibility = "visible";
//            ddlm2.style.visibility = "visible";
//            ddlm3.style.visibility = "visible";
//            ddlm4.style.visibility = "visible";

//            lbl1.style.visibility = "visible";
//            lbl2.style.visibility = "visible";
//            lbl3.style.visibility = "visible";
//            lbl4.style.visibility = "visible";

        }

    }

    //-----------------------------------------------------------------------
  
    function ChangeDate() {

        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

        if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
        else { txttdate.value = txtfdate.value; }

    }
   

</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid">
<TBODY>
<TR>
<TD style="TEXT-ALIGN: center" class=" field_input" colSpan=5>
<asp:Label id="lblHeading" runat="server" Text="Complimentary Reports" ForeColor="White" Width="717px" CssClass="field_heading"></asp:Label>
</TD>
</TR>
<TR>
<TD colSpan=3>
<TABLE>
<TBODY>
<TR>
<TD style="WIDTH: 3px" class="field_input">
<asp:Panel id="Panel1" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Date Range"><TABLE><TBODY>
<TR>
<TD style="WIDTH: 65px" class="field_input">
<asp:Label id="lblFrmDate" runat="server" Text="From Date" Width="62px"></asp:Label>
</TD>
<TD style="WIDTH: 15%" class="field_input">
<asp:TextBox id="txtFromDate" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox>
 <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ErrorMessage="MskVFromDate" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator>
</TD><TD style="WIDTH: 65px" class="field_input">
<asp:Label id="lblToDate" runat="server" Text="To Date" Width="62px"></asp:Label>
</TD>
<TD style="WIDTH: 20%" class="field_input">
 <asp:TextBox id="txtToDate" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox><asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="MskToDate"></cc1:MaskedEditValidator>
 </TD>
 <TD style="WIDTH: 20%" class="field_input"></TD><TD style="WIDTH: 20%" class="field_input">
</TD></TR>

    <tr>
        <td class="field_input" style="width: 65px">
            <asp:Label ID="Label3" runat="server" Text="Report Type" Width="62px"></asp:Label></td>
        <td class="field_input"><SELECT style="WIDTH: 165px" id="ddlrpttype" class="field_input" tabIndex=0 onchange="CallWebMethod('sptypecode');" runat="server">
            <OPTION value="0" selected>Brief</option>
            <OPTION value="1">Detailed</option>
        </select>
        </td>
        <td class="field_input">
        </td>
        <td class="field_input" colspan="3">
            </td>
    </tr>
 
</TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE></TD></TR><TR><TD colSpan=3><TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid" class="field_input"><TBODY><TR><TD colSpan=3><asp:Panel id="pnlmarket" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Market"><TABLE><TBODY><TR><TD style="WIDTH: 100px" rowSpan=2><INPUT id="rbmarketall" type=radio CHECKED name="Market" runat="server" /> All&nbsp;</TD><TD style="WIDTH: 100px" rowSpan=2><INPUT id="rbmarketrange" type=radio name="Market" runat="server" />Range</TD><TD style="WIDTH: 54px"><asp:Label id="lblmarketcode" runat="server" Text="Code From" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlmarketcode" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('plgrpcode');" runat="server"></SELECT></TD><TD style="WIDTH: 60px"><asp:Label id="lblmarketname" runat="server" Text="Name From" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlmarketname" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('plgrpname');" runat="server"></SELECT></TD></TR><TR><TD style="WIDTH: 54px"><asp:Label id="lblmarketcodeto" runat="server" Text="Code To" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlmarketcodeto" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('plgrpcodeto');" runat="server"></SELECT></TD><TD style="WIDTH: 60px"><asp:Label id="lblmarketnameto" runat="server" Text="Name To" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlmarketnameto" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('plgrpnameto');" runat="server"></SELECT></TD></TR></TBODY></TABLE></asp:Panel></TD></TR>
<TR><TD colSpan=3><asp:Panel id="pnlcustomer" runat="server" Width="700px" CssClass="field_input" GroupingText="Select Customer"><TABLE><TBODY><TR><TD style="WIDTH: 100px" rowSpan=2><INPUT id="rbCustall" type=radio CHECKED name="Customer" runat="server" /> All&nbsp;</TD><TD style="WIDTH: 100px" rowSpan=2><INPUT id="rbcustrange" type=radio name="Customer" runat="server" />Range</TD><TD style="WIDTH: 53px"><asp:Label id="lblcustomercode" runat="server" Text="Code From" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlcustomercode" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('agentcode');" runat="server"></SELECT></TD><TD style="WIDTH: 62px"><asp:Label id="lblcustomername" runat="server" Text="Name From" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlcustomername" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('agentname');" runat="server"></SELECT></TD></TR><TR><TD style="WIDTH: 53px"><asp:Label id="lblcustomercodeto" runat="server" Text="Code To" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlcustomercodeto" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('agentcodeto');" runat="server"></SELECT></TD><TD style="WIDTH: 62px"><asp:Label id="lblcustomernameto" runat="server" Text="Name To" Width="62px"></asp:Label></TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 196px" id="ddlcustomernameto" class="field_input" disabled tabIndex=0 onchange="CallWebMethod('agentnameto');" runat="server"></SELECT></TD></TR></TBODY></TABLE></asp:Panel></TD></TR>



<TR>
<TD style="TEXT-ALIGN: center" colSpan=2>
<asp:Button id="btnExport" tabIndex=6 onclick="btnExport_Click" runat="server" Text="Export" Width="46px" CssClass="field_button" CausesValidation="False" Visible ="false" ></asp:Button>&nbsp;
<asp:Button id="btnLoadreport" tabIndex=5 onclick="btnLoadreport_Click" runat="server" Text="Load Report" Width="83px" CssClass="field_button"></asp:Button> &nbsp;
 <asp:Button id="btnHelp" tabIndex=7 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
 <cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender> <cc1:CalendarExtender id="ClsExToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnRevDate" Format="dd/MM/yyyy">
    </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender> <cc1:MaskedEditExtender id="MskToDate" runat="server" TargetControlID="txtToDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender>
    
<input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;" type="text" />
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content> 