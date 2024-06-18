<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master"  EnableEventValidation="true" AutoEventWireup="false" CodeFile="DeskExcursionRequestSubEntry.aspx.vb" Inherits="ExcursionModule_DeskExcursionRequestSubEntry" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/ExcursionModule/ExcursionUserControls/wucExcursionRequesySubEntry.ascx"
    TagName="wucExcursionRequestSubEntry" TagPrefix="wuc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <style type="text/css">
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
            width: 250px;
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
    </style>
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <script src="../Content/js/AutoComplete.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function FillFlightDetails(flightTranID) {

            var ddlFlightNo = document.getElementById("<%=ddlFlightNo.ClientID%>");
            var hdnFlightNo = document.getElementById("<%=hdnFlightNo.ClientID%>");
            var ddlFlightType = document.getElementById("<%=ddlFlightType.ClientID%>");

            var txtArrivalDate = document.getElementById("<%=txtArrivalDate.ClientID%>");
            var txtDepartureDate = document.getElementById("<%=txtDepartureDate.ClientID%>");

            var type = ddlFlightType.options[ddlFlightType.selectedIndex].value;

            ddlFlightNo.value = flightTranID;

            hdnFlightNo.value = ddlFlightNo.options[ddlFlightNo.selectedIndex].value;
            var transferTypeDateValue = ""
            if (type = 1) {
                transferTypeDateValue = txtArrivalDate.value
            }
            else {
                transferTypeDateValue = txtDepartureDate.value
            }

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value;

            if (hdnFlightNo.value != "[Select]") {

                ColServices.clsServices.GetFlightTimeExcursion(constr, type, transferTypeDateValue, hdnFlightNo.value, FillFlightTime, ErrorHandler, TimeOutHandler);
            }

        }

        function DecRound(amtToRound) {

            var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
            nodecround = Math.pow(10, parseInt(txtdec.value));
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
        }


        function ShowFlightDiv() {
            var ShowFlightHelp = document.getElementById("<%=ShowFlightHelp.ClientID%>");
            ShowFlightHelp.style.display = "block";

        }

        function HideFlightDiv() {
            var ShowFlightHelp = document.getElementById("<%=ShowFlightHelp.ClientID%>");
            ShowFlightHelp.style.display = "none";

        }

        function Showticketdetdiv() {
            var ShowFlightHelp = document.getElementById("<%=showtkts.ClientID%>");
            ShowFlightHelp.style.display = "block";

        }

        function Hideticketdetdiv() {
            var ShowFlightHelp = document.getElementById("<%=showtkts.ClientID%>");
            ShowFlightHelp.style.display = "none";

        }

        function FormValidation(state) {

            var ddlExcursionGroup = document.getElementById("<%=ddlExcursionGroup.ClientID%>");
            var ddlExcursionSubGroup = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");
            var ddlHotel = document.getElementById("<%=ddlHotel.ClientID%>");
            var ddlExcursionProvider = document.getElementById("<%=ddlExcursionProvider.ClientID%>");
            var txtGuestName = document.getElementById("<%=txtGuestName.ClientID%>");

            var chkCancel = document.getElementById("<%=chkCancel.ClientID%>");
            var txtCancelReason = document.getElementById("<%=txtCancelReason.ClientID%>");
            var chkFreeToCustomer = document.getElementById("<%=chkFreeToCustomer.ClientID%>");
            var txtAdultRate = document.getElementById("<%=txtAdultRate.ClientID%>");
            var txtChildRate = document.getElementById("<%=txtChildRate.ClientID%>");
            var txtChild = document.getElementById("<%=txtChild.ClientID%>");



            if (ddlExcursionGroup.value == "[Select]" || ddlExcursionGroup.value == "") {
                ddlExcursionGroup.focus();
                alert("Please select Excursion Group");
                return false;
            }

            if (ddlExcursionSubGroup.value == "[Select]" || ddlExcursionSubGroup.value == "") {
                ddlExcursionSubGroup.focus();
                alert("Please select Excursion Sub Group");
                return false;
            }

            if (txtGuestName.value == "") {
                txtGuestName.focus();
                alert("Please enter Guest Name");
                return false;
            }

            if (ddlHotel.value == "[Select]" || ddlHotel.value == "") {
                ddlHotel.focus();
                alert("Please select Hotel");
                return false;
            }

            //        if (ddlExcursionProvider.value == "[Select]" || ddlExcursionProvider.value == "") {
            //            ddlExcursionProvider.focus();
            //            alert("Please select Excursion Provider");
            //            return false;
            //        }
            // validation for cost and selling amount
            var txtAmountAED = document.getElementById("<%=txtAmountAED.ClientID%>");
            var txtCostAmountAED = document.getElementById("<%=txtCostAmountAED.ClientID%>");

            if (parseFloat(txtCostAmountAED.value) > parseFloat(txtAmountAED.value)) {
                alert("Cost is greater than selling");
                return false;
            }

            //if (chkCancel.checked == true && txtCancelReason.value == "") {
            //                alert("Please enter cancel reason");
            //                return false;
            //            }

            //            if (chkFreeToCustomer.checked == false && txtAdultRate.value == "0") {
            //                alert("Adult Rate cannot be 0");
            //                return false;
            //            }

            if (txtAdultRate.value == "0") {
                alert("Adult Rate cannot be 0");
                return false;
            }

            if (txtChildRate.value == "0" && txtChild.value != "0") {
                if (confirm('Child Rate is 0. Do you want to continue??') == true) {
                    return true;
                }
                else {
                    return false;
                }

            }


        }




        function CallWebMethod(methodType) {
            switch (methodType) {

                case "FilterSubGroup":


                    var select = document.getElementById("<%=ddlExcursionGroup.ClientID%>");
                    var ddlExcursionGroup = document.getElementById("<%=ddlExcursionGroup.ClientID%>");
                    var codeid = select.options[select.selectedIndex].value;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;

                    var ddlExcursionSubGroup = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");
                    var ddlExcursionType = document.getElementById("<%=ddlExcursionType.ClientID%>");
                    var hdnExcursionSubGroupCode = document.getElementById("<%=hdnExcursionSubGroupCode.ClientID%>");
                    var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");

                    hdnExcursionSubGroupCode.value = ddlExcursionSubGroup.options[ddlExcursionSubGroup.selectedIndex].value;
                    hdnExcursionTypeCode.value = ddlExcursionType.options[ddlExcursionType.selectedIndex].value;

                    var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
                    hdnExcursionGroupCode.value = ddlExcursionGroup.options[ddlExcursionGroup.selectedIndex].value;

                    if (codeid != '') {

                        ColServices.clsServices.GetExcursionSubGroup(constr, codeid, FillExcursionSubGroup, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetExcursionGroupType(constr, codeid, FillExcursionType, ErrorHandler, TimeOutHandler);

                    }

                    break;

                case "ExcursionSubGroup":
                    var select = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");
                    var hdnExcursionSubGroupCode = document.getElementById("<%=hdnExcursionSubGroupCode.ClientID%>");
                    hdnExcursionSubGroupCode.value = select.options[select.selectedIndex].value;
                    break;

                case "ExcursionType":

                    var select = document.getElementById("<%=ddlExcursionType.ClientID%>");
                    var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                    var txtTourDate = document.getElementById("<%=txtTourDate.ClientID%>");
                    hdnExcursionTypeCode.value = select.options[select.selectedIndex].value;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;

                    var hdnSellingTypeForWS = document.getElementById("<%=hdnSellingTypeForWS.ClientID%>");
                    var hdnSpersonCodeForWS = document.getElementById("<%=hdnSpersonCodeForWS.ClientID%>");

                    ColServices.clsServices.GetMain_SubGroupByType(constr, hdnExcursionTypeCode.value, GetMain_Sub_Excursion, ErrorHandler, TimeOutHandler);
                    //txtTourDate.value = '10/04/2014'
                    //ColServices.clsServices.GetSellingPrices(constr, txtTourDate.value, txtTourDate.value, 'BARDHOW', 'DGT', 'DIMITRY', GetSellingPrices, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetSellingPrices(constr, txtTourDate.value, txtTourDate.value, hdnExcursionTypeCode.value, hdnSellingTypeForWS.value, hdnSpersonCodeForWS.value, GetSellingPrices, ErrorHandler, TimeOutHandler);
                    //ColServices.clsServices.GetExcursionProvider(constr, txtTourDate.value, txtTourDate.value, 'BOUQUET', FillExcursionProvider, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetExcursionProvider(constr, txtTourDate.value, txtTourDate.value, hdnExcursionTypeCode.value, FillExcursionProvider, ErrorHandler, TimeOutHandler);

                    break;

                case "ExcursionProvider":

                    var select = document.getElementById("<%=ddlExcursionProvider.ClientID%>");
                    var hdnExcursionProvider = document.getElementById("<%=hdnExcursionProvider.ClientID%>");
                    var txtTourDate = document.getElementById("<%=txtTourDate.ClientID%>");
                    hdnExcursionProvider.value = select.options[select.selectedIndex].value;

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;

                    var ddlExcursionType = document.getElementById("<%=ddlExcursionType.ClientID%>");
                    var othtypecode = ddlExcursionType.options[ddlExcursionType.selectedIndex].value;
                    var ddlExcursionSubGroup = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");

                    ColServices.clsServices.GetCostCurr_CostConvRate(constr, hdnExcursionProvider.value, GetCostCurrency_Rate, ErrorHandler, TimeOutHandler);
                    //txtTourDate.value = '10/04/2014'
                    //ColServices.clsServices.GetCostPrices(constr, txtTourDate.value, txtTourDate.value, 'GILFCLUB6', 'A-000075', GetCostPrices, ErrorHandler, TimeOutHandler);
                    ColServices.clsServices.GetCostPrices(constr, txtTourDate.value, txtTourDate.value, othtypecode, hdnExcursionProvider.value, GetCostPrices, ErrorHandler, TimeOutHandler);
                    break;

                case "FlightType":
                    var select = document.getElementById("<%=ddlFlightType.ClientID%>");
                    var hdnFlightType = document.getElementById("<%=hdnFlightType.ClientID%>");
                    var type = select.options[select.selectedIndex].value;
                    hdnFlightType.value = type;
                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;
                    ColServices.clsServices.GetFlightCodeExcursion(constr, type, FillFlightNo, ErrorHandler, TimeOutHandler);
                    break;

                case "FlightNo":
                    var ddlFlightNo = document.getElementById("<%=ddlFlightNo.ClientID%>");
                    var hdnFlightNo = document.getElementById("<%=hdnFlightNo.ClientID%>");
                    var ddlFlightType = document.getElementById("<%=ddlFlightType.ClientID%>");

                    var txtArrivalDate = document.getElementById("<%=txtArrivalDate.ClientID%>");
                    var txtDepartureDate = document.getElementById("<%=txtDepartureDate.ClientID%>");

                    var type = ddlFlightType.options[ddlFlightType.selectedIndex].value;
                    hdnFlightNo.value = ddlFlightNo.options[ddlFlightNo.selectedIndex].value;
                    var transferTypeDateValue = ""
                    if (type = 1) {
                        transferTypeDateValue = txtArrivalDate.value
                    }
                    else {
                        transferTypeDateValue = txtDepartureDate.value
                    }

                    var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                    constr = connstr.value;

                    if (hdnFlightNo.value != "[Select]") {

                        ColServices.clsServices.GetFlightTimeExcursion(constr, type, transferTypeDateValue, hdnFlightNo.value, FillFlightTime, ErrorHandler, TimeOutHandler);
                    }


                    break;
            }
        }


        function FillFlightTime(result) {

            var ddlFlightType = document.getElementById("<%=ddlFlightType.ClientID%>");
            var txtPickTime = document.getElementById("<%=txtPickTime.ClientID%>");
            var txtFlightTime = document.getElementById("<%=txtFlightTime.ClientID%>");
            var txtAirport = document.getElementById("<%=txtAirport.ClientID%>");
            var type = ddlFlightType.options[ddlFlightType.selectedIndex].value;
            if ((result.length) > 0) {
                if ((result[0]) != null)
                    if (type = 0) {
                        txtPickTime.value = result[0];
                        txtAirport.value = result[1];
                    }
                    else {
                        txtFlightTime.value = result[0];
                        txtAirport.value = result[1];
                    }
                else {
                    txtFlightTime.value = "";
                    txtPickTime.value = "";
                    txtAirport.value = "";
                }
            }
        }


        function GetMain_Sub_Excursion(result) {
       
            var ddlExcursionGroup = document.getElementById("<%=ddlExcursionGroup.ClientID%>");
            var ddlExcursionSubGroup = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");
            var hdnExcursionGroupCode = document.getElementById("<%=hdnExcursionGroupCode.ClientID%>");
            var hdnExcursionSubGroupCode = document.getElementById("<%=hdnExcursionSubGroupCode.ClientID%>");
            
            
            if (result[0] != null && result[0] != NaN) {
                ddlExcursionGroup.value = result[0];
                hdnExcursionGroupCode.value = result[0];
                }

            if (result[1] != null && result[1] != NaN) {
                ddlExcursionSubGroup.value = result[1];
                hdnExcursionSubGroupCode.value = result[1];

            }

        }

        function GetCostCurrency_Rate(result) {

            var txtCostCurr = document.getElementById("<%=txtCostCurr.ClientID%>");
            var txtCostConvRate = document.getElementById("<%=txtCostConvRate.ClientID%>");

            var hdnCostCurr = document.getElementById("<%=hdnCostCurr.ClientID%>");
            var hdnCostCurrConvRate = document.getElementById("<%=hdnCostCurrConvRate.ClientID%>");




            if (result[0] != null && result[0] != NaN) {
                txtCostCurr.value = result[0];
                hdnCostCurr.value = txtCostCurr.value;
            }

            if (result[1] != null && result[1] != NaN) {
                txtCostConvRate.value = result[1];
                hdnCostCurrConvRate.value = txtCostConvRate.value;
            }

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


        function FillExcursionSubGroup(result) {
            if (result.length > 0) {
                var hdnExcursionSubGroupCode = document.getElementById("<%=hdnExcursionSubGroupCode.ClientID%>");
                var ddlExcursionSubGroup = document.getElementById("<%=ddlExcursionSubGroup.ClientID%>");
                RemoveAll(ddlExcursionSubGroup)

                for (var i = 0; i < result.length; i++) {
                    var option = new Option(result[i].ListText, result[i].ListValue);
                    ddlExcursionSubGroup.options.add(option);
                }

                hdnExcursionSubGroupCode.value = ddlExcursionSubGroup.options[ddlExcursionSubGroup.selectedIndex].value;

            }
        }


        function FillExcursionType(result) {
            if (result.length > 0) {
                var ddlExcursionType = document.getElementById("<%=ddlExcursionType.ClientID%>");
                var hdnExcursionTypeCode = document.getElementById("<%=hdnExcursionTypeCode.ClientID%>");
                RemoveAll(ddlExcursionType)

                for (var i = 0; i < result.length; i++) {
                    var option = new Option(result[i].ListText, result[i].ListValue);
                    ddlExcursionType.options.add(option);
                }
                hdnExcursionTypeCode.value = ddlExcursionType.options[ddlExcursionType.selectedIndex].value;
            }
        }

        function FillFlightNo(result) {
            if (result.length > 0) {
                var ddlFlightNo = document.getElementById("<%=ddlFlightNo.ClientID%>");
                RemoveAll(ddlFlightNo)

                for (var i = 0; i < result.length; i++) {
                    var option = new Option(result[i].ListText, result[i].ListValue);
                    ddlFlightNo.options.add(option);
                }

            }
        }

        function GetSellingPrices(result) {

            var txtAdultRate = document.getElementById("<%=txtAdultRate.ClientID%>");
            var txtSPersonComPer = document.getElementById("<%=txtSPersonComPer.ClientID%>");
            var txtChildRate = document.getElementById("<%=txtChildRate.ClientID%>");

            var hdnAdultRate = document.getElementById("<%=hdnAdultRate.ClientID%>");
            var hdnChildRate = document.getElementById("<%=hdnChildRate.ClientID%>");
            var hdnSPersonComPer = document.getElementById("<%=hdnSPersonComPer.ClientID%>");

            if (result[0] != "" && result[0] != NaN) {
                txtAdultRate.value = DecRound(result[0]);
            }
            else {
                txtAdultRate.value = 0;
            }

            if (result[1] != "" && result[1] != NaN) {
                txtChildRate.value = DecRound(result[1]);
            }
            else {
                txtChildRate.value = 0;
            }

            if (result[2] != "" && result[2] != NaN) {
                txtSPersonComPer.value = DecRound(result[2]);
            }
            else {
                txtSPersonComPer.value = 0;
            }




            //hdnAdultRate.value = result[0];
            //hdnChildRate.value = result[1];
            //hdnSPersonComPer.value = result[2];

            calculateAdultChildRate();


        }

        function GetCostPrices(result) {

            var txtAdultCostRate = document.getElementById("<%=txtAdultCostRate.ClientID%>");
            var txtChildCostRate = document.getElementById("<%=txtChildCostRate.ClientID%>");

            if (result[0] != "" && result[0] != NaN) {
                txtAdultCostRate.value = DecRound(result[0]);
            }
            else {
                if (txtAdultCostRate.value == "" || isNaN(txtAdultCostRate.value) || txtAdultCostRate.value == 0) {
                    txtAdultCostRate.value = 0
                }
            }

            if (result[1] != "" && result[1] != NaN) {
                txtChildCostRate.value = DecRound(result[1]);
            }
            else {
                if (txtChildCostRate.value == "" || isNaN(txtChildCostRate.value) || txtChildCostRate.value == 0) {
                    txtChildCostRate.value = 0
                }
            }


            //    hdnAdultRate.value = result[0];
            //    hdnChildRate.value = result[1];
            //    hdnSPersonComPer.value = result[2];

            calculateAdultChildCostRate();

        }

        function FillExcursionProvider(result) {

            if (result.length > 0) {
                var ddlExcursionProvider = document.getElementById("<%=ddlExcursionProvider.ClientID%>");
                var hdnExcursionProvider = document.getElementById("<%=hdnExcursionProvider.ClientID%>");

                RemoveAll(ddlExcursionProvider)

                for (var i = 0; i < result.length; i++) {
                    var option = new Option(result[i].ListText, result[i].ListValue);
                    ddlExcursionProvider.options.add(option);
                }
                hdnExcursionProvider.value = ddlExcursionProvider.options[ddlExcursionProvider.selectedIndex].value;

                var txtTourDate = document.getElementById("<%=txtTourDate.ClientID%>");
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value;
                var ddlExcursionType = document.getElementById("<%=ddlExcursionType.ClientID%>");
                var othtypecode = ddlExcursionType.options[ddlExcursionType.selectedIndex].value;
                //16082014
                if (ddlExcursionType.value == "[Select]") {
                    ddlExcursionProvider.selectedIndex = result.length - 1
                }
                ddlExcursionProvider.value = "[Select]";
                hdnExcursionProvider.value = "[Select]";
                ColServices.clsServices.GetCostCurr_CostConvRate(constr, hdnExcursionProvider.value, GetCostCurrency_Rate, ErrorHandler, TimeOutHandler);

                ColServices.clsServices.GetCostPrices(constr, txtTourDate.value, txtTourDate.value, othtypecode, hdnExcursionProvider.value, GetCostPrices, ErrorHandler, TimeOutHandler);
            }

        }

        function calculateAdultChildRate() {


            var txtAdult = document.getElementById("<%=txtAdult.ClientID%>");
            var txtAdultRate = document.getElementById("<%=txtAdultRate.ClientID%>");
            var txtChild = document.getElementById("<%=txtChild.ClientID%>");
            var txtChildRate = document.getElementById("<%=txtChildRate.ClientID%>");
            var txtAmount = document.getElementById("<%=txtAmount.ClientID%>");
            var txtAmountAED = document.getElementById("<%=txtAmountAED.ClientID%>");
            var txtSPersonComPer = document.getElementById("<%=txtSPersonComPer.ClientID%>");
            var txtSPersonCom = document.getElementById("<%=txtSPersonCom.ClientID%>");

            var ExConvRate = '<%= Session("ExConvRate") %>';

            var hdnAmount = document.getElementById("<%=hdnAmount.ClientID%>");
            var hdnAmountAED = document.getElementById("<%=hdnAmountAED.ClientID%>");

            var txtTotalAmountAED = document.getElementById("<%=txtTotalAmountAED.ClientID%>");



            if (txtChild.value == '') {
                txtChild.value = 0;
            }

            if (txtAdult.value == '') {
                alert('Please Enter Adult');
                txtAmount.value = 0;
                txtAmountAED.value = 0;
                txtSPersonCom.value = 0;
                return false;
            }

            if (parseInt(txtAdult.value) == 0) {
                alert('Adult should be greater than or equal to one');
                txtAmount.value = 0;
                txtAmountAED.value = 0;
                txtSPersonCom.value = 0;
                return false;
            }
            else {
                //alert('I am in else');
                txtAmount.value = ((parseFloat(txtAdult.value) * parseFloat(txtAdultRate.value)) + (parseFloat(txtChild.value) * parseFloat(txtChildRate.value)))
                txtAmount.value = DecRound(txtAmount.value);
                hdnAmount.value = txtAmount.value;




                txtAmountAED.value = (parseFloat(txtAmount.value) * parseFloat(ExConvRate))
                txtAmountAED.value = DecRound(txtAmountAED.value);
                hdnAmountAED.value = txtAmountAED.value;

                txtTotalAmountAED.value = hdnAmountAED.value;


                txtSPersonCom.value = (parseFloat(txtAmountAED.value) * (parseFloat(txtSPersonComPer.value) / 100))
                txtSPersonCom.value = DecRound(txtSPersonCom.value);

                calculateAdultChildCostRate();

                return true;
            }


        }


        function calculateAdultChildCostRate() {



            var txtAdult = document.getElementById("<%=txtAdult.ClientID%>");
            var txtAdultCostRate = document.getElementById("<%=txtAdultCostRate.ClientID%>");
            var txtChild = document.getElementById("<%=txtChild.ClientID%>");
            var txtChildCostRate = document.getElementById("<%=txtChildCostRate.ClientID%>");
            var txtCostAmount = document.getElementById("<%=txtCostAmount.ClientID%>");
            var txtCostAmountAED = document.getElementById("<%=txtCostAmountAED.ClientID%>");
            var txtCostConvRate = document.getElementById("<%=txtCostConvRate.ClientID%>");
            var hdnCostAmount = document.getElementById("<%=hdnCostAmount.ClientID%>");
            var hdnCostAmountAED = document.getElementById("<%=hdnCostAmountAED.ClientID%>");
            var ddlExcursionProvider = document.getElementById("<%=ddlExcursionProvider.ClientID%>");
            var hdnIsMultipleCost = document.getElementById("<%=hdnIsMultipleCost.ClientID%>");

            var ExConvRate = '<%= Session("ExConvRate") %>';


            if (txtChild.value == '') {
                txtChild.value = 0;
            }

            if (txtAdult.value == '') {
                alert('Please Enter Adult');
                txtCostAmount.value = 0;
                txtCostAmountAED.value = 0;
                return false;
            }

            if (parseInt(txtAdult.value) == 0) {
                alert('Adult should be greater than or equal to one');
                txtCostAmount.value = 0;
                txtCostAmountAED.value = 0;
                return false;
            }
            else {
                if (hdnIsMultipleCost.value == 0 || hdnIsMultipleCost.value == "") {

                    txtCostAmount.value = ((parseFloat(txtAdult.value) * parseFloat(txtAdultCostRate.value)) + (parseFloat(txtChild.value) * parseFloat(txtChildCostRate.value)))
                    txtCostAmount.value = DecRound(txtCostAmount.value);
                    hdnCostAmount.value = txtCostAmount.value;

                    //txtCostAmountAED.value = (parseFloat(txtCostAmount.value) * parseFloat(ExConvRate))
                    txtCostAmountAED.value = (parseFloat(txtCostAmount.value) * parseFloat(txtCostConvRate.value))
                    if (isNaN(txtCostAmountAED.value)) { txtCostAmountAED.value = 0; }
                    txtCostAmountAED.value = DecRound(txtCostAmountAED.value);
                    hdnCostAmountAED.value = txtCostAmountAED.value;
                    return true;
                }
                else {
                    if (txtCostAmountAED.value > 0) {
                        alert('This excursion cost is already given from multiple cost. if you want to chage cost rate, Please change the adult or child cost rate from multiple cost');
                    }
                }
            }
        }



        function OnlyNumber(evt) {

            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
    ((evt.which) ? evt.which : 0));

            if (charCode != 47 && (charCode > 45 && charCode < 58)) {
                return true;
            }
            if (charCode == 8) {
                return true;
            }

            return false;

        }


        function filltxthotel() {
            var ddlhotel = document.getElementById("<%=ddlhotel.ClientID%>");
            var txthotel = document.getElementById("<%=txthotel.ClientID%>");
            txthotel.value = ddlhotel.options[ddlhotel.selectedIndex].text;

        }

        function disconfno() {
            var chksupconf = document.getElementById("<%=chksupconf.ClientID%>");
            var txtsupconfno = document.getElementById("<%=txtsupconfno.ClientID%>");
            var lblsupconf = document.getElementById("<%=lblsupconf.ClientID%>");

            if (chksupconf.checked) {
                txtsupconfno.style.display = "block"
                lblsupconf.style.display = "block"
            }
            else {
                txtsupconfno.style.display = "none"
                lblsupconf.style.display = "none"
            }

        }


    </script>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table style="border: gray 2px solid; width: 1000px">
                <tbody>
                    <tr>
                        <td class="field_heading" colspan="6" align="center">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Excursion Sub Entry" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Tour Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtTourDate" runat="server" CssClass="txtbox" TabIndex="1" ValidationGroup="MKE"
                                Width="80px"/>
                            <asp:ImageButton ID="ImgBtnTourDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="54" />
                            <cc1:MaskedEditValidator ID="MEVTourDate" runat="server" ControlExtender="MEETourDate"
                                ControlToValidate="txtTourDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                EmptyValueMessage="Date is required" ErrorMessage="" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator>
                            <cc1:MaskedEditExtender ID="MEETourDate" TargetControlID="txtTourDate" runat="server"
                                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                MaskType="Date" MessageValidatorTip="true">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CLFTourDate" TargetControlID="txtTourDate" PopupButtonID="ImgBtnTourDate"
                                runat="server" Format="dd/MM/yyyy" />
                        </td>
                         <td>
                        </td>
                                                                      
                        <td class="td_cell">
                          <asp:Label ID="lblcancel"  Text="Cancel Reason" runat="server" Visible="false"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtCancelReason" runat="server" CssClass="txtbox" TabIndex="2"
                                Width="120px" />
                        </td>
                    </tr>
                    <tr>
                     <td class="td_cell">
                            Excursion Type
                        </td>
                        <td>
                            <select style="width: 250px" id="ddlExcursionType" onchange="CallWebMethod('ExcursionType')"
                                class="field_input" runat="server" tabindex="2">
                                <option selected="selected"></option>
                            </select>
                        </td>
                               <td class="td_cell">
                            <asp:CheckBox ID="chkFreeToCustomer" Text="Free to Customer" runat="server" TabIndex="4" />
                        </td>
                        <td class="td_cell">
                            <asp:CheckBox ID="chkFreeFromSupplier" Text="Free From Supplier" runat="server" TabIndex="5" />
                        </td>
                                
                         
                    </tr>
                   <%-- <tr>
                        
                        <td>
                        </td>
                        <td>
                        </td>
                       
                    </tr>--%>
                    <%--<tr></tr>commented by Archana on 18/03/2015--%>
                   <%-- <tr>
                       
                        <td>
                        </td>
                        <td>
                        </td>
                      

                    </tr> tr is commented by Archana on 18/03/2015 --%>
                    <tr>
                        <td class="td_cell">
                            Guest Name
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtGuestName" runat="server" CssClass="txtbox" Style="text-transform: uppercase"
                                TabIndex="3" Width="250px" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="td_cell">
                            Room No
                        </td>
                        <td>
                            <asp:TextBox ID="txtRoomNo" runat="server" CssClass="txtbox" TabIndex="4" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Adult
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdult" runat="server" CssClass="txtbox TextBoxRightAlign" TabIndex="5"
                                Text="1" Width="120px" />
                        </td>
                        <td class="td_cell">
                            Child
                        </td>
                        <td>
                            <asp:TextBox ID="txtChild" runat="server" CssClass="txtbox TextBoxRightAlign" TabIndex="6"
                                Width="120px" />
                        </td>
                        <td class="td_cell">
                            Attention
                        </td>
                        <td>
                            <asp:TextBox ID="txtAttention" runat="server" CssClass="txtbox" TabIndex="7" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Adult Rate
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdultRate" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="8" Width="120px" />
                        </td>
                        <td class="td_cell">
                            Child Rate
                        </td>
                        <td>
                            <asp:TextBox ID="txtChildRate" runat="server" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="9" Width="120px" />
                        </td>
                        <td class="td_cell">
                        </td>
                        <td>
                            <asp:TextBox ID="txtAirport" Style="display: none" runat="server" CssClass="txtbox"
                                TabIndex="13" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Amount
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" ReadOnly="true" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="10" Width="120px" />
                        </td>
                        <td class="td_cell">
                            Amount(AED)
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmountAED" ReadOnly="true" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="11" Width="120px" />
                        </td>
                        <td class="td_cell">
                        </td>
                       
                    </tr>
                    <tr>
                        
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="td_cell">
                        </td>
                        
                    </tr>
                  
                    <tr>
                      
                       
                        <td class="td_cell">
                            Pickup Time
                        </td>
                        <td>
                            <asp:TextBox ID="txtPickTime" runat="server" CssClass="txtbox" TabIndex="12" Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 160px">
                            Hotel
                        </td>
                        <td colspan="1">
                            <input type="text" class="field_input MyAutoCompleteHotelClass MyAutoCompleteHotelTypeClass"
                                id="txthotel" tabindex="13" runat="server" style="width: 250px;" />
                        </td>
                        <td class="td_cell" style="width: 100px">
                            <asp:Label Style="display: none" ID="lbltransferid" runat="server" Text="Transfer Id."></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox Style="display: none" ID="txttrfno" runat="server" CssClass="txtbox"
                                TabIndex="18" ReadOnly="true" Width="100px" />
                        </td>
                        <td class="td_cell" style="width: 170px">
                            Excursion Time
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtexctime" runat="server" CssClass="txtbox" TabIndex="14" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 160px">
                        </td>
                        <td style="width: 200px" colspan="1">
                            <select style="width: 250px" id="ddlHotel" onchange="filltxthotel()" class="field_input MyDropDownListsuppValue"
                                runat="server" tabindex="15">
                                <option selected="selected"></option>
                            </select>
                        </td>
                        <td class="td_cell">
                            Sup Conf.
                        </td>
                        <td class="td_cell">
                            <asp:CheckBox ID="chksupconf" Text=" " runat="server" onclick="disconfno();" TabIndex="16" />
                        </td>
                        <td class="td_cell">
                            Trf Required.
                        </td>
                        <td class="td_cell">
                            <asp:CheckBox ID="chktrfreq" Text=" " runat="server" TabIndex="17" />
                        </td>
                    </tr>
                    <tr>
                        
                        <td class="td_cell">
                            <asp:Label ID="lblsupconf" runat="server" TabIndex="18" Text="Sup.Conf No." Style="display: none"
                                Width="100px" />
                        </td>
                        <td class="td_cell">
                            <asp:TextBox ID="txtsupconfno" Style="display: none" runat="server" CssClass="txtbox"
                                TabIndex="19" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            Confirmed
                        </td>
                        <td>
                            <select style="width: 120px" id="ddlConfirmed" class="field_input" runat="server"
                                tabindex="20">
                                <option value="1" selected="selected">Yes</option>
                                <option value="0">No</option>
                            </select>
                        </td>
                        <td class="td_cell">
                            Confirmation No
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmationNo" runat="server" MaxLength="30" CssClass="txtbox"
                                TabIndex="21" Width="120px" />
                        </td>
                        <td class="td_cell">
                            Remarks
                        </td>
                        <td rowspan="2">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="txtbox"
                                TabIndex="22" Width="200px" Height="50px" />
                        </td>
                        </tr>

                        <tr>

                        <td class="td_cell">
                            Reminder Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtreminder" runat="server" CssClass="txtbox" TabIndex="23" ValidationGroup="MKE"
                                Width="80px" />
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="54" />
                            <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MEETourDate"
                                ControlToValidate="txtreminder" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                EmptyValueMessage="Date is required" ErrorMessage="" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtreminder" runat="server"
                                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                MaskType="Date" MessageValidatorTip="true">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtreminder" PopupButtonID="ImageButton1"
                                runat="server" Format="dd/MM/yyyy" />
                        </td>


                        <td class="td_cell">
                            Total Amount AED)
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAmountAED" ReadOnly="true" Text="0" runat="server" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="24" Width="200px" />
                        </td>

</tr>











                    
                    <tr>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                    </tr>

                    <tr>
                    <td class="td_cell">
                           <asp:Label ID="grp"  Text="Excursion Group" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <select style="width: 250px" id="ddlExcursionGroup" class="field_input" onchange="CallWebMethod('FilterSubGroup')"
                                runat="server" tabindex="30">
                                <option selected="selected"></option>
                            </select>
                        </td>
                       <%-- Added by Archana on 08/03/2015 as excursion group to be hidden--%>

                       <td class="td_cell">
                          <asp:Label ID="subgrp" Text="Excursion Sub Group" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <select  id="ddlExcursionSubGroup" onchange="CallWebMethod('ExcursionSubGroup')"
                                class="field_input" runat="server" tabindex="31"  style="width: 250px">
                                <option selected="selected"></option>
                            </select>
                        </td>
                               <%-- Added by Archana on 08/03/2015 as excursion sub group to be hidden--%>
                    
                    
                    </tr>


                    <tr>
                    
                     <td class="td_cell">
                        <asp:Label ID="lblcomm" runat="server" Text="S.Person Comm %" Visible="false"></asp:Label>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtSPersonComPer" Text="0" runat="server" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="32" Width="120px" />
                        </td>
                        <%--Changed by Archana on 08/03/2015 --kept comm as visible false as assigned by Shahul sir--%>

                        <td class="td_cell" style="width: 150px">
                         <asp:Label ID="lblpersoncomm" runat="server" Text="S.Person Comm" Visible="false"></asp:Label>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtSPersonCom" runat="server" ReadOnly="true" CssClass="txtbox TextBoxRightAlign"
                                Text="0" TabIndex="33" Width="120px"/>
                        </td>
                    
                    
                    </tr>


                    <tr>
                    
                    <td class="td_cell">
                         <asp:Label ID="ExProv" runat="server" Text="Excursion Provider" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <select style="width: 250px" id="ddlExcursionProvider" onchange="CallWebMethod('ExcursionProvider')"
                                class="field_input" runat="server" tabindex="34" >
                                <option selected="selected"></option>
                            </select>
                        </td><%-- Changed by Archana on 08/03/2015-- Kept excursionProvider visible as False as assigned by Shahul Sir--%>


<td class="td_cell">
                        <asp:Label ID="cocurr" runat="server" Text="Cost Currency" Visible="false"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtCostCurr" runat="server" ReadOnly="true" CssClass="txtbox" TabIndex="35"
                                Width="120px" />
                        </td>
                        <%-- Changed by Archana on 08/03/2015-- Kept Cost Currency visible as False as assigned by Shahul Sir--%>
                    
                    
                    </tr>
                    

                    <tr>
                    
                    <td class="td_cell">
                            
                            <asp:Label ID="cocon" runat="server" Text="Cost Conv Rate" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCostConvRate" runat="server" CssClass="txtbox TextBoxRightAlign"
                                ReadOnly="true" TabIndex="36" Width="120px" />
                        </td>

                        
						<td class="td_cell">
                         <asp:Label ID="adulco" runat="server" Text="Adult Cost Rate" Visible="false"></asp:Label>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdultCostRate" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="37" Width="120px"/>
                        </td>
                        <%-- Changed by Archana on 08/03/2015-- Kept Adult Cost Rate visible as False as assigned by Shahul Sir--%>
                    
                    
                    </tr>


                    <tr>
                     <td class="td_cell">
                         <asp:Label ID="chilco" runat="server" Text="Child Cost Rate" Visible="false"></asp:Label>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtChildCostRate" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="38" Width="120px" />
                        </td>
                        <%-- Changed by Archana on 08/03/2015-- Kept Child Cost Rate visible as False as assigned by Shahul Sir--%>

                          <td class="td_cell">
                         <asp:Label ID="cosamt" runat="server" Text="Cost Amount" Visible="false"></asp:Label>
                           
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtCostAmount" ReadOnly="true" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                TabIndex="39" Width="120px"  /> &nbsp;
                            <asp:Button ID="btnMultipleCost" Text="Multiple Cost" UseSubmitBehavior="false" OnClick="btnMultipleCost_Click"
                                runat="server" CssClass="field_button" Width="120px" />
                        </td>
                         <%-- Changed by Archana on 08/03/2015-- Kept Cost Amount and multiple cost visible as False as assigned by Shahul Sir--%>
                    
                    
                    </tr>


                    <tr>
                     <td class="td_cell">
                         <asp:Label ID="coaed" runat="server" Text="Cost Amt(AED)" Visible="false"></asp:Label>
                           
                        </td>
						
						
						
						 <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCostAmountAED" ReadOnly="true" runat="server" Text="0" CssClass="txtbox TextBoxRightAlign"
                                        TabIndex="40" Width="120px"  />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <%-- Changed by Archana on 08/03/2015-- Kept Cost Amt(AED) visible as False as assigned by Shahul Sir--%>
                    

                    <td class="td_cell" style="width: 160px">
                            
                             <asp:Label ID="prov" runat="server" Text="Provider Ticket No." Visible="false"></asp:Label>
                        </td>
                        <td colspan="1">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtprotktno" runat="server" ReadOnly="true" CssClass="txtbox" TextMode="MultiLine"
                                        TabIndex="41" Width="180px" Height="56px" />&nbsp
                                    <asp:Button ID="btnselect" Text="Select" runat="server" CssClass="field_button" Width="60px" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    
                    
                    </tr>


                     <tr>
                        
                        <td style="display: none" class="td_cell">
                            Flight Type
                        </td>
                        <td>
                            <select style="width: 200px; display: none" id="ddlFlightType" class="field_input"
                                onchange="CallWebMethod('FlightType')" runat="server">
                                <option value="1" selected="selected">Arrival</option>
                                <option value="0">Departure</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        
                        <td style="display: none" class="td_cell">
                            Flight No
                        </td>
                        <td style="display: none">
                            <select style="width: 100px; display: none" id="ddlFlightNo" onchange="CallWebMethod('FlightNo')"
                                class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                            <%--<input type="button" id="btnGetGlight" value="FlightDetails" onclick="ShowFlightDiv()" class="field_button"  />--%>
                            <asp:Button ID="btnGetGlight" Text="Flight Details" runat="server" CssClass="field_button"
                                Width="95px" />&nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="td_cell">
                         <asp:Label ID="lblinv" runat="server" Text="Incomimg Invoice No" Visible="false"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtIncomeInvoiceNo" runat="server" CssClass="txtbox" TabIndex="42"
                                Width="120px" Visible="false" />
                        </td>
                        <%--Changed by Archana on 08/03/2015--Kept Incomimg Invoice No as visible false as assigned by Shahul sir--%>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                          <asp:Label ID="lbldebit" runat="server" Text="Debit Note No" Visible="false"></asp:Label>
                           
                        </td>
                        <td>
                            <asp:TextBox ID="txtDebitNoteNo" runat="server" CssClass="txtbox" TabIndex="43" Width="120px" Visible="false" />
                        </td>
                         <%--Changed by Archana on 08/03/2015--Kept Debit Note No as visible false as assigned by Shahul sir--%>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="td_cell">
                        <asp:Label ID="lbltour" runat="server" Text="Tour Guide" Visible="false"></asp:Label>
                        </td>
                       
                        <td>
                            <select style="width: 250px" id="ddlTourGuide" class="field_input" runat="server"
                                tabindex="44" visible="false">
                                <option selected="selected"></option>
                            </select>
                        </td>
                         <%--Changed by Archana on 08/03/2015--Kept Tour Guide as visible false as assigned by Shahul sir--%>
                        <td class="td_cell">
                        </td>
                        <td>
                        </td>
                        
                    </tr>

                    <tr>
                    
                    <td class="td_cell">
                            <asp:CheckBox ID="chkAmendment" Text="Amendment" runat="server" TabIndex="45" Visible="false" />
                        </td> <%--Changed by Archana on 08/03/2015-- Kept Amendment as visible false as assigned by Shahul sir--%>
                        <td class="td_cell">
                            <asp:CheckBox ID="chkCancel" Text="Cancel" runat="server" TabIndex="46"  Visible="false"/>
                        </td><%--Changed by Archana on 08/03/2015-- Kept Cancel as visible false as assigned by Shahul sir--%>
                    </tr>
                    <tr>
                        
                        <td style="display: none" class="td_cell">
                            Flight Type
                        </td>
                        <td>
                            <select style="width: 200px; display: none" id="Select1" class="field_input"
                                onchange="CallWebMethod('FlightType')" runat="server">
                                <option value="1" selected="selected">Arrival</option>
                                <option value="0">Departure</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        
                        <td style="display: none" class="td_cell">
                            Flight No
                        </td>
                        <td style="display: none">
                            <select style="width: 100px; display: none" id="Select2" onchange="CallWebMethod('FlightNo')"
                                class="field_input" runat="server">
                                <option selected="selected"></option>
                            </select>
                            <%--<input type="button" id="btnGetGlight" value="FlightDetails" onclick="ShowFlightDiv()" class="field_button"  />--%>
                            <asp:Button ID="Button2" Text="Flight Details" runat="server" CssClass="field_button"
                                Width="95px" />&nbsp;
                        </td>
                    </tr>

                    <tr>
                     <td style="display: none">
                            <asp:TextBox ID="txtArrivalDate" runat="server" CssClass="txtbox" TabIndex="47" ValidationGroup="MKE"
                                Width="80px" />
                            <asp:ImageButton ID="ImgBtnArrivalDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="54" />
                            <cc1:MaskedEditValidator ID="MEVArrivalDate" runat="server" ControlExtender="MEEArrivalDate"
                                ControlToValidate="txtArrivalDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                EmptyValueMessage="Date is required" ErrorMessage="" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator>
                            <cc1:MaskedEditExtender ID="MEEArrivalDate" TargetControlID="txtArrivalDate" runat="server"
                                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                                MaskType="Date" MessageValidatorTip="true">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CLFArrivalDate" TargetControlID="txtArrivalDate" PopupButtonID="ImgBtnArrivalDate"
                                runat="server" Format="dd/MM/yyyy" />
                        </td>


                        <td style="display: none">
                            <asp:TextBox ID="txtDepartureDate" runat="server" CssClass="txtbox" TabIndex="48"
                                ValidationGroup="MKE" Width="80px" />
                            <asp:ImageButton ID="ImgBtnDepartureDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="54" />
                            <cc1:MaskedEditValidator ID="MEVDepartureDate" runat="server" ControlExtender="MEEDepartureDate"
                                ControlToValidate="txtDepartureDate" CssClass="field_error" Display="Dynamic"
                                EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage=""
                                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                TooltipMessage="Input a date in dd/mm/yyyy format" ValidationGroup="MKE" Width="23px"></cc1:MaskedEditValidator>
                            <cc1:MaskedEditExtender ID="MEEDepartureDate" TargetControlID="txtDepartureDate"
                                runat="server" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True"
                                Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CLFDepartureDate" TargetControlID="txtDepartureDate" PopupButtonID="ImgBtnDepartureDate"
                                runat="server" Format="dd/MM/yyyy" />
                        </td>
                    
                    </tr>

                    <tr>
                        <td class="td_cell">
                            <asp:Label Style="display: none" ID="lblsupplier" runat="server" Text="Supplier"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsupplier" Style="display: none" ReadOnly="true" CssClass="txtbox"
                                runat="server" Width="249px" />
                        </td>
                        <td class="td_cell">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_cell">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell">
                            <asp:Label Style="display: none" ID="lbldriver" runat="server" Text="Driver"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdriver" ReadOnly="true" Style="display: none" CssClass="txtbox"
                                runat="server" Width="249px" />
                        </td>
                        <td class="td_cell">
                            <asp:Label ID="lblmob" Style="display: none" runat="server" Text="Mob."></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmob" ReadOnly="true" Style="display: none" CssClass="txtbox"
                                runat="server" Width="120px" />
                        </td>
                        <td class="td_cell">
                        </td>
                        <td>
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="field_button" Width="60px" />&nbsp;
                            <asp:Button ID="btnExit" Text="Exit" runat="server" CssClass="field_button" Width="60px" />
                            &nbsp;
                            <asp:Button ID="Button1" Text="checkPostback" Visible="false" runat="server" CssClass="field_button"
                                Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <!--not required to display-->
                        <td class="td_cell" style="width: 170px; display: none">
                            Flight Time
                        </td>
                        <td style="width: 100px; display: none">
                            <asp:TextBox ID="txtFlightTime" runat="server" CssClass="txtbox" TabIndex="17" Width="100px" />
                        </td>
                        
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(function () {

                });

                prm.add_endRequest(function () {
                    MyAutohotel_rptFillArray();

                });




            </script>
            <div id="ShowFlightHelp" runat="server" style="display: none; left: 400px; top: 100px;
                position: absolute; z-index: 100; height: 440px; width: 511px; border: 3px solid green">
                <div id="ShowFlightHelp2" runat="server" style="overflow: auto; max-height: 400px;
                    max-width: 510px; min-height: 395px">
                    <asp:GridView ID="gv_Flight" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" CssClass="td_cell">
                        <Columns>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblFlightNo" runat="server" Text='<%# Bind("flightcode") %>'></asp:Label>
                                    <asp:Label ID="lblFlightTranID" runat="server" Text='<%# Bind("flight_tranid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField HeaderText="Action" Text="Select" CommandName="SelectRow">
                                <ItemStyle HorizontalAlign="Left" ForeColor="Blue" VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="city" SortExpression="city" HeaderText="City">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="arrivetime1" SortExpression="arrivetime1" HeaderText="Time">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="airport" SortExpression="airport" HeaderText="Origin-Destination">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="200px"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="flightcode" SortExpression="flightcode" HeaderText="Flight Code">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                            </asp:BoundField>
                        </Columns>
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                        <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                        <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                        <AlternatingRowStyle BackColor="Transparent" Font-Size="10px"></AlternatingRowStyle>
                    </asp:GridView>
                </div>
                <table style="float: left">
                    <tr>
                        <td colspan="4" align="center" valign="middle">
                            <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False"
                                CssClass="lblmsg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <select style="width: 140px" id="ddlSearchType" class="field_input" runat="server">
                                <option value="1">City</option>
                                <option value="0">Origin-Destination</option>
                                <option value="2">Time</option>
                                <option value="3" selected="selected">Flight Code</option>
                            </select>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="txtbox" Width="225px" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="field_button" Width="60px" />&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="field_button" Width="60px" />&nbsp;
                        </td>
                    </tr>
                </table>
                <%--<input type="button" id="btnClose" value="Close" onclick="HideFlightDiv()" class="field_button"  />--%>
            </div>
            <div id="showtkts" runat="server" style="display: none; left: 400px; top: 100px;
                position: absolute; z-index: 100; height: 440px; width: 220px; border: 3px solid green">
                <div id="showtkts1" runat="server" style="overflow: auto; max-height: 390px; max-width: 220px;
                    min-height: 380px">
                    <asp:GridView ID="gvticketdet" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" CssClass="td_cell" TabIndex="37">
                        <FooterStyle CssClass="grdfooter" />
                        <Columns>
                            <asp:TemplateField HeaderText="ticketno" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblticket" runat="server" Text='<%# Bind("ticketno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <input id="chkselect" type="checkbox" checked='<%# Bind("status") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket.">
                                <ItemTemplate>
                                    <asp:Label ID="lblticketno" Width="155px" Text='<%# Bind("ticketno") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="grdRowstyle" />
                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                        <HeaderStyle CssClass="grdheader" />
                    </asp:GridView>
                </div>
                <table style="float: left">
                    <tr>
                        <td colspan="4" align="center" valign="middle">
                            <asp:Label ID="lblmsg1" runat="server" Text="Records not found. Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="220px" Visible="False"
                                CssClass="lblmsg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtSearch1" runat="server" CssClass="txtbox" Width="100px" />
                        </td>
                        <td>
                            <asp:Button ID="btnsearch1" Text="Search" runat="server" CssClass="field_button"
                                Width="50px" />&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnclose1" Text="Close" runat="server" CssClass="field_button" Width="50px" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnselect1" runat="server" CssClass="field_button" Text="Select"
                                Width="50px" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <cc1:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
                CancelControlID="btnClose" OkControlID="btnOkay" TargetControlID="btnInvisibleGuest"
                PopupControlID="ShowFlightHelp" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="Modalpopuptkts" runat="server" BehaviorID="Modalpopuptkts"
                CancelControlID="btnClose1" OkControlID="btnOkay" TargetControlID="btnInvisibleTick"
                PopupControlID="showtkts" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </cc1:ModalPopupExtender>
            <input id="dummy" type="button" style="display: none" runat="server" />
            <asp:ModalPopupExtender runat="server" ID="ModalPopupMultipleCost" TargetControlID="dummy"
                PopupControlID="PanelMultipleCost" BackgroundCssClass="ModalPopupBG" DropShadow="true"
                CancelControlID="btnSubEntryClose" />
            <asp:Panel ID="PanelMultipleCost" Style="display: none" runat="server" BorderStyle="Double"
                BorderWidth="6px">
                <div class="HellowWorldPopup" style="font-family: Arial, Helvetica, sans-serif">
                    <div class="PopupHeader" id="Div2">
                        <center style="background-color: #CCCCCC;">
                            MultipleCost</center>
                    </div>
                    <div class="PopupBody">
                        <center>
                            <center>
                                <wuc:wucExcursionRequestSubEntry ID="wucExcSumSubEntry" runat="server" />
                            </center>
                            <br />
                            <asp:Button ID="btnMCAddRow" runat="server" CssClass="field_button" Text="Add Row" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnMCDelRow" runat="server" CssClass="field_button" Text="Delete Row" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnMCSave" runat="server" CssClass="field_button" UseSubmitBehavior="true"
                                OnClick="btnMCSave_Click" Text="Save" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSubEntryClose" runat="server" CssClass="field_button" Text="Close"
                                UseSubmitBehavior="false" />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </div>
            </asp:Panel>
            <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
            <input id="btnInvisibleTick" runat="server" type="button" value="Cancel" style="visibility: hidden" />
            <input id="btnOkay" type="button" value="OK" style="visibility: hidden" />
            <input id="btnCancel1" type="button" value="Cancel" style="visibility: hidden" />
            <asp:HiddenField ID="hdntktdet" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnIsMultipleCost" runat="server" />
    <asp:HiddenField ID="txtconnection" runat="server" />
    <asp:HiddenField ID="hdnExcursionID" runat="server" />
    <asp:HiddenField ID="hdnExcursionSubGroupCode" runat="server" />
    <asp:HiddenField ID="hdnExcursionTypeCode" runat="server" />
    <asp:HiddenField ID="hdnAdultRate" runat="server" />
    <asp:HiddenField ID="hdnChildRate" runat="server" />
    <asp:HiddenField ID="hdnSPersonComPer" runat="server" />
    <asp:HiddenField ID="hdnExcursionProvider" runat="server" />
    <asp:HiddenField ID="hdnFlightType" Value="1" runat="server" />
    <asp:HiddenField ID="hdnFlightNo" runat="server" />
    <asp:HiddenField ID="hdnExcursionGroupCode" runat="server" />
    <asp:HiddenField ID="hdnSellingTypeForWS" runat="server" />
    <asp:HiddenField ID="hdnSpersonCodeForWS" runat="server" />
    <asp:HiddenField ID="hdnPartyCodeForWS" runat="server" />
    <asp:HiddenField ID="hdnCostCurr" runat="server" />
    <asp:HiddenField ID="hdnCostCurrConvRate" runat="server" />
    <asp:HiddenField ID="hdnAmount" runat="server" />
    <asp:HiddenField ID="hdnAmountAED" runat="server" />
    <asp:HiddenField ID="hdnCostAmount" runat="server" />
    <asp:HiddenField ID="hdnCostAmountAED" runat="server" />
    <asp:HiddenField ID="txtdecimal" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>

