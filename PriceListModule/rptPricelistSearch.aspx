<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptPricelistSearch.aspx.vb"
    Inherits="rptPricelistSearch" MasterPageFile="~/PriceListMaster.master" Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <%-- <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .ModalPopupBG
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("[id*=chkAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=chk2]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkAll]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chk2]", grid).length == $("[id*=chk2]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    


    </script>
    <%--  Supplier Catgeory Code Added Ends--%><%-- Supplier Catgeory Code Added Starts  --%><%--  Supplier Catgeory Code Added Ends--%>
    <script language="javascript" type="text/javascript">




        function CallWebMethod(methodType) {
            switch (methodType) {
                case "sptype":
                    var select = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlSpTypeName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value


                    ColServices.clsServices.GetCatCodeListnew(constr, sptype, FillCatCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCatNameListnew(constr, sptype, FillCatNames, ErrorHandler, TimeOutHandler);


                    ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCatCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCatNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);


                    var select = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = select.options[select.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, null, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, null, FillRoomTypeNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;

                case "sptypename":
                    var select = document.getElementById("<%=ddlSpTypeName.ClientID%>");
                    var sptype = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsptypecode.ClientID%>").value = sptype;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetCatCodeListnew(constr, sptype, FillCatCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCatNameListnew(constr, sptype, FillCatNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCatCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCatNames, ErrorHandler, TimeOutHandler);

                    var select = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = select.options[select.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    var select = document.getElementById("<%=ddlPartyCode.ClientID%>");
                    var party = select.options[select.selectedIndex].text;

                    ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, party, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, party, FillRoomTypeNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;

                case "catcode":
                    var select = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlCatName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdncategory.ClientID%>").value = cat;

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;
                case "catname":
                    var select = document.getElementById("<%=ddlCatName.ClientID%>");
                    var cat = select.options[select.selectedIndex].value;
                    var selectname = document.getElementById("<%=ddlCCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdncategory.ClientID%>").value = cat;
                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;

                case "ctrycode":

                    var select = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = select.options[select.selectedIndex].text;
                    var selectname = document.getElementById("<%=ddlcontName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnctrycode.ClientID%>").value = ctry;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;

                case "ctryname":
                    var select = document.getElementById("<%=ddlcontName.ClientID%>");
                    var ctry = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlContCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnctrycode.ClientID%>").value = ctry;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;
                case "citycode":
                    var select = document.getElementById("<%=ddlCityCode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlCityName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                    document.getElementById("<%=hdncitycode.ClientID%>").value = city;
                    var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                    txtcode.value = city;

                    var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].value;

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;
                case "cityname":
                    var select = document.getElementById("<%=ddlCityName.ClientID%>");
                    var city = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlCityCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdncitycode.ClientID%>").value = null;
                    document.getElementById("<%=hdncitycode.ClientID%>").value = city;

                    var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                    txtcode.value = city;

                    var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].text;


                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSupplierCodeAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSupplierNameAllListnew(constr, sptype, cat, null, ctry, city, FillSupplierNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('Supplier');
                    break;
                case "scatcode":
                    var select = document.getElementById("<%=ddlscatcode.ClientID%>");
                    var city = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlscatname.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsellcatcode.ClientID%>").value = city;

                    var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                    txtcode.value = city;

                    var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].value;

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    //                ColServices.clsServices.GetSellCatCodeListnew(constr, sptype,  FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                    //                ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);


                    break;
                case "scatname":

                    var select = document.getElementById("<%=ddlscatname.ClientID%>");
                    var city = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlscatcode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsellcatcode.ClientID%>").value = city;

                    var txtcode = document.getElementById("<%=txtCityCode.ClientID%>");
                    txtcode.value = city;

                    var txtname = document.getElementById("<%=txtCityName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].text;


                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;

                    var selectcatcode = document.getElementById("<%=ddlCCode.ClientID%>");
                    var cat = selectcatcode.options[selectcatcode.selectedIndex].text;

                    var selectcountry = document.getElementById("<%=ddlContCode.ClientID%>");
                    var ctry = selectcountry.options[selectcountry.selectedIndex].text;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value




                    break;
                case "suppliercode":
                    var select = document.getElementById("<%=ddlPartyCode.ClientID%>");
                    var party = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlPartyName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                    var txtcode = document.getElementById("<%=txtSupplierCode.ClientID%>");
                    txtcode.value = party;

                    var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].value;


                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                    var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                    //    ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                    //    ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                    //                ColServices.clsServices.GetSellCatCodeListnew(constr, sptype, FillSellCategoryCodes, ErrorHandler, TimeOutHandler);
                    //                ColServices.clsServices.GetSellCatNameListnew(constr, sptype, FillSellCategoryNames, ErrorHandler, TimeOutHandler);



                    ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, party, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetCatCityCode(constr, txtcode.value, FillCatCity, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, party, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, party, FillRoomTypeNames, ErrorHandler, TimeOutHandler);






                    FillCurrencyCode('Supplier');
                    break;
                case "suppliername":
                    var select = document.getElementById("<%=ddlPartyName.ClientID%>");
                    var party = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlPartyCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;

                    document.getElementById("<%=hdnsuppliercode.ClientID%>").value = party;
                    var partycode = selectname.options[selectname.selectedIndex].value;

                    var txtcode = document.getElementById("<%=txtSupplierCode.ClientID%>");
                    txtcode.value = party;

                    var txtname = document.getElementById("<%=txtSupplierName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].text;

                    var selectsptype = document.getElementById("<%=ddlSPTypeCode.ClientID%>");
                    var sptype = selectsptype.options[selectsptype.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value



                    var ddlcontName = document.getElementById("<%=ddlcontName.ClientID%>");
                    var ctry = ddlcontName.options[ddlcontName.selectedIndex].value;

                    //     ColServices.clsServices.GetCityCodeListnew(constr, ctry, FillCityCodes, ErrorHandler, TimeOutHandler);
                    //     ColServices.clsServices.GetCityNameListnew(constr, ctry, FillCityNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSellingCategoryCodeBySupplier(constr, party, FillSellingCategoryCodeAndName, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetCatCityCode(constr, txtcode.value, FillCatCity, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetRoomTypeCodeListnew(constr, sptype, party, FillRoomTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetRoomTypeNameListnew(constr, sptype, party, FillRoomTypeNames, ErrorHandler, TimeOutHandler);


                    FillCurrencyCode('Supplier');

                    break;

                case "marketcode":
                    var select = document.getElementById("<%=ddlMarketCode.ClientID%>");
                    var plgrp = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlMarketName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnmarketcode.ClientID%>").value = plgrp;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSellCodeListnew(constr, plgrp, FillSellCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellNameListnew(constr, plgrp, FillSellNames, ErrorHandler, TimeOutHandler);

                    FillCurrencyCode('SellType');

                    break;

                case "marketname":
                    var select = document.getElementById("<%=ddlMarketName.ClientID%>");
                    var plgrp = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlMarketCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnmarketcode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSellCodeListnew(constr, plgrp, FillSellCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellNameListnew(constr, plgrp, FillSellNames, ErrorHandler, TimeOutHandler);


                    FillCurrencyCode('SellType');
                    break;

                case "sellcode":
                    var select = document.getElementById("<%=ddlSellingCode.ClientID%>");
                    var sellcat = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlSellingName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    var hdnsellingtype = document.getElementById("hdnsellingtype")
                    hdnsellingtype.value = sellcat
                    alert(hdnmarketcode.value);
                    var txtcode = document.getElementById("<%=txtSellingTypeCode.ClientID%>");
                    txtcode.value = sellcat;

                    var txtname = document.getElementById("<%=txtSellingTypeName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].value;

                    FillCurrencyCode('SellType');
                    break;
                case "sellname":
                    var select = document.getElementById("<%=ddlSellingName.ClientID%>");
                    var sellcat = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlSellingCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnsellingtype.ClientID%>").value = sellcat;
                    var txtcode = document.getElementById("<%=txtSellingTypeCode.ClientID%>");
                    txtcode.value = sellcat;

                    var txtname = document.getElementById("<%=txtSellingTypeName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].text;

                    FillCurrencyCode('SellType');
                    break;

                case "agentcode":
                    var select = document.getElementById("<%=ddlAgentCode.ClientID%>");
                    var agent = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlAgentName.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnagentcode.ClientID%>").value = agent;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetMktCodeListnew(constr, agent, FillMarketCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetMktNameListnew(constr, agent, FillMarketNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSellingTypeCodeListnew(constr, agent, FillSellTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellingTypeNameListnew(constr, agent, FillSellTypeNames, ErrorHandler, TimeOutHandler);

                    break;
                //               Added agentcode by Archana on 24/05/2015     
                case "agentname":
                    var select = document.getElementById("<%=ddlAgentName.ClientID%>");
                    var agent = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlAgentCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnagentcode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetMktCodeListnew(constr, agent, FillMarketCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetMktNameListnew(constr, agent, FillMarketNames, ErrorHandler, TimeOutHandler);

                    ColServices.clsServices.GetSellingTypeCodeListnew(constr, agent, FillSellTypeCodes, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellingTypeNameListnew(constr, agent, FillSellTypeNames, ErrorHandler, TimeOutHandler);


                    break;
                //                 Added agentname by Archana on 24/05/2015      

                case "seascode":
                    var select = document.getElementById("<%=ddlseas1code.ClientID%>");
                    var seascode = select.options[select.selectedIndex].text;

                    var selectname = document.getElementById("<%=ddlseas1name.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnseasoncode.ClientID%>").value = seascode;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSeasdatenew(constr, seascode, FillSeasdate, ErrorHandler, TimeOutHandler);
                    break;
                case "seasname":
                    var select = document.getElementById("<%=ddlseas1name.ClientID%>");
                    var seascode = select.options[select.selectedIndex].value;

                    var selectname = document.getElementById("<%=ddlseas1code.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnseasoncode.ClientID%>").value = seascode;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value

                    ColServices.clsServices.GetSeasdatenew(constr, seascode, FillSeasdate, ErrorHandler, TimeOutHandler);

                    break;

                case "rmtypcode":
                    var select = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                    var selectname = document.getElementById("<%=ddlRmtypename.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.value;
                    var txtcode = document.getElementById("<%=txtRoomTypeCode.ClientID%>");
                    txtcode.value = select.options[select.selectedIndex].value;

                    var txtname = document.getElementById("<%=txtRoomTypeName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].value;


                    break;

                case "rmtypname":
                    var select = document.getElementById("<%=ddlRmtypename.ClientID%>");
                    var selectname = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
                    selectname.value = select.options[select.selectedIndex].text;
                    document.getElementById("<%=hdnroomtypecode.ClientID%>").value = selectname.options[selectname.selectedIndex].text;
                    var txtcode = document.getElementById("<%=txtRoomTypeCode.ClientID%>");
                    txtcode.value = selectname.options[selectname.selectedIndex].value;

                    var txtname = document.getElementById("<%=txtRoomTypeName.ClientID%>");
                    txtname.value = select.options[select.selectedIndex].text;

                    break;

            }
        }


        function FillCatCodes(result) {
            var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillCatNames(result) {
            var ddl = document.getElementById("<%=ddlCatName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCategoryCodes(result) {
            var ddl = document.getElementById("<%=ddlscatcode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCategoryNames(result) {
            var ddl = document.getElementById("<%=ddlscatname.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCatCodes(result) {
            var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCatNames(result) {
            var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }



        function FillCityCodes(result) {
            var ddl = document.getElementById("<%=ddlCityCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillCityNames(result) {
            var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSupplierCodes(result) {
            var ddl = document.getElementById("<%=ddlPartyCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillSupplierNames(result) {
            var ddl = document.getElementById("<%=ddlPartyName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSellCodes(result) {
            var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillSellNames(result) {
            var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }



        function FillMarketCodes(result) {
            var ddl = document.getElementById("<%=ddlMarketCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            var ddlAgent = document.getElementById("<%=ddlAgentName.ClientID%>");
            var hdnmarketcode = document.getElementById("<%=hdnmarketcode.ClientID%>");
            if (ddlAgent.value == "[Select]") {
                ddl.value = "[Select]";
                hdnmarketcode.value = "[Select]";
            }
        }

        //      Added FillMarketCodes function by Archana on 24/05/2015
        function FillMarketNames(result) {

            var ddl = document.getElementById("<%=ddlMarketName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            var ddlAgent = document.getElementById("<%=ddlAgentName.ClientID%>");
            var hdnmarketcode = document.getElementById("<%=hdnmarketcode.ClientID%>");
            if (ddlAgent.value == "[Select]") {
                ddl.value = "[Select]";
                hdnmarketcode.value = "[Select]";
            }
        }

        //      Added FillMarketNames function by Archana on 24/05/2015
        function FillSellTypeCodes(result) {

            var ddl = document.getElementById("<%=ddlSellingCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            var ddlAgent = document.getElementById("<%=ddlAgentName.ClientID%>");
            var hdnmarketcode = document.getElementById("<%=hdnmarketcode.ClientID%>");
            if (ddlAgent.value == "[Select]") {
                ddl.value = "[Select]";
                hdnmarketcode.value = "[Select]";
            }
            //ddl.value = "[Select]";
        }
        //    Added FillSellTypeCodes function by Archana on 24/05/2015
        function FillSellTypeNames(result) {
            var ddl = document.getElementById("<%=ddlSellingName.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            var ddlAgent = document.getElementById("<%=ddlAgentName.ClientID%>");
            var hdnmarketcode = document.getElementById("<%=hdnmarketcode.ClientID%>");
            if (ddlAgent.value == "[Select]") {
                ddl.value = "[Select]";
                hdnmarketcode.value = "[Select]";
            }
            //ddl.value = "[Select]"
        }
        //    Added FillSellTypeNames function by Archana on 24/05/2015
        function FillCatCity(result) {
            var ddlCityName = document.getElementById("<%=ddlCityName.ClientID%>");
            var ddlCityCode = document.getElementById("<%=ddlCityCode.ClientID%>");
            var ddlCCode = document.getElementById("<%=ddlCCode.ClientID%>");
            var ddlCatName = document.getElementById("<%=ddlCatName.ClientID%>");
            document.getElementById("<%=hdncitycode.ClientID%>").value = null;
            var txtcitycode = document.getElementById("<%=txtCityCode.ClientID%>");
            var txtCityName = document.getElementById("<%=txtCityName.ClientID%>");

            ddlCityName.value = result[0].ListValue;
            ddlCatName.value = result[0].ListText;
            document.getElementById("<%=hdncitycode.ClientID%>").value = result[0].ListValue;
            document.getElementById("<%=hdncategory.ClientID%>").value = null;
            document.getElementById("<%=hdncategory.ClientID%>").value = result[0].ListText;




            for (var i = 0; i < ddlCityCode.length - 1; i++) {
                if (ddlCityCode.options[i].text == result[0].ListValue) {
                    ddlCityCode.selectedIndex = i;
                    break;
                }
            }
            for (var i = 0; i < ddlCCode.length - 1; i++) {
                if (ddlCCode.options[i].text == result[0].ListText) {
                    ddlCCode.selectedIndex = i;
                    break;
                }
            }

            txtcitycode.value = ddlCityName.value;
            txtCityName.value = ddlCityCode.value;

            //        ddlCityCode.value = ddlCityName.options[ddlCityName.selectedIndex].text;
            //        ddlCCode.value = ddlCatName.options[ddlCatName.selectedIndex].text;

        }

        function FillSellingCategoryCodeAndName(result) {
            var ddlscatcode = document.getElementById("<%=ddlscatcode.ClientID%>");
            var ddlscatname = document.getElementById("<%=ddlscatname.ClientID%>");
            for (var i = 0; i < ddlscatcode.length - 1; i++) {
                if (ddlscatcode.options[i].text == result) {
                    ddlscatcode.selectedIndex = i;
                    ddlscatname.selectedIndex = i;
                    break;
                }
            }
        }
        function FillRoomTypeCodes(result) {
            var ddl = document.getElementById("<%=ddlRmtypeCode.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }
        function FillRoomTypeNames(result) {
            var ddl = document.getElementById("<%=ddlRmtypename.ClientID%>");
            RemoveAll(ddl)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddl.options.add(option);
            }
            ddl.value = "[Select]";
        }

        function FillSeasdate(result) {



            var fdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var tdate = document.getElementById("<%=txtToDate.ClientID%>");

            //        document.getElementById("<%=hdnfromdate.ClientID%>").value = result[0].ListText;
            //        document.getElementById("<%=hdntodate.ClientID%>").value = result[0].ListText;



            fdate.value = result[0].ListText;
            tdate.value = result[0].ListText;

            //        window.opener.document.getElementById('ctl00_Main_dtpFromDate').txtDate.Text=d1.value;
            //        window.close();

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


        function checkTelephoneNumber(e) {

            if ((event.keyCode < 45 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }

        function checkNumberhtml(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                return true;
            }
            return false;
        }

        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

        }
        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            //var txttdate=document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { ColServices.clsServices.GetQueryReturnFromToDate('FromDate', 30, txtfdate.value, FillToDate, ErrorHandler, TimeOutHandler); }
        }
        function FillToDate(result) {
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
            txttdate.value = result.substring(0, 5) + '/2020';
        }

        function FillCurrencyCode(typ) {
            var strQry = "";
            if (typ == "Supplier") {
                var ddlSupCode = document.getElementById("<%=ddlPartyCode.ClientID%>");
                var ddlSupname = document.getElementById("<%=ddlPartyName.ClientID%>");
                var ddlSellCode = document.getElementById("<%=ddlSellingCode.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                if (ddlSellCode.value == "[Select]") {
                    strQry = "select p.currcode,r.convrate from partymast p,currrates r where p.currcode=r.currcode and p.partycode='" + ddlSupname.value + "' and r.tocurr in ( select option_selected from reservation_parameters where param_id=457)";
                    ColServices.clsServices.GetQueryReturnStringArraynew(constr, strQry, 2, FillCurrCode, ErrorHandler, TimeOutHandler);
                }
            }
            else if (typ == "SellType") {
                var ddlSellCode = document.getElementById("<%=ddlSellingCode.ClientID%>");
                strQry = "select s.currcode,r.convrate from sellmast s,currrates r where s.currcode=r.currcode and s.sellcode='" + ddlSellCode.options[ddlSellCode.selectedIndex].text + "' and r.tocurr in ( select option_selected from reservation_parameters where param_id=457)";
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, strQry, 2, FillCurrCode, ErrorHandler, TimeOutHandler);

                if (ddlSellCode.value == "[Select]") {
                    var ddlSupCode = document.getElementById("<%=ddlPartyCode.ClientID%>");
                    if (ddlSupCode.value != "[Select]") {
                        strQry = "select p.currcode,r.convrate from partymast p,currrates r where p.currcode=r.currcode and p.partycode='" + ddlSupCode.options[ddlSupCode.selectedIndex].text + "' and r.tocurr in ( select option_selected from reservation_parameters where param_id=457)";
                        ColServices.clsServices.GetQueryReturnStringArraynew(constr, strQry, 2, FillCurrCode, ErrorHandler, TimeOutHandler);
                    }

                }
                //       else
                //       {
                //       
                //       }
            }
        }

        function FillCurrCode(result) {
            var txtcurrcode = document.getElementById("<%=txtCurrency.ClientID%>");
            if (result[0] == null) { result[0] = ''; }
            txtcurrcode.value = result[0];
        }


        function valid() {
            var ddlctrycode = document.getElementById("<%=ddlcontCode.ClientID%>");
            var ddlcitycode = document.getElementById("<%=ddlCityCode.ClientID%>");

            if (ddlctrycode.value == '[Select]' || ddlctrycode.value == '') {
                alert('Select  Country');
                return false;
            }
            if (ddlcitycode.value == '[Select]' || ddlcitycode.value == '') {
                alert('Select  City');
                return false;
            }
            return true;
        }

    </script>
    <table>
        <tr>
            <td style="width: 100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center">
                            Price List
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <span class="td_cell" style="color: #ff0000"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSupplierTypeCode" runat="server" Text="Supplier Type Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlSPTypeCode" class="drpdown" tabindex="1" onchange="CallWebMethod('sptype')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblsuppliertypename" runat="server" Text="Supplier Type Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 238px" id="ddlSpTypeName" class="drpdown" tabindex="2" onchange="CallWebMethod('sptypename')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCountryCode" runat="server" Text="Country Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlContCode" class="drpdown" tabindex="3" onchange="CallWebMethod('ctrycode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblcountryname" runat="server" Text="Country Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlcontName" class="drpdown" tabindex="4" onchange="CallWebMethod('ctryname')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCityCode" runat="server" Text="City Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlCityCode" class="drpdown" tabindex="5" onchange="CallWebMethod('citycode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblcityname" runat="server" Text="City Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 117px" id="ddlCityName" class="drpdown" tabindex="6" onchange="CallWebMethod('cityname')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                    <asp:CheckBox ID="chkexcept" TabIndex="7" runat="server" Text="Except Selected" CssClass="td_cell"
                                                        Width="91px"></asp:CheckBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCategorycode" runat="server" Text="Category Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlCCode" class="drpdown" tabindex="8" onchange="CallWebMethod('catcode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlCatName" class="drpdown" tabindex="9" onchange="CallWebMethod('catname')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <%-- Supplier Catgeory Code Added Starts  --%>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSupplierCode0" runat="server" CssClass="td_cell" Text="Selling Category Code"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select id="ddlscatcode" runat="server" class="drpdown" name="D1" onchange="CallWebMethod('scatcode')"
                                                        style="width: 200px" tabindex="10" visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblSupplierName0" runat="server" CssClass="td_cell" Text="Selling Category Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <select id="ddlscatName" runat="server" class="drpdown" name="D2" onchange="CallWebMethod('scatname')"
                                                        style="width: 237px" tabindex="11" visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <%--  Supplier Catgeory Code Added Ends--%>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSupplierCode" runat="server" Text="Supplier Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlPartyCode" class="drpdown" tabindex="12" onchange="CallWebMethod('suppliercode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblSupplierName" runat="server" Text="Supplier Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlPartyName" class="drpdown" tabindex="13" onchange="CallWebMethod('suppliername')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Room Type Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlRmtypeCode" class="drpdown" tabindex="14" onchange="CallWebMethod('rmtypcode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="Label3" runat="server" Text="Room Type Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlRmtypename" class="drpdown" tabindex="15" onchange="CallWebMethod('rmtypname')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMarketCode" runat="server" Text="Market Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlMarketCode" class="drpdown" tabindex="16" onchange="CallWebMethod('marketcode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblMarketName" runat="server" Text="Market Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlMarketName" class="drpdown" tabindex="17" onchange="CallWebMethod('marketname')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 99px">
                                                    <asp:Label ID="lblSellingCategoryCode" runat="server" Text="Selling Type Code" CssClass="td_cell"
                                                        Width="123px"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select id="ddlSellingCode" runat="server" class="drpdown" onchange="CallWebMethod('sellcode')"
                                                        style="width: 200px" tabindex="18" visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblsellingcategoryname" runat="server" CssClass="td_cell" Text="Selling Type Name"
                                                        Width="124px"></asp:Label>
                                                </td>
                                                <td>
                                                    <select id="ddlSellingName" runat="server" class="drpdown" onchange="CallWebMethod('sellname')"
                                                        style="border-color: #000000; border-style: solid; border-width: 0.5px;" tabindex="19"
                                                        visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAgentCode" runat="server" Text="Agent Code" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select style="width: 200px" id="ddlAgentCode" class="drpdown" tabindex="20" onchange="CallWebMethod('agentcode')"
                                                        runat="server" visible="true">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblAgentName" runat="server" Text="Agent Name" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <select style="width: 237px" id="ddlAgentName" class="drpdown" tabindex="21" onchange="CallWebMethod('agentname')"
                                                        runat="server" visible="true">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <%-- Added Agent Code and Agent Name by Archana on 21/05/2015--%>
                                            <tr>
                                                <td colspan="4">
                                                    <div id="divcost" runat="server" style="border-style: solid; border-width: 0.5px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblmsg" runat="server" CssClass="td_cell" ForeColor="#C00000" Text="( * Leave Selling Type Code Field as Blank to Print Net Cost)"
                                                                                    Width="400px"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label11" runat="server" CssClass="td_cell" Text="Print Cost"></asp:Label>
                                                                                &nbsp;&nbsp;
                                                                                <asp:RadioButton ID="rbcostnet" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                                                    Checked="True" CssClass="search_button" ForeColor="Black" GroupName="costSearch"
                                                                                    TabIndex="22" Text="Net Payable" wfdid="w6" Width="80px" />
                                                                                &nbsp;
                                                                                <asp:RadioButton ID="rbcosthotel" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                                                    CssClass="search_button" ForeColor="Black" GroupName="costSearch" TabIndex="23"
                                                                                    Text="Hotel Cost" Width="80px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;<asp:Label ID="Label4" runat="server" CssClass="td_cell" Text="Season Code"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <select id="ddlseas1code" runat="server" class="drpdown" name="D3" onchange="CallWebMethod('seascode')"
                                                        style="width: 200px" tabindex="24" visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="Label5" runat="server" Text="Season Name" CssClass="td_cell" Width="124px"></asp:Label>
                                                </td>
                                                <td>
                                                    <select id="ddlseas1name" runat="server" class="drpdown" onchange="CallWebMethod('seasname')"
                                                        style="width: 200px" tabindex="25" visible="true">
                                                        <option selected=""></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td style="width: 176px">
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                    <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" ControlExtender="MEFromDate"
                                                        ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format">
                                                    </cc1:MaskedEditValidator>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblTodate" runat="server" Text="To Date" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                    <asp:ImageButton ID="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:MaskedEditValidator ID="MEVToDate" runat="server" ControlExtender="METoDate"
                                                        ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText=" "
                                                        EmptyValueMessage="Date is required" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format">
                                                    </cc1:MaskedEditValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Sell Type" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td class="td_cell" style="width: 300px">
                                                    <asp:RadioButton ID="rball" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                        Checked="True" CssClass="search_button" ForeColor="Black" GroupName="GrSearch"
                                                        TabIndex="26" Text="All" Width="60px" />
                                                    &nbsp;
                                                    <asp:RadioButton ID="rbbeach" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                        CssClass="search_button" ForeColor="Black" GroupName="GrSearch" TabIndex="27"
                                                        Text="Beach" Visible="False" wfdid="w6" Width="60px" />
                                                    <asp:RadioButton ID="rbcity" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                        CssClass="search_button" ForeColor="Black" GroupName="GrSearch" TabIndex="28"
                                                        Text="City" Visible="False" Width="60px" />
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="lblpolicyselect" runat="server" Text="Print Meal Plan" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbmealyes" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                        Checked="True" CssClass="search_button" ForeColor="Black" GroupName="MealSearch"
                                                        TabIndex="29" Text="Yes" wfdid="w6" Width="80px" />
                                                    &nbsp;<asp:RadioButton ID="rbmealno" runat="server" AutoPostBack="True" BorderColor="#404040"
                                                        CssClass="search_button" ForeColor="Black" GroupName="MealSearch" TabIndex="30"
                                                        Text="No" Width="80px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblatatus" runat="server" Text="Approval Status" CssClass="td_cell"
                                                        Width="103px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlapprovestatus" runat="server" CssClass="drpdown" Width="191px">
                                                        <asp:ListItem Value="1">Approved</asp:ListItem>
                                                        <asp:ListItem Value="0">Unapproved</asp:ListItem>
                                                        <asp:ListItem Value="2">All</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 207px">
                                                    <asp:Label ID="Label8" runat="server" CssClass="td_cell" Text="Show in Web" Width="103px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlshowweb" runat="server" CssClass="drpdown" Width="80px">
                                                        <asp:ListItem Value="2">All</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Updated as on" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUpdateAsOn" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgbtnUpdateAsOn" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                    <cc1:MaskedEditValidator ID="MEVUpdateAsOn" runat="server" ControlExtender="MEUpdatAsOn"
                                                        ControlToValidate="txtUpdateAsOn" CssClass="field_error" Display="Dynamic" EmptyValueMessage="Date is required"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format">
                                                    </cc1:MaskedEditValidator>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkproviderfilter" runat="server" CssClass="chkbox" Text="Show Selected Providers for price list only "
                                                        Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" Text="Currency" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td>
                                                    <input id="txtCurrency" class="txtbox" type="text" runat="server" readonly />
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chkDisplayRates" runat="server" CssClass="chkbox" Text="Display Rates in"
                                                        Width="208px" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input id="txtConversionRate" class="txtbox" type="text" maxlength="15" runat="server"
                                                        visible="False" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblConvRate" runat="server" CssClass="td_cell" Text="Conversion To"
                                                        Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 207px">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" Text="Report Type" CssClass="td_cell"></asp:Label>
                                                </td>
                                                <td colspan="3" class="td_cell" style="color: #c00000">
                                                    <asp:DropDownList ID="ddlrpttype" runat="server" CssClass="drpdown" Width="191px">
                                                        <%--  <asp:ListItem Value="0">Portrait</asp:ListItem>--%>
                                                        <asp:ListItem Value="1">Landscape</asp:ListItem>
                                                    </asp:DropDownList>
                                                    Remarks can see only in Landscape
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="Label7" runat="server" CssClass="td_cell" Text="Remarks" Visible="False"></asp:Label>
                                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="td_cell" Height="40px" TabIndex="31"
                                                        TextMode="MultiLine" Visible="False" Width="532px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" class="td_cell" style="color: #c00000;">
                                                 <asp:Button ID="btnSelect" runat="server" Text="Select Countries" CssClass="btn" /> &nbsp;
                                                    <asp:TextBox ID="txtbox" runat="server" ReadOnly="true" CssClass="txtbox" 
                                                        Width="550px"></asp:TextBox> <br />
                                           Select Countries for promotion if report has to be filtered by country specific promotions
                                                    <asp:HiddenField ID="hdntxtbox" runat="server" />
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="4">
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtCityCode" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtCityName" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtSellingTypeCode"
                                                        type="text" runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtSellingTypeName"
                                                        type="text" runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtAgentCode" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtAgentName" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtSupplierCode" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtSupplierName" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtRoomTypeCode" type="text"
                                                        runat="server" />
                                                    <input style="visibility: hidden; width: 9px; height: 3px" id="txtRoomTypeName" type="text"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: center">
                                                    <asp:Button ID="BtnClear" runat="server" CssClass="btn" TabIndex="32" Text="Clear" />
                                                    &nbsp;
                                                    <asp:Button ID="BtnPrint" runat="server" CssClass="btn" OnClientClick="return valid();"
                                                        TabIndex="33" Text="Load Report" />
                                                    &nbsp;
                                                    <asp:Button ID="btnhelp" runat="server" CssClass="btn" OnClick="btnhelp_Click" TabIndex="34"
                                                        Text="Help" />
                                                    <input style="visibility: hidden; width: 12px; height: 9px" id="txtconnection" type="text"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <div id="ShowCountries" runat="server" style="overflow: scroll; height: 200px; width: 450px;
                                                    border: 3px solid green; background-color: White; display: none">
                                                    <asp:GridView ID="gv_ShowCountries" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" CssClass="td_cell" Width="475px">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ctrycode" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="txtcountrycode" runat="server" Text='<%# Bind("ctrycode") %>'></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkAll" />
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chk2" Width="10px" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ctrycode" SortExpression="ctrycode" HeaderText="Country Code">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ctryname" SortExpression="ctryname" HeaderText="Country name">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                                        <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                                        <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                                    </asp:GridView>
                                                    <table style="float: left">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOk1" runat="server" CssClass="field_button" Text="Ok" Width="80px" />&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClear1" runat="server" CssClass="field_button" Text="Clear" Width="80px" />&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                    CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                    PopupControlID="ShowCountries" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                    BackgroundCssClass="ModalPopupBG">
                                                </cc1:ModalPopupExtender>
                                                <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                <input id="btnOkayEB" type="button" value="OK" style="visibility: hidden" />
                                                <input id="btnCancelEB" type="button" value="Cancel" style="visibility: hidden" />
                                            </td>
                                        </tr>
                                    </table>
                                    <cc1:CalendarExtender ID="CEFromDate" runat="server" TargetControlID="txtFromDate"
                                        PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                                        Mask="99/99/9999" MaskType="Date">
                                    </cc1:MaskedEditExtender>
                                    <cc1:CalendarExtender ID="CEToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="ImgBtnToDate"
                                        Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                                        Mask="99/99/9999" MaskType="Date">
                                    </cc1:MaskedEditExtender>
                                    <cc1:CalendarExtender ID="CEUpdateAsOn" runat="server" TargetControlID="txtUpdateAsOn"
                                        PopupButtonID="imgbtnUpdateAsOn" Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MEUpdatAsOn" runat="server" TargetControlID="txtUpdateAsOn"
                                        Mask="99/99/9999" MaskType="Date">
                                    </cc1:MaskedEditExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
    <CR:CrystalReportViewer ID="CRVSupplierPolicies" runat="server" AutoDataBind="true"
        HasCrystalLogo="False" ToolbarImagesFolderUrl="images/crystaltoolbar/" />
    <select id="ddlSeasoncode" runat="server" class="drpdown" onchange="CallWebMethod('seascode')"
        style="width: 170px" tabindex="35" visible="false">
        <option selected="selected"></option>
    </select>
    <select id="ddlSeasonName" runat="server" class="drpdown" onchange="CallWebMethod('seasname')"
        style="width: 237px" tabindex="36" visible="false">
        <option selected="selected"></option>
    </select>
    <br />
    <asp:HiddenField ID="hdnsptypecode" runat="server" />
    <asp:HiddenField ID="hdncategory" runat="server" />
    <asp:HiddenField ID="hdnsellcatcode" runat="server" />
    <asp:HiddenField ID="hdnctrycode" runat="server" />
    <asp:HiddenField ID="hdncitycode" runat="server" />
    <asp:HiddenField ID="hdnsuppliercode" runat="server" />
    <asp:HiddenField ID="hdnroomtypecode" runat="server" />
    <asp:HiddenField ID="hdnmarketcode" runat="server" />
    <asp:HiddenField ID="hdnsellingtype" runat="server" />
    <asp:HiddenField ID="hdnagentcode" runat="server" />
    <asp:HiddenField ID="hdnsellpricecode" runat="server" />
    <asp:HiddenField ID="hdnseasoncode" runat="server" />
    <asp:HiddenField ID="hdnfromdate" runat="server" />
    <asp:HiddenField ID="hdntodate" runat="server" />
</asp:Content>
