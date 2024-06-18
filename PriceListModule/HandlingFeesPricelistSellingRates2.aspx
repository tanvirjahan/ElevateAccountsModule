<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="HandlingFeesPricelistSellingRates2.aspx.vb" Inherits="PriceListModule_HandlingFeesPricelistSellingRates2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
 
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
    
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript"  type ="text/javascript" >










    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        //if (charCode != 47 && (charCode > 45 && charCode < 58)) {    
        if (charCode != 47 && (charCode > 44 && charCode < 58)) {
            return true;
        }
        return false;
    }
   
   
 



    function GetValueFromSS() {

        var ddl = document.getElementById("<%=ddlSubSeasonName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSubSeasonCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCodeSS() {

        var ddl = document.getElementById("<%=ddlSubSeasonCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlSubSeasonName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }


</script>


<asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 843px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text="Price list - Selling Rates" 
        CssClass="field_heading" Width="837px"></asp:Label></TD></TR><TR><TD style="WIDTH: 212px" class="td_cell">PL Code</TD><TD style="WIDTH: 59px">
    <asp:TextBox id="txtPLCode" runat="server" CssClass="field_input" Width="191px" 
        ReadOnly="True"></asp:TextBox></TD><TD style="WIDTH: 11px"><%--<INPUT style="WIDTH: 89px" id="txtBlockCode" readOnly type=text runat="server" Visible="false" />--%></TD><TD style="WIDTH: 100px">
        &nbsp;</TD></TR>


        
          <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Type</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD>
        <TD style="WIDTH: 122px">
         <SELECT style="WIDTH: 200px" id="ddlOtherSell" class="field_input" tabIndex=12 onchange="CallWebMethod('othsellcode')" runat="server">
                <OPTION selected></option>
            </select>
 
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Type Name</SPAN></TD>
        <TD>
        <SELECT style="WIDTH: 200px" id="ddlOtherSellName" class="field_input" tabIndex=13 onchange="CallWebMethod('othsellname')" runat="server">
                <OPTION selected></option>
            </select>
        
        </TD></TR>





         <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Selling Currency</SPAN> </TD>
        <TD style="WIDTH: 122px">
        <asp:TextBox id="TxtSellCurr" tabIndex=12 runat="server" readOnly  CssClass="field_input" Width="194px"></asp:TextBox>
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Conv.Rate</SPAN></TD>
        <TD>
       <asp:TextBox id="TxtSellConvRate" tabIndex=12 runat="server"  readOnly CssClass="field_input" Width="194px"></asp:TextBox>
        </TD></TR>
   

   
       <tr><TD style="WIDTH: 201px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Market Code</SPAN> <SPAN style="COLOR: #ff0000">*</SPAN></TD>
        <TD style="WIDTH: 122px">
         <SELECT style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=12 onchange="CallWebMethod('marketcode')" runat="server">
                <OPTION selected></option>
            </select>
 
        
        </TD>
        <TD style="WIDTH: 190px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Market Name</SPAN></TD>
        <TD>
        <SELECT style="WIDTH: 200px" id="ddlMarketName" class="field_input" tabIndex=13 onchange="CallWebMethod('marketname')" runat="server">
                <OPTION selected></option>
            </select>
        
        </TD></TR>

        
       

   <%-- <tr>
        <td class="td_cell" style="WIDTH: 212px">
            Promotion Name</td>
        <td style="WIDTH: 59px">
            <select ID="ddlPromotion" runat="server" class="field_input" 
                disabled="disabled" name="D1" onchange="CallWebMethod('promotion');" 
                style="WIDTH: 197px">
                <option selected=""></option>
            </select></td>
        <td style="WIDTH: 11px">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="td_cell" style="WIDTH: 212px">
            Promotion Code</td>
        <td style="WIDTH: 59px">
            <asp:TextBox ID="txtPromotionCode" runat="server" CssClass="txtbox" 
                Enabled="False" Width="191px"></asp:TextBox>
        </td>
        <td style="WIDTH: 11px">
            &nbsp;</td>
    </tr>--%>
    <TR><TD style="WIDTH: 212px" class="td_cell">Sub Season</TD><TD style="WIDTH: 59px"><SELECT onblur="GetValueFromSS()" style="WIDTH: 200px" id="ddlSubSeasonCode" class="drpdown" disabled runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 11px"><SELECT onblur="GetValueCodeSS()" style="WIDTH: 200px" id="ddlSubSeasonName" class="drpdown" disabled runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    
    <TR><TD style="WIDTH: 212px" class="td_cell">  <INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtErrMsg" type=text 
              runat="server" /></TD></TR>
   <%-- <TD>
        <asp:TextBox ID="dpRevDate" runat="server" AutoPostBack="false" 
            CssClass="fiel_input" Enabled="False" Width="80px"></asp:TextBox>
        <cc1:CalendarExtender ID="dpRevsiondate_CalendarExtender" runat="server" 
            Format="dd/MM/yyyy" PopupButtonID="ImgPBtnFrmDt" TargetControlID="dpRevDate">
        </cc1:CalendarExtender>
        &nbsp;<asp:ImageButton ID="ImgPBtnFrmDt" runat="server" Enabled="False" 
            ImageUrl="~/Images/Calendar_scheduleHS.png" />
    </TD><TD style="WIDTH: 11px"><INPUT style="VISIBILITY: hidden; WIDTH: 8px" id="txtErrMsg" type=text runat="server" /></TD></TR><TR><TD style="WIDTH: 212px" class="td_cell">Price List Type</TD><TD style="WIDTH: 59px"><asp:DropDownList id="ddlPriceList" runat="server" CssClass="drpdown" Width="197px" Enabled="False" AutoPostBack="True"><asp:ListItem>[Select]</asp:ListItem>
<asp:ListItem>Normal Rates 1 Night</asp:ListItem>
<asp:ListItem>Weekend Rates 1 Night</asp:ListItem>
<asp:ListItem>Weekly Rates 7 Nights</asp:ListItem>
<asp:ListItem>Normal Rates &gt; 1 Night</asp:ListItem>
<asp:ListItem Value="Weekend Rates &gt; 1 Night"></asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 11px"><asp:CheckBox id="ChkBManual" runat="server" Text="Manual" CssClass="chkbox" Width="51px" Enabled="False"></asp:CheckBox></TD></TR><TR><TD colSpan=3>
    &nbsp;</TD><TD class="td_cell" vAlign=top align=left rowSpan=1></TD></TR><TR>
    <TD class="td_cell" colSpan=4><asp:CheckBox id="ChkWeek1" runat="server" Text="Weekend Option 1" Enabled="False"></asp:CheckBox>&nbsp;<asp:Label id="lblWEO1" runat="server" Text="Week end1 option" __designer:wfdid="w3"></asp:Label> <asp:CheckBox id="ChkWeek2" runat="server" Text="Weekend Option 2" CssClass="chkbox" Enabled="False"></asp:CheckBox> <asp:Label id="lblWEO2" runat="server" Text="Week end2 option" __designer:wfdid="w4"></asp:Label> <asp:Label id="lblSellingCode" runat="server" Visible="False" __designer:wfdid="w1"></asp:Label></TD>
    </TR>--%>
    <tr>
        <td class="td_cell" colspan="4">
            <table style="width: 100%">
                <tr>
                    <td>
                        <strong>Dates</strong></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td valign="top" class="td_cell">
                        <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                            AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="3" CssClass="grdstyle" Enabled="False" Font-Size="10px" 
                            GridLines="Vertical" tabIndex="12" Width="1px">
                            <FooterStyle CssClass="grdfooter" />
                            <Columns>
                                <asp:BoundField DataField="SrNo" HeaderText="Sr No" Visible="False">
                                </asp:BoundField>
                                <%--<asp:TemplateField HeaderText="From Date">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
<ews:DatePicker id="FrmDate" runat="server" Width="183px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="To Date">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
<ews:DatePicker id="ToDate" runat="server" Width="183px" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker> 
</ItemTemplate>
</asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="From Date">
                                    <ItemTemplate>
                                        <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                        <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px">
                                        </asp:TextBox>
                                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                                            PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtfromDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                                            MaskType="Date" TargetControlID="txtfromDate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                            ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <br />
                                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                                            ControlExtender="MeFromDate" ControlToValidate="txtfromDate" 
                                            CssClass="field_error" Display="Dynamic" 
                                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                            ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                                            InvalidValueMessage="Invalid Date" 
                                            TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date">
                                    <%--<EditItemTemplate>
&nbsp;
</EditItemTemplate>
<FooterTemplate>
&nbsp;
</FooterTemplate>
<ItemTemplate>
<ews:DatePicker id="dtpToDate" tabIndex=10 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker> 
</ItemTemplate>--%>
                                    <ItemTemplate>
                                        <%--<ews:DatePicker id="dpFromDate" tabIndex=9 runat="server" CssClass="field_input" Width="171px" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy" DateFormatString="dd/MM/yyyy"></ews:DatePicker>--%>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px">
                                        </asp:TextBox>
                                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                                            PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                                            MaskType="Date" TargetControlID="txtToDate">
                                        </cc1:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                                            ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <br />
                                        <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                                            ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                                            Display="Dynamic" EmptyValueBlurredText="Date is required" 
                                            EmptyValueMessage="Date is required" ErrorMessage="MeToDate" 
                                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                                            TooltipMessage="Input a Date in Date/Month/Year">
                                        </cc1:MaskedEditValidator>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle" />
                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="grdheader" />
                            <AlternatingRowStyle CssClass="grdAternaterow" />
                        </asp:GridView>
                    </td>
                    <td class="td_cell" valign="top">
                        &nbsp;</td>
                    <td class="td_cell" valign="top">
                      
                    </td>
                    <td valign="top">
                        &nbsp;</td>
                    <td valign="top" class="td_cell">
                        
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>

<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" class="td_cell"></TD></TR>
<TR><TD style="TEXT-ALIGN: center" class="td_cell">
        <TABLE  class="td_cell"><TBODY>
        <TR><TD style="TEXT-ALIGN: left"><asp:Button id="btnCheckPrices" runat="server" Text="Check Prices Below Cost Prices/Profit" Width="216px" CssClass="search_button" __designer:wfdid="w1"></asp:Button></TD>
        <TD style="TEXT-ALIGN: left">
    <asp:Label id="lblLightCoral" runat="server" Width="80px" __designer:wfdid="w1" 
        BackColor="LightCoral"></asp:Label>&nbsp;<asp:Label id="Label2" runat="server" Text="Selling price lower than cost price" __designer:wfdid="w3"></asp:Label></TD></TR><TR><TD style="TEXT-ALIGN: left"></TD><TD style="TEXT-ALIGN: left">
        <asp:Label id="lblLightBlue" runat="server" Width="72px" __designer:wfdid="w1" 
            BackColor="LightBlue"></asp:Label>&nbsp;&nbsp;<asp:Label id="lblProfitPer" runat="server" Text="Selling price lower than profit %" __designer:wfdid="w4"></asp:Label></TD></TR></TBODY></TABLE></TD></TR>
            
            <TR><TD >
            
            <asp:GridView id="gv_SearchResult" runat="server" Font-Size="10px"  CssClass="td_cell" __designer:wfdid="w14" CellPadding="3" GridLines="Vertical" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" BackColor="White">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
  <asp:BoundField DataField="Sr No" HeaderText="Sr No" ></asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Code" HeaderText="Service Type Code">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Service Type Name"  HeaderText="Service Type Name">
                                                       
                                                    </asp:BoundField>
</Columns>

<RowStyle CssClass="grdrowstyle"></RowStyle>
<EmptyDataTemplate>
<INPUT id="txtSGL" type=text runat="server" />
</EmptyDataTemplate>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView></TD></TR></TBODY></TABLE><TABLE ><TBODY><TR>
                <TD style="WIDTH: 200px" class="td_cell" align=left rowSpan=4><asp:Panel id="Panel1" runat="server" Width="246px" __designer:wfdid="w3" GroupingText="<font color=white>1</font>"><TABLE style="WIDTH: 232px"><TBODY><TR><TD style="WIDTH: 100px">-1 Incl</TD><TD style="WIDTH: 100px">-2 N/Incl</TD><TD style="WIDTH: 100px">-3 Free</TD><TD style="WIDTH: 100px">-4 N/A</TD><TD style="WIDTH: 100px">-5 On Request</TD></TR></TBODY></TABLE></asp:Panel></TD></TR><TR>
                <TD style="WIDTH: 329px; TEXT-ALIGN: center" class=" "><asp:CheckBox id="chkAllow" runat="server" Text="Allow Any Way" Font-Size="8pt" Font-Names="Verdana" Width="120px" __designer:wfdid="w1" BackColor="#FFC0C0" ForeColor="Blue"></asp:CheckBox></TD></TR><TR>
                <TD style="WIDTH: 500px; TEXT-ALIGN: left" class="td_cell">
                    &nbsp;<asp:Button id="btnSave" runat="server" Text="Update"  Width="48px" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="tbnClose" runat="server" Text="Return To Search" Width="120px" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnhelp" onclick="btnhelp_Click" runat="server" Text="Help" __designer:dtid="2251799813685366" Width="46px" CssClass="btn" __designer:wfdid="w9"></asp:Button></TD></TR>
                <tr>
                    <td class="td_cell" style="WIDTH: 500px; TEXT-ALIGN: left">
                        <asp:HiddenField ID="hdnpricelist" runat="server" />
                        <select ID="ddlGroupCode" runat="server" class="field_input" 
                        style="WIDTH: 200px" tabindex="10" visible="false">
                        <option selected=""></option>
                    </select>
                    </td>
                </tr>
                </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>

</asp:Content>

