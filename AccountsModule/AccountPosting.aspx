<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="AccountPosting.aspx.vb" Inherits="AccountPosting" %>

<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Invoice Accounts Posting" Style="padding: 2px"
                            CssClass="field_heading" Width="100%">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%; padding: 2px 4px 0px 4px">
                            <tr>
                                <td style="width: 12%;">
                                    <label class="field_caption">
                                        Transaction ID</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 20%;">
                                      <asp:TextBox ID="txtTransId" CssClass="field_input" runat="server" TabIndex="1" Enabled="false" Width="90%"></asp:TextBox>                                  
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="4">
                            </td></tr>
                            <tr>
                            <td colspan="4">
                            <label class="field_caption" id="lblCustomer" style="font-weight:bold;vertical-align:super" >Customer Posting</label>
                             <div id="divCust" style="min-height: 340px; max-height: 340px; max-width:95vw; overflow:scroll">                                
                                <asp:GridView ID="gvCust" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" style="padding-top:3px; padding-bottom:3px;" TabIndex="3" ShowFooter="true">                                    
                                    <Columns>                                                                                                                                                               
                                        <asp:BoundField DataField="AccountType" HeaderText="Account Type" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>

                                        <asp:TemplateField HeaderText="Account Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcctName" runat="server" Text='<%# Bind("AccountName") %>'></asp:Label>                                               
                                                <asp:Label ID="lblAcctCode" runat="server" style="display:none" Text='<%# Bind("AccountCode") %>'></asp:Label>                                               
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                         
                                        <asp:TemplateField HeaderText="Control Account Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCtrlAcctName" runat="server" Text='<%# Bind("controlAccountName") %>'></asp:Label>                                               
                                                <asp:Label ID="lblCtrlAcctCode" runat="server" style="display:none" Text='<%# Bind("controlAccountCode") %>'></asp:Label>                                               
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                      
                                                          
                                        <asp:BoundField DataField="currCode" HeaderText="Currency" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   
                                        
                                        <asp:BoundField DataField="acc_currency_rate" HeaderText="Currency Rate" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   
                                                                             
                                        <asp:TemplateField HeaderText="Debit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcc_debit" runat="server" Text='<%# Bind("acc_debit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Credit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcc_credit" runat="server" Text='<%# Bind("acc_credit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Debit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblacc_base_debit" runat="server" Text='<%# Bind("acc_base_debit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Credit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblacc_base_credit" runat="server" Text='<%# Bind("acc_base_credit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>                                                                               
                                        
                                        <asp:BoundField DataField="acc_narration" HeaderText="Narration" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField> 
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />                                    
                                </asp:GridView>
                                <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                            </td>
                            </tr>
                            <tr><td colspan="4" style="height:10px"></td></tr>
                            <tr>
                            <td colspan="4">
                            <label class="field_caption" id="lblProv" style="font-weight:bold;vertical-align:super" >Provisional Supplier Posting</label>
                             <div id="divProv" style="min-height: 340px; max-height: 340px; max-width:95vw; overflow:scroll">                                
                                <asp:GridView ID="gvProv" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" style="padding-top:3px; padding-bottom:3px;" TabIndex="4" ShowFooter="true">                                    
                                    <Columns>                                                                                                                                                               
                                        <asp:BoundField DataField="AccountType" HeaderText="Account Type" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>

                                        <asp:TemplateField HeaderText="Account Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcctName" runat="server" Text='<%# Bind("AccountName") %>'></asp:Label>                                               
                                                <asp:Label ID="lblAcctCode" runat="server" style="display:none" Text='<%# Bind("AccountCode") %>'></asp:Label>                                               
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                         
                                        <asp:TemplateField HeaderText="Control Account Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCtrlAcctName" runat="server" Text='<%# Bind("controlAccountName") %>'></asp:Label>                                               
                                                <asp:Label ID="lblCtrlAcctCode" runat="server" style="display:none" Text='<%# Bind("controlAccountCode") %>'></asp:Label>                                               
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                      
                                                          
                                        <asp:BoundField DataField="currCode" HeaderText="Currency" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   
                                        
                                        <asp:BoundField DataField="acc_currency_rate" HeaderText="Currency Rate" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   
                                        
                                        <asp:TemplateField HeaderText="Debit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcc_debit" runat="server" Text='<%# Bind("acc_debit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Credit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcc_credit" runat="server" Text='<%# Bind("acc_credit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Debit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblacc_base_debit" runat="server" Text='<%# Bind("acc_base_debit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Credit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblacc_base_credit" runat="server" Text='<%# Bind("acc_base_credit") %>'></asp:Label>              
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />                                        
                                        </asp:TemplateField>   
                                        
                                        <asp:BoundField DataField="acc_narration" HeaderText="Narration" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField> 
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />                                    
                                </asp:GridView>
                                <asp:Label ID="Label2" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>                   

</asp:Content>

