<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="CurrencyConversionRates.aspx.vb" Inherits="CurrencyConversionRates"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script type="text/javascript">
<!--
        // WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >
    //window.history.forward();
    function checkNumber(e) {
        if (event.keyCode == 46)
        { return true; }

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }


    function ValueStore() {
        var ddltocurr = document.getElementById("<%=ddlToCurrency.ClientID%>");
        var ddlfromcurrency = document.getElementById("<%=ddlFromCurrency.clientID%>");
        var hdnfromcurr = document.getElementById("<%=hdnfromcurrency.ClientID%>");
        var hdntocurr = document.getElementById("<%=hdntoCurrency.ClientID%>");
        hdnfromcurr.value = ddlfromcurrency.value;
        hdntocurr.value = ddltocurr.value;
    }
</script>

    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;">
        <tr>
            <td style="text-align: center;" class="field_heading" align="center">
                <asp:Label ID="lblheading" runat="server" CssClass="field_heading" Text="Currency Conversion Rates"
                    Width="100%"></asp:Label></td>
        </tr>
        <tr>
            <td class="td_cell" style="text-align: left">
                <table class="td_cell">
                    <tr>
                        <td style="width: 100px">
                            From Currency</td>
                        <td style="width: 100px">
                            <select id="ddlFromCurrency" runat="server" class="drpdown" onchange="ValueStore();" style="width: 120px">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td style="width: 100px">
                            To Currency</td>
                        <td style="width: 100px">
                            <select id="ddlToCurrency" runat="server" onchange="ValueStore();" class="drpdown" style="width: 120px">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td style="width: 54px">
                            <asp:UpdatePanel id="UpdatePanel3" runat="server">
                                <contenttemplate>
                            <asp:Button ID="btnFill" runat="server" CssClass="search_button" Text="Fill" 
                                        Font-Bold="True" />
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="td_cell" style="text-align: left">
                <table>
                    <tr>
                        <td>
                <asp:Button ID="btnExcel" runat="server" CssClass="btn" OnClick="btnExcel_Click"
                    Text="Export To Excel" /></td>
                        <td>
                            <asp:Button ID="btnRep" runat="server" CssClass="btn" Font-Bold="False"
                    Text="Report" /></td>
                        <td>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:Button id="btnhelp" runat="server" Text="Help" Font-Bold="False" CssClass="btn"></asp:Button>
</contenttemplate>
    </asp:UpdatePanel></td>
                        <td>
                            <asp:Button ID="btnClose" runat="server" CssClass="btn" Font-Bold="True"
                    Text="Exit" Visible="False" /></td>
                        <td>
                            &nbsp;<asp:Button ID="btnUpdate" runat="server" CssClass="btn" Font-Bold="False"
                    Text="Save" Visible="False" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td class="td_cell" style="width: 100%; text-align: left">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<DIV style="HEIGHT: 250px; width: 100%;" class="container">
    <asp:GridView id="gv_SearchResult" runat="server" Width="100%" 
        CssClass="grdstyle1" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="frmcurrcode" HeaderText="From Currency"></asp:BoundField>
<asp:TemplateField HeaderText="Conversion">
<ItemStyle Width="130px" HorizontalAlign="Right"></ItemStyle>

<HeaderStyle Width="130px" HorizontalAlign="Right"></HeaderStyle>
<ItemTemplate>
<INPUT style="WIDTH: 128px; TEXT-ALIGN: right" id="txtConversion" class="field_input" type=text maxLength=12 runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tocurr" SortExpression="tocurr" HeaderText="To Currency"></asp:BoundField>
<asp:BoundField DataField="convrate" HeaderText="Conversion Rate"></asp:BoundField>
<asp:TemplateField Visible="False" HeaderText="From Currency"><ItemTemplate>
<asp:Label id="lblFromCurrency" runat="server" Text='<%# Bind("currcode") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>

<HeaderStyle CssClass="grdheader"></HeaderStyle>
</asp:GridView>
    <br />
</DIV>
</contenttemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;" class="td_cell">
                <asp:Button ID="btnSave" runat="server"
                            CssClass="btn" Font-Bold="True" Text="Save" />&nbsp;<asp:Button
                                ID="btnExit" runat="server" CssClass="btn" Font-Bold="True" 
                    Text="Exit" />&nbsp;<asp:Button ID="btnReport" runat="server" CssClass="btn"
                        Font-Bold="True" Text="Report" Visible="False" /> &nbsp;
                <asp:Button ID="btnExport" runat="server" CssClass="btn" Text="Export To Excel" 
                    Visible="False" /></td>
                    
<td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
    </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hdnfromcurrency" runat="server" />
     <asp:HiddenField ID="hdntocurrency" runat="server" />

</asp:Content>

