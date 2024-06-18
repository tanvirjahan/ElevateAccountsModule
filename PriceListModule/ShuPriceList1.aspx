<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ShuPriceList1.aspx.vb" Inherits="ShuPriceList1" %>
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

            case "curcode":
                var select = document.getElementById("<%=ddlcurcode.ClientID%>");
                var selectname = document.getElementById("<%=ddlcurname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "curname":
                var select = document.getElementById("<%=ddlcurname.ClientID%>");
                var selectname = document.getElementById("<%=ddlcurcode.ClientID%>");
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

    function FormValidation(state) {
        if (document.getElementById("<%=ddlSPType.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSPType.ClientID%>").focus();
            alert("Select Supplier Type Code.");
            return false;
        }





        else if (document.getElementById("<%=ddlSubSeasCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSubSeasCode.ClientID%>").focus();
            alert("Select Sub Season Code.");
            return false;
        }


        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to generate Transfer price list?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to generate Transfer price list?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to generate Transfer price list?') == false) return false; }

        }
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
   <%-- <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Currency Code</SPAN> </SPAN></TD>
        <TD style="WIDTH: 122px">
        <asp:TextBox id="txtCurrCode" tabIndex=12 runat="server" CssClass="field_input" Width="194px"></asp:TextBox>
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD>
        <TD><asp:TextBox id="txtCurrName" tabIndex=13 runat="server" CssClass="field_input" Width="294px"></asp:TextBox></TD></TR>
        --%>
         <tr><td style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Code&nbsp; <SPAN style="COLOR: #ff0000">*</SPAN></SPAN></TD>
          <td style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlcurcode" class="field_input" tabIndex=6 onchange="CallWebMethod('curcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
          <td style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Currency Name</SPAN></TD>
          <td><SELECT style="WIDTH: 300px" id="ddlcurname" class="field_input" tabIndex=7 onchange="CallWebMethod('curname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
          
    <tr><TD style="WIDTH: 201px" align=left><SPAN style="FONT-SIZE: 8pt"><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN> <SPAN style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></SPAN></TD>
        <TD style="WIDTH: 122px"><SELECT style="WIDTH: 200px" id="ddlSubSeasCode" class="field_input" tabIndex=14 onchange="CallWebMethod('SeasCode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Sub Season Code</SPAN></TD>
        <TD><SELECT style="WIDTH: 300px" id="ddlSubSeasName" class="field_input" tabIndex=15 onchange="CallWebMethod('SeasName');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    
  
         <tr>
             <td align="left" style="WIDTH: 201px"><SPAN style="FONT-SIZE: 8pt"><span style ="FONT-FAMILY: Arial"> 
                 Transfer Types </span>  <span style="FONT-SIZE: 8pt">
                     <span style="COLOR: #ff0000; FONT-FAMILY: Arial">*</SPAN></span></span>
             </td>
             <td style="WIDTH: 122px">
                 <asp:DropDownList ID="ddlServerType" runat="server"   CssClass="fiel_input"  tabindex="3" Width="200px">                  
                 </asp:DropDownList>
</td>
             <td align="left" class="td_cell" style="WIDTH: 190px">
                 </td>
             <td>
                 </td>
         </tr>
    
  
    <tr><TD style="WIDTH: 201px; HEIGHT: 36px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Remark</SPAN></TD>
        <TD style="WIDTH: 183px; HEIGHT: 36px" align=left colSpan=3>
            <TEXTAREA style="WIDTH: 607px; HEIGHT: 29px" id="txtRemark" class="field_input" 
                tabIndex=18 runat="server"></TEXTAREA></TD></TR>
                
                
   <tr><TD style="WIDTH: 201px" class="td_cell" align=left colSpan=2>
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
                                    <asp:CheckBox ID="chkConsdierForMarkUp" runat="server" Font-Bold="False" Visible="False"
                                            Text="Consider this supplier for markup " />
                                    </td></TR><TR><TD style="HEIGHT: 22px" class="td_cell" align=right colSpan=4>
    &nbsp;<asp:Button id="btnGenerate" tabIndex=20 onclick="btnGenerate_Click" runat="server" Text="Generate" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnCancel" tabIndex=21 onclick="btnCancel_Click" 
            runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>
    &nbsp; <asp:Button id="btnhelp" tabIndex=22 onclick="btnhelp_Click" runat="server" 
            Text="Help" Height="20px" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
            <SELECT style="WIDTH: 200px" id="ddlGroupCode" class="field_input" tabIndex=10 onchange="CallWebMethod('GroupCode');" visible="false" runat="server"> <OPTION selected></OPTION></SELECT>
</contenttemplate>
</asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>
</asp:Content>

