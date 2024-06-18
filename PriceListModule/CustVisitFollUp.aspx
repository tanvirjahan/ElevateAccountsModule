<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustVisitFollUp.aspx.vb" Inherits="CustVisitFollUp"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

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
<asp:Panel id="PanelVisit" runat="server" Width="700px" GroupingText="Visit Follow Up"><TABLE style="WIDTH: 689px"><TBODY>
    <tr>
        <td align="left">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 675px">
                <tr>
                    <td style="width: 46px; height: 19px">
                        <asp:Label ID="Label1" runat="server" Text="Visit Id" Width="89px"></asp:Label></td>
                    <td style="width: 165px; height: 19px"><INPUT style="WIDTH: 153px" id="txtvisitid" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></td>
                    <td style="width: 1px; height: 19px">
                    </td>
                    <td style="width: 182px; height: 19px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 46px; height: 16px">
                        <asp:Label ID="Label2" runat="server" Text="Visit Date From" Width="87px"></asp:Label></td>
                    <td style="width: 165px; height: 16px">
                        <asp:TextBox ID="txtPfromdate" runat="server" CssClass="field_input" 
                            Width="80px"></asp:TextBox>&nbsp;<asp:ImageButton
                            ID="ImgPBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:MaskedEditValidator ID="MEVPFromDate" runat="server" ControlExtender="MEPFromDate"
                            ControlToValidate="txtPfromdate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                            EmptyValueMessage="Date is required" ErrorMessage="MEPFromDate" InvalidValueBlurredMessage="Invalid Date"
                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></td>
                    <td style="width: 1px; height: 16px">
                        <asp:Label ID="Label3" runat="server" Text="Visit Date To" Width="87px"></asp:Label></td>
                    <td style="width: 182px; height: 16px">
                        <asp:TextBox ID="txtPtodate" runat="server" CssClass="field_input" Width="80px"></asp:TextBox><asp:ImageButton
                            ID="ImgPBtntoDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                        <cc1:MaskedEditValidator ID="MEVPToDate" runat="server" ControlExtender="MEPToDate"
                            ControlToValidate="txtPToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                            EmptyValueMessage="Date is required" ErrorMessage="MEPToDate" InvalidValueBlurredMessage="Invalid Date"
                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="left">
            <TABLE><TBODY><TR><TD style="WIDTH: 266px">&nbsp;<asp:Button id="btnVisitFollo" 
                    tabIndex=80 onclick="btnVisitFollo_Click" runat="server" Text="Add" 
                    CssClass="field_button"></asp:Button>&nbsp;
                            <asp:Button id="btnSearch" tabIndex=83 onclick="btnSearch_Click" 
                    runat="server" Text="Search" CssClass="field_button"></asp:Button>&nbsp;
                            <asp:Button ID="BtnVisitCancel" runat="server" CssClass="field_button" OnClick="BtnVisitCancel_Click"
                                TabIndex="83" Text="Return To Search" /></TD><TD style="WIDTH: 100px">
                    <asp:Button id="btnHelp" tabIndex=84 onclick="btnHelp_Click" runat="server" 
                        Text="Help" CssClass="field_button"></asp:Button></TD></TR></TBODY></TABLE>
        </td>
    </tr>
    <tr>
        <td align="left" style="height: 118px">
            <asp:GridView id="gv_VisitFollow" tabIndex=81 runat="server" AutoGenerateColumns="False" AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CssClass="td_cell" OnSorting="gv_VisitFollow_Sorting" OnRowCommand="gv_VisitFollow_RowCommand" Width="661px">
                            <Columns>
                            <asp:TemplateField  HeaderText="Visit Id"><EditItemTemplate>
                             <asp:TextBox ID="TextBox1" runat="server"  
                                    Text='<%# Dateformat(DataBinder.Eval (Container.DataItem, "visitid")) %>' 
                                    CssClass="field_input" ></asp:TextBox>                                                                    
                            </EditItemTemplate>
                            <ItemTemplate>
                            <asp:Label id="lblvisitid"  runat="server"  Text='<%# DataBinder.Eval (Container.DataItem, "visitid") %>'></asp:Label> 
                            </ItemTemplate>
                            </asp:TemplateField >
                                                    
                              <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy}" DataField="visitdate" SortExpression="visitdate" HeaderText="Visit Date">
                             <ItemStyle Width="1440px"></ItemStyle>
                             <HeaderStyle Width="1440px"></HeaderStyle>
                             </asp:BoundField>  
                                                           
                            <asp:BoundField DataField="cperson" SortExpression="cperson" HeaderText="Contact Person">
                            <ItemStyle Wrap=True Width="3000px"></ItemStyle>
                            <HeaderStyle Wrap=True Width="3000px"></HeaderStyle>
                            </asp:BoundField>   
                            
                            <asp:BoundField DataField="remarks" SortExpression="remarks" HeaderText="Remarks">
                            <ItemStyle Wrap=True Width="4000px"></ItemStyle>
                            <HeaderStyle Wrap=True Width="4000px"></HeaderStyle>
                            </asp:BoundField>                

                             <asp:BoundField DataField="reqaction" SortExpression="reqaction" HeaderText="Action Required">
                            <ItemStyle  Wrap=True Width="4000px"></ItemStyle>
                            <HeaderStyle Wrap=True Width="4000px"></HeaderStyle>
                            </asp:BoundField>    
                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
                            <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                            <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>
                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
                            <ItemStyle ForeColor="Blue"></ItemStyle>
                            </asp:ButtonField>                            
                                                         
                            </Columns>
    <RowStyle CssClass="grdRowstyle" ForeColor="Black" />
    <HeaderStyle CssClass="grdheader" ForeColor="white" />
    <AlternatingRowStyle CssClass="grdAternaterow" BorderWidth="10px" />
                        </asp:GridView> 
        </td>
    </tr>
    </TBODY></TABLE>
</asp:Panel> &nbsp; &nbsp;&nbsp;
    <cc1:CalendarExtender ID="CEPFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtnFrmDt"
        TargetControlID="txtPFromDate">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MEPFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
        TargetControlID="txtPFromDate">
    </cc1:MaskedEditExtender>
    <cc1:CalendarExtender ID="CEPToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgPBtntoDt"
        TargetControlID="txtPToDate">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MEPToDate" runat="server" Mask="99/99/9999" MaskType="Date"
        TargetControlID="txtPToDate">
    </cc1:MaskedEditExtender>
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

