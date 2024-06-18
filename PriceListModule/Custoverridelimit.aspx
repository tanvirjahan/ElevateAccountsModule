<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Custoverridelimit.aspx.vb" Inherits="Custoverridelimit" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
        
function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }           
			
		
function checkNumber(e)
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}

</script>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" CssClass="field_heading" Width="882px"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD style="WIDTH: 85%" vAlign=top><DIV style="WIDTH: 100%" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelGeneral" runat="server" Width="700px" 
        GroupingText="Allow User for Booking"><TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px" align=left>
        <table style="width: 507%">
            <tr>
                <td style="width: 133px"><strong>
                    <asp:Label ID="Label1" runat="server" Text="Override Limit"></asp:Label></strong>
                </td>
                <td>
                    <span style="COLOR: #ff0000">
                    &nbsp;<INPUT style="WIDTH: 213px" id="txtoverridelimit" class="field_input" 
                        tabIndex=2 type=text maxLength=100 runat="server" /></span></td>
            </tr>
        </table>
        </TD></TR>
        <tr>
            <td align="left" style="WIDTH: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="left" style="WIDTH: 100px">
                Remarks
            </td>
        </tr>
        <TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><asp:TextBox id="txtremarks" 
                tabIndex=89 runat="server" Height="100px" CssClass="field_input" Width="389px" 
                TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><TABLE><TBODY><TR><TD style="WIDTH: 59px">
    <asp:Button id="BtnGeneralSave" tabIndex=90 onclick="BtnGeneralSave_Click" 
        runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 115px">
    <asp:Button id="BtnGeneralCancel" tabIndex=91 onclick="BtnGeneralCancel_Click" 
        runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 115px">
        </TD></TR><tr><td style="WIDTH: 59px">&nbsp;</td>
            <td style="WIDTH: 115px">
                &nbsp;</td>
            <td style="WIDTH: 115px">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblwebserviceerror" runat="server" style="display: none" 
                    Text="Webserviceerror"></asp:Label>
                <div>
                    <asp:GridView ID="gv_actionuserlog" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
                        BorderStyle="None" BorderWidth="1px" CssClass="td_cell" tabIndex="81" 
                        Width="661px">
                        <Columns>
                          <asp:BoundField DataField="Sno" HeaderText="Sno" 
                                SortExpression="Sno.">
                                <ItemStyle Width="100px" Wrap="True" />
                                <HeaderStyle Width="100px" Wrap="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="requestid" HeaderText="Booking no." 
                                SortExpression="Booking no.">
                                <ItemStyle Width="500px" Wrap="True" />
                                <HeaderStyle Width="500px" Wrap="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="overridelimit" HeaderText="Override Limit" 
                                SortExpression="Override Limit">
                                <ItemStyle Width="500px" Wrap="True" />
                                <HeaderStyle Width="500px" Wrap="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="odate" DataFormatString="{00:dd/MM/yyyy}" 
                                HeaderText="Date" HtmlEncode="False" SortExpression="odate">
                                <ItemStyle Width="500px" />
                                <HeaderStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="remarks" HeaderText="Remarks" 
                                SortExpression="remarks">
                                <ItemStyle Width="4000px" Wrap="True" />
                                <HeaderStyle Width="4000px" Wrap="True" />
                            </asp:BoundField>
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" ForeColor="Black" />
                        <HeaderStyle CssClass="grdheader" ForeColor="white" />
                        <AlternatingRowStyle BorderWidth="10px" CssClass="grdAternaterow" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        </TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel></DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

