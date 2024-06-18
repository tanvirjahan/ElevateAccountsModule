<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="TrfPaxSlab.aspx.vb" Inherits="PriceListModule_TrfPaxSlab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

 <table width ="100%" >
  <tr>
        <td style="width: 100% ; text-align :center " colspan="2" > 
            <asp:Label id="lblHeading" runat="server" Text="Transfer Pax Slab" 
        ForeColor="White" __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label></td>
        <td> </td>
    </tr>
    <tr>
    <td colspan="2"></td>
    <td></td>
    </tr>
        <tr>
            <td  align="left" style="width: 100%" colspan="2">
 <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<table class="td_cell" align="center" >
    <tbody>
   
        <tr>
            <td colspan="4" align ="center">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="300px" BorderColor="Black" BorderWidth ="1px" >
                    <asp:GridView ID="grdPaxSlab" runat="server" BackColor="White" 
                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                            tabIndex="12" Width="325px" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#6B6B9A" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="paxslab" HeaderText="Pax Slab No">
                                    <ItemStyle Width="100px" />
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                               
                                <asp:TemplateField HeaderText="Vehicle Type">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlVehicleType" runat="server" AutoPostBack="True" 
                                            CssClass="drpdown" >   
                                            <asp:ListItem>[Select]</asp:ListItem>                                        
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Multiply By Unit">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text=''></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUnit" runat="server" CssClass="field_input" 
                                            Text='' Width="70px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                            <RowStyle CssClass="grdRowstyle" />
                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="grdheader" />
                            <AlternatingRowStyle CssClass="grdAternaterow" />
                        </asp:GridView>
                   
                </asp:Panel>
            
            </td>        
        </tr> 

        <tr>
            <td>
        
                    <asp:Button ID="btnAddLines" runat="server" CssClass="btn" 
                         Text="Add Row" />&nbsp;
                    <asp:Button ID="btnDeleteRow" runat="server" CssClass="btn" 
                         Text="Delete last Row" />
            </td>
            
           
        
        </tr>
   





 </tbody> 
</table>

</contenttemplate> 
</asp:UpdatePanel> 
</td> 
<td> </td>
</tr>
<tr>
<td align ="center" style="width: 100%" >

                    <asp:Button ID="btnSave" runat="server" CssClass="btn" 
                         Text="Save" />            
&nbsp;&nbsp;&nbsp;
 <asp:Button ID="btnReturn" runat="server" CssClass="btn"  Text="Return" />
</td>
<td>
    &nbsp;</td>
</tr>
    </table><asp:HiddenField ID="hdnGrpCode" runat="server" />
</asp:Content>

