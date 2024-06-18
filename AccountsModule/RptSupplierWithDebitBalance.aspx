<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptSupplierWithDebitBalance.aspx.vb" Inherits="RptSupplierWithDebitBalance"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >

    function CallWebMethod(methodType) {

        switch (methodType) {

            case "ctrycode":
                var select = document.getElementById("<%=ddlCountrycode.ClientID%>");
                var selectname = document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var selectname2 = document.getElementById("<%=ddlCountryNameto.ClientID%>");
                selectname2.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCountrycodeto.ClientID%>");
                select1.value = selectname2.options[selectname2.selectedIndex].text;
                break;
            case "ctryname":
                var select = document.getElementById("<%=ddlCountryName.ClientID%>");

                var selectname = document.getElementById("<%=ddlCountrycode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlCountrycodeto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCountryNameto.ClientID%>");
                select1.value = selectname1.options[selectname1.selectedIndex].text;
                break;
            case "ctrycodeto":
                var select = document.getElementById("<%=ddlCountrycodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCountryNameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "ctrynameto":
                var select = document.getElementById("<%=ddlCountryNameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCountrycodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "catcode":
                var select = document.getElementById("<%=ddlCategorycode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCategoryname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var selectname2 = document.getElementById("<%=ddlCategorynameto.ClientID%>");
                selectname2.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCategorycodeto.ClientID%>");
                select1.value = selectname2.options[selectname2.selectedIndex].text;

                break;
            case "catname":
                var select = document.getElementById("<%=ddlCategoryname.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCategorycode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlCategorycodeto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCategorynameto.ClientID%>");
                select1.value = selectname1.options[selectname1.selectedIndex].text;

                break;
            case "catcodeto":
                var select = document.getElementById("<%=ddlCategorycodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCategorynameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "catnameto":
                var select = document.getElementById("<%=ddlCategorynameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCategorycodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "citycode":
                var select = document.getElementById("<%=ddlCitycode.ClientID%>");
                var selectname = document.getElementById("<%=ddlCityname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname2 = document.getElementById("<%=ddlCitynameto.ClientID%>");
                selectname2.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCitycodeto.ClientID%>");
                select1.value = selectname2.options[selectname2.selectedIndex].text;
                break;
            case "cityname":
                var select = document.getElementById("<%=ddlCityname.ClientID%>");
                var selectname = document.getElementById("<%=ddlCitycode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlCitycodeto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlCitynameto.ClientID%>");
                select1.value = selectname1.options[selectname1.selectedIndex].text;

                break;
            case "citycodeto":
                var select = document.getElementById("<%=ddlCitycodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCitynameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "citynameto":
                var select = document.getElementById("<%=ddlCitynameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlCitycodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partycode":
                var select = document.getElementById("<%=ddlSuppliercode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSuppliername.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var select1 = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppliername.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSuppliercode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var select1 = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;
                break;
            case "partycodeto":
                var select = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partynameto":
                var select = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;



        }
    }

    //-----------------------------------------------------------------
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
            case "Category":
                var ddlm1 = document.getElementById("<%=ddlCategorycode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlCategoryname.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlCategorycodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlCategorynameto.ClientID%>");
                break;
            case "City":
                var ddlm1 = document.getElementById("<%=ddlCitycode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlCityname.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlCitycodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlCitynameto.ClientID%>");
                break;
            case "Supplier":
                var ddlm1 = document.getElementById("<%=ddlSuppliercode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlSuppliername.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
                break;
            case "Country":
                var ddlm1 = document.getElementById("<%=ddlCountrycode.ClientID%>");
                var ddlm2 = document.getElementById("<%=ddlCountryName.ClientID%>");
                var ddlm3 = document.getElementById("<%=ddlCountrycodeto.ClientID%>");
                var ddlm4 = document.getElementById("<%=ddlCountryNameto.ClientID%>");
                break;

        }

        if (Opt == 'A') {
            ddlm1.disabled = true;
            ddlm2.disabled = true;
            ddlm3.disabled = true;
            ddlm4.disabled = true;
        }
        else {
            ddlm1.disabled = false;
            ddlm2.disabled = false;
            ddlm3.disabled = false;
            ddlm4.disabled = false;
        }

    }

    function fillSup(ddlSup) {
        var sup = document.getElementById(ddlSup);
        var ddlm1 = document.getElementById("<%=ddlSuppliercode.ClientID%>");
        var ddlm2 = document.getElementById("<%=ddlSuppliername.ClientID%>");
        var ddlm3 = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
        var ddlm4 = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value


        if (sup.value == 'S') {
            sqlstr = "select partycode, partyname from partymast where active=1 order by partycode";
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupCodes, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupCodes, ErrorHandler, TimeOutHandler);
            sqlstr = "select partyname, partycode from partymast where active=1 order by partyname";
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupNames, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupNames, ErrorHandler, TimeOutHandler);
        }
        else if (sup.value == 'A') {
            sqlstr = "select supagentcode, supagentname from supplier_agents where active=1 order by supagentcode";
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupCodes, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupCodes, ErrorHandler, TimeOutHandler);
            sqlstr = "select supagentname, supagentcode from supplier_agents where active=1 order by supagentname";
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupNames, ErrorHandler, TimeOutHandler);
            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupNames, ErrorHandler, TimeOutHandler);
        }
        else {

        }

    }

    function FillFromSupCodes(result) {
        var ddl = document.getElementById("<%=ddlSuppliercode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillFromSupNames(result) {
        var ddl = document.getElementById("<%=ddlSuppliername.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillToSupCodes(result) {
        var ddl = document.getElementById("<%=ddlSuppliercodeto.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillToSupNames(result) {
        var ddl = document.getElementById("<%=ddlSuppliernameto.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }



</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
      <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid"><TBODY><TR><TD style="TEXT-ALIGN: center" class=" field_heading" colSpan=5>Supplier With Debit Balance</TD></TR><TR><TD colSpan=3><TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid" class="field_input"><TBODY><TR><TD colSpan=4>Select Supplier/ Supplier Agent <SELECT style="WIDTH: 125px" id="ddlSupType" class="drpdown" runat="server"> <OPTION value="S" selected>Supplier</OPTION> <OPTION value="A">SupplierAgent</OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 338px" colSpan=3><asp:Panel id="Panel1" runat="server" CssClass="field_input" Width="475px" GroupingText="Select Date "><TABLE><TR><TD style="WIDTH: 59px" class="field_input">As Date On</TD><TD class="field_input"><asp:TextBox id="txtFromDate" runat="server" Width="80px" CssClass="fiel_input"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ControlExtender="MskFromDate" ControlToValidate="txtFromDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR></TABLE></asp:Panel></TD><TD colSpan=1></TD></TR><TR><TD style="WIDTH: 338px" colSpan=3><asp:Panel id="Panel3" runat="server" CssClass="td_cell" Width="475px" GroupingText="Select Supplier / Supplier Agent"><TABLE style="WIDTH: 457px"><TBODY><TR><TD style="WIDTH: 55px" class="td_cell"><INPUT id="rbSupall" type=radio CHECKED name="Supplier" runat="server" /> All</TD><TD style="WIDTH: 54px" class="td_cell">Code From</TD><TD style="WIDTH: 118px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlSuppliercode" class="drpdown" disabled onchange="CallWebMethod('partycode');" runat="server"></SELECT> </TD><TD style="WIDTH: 59px" class="td_cell">&nbsp;Name From</TD><TD class="td_cell"><SELECT style="WIDTH: 141px" id="ddlSuppliername" class="drpdown" disabled onchange="CallWebMethod('partyname');" runat="server"></SELECT> </TD></TR><TR><TD style="WIDTH: 55px; HEIGHT: 22px" class="td_cell"><INPUT id="rbSuprange" type=radio name="Supplier" runat="server" /> Range</TD><TD style="WIDTH: 54px; HEIGHT: 22px" class="td_cell">Code To </TD><TD style="WIDTH: 118px; HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlSuppliercodeto" class="drpdown" disabled onchange="CallWebMethod('partycodeto');" runat="server"></SELECT> </TD><TD style="WIDTH: 59px; HEIGHT: 22px" class="td_cell">&nbsp;Name To</TD><TD style="HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 141px" id="ddlSuppliernameto" class="drpdown" disabled onchange="CallWebMethod('partynameto');" runat="server"></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD><TD colSpan=1><asp:Panel id="Panel2" runat="server" CssClass="td_cell" Width="475px" GroupingText="Select Category"><TABLE style="WIDTH: 461px"><TBODY><TR><TD style="WIDTH: 61px" class="td_cell"><INPUT id="rbCatall" type=radio CHECKED name="Category" runat="server" /> All</TD><TD style="WIDTH: 59px" class="td_cell">Code From </TD><TD style="WIDTH: 112px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlCategorycode" class="drpdown" disabled onchange="CallWebMethod('catcode');" runat="server"></SELECT> </TD><TD style="WIDTH: 56px" class="td_cell">Name From </TD><TD class="td_cell"><SELECT style="WIDTH: 141px" id="ddlCategoryname" class="drpdown" disabled onchange="CallWebMethod('catname');" runat="server"></SELECT> </TD></TR><TR><TD style="WIDTH: 61px; HEIGHT: 22px" class="td_cell"><INPUT id="rbCatrange" type=radio name="Category" runat="server" /> Range</TD><TD style="WIDTH: 59px; HEIGHT: 22px" class="td_cell">&nbsp;Code To</TD><TD style="WIDTH: 112px; HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlCategorycodeto" class="drpdown" disabled onchange="CallWebMethod('catcodeto');" runat="server"></SELECT> </TD><TD style="WIDTH: 56px; HEIGHT: 22px" class="td_cell">Name To </TD><TD style="HEIGHT: 22px" class="td_cell"><SELECT style="WIDTH: 141px" id="ddlCategorynameto" class="drpdown" disabled onchange="CallWebMethod('catnameto');" runat="server"></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 338px" colSpan=3><asp:Panel id="pnlCountry" runat="server" CssClass="field_input" Width="475px" GroupingText="Select Country"><TABLE><TR><TD style="WIDTH: 71px"><INPUT id="rbcountryall" type=radio CHECKED name="Country" runat="server" /> All</TD><TD style="WIDTH: 55px">Code From</TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 118px" id="ddlCountrycode" class="drpdown" disabled tabIndex=0 onchange="CallWebMethod('ctrycode');" runat="server"></SELECT></TD><TD style="WIDTH: 60px">Name From</TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 141px" id="ddlCountryName" class="drpdown" disabled tabIndex=0 onchange="CallWebMethod('ctryname');" runat="server"></SELECT></TD></TR><TR><TD style="WIDTH: 71px"><INPUT id="rbcountryrange" type=radio name="Country" runat="server" /> Range</TD><TD style="WIDTH: 55px">Code To</TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 118px" id="ddlCountrycodeto" class="drpdown" disabled tabIndex=0 onchange="CallWebMethod('ctrycodeto');" runat="server"></SELECT></TD><TD style="WIDTH: 60px">Name To</TD><TD style="WIDTH: 100px"><SELECT style="WIDTH: 141px" id="ddlCountrynameto" class="drpdown" disabled tabIndex=0 onchange="CallWebMethod('ctrynameto');" runat="server"></SELECT></TD></TR></TABLE></asp:Panel></TD><TD colSpan=1><asp:Panel id="Panel6" runat="server" CssClass="td_cell" Width="475px" GroupingText="Select City"><TABLE style="WIDTH: 458px"><TBODY><TR><TD style="WIDTH: 60px" class="td_cell"><INPUT id="rbCityall" type=radio CHECKED name="Market" runat="server" /> All</TD><TD style="WIDTH: 56px" class="td_cell">&nbsp;Code From</TD><TD style="WIDTH: 110px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlCitycode" class="drpdown" disabled onchange="CallWebMethod('citycode');" runat="server"></SELECT> </TD><TD style="WIDTH: 56px" class="td_cell">Name From</TD><TD class="td_cell"><SELECT style="WIDTH: 141px" id="ddlCityname" class="drpdown" disabled onchange="CallWebMethod('cityname');" runat="server"></SELECT> </TD></TR><TR><TD style="WIDTH: 60px" class="td_cell"><INPUT id="rbCityrange" type=radio name="Market" runat="server" /> Range</TD><TD style="WIDTH: 56px" class="td_cell">Code To </TD><TD style="WIDTH: 110px" class="td_cell"><SELECT style="WIDTH: 118px" id="ddlCitycodeto" class="drpdown" disabled onchange="CallWebMethod('citycodeto');" runat="server"></SELECT> </TD><TD style="WIDTH: 56px" class="td_cell">Name To </TD><TD class="td_cell"><SELECT style="WIDTH: 141px" id="ddlCitynameto" class="drpdown" disabled onchange="CallWebMethod('citynameto');" runat="server"></SELECT> </TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR><TD style="WIDTH: 338px" colSpan=3>Report Filter &nbsp; &nbsp; <SELECT style="WIDTH: 153px" id="Select1" class="drpdown" tabIndex=0 onchange="CallWebMethod('sptypecode');" runat="server"><OPTION value="Code" selected>Code</OPTION><OPTION value="Name">Name</OPTION></SELECT></TD><TD colSpan=1></TD></TR><TR><TD colSpan=3></TD><TD colSpan=1></TD></TR><TR><TD colSpan=3>&nbsp;Currency &nbsp; &nbsp; &nbsp;&nbsp; <SELECT style="WIDTH: 153px" id="ddlcurrency" class="drpdown" tabIndex=0 onchange="CallWebMethod('sptypecode');" runat="server"></SELECT></TD><TD colSpan=1></TD></TR><TR><TD style="WIDTH: 338px" colSpan=3>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="TEXT-ALIGN: right" colSpan=1>&nbsp; 
        <asp:Button id="btnLoadreport" tabIndex=5 runat="server" Text="Load Report" 
            CssClass="btn"></asp:Button>&nbsp;
         <asp:Button id="btnExit" tabIndex=6 onclick="btnExit_Click" runat="server" 
            Text=" Exit" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;
         <asp:Button id="btnhelp" tabIndex=39 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="btn"></asp:Button></TD></TR></TBODY></TABLE><cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender><cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
    </cc1:MaskedEditExtender><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

