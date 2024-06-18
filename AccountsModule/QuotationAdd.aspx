<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="QuotationAdd.aspx.vb" Inherits="AccountsModule_QuotationAdd" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type="text/javascript" >
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <contenttemplate>
     <center>
        
              <table width:"600px" height="900px">
              <tr>
              <td  align="center">
               <asp:Label ID="lblHeadingHTL" runat="server" CssClass="field_heading" 
               Text="Quotation" Width="100%"></asp:Label></center> 
              
              
              </td>
              </tr>











              </table>






               </contenttemplate> 
               </asp:UpdatePanel> 
</asp:Content>

