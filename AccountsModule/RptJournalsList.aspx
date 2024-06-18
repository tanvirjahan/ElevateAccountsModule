 

<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptJournalsList.aspx.vb" Inherits="AccountsModule_RptJournalsList" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<TABLE style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; WIDTH: 100%; BORDER-BOTTOM: gray 1px solid" class="td_cell" align=left>
<TBODY>
<TR>
<TD style="WIDTH: 100%" class="field_heading" align=center colSpan=5>
<asp:Label id="lblHeading" runat="server" Text="Report Journals List" CssClass="field_heading" Width="100%"></asp:Label></TD></TR>

<TR><TD style="WIDTH: 100%" class="td_cell" align=left colSpan=5>
<TABLE style="WIDTH: 100%" class="td_cell" align=left><TBODY><TR><TD style="WIDTH: 77px" class="td_cell" align=left>As on Date</TD>
    <TD class="td_cell" vAlign=middle align=left>
    <asp:TextBox id="txtFromDate" tabIndex=1 runat="server" CssClass="txtbox" 
        Width="96px"></asp:TextBox>&nbsp; <asp:ImageButton id="ImgBtnFrmDt" tabIndex=2 runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:maskededitvalidator id="MskVFromDt" runat="server" controlextender="MskFromDate" controltovalidate="txtFromDate" cssclass="field_error" display="Dynamic" emptyvalueblurredtext="*" emptyvaluemessage="Date is required" errormessage="MskVFromDate" invalidvalueblurredmessage="*" invalidvaluemessage="Invalid Date" tooltipmessage="Input a date in dd/mm/yyyy format" width="1px"></cc1:maskededitvalidator></TD></TR><TR style ="display:none "><TD style="WIDTH: 77px" class="td_cell" align=left>
        Level</TD><TD class="td_cell" vAlign=middle align=left>
    <asp:DropDownList  style="display:none" id="ddlselect" tabIndex=3 runat="server" CausesValidation="True" 
        CssClass="drpdown">
     
                            </asp:DropDownList></TD>
                            <td style="width: 180px;Display: None" colspan="2" >
                                                
                                                <asp:CheckBox ID="chknew"  CssClass="td_cell" AutoPostBack="True"
                                        runat="server" Font-Bold="True" Text="New Format" Checked="True" />
                                                
                                            </td>
                            </TR>
                          
                         
                   

                            <TR><TD  class="td_cell" align=left></TD>
                                <TD class="td_cell" 
                                    vAlign=middle align=center colSpan=3><asp:Button id="btnLoadReprt" style="display:none" onclick="btnLoadReprt_Click" runat="server" 
            Text="Load Report"   tabIndex="4" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnPdfReport" onclick="btnPdfReport_Click" runat="server" 
     
                        Text="Pdf Report" tabIndex=4 CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnExcelReport" onclick="btnExcelReport_Click" runat="server" 
            Text="Excel Report" tabIndex=4 CssClass="btn"></asp:Button>&nbsp;</TD><td></td><td></td></TR><TR><TD style="WIDTH: 77px">
    <asp:Button id="Button1" runat="server" Text="Export" CssClass="btn" 
        OnClick="Button1_Click1" Visible="False"></asp:Button></TD><TD align=right>
            &nbsp;<asp:Button 
            id="btnhelp" onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="btn" Visible="False"></asp:Button></TD>
            <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
    </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
            
            
            
            
            </TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 1003px" colSpan=5><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy><cc1:CalendarExtender ID="ClsExFromDate" runat="server"
                                Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                    ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                    TargetControlID="txtFromDate">
                                </cc1:MaskedEditExtender>
                            </TD></TR></TBODY></TABLE>

<script language="javascript" type="text/javascript">



    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }
    

</script>
</asp:Content>

