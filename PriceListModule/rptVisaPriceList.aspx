<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptVisaPricelist.aspx.vb" Inherits="rptVisaPricelist"  MasterPageFile="~/PriceListMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
    

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<%--<script language="javascript" src="js\date-picker.js"></script>  --%>
    <%--<script language="javascript" src="js\datefun.js"></script>--%>
   <%--//    (document.getElementById("<%=txtFromDate.ClientID%>").value=="")||(document.getElementById("<%=txtToDate.ClientID%>").value=="") )--%>

<script type="text/javascript">
    /*<!--
    WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
    // -->*/
</script>
<script language="javascript" type="text/javascript" >
    function CallWebMethod(methodType) {
        switch (methodType) {
            case "othergroupcode":
                var select = document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
                var party = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlOtherGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetotherServiceCatnew(constr, party, FillList, ErrorHandler, TimeOutHandler);
                break;
            case "othergroupname":
                var select = document.getElementById("<%=ddlOtherGroupName.ClientID%>");
                var party = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetotherServiceCatnew(constr, party, FillList, ErrorHandler, TimeOutHandler);
                break;

            case "marketcode":
                var select = document.getElementById("<%=ddlMarketCode.ClientID%>");
                var plgrp = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "marketname":
                var select = document.getElementById("<%=ddlMarketName.ClientID%>");
                var plgrp = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "sellcode":
                var select = document.getElementById("<%=ddlSellingCode.ClientID%>");
                var sellcat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSellingName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;
            case "sellname":
                var select = document.getElementById("<%=ddlSellingName.ClientID%>");
                var sellcat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSellingCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }
    function FillList(result) {
        var lstleft = document.getElementById("<%=lstOthCatLeft.ClientID%>");
        var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
        var hdnServiceCat = document.getElementById("<%=hdnServiceCat.ClientID%>");
        for (var j = lstright.length - 1; j >= 0; j--) {
            lstright.remove(j);
        }
        for (var j = lstleft.length - 1; j >= 0; j--) {
            lstleft.remove(j);
        }
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            lstleft.options.add(option);
        }
        hdnServiceCat.value = ''
    }





    function FillSellCatCodes(result) {
        var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSellCatNames(result) {
        var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
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


    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
            return false;
        }

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

    function ChangeDate() {

        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

        if (txtfdate.value == '') {
            alert("Enter From Date.");
            txtfdate.focus();
        }

        if (txtfdate.value != '') {
            var datearray = txtfdate.value.split("/");
            var mn = datearray[1];
            mn = mn * 1;
            mn = mn - 1;
            var yDate = new Date();
            yDate.setDate(datearray[0]);
            yDate.setMonth(mn);
            yDate.setYear(datearray[2]);

            var todt = DateAdd('d', 30, yDate);
            var nyr = todt.getFullYear();
            var nmn = todt.getMonth();
            nmn = nmn * 1;
            nmn = nmn + 1;
            nmn = Right('0' + nmn, 2);
            var ndy = Right('0' + todt.getDate(), 2);
            txttdate.value = ndy + "/" + nmn + "/" + nyr;
        }
        // else {ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);}
    }
    function FillToDate(result) {
        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
        txttdate.value = result;
    }

    function lstaddall() {
        var ddlOtherGroupCode = document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
        if (ddlOtherGroupCode.value != '[Select]') {
            var hdnServiceCat = document.getElementById("<%=hdnServiceCat.ClientID%>");
            var lstleft = document.getElementById("<%=lstOthCatLeft.ClientID%>");
            var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
            hdnServiceCat.value = '';
            for (var j = 0; j < lstright.length; j++) {
                lstright.remove(j);
            }
            var len = parseInt(lstleft.length);
            if (len > 12) {
                len = 12
            }
            for (var j = 0; j < len; j++) {

                var option = new Option(lstleft.options[0].text, lstleft.options[0].value);
                lstright.options.add(option);
                //hdnServiceCat.value=hdnServiceCat.value+'$'+lstleft.options[0].value
                lstleft.remove(0);
            }
            hdnServiceCat.value = '';
            for (var i = 0; i < lstright.length; i++) {
                hdnServiceCat.value = hdnServiceCat.value + '$' + lstright.options[i].value;
            }
        }
        else {
            alert('Select service group');
        }
    }
    function lstadd() {
        var ddlOtherGroupCode = document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
        if (ddlOtherGroupCode.value != '[Select]') {
            var hdnServiceCat = document.getElementById("<%=hdnServiceCat.ClientID%>");
            var lstleft = document.getElementById("<%=lstOthCatLeft.ClientID%>");
            var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
            if (lstright.length > 11) {
                alert('You are not allowed to select more than 12.');
            }
            else {
                var j = lstleft.selectedIndex;
                var option = new Option(lstleft.options[lstleft.selectedIndex].text, lstleft.options[lstleft.selectedIndex].value);
                lstright.options.add(option);

                lstleft.remove(j);
            }
            hdnServiceCat.value = '';
            for (var i = 0; i < lstright.length; i++) {
                hdnServiceCat.value = hdnServiceCat.value + '$' + lstright.options[i].value;
            }
        }
        else {
            alert('Select service group');
        }
    }
    function lstremove() {
        var hdnServiceCat = document.getElementById("<%=hdnServiceCat.ClientID%>");
        var lstleft = document.getElementById("<%=lstOthCatLeft.ClientID%>");
        var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
        var j = lstright.selectedIndex;
        // var strcat =new Array();
        //strcat=hdnServiceCat.split('$');
        var option = new Option(lstright.options[lstright.selectedIndex].text, lstright.options[lstright.selectedIndex].value);
        lstleft.options.add(option);
        lstright.remove(j);
        hdnServiceCat.value = '';
        for (var i = 0; i < lstright.length; i++) {
            hdnServiceCat.value = hdnServiceCat.value + '$' + lstright.options[i].value;
        }
        //alert(hdnServiceCat.value)
    }
    function lstremoveall() {
        var hdnServiceCat = document.getElementById("<%=hdnServiceCat.ClientID%>");
        hdnServiceCat.value = '';
        var lstleft = document.getElementById("<%=lstOthCatLeft.ClientID%>");
        var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
        var select = document.getElementById("<%=ddlOtherGroupName.ClientID%>");
        var party = select.options[select.selectedIndex].value;
        var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        constr = connstr.value

        for (var j = 0; j < lstright.length; j++) {
            lstright.remove(j);
        }
        ColServices.clsServices.GetotherServiceCatnew(constr, party, FillList, ErrorHandler, TimeOutHandler);
    }

    function ValidateData() {
        var ddlOtherGroupCode = document.getElementById("<%=ddlOtherGroupCode.ClientID%>");
        var ddlOtherGroupName = document.getElementById("<%=ddlOtherGroupName.ClientID%>");
        var ddlMarketCode = document.getElementById("<%=ddlMarketCode.ClientID%>");
        var ddlMarketName = document.getElementById("<%=ddlMarketName.ClientID%>");
        var ddlSellingCode = document.getElementById("<%=ddlSellingCode.ClientID%>");
        var ddlSellingName = document.getElementById("<%=ddlSellingName.ClientID%>");
        if (ddlOtherGroupCode.value == '[Select]' || ddlOtherGroupName.value == '[Select]') {
            alert('Please Select Group');
            return false;
        }
        if (ddlMarketCode.value == '[Select]' || ddlMarketName.value == '[Select]') {
            alert('Please Select Market');
            return false;
        }
        if (ddlSellingCode.value == '[Select]' || ddlSellingName.value == '[Select]') {
            alert('Please Select Selling Type');
            return false;
        }
        /*var lstright = document.getElementById("<%=lstOthCatRight.ClientID%>");
        if(lstright.length < 1)
        {
        alert('Please Select Service Category');
        return false;
        }*/
        return true;
    }
    
</script> 

    <table>
        <tr>
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Visa Price List Report</td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
  
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD><asp:Label id="Label2" runat="server" 
        Text="Group Code" CssClass="td_cell" Width="154px"></asp:Label></TD><TD>
<SELECT style="WIDTH: 170px" id="ddlOtherGroupCode" class="drpdown" tabIndex=1 onchange="CallWebMethod('othergroupcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD>
    <asp:Label id="Label3" runat="server" Text="Group Name" 
        CssClass="td_cell" Width="165px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlOtherGroupName" class="drpdown" tabIndex=2 onchange="CallWebMethod('othergroupname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlMarketCode" class="drpdown" tabIndex=3 onchange="CallWebMethod('marketcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD dir="ltr"><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlMarketName" class="drpdown" tabIndex=4 onchange="CallWebMethod('marketname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="lblSellingCategoryCode" runat="server" Text="Selling Type Code" CssClass="td_cell" Width="123px"></asp:Label></TD><TD><SELECT style="WIDTH: 170px" id="ddlSellingCode" class="drpdown" tabIndex=5 onchange="CallWebMethod('sellcode')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD><TD><asp:Label id="lblsellingcategoryname" runat="server" Text="Selling Type Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD><SELECT style="WIDTH: 237px" id="ddlSellingName" class="drpdown" tabIndex=6 onchange="CallWebMethod('sellname')" runat="server" Visible="true"> <OPTION selected></OPTION></SELECT> </TD></TR><TR><TD><asp:Label id="Label4" runat="server" Text="Season Code" CssClass="td_cell"></asp:Label></TD><TD><asp:DropDownList id="ddlseas1code" tabIndex=7 runat="server" CssClass="drpdown" Width="170px" OnSelectedIndexChanged="ddlseas1code_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD><asp:Label id="Label5" runat="server" Text="Season Name" CssClass="td_cell" Width="124px"></asp:Label></TD><TD><asp:DropDownList id="ddlseas1name" tabIndex=8 runat="server" CssClass="drpdown" Width="237px" OnSelectedIndexChanged="ddlseas1name_SelectedIndexChanged"></asp:DropDownList></TD></TR>
    <tr>
        <td>
            <asp:Label ID="lblapprove" runat="server" CssClass="td_cell" Text="Approved/Unapproved"
                Width="135px"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" Width="171px">
                <asp:ListItem Value="2">All</asp:ListItem>
                <asp:ListItem Value="1">Approve</asp:ListItem>
                <asp:ListItem Value="0">Unapprove</asp:ListItem>
            </asp:DropDownList></td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <TR><TD><asp:Label id="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtFromDate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="9"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label></TD><TD><asp:TextBox id="txtToDate" runat="server" CssClass="fiel_input" Width="80px" TabIndex="10"></asp:TextBox><asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD></TR>
    <tr>
        <td style="height: 49px">
            <asp:Label ID="Label1" runat="server" CssClass="td_cell" Text="Service Category"></asp:Label></td>
        <td style="height: 49px">
            <asp:ListBox ID="lstOthCatLeft" runat="server" Height="100px" Width="170px" CssClass="field_input" TabIndex="11"></asp:ListBox></td>
        <td align="center" style="height: 49px" valign="middle">
             <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 23px; height: 25px">
                        <input id="btnAddAll" runat ="server" type="button" value=">>" onclick="lstaddall();" style="width: 75px" class="btn" /></td>
                </tr>
                <tr>
                    <td style="width: 23px; height: 25px">
                        <input id="btnAdd" runat ="server" type="button" value=">" onclick="lstadd();" style="width: 75px" class="btn"/></td>
                </tr>
                <tr>
                    <td style="width: 23px; height: 25px">
                        <input id="btnRemove" type="button" runat ="server" value="<" onclick="lstremove();" style="width: 75px" class="btn"/></td>
                </tr>
                <tr>
                    <td style="width: 23px; height: 25px">
                        <input id="btnRemoveAll" runat ="server"  type="button" value="<<" onclick="lstremoveall();" style="width: 75px" class="btn"/></td>
                </tr>
            </table>
        </td>
        <td style="height: 49px">
            <asp:ListBox ID="lstOthCatRight" runat="server" Height="100px" Width="170px" CssClass="field_input" TabIndex="16"></asp:ListBox></td>
    </tr>
    <TR><TD style="TEXT-ALIGN: center" colSpan=4>
    <asp:Button id="BtnClear" tabIndex=17 runat="server" Text="Clear" CssClass="btn"></asp:Button>&nbsp;
    <asp:Button id="BtnPrint" tabIndex=18 runat="server" Text="Load Report" CssClass="btn" OnClientClick="return ValidateData();"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=19 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="btn"></asp:Button>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
            height: 9px" type="text" /></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
                                    <asp:HiddenField ID="hdnServiceCat" runat="server" />
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
    &nbsp; &nbsp; &nbsp; &nbsp;
    <br />

</asp:Content>