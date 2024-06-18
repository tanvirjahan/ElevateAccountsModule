<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ViewCalculation.aspx.vb" Inherits="PriceListModule_ViewCalculation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
    function closeWin() {
        window.close();
        return false;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <table style="width: 100%">
        <tr>
            <td align="center" >
                <asp:Label id="lblHeading" runat="server" Text="View Calculation" 
                    CssClass="field_heading" Width="961px"></asp:Label></td>
        </tr>
        <tr>
            <td class="td_cell">
                
            </td>
        </tr>
        <tr>
            <td align ="center"  class="td_cell" 
                style="font-size: 12px; font-weight: bold;">
                Package</td>
        </tr>
            <tr>
                <td align="center" >
                  <asp:GridView id="gv_PkgMain" runat="server" Font-Size="10px" CssClass="td_cell" 
                        Width="916px" BackColor="White" BorderColor="#999999" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" 
                        AutoGenerateColumns="False">
                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                            <Columns>
                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblsellcode" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "sellcode") %>'  ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width ="150px" HeaderText="Selling Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsellname" Font-Bold="true" Font-Size ="12px" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "sellname") %>'  ></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField   HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblconvrate" Font-Bold="true" Font-Size ="12px" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "convrate") %>'  ></asp:Label>
                                        <asp:GridView id="gv_PkgMain" tabIndex="0" runat="server" Font-Size="10px" CssClass="td_cell" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" Width="100%">
                                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="rmcatcode" SortExpression="rmcatcode" HeaderText="Room Category"></asp:BoundField>
                                                    <asp:BoundField DataField="roomsell" SortExpression="roomsell" HeaderText="Room Price + Profit"></asp:BoundField>
                                                    <asp:BoundField DataField="suplsell" SortExpression="suplsell" HeaderText="Supplement Price + Profit"></asp:BoundField>
                                                    <asp:BoundField DataField="trfssell" SortExpression="trfssell" HeaderText="Transfers Price + Profit"></asp:BoundField>
                                                    <asp:BoundField DataField="othsell" SortExpression="othsell" HeaderText="Others Price + Profit"></asp:BoundField>
                                                    <asp:BoundField DataField="pkgsellprice" SortExpression="pkgsellprice" HeaderText="Total Selling Price"></asp:BoundField>                                                   
                                                </Columns>
                                        <RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>
                                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                        <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                        <HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        <RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>
                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                        <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>
                        <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                    </asp:GridView>
                  </td>
            </tr>
        <tr>
            <td class="td_cell">
                
                </td>
        </tr>
               <tr>
            <td align ="center"  class="td_cell" 
                style="font-size: 12px; font-weight: bold;">
                Additional</td>
        </tr>
            <tr>
                <td align="center" >
                  <asp:GridView id="gv_Additional" runat="server" Font-Size="10px" CssClass="td_cell" 
                        Width="916px" BackColor="White" BorderColor="#999999" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" 
                        AutoGenerateColumns="False">
                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                            <Columns>
                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblsellcode" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "sellcode") %>'  ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width ="150px" HeaderText="Selling Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsellname" Font-Bold="true" Font-Size ="12px" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "sellname") %>'  ></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width ="750px"  HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblconvrate" Font-Bold="true" Font-Size ="12px" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "convrate") %>'  ></asp:Label>
                                        <asp:GridView id="gv_AdChild" tabIndex="0" runat="server" Font-Size="10px" CssClass="td_cell" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" Width="100%">
                                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="othservice" SortExpression="othservice" HeaderText="Service"></asp:BoundField>
                                                    <asp:BoundField DataField="sellprice" SortExpression="sellprice" HeaderText="Price + Profit"></asp:BoundField>
                                                </Columns>
                                        <RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>
                                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                        <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                        <HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        <RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>
                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                        <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>
                        <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                    </asp:GridView>
                  </td>
            </tr>
        <tr>
            <td class="td_cell">
                
                </td>
        </tr>
        <tr>
            <td align="center">

                <asp:Button ID="btnClose" runat="server" CssClass="field_button" 
                    OnClientClick="return closeWin();" Text="Close" />

                </td>
        </tr>
        <tr>
            <td class="td_cell">

            </td>
        </tr>
            <tr>
                <td class="td_cell">
                    &nbsp;</td>
           </tr>
    </table>
    <input id="txtconnection" runat="server" style="visibility:hidden;" type="text" />
    </contenttemplate> 
</asp:UpdatePanel>
</asp:Content>

