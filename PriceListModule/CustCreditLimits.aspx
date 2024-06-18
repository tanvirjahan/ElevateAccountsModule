<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="CustCreditLimits.aspx.vb" Inherits="PriceListModule_CustCreditLimits" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<script language="javascript" type="text/javascript" >

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }


    function checkNumber(e) {
        if (event.keyCode < 45 || event.keyCode > 57) {
            return false;
        }
    }


</script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        width: 800px; border-bottom: gray 2px solid" class="td_cell" align="left">
        <tbody>
            <tr>
                <td valign="top" align="center" width="150" colspan="2">
                    <asp:Label ID="lblHeading" runat="server" Text="Customers" ForeColor="White" CssClass="field_heading_agent"
                        Width="800px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 15%" valign="top" align="left">
                    &nbsp;</td>
                <td style="width: 85%" class="td_cell" valign="top" align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 15%" valign="top" align="left">
                    <span style="color: #ff0000" class="td_cell"></span>
                </td>
                <td style="width: 85%" class="td_cell" valign="top" align="left">
                    &nbsp;&nbsp;
                    Code <span style="color: #ff0000" class="td_cell">*&nbsp; </span>
                    <input style="width: 196px" id="txtCustomerCode" class="field_input" tabindex="1"
                        type="text" maxlength="20" runat="server" />
                    &nbsp; Name&nbsp;
                    <input style="width: 244px" id="txtCustomerName" class="field_input" tabindex="2"
                        type="text" maxlength="100" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%" valign="top" align="left">
                    &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl>
                </td>
                <td style="width: 85%" valign="top">
                    <div style="width: 100%" id="iframeINF" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="PanelGeneral" runat="server" Width="600px" 
                                    GroupingText="Credit Limits" Height="310px">
                                    <table style="width: 530px; height: 160px;">
                                        <tbody>
                                        <TR>
<TD style="WIDTH:252px; HEIGHT: 25px" align=left ><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">Credit Days</SPAN>
    <span class="td_cell" style="COLOR: #ff0000">*&nbsp;</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
    <TD style="WIDTH:180px; HEIGHT: 25px"  >
    <INPUT style="WIDTH:120px;TEXT-ALIGN: right"   id="txtcreditdays" class="field_input" tabIndex="3" type=text maxLength=50 runat="server" />
    </td>

</TD>
<TD style="WIDTH:118px; HEIGHT: 25px" align=left ><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: Arial">Credit Limit</SPAN>
    <span class="td_cell" style="COLOR: #ff0000">*&nbsp;</span>
    </td>
    <td style="WIDTH:250px; HEIGHT: 25px" >
    <INPUT style="WIDTH:120px;TEXT-ALIGN: right" id="txtcreditlimit" class="field_input" tabIndex="4" type=text maxLength=50 runat="server" />
</TD>
</tr>
 <TR>
<TD style="WIDTH:252px; HEIGHT: 25px" align=left ><SPAN style="FONT-SIZE: 9t; FONT-FAMILY: Arial">Grace Credit Days</SPAN>
     &nbsp;&nbsp;&nbsp;&nbsp;  
     </td>
     <td style="WIDTH:180px; HEIGHT: 25px" >
    <INPUT style="WIDTH:120px;TEXT-ALIGN: right" id="txtgrcreditdays" class="field_input" tabIndex="5" type=text maxLength=50 runat="server" />
</TD>
<TD style="WIDTH:118px; HEIGHT: 25px" align=left ><SPAN style="FONT-SIZE:9pt; FONT-FAMILY: Arial">Grace Limit</SPAN>
     &nbsp;&nbsp;&nbsp; 
     </td>
     <td style="WIDTH:250px; HEIGHT: 25px">
    <INPUT style="WIDTH:120px;TEXT-ALIGN: right" id="txtgrlimit" class="field_input" tabIndex="6" type=text maxLength=50 runat="server" />
</TD>
</tr>
<tr>
 <TD style="width: 252px"><SPAN style="FONT-SIZE: 10pt; FONT-FAMILY: Arial">Enforce Credit Limit</SPAN>
 </td>
 <td  style="width: 180px">
     <asp:CheckBox  id="chkencrlimit"   tabIndex="7"   runat="server"></asp:checkbox>
  
</TD>
<td></td>
<td></td>
</tr>
<tr>
<td style="width: 258px">
<SPAN style="FONT-SIZE: 10pt; FONT-FAMILY: Arial">Allow Booking within Cancellation</SPAN>
</td>
<td style="width: 100px">
 <asp:CheckBox  id="chkcancel"   tabIndex="7"   runat="server"></asp:checkbox>
</td>
<td></td>
<td></td>
</tr>

<tr>
<td style="width: 252px">&nbsp;</td>
<td style="width: 180px">
</td>
<td style="width: 118px">
</td>
<td style="width: 250px">
</td>
</tr> 
                                     
                                            <tr>
                                            <td style="width: 252px; height: 35px;">
                                                </td>
                                                <td style="width: 180px"></td>
                                                <td style="width: 118px; height: 35px;"align=left><SPAN style="FONT-SIZE: 10pt; FONT-FAMILY: Arial">Blocked</SPAN> 
                                                    &nbsp;&nbsp;&nbsp; &nbsp;
                                                      </td>
                                                    <td style="width: 250px">
                                                  
                                                    <asp:DropDownList ID="ddlblocked" runat="server" TabIndex="8" Height="22px" 
                                                        Width="121px">
                                                        <asp:ListItem>[Select]</asp:ListItem>
                                                        <asp:ListItem Value="1">YES</asp:ListItem>
                                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                     
                                            </tr>
                                     <tr>
                                     <td style="width: 252px; height: 45px;"></td>
                                     <td style="width: 180px; height: 45px;" align="right">
                                         <asp:Label ID="lblActstatus" runat="server" Font-Bold="True" Font-Size="Larger" 
                                             Text="Status"></asp:Label>
                                         </TD> 
      <td align="left" colspan ="2" style="height: 45px;" >
          <table>
              <tr>
                  <td align="center" 
                      style="border: 1px solid #000000; height: 35px; width: 141px;">
                      <asp:Label ID="lblstatus" runat="server" Font-Bold="True" Font-Overline="False" 
                          Font-Size="Larger" ForeColor="#009999" Height="26px" Text="Active" 
                          Width="209px"></asp:Label>
                  </td>
              </tr>
          </table>
      </td>
                                         <tr>
                                             <td style="width: 252px; height: 40px;">
                                                                                                 <asp:Button ID="BtnSave" runat="server" CssClass="field_button" tabIndex="9" 
                                                     Text="Save" Width="69px" />
                                             </td>
                                             <td colspan="2" style="height: 40px">
                                                 <asp:Button ID="BtnCancel" runat="server" CssClass="field_button" 
                                                     onclick="BtnCancel_Click" tabIndex="10" Text="Return To Search" Width="146px" />
                                                 <asp:Button ID="btnHelp" runat="server" __designer:dtid="1688858450198528" 
                                                     __designer:wfdid="w10" CssClass="field_button" tabIndex="11" OnClick="btnHelp_Click" Text="Help" />
                                             </td>
                                             <td style="height: 40px; width: 250px;">
                                                 &nbsp;</td>
                                         </tr>
                                     </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

</asp:Content>

