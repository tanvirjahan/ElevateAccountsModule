<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="CustUsermail.aspx.vb" Inherits="CustUsermail" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
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
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" CssClass="field_heading" Width="874px"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD vAlign=top><DIV style="WIDTH: 800px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelUser" runat="server" Width="700px" GroupingText="User Details"><TABLE style="WIDTH: 403px"><TBODY><TR><TD style="WIDTH: 355px; TEXT-ALIGN: right" align=right></TD></TR><TR><TD style="WIDTH: 355px" align=left><asp:GridView id="gv_Email" tabIndex=67 runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="no" HeaderText="Sr No" />
                            <asp:TemplateField HeaderText="Contect Person Name &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;">
                                <ItemTemplate>
                                    <input id="txtPerson" runat="server" class="field_input" maxlength="100" style="width: 215px"
                                        type="text" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Address &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;">
                                <ItemTemplate>
                                    <input id="txtEmail" runat="server" class="field_input" maxlength="100" style="width: 220px"
                                        type="text" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact No &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;">
                                <ItemTemplate>
                                    <input id="txtContactNo" runat="server" class="field_input" maxlength="15" style="width: 159px"
                                        type="text" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView> </TD></TR><TR><TD align=left><TABLE><TBODY><TR><TD style="WIDTH: 272px">
        <asp:Button id="BtnUserDetailAdd" tabIndex=66 onclick="BtnUserDetailAdd_Click" 
            runat="server" Text="Add" CssClass="field_button"></asp:Button>&nbsp; 
        <asp:Button id="BtnUserSave" tabIndex=68 onclick="BtnUserSave_Click" 
            runat="server" Text="Save" CssClass="field_button"></asp:Button>&nbsp; <asp:Button id="BtnUserCancel" tabIndex=69 onclick="BtnUserCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px">
            <asp:Button id="btnHelp" tabIndex=70 onclick="btnHelp_Click" runat="server" 
                Text="Help" __designer:dtid="1688858450198528" CssClass="field_button" 
                __designer:wfdid="w3"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

