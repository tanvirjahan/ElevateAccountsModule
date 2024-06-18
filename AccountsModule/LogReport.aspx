<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="LogReport.aspx.vb" Inherits="AccountsModule_LogReport" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >


</script>


<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
         border-bottom: gray 2px solid"  width="1000px" height="180px">
        <tr>
            <td align="center" class="field_heading" >
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" 
                    Text=" View Report" Width="698px"></asp:Label></td>
        </tr>
        <tr>
            <td >
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD><TABLE><TBODY><TR><TD><asp:Label id="Label9" runat="server" Text="From Date" Width="110px" CssClass="field_caption"></asp:Label></TD><TD>

<asp:TextBox id="txtFromDate" tabIndex=1 runat="server" Width="80px" 
        CssClass="txtbox"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>
        
        <cc1:MaskedEditValidator id="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MEFromDate"></cc1:MaskedEditValidator>
        
        </TD><TD><asp:Label id="lblTodate" runat="server" Text="To Date" ForeColor="Black" Width="110px" CssClass="field_caption"></asp:Label></TD><TD>
<asp:TextBox id="txtToDate" tabIndex=2 runat="server" Width="80px" 
        CssClass="txtbox"></asp:TextBox><asp:ImageButton id="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate"></cc1:MaskedEditValidator></TD></TR>
       
       
       <tr>
       
       <td colspan="4">
       </td>
       </tr> 
        <TR>
        <TD>
    <asp:Label id="lblType" runat="server" Text="Type" Width="110px" 
                CssClass="field_caption"></asp:Label>
    
    </TD>
    <td>
    

        <select id="ddlType" runat="server" class="dropdown" style="WIDTH: 200px" 
            tabindex="3">
            <option selected=" " value="[Select]">[Select]</option>
        

        <option  value="JV">Journel</option>

         <option  value="RV">Receipts</option>

          <option  value="CPV">Cash Payments</option>
           <option  value="BPV">BanK Payments</option>
            <option  value="CN">Credit Notes</option>
            <option  value="DN">Debit Notes</option>

        </select>
      </td>





      <td>
      
      <asp:Label id="lblvoucherno" runat="server" Text="Voucher No" Width="110px" 
                CssClass="field_caption"></asp:Label>
      
      
      </td>
      <td>
     <asp:TextBox Text="" runat="server" ID="txtdocno">
     </asp:TextBox>
      
      
      </td>
      </TR>

      <tr>
      <td colspan="4">
      </td>
      </tr>

      <tr>
     <td class="td_cell">
User
</td>
<td>

<select id="ddlUser" runat="server" class="dropdown" style="WIDTH: 154px" 
            tabindex="4"> 
                <option selected="" value="[Select]">[Select]</option>

                </select>


</td>
      
      </tr>


      <tr>
      
      <td>
     <asp:Button id="btnReport" tabIndex=14 runat="server" 
            Text=" Report" CssClass="btn" CausesValidation="False" EnableTheming="True"></asp:Button>&nbsp;
        <asp:Button id="btnClear" tabIndex=15  runat="server" 
            Text="Clear" Font-Bold="False" CssClass="btn"></asp:Button> 
      
      </td>
      </tr>
      <tr>
    <td>
      <asp:Label id="lblMsg1" runat="server" 
                Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" 
                CssClass="lblmsg" Visible="False"></asp:Label>

                </td>

  </tr>

  <TR>
  <TD>
  <cc1:CalendarExtender id="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate"></cc1:CalendarExtender>
  
  <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtFromDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender><cc1:CalendarExtender id="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate" TargetControlID="txtToDate"></cc1:CalendarExtender><cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtToDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
  
  </TD></TR>


</contenttemplate> 
</asp:UpdatePanel> 



    </table>
</asp:Content>

