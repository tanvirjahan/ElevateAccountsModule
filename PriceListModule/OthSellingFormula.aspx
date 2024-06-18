<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OthSellingFormula.aspx.vb" Inherits="PriceListModule_OthSellingFormula" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<script language="javascript" type="text/javascript" >

    function CallWebMethod(methodType) {
        switch (methodType) {
            case "sellcode":
                var select = document.getElementById("<%=ddlSellCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCodeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var sqlstr = "select Distinct  currmast.currcode from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 and sellmast.sellcode='" + codeid + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrCodes, ErrorHandler, TimeOutHandler);
                sqlstr = "select Distinct  currmast.currname from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 and sellmast.sellcode='" + codeid + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrNames, ErrorHandler, TimeOutHandler);

                break;
            case "sellname":
                var select = document.getElementById("<%=ddlCodeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSellCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var sqlstr = "select Distinct  currmast.currcode from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 and sellmast.sellcode='" + codeid + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrCodes, ErrorHandler, TimeOutHandler);
                sqlstr = "select Distinct  currmast.currname from  currmast inner join sellmast on currmast.currcode=sellmast.currcode   where currmast.active=1 and sellmast.sellcode='" + codeid + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrNames, ErrorHandler, TimeOutHandler);

                break;

            case "calcode":
                var select = document.getElementById("<%=ddlCalculateFrm.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlCalculateName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "calname":
                var select = document.getElementById("<%=ddlCalculateName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlCalculateFrm.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;
            case "grpcode":
                var select = document.getElementById("<%=ddlGrpCode.ClientID %>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGrpName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

            case "grpname":
                var select = document.getElementById("<%=ddlGrpName.ClientID %>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGrpCode.ClientID%>");
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


    function FillCurrCodes(result) {
        var txtcur = document.getElementById("<%=txtCurrencyCode.ClientID%>");
        txtcur.value = result;
    }
    function FillCurrNames(result) {
        var txtcurname = document.getElementById("<%=txtCurrencyName.ClientID%>");
        txtcurname.value = result;
    }


    function FormValidation(state) {
        if ((document.getElementById("<%=ddlSellCode.ClientID%>").value == "[Select]") || (document.getElementById("<%=txtCurrencyCode.ClientID%>").value == "") || (document.getElementById("<%=ddlOperator.ClientID%>").value == "[Select]") || (document.getElementById("<%=txtFrmValue.ClientID%>").value == 0) || (document.getElementById("<%=txtFrmValue.ClientID%>").value == "") || (document.getElementById("<%=txtFormula.ClientID%>").value == "")) {
            if (document.getElementById("<%=ddlSellCode.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlSellCode.ClientID%>").focus();
                alert("Please Select Selling Code");
                return false;
            }

            else if (document.getElementById("<%=txtCurrencyCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCurrencyCode.ClientID%>").focus();
                alert("Please Select Currency");
                return false;
            }

            else if (document.getElementById("<%=ddlOperator.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlOperator.ClientID%>").focus();
                alert("Please Select Operator");
                return false;
            }

            else if (document.getElementById("<%=txtFrmValue.ClientID%>").value <= 0) {
                document.getElementById("<%=txtFrmValue.ClientID%>").focus();
                alert("Value Field cannot be zero or Negative");
                return false;
            }

            else if (document.getElementById("<%=txtFrmValue.ClientID%>").value == "") {
                document.getElementById("<%=txtFrmValue.ClientID%>").focus();
                alert("Please Enter The value Field");
                return false;
            }

            else if (document.getElementById("<%=txtFormula.ClientID%>").value == "") {
                document.getElementById("<%=txtFormula.ClientID%>").focus();
                alert("Formula field cannot be blank");
                return false;
            }

        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save  Selling Formula?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update  Selling Formula?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete  Selling Formula?') == false) return false; }
        }
    }


    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 45 && charCode < 58)) {
            return true;
        }
        return false;
    }

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

</script>

<asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY>
<TR><TD class="field_heading" align=center colSpan=5>
<asp:Label id="lblHeading" runat="server" Text="Add New Other Selling Formula" CssClass="field_heading" Width="889px"></asp:Label></TD></TR>

<TR><TD class="td_cell">Code <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD class="td_cell" align=left><SELECT style="WIDTH: 200px" id="ddlSellCode" class="field_input" tabIndex=1 onchange="CallWebMethod('sellcode')" runat="server">
 <OPTION selected></OPTION></SELECT></TD>
 <TD style="WIDTH: 99px" class="td_cell" align=left>&nbsp;Name&nbsp;<SPAN style="COLOR: red" class="td_cell">*&nbsp;</SPAN></TD>
 <TD style="WIDTH: 303px" class="td_cell"><SELECT style="WIDTH: 300px" id="ddlCodeName" class="field_input" tabIndex=2 onchange="CallWebMethod('sellname')" runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD style="WIDTH: 138px" class="td_cell"></TD></TR>
 
 <TR><TD class="td_cell">Currency&nbsp;</TD><TD class="td_cell" align=left>
 <INPUT style="WIDTH: 194px" id="txtCurrencyCode" class="field_input" tabIndex=3 readOnly type=text runat="server" /></TD>
 <TD class="td_cell" align=left>&nbsp;currency <SPAN class="td_cell">&nbsp;Name</SPAN></TD><TD class="td_cell" align=left>
 <INPUT style="WIDTH: 294px" id="txtCurrencyName" class="field_input" tabIndex=4 readOnly type=text maxLength=20 runat="server" /></TD>
 <TD style="WIDTH: 138px" class="td_cell" align=left></TD></TR>

 <TR><TD class="td_cell">Group Code <SPAN style="COLOR: #ff0000">*</SPAN></TD>
 <TD><SELECT style="WIDTH: 200px" id="ddlGrpCode" class="field_input" tabIndex=5 onchange="CallWebMethod('grpcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD class="td_cell">
    <asp:Label id="Label3" runat="server" Text="Group Name" CssClass="field_Caption" Width="145px"></asp:Label></TD>
 <TD><SELECT style="WIDTH: 300px" id="ddlGrpName" class="field_input" tabIndex=6 onchange="CallWebMethod('grpname')" runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD style="WIDTH: 138px" class="td_cell"></TD></TR>
 
 <TR><TD class="td_cell">Calculate From <SPAN style="COLOR: #ff0000">*</SPAN></TD>
 <TD><SELECT style="WIDTH: 200px" id="ddlCalculateFrm" class="field_input" tabIndex=5 onchange="CallWebMethod('calcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD class="td_cell">
    <asp:Label id="Label1" runat="server" Text="Calculate From Name" 
        CssClass="field_Caption" Width="145px"></asp:Label></TD>
 <TD><SELECT style="WIDTH: 300px" id="ddlCalculateName" class="field_input" tabIndex=6 onchange="CallWebMethod('calname')" runat="server"> <OPTION selected></OPTION></SELECT></TD>
 <TD style="WIDTH: 138px" class="td_cell">&nbsp;(Leave Blank for calculating from cost)</TD></TR>
 
 <TR><TD class="td_cell">
 <asp:Label id="Label2" runat="server" Text="Formula Currency From" CssClass="field_Caption" Width="126px"></asp:Label> </TD><TD class="td_cell">
 <SELECT style="WIDTH: 200px" id="ddlFormulafrom" class="field_input" tabIndex=7 onchange="CallWebMethod('calcode')" runat="server"> 
 <OPTION value="Selling Type">Selling Type</OPTION><OPTION value="Supplier" selected>Supplier</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD><TD class="td_cell"></TD><TD class=" " colSpan=1></TD><TD style="WIDTH: 138px" class=" " colSpan=1></TD></TR><TR><TD class="td_cell">Operator <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD><asp:DropDownList id="ddlOperator" tabIndex=8 runat="server" CssClass="field_input" Width="200px"><asp:ListItem>[Select]</asp:ListItem>
</asp:DropDownList></TD><TD><asp:Button id="btnaddtostring1" tabIndex=9 
            onclick="btnaddtostring1_Click" runat="server" Text="Add to String" 
            CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 303px"></TD><TD style="WIDTH: 138px"></TD></TR>
            
 <TR><TD style="WIDTH: 85px" class="td_cell">Value <SPAN style="COLOR: #ff0000">*</SPAN></TD>
 <TD><INPUT style="WIDTH: 194px; TEXT-ALIGN: right" id="txtFrmValue" tabIndex=10 onkeypress="return checkNumber()" type=text runat="server" /></TD><TD>
    <asp:Button id="btnAddtostring2" tabIndex=11 onclick="btnAddtostring2_Click" 
        runat="server" Text="Add to String" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 303px"></TD>
        <TD style="WIDTH: 138px"></TD></TR><TR><TD style="WIDTH: 85px" class="td_cell">Formula <SPAN style="COLOR: #ff0000">*</SPAN></TD><TD colSpan=2><INPUT style="WIDTH: 294px; TEXT-ALIGN: right" id="txtFormula" tabIndex=12 readOnly type=text runat="server" /></TD><TD style="WIDTH: 303px" colSpan=1></TD><TD style="WIDTH: 138px" colSpan=1></TD></TR><TR><TD>
    <asp:Button id="btnClear" tabIndex=15 onclick="btnClear_Click" runat="server" 
        Text="Clear Fomula" CssClass="field_button"></asp:Button></TD><TD colSpan=2>
        <asp:Button id="btnSave" tabIndex=13  runat="server" 
            Text="Save" CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button id="btnCancel" tabIndex=14 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Button id="btnhelp" tabIndex=15 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 303px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD><TD style="WIDTH: 138px"></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

</asp:Content>

