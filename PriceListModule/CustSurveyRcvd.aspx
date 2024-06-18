<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustSurveyRcvd.aspx.vb" Inherits="CustSurveyRcvd" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" Width="872px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD vAlign=top><DIV style="WIDTH: 800px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelSurvey" runat="server" Height="260px" Width="600px" 
        GroupingText="Survey From Received">&nbsp;<TABLE style="WIDTH: 403px"><TBODY><TR><TD style="TEXT-ALIGN: right" align=left>
        <asp:Button id="Btnaddsurvey" tabIndex=84 onclick="Btnaddsurvey_Click" 
            runat="server" Text="Add" CssClass="field_button"></asp:Button> <asp:Button id="btnViewForm" tabIndex=85 runat="server" Text="View Form Submitted" CssClass="field_button"></asp:Button> </TD></TR><TR><TD align=left><asp:GridView id="grvSurvey" tabIndex=86 runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="no" HeaderText="Sr No" />
                                <asp:TemplateField HeaderText="Survey Date">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <ews:DatePicker ID="dpDateSurvey" runat="server" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
                                            Width="185px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted By">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <input id="txtSubmitedBy" runat="server" type="text" class="field_input" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <input id="txtRemarkSurvey" runat="server" type="text" class="field_input" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView> </TD></TR><TR><TD align=left><TABLE style="WIDTH: 269px"><TBODY><TR><TD style="WIDTH: 29px; HEIGHT: 22px">
            <asp:Button id="BtnSurveySave" tabIndex=87 onclick="BtnSurveySave_Click" 
                runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 98px; HEIGHT: 22px"><asp:Button id="BtnSurveyCancel" tabIndex=88 onclick="BtnSurveyCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 98px; HEIGHT: 22px">
            <asp:Button id="btnHelp" tabIndex=89 onclick="btnHelp_Click" runat="server" 
                Text="Help" __designer:dtid="1688858450198528" CssClass="field_button" 
                __designer:wfdid="w2"></asp:Button></TD></TR></TBODY></TABLE></TD><TD align=left></TD></TR></TBODY></TABLE></asp:Panel> 
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

