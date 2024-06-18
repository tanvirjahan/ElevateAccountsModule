<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="CusMultiMail.aspx.vb" Inherits="CusMultiMail" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">



        //WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');



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
			

    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Customer" ForeColor="White" CssClass="field_heading"
                                Width="800px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" width="150">
                            Code <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td class="td_cell" valign="top" align="left">
                            <input style="width: 196px" id="txtCode" class="field_input" disabled tabindex="1"
                                type="text" maxlength="20" runat="server" />
                            &nbsp; Name&nbsp;
                            <input style="width: 310px" id="txtName" class="field_input" disabled tabindex="4"
                                type="text" maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" width="150">
                            <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl>
                        </td>
                        <td style="width: 100px" valign="top">
                            <div style="width: 656px" id="iframeINF" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 669px">
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell" colspan="2">
                                                        <asp:Panel ID="PanelEmail" runat="server" Width="577px" 
                                                            GroupingText="Multiple Email">
                                                            <table style="width: 500px">
                                                                <tbody>
                                                              <%--      <tr>
                                                                        <td align="left">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>--%>
                                                                    <tr>
                                                                        <td align="left">
                                                                                            <asp:GridView ID="gv_Email" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                                CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                TabIndex="13" Width="572px">
                                                <FooterStyle CssClass="grdfooter" />
                                                                                <Columns>
                                                                                                                        <asp:TemplateField>
                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                        <HeaderTemplate>
                                                            <asp:CheckBox runat="server" ID="chkemaildetAll" style="display:none" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkemaildet" Width="15px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
                                              
                                                                                    <%--HeaderText="Contect Person Name &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"--%>
                                                                                    <asp:TemplateField  HeaderText="Contact Details">
                                                  
                                                                                        <ItemTemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                         <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact Person Name</span> <span
                                                                                                            style="color: red">*</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <input style="width: 220px" id="txtPerson" class="field_input" type="text" maxlength="100"
                                                                                                            runat="server" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                    <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact Number</span> <span
                                                                                                            style="color: red">*</span>
                                                                                                                                                                                                         </td>
                                                                                                    <td>
                                                                                                        <input style="width: 220px" id="txtContactNo" class="field_input" type="text" maxlength="25"
                                                                                                            runat="server" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                      <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Email Address</span> <span
                                                                                                            style="color: red">*</span>
                                                                                                     
                                                                                                    
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <input style="width: 220px" id="txtEmail" class="field_input" type="text" maxlength="100"
                                                                                                            runat="server" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                                    <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Designation</span> 
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <input id="txtdesignation" runat="server" class="field_input" type="text" maxlength="200"
                                                                                                            style="width: 220px;" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--<asp:TemplateField HeaderText="Email Address &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Contact No &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>

</ItemTemplate>
</asp:TemplateField>--%>
                                                                                </Columns>
                                                                                      <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <table style="width: 222px">
                                                                                <tbody>
                                                                                <TR>  <td style="width: 29px">
        <asp:Button id="btnaddrow" tabIndex=38 runat="server" Text="Add Row" 
            CssClass="field_button"  Width="94px"></asp:Button></TD>
         <td style="width: 100px">
            <asp:Button id="btndelrow" tabIndex=39  runat="server" Text="Delete Row" 
                 CssClass="field_button" Width="94px"  > </asp:Button>
        
                      </TD>                                          
                <td style="width: 100px">
                </td>
                </TR>
                                                                                    <tr>
                                                                                        <td style="width: 29px">
                                                                                            <asp:Button ID="BtnEmailSave"  tabIndex=38 runat="server" Text="Save"  
                                                                                                CssClass="field_button" Width="93px"></asp:Button>
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Button ID="BtnEmailCancel" TabIndex="61" OnClick="BtnEmailCancel_Click" runat="server"
                                                                                                Text="Return To Search" CssClass="field_button"></asp:Button>
                                                                                        </td>
                                                                                        <td>
                                                                                         <asp:Button ID="btnhelp" TabIndex="9"  runat="server"  Text="Help" CssClass="field_button" onclick="btnHelp_click"></asp:Button>
                                                                                        </td>
                                                                                          
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:Label ID="lblwebserviceerror" runat="server" Style="display: none" Text="Webserviceerror"></asp:Label>
                                                                                            <asp:CheckBox ID="chkrmvmail" runat="server" Text="Remove all the rows" 
                                                                                                Visible="False" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
