<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="HandlingFeesPricelist1.aspx.vb" Inherits="PriceListModule_HandlingFeesPricelist1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 

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

            
        

        
        
            case "GroupCode":
                var select = document.getElementById("<%=ddlGroupCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "GroupName":
                var select = document.getElementById("<%=ddlGroupName.ClientID%>");
                var selectname = document.getElementById("<%=ddlGroupCode.ClientID%>");
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




            case "othsellcode":
                var select = document.getElementById("<%=ddlOtherSell.ClientID%>");
                var selectname = document.getElementById("<%=ddlOtherSellName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var sqlstr = "select Distinct  currmast.currcode from  currmast inner join othsellmast on currmast.currcode=othsellmast.currcode   where currmast.active=1 and othsellmast.othsellcode='" + select.options[select.selectedIndex].text + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrCodes, ErrorHandler, TimeOutHandler);




          

              
            case "othsellname":
                var select = document.getElementById("<%=ddlOtherSellName.ClientID%>");
                var selectname = document.getElementById("<%=ddlOtherSell.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
           



                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                var sqlstr = "select Distinct  currmast.currcode from  currmast inner join othsellmast on currmast.currcode=othsellmast.currcode   where currmast.active=1 and othsellmast.othsellcode='" + select.value + "'";
                ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillCurrCodes, ErrorHandler, TimeOutHandler);

             
         


        }
    }




    function FillCurrCodes(result) {
  
        var txtcur = document.getElementById("<%=TxtSellCurr.ClientID%>");
        txtcur.value = result;

        var sqlstr = "select convrate from currrates where currcode=(select option_selected from reservation_parameters where param_id=457) and tocurr='" + result + "'";

        ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillConvrates, ErrorHandler, TimeOutHandler);

    }



    function FillConvrates(result) {
        var txtcur = document.getElementById("<%=TxtSellConvRate.ClientID%>");
        txtcur.value = result;
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
      





        if (document.getElementById("<%=ddlSubSeasCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSubSeasCode.ClientID%>").focus();
            alert("Select Sub Season Code.");
            return false;
        }


      



        if (document.getElementById("<%=ddlOtherSell.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlOtherSell.ClientID%>").focus();
            alert("Select Selling Type.");
            return false;
        }

        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to Delete price list?') == false) return false; }

        }
    }



</script>


 <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
   <TABLE style="BORDER-RIGHT: gray 2pt solid; BORDER-TOP: gray 2pt solid; BORDER-LEFT: gray 2pt solid; BORDER-BOTTOM: gray 2pt solid">
     <TBODY><TR><TD style="HEIGHT: 18px; TEXT-ALIGN: center" class="field_heading" align=left colSpan=4>
          <asp:Label id="lblHeading" runat="server" Text="Add New Other service price List" CssClass="field_heading" Width="744px"></asp:Label></TD></TR>
     
     <tr><td style="width: 201px" class="td_cell" align=left>
        <SPAN style="FONT-FAMILY: Arial">PL Code</SPAN></TD><TD style="WIDTH: 122px">
            <INPUT style="WIDTH: 194px" id="txtPlcCode" class="field_input" disabled tabIndex=1 type=text runat="server" /></TD>
            <TD style="WIDTH: 190px" class="td_cell" align=left></TD>
            <TD></TD></TR>
             

 
   
    
    <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Code</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD>
        <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group Name</SPAN></TD>
        <TD><SELECT style="WIDTH: 300px" id="ddlGroupName" class="field_input" tabIndex=11 onchange="CallWebMethod('GroupName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>



          <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Type</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD>
        <TD style="WIDTH: 122px">
         <SELECT style="WIDTH: 200px" id="ddlOtherSell" class="field_input" tabIndex=12 onchange="CallWebMethod('othsellcode')" runat="server">
                <OPTION selected></option>
            </select>
 
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Type Name</SPAN></TD>
        <TD>
        <SELECT style="WIDTH: 200px" id="ddlOtherSellName" class="field_input" tabIndex=13 onchange="CallWebMethod('othsellname')" runat="server">
                <OPTION selected></option>
            </select>
        
        </TD></TR>





         <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Currency</SPAN> </TD>
        <TD style="WIDTH: 122px">
        <asp:TextBox id="TxtSellCurr" tabIndex=12 runat="server" readOnly  CssClass="field_input" Width="194px"></asp:TextBox>
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Conv.Rate</SPAN></TD>
        <TD>
       <asp:TextBox id="TxtSellConvRate" tabIndex=12 runat="server"  readOnly CssClass="field_input" Width="194px"></asp:TextBox>
        </TD></TR>
   

   
      





     
        
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN> <SPAN style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></SPAN></TD>
        <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=14 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN></TD>
        <TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=15 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    
  
    <tr><TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD>
        <TD style="WIDTH: 183px; HEIGHT: 36px" align=left colSpan=3>
            <TEXTAREA style="WIDTH: 607px; HEIGHT: 29px" id="txtRemark" class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA></TD></TR>
                
                
   <tr><TD style="WIDTH: 201px" class="td_cell" align=left colSpan=2>
   
   </TD>
        <%--<TD style="WIDTH: 122px"><ews:DatePicker id="dpFromdate" tabIndex=16 runat="server" CssClass="field_input" Width="180px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker></TD>
         <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">To Date </SPAN></TD>
        <TD><ews:DatePicker id="dpToDate" tabIndex=17 runat="server" CssClass="field_input" Width="180px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>
        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /></TD>--%> 
       <TD style="WIDTH: 122px">
           <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />
       </TD>
         </TR>

                
   <TR><TD style="WIDTH: 201px" class="td_cell" align=left><%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                    <INPUT id="ChkActive" tabIndex=19 type=checkbox CHECKED runat="server" />Active</TD><TD style="WIDTH: 183px" align=left >
   
    <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
        Visible="False" /></TD><td style=" HEIGHT: 23px "  >
                                    <asp:CheckBox ID="chkConsdierForMarkUp" runat="server"  checked Font-Bold="False" Visible="False"
                                            Text="Consider this supplier for markup " />
                                    </td></TR><TR><TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=4>
    &nbsp;<asp:Button id="btnGenerate" tabIndex=20 onclick="btnGenerate_Click" runat="server" Text="Generate" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnCancel" tabIndex=21 onclick="btnCancel_Click" 
            runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=22 onclick="btnhelp_Click" runat="server" 
            Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
           <%-- <SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" visible="false" runat="server"> <OPTION selected></OPTION></SELECT>--%>
</contenttemplate>
</asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>

</asp:Content>

