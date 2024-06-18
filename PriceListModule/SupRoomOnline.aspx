<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupRoomOnline.aspx.vb" Inherits="PriceListModule_SupRoomOnline" %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
 <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

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

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Suppliers" CssClass="field_heading" Width="872px" ForeColor="White"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD vAlign=top><DIV style="WIDTH: 800px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelVisit" runat="server" Width="700px" GroupingText="Room Types"><TABLE style="WIDTH: 689px"><TBODY><TR><TD align=left><TABLE style="WIDTH: 675px" cellSpacing=0 cellPadding=0 border=0><TBODY><TR><TD style="WIDTH: 46px; HEIGHT: 19px"><asp:Label id="Label1" runat="server" Visible="False" Text="Visit Id" Width="89px" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 165px; HEIGHT: 19px"><INPUT style="DISPLAY: none; WIDTH: 153px" id="txtvisitid" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /></TD><TD style="WIDTH: 1px; HEIGHT: 19px"></TD><TD style="WIDTH: 182px; HEIGHT: 19px"></TD></TR><TR><TD style="WIDTH: 46px; HEIGHT: 16px"><asp:Label id="Label2" runat="server" Visible="False" Text="Visit Date From" Width="87px" __designer:wfdid="w2"></asp:Label></TD><TD style="WIDTH: 165px; HEIGHT: 16px"><asp:TextBox id="txtPfromdate" runat="server" Visible="False" CssClass="fiel_input" Width="80px" __designer:wfdid="w3"></asp:TextBox>&nbsp;<asp:ImageButton id="ImgPBtnFrmDt" runat="server" Visible="False" ImageUrl="~/Images/Calendar_scheduleHS.png" __designer:wfdid="w4"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVPFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" ErrorMessage="MEPFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtPfromdate" ControlExtender="MEPFromDate" __designer:wfdid="w5"></cc1:MaskedEditValidator></TD><TD style="WIDTH: 1px; HEIGHT: 16px"><asp:Label id="Label3" runat="server" Visible="False" Text="Visit Date To" Width="87px" __designer:wfdid="w6"></asp:Label></TD><TD style="WIDTH: 182px; HEIGHT: 16px"><asp:TextBox id="txtPtodate" runat="server" Visible="False" CssClass="fiel_input" Width="80px" __designer:wfdid="w7"></asp:TextBox><asp:ImageButton id="ImgPBtntoDt" runat="server" Visible="False" ImageUrl="~/Images/Calendar_scheduleHS.png" __designer:wfdid="w8"></asp:ImageButton> <cc1:MaskedEditValidator id="MEVPToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date" ErrorMessage="MEPToDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required" Display="Dynamic" ControlToValidate="txtPToDate" ControlExtender="MEPToDate" __designer:wfdid="w9"></cc1:MaskedEditValidator></TD></TR></TBODY></TABLE></TD></TR><TR><TD align=left><TABLE><TBODY><TR><TD style="WIDTH: 266px">&nbsp;<asp:Button id="btnVisitFollo" tabIndex=80 onclick="btnVisitFollo_Click" runat="server" Text="Add" CssClass="field_button" Width="48px" __designer:wfdid="w10"></asp:Button>&nbsp; <asp:Button id="btnSearch" tabIndex=83 onclick="btnSearch_Click" runat="server" Visible="False" Text="Search" CssClass="field_button" Width="54px" __designer:wfdid="w11"></asp:Button>&nbsp; <asp:Button id="BtnVisitCancel" tabIndex=83 onclick="BtnVisitCancel_Click" runat="server" Text="Return To Search" CssClass="field_button" __designer:wfdid="w12"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnHelp" tabIndex=84 onclick="btnHelp_Click" runat="server" Visible="False" Text="Help" CssClass="field_button" Width="45px" __designer:wfdid="w13"></asp:Button></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="HEIGHT: 118px" align=left>
<asp:GridView id="gv_VisitFollow" tabIndex=81 runat="server" CssClass="td_cell" Width="661px" OnRowCommand="gv_VisitFollow_RowCommand" OnSorting="gv_VisitFollow_Sorting" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" BackColor="White" AllowSorting="True" AutoGenerateColumns="False" __designer:wfdid="w14">
<RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="Room Type Code" SortExpression="rmtypcode"><EditItemTemplate>
                             <asp:TextBox ID="TextBox1" runat="server"  Text='<%# Dateformat(DataBinder.Eval (Container.DataItem, "rmtypcode")) %>' ></asp:TextBox>                                                                    
                            
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblvisitid" runat="server" Text='<%# DataBinder.Eval (Container.DataItem, "rmtypcode") %>' __designer:wfdid="w33"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="rmtypname" HeaderText="Room Type Name" HtmlEncode="False" SortExpression="rmtypname">
<HeaderStyle Width="1440px"></HeaderStyle>

<ItemStyle Width="1440px"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="roomdescription" HeaderText="Desc" SortExpression="roomdescription">
<HeaderStyle Wrap="True" Width="3000px"></HeaderStyle>

<ItemStyle Wrap="True" Width="3000px"></ItemStyle>
</asp:BoundField>
<asp:ButtonField CommandName="Editrow" Text="Edit" HeaderText="Action">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField CommandName="View" Text="View" HeaderText="Action">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField CommandName="Deleterow" Text="Delete" HeaderText="Action">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle  CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle  CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> </TD></TR></TBODY></TABLE></asp:Panel> &nbsp; &nbsp;&nbsp; <cc1:CalendarExtender id="CEPFromDate" runat="server" TargetControlID="txtPFromDate" PopupButtonID="ImgPBtnFrmDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEPFromDate" runat="server" TargetControlID="txtPFromDate" MaskType="Date" Mask="99/99/9999">
    </cc1:MaskedEditExtender> <cc1:CalendarExtender id="CEPToDate" runat="server" TargetControlID="txtPToDate" PopupButtonID="ImgPBtntoDt" Format="dd/MM/yyyy">
    </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MEPToDate" runat="server" TargetControlID="txtPToDate" MaskType="Date" Mask="99/99/9999">
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

