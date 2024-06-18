<%@ Page Title="" Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="ChildMealMapping.aspx.vb" Inherits="PriceListModule_ChildMealMapping" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script type="text/javascript">
       
     function checkNumber(e) {
         if (event.keyCode == 46)
         { return true; }

         if ((event.keyCode < 47 || event.keyCode > 57)) {
             return false;
         }

     }

     function FillPlistCode(plistcode, plistname) {

         var plistcode = document.getElementById(plistcode);
         var ddlplistname = document.getElementById(plistname);
         var codeid = plistcode.options[plistcode.selectedIndex].text;
         ddlplistname.value = codeid;
     }

     function FillPlistName(plistname, plistcode) {

         var ddlplistname = document.getElementById(plistname);
         var plistcode = document.getElementById(plistcode);
         var codeid = ddlplistname.options[ddlplistname.selectedIndex].text;
         plistcode.value = codeid;
     }




</script>
<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>

<table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;">
<tr>
<td style="text-align: center;" class="field_heading" align="center">
<asp:Label ID="lblheading" runat="server" CssClass="field_heading" Text="Child Meal Mapping" Width="843px"></asp:Label></td>
</tr>
<tr>
<td>
<table style="width:555px">
<tr>
<td  class="td_cell" style="width: 500px; text-align: left">






<%--<DIV ="HEIGHT: 250px; width: 480px;" class="container">--%>
<%--<asp:Panel ID="Panel4" runat="server" Height="165px" ScrollBars="Auto">--%>
<asp:GridView ID="grdchildmapping" runat="server"  AutoGenerateColumns ="false" Width="500px" cssclass="grdstyle1">
<Columns>
<asp:BoundField  DataField="childmealcode" HeaderText="Child Meal Code"></asp:BoundField>
<asp:BoundField  DataField="childmealname" HeaderText="Child Meal Name"></asp:BoundField>


<asp:TemplateField HeaderText="Plist Meal Code" >
<ItemStyle Width="110px" HorizontalAlign="left"></ItemStyle>
<HeaderStyle Width="110px" HorizontalAlign="left"></HeaderStyle>
<ItemTemplate>
<select style="WIDTH: 105px" id="ddlPlistmealCode"  class="drpdown" runat="server"></select> 

</ItemTemplate> 
</asp:TemplateField>

<asp:TemplateField HeaderText="Plist Meal Name" >
<ItemStyle Width="130px" HorizontalAlign="left"></ItemStyle>
<HeaderStyle Width="130px" HorizontalAlign="left"></HeaderStyle>
<ItemTemplate>
<select  style="WIDTH: 150px" id="ddlPlistmealName"  class="drpdown" runat="server"></select> 
</ItemTemplate> 
</asp:TemplateField>
<asp:BoundField DataField="mealcode" HeaderText="mealcode" Visible ="false"></asp:BoundField>
<asp:BoundField DataField="mealname" HeaderText="mealname" Visible="false"></asp:BoundField>
</Columns>
<HeaderStyle CssClass="grdheader"></HeaderStyle>
</asp:GridView>
<%--</DIV>--%>
<%--</asp:Panel>--%>



</td>


</tr>
</table> <%--Inner table closing--%>
<tr>

 <td style="text-align: left;" class="td_cell">
  <asp:Button ID="btnSave" runat="server"  CssClass="btn" Font-Bold="True" Text="Save" />&nbsp;<asp:Button ID="btnExit" runat="server" CssClass="btn" Font-Bold="True" 
Text="Exit" />&nbsp;<asp:Button ID="btnReport" runat="server" CssClass="btn" Font-Bold="True" Text="Report" Visible="True" /> &nbsp;
</td>
</tr>
</td><%----first table closing for td--%>
</tr><%----first table closing for tr--%>
</table><%----first table closing for table--%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Content>

