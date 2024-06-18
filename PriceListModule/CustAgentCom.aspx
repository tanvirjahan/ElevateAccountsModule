<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustAgentCom.aspx.vb" Inherits="PriceListModule_CustAgentCom" %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="ews" %>
<%@ OutputCache location="none" %> 
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript" >

function TimeOutHandler(result)
{
    alert("Timeout :" + result);
}

function ErrorHandler(result)
{
    var msg = result.get_exceptionType() + "\r\n";
    msg += result.get_message() + "\r\n";
    msg += result.get_stackTrace();
    alert(msg);
}

function checkNumber(e)
{
    if (event.keyCode < 45 || event.keyCode > 57)
    {
        return false;
    }
}

function checkNumberDecimal(evt, txt) {

    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : ((evt.which) ? evt.which : 0));
    if (charCode != 47 && (charCode > 44 && charCode < 58)) {
        var value = txt.value;
        var indx = value.indexOf('.');
        var deci = 2;
        var lngLenght = deci;
        if (indx < 0) { return true; }

        var digit = value.substring(indx + 1);
        if (digit.length > lngLenght - 1) { return false; }
        else { return true; }
    }
    return false;
}

function ToValidatePercentage(txtPercentage, txtfuelcost, txtfuelvalue) {
    
    var percentage = document.getElementById(txtPercentage);
//    var fuelcost = document.getElementById(txtfuelcost);
//    var fuelvalue = document.getElementById(txtfuelvalue);

    if (percentage.value == "" ) {
        alert("Percentage field can not be blank");
        return false;
    }
   

}

//function FillAgentCode(ddlAgentCode, ddlAgentName) {
//    var agentcode = document.getElementById(ddlAgentCode);
//    var agentname = document.getElementById(ddlAgentName);
//    agentname.value = agentcode.options[agentcode.selectedIndex].text;

//}

//function FillAgentName(ddlAgentName, ddlAgentCode) {
//    var agentname = document.getElementById(ddlAgentName);
//    var agentcode = document.getElementById(ddlAgentCode);
//    agentcode.value = agentname.options[agentname.selectedIndex].text;

//}

</script>


        <TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left>
            <TR>
                <TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" Width="872px" CssClass="field_heading"></asp:Label></TD>
            </TR>
            <TR>
                <TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD>
                <TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input"  type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" type=text maxLength=100 runat="server" /></TD>
            </TR>
            <TR>
                <TD style="WIDTH: 15%" valign="top"  align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD>
                <TD style="WIDTH: 85%" valign="middle"  >
                    <DIV  id="iframeINF" runat="server">
                        <asp:UpdatePanel id="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel id="PanelCommission" Width="520px" runat="server"  GroupingText="Percentage of Commission">
                                   
                                                          
                                                            <asp:GridView id="gv_row"   runat="server" Width="510px" Font-Size="10px" CssClass="td_cell " GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" BackColor="White">
                                                             <Columns>
                                             <asp:TemplateField   Visible="false" HeaderText="LineNo" HeaderStyle-BackColor="#06788B"  >
                                                <EditItemTemplate>
                                                    <asp:TextBox id="txtlineno" runat="server" Text='<%# Bind("agentcommissioncode") %>'></asp:TextBox> 
                                                </EditItemTemplate>
                                                <ItemTemplate  >
                                                    <asp:Label id="lblLineNo" runat="server" Text='<%# Bind("agentcommissioncode") %>'></asp:Label> 
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                          
                                             
                                              <asp:TemplateField HeaderText="Select"  HeaderStyle-BackColor="#06788B" >
                                                <ItemTemplate>
                                                    <asp:CheckBox id="chkDel" runat="server" Width="17px"></asp:CheckBox> 
                                                </ItemTemplate>
                                             </asp:TemplateField>

                                              <asp:TemplateField  HeaderText="From Date" HeaderStyle-BackColor="#06788B" >
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                   <asp:TextBox id="txtFromDate"  runat="server" CssClass="fiel_input" Width="100px" ValidationGroup="MKE" ></asp:TextBox>&nbsp;
                                                   <asp:ImageButton id="ImgBtnFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;
                                                   <cc1:MaskedEditValidator id="MaskEdValidDate1" runat="server" CssClass="field_error" Width="1px" Height="20px"
                                                        Display="Dynamic" ControlExtender="MskEdExtendDate1" ControlToValidate="txtfromdate"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                         TooltipMessage="Input a date in dd/mm/yyyy format" />
                                                   <cc1:MaskedEditExtender id="MskEdExtendDate1" runat="server" TargetControlID="txtfromdate" AcceptNegative="Left" DisplayMoney="Left" MaskType="Date" Mask="99/99/9999" />
                                                   <cc1:CalendarExtender id="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFromDate" TargetControlID="txtfromdate" />

                                                </ItemTemplate>
                                             </asp:TemplateField>  
                                             
                                              <asp:TemplateField  HeaderText="To Date" HeaderStyle-BackColor="#06788B">
                                                <ItemTemplate>
                                               <asp:TextBox id="txtToDate"  runat="server" CssClass="fiel_input" Width="98px" ValidationGroup="MKE" ></asp:TextBox>&nbsp;
                                                   <asp:ImageButton id="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;
                                                  <cc1:MaskedEditValidator id="MaskEdValidDate2" runat="server" CssClass="field_error" Width="1px" Height="20px"
                                                        Display="Dynamic" ControlExtender="MskEdExtendDate2" ControlToValidate="txtToDate"
                                                        InvalidValueBlurredMessage="Invalid Date"  InvalidValueMessage="Invalid Date"
                                                        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"  TooltipMessage="Input a date in dd/mm/yyyy format" />
                                                   <cc1:MaskedEditExtender id="MskEdExtendDate2" runat="server" TargetControlID="txtToDate" AcceptNegative="Left" DisplayMoney="Left" MaskType="Date" Mask="99/99/9999" />
                                                   <cc1:CalendarExtender id="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDate" TargetControlID="txtToDate" />
                                                </ItemTemplate>
                                             </asp:TemplateField>  
                                             
                                              <asp:TemplateField  HeaderText="Percentage" HeaderStyle-BackColor="#06788B">
                                                <ItemTemplate>
                                                  <asp:TextBox id="txtPercentage"   runat="server" class="field_input" maxlength="20" ></asp:TextBox> 
                                                </ItemTemplate>
                                             </asp:TemplateField>  

                                           <%--  <asp:ButtonField HeaderText="Action" Text="Update" CommandName="EditRow">
                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                            </asp:ButtonField>--%>
                                             
                                            
                                                 
                                        </Columns>
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                            <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                            <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                            <AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
                                      </asp:GridView> 
                                                      
                                                                                                                
                                      <asp:Label id="lblMsg" runat="server" Width="357px" Text="No Records" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Visible="False"></asp:Label>
                                                                
                                   
                                </asp:Panel> &nbsp; &nbsp;&nbsp;
                              
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </DIV>
                </TD>
            </TR>
            <tr>
            <td>&nbsp;</td>
             <td><asp:Button id="btnAddRow"  Width="20%" TabIndex="2"  runat="server" Text="Add Row" 
                                                                        CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;   <asp:Button id="btnDeleteRow" Width="20%" TabIndex="3"  runat="server" Text="Delete Row" 
                                                                        CssClass="field_button"></asp:Button>&nbsp;</td>
            </tr>

            <tr>
            <td> </td>
             <td><asp:Button ID="btnSave"  Width="20%" runat="server" CssClass="field_button"  TabIndex="4"
                                                                        Text="Save" />&nbsp;&nbsp;&nbsp;  <asp:Button id="btnCancel"  Width="20%" TabIndex="5"  runat="server" Text="Return To Search" CssClass="field_button" /></td>
            </tr>
                  
                                                                 
                                                                    
                                                                
                                                               
         
        </TABLE>
         <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtgridrows" type=text maxLength=15 runat="server" />
<asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
<services>
    <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
</asp:ScriptManagerProxy>

</asp:Content>

