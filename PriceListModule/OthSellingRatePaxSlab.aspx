<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OthSellingRatePaxSlab.aspx.vb" Inherits="PriceListModule_OthSellingRatePaxSlab" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %>      
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">



<script language ="javascript"  type ="text/javascript" >



    function CallWebMethod(methodType) {
        switch (methodType) {
            case "sptypecode":
                var select = document.getElementById("<%=ddlSPCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlSPName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSuppAgentCodeListnew(constr, codeid, FillSupplierAgentCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr, codeid, FillSupplierAgentName, ErrorHandler, TimeOutHandler);
                break;
            case "sptypename":
                var select = document.getElementById("<%=ddlSPName.ClientID%>");
                var codeid = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlSPCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value

                ColServices.clsServices.GetSupplierCodeListnew(constr, codeid, FillSupplierCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSupplierNameListnew(constr, codeid, FillSupplierName, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSuppAgentCodeListnew(constr, codeid, FillSupplierAgentCode, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetSuppAgentNameListnew(constr, codeid, FillSupplierAgentName, ErrorHandler, TimeOutHandler);
                break;
            case "partycode":
                var select = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlSupplierName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSupplierName.ClientID%>");
                var selectname = document.getElementById("<%=ddlSupplierCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "supagentcode":
                var select = document.getElementById("<%=ddlSupplierACode.ClientID%>");
                var selectname = document.getElementById("<%=ddlSupplierAName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "supagentname":
                var select = document.getElementById("<%=ddlSupplierAName.ClientID%>");
                var selectname = document.getElementById("<%=ddlSupplierACode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "currcode":
                var select = document.getElementById("<%=ddlCurrencyCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlCurrencyName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "currname":
                var select = document.getElementById("<%=ddlCurrencyName.ClientID%>");
                var selectname = document.getElementById("<%=ddlCurrencyCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "subseascode":
                var select = document.getElementById("<%=ddlSubSeasonCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasonName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "subseasname":
                var select = document.getElementById("<%=ddlSubSeasonName.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasonCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }
    function FillSupplierCode(result) {
        var ddl = document.getElementById("<%=ddlSupplierCode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSupplierName(result) {
        var ddl = document.getElementById("<%=ddlSupplierName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }
    function FillSupplierAgentCode(result) {
        var ddl = document.getElementById("<%=ddlSupplierACode.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }

    function FillSupplierAgentName(result) {
        var ddl = document.getElementById("<%=ddlSupplierAName.ClientID%>");
        RemoveAll(ddl)
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].ListText, result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value = "[Select]";
    }


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


 <table style="width: 100%">
                    <tr>
                        <td class="field_heading" colspan="5" style="text-align: center" align="center">
                            <asp:Label ID="lblheading" runat="server" CssClass="field_heading" Text="Other service Price List - Selling Rates Pax Slab"
                                Width="837px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 111px">
                            PL Code</td>
                        <td style="width: 4px">
                            <asp:TextBox ID="txtPLCode" runat="server" CssClass="field_input" Enabled="False"
                                Width="172px" ReadOnly="True"></asp:TextBox></td>
                        <td style="width: 130px">
                            <%--<input id="txtBlockCode" runat="server" style="width: 89px" type="text" 
                                visible="false" readonly="readOnly" class="field_input" />--%></td>
                        <td style="width: 1px">
                        </td>
                        <td style="width: 172px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td_cell  " style="width: 111px">
                            SP Type</td>
                        <td style="width: 4px">
                            <select id="ddlSPCode" runat="server" class="drpdown"  onchange = "CallWebMethod('sptypecode');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell" style="width: 130px; text-align: left">
                            SP Name</td>
                        <td style="width: 1px; text-align: left">
                            <select id="ddlSPName" runat="server" class="drpdown"  onchange = "CallWebMethod('sptypename');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <%--<td align="left" class="td_cell" rowspan="8" valign="top" style="width: 172px">
                            <asp:RadioButtonList ID="rdSellingList" runat="server" Width="169px">
                            </asp:RadioButtonList></td>--%>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 111px">
                            Supplier Agent</td>
                        <td style="width: 4px">
                            <select id="ddlSupplierACode" runat="server" class="drpdown" onchange = "CallWebMethod('supagentcode');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell" style="width: 130px">
                            Supplier Agent Name</td>
                        <td style="width: 1px">
                            <select id="ddlSupplierAName" runat="server" class="drpdown" onchange = "CallWebMethod('supagentname');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 111px">
                            Supplier</td>
                        <td style="width: 4px">
                            <select id="ddlSupplierCode" runat="server" class="drpdown" onchange = "CallWebMethod('partycode');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell" style="width: 130px">
                            Supplier Name</td>
                        <td style="width: 1px">
                            <select id="ddlSupplierName" runat="server" class="drpdown" onchange = "CallWebMethod('partyname');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 111px">
                            Currency</td>
                        <td style="width: 4px">
                            <select id="ddlCurrencyCode" runat="server" class="drpdown" onchange = "CallWebMethod('currcode');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell" style="width: 130px">
                            Currency Name</td>
                        <td style="width: 1px">
                            <select id="ddlCurrencyName" runat="server" class="drpdown" onchange = "CallWebMethod('currname');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                     <%--<tr>
                       <td class="td_cell" style="width: 111px">
    Promotion Name</td>
                        <td style="width: 4px">
        <select ID="ddlPromotion" runat="server" class="fiel_input" name="D1" 
            onchange="CallWebMethod('promotion');" style="WIDTH: 178px" __designer:mapid="f4" 
                                disabled="disabled">
            <option selected="" __designer:mapid="f5"></option>
        </select></td>
                        <td class="td_cell" style="width: 130px">
                            Promotion Code</td>
                        <td style="width: 1px">
    <asp:TextBox ID="txtPromotionCode" runat="server" CssClass="txtbox" 
        Enabled="False" Width="181px"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="td_cell" style="width: 111px">
                            Sub Season</td>
                        <td style="width: 4px">
                            <select id="ddlSubSeasonCode" runat="server" class="drpdown" onchange = "CallWebMethod('subseascode');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell" style="width: 130px">
                            Sub Season Name</td>
                        <td style="width: 1px">
                            <select id="ddlSubSeasonName" runat="server" class="drpdown" onchange = "CallWebMethod('subseasname');"
                                style="width: 180px" disabled="disabled">
                                <option selected="selected"></option>
                            </select>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="td_cell" style="width: 111px" valign="top">
                            Revision Date</td>
                        <td valign="top" class="td_cell">



            <asp:TextBox ID="dpRevDate" runat="server" AutoPostBack="false" 
                CssClass="fiel_input" Width="80px" Enabled="False"></asp:TextBox>
            <cc1:CalendarExtender ID="dpRevsiondate_CalendarExtender" runat="server" 
                Format="dd/MM/yyyy" PopupButtonID="ImgPBtnFrmDt" 
                TargetControlID="dpRevDate">
            </cc1:CalendarExtender>
            &nbsp;<asp:ImageButton ID="ImgPBtnFrmDt" runat="server" 
                ImageUrl="~/Images/Calendar_scheduleHS.png" Enabled="False" />

</td>
                        <td colspan="2" valign="top">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 111px;">
                            Price List Type</td>
                        <td style="width: 4px;">
                            <asp:DropDownList ID="ddlPriceList" runat="server" AutoPostBack="True" CssClass="drpdown"
                                OnSelectedIndexChanged="ddlPriceList_SelectedIndexChanged" Width="177px" Enabled="False">
                                <asp:ListItem>[Select]</asp:ListItem>
                                <asp:ListItem>Normal Rates 1 Night</asp:ListItem>
                                <asp:ListItem>Weekend Rates 1 Night</asp:ListItem>
                                <asp:ListItem>Weekly Rates 7 Nights</asp:ListItem>
                                <asp:ListItem>Normal Rates &gt; 1 Night</asp:ListItem>
                                <asp:ListItem Value="Weekend Rates &gt; 1 Night"></asp:ListItem>
                            </asp:DropDownList></td>
                        <td style="width: 130px;">
                            <asp:CheckBox ID="ChkBManual" runat="server" CssClass="chkbox" OnCheckedChanged="ChkBManual_CheckedChanged"
                                Text="Manual" Width="67px" Enabled="False" /></td>
                        <td style="width: 1px;" class="td_cell">
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 14px;
                                height: 9px" type="text" class="field_input" /><asp:Label ID="lblselling" 
                                runat="server" Width="125px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" colspan="5">
                            <asp:CheckBox ID="ChkWeek1" runat="server" Text="Weekend Option 1" Visible="False" Enabled="False" />
                            <asp:Label ID="lblWEO1" runat="server" Text="Week end1 option"></asp:Label>
                            <asp:CheckBox ID="ChkWeek2" runat="server" CssClass="chkbox" Text="Weekend Option 2"
                                Visible="False" Enabled="False" />&nbsp;
                            <asp:Label ID="lblWEO2" runat="server" Text="Week end2 option"></asp:Label></td>
                    </tr>--%>
                    <tr>
                        <td class="td_cell" colspan="5"> <input id="txtconnection" runat="server" style="visibility: hidden; width: 14px;
                                height: 9px" type="text" class="field_input" /><asp:Label ID="lblselling" 
                                runat="server" Width="125px" ></asp:Label>
                                </td> </tr>
                            <table style="width: 100%">
                                <tr>
                                    <td class="td_cell" >
                <strong __designer:mapid="a3a">Dates</strong></td>
                                    <td>
                                        &nbsp;</td>
                                    <td class="td_cell" >
                                       <strong>Market</strong></td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                            <asp:Label ID="Label1" runat="server" CssClass="field_heading" Text="Selling Codes & Names"
                                Width="169px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                    <asp:GridView ID="grdDates" runat="server" AllowSorting="True" 
                        AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" 
                        CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical" 
                        tabIndex="12" Width="1px" Enabled="False">
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
                                        TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
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
                                        TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
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
                                    <td valign="top">
                                        &nbsp;</td>
                                    <td valign="top">
                                        <asp:Panel ID="Panel2" runat="server" CssClass="td_cell" Height="225px">
                                            <asp:GridView ID="gv_Market" runat="server" AutoGenerateColumns="False" 
                            BorderColor="#999999" BorderStyle="None" 
    BorderWidth="1px" CellPadding="3" 
                            CssClass="grdstyle" Font-Size="10px" 
    GridLines="Vertical" Width="360px" Enabled="False">
                                                <RowStyle CssClass="grdRowstyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Market Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcode" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("plgrpcode") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="400px" />
                                                        <ItemStyle Width="400px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="plgrpname" HeaderText="Market Name">
                                                    <HeaderStyle Width="1000px" />
                                                    <ItemStyle Width="1000px" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle CssClass="grdfooter" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <HeaderStyle CssClass="grdheader" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                    <td valign="top">
                                        &nbsp;</td>
                                    <td valign="top">
 <asp:Panel id="Panel1" runat="server" Height="225px" Width="550px" Wrap="true" 
         CssClass="td_cell">
        Sell Codes &amp; Names
        <br />
        <asp:Table ID="tbl" runat="server" ViewStateMode="Enabled">
        </asp:Table>
        <br />
        <br />
        
        </asp:Panel>
 
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr><%--OnClick="btnBack_Click" OnClick="btnGenerate_Click"OnClick="btnhelp_Click"--%>
                        <td class="td_cell" colspan="5">
                            <asp:Button ID="btnBack" runat="server" CssClass="btn" 
                                Text="Back" />&nbsp;
                            <asp:Button ID="btnGenerate" runat="server" CssClass="btn"
                                Text="Generate" />&nbsp;
                            <asp:Button ID="btnClose" runat="server" CssClass="btn" 
                                Text="Return To Search" />&nbsp;<asp:Button
                                ID="btnhelp" runat="server" CssClass="btn" 
                                Text="Help" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" colspan="5">
                            <asp:HiddenField ID="hdnpricelist" runat="server" />
                        </td>
                    </tr>
                </table>
           
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/ClsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>


</asp:Content>

