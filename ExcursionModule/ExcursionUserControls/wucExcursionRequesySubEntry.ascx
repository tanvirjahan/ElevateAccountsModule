
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="wucExcursionRequesySubEntry.ascx.vb"
    Inherits="ExcursionModule_ExcursionUserControls_wucExcursionRequesySubEntry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../../css/Styles.css" rel="stylesheet" type="text/css" />
<script src="../../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
<style type="text/css">
    .hiddencol
    {
        display: none;
    }
    
    .spanclass
    {
        text-decoration: none;
        display: inline;
        margin: 0;
        padding: 0px 0px 0px 30px;
        _padding: 0px 0px 0px 30px; /* this did the trick. Only IE6 should 
	padding-left:90px;*/
    }
    
    .ModalPopupBG
    {
        background-color: gray;
        filter: alpha(opacity=50);
        opacity: 0.7;
    }
    
    .HellowWorldPopup
    {
        min-width: 200px;
        min-height: 150px;
        background: white;
        font-size: 10pt;
        font-weight: bold;
        border-bottom-style: double;
        border-width: medium;
    }
    
    #TextArea1
    {
        height: 142px;
        width: 418px;
    }
    #Text1
    {
        width: 91px;
    }
    #Select1
    {
        width: 225px;
    }
    #Select3
    {
        width: 166px;
    }
    #txtNotes
    {
        height: 22px;
        width: 181px;
    }
    
    
    #ddlArrDep
    {
        width: 35px;
    }
    
    
    *
    {
        outline: none;
        font-weight: 700;
    }
    
    
    .HideControl
    {
        display: none;
    }
    .style1
    {
        width: 422px;
        text-align: right;
    }
</style>
<script type="text/javascript">
    var ddlSupplier1 = null;
    var ddlExcType1 = null;
    var txtCurrency1 = null;
    var txtAdultCostRate1 = null;
    var txtChildCostRate1 = null;
    var txtConvRate1 = null;
    var txtCostValue1 = null;
    var hdnSupplier1 = null;
    var hdnExcType1 = null;
    var hdnAdultCostRate1 = null;
    var hdnChildCostRate1 = null;
    var hdnCostCurrency1 = null;
    var hdnConversionRate1 = null;
    var hdnCostValue1 = null;
    var txtAdult1 = null;
    var txtChild1 = null;

    var txtnoofunits1 = null;
    var hdnnoofunits1 = null;

    $(document).ready(function () {
        CalculateTotal();
    })


    function f_grid_selectAll(sender) {
        var value = sender.checked;
        var grid = document.getElementById("<%= grdMultipleCost.ClientID %>");
        var inputList = grid.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            if (inputList[i].type == "checkbox") {
                inputList[i].checked = value;
            }
        }
    }

    function checkNumber(evt, txt) {        
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            if (charCode == 46) {
                return true;
            }
            return false;
        } else {
            return true;
        }


    }

    function OnChangeddlSupplierWuc(constr, ddlSupplier, txtCurrency, txtAdultCostRate, txtConvRate, txtCostValue, hdnSupplier, hdnAdultCostRate, hdnCostCurrency, hdnConversionRate, hdnCostValue, ddlExcType, txtTourDate, txtChildCostRate, hdnChildCostRate, txtAdult, txtChild, txtnoofunits,sender) {        
        ddlSupplier1 = document.getElementById(ddlSupplier);
        ddlExcType1 = document.getElementById(ddlExcType);

        txtCurrency1 = document.getElementById(txtCurrency);
        txtAdultCostRate1 = document.getElementById(txtAdultCostRate);
        txtChildCostRate1 = document.getElementById(txtChildCostRate);
        txtConvRate1 = document.getElementById(txtConvRate);
        txtCostValue1 = document.getElementById(txtCostValue);

        txtnoofunits1 = document.getElementById(txtnoofunits);

        txtTourDate = document.getElementById(txtTourDate);

        txtAdult1 = document.getElementById(txtAdult);
        txtChild1 = document.getElementById(txtChild);

        hdnSupplier1 = document.getElementById(hdnSupplier);
        hdnAdultCostRate1 = document.getElementById(hdnAdultCostRate);
        hdnChildCostRate1 = document.getElementById(hdnChildCostRate);
        
        hdnCostCurrency1 = document.getElementById(hdnCostCurrency);
        hdnConversionRate1 = document.getElementById(hdnConversionRate);
        hdnCostValue1 = document.getElementById(hdnCostValue);

        hdnSupplier1.value = ddlSupplier1.value;
        if (sender == 'supplier') {
            SetExcursionType(constr, ddlSupplier1.value);
        }
        
        ColServices.clsServices.GetCostPrices(constr, txtTourDate.value, txtTourDate.value, ddlExcType1.value, ddlSupplier1.value, GetCostCurrencyWuc_Rate, ErrorHandler, TimeOutHandler);
    }

    function GetCostCurrencyWuc_Rate(result) {
        var hdnTrnsfrType = document.getElementById("<%= hdnTrnsfrType.ClientID %>");
 
        if (result[0] != null && result[0] != NaN) {
            txtAdultCostRate1.value = toFixed(result[0],2);
            hdnAdultCostRate1.value = txtAdultCostRate1.value;
        }
        if (result[1] != null && result[1] != NaN) {
            txtChildCostRate1.value = toFixed(result[1],2);
            hdnChildCostRate1.value = txtChildCostRate1.value;
        }
        var adultCostVal = 0.0;
        var childCostVal = 0.0;

        if (hdnTrnsfrType.value == ddlExcType1.value) {
            adultCostVal = parseFloat(txtnoofunits1.value) * parseFloat(txtAdultCostRate1.value);
            txtnoofunits1.disabled = false;
        } else {
            if (txtAdult1.value != "" || txtAdult1.value != NaN) {
                adultCostVal = parseFloat(txtAdult1.value) * parseFloat(txtAdultCostRate1.value);
            }
            if (txtChild1.value != "" || txtChild1.value != NaN) {
                childCostVal = parseFloat(txtChild1.value) * parseFloat(txtChildCostRate1.value);
            }
            txtnoofunits1.value = 1;
            txtnoofunits1.disabled = true;
        }        
        txtConvRate1.value = 1.00;
        txtCostValue1.value = toFixed(parseFloat(adultCostVal) + parseFloat(childCostVal), "2");
        if (isNaN(txtCostValue1.value)) { txtCostValue1.value = 0.0 } else { txtCostValue1.value = toFixed(txtCostValue1.value,2) };
        hdnCostValue1.value = txtCostValue1.value;
        CalculateTotal();
    }

    function toFixed(value, precision) {
        var power = Math.pow(10, precision || 0);
        return String(Math.round(value * power) / power);
    }

//    function CalculateCostValue(constr, txtCurrency, txtAdultCostRate, txtConvRate, txtCostValue, hdnSupplier, hdnAdultCostRate, hdnCostCurrency, hdnConversionRate, hdnCostValue,txtChildCostRate,hdnChildCostRate) {
//        txtCurrency1 = document.getElementById(txtCurrency);
//        txtAdultCostRate1 = document.getElementById(txtAdultCostRate);
//        txtChildCostRate1 = document.getElementById(txtChildCostRate);
//        txtConvRate1 = document.getElementById(txtConvRate);
//        txtCostValue1 = document.getElementById(txtCostValue);
//        hdnCostRate1 = document.getElementById(hdnAdultCostRate);
//        hdnChildCostRate1 = document.getElementById(hdnChildCostRate);
//        hdnCostCurrency1 = document.getElementById(hdnCostCurrency);
//        hdnConversionRate1 = document.getElementById(hdnConversionRate);
//        hdnCostValue1 = document.getElementById(hdnCostValue);
//        if (txtConvRate1.value != null) {
//            if (txtAdultCostRate1.value != "") {
//                txtCostValue1.value = parseFloat(txtAdultCostRate1.value) * parseFloat(txtConvRate1.value);                
//                hdnCostValue1.value = txtCostValue1.value;
//            }
//        }
//        CalculateTotal();
//    }

    function CalculateCostValueByUnits(constr, txtAdultCostRate, txtnoofunits, txtCostValue, hdnSupplier, hdnAdultCostRate, hdnnoofunits, hdnCostValue, txtCovrate, txtChildCostRate, hdnChildCostRate, txtAdult, txtChild, ddlExcType) {
        var hdnTrnsfrType = document.getElementById("<%= hdnTrnsfrType.ClientID %>");        
        txtAdultCostRate1 = document.getElementById(txtAdultCostRate);
        txtChildCostRate1 = document.getElementById(txtChildCostRate);
        txtnoofunits1 = document.getElementById(txtnoofunits);
        txtCostValue1 = document.getElementById(txtCostValue);
        txtConvRate1 = document.getElementById(txtCovrate);
        ddlExcType1 = document.getElementById(ddlExcType);        
        hdnAdultCostRate1 = document.getElementById(hdnAdultCostRate);
        hdnChildCostRate1 = document.getElementById(hdnChildCostRate);

        txtAdult1 = document.getElementById(txtAdult);
        txtChild1 = document.getElementById(txtChild);
        hdnnoofunits1 = document.getElementById(hdnnoofunits);
        hdnCostValue1 = document.getElementById(hdnCostValue);
        var adultCostVal = 0.0;
        var childCostVal = 0.0;
        if (txtChildCostRate1.value == '') { txtChildCostRate1.value = 0 };
        if (txtAdultCostRate1.value == '') { txtAdultCostRate1.value = 0 };
        if (txtnoofunits1.value == '') { txtnoofunits1.value = 0 };

        if (hdnTrnsfrType.value == ddlExcType1.value) {
            adultCostVal = parseFloat(txtnoofunits1.value) * parseFloat(txtAdultCostRate1.value);            
            txtnoofunits1.disabled = false;
        } else {
            if (txtAdult1.value != "" || txtAdult1.value != NaN) {
                adultCostVal = parseFloat(txtAdult1.value) * parseFloat(txtAdultCostRate1.value);
            }

            if (txtChild1.value != "" || txtChild1.value != NaN) {
                childCostVal = parseFloat(txtChild1.value) * parseFloat(txtChildCostRate1.value);
            }
            txtnoofunits1.value = 1;
            txtnoofunits1.disabled = true;
        }
        txtConvRate1.value = 1.00;
        txtCostValue1.value = parseFloat(adultCostVal) + parseFloat(childCostVal);
        if (isNaN(txtCostValue1.value)) { txtCostValue1.value = 0.0 } else { txtCostValue1.value = toFixed(txtCostValue1.value, 2) };
        hdnCostValue1.value = txtCostValue1.value;
        CalculateTotal();
    }

    function CalculateTotal() {
        var grid = document.getElementById("<%= grdMultipleCost.ClientID %>");
        var total = 0.0;
        if (grid != null) {
            var inputList = grid.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                if (inputList[i].type == "text") {
                    var IdList = inputList[i].id.split("_");
                    var item = IdList.length;
                    if (IdList[item - 1] == "txtCostValue") {
                        if (inputList[i].value == '') { inputList[i].value = 0 }
                        total = parseFloat(total) + parseFloat(inputList[i].value);
                    }
                }
            }
            var txttot = document.getElementById("<%= txtTotal.ClientID %>");
            txttot.value = total;
            //var a = inputList[i].value;
        }
    }

    function SetExcursionType(constr, supplier) {        
        var hdnDefaultExcType = document.getElementById("<%= hdnDefaultExcType.ClientID %>");
        var hdnIsGroupReservation = document.getElementById("<%= hdnIsGroupReservation.ClientID %>");        
        if (supplier == '[Select]') {
            ddlExcType1.value = '[Select]';
            return;
        }

        if (hdnIsGroupReservation.value == "1") {
            $.ajax({
                type: "POST",
                url: "GroupReservationSummaryExcursion.aspx/GetExcTypeCodeFromGroup",
                data: '{"constr":"' + constr + '","supplier":"' + supplier + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d == 'TRFS') {
                        ddlExcType1.value = 'TOTALTRANS';
                    } else {
                        ddlExcType1.value = hdnDefaultExcType.value;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });

        } else {
            $.ajax({
                type: "POST",
                url: "ExcursionRequestSubEntry.aspx/GetExcTypeCode",
                data: '{"constr":"' + constr + '","supplier":"' + supplier + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d == 'TRFS') {
                        ddlExcType1.value = 'TOTALTRANS';
                    } else {
                        ddlExcType1.value = hdnDefaultExcType.value;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }        
    }

    function BindExcType(result) {        
//        var xmlDoc = $.parseXML(result);
//        var xml = $(xmlDoc);
//        var colExcType = xml.find("NewDataSet");
//        var nodeVal = '';
//        $.each(colExcType, function () {
//            var row = $(this);
//            var isIE = navigator.appName;
//            if (isIE == "Microsoft Internet Explorer") {
//                nodeVal = colExcType.rows[0].cells[0].childNodes[0].id;
//            }
//            else {
//                nodeVal = colExcType.rows[0].cells[0].children[0].id;
//                
//            }
//        });
//        
      
    }


</script>
<table>
    <tr>
        <td>
            <asp:UpdatePanel ID="upnlGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="PanelgridMultipleCost" Height="200px" ScrollBars="Vertical" runat="server"
                        BorderStyle="Double" BorderWidth="6px">
                        <asp:Label ID="lblGridNoRows" runat="server" Width="180px" Text="No records to display."
                            Visible="false" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="grdMultipleCost" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            Font-Names="Verdana" Font-Size="12px" ShowHeader="true" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle Width="1px" />
                                    <HeaderTemplate>
                                        <input id="chkAll" type="checkbox" runat="server" onclick="f_grid_selectAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="chk" type="checkbox" runat="server" />
                                        <asp:HiddenField ID="hdnRowID" runat="server" Value="<%# Bind('RowId') %>" />
                                        <asp:HiddenField ID="hdnrlineno" runat="server" Value="<%# Bind('rlineno') %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnDDLSupplier" runat="server" Value="<%# Bind('Supplier') %>" />
                                        <asp:DropDownList ID="ddlSupplier" runat="server" Width="180px">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excursion Type">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnExcType" runat="server" Value="<%# Bind('othtypcode') %>" />
                                        <asp:DropDownList ID="ddlExcType" runat="server" Width="180px">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adult Cost Rate">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnAdultCostRate" runat="server" Value="<%# Bind('AdultCostRate') %>" />
                                        <asp:TextBox ID="txtAdultCostRate" runat="server" Text="<%# Bind('AdultCostRate') %>"
                                            Width="40px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Child Cost Rate">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnChildCostRate" runat="server" Value="<%# Bind('ChildCostRate') %>" />
                                        <asp:TextBox ID="txtChildCostRate" runat="server" Text="<%# Bind('ChildCostRate') %>"
                                            Width="40px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Currency">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnCostCurrency" runat="server" Value="<%# Bind('CostCurrency') %>" />
                                        <asp:TextBox ID="txtCostCurrency" runat="server" Text="<%# Bind('CostCurrency') %>"
                                            Width="40px"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                    <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Conversion Rate">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnConversionRate" runat="server" Value="<%# Bind('ConversionRate') %>" />
                                        <asp:TextBox ID="txtConversionRate" runat="server" Text="<%# Bind('ConversionRate') %>"
                                            Width="40px"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                    <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tour Date">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnTourDate" runat="server" />
                                        <asp:TextBox ID="txtTourDate" runat="server" Width="63px" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adult">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnAdult" runat="server" />
                                        <asp:TextBox ID="txtAdult" runat="server" Width="40px" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Child">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnChild" runat="server" />
                                        <asp:TextBox ID="txtChild" runat="server" Width="40px" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No Of Units">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnnoofunits" runat="server" Value="<%# Bind('noofunits') %>" />
                                        <asp:TextBox ID="txtnoofunits" runat="server" Enabled="false" Text="<%# Bind('noofunits') %>"
                                            Width="40px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Value">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnCostValue" runat="server" Value="<%# Bind('CostValue') %>" />
                                        <asp:TextBox ID="txtCostValue" runat="server" Text="<%# Bind('CostValue') %>" Width="60px"
                                            Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnRemarks" runat="server" Value="<%# Bind('remarks') %>" />
                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Text="<%# Bind('remarks') %>"
                                            Width="90px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<table>
    <tr>
        <td class="style1">
            Total:
        </td>
        <td>            
            <input id="txtTotal" type="text" runat="server" class="txtbox" readonly="readonly" />
            <asp:HiddenField ID="hdnTrnsfrType" runat="server" Value='' />
            <asp:HiddenField ID="hdnDefaultExcType" runat="server" Value='' />
            <asp:HiddenField ID="hdnIsGroupReservation" runat="server" Value='0' />
        </td>
    </tr>
</table>
