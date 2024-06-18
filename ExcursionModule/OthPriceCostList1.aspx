<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OthPriceCostList1.aspx.vb" Inherits="TransportModule_OthPriceList1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language ="javascript" type ="text/javascript" >

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

    function CallWebMethod(methodType) {
        switch (methodType) {

            case "sptypecode":
                var select = document.getElementById("<%=ddlSPType.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;

                 var codetext =select.options[select.selectedIndex].text;

                var selectname = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
               
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSellingCurrCodecost(constr, selectname.value, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
                 //ColServices.clsServices.GetSellingCurrCodeexc(constr, codetext, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
                //ColServices.clsServices.GetSellingCurrNameexc(constr, codeid, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                break;
            case "sptypename":
                var select = document.getElementById("<%=ddlSPTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSPType.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var codetext = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSellingCurrCodecost(constr, codeid, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
                //ColServices.clsServices.GetSellingCurrCodeexc(constr, codeid, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);
               // ColServices.clsServices.GetSellingCurrNameexc(constr, codetext, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
               
                break;


            case "exccode":
                var select = document.getElementById("<%=ddlexccode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text ;
                var selectname = document.getElementById("<%=ddlexcname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                break;

            case "excname":
                var select = document.getElementById("<%=ddlexcname.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexccode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;
            

            case "groupcd":
                var select = document.getElementById("<%=ddlGroupCode.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

            case "groupnm":
                var select = document.getElementById("<%=ddlGroupName.ClientID%>");
                codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                break;

        }
    }


    function FillSupplierCodes(result) {
        var ddl = document.getElementById("<%=ddlSPType.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSupplierNames(result) {
        var ddl = document.getElementById("<%=ddlSPTypeName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }


    function FillSupplierCurrCode(result) {


        if (result.length > 0) {
            var txt = document.getElementById("<%=txtCurrCode.ClientID%>");
            txt.value = result[0].ListValue;
            var txt1 = document.getElementById("<%=txtCurrName.ClientID%>");
            txt1.value = result[0].ListText;
        }

    }




//    function FillSupplierCurrCode(result) {
//        var txt = document.getElementById("<%=txtCurrCode.ClientID%>");
//        txt.value = result;
//    }

    function FillSupplierCurrName(result) {
        var txt = document.getElementById("<%=txtCurrName.ClientID%>");
        txt.value = result;
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
        if (document.getElementById("<%=ddlSPType.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSPType.ClientID%>").focus();
            alert("Select Selling Type Code.");
            return false;
        }

        else if (document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlGroupCode.ClientID%>").focus();
            alert("Select Group Code.");
            return false;

            if ((document.getElementById("<%=ddlexccode.ClientID%>").style.visibility = "hidden")){

            
                    if (document.getElementById("<%=ddlexccode.ClientID%>").value == "[Select]") {
//                    document.getElementById("<%=ddlexccode.ClientID%>").focus();
//                    alert("Select Airport Code.");
//                    return false;
                    }
            }
        }

        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to generate Excursion Cost Price list?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to generate Excursion Cost Price list?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to generate Excursion Cost  Price list?') == false) return false; }

        }
    }



    function ddlSPType_onclick() {

    }

</script>


 <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
   <TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid">
     <TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4>
          <asp:Label id="lblHeading" runat="server" Text="Add New Excursion Cost price List" CssClass="field_heading" Width="744px"></asp:Label></TD></TR>
     
     <tr><td style="width: 201px" class="td_cell" align=left>
        <SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px">
            <INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD>
            <TD style="WIDTH: 190px" class="td_cell" align=left></TD>
            <TD></TD></TR><TR><TD style="WIDTH: 201px; HEIGHT: 3px" class="td_cell" align=left>
            Party<SPAN style="FONT-FAMILY: Arial">Code&nbsp;<SPAN style="COLOR: #ff0000">* </SPAN></SPAN></TD>
            <td style="WIDTH: 122px; HEIGHT: 3px"><SELECT style="WIDTH: 200px" id="ddlSPType" class="field_input" tabIndex=2 onchange="CallWebMethod('sptypecode');" runat="server" > <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px; HEIGHT: 3px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">
                &nbsp;Party Name</SPAN></TD>
            <td style="HEIGHT: 3px"><SELECT style="WIDTH: 300px" id="ddlSPTypeName" class="field_input" tabIndex=3 onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
            
            <tr>
                <td align="left" class="td_cell" style="WIDTH: 201px; HEIGHT: 3px">
                    Group Code<SPAN style="COLOR: #ff0000">*</SPAN>
                </td>
                <td style="WIDTH: 122px; HEIGHT: 3px">
                    <select id="ddlGroupCode" runat="server" class="field_input" name="D1" 
                        onchange="CallWebMethod('groupcd');" 
                        style="WIDTH: 200px" tabindex="10">
                        <option selected=""></option>
                    </select>
                </td>
                <td align="left" class="td_cell" style="WIDTH: 190px; HEIGHT: 3px">
                    Group Name</td>
                <td style="HEIGHT: 3px">
                    <select id="ddlGroupName" runat="server" class="field_input" name="D2" 
                        onchange="CallWebMethod('groupnm');" style="WIDTH: 300px" tabindex="11">
                        <option selected=""></option>
                    </select>
                </td>
         </tr>
            
            <tr id="airport" style="visibility:visible"><td style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">
            <asp:Label ID='airportcode' Text='Excursion Code' runat="server" Visible ="false" ></asp:Label> </SPAN></TD>
            <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlexccode" visible ="false" 
                    class="field_input" tabIndex=4 onchange="CallWebMethod('airportcd');" 
                    runat="server"> <OPTION selected></OPTION></SELECT></TD>
            <td style="WIDTH: 190px" class="td_cell" align=left><asp:Label ID ="lblexcname" visible ="false" runat ="server"> Excursion Name</asp:Label> </TD><TD>
                <SELECT style="WIDTH: 300px" id="ddlexcname" visible ="false" class="field_input" tabIndex=5 
                    onchange="CallWebMethod('airportnm');" runat="server" 
                    onclick="return ddlAirportnm_onclick()"> <OPTION selected></OPTION></SELECT></TD></TR>

    </div>


     
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Currency Code</SPAN> </SPAN></TD>
        <TD style="WIDTH: 122px"><asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" CssClass="field_input" Width="194px"></asp:TextBox></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD>
        <TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR>
        
    <tr><TD style="WIDTH: 201px" align=left>&nbsp;</TD>
        <TD style="WIDTH: 122px">&nbsp;</TD>
        <TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD>
        <TD style="WIDTH: 183px; HEIGHT: 36px" align=left>
            <TEXTAREA style="WIDTH: 363px; HEIGHT: 29px" id="txtRemark" class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA></TD></TR>

       </TBODY>
       <caption>
           <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
           </TD>
           </TR>
           <tr>
               <td align="left" class="td_cell" style="WIDTH: 201px">
                   <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                   <INPUT id="ChkActive" visible =false tabIndex=19  type=checkbox CHECKED 
           runat="server" />
                   
<asp:Label ID='lblactive' Text='Active' Visible=false  runat="server" ></asp:Label></td>
               <td align="left" style="WIDTH: 183px">
                   <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" 
                       Text="Approve/Unapprove" Visible="False" />
               </td>
               <td style=" HEIGHT: 23px ">
                   <asp:CheckBox ID="chkConsdierForMarkUp" runat="server" Font-Bold="False" 
                       Text="Consider this supplier for markup " Visible="False" />
               </td>
           </tr>
           <tr>
               <td align="right" class="td_cell" colspan="4" style="HEIGHT: 22px">
                   &nbsp;<asp:Button ID="btnGenerate" runat="server" CssClass="field_button" 
                       onclick="btnGenerate_Click" tabIndex="20" Text="Generate" />
                   &nbsp;
                   <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                       onclick="btnCancel_Click" tabIndex="21" Text="Return to Search" />
                   &nbsp;
                   <asp:Button ID="btnhelp" runat="server" CssClass="field_button" Height="20px" 
                       onclick="btnhelp_Click" tabIndex="22" Text="Help" />
               </td>
           </tr>
           </TBODY>
       </caption>
       </TABLE>
           <%-- <SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" visible="false" runat="server"> <OPTION selected></OPTION></SELECT>--%>
</contenttemplate>
</asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>

</asp:Content>

