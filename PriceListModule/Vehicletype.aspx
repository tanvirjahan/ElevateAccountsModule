<%@ Page Language="VB"  MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Vehicletype.aspx.vb" Inherits="PriceListModule_Vehicletype" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

  <script language ="javascript" type="text/javascript" >

      function checkNumber(e)
     {
         if ((event.keyCode < 47 || event.keyCode > 57)) 
         {
             return false;
         }
      }

      function checkCharacter(e)
       {
          if (event.keyCode == 32 || event.keyCode == 46)
              return;
          if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
              return false;
       }

      }    
      
      
</script> 

<asp:UpdatePanel id="UpdatePanel1" runat="server">
  <contenttemplate>

<table style="border: 2px solid gray; width: 888px; height: 446px">
    <tbody>
        <tr><td style="HEIGHT: 4px" class="td_cell" align=center colSpan=5><asp:Label id="lblHeading" runat="server" Text="Add New Vehicle Types" ForeColor="White" 
        CssClass="field_heading" Width="725px"></asp:Label></td>
        <td style="HEIGHT: 4px; width: 48px;" class="td_cell" align=center colSpan=1></td></tr>
        <tr style="COLOR: #ff0000"><td style="WIDTH: 130px; HEIGHT: 7px" class="td_cell"><SPAN style="COLOR: #000000">Code </SPAN>
        <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </SPAN></TD>
        <td style="WIDTH: 1%; COLOR: #000000; HEIGHT: 7px"><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" />
        <SPAN style="COLOR: #000000"></SPAN></TD>
        <td style="WIDTH: 1%; COLOR: #000000; HEIGHT: 7px"></td><td style="WIDTH: 10%; COLOR: #000000; HEIGHT: 7px"></td>
        <td style="WIDTH: 23px; COLOR: #000000; HEIGHT: 7px"></td>
            <td style="COLOR: #000000; HEIGHT: 7px; width: 48px;"></td></TR>
        
        <TR style="COLOR: #000000">
        <td style="WIDTH: 130px; HEIGHT: 2px" class="td_cell"><SPAN style="COLOR: #000000">Name</SPAN><SPAN style="COLOR: #ff0000"> <SPAN class="td_cell">*</SPAN></SPAN></td>
        <td style="WIDTH: 1%; COLOR: #ff0000; HEIGHT: 2px">
            <INPUT style="WIDTH: 196px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></td>
        <td style="WIDTH: 1%; COLOR: #ff0000; HEIGHT: 2px"></td><td style="WIDTH: 10%; COLOR: #ff0000; HEIGHT: 2px"></TD><TD style="WIDTH: 23px; COLOR: #ff0000; HEIGHT: 2px"></td>
        <td style="COLOR: #ff0000; HEIGHT: 2px; width: 48px;"></td></TR>

        <%--<TR><TD style="WIDTH: 130px; HEIGHT: 10px" class="td_cell">Group Code&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD style="WIDTH: 1%; HEIGHT: 10px" class="td_cell" align=left><SPAN style="COLOR: #ff0000">
        <SELECT style="WIDTH: 200px" id="ddlOtherGrpCode" class="field_input" tabIndex=3 onchange="GetOtherGrpValueFrom()" runat="server">
         <OPTION selected></OPTION></SELECT></SPAN></TD><TD style="WIDTH: 1%; HEIGHT: 10px" class="td_cell" align=left></TD>
         <TD style="WIDTH: 10%; HEIGHT: 10px" class="td_cell" align=left>Group&nbsp;Name&nbsp;&nbsp;</TD><TD style="WIDTH: 23px; HEIGHT: 10px" class="td_cell" align=left>
         <SELECT style="WIDTH: 350px" id="ddlOtherGrpName" class="field_input" tabIndex=4 onchange="GetOtherGrpValueCode()" runat="server"> <OPTION selected></OPTION></SELECT></TD>
         <TD style="HEIGHT: 10px" class="td_cell" align=left></TD></TR>--%>

         <TR><TD style="WIDTH: 130px; height: 32px;" class="td_cell">Order&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
         <TD style="WIDTH: 1%; height: 32px;"><INPUT style="TEXT-ALIGN: right" id="txtOrder" tabIndex=5 type=text maxLength=10 runat="server" /></TD>
             <TD style="WIDTH: 1%; height: 32px;"></TD>
         <TD style="WIDTH: 10%; height: 32px;"></TD><TD style="WIDTH: 23px; height: 32px;"></TD>
             <TD style="height: 32px; width: 48px;"></TD></TR>
         
         <TR><TD style="WIDTH: 130px; height: 30px;" class="td_cell">Min Pax&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
             <TD style="WIDTH: 1%; height: 30px;">
         <INPUT style="TEXT-ALIGN: right" id="txtMinPax" tabIndex=6 type=text runat="server" /></TD>
             <TD style="WIDTH: 1%; height: 30px;"></TD><TD style="WIDTH: 10%; height: 30px;"></TD>
         <TD style="WIDTH: 23px; height: 30px;"></TD><TD style="height: 30px; width: 48px;"></TD></TR><TR><TD style="WIDTH: 130px; HEIGHT: 8px" class="td_cell">Max Pax&nbsp;&nbsp;<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
         <TD style="WIDTH: 1%; HEIGHT: 8px"><INPUT style="TEXT-ALIGN: right" id="txtMaxPax" tabIndex=7 type=text runat="server" /></TD><TD style="WIDTH: 1%; HEIGHT: 8px"></TD>
         <TD style="WIDTH: 10%; HEIGHT: 8px"></TD><TD style="WIDTH: 23px; HEIGHT: 8px"></TD>
        <TD style="HEIGHT: 8px; width: 48px;"></TD></TR>
         
         <TR><TD style="WIDTH: 130px; height: 44px;" class="td_cell">Remark&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD>
         <TD style="height: 44px" ><asp:TextBox id="txtRemark" tabIndex=8 runat="server" Height="44px" Width="613px" TextMode="MultiLine"></asp:TextBox></TD>
             <TD colSpan=1 style="height: 44px"></TD></TR>

    <tr>
        <%--<td class="td_cell" style="width: 130px">
            Display Name</td>
        <td colspan="4">
            <INPUT style="width: 329px;" id="txtdispname" tabIndex=9 type="text" runat="server" maxlength="50" /></td>
        <td colspan="1">
        </td>--%>
         <td class="td_cell" style="width: 130px; height: 21px;">
           Capacity</td>
        <td colspan="4" style="height: 21px">
            <INPUT style="width: 329px;" id="txtCapacity" tabIndex="9" type="text" runat="server" maxlength="30" /></td>
        <td colspan="1" style="height: 21px; width: 48px;">
        </td>
    </tr>

   <tr>
    <td style="width: 130px; height: 2px" class="td_cell">Options</TD><TD style="width: 1%; height: 2px">
        <input style="width: 329px" id="txtOptions" tabindex="10" type="text" maxlength="30" runat="server" /></TD>
        <%-- <td style="width: 1%; height: 2px"></TD><td style="width: 10%; height: 2px"></TD>
        <td style="width: 23px; height: 2px">&nbsp;</TD><TD style="height: 2px"></TD>--%>
  </TR>

<tr>
<%--<td style="width: 130px; height: 2px" class="td_cell">Calculated by Pax/ Unit&nbsp;&nbsp;</TD>
        <td style="width: 1%; height: 2px">
    <asp:DropDownList id="ddlCalcPax" tabIndex=11 runat="server" Width="60px" ><asp:ListItem Selected="True">Yes</asp:ListItem>
 <asp:ListItem>No</asp:ListItem> </asp:DropDownList>&nbsp; </TD>
 <td style="width: 1%; height: 2px"></TD><td style="width: 10%; height: 2px">
 <asp:Label id="lbladult" runat="server" Text="Adult/ Child" Font-Size="8pt" Font-Names="Arial" Width="55px"></asp:Label></TD>
 <td style="width: 23px; height: 2px">
    <asp:DropDownList id="ddladchild" TabIndex=12 runat="server" Width="95px"><asp:ListItem Selected="True" Value="A">Adult</asp:ListItem>
<asp:ListItem Value="C">Child</asp:ListItem>
</asp:DropDownList></TD><TD style="HEIGHT: 2px"></TD>--%>
</TR>


<TR><TD style="WIDTH: 129px; HEIGHT: 1px" class="td_cell" colSpan=5>

<table style="width: 334px; height: 52px" border=0><TBODY>
<TR>


<TD style="width: 100%; height: 4px">
    <input id="chkshuttle"  type=checkbox  runat="server" />Shuttle</TD></TR>
<TR>


<TD style="width: 100%; height: 4px">
    <input id="ChkPakReq" tabIndex=13 type=checkbox checked runat="server" />Pax Check Required</TD></TR>
    <%--<TR><TD style="WIDTH: 103px; HEIGHT: 1px" colSpan=2>
        <INPUT id="ChkPrnRemark" tabIndex=14 type=checkbox CHECKED runat="server" />Print Remark</TD></TR>--%>
        
        <TR><TD style="WIDTH: 100%">
        <input id="ChkActive" tabIndex=15 type=checkbox CHECKED runat="server" />Active</TD></TR>
  </TBODY></TABLE></TD><TD style="HEIGHT: 1px; width: 48px;" class="td_cell" 
        colSpan=1></TD></TR>
  
  <TR><TD style="WIDTH: 130px; HEIGHT: 22px">
    <asp:Button id="btnSave" tabIndex=16 onclick="btnSave_Click" runat="server" 
        Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 1%; HEIGHT: 22px">
        <asp:Button id="btnCancel" tabIndex=17 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD>
        <TD style="WIDTH: 1%; HEIGHT: 22px"></TD><TD style="WIDTH: 10%; HEIGHT: 22px">
       <asp:Button id="btnhelp" tabIndex=18 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="field_button" Width="46px"></asp:Button></TD><TD style="WIDTH: 23px; HEIGHT: 22px">
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" class="field_input" /></TD>
      <TD style="HEIGHT: 22px; width: 48px;"></TD></TR></TBODY></TABLE>    <SELECT style="WIDTH: 200px" id="ddlOtherGrpCode" class="field_input" 
        tabIndex=3 onchange="GetOtherGrpValueFrom()" runat="server" visible="False">
         <OPTION selected></OPTION></SELECT>
</contenttemplate>
    </asp:UpdatePanel>
    
     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
 

</asp:Content>