<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="ReceiptsNewChange.aspx.vb" Inherits="ReceiptsNewChange" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);  
    </script>
    <script src="../Content/js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../Content/js/JqueryUI.js" type="text/javascript"></script>
    <script src="../Content/js/accounts.js" type="text/javascript"></script>
    <link type="text/css" href="../Content/css/JqueryUI.css" rel="Stylesheet" />
    <style type="text/css">
             .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .hiddencol
        {
            display: none;
        }
        .style1
        {
            height: 56px;
        }
        .filed_caption
        {
            margin-left: 0px;
        }
    </style>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {


            CustBankNameAutoCompleteExtenderKeyUp();
            GAccAutoCompleteExtenderKeyUp();
            SrcCtryAutoCompleteExtenderKeyUp();
            AutoCompleteExtender_BankNameKeyUp();

        });

    </script>
    <script type="text/javascript" charset="utf-8">

        function ShowProgress() {
            var ModalPopupLoading = $find("ModalPopupLoading");

                ModalPopupLoading.show();
                return true;
           
        }
        function HideProgress() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.hide();
            return true;

        }
        function GAccAutoCompleteExtenderKeyUp() {


            $("#<%=grdReceipt.ClientID%> tr input[id*='txtGAccName']").each(function () {

                $(this).change(function (event) {
                    var hiddenfieldID1 = $(this).attr("id").replace("txtGAccName", "txtGAccName");
                    var hiddenfieldID = $(this).attr("id").replace("txtGAccName", "txtGAccCode");


                    if ($get(hiddenfieldID1).value == '') {

                        $get(hiddenfieldID).value = '';
                    }

                });

                $(this).keyup(function (event) {

                    var hiddenfieldID = $(this).attr("id").replace("txtGAccName", "ddlType");
                    var hiddenfieldID1 = $(this).attr("id").replace("txtGAccName", "txtConAccName_AutoCompleteExtender");
                    var hiddenfieldID2 = $(this).attr("id").replace("txtGAccName", "txtGAccName");
                    var hiddenfieldID3 = $(this).attr("id").replace("txtGAccName", "txtGAccCode");
                    var hiddenfieldID4 = $(this).attr("id").replace("txtGAccName", "txtConAccCode");
                    var hiddenfieldID5 = $(this).attr("id").replace("txtGAccName", "txtConAccName");
                    var hiddenfieldID6 = $(this).attr("id").replace("txtGAccName", "txtgnarration");
                    var hiddenfieldID7 = $(this).attr("id").replace("txtGAccName", "ddlCostCode");
                    var hiddenfieldID8 = $(this).attr("id").replace("txtGAccName", "ddlCostName");

                    var hiddenfieldID9 = $(this).attr("id").replace("txtGAccName", "accSearch");
                    var hiddenfieldID10 = $(this).attr("id").replace("txtGAccName", "txtCurrency");
                    var hiddenfieldID11 = $(this).attr("id").replace("txtGAccName", "txtConvRate");

                    var hiddenfieldID12 = $(this).attr("id").replace("txtGAccName", "txtDebit");
                    var hiddenfieldID13 = $(this).attr("id").replace("txtGAccName", "txtBaseDebit");
                    var hiddenfieldID14 = $(this).attr("id").replace("txtGAccName", "txtCredit");
                    var hiddenfieldID15 = $(this).attr("id").replace("txtGAccName", "txtBaseCredit");
                    var hiddenfieldID16 = $(this).attr("id").replace("txtGAccName", "txtSrcCtryCode");
                    var hiddenfieldID17 = $(this).attr("id").replace("txtGAccName", "txtSrcCtryName");

                    fillinkeyup(hiddenfieldID, hiddenfieldID1)




                    //                     fill_acountcode(hiddenfieldID, hiddenfieldID1, hiddenfieldID2, hiddenfieldID3, hiddenfieldID4, hiddenfieldID5, hiddenfieldID6, hiddenfieldID7, hiddenfieldID8, hiddenfieldID9, hiddenfieldID10, hiddenfieldID11, hiddenfieldID12, hiddenfieldID13, hiddenfieldID14, hiddenfieldID15, hiddenfieldID16, hiddenfieldID17, hiddenfieldID17)

                    if ($get(hiddenfieldID2).value == '') {

                        $get(hiddenfieldID3).value = '';
                    }

                });
            });



        }

        function SrcCtryAutoCompleteExtenderKeyUp() {


            $("#<%=grdReceipt.ClientID%> tr input[id*='txtSrcCtryName']").each(function () {

                $(this).change(function (event) {
                    var hiddenfieldID1 = $(this).attr("id").replace("txtSrcCtryName", "txtSrcCtryName");
                    var hiddenfieldID = $(this).attr("id").replace("txtSrcCtryName", "txtSrcCtryCode");


                    if ($get(hiddenfieldID1).value == '') {

                        $get(hiddenfieldID).value = '';
                    }

                });

                $(this).keyup(function (event) {

                    var hiddenfieldID1 = $(this).attr("id").replace("txtSrcCtryName", "txtSrcCtryName");
                    var hiddenfieldID = $(this).attr("id").replace("txtSrcCtryName", "txtSrcCtryCode");


                    if ($get(hiddenfieldID1).value == '') {

                        $get(hiddenfieldID).value = '';
                    }

                });
            });



        }
        function SrcCtryautocompleteselectedControl(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("txtSrcCtryName_AutoCompleteExtender", "txtSrcCtryCode");


            $get(hiddenfieldID).value = eventArgs.get_value();
            //              alert(eventArgs.get_value(1));


        }

        function GAccautocompleteselectedControl(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtGAccCode");
            var hiddenfieldID1 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtGAccName");

            var hiddenfieldID2 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtCurrency");
            var hiddenfieldID3 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtConvRate");
            var hiddenfieldID4 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtConAccCode");
            var hiddenfieldID5 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtConAccName");
            var hiddenfieldID6 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "ddlType");
            var hiddenfieldID7 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtacctcode");
            var hiddenfieldID8 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtacctname");
            var hiddenfieldID9 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtCredit");
            var hiddenfieldID10 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtBaseCredit");
            var hiddenfieldID11 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtDebit");
            var hiddenfieldID12 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtBaseDebit");
            var hiddenfieldID13 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtctrolaccode");
            var hiddenfieldID14 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtcontrolacname");
            var hiddenfieldID15 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "accSearch");
            var hiddenfieldID16 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtSrcCtryName_AutoCompleteExtender");
            var hiddenfieldID17 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtSrcCtryCode");
            var hiddenfieldID18 = source.get_id().replace("txtConAccName_AutoCompleteExtender", "txtSrcCtryName");

            $get(hiddenfieldID).value = eventArgs.get_value();

            FillGAName(hiddenfieldID, hiddenfieldID1, hiddenfieldID2, hiddenfieldID3, hiddenfieldID4, hiddenfieldID5, hiddenfieldID6, hiddenfieldID7, hiddenfieldID8, hiddenfieldID9, hiddenfieldID10, hiddenfieldID11, hiddenfieldID12, hiddenfieldID13, hiddenfieldID14, hiddenfieldID15, hiddenfieldID16, hiddenfieldID17, hiddenfieldID18);

            if (document.getElementById(hiddenfieldID6).value != 'G') {

                GetCountryDetails(eventArgs.get_value(), hiddenfieldID6, hiddenfieldID16, hiddenfieldID17, hiddenfieldID18);
            }


        }

        //            function BankNameautocompleteselected(source, eventArgs) {
        //                if (eventArgs != null) {
        //                    document.getElementById('<%=TxtBankcode.ClientID%>').value = eventArgs.get_value();
        //                    alert(document.getElementById('<%=TxtBankcode.ClientID%>').value);
        //                    FillCurrConv('<%=TxtBankcode.ClientID%>', '<%=TxtBankName.ClientID%>')
        //                }
        //                else {
        //                    document.getElementById('<%=TxtBankcode.ClientID%>').value = '';
        //                }

        //            }

        function AutoCompleteExtender_BankNameKeyUp() {
            $("#<%= txtBankName.ClientID %>").bind("change", function () {

                var hiddenfieldID1 = document.getElementById('<%=txtBankName.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtBankCode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
                SetBankContextkey();
            });
            $("#<%= txtBankName.ClientID %>").keyup("change", function () {

                var hiddenfieldID1 = document.getElementById('<%=txtBankName.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtBankCode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
                SetBankContextkey();
            });

        }

        function CustBankNameAutoCompleteExtenderKeyUp() {
            $("#<%= txtCustBankName.ClientID %>").bind("change", function () {
                var hiddenfieldID1 = document.getElementById('<%=txtCustBankName.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtCustBankCode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });

            $("#<%= txtCustBankName.ClientID %>").keyup("change", function () {
                var hiddenfieldID1 = document.getElementById('<%=txtCustBankName.ClientID%>');
                var hiddenfieldID = document.getElementById('<%=txtCustBankCode.ClientID%>');
                if (hiddenfieldID1.value == '') {
                    hiddenfieldID.value = '';
                }
            });
        }

        function BankNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {

                document.getElementById('<%=TxtBankcode.ClientID%>').value = eventArgs.get_value();
                FillCurrConvheader('<%=TxtBankcode.ClientID%>', '<%=TxtBankName.ClientID%>')
            }
            else {
                document.getElementById('<%=TxtBankcode.ClientID%>').value = '';
            }
            SetBankContextkey();
        }
        //         var divid = hdn = document.getElementById("<%= txtDivCode.ClientID%>") ;
        //          alert(divid.value);

        function GAccautocompleteselected(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("TxtGAccname_AutoCompleteExtender", "TxtGAcccode");


            $get(hiddenfieldID).value = eventArgs.get_value();




        }

        //          function BankNameautocompleteselected(source, eventArgs) {
        //              if (eventArgs != null) {
        //                  document.getElementById('<%=TxtBankcode.ClientID%>').value = eventArgs.get_value();
        //              }
        //              else {
        //                  document.getElementById('<%=TxtBankcode.ClientID%>').value = '';
        //              }

        //          }


        function CustBankNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {

                document.getElementById('<%=txtCustBankCode.ClientID%>').value = eventArgs.get_value();


            }
            else {
                document.getElementById('<%=txtCustBankCode.ClientID%>').value = '';

            }
        }
          
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            CustBankNameAutoCompleteExtenderKeyUp();
            GAccAutoCompleteExtenderKeyUp();
            AutoCompleteExtender_BankNameKeyUp();

            SetBankContextkey();

            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">

        function ConfirmContinue(msg, btnId) {
            var btn = document.getElementById(btnId);
            var hdn = document.getElementById("<%= hdnValidate.ClientID%>");
            if (confirm(msg) == true) {
                btn.click();
            }
            else {
                hdn.click();
            }
        }


        function GetCountryDetails(CustCode, Acctype, SrcCtryautoComp, srcctrycode, srcname) {

            Acctype = document.getElementById(Acctype).value;


            $.ajax({
                type: "POST",
                url: "ReceiptsNew.aspx/GetCountryDetails",
                data: '{CustCode:  "' + CustCode + '",Acctype: "' + Acctype + '",SrcCtryautoComp:" ' + SrcCtryautoComp + '",srcctrycode :"' + srcctrycode + '",srcname :"' + srcname + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var xmlDoc = $.parseXML(response.d);
                    var xml = $(xmlDoc);

                    var Countries = xml.find("Countries");

                    var rowCount = Countries.length;


                    if (rowCount == 1) {

                        $.each(Countries, function () {
                            document.getElementById(srcname).value = ''
                            document.getElementById(srcctrycode).value = '';
                            document.getElementById(srcname).value = $(this).find("ctryname").text();
                            document.getElementById(srcctrycode).value = $(this).find("ctrycode").text();
                            document.getElementById(srcname).setAttribute("readonly", true);

                            //                            $find(SrcCtryautoComp).setAttribute("Enabled", false);

                        });
                    }
                    else {

                        //                        $find(SrcCtryautoComp).setAttribute("Enabled", true);
                        //                        $find('txtSrcCtryName_AutoCompleteExtender').setAttribute("Enabled", true);

                        document.getElementById(srcname).value = ''
                        document.getElementById(srcctrycode).value = '';
                        document.getElementById(srcname).removeAttribute("disabled");
                        document.getElementById(srcname).removeAttribute("readonly");
                    }

                },

                failure: function (response) {
                    alert('failure');
                    alert(response.d);
                },
                error: function (response) {
                    alert('error');
                    alert(response.d);
                }
            });
        }

        function OnSuccess(response) {
            alert(response);
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var Countries = xml.find("Countries");
            var rowCount = Countries.length;
            alert(rowCount);
            document.getElementById(srccode).value = 'fsdf';
            if (rowCount == 1) {

                $.each(Countries, function () {


                });
            }
            else {

                $find('txtSrcCtryName_AutoCompleteExtender').setAttribute("Enabled", true);
            }
        };


        function OpenModalDialog(url, diaHeight) {

            var vReturnValue;
            if (diaHeight == null || diaHeight == "")
                diaHeight = "300";
            if (url != null) {
                vReturnValue = window.showModalDialog(url, "#1", "dialogHeight: " + diaHeight + "px; dialogWidth: 650px; dialogTop: 190px; dialogLeft: 120px;dialogRight:220px; edge: Raised; center: Yes; help: No; resizable: No; status: No;");
            }
            else {
                alert("No URL passed to open");
            }
            if (vReturnValue != null && vReturnValue == true) {

                return vReturnValue
            }
            else {   //alert(vReturnValue);
                //alert(vReturnValue);
                return false;
            }
        }

        //----------------------------------------
        var nodecround = null;
        var txtgrdcrate = null;
        var txtfill = null;
        var txtcrate = null;
        var ddlACode = null;
        var ddlAName = null;
        var ddlAConAcc = null;
        var ddlAConAccNm = null;
        var txtAConAcc = null;
        var txtAConAccNm = null;
        var sqlstr = null;
        var txtgrdDBAmt = null;
        var txtgrdCnvtAmt = null;
        var sddltyp = null;
        var sddlACode = null;

        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "");
        }
        function DecRound(amtToRound) {

            var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
            nodecround = Math.pow(10, parseInt(txtdec.value));
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
        }

        function chkTextLock(evt) {
            return false;
        }
        function chkTextLock1(evt) {
            if (evt.keyCode = 9) {
                return true;
            }
            else {
                return false;
            }
            return false;

        }

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

        function checkNumber1(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if ((charCode > 47 && charCode < 58)) {
                //if (charCode != 47 && (charCode > 44 && charCode < 58)) {    
                return true;
            }
            return false;
        }

        function checkNumber2(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if ((charCode > 46 && charCode < 58)) {

                return true;
            }
            return false;
        }
        function checkNumberDecimal(evt, txt) {

            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                var value = txt.value;
                var indx = value.indexOf('.');
                var deci = document.getElementById("<%=txtdecimal.ClientID%>");
                var lngLenght = deci.value;
                if (indx < 0) {
                    return true;
                }

                var digit = value.substring(indx + 1);
                if (digit.length > lngLenght - 1) {
                    return false;
                }
                else {
                    return true;
                }
            }
            return false;
        }



        function DecFormat(value) {

            var rdamt = null;
            var indx = value.indexOf('.');
            var deci = document.getElementById("<%=txtdecimal.ClientID%>");
            var lngLenght = deci.value;
            if (indx < 0) {
                rdamt = value + ".000";
                return rdamt;
            }
            var digit = value.substring(indx + 1);
            if (digit.length > lngLenght - 1) {
                rdamt = value;
                return rdamt;
            }
            else {
                var nozeros = parseInt(lngLenght) - parseInt(digit.length);
                if (nozeros == 1)
                { rdamt = value + "0"; }
                else if (nozeros == 2)
                { rdamt = value + "00"; }
                else if (nozeros == 3)
                { rdamt = value + "000"; }
                else
                { return value; }
                return rdamt;
            }
            return rdamt;
        }


        function fillacountcode(hiddenfieldIDname, hiddenfieldIDcode) {

            alert(hiddenfieldIDname);
            alert(hiddenfieldIDcode);
        }
        function FillCodeName(ddlcode, ddlname) {
            ddlc = document.getElementById(ddlcode);
            ddln = document.getElementById(ddlname);
            ddln.value = ddlc.options[ddlc.selectedIndex].text;
        }

        function filldept(ddldept) {
            ddldept = document.getElementById(ddldept);

            ddldept.value = ddldept.value
        }

        function OnchangeCashBank(ddlC) {

            ddlC = document.getElementById(ddlC);
            FillBankCashDet();

            if (ddlC.value == 'Cash') {
                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
                //   ddl.style.visibility = "hidden";
                ddl.style.visibility = "visible";
                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
                //   txtchq.style.visibility = "hidden";
                txtchq.style.visibility = "visible";
                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
                //   txtchqdt.style.visibility = "hidden";
                txtchqdt.style.visibility = "visible";

                var img = document.getElementById("<%=ImageButton1.ClientId%>");
                //    img.style.visibility = "hidden";
                img.style.visibility = "Visible";

                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
                //   lbl1.style.visibility = "hidden";
                lbl1.style.visibility = "Visible";

                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
                //   lbl2.style.visibility = "hidden";
                lbl2.style.visibility = "Visible";
                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");
                //   lbl3.style.visibility = "hidden";
                lbl3.style.visibility = "Visible";




            }
            else if (ddlC.value == 'Bank') {

                var txtTranTypes = document.getElementById("<%=txtTranType.ClientId%>");
                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
                txtchq.style.visibility = "visible";
                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
                txtchqdt.style.visibility = "visible";
                var img = document.getElementById("<%=ImageButton1.ClientId%>");
                img.style.visibility = "visible";
                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
                lbl1.style.visibility = "visible";
                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
                lbl2.style.visibility = "visible";
                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");



                if (txtTranTypes.value.trim() == 'RV' || txtTranTypes.value.trim() == 'CV') {
                    ddl.style.visibility = "visible";
                    lbl3.style.visibility = "visible";
                }
                else //if (trim(txtTranTypes.value)=='PV' )
                {
                    ddl.style.visibility = "hidden";
                    lbl3.style.visibility = "hidden";
                }
            }
        }
        function OnchangeCashBank(ddlC) {
            ddlC = document.getElementById(ddlC);

            if (ddlC.value == 'Cash') {

                document.getElementById("<%=lblpostdatebank.ClientId%>").style.display = 'none';
                document.getElementById("<%=lbldate.ClientId%>").style.display = 'block';
               
            }            
            else if (ddlC.value == 'Bank') {
                document.getElementById("<%=lblpostdatebank.ClientId%>").style.display = 'block';
                document.getElementById("<%=lbldate.ClientId%>").style.display = 'none';

            }


            //            if (ddlC.value == 'Cash') {
            //                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
            //                //   ddl.style.visibility = "hidden";
            //                ddl.style.visibility = "visible";
            //                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
            //                //   txtchq.style.visibility = "hidden";
            //                txtchq.style.visibility = "visible";
            //                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
            //                //   txtchqdt.style.visibility = "hidden";
            //                txtchqdt.style.visibility = "visible";

            //                var img = document.getElementById("<%=ImageButton1.ClientId%>");
            //                //    img.style.visibility = "hidden";
            //                img.style.visibility = "Visible";

            //                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
            //                //   lbl1.style.visibility = "hidden";
            //                lbl1.style.visibility = "Visible";

            //                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
            //                //   lbl2.style.visibility = "hidden";
            //                lbl2.style.visibility = "Visible";
            //                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");
            //                //   lbl3.style.visibility = "hidden";
            //                lbl3.style.visibility = "Visible";
            //            }
            //            else if (ddlC.value == 'Bank') {

            //                var txtTranTypes = document.getElementById("<%=txtTranType.ClientId%>");
            //                var txtchq = document.getElementById("<%=txtChequeNo.ClientId%>");
            //                txtchq.style.visibility = "visible";
            //                var txtchqdt = document.getElementById("<%=txtChequeDate.ClientId%>");
            //                txtchqdt.style.visibility = "visible";
            //                var img = document.getElementById("<%=ImageButton1.ClientId%>");
            //                img.style.visibility = "visible";
            //                var lbl1 = document.getElementById("<%=lblChN.ClientId%>");
            //                lbl1.style.visibility = "visible";
            //                var lbl2 = document.getElementById("<%=lblChD.ClientId%>");
            //                lbl2.style.visibility = "visible";
            //                var ddl = document.getElementById("<%=ddlCustBank.ClientId%>");
            //                var lbl3 = document.getElementById("<%=lblChB.ClientId%>");
            //                if (txtTranTypes.value.trim() == 'RV') {
            //                    ddl.style.visibility = "visible";
            //                    lbl3.style.visibility = "visible";
            //                }
            //                else //if (trim(txtTranTypes.value)=='PV' )
            //                {
            //                    ddl.style.visibility = "hidden";
            //                    lbl3.style.visibility = "hidden";
            //                }
            //            }
            FillBankCashDet();
        }

        function FillBankCashDet() {
            SetBankContextkey();

        }


        function SetBankContextkey() {

            var contxt = '';

            var strSqlQry1;
            var strSqlQry2;
            var strSqlQry3;
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>").value;

            var ddlcashbank = document.getElementById('<%=ddlCashBank.ClientID%>');


            contxt = ddlcashbank.value + '||' + txtdiv;

            // $find('AutoCompleteExtender_txtBankName').set_contextKey(contxt);
            $find('<%=AutoCompleteExtender_txtBankName.ClientID%>').set_contextKey(contxt);

        }
        function FillBankCodes(result) {

            ddlbankcd = document.getElementById('<%=ddlAccCode.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }
        function FillBankNames(result) {
            ddlbankcd = document.getElementById('<%=ddlAccName.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }
        function FillCurrCodes(result) {
            ddlbankcd = document.getElementById('<%=ddlCurrCode.ClientID%>');
            RemoveAll(ddlbankcd)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlbankcd.options.add(option);
            }
            ddlbankcd.value = "[Select]";
        }

        function FillCurrCode(result) {
            var ddlbankcd = document.getElementById('<%=txtCurrency.ClientID%>');
            ddlbankcd.value = result;

        }



        //        function FillCode(ddlIccd, ddlIcnm) {

        //            ddlIccode = document.getElementById(ddlIccd);
        //            ddlIcname = document.getElementById(ddlIcnm);

        //            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
        //            ddlIcname.value = codeid;

        //            var txtacccode = document.getElementById('<%=txtAccCode.ClientID%>');
        //            var txtaccname = document.getElementById('<%=txtAccName.ClientID%>');
        //            var txtcurrcode = document.getElementById('<%=txtCurrCode.ClientID%>');
        //            txtacccode.value = ddlIccode.options[ddlIccode.selectedIndex].value;
        //            txtaccname.value = ddlIccode.options[ddlIccode.selectedIndex].text;
        //            txtcurrcode.value = ddlIccode.options[ddlIccode.selectedIndex].text;

        //            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        //            constr = connstr.value
        //            alert(txtcurrcode.value);

        //            var ddlcur = document.getElementById('<%=ddlCurrCode.ClientID%>');
        ////            ddlcur.value = codeid;
        //            var currsqlstr
        //            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');
        //            txtdivcode = document.getElementById("<%=txtDivCode.ClientId%>");

        //            //sqlstr="select convrate,convrate from currrates where currcode='"+ ddlcur.options[ddlIcname.selectedIndex].text +"' and Tocurr='"+ txtbase.value +"'"
        //            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdivcode.value + "' and  currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid + "' "
        //            ColServices.clsServices.GetQueryReturnStringValuenew(constr, currsqlstr, FillCvntRate, ErrorHandler, TimeOutHandler);
        //            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdivcode.value + "' and  currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid + "' "
        //            ColServices.clsServices.GetQueryReturnStringListnew(constr, strSqlQry3, FillCurrCodes, ErrorHandler, TimeOutHandler);



        //            sqlstr = "sp_get_account_balance  '" + txtdivcode.value + "','G','" + codeid + "'";

        //            ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillBalance, ErrorHandler, TimeOutHandler);

        //            txtconvrate = document.getElementById("<%=txtConvRate.ClientId%>");
        //            if (trim(ddlcur.options[ddlcur.selectedIndex].text) == trim(txtbase.value)) {
        //                txtconvrate.readOnly = true;
        //                txtconvrate.disabled = true;
        //            }
        //            else {
        //                txtconvrate.readOnly = false;
        //                txtconvrate.disabled = false;
        //            }


        //        }

        //        //----------------------------------------
        //        function FillName(ddlIccd, ddlIcnm) {
        //            ddlIccode = document.getElementById(ddlIccd);
        //            ddlIcname = document.getElementById(ddlIcnm);

        //            var codeid = ddlIcname.options[ddlIcname.selectedIndex].value;
        //            ddlIccode.value = ddlIcname.options[ddlIcname.selectedIndex].text;

        //            var txtacccode = document.getElementById('<%=txtAccCode.ClientID%>');
        //            var txtaccname = document.getElementById('<%=txtAccName.ClientID%>');
        //            var txtcurrcode = document.getElementById('<%=txtCurrCode.ClientID%>');
        //            txtacccode.value = ddlIcname.options[ddlIcname.selectedIndex].text;
        //            txtaccname.value = ddlIcname.options[ddlIcname.selectedIndex].value;
        //            txtcurrcode.value = ddlIcname.options[ddlIcname.selectedIndex].value;



        //            var ddlcur = document.getElementById('<%=ddlCurrCode.ClientID%>');
        //            ddlcur.value = ddlIcname.value;

        //            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        //            constr = connstr.value

        //            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");
        //            var currsqlstr
        //            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');
        //            //sqlstr="select convrate,convrate from currrates where currcode='"+ ddlcur.options[ddlIcname.selectedIndex].text +"' and Tocurr='"+ txtbase.value +"'"
        //            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdiv.value + "' and currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid + "' "
        //            ColServices.clsServices.GetQueryReturnStringValuenew(constr, currsqlstr, FillCvntRate, ErrorHandler, TimeOutHandler);
        //            txtconvrate = document.getElementById("<%=txtConvRate.ClientId%>");
        //            if (trim(ddlcur.options[ddlcur.selectedIndex].text) == trim(txtbase.value)) {
        //                txtconvrate.readOnly = true;
        //                txtconvrate.disabled = true;
        //            }
        //            else {
        //                txtconvrate.readOnly = false;
        //                txtconvrate.disabled = false;
        //            }
        //        }
        //        //----------------------------------------

        function FillCurrConvheader(TxtBankCode, TxtBankName) {
            
            var codeid = document.getElementById('<%=TxtBankCode.ClientID%>').value;

            var txtaccname = document.getElementById('<%=TxtBankName.ClientID%>').value;
            document.getElementById('<%=hdnConvRate.ClientID%>').value = ""

            var txtcurrcode = document.getElementById('<%=txtCurrency.ClientID%>');

            var txtacccode = document.getElementById('<%=txtAccCode.ClientID%>');
            var txtaccnameh = document.getElementById('<%=txtAccName.ClientID%>');
            var txtcurrcodeh = document.getElementById('<%=txtCurrCode.ClientID%>');
            txtacccode.value = codeid;

            txtaccnameh.value = txtaccname;
            txtcurrcodeh.value = txtaccname;
            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");
            var currsqlstr
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');

            currsqlstr = "select convrate  from currrates,acctmast  where acctmast.div_code='" + txtdiv.value + "' and currrates.currcode=acctmast.currcode and Tocurr='" + txtbase.value + "' and acctmast.acctcode='" + codeid.toString() + "' "


            ColServices.clsServices.GetQueryReturnStringValuenew(constr, currsqlstr, FillCvntRate, ErrorHandler, TimeOutHandler);
            var strSqlQry;
            var ddlcashbank = document.getElementById('<%=ddlCashBank.ClientID%>');
            if (ddlcashbank.value == 'Cash') {
                strSqlQry = "select  Currcode   from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and bank_master_type.cashbanktype='C' and acctmast.acctcode='" + codeid + "'  order by acctcode";
            }
            else if (ddlcashbank.value == 'Bank') {
                strSqlQry = "select  Currcode  from acctmast ,bank_master_type where acctmast.div_code='" + txtdiv.value + "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code and  bankyn='Y' and bank_master_type.cashbanktype='B'  and acctmast.acctcode='" + codeid + "' order by acctcode";
            }
            ColServices.clsServices.GetQueryReturnStringValuenew(constr, strSqlQry, FillCurrCode, ErrorHandler, TimeOutHandler);


            sqlstr = "sp_get_account_balance  '" + txtdiv.value + "','G','" + codeid + "'";

            ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr, FillBalance, ErrorHandler, TimeOutHandler);

            txtconvrate = document.getElementById("<%=txtConvRate.ClientId%>");

            //            if (trim(txtcurrcode.text) == trim(txtbase.value)) {
            //                txtconvrate.readOnly = true;
            //                txtconvrate.disabled = true;
            //            }
            //            else {
            //                txtconvrate.readOnly = false;
            //                txtconvrate.disabled = false;
            //            }
        }




        function FillBalance(result) {

            txtTranTypes = document.getElementById("<%=txtTranType.ClientID%>");
            txtbalan = document.getElementById("<%=txtBalance.ClientId%>");
        //  Added by Natraj on 28/03/2021
            txtbalanorig = document.getElementById("<%=txtBalanceOrig.ClientId%>");

            txtmodes = document.getElementById("<%=txtMode.ClientId%>");
            txtOldAmounts = document.getElementById("<%=txtOldAmount.ClientID%>");
            var balance = DecRound(result);


            if (txtmodes.value == 'Edit') {
                if (trim(txtTranTypes.value) == 'RV' || trim(txtTranTypes.value) == 'CV') {
                    balance = DecRound(DecRound(result) - DecRound(txtOldAmounts.value));
                    //txtbalan.value=DecFormat(balance);
                }
                else //if (trim(txtTranTypes.value)=='PV' )
                {
                    balance = DecRound(DecRound(result) + DecRound(txtOldAmounts.value));
                    //txtbalan.value=DecFormat( balance);
                }
            }
            
            //txtbalan.value=DecFormat( txtbalan.value);
            lblcrdr = document.getElementById("<%=lblBalCrDr.ClientId%>");
            lblorigcrdr = document.getElementById("<%=lblBalOrigCrDr.ClientId%>");
            if (DecRound(balance) > 0) {
                lblcrdr.innerHTML = "Cr";
                lblorigcrdr.innerHTML = "Cr";
            }
            else {
                lblcrdr.innerHTML = "Dr";
                lblorigcrdr.innerHTML = "Dr";
            }
            //txtbalan.value=Math.abs(balance);
            //alert(String(Math.abs(balance)));
            txtbalan.value = DecFormat(String(Math.abs(balance)));
            
            txtbalanorig.value = balance;
        }
        //        //----------------------------------------
        //        function FillGACode(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtaccd, txtacnm, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtcontcd, txtcontnm, txtboxauto) {
        //            alert('dd');
        //            txtgrdDebAmt = document.getElementById(txtdbamt);
        //            txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
        //            txtgrdCrAmt = document.getElementById(txtcramt);
        //            txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
        //            txtauto = document.getElementById(txtboxauto);

        //            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");


        //            ddltyp = document.getElementById(ddltp);
        //            var strtp = ddltyp.value;


        //            ddlIccode = document.getElementById(ddlIccd);
        //            ddlIcname = document.getElementById(ddlIcnm);
        //            ddlAConAcc = document.getElementById(ddlIContAcc);
        //            ddlAConAccNm = document.getElementById(ddlConAccnm);
        //            txtAConAcc = document.getElementById(txtcontcd);
        //            txtAConAccNm = document.getElementById(txtcontnm);

        //            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
        //            ddlIcname.value = codeid;

        //            txtaccd = document.getElementById(txtaccd);
        //            txtacnm = document.getElementById(txtacnm);
        //            txtaccd.value = ddlIccode.options[ddlIccode.selectedIndex].value;
        //            txtacnm.value = codeid;


        //            txtfill = document.getElementById(txtcurr);
        //            txtgrdcrate = document.getElementById(txtrate);

        //            //    sqlstr="select cur,cur from view_account where Code='" + ddlIcname.value + "'" ;
        //            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCurr,ErrorHandler,TimeOutHandler);
        //            //   
        //            //   
        //            //    var txtbase =document.getElementById('<%=txtbasecurr.ClientID%>');
        //            //    sqlstr="select convrate from currrates ,view_account  where  currrates.currcode=view_account.cur and   view_account.code='"+ ddlIcname.value +"' and Tocurr='"+ txtbase.value +"'"
        //            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillGrdCvntRate,ErrorHandler,TimeOutHandler);


        //            var sqlstr1, sqlstr2
        //            ddlAConAcc.disabled = false;
        //            ddlAConAccNm.disabled = false;
        //            if (strtp == 'C') {
        //                sqlstr1 = " select distinct view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";
        //                sqlstr2 = " select distinct  acctmast.acctname   from acctmast ,view_account where view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and   view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  acctmast.acctname";
        //            }
        //            else if (strtp == 'S') {
        //                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"

        //                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by acctmast.acctname"

        //            }
        //            else if (strtp == 'A') {
        //                sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

        //                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"
        //            }
        //            else if (strtp == 'G') {
        //                //     sqlstr1=" select '' as controlacctcode, '' as acctname  "  
        //                //     sqlstr2=" select '' as acctname , '' as controlacctcode "
        //                ddlAConAcc.value = '[Select]';
        //                ddlAConAccNm.value = '[Select]';
        //                ddlAConAcc.disabled = true;
        //                ddlAConAccNm.disabled = true;
        //            }

        //            if (strtp != '[Select]') {
        //                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
        //                constr = connstr.value
        //                if (strtp != 'G') {
        //                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);
        //                    //ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr2, FillControlAccName, ErrorHandler, TimeOutHandler);
        //                }
        //                FillCustDetails(strtp, codeid);
        //            }
        //            else {
        //                alert('Please Select Account Type');
        //                ddlIccode.value = "[Select]";
        //                ddlIcname.value = "[Select]";
        //                txtauto.value = "";
        //            }
        //        }
        //----------------------------------------
        function FillGAName(ddlIccd, ddlIcnm, txtcurr, txtrate, ddlIContAcc, ddlConAccnm, ddltp, txtaccd, txtacnm, txtcramt, txtcrbaseamt, txtdbamt, txtdbbaseamt, txtcontcd, txtcontnm, txtboxauto, txtSrcCtryName_AutoCompleteExtender, txtSrcctrycode, txtsrcctryname) {

            debugger;



            // var isforex;

            txtgrdDebAmt = document.getElementById(txtdbamt);
            txtgrdDbBaseAmt = document.getElementById(txtdbbaseamt);
            txtgrdCrAmt = document.getElementById(txtcramt);
            txtgrdCrBaseAmt = document.getElementById(txtcrbaseamt);
            txtauto = document.getElementById(txtboxauto);


            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");

            ddltyp = document.getElementById(ddltp);
            var strtp = ddltyp.value;

            ddlIccode = document.getElementById(ddlIccd);

            ddlIcname = document.getElementById(ddlIcnm);

            ddlAConAcc = document.getElementById(ddlIContAcc);

            ddlAConAccNm = document.getElementById(ddlConAccnm);

            txtAConAcc = document.getElementById(txtcontcd);
            txtAConAccNm = document.getElementById(txtcontnm);

            var codeid = ddlIccode.value;


            txtaccd = document.getElementById(txtaccd);
            txtacnm = document.getElementById(txtacnm);



            txtfill = document.getElementById(txtcurr);
            txtgrdcrate = document.getElementById(txtrate);

            var sqlstr1, sqlstr2
            ddlAConAcc.disabled = false;
            ddlAConAccNm.disabled = false;

            txtsrcctryname.disabled = false;
            if (strtp == 'C') {

                sqlstr1 = " select  view_account.controlacctcode, acctmast.acctname   from view_account  join acctmast on  view_account.div_code=acctmast.div_code and acctmast.div_code='" + txtdivauto.value + "'  and  view_account.controlacctcode= acctmast.acctcode  and type= '" + strtp + "' and view_account.code='" + codeid + "' order by  view_account.controlacctcode";


                var contxt = strtp + '||' + codeid;

                $find(txtSrcCtryName_AutoCompleteExtender).set_contextKey(contxt);


            }

            else if (strtp == 'S') {

                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" + txtdivauto.value + "'  and partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + codeid + "' order by controlacctcode"




            }
            else if (strtp == 'A') {
                sqlstr1 = " select distinct supplier_agents.controlacctcode   , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + codeid + "' order by controlacctcode"

                //                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where  acctmast.div_code='" + txtdivauto.value + "'  and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode ='" + codeid + "' order by acctmast.acctname"


            }
            else if (strtp == 'G') {

                txtsrcctryname.disabled = true;

                ddlAConAcc.value = '';
                ddlAConAccNm.value = ' ';
                ddlAConAcc.disabled = true;
                ddlAConAccNm.disabled = true;



            }


            if (strtp != '[Select]') {

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                if (strtp != 'G') {

                    ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillControlAcc, ErrorHandler, TimeOutHandler);

                }

                else {
                    var sqlstr1 ;
                    if (txtdivauto.value == "01") {
                        sqlstr1 = "select option_selected from reservation_parameters where param_id=85";
                    }
                    else {
                        sqlstr1 = "select option_selected from reservation_parameters where param_id=86";
                    }                    
                    ColServices.clsServices.GetQueryReturnStringnew(constr, sqlstr1, fillforex, ErrorHandler, TimeOutHandler);
                    // var isforex;

                    var aedamt = document.getElementById("<%=txtCnvAmount.ClientID%>");
                    var totaldiff = document.getElementById("<%=txttotBaseDiff.ClientID%>");
                    var totaldebit = document.getElementById("<%=txtTotalDebit.ClientID%>");
                    var totalcredit = document.getElementById("<%=txtTotalCredit.ClientID%>");
                    var totalbasedebit = document.getElementById("<%=txtTotBaseDebit.ClientID%>");
                    var totalbasecredit = document.getElementById("<%=txtTotBaseCredit.ClientID%>");
                    var ConvRate = document.getElementById("<%=txtConvRate.ClientID%>");
                    var txttrantype = document.getElementById("<%=txtTranType.ClientID%>");

                    function fillforex(result) {
                        if (result == trim(ddlIccode.value)) {
                            txtgrdDebAmt.value = 0;
                            txtgrdDbBaseAmt.value = 0;
                            txtgrdCrAmt.value = 0;
                            txtgrdCrBaseAmt.value = 0;
                            grdTotal();
                            var checkdiff;
                            if (txttrantype.value == "RV") {
                                checkdiff = DecRound(aedamt.value - totalbasecredit.value);
                            }
                            else if (txttrantype.value == "CV") {
                                checkdiff = DecRound(aedamt.value - totalbasecredit.value);
                            }
                            else if (txttrantype.value == "CPV" || txttrantype.value == "BPV") {
                                checkdiff = DecRound(aedamt.value - totalbasedebit.value);
                                checkdiff = checkdiff * -1;
                            }                           
                            if (checkdiff < 0) {
                                txtgrdDebAmt.value = Math.abs(checkdiff);
                                var amt1 = checkdiff;  //txtgrdcrate.Value;
                                txtgrdDbBaseAmt.value = Math.abs(amt1);                                
                            }
                            if (checkdiff > 0) {
                                txtgrdCrAmt.value = checkdiff;                                
                                var amt1 = checkdiff;   //ConvRate.Value;
                                txtgrdCrBaseAmt.value = Math.abs(amt1);                                
                            }
                            grdTotal();
                        }

                    }
                }


                //                                if (strtp == 'S') {
                //                var sqlstr2 = " select distinct partymast.ctrycode ,ctrymast.ctryname   from partymast  join ctrymast  on partymast.ctrycode= ctrymast.ctrycode where  partymast.partycode='" + codeid + "' order by partymast.ctrycode"
                //                    ColServices.clsServices.GetQueryReturnStringValuenew(constr, sqlstr2, 2, FillCtryCodeName, ErrorHandler, TimeOutHandler);
                //                }


                FillCustDetails(strtp, codeid);


            }


            else {
                alert('Please Select Account Type');

                ddlIccode.value = "";
                ddlIcname.value = "";
                txtauto.value = "";
            }
        }

        function getabs(absvalue) {
            return Math.abs(absvalue);
        }


        function FillCustDetails(typ, codeid) {

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById("<%=txtDivCode.ClientID%>");


            var crdsqlstr = "select cur,convrate,controlacctcode  from  view_account left outer join    currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where view_account.div_code='" + txtdiv.value + "' and code = '" + codeid + "' and type='" + typ + "' ";

            ColServices.clsServices.GetQueryReturnStringArraynew(constr, crdsqlstr, 3, FiilCustDt, ErrorHandler, TimeOutHandler);

        }
        function FillCtryName(result) {

            txtsrcctryname.value = result[1];
            txtsrcctrynm.disabled = true;
        }
        function FiilCustDt(result) {

            txtfill.value = result[0];
            txtgrdcrate.value = result[1];
            //            txtAConAccNm.value = ddlAConAccNm.value;

            GrdExchangeRateChange(result[1]);
            var txtbase = document.getElementById('ctl00_Main_txtbasecurr');

            if (trim(txtfill.value) == trim(txtbase.value)) {

                txtgrdcrate.readOnly = true;
                txtgrdcrate.style.backgroundColor = "#E6E6E6";
                //txtgrdcrate.disabled = true;
            }
            else {
                txtgrdcrate.readOnly = false;
                txtgrdcrate.style.backgroundColor = "white";
                //txtgrdcrate.disabled = false;
            }

            //ValidateMarketCode(codeid);
        }

        function FillCTCode(ddlIccd, ddlIcnm, txtcd, txtnm) {
            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIccode.options[ddlIccode.selectedIndex].text;
            ddlIcname.value = codeid;

            txtctd = document.getElementById(txtcd);
            txtctnm = document.getElementById(txtnm);
            txtctd.value = ddlIccode.options[ddlIccode.selectedIndex].value;
            txtctnm.value = codeid;

        }

        function FillCTName(ddlIccd, ddlIcnm, txtcd, txtnm) {

            ddlIccode = document.getElementById(ddlIccd);
            ddlIcname = document.getElementById(ddlIcnm);

            var codeid = ddlIcname.options[ddlIcname.selectedIndex].text;
            ddlIccode.value = codeid;

            txtctd = document.getElementById(txtcd);
            txtctnm = document.getElementById(txtnm);
            txtctd.value = codeid;
            txtctnm.value = ddlIcname.options[ddlIcname.selectedIndex].value;

        }


        //        function ValidateMarketCode(ddlAcc) {
        //            var crdsqlstr = "SELECT COUNT(acctcode) FROM acctgroup Where childid IN (115,120) and acctcode='" + ddlAcc + "'";
        //            ColServices.clsServices.GetAcctCodeCount(constr, crdsqlstr, ValidAlertMsg, ErrorHandler, TimeOutHandler);
        //        }

        //        function ValidAlertMsg(result) {
        //            var count = result;
        //        }

        //----------------------------------------
        function FillCurr(result) {

            txtfill.value = result[0].ListText;

            //    sqlstr="select convrate,convrate from currrates where currcode='"+ txtfill.value +"' and Tocurr='"+ txtbase.value +"'"

            //    ColServices.clsServices.GetQueryReturnStringValuenew(constr,sqlstr,FillCvntRate,ErrorHandler,TimeOutHandler);

        }
        //-----------------------------------------
        function AssignConvRate() {
            document.getElementById('<%=hdnConvRate.ClientID%>').value = document.getElementById('<%=txtConvRate.ClientID%>').value;
        }
        function FillCvntRate(result) {
            txtt = document.getElementById('<%=txtConvRate.ClientID%>');
            txtt.value =  (result); //removed val Tanvir 20102023
            document.getElementById('<%=hdnConvRate.ClientID%>').value = txtt.value;
            ExchangeRateChange(result);
                
        }
        function FillGrdCvntRate(result) {
            txtgrdcrate.value = DecRound(result);
            GrdExchangeRateChange(result)
            var txtbase = document.getElementById('<%=txtbasecurr.ClientID%>');


            if (trim(txtfill.value) == trim(txtbase.value)) {

                txtgrdcrate.readOnly = true;
            }
            else {

                txtgrdcrate.readOnly = false;
            }

        }


        function FillCombotoText(ddlc, txtt) {
            ddlcs = document.getElementById(ddlc);
            txtts = document.getElementById(txtt);

            var codeid = ddlcs.options[ddlcs.selectedIndex].text;
            txtts.value = codeid;
        }

        function changedate(trandate, chkdate) {

            trandate = document.getElementById(trandate);

            chkdate = document.getElementById(chkdate);

            chkdate.value = trandate.value;
        }


        function fillinkeyup(ddltp, AutoCompleteExtender) {
            ddltyp = document.getElementById(ddltp);
            var strtp = ddltyp.value;
            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");
            var contxt = strtp + '||' + txtdivauto.value;
            //        alert(contxt);
            $find(AutoCompleteExtender).set_contextKey(contxt);
        }
        function fill_acountcode(ddltp, AutoCompleteExtender, ddlcode, ddlname, ddlConAccd, ddlConAccnm, txtgnarr, ddlcostcd, ddlcostnm, txtboxauto, txtgrdcurr, txtgrdconvrate, txtgrddebt, txtgrdbsdebt, txtgrdcr, txtgrdbscr, txtgrdsrcctryc, txtgrdsrcctryn, txtdiv) {
            //            alert('dd');

            ddltyp = document.getElementById(ddltp);
            //  AutoCompleteExtender = document.getElementById(AutoCompleteExtender);
            var strtp = ddltyp.value;
            ddlACode = document.getElementById(ddlcode);
            ddlACode.value = '';
            ddlAName = document.getElementById(ddlname);
            ddlAName.value = '';
            ddlAConAcc = document.getElementById(ddlConAccd);
            ddlAConAccNm = document.getElementById(ddlConAccnm);
            ddlAConAcc.value = '';
            ddlAConAccNm.value = '';

            txtgrdcurr = document.getElementById(txtgrdcurr);

            txtgrdcurr.value = '';
            txtgrdconvrate = document.getElementById(txtgrdconvrate);
            txtgrdconvrate.value = '';
            txtgrddebt = document.getElementById(txtgrddebt);
            txtgrddebt.value = '';
            txtgrdbsdebt = document.getElementById(txtgrdbsdebt);
            txtgrdbsdebt.value = '';
            txtgrdcr = document.getElementById(txtgrdcr);
            txtgrdcr.value = '';
            txtgrdbscr = document.getElementById(txtgrdbscr);
            txtgrdbscr.value = '';

            txtgrdsrcctryc = document.getElementById(txtgrdsrcctryc);
            txtgrdsrcctryc.value = '';
            txtgrdsrcctryn = document.getElementById(txtgrdsrcctryn);
            txtgrdsrcctryn.value = '';
            ddlcostcode = document.getElementById(ddlcostcd);
            ddlcostname = document.getElementById(ddlcostnm);
            var txtdivauto = document.getElementById("<%=txtDivCode.ClientId%>");
            var contxt = strtp + '||' + txtdivauto.value;

            $find(AutoCompleteExtender).set_contextKey(contxt);







            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value



            //            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr3, FillControlAcc, ErrorHandler, TimeOutHandler);
            //            ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr4, FillControlAccName, ErrorHandler, TimeOutHandler);



            txtgnarrs = document.getElementById(txtgnarr);
            txtnarr = document.getElementById("<%=txtnarration.ClientID%>");
            txtgnarrs.value = trim(txtnarr.value);

            ddlcostcode.disabled = false;
            ddlcostname.disabled = false;
            if (strtp == 'G') {

                txtgrdsrcctryn.disabled = true;
                ddlcostcode.disabled = false;
                ddlcostname.disabled = false;
            }
            else {
                ddlcostcode.disabled = true;
                ddlcostname.disabled = true;
            }


            OnChangeType(txtboxauto, ddlname, ddltp);

        }

        function FillACodes(result) {
            RemoveAll(ddlACode)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlACode.options.add(option);
            }
            ddlACode.value = "[Select]";
        }

        function FillANames(result) {
            RemoveAll(ddlAName)
            for (var i = 0; i < result.length; i++) {
                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlAName.options.add(option);
            }
            ddlAName.value = "[Select]";
        }

        function FillControlAcc(result) {
            if (result != '') {

                ddlAConAcc.value = result[0].ListText;

                ddlAConAcc.readonly = true;
                ddlAConAcc.disabled = true;
                txtAConAcc.value = result[0].ListText;

                ddlAConAccNm.value = result[0].ListValue;
                ddlAConAccNm.readonly = true;
                ddlAConAccNm.disabled = true;
                txtAConAccNm.value = result[0].ListValue;
            }
            else {
                ddlAConAccNm.value = '';
                ddlAConAcc.value = '';
            }
            ////            RemoveAll(ddlAConAcc)
            //            for (var i = 0; i < result.length; i++) {
            //                var option = new Option(result[i].ListText, result[i].ListValue);
            //                ddlAConAcc.value=option;
            //            }
            ////            ddlAConAcc.value = "[Select]";


            //            if (sddltyp != '[Select]' && sddlACode != null) {
            //                //    alert(sddltyp + ' ' + sddlACode);
            //                //    FillCustDetails(sddltyp, sddlACode);
            //            }
        }
        function FillControlAccName(result) {
            RemoveAll(ddlAConAccNm)
            for (var i = 0; i < result.length; i++) {

                var option = new Option(result[i].ListText, result[i].ListValue);
                ddlAConAccNm.options.add(option);
            }
            ddlAConAccNm.value = " ";


            if (sddltyp != '[Select]' && sddlACode != null) {
                //  alert(sddltyp +' ' + sddlACode);
                //   FillCustDetails(sddltyp, sddlACode);
            }
        }
        //---------------------------------------------------

        //-----    Common

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        //---------------------------------------------------
        function ExchangeRateChange(result) {
            txtDBAmt = document.getElementById("<%=txtAmount.ClientID%>");
            txtCnvtRate = result;
            txtCnvtAmt = document.getElementById("<%=txtCnvAmount.ClientID%>");

            if (txtDBAmt.value == '') { txtDBAmt.value = 0; }
            var cAmt = DecRound(txtDBAmt.value) * txtCnvtRate;
            txtCnvtAmt.value = DecRound(cAmt);

        }
        // function  ExchangeRateChange(txtDBAmt,txtCnvtRate,txtCnvtAmt)
        // {
        //    var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
        //    nodecround=Math.pow(10,parseInt(txtdec.value));


        //    txtDBAmt = document.getElementById(txtDBAmt);
        //    txtCnvtRate = document.getElementById(txtCnvtRate);
        //    txtCnvtAmt = document.getElementById(txtCnvtAmt);  

        //    if (txtDBAmt.value==''){txtDBAmt.value=0;}
        //    var cAmt=parseFloat(txtDBAmt.value) *  parseFloat(txtCnvtRate.value) ;
        //    txtCnvtAmt.value =Math.round(cAmt*nodecround)/nodecround;
        //    //cAmt.toFixed(3);
        //     
        //    //Call Grd Total 
        //  
        // }
        //---------------------------------------------------
        function GrdExchangeRateChange(result) {
            txtCnvtRate = result;

            if (trim(txtgrdDebAmt.value) == '') { txtgrdDebAmt.value = 0; }
            if (isNaN(txtgrdDebAmt.value) == true) { txtgrdDebAmt.value = 0; }
            if (trim(txtgrdCrAmt.value) == '') { txtgrdCrAmt.value = 0; }
            if (isNaN(txtgrdCrAmt.value) == true) { txtgrdCrAmt.value = 0; }

            var amt = DecRound(txtgrdDebAmt.value) * parseFloat(txtCnvtRate);
            txtgrdDbBaseAmt.value = DecRound(amt);


            var amt1 = DecRound(txtgrdCrAmt.value) * parseFloat(txtCnvtRate);
            txtgrdCrBaseAmt.value = DecRound(amt1);

            // grdTotal()
        }

        function convertInRate(txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {

            var txtgrdDebAmt1 = document.getElementById(txtdbamt);

            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }


            if (pstr == 'Debit') {
                if (Number(txtgrdCrAmt1.value) > 0) {

                    txtgrdDebAmt1.value = 0;
                    //  txtgrdDebAmt1.readOnly=true;
                }
                else if (Number(txtgrdCrAmt1.value) == 0 || txtgrdCrAmt1.value == '') {

                    // txtgrdCrAmt1.readOnly=true;
                }
            }
            else if (pstr == 'Credit') {

                if (Number(txtgrdDebAmt1.value) > 0) {
                    txtgrdCrAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
                else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                    //txtgrdDebAmt1.readOnly=true;
                }
            }


            var amt = DecRound(txtgrdDebAmt1.value) * parseFloat(txtCnvtRate1.value);

            txtgrdDbBaseAmt1.value = DecRound(amt);

            var amt1 = DecRound(txtgrdCrAmt1.value) * parseFloat(txtCnvtRate1.value);
            txtgrdCrBaseAmt1.value = DecRound(amt1);
            grdTotal()
        }

        function convertInRateBase(txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {

            var txtgrdDebAmt1 = document.getElementById(txtdbamt);
            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }
            if (trim(txtgrdDbBaseAmt1.value) == '') { txtgrdDbBaseAmt1.value = 0; }
            if (isNaN(txtgrdDbBaseAmt1.value) == true) { txtgrdDbBaseAmt1.value = 0; }
            if (trim(txtgrdCrBaseAmt1.value) == '') { txtgrdCrBaseAmt1.value = 0; }
            if (isNaN(txtgrdCrBaseAmt1.value) == true) { txtgrdCrBaseAmt1.value = 0; }

            if (pstr == 'Debit') {
                if (Number(txtgrdCrBaseAmt1.value) > 0) {

                    txtgrdDbBaseAmt1.value = 0;
                    //  txtgrdDebAmt1.readOnly=true;
                }
                //         else if (Number(txtgrdCrBaseAmt1.value) == 0 || txtgrdCrBaseAmt1.value == '') {

                //             // txtgrdCrAmt1.readOnly=true;
                //         }
            }
            else if (pstr == 'Credit') {

                if (Number(txtgrdDbBaseAmt1.value) > 0) {
                    txtgrdCrBaseAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
                //         else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                //             //txtgrdDebAmt1.readOnly=true;
                //         }
            }


            var amt = DecRound(txtgrdDbBaseAmt1.value) / parseFloat(txtCnvtRate1.value);

            txtgrdDebAmt1.value = DecRound(amt);

            var amt1 = DecRound(txtgrdCrBaseAmt1.value) / parseFloat(txtCnvtRate1.value);
            txtgrdCrAmt1.value = DecRound(amt1);
            grdTotal()
        }


        function convertInRateAmount(txtDBAmt, txtCnvtRate, txtCnvtAmt) {

            txtDBAmt = document.getElementById(txtDBAmt);
            txtCnvtRate = document.getElementById(txtCnvtRate);
            txtCnvtAmt = document.getElementById(txtCnvtAmt);

            if (trim(txtDBAmt.value) == '') { txtDBAmt.value = 0; }
            var amt = DecRound(txtDBAmt.value) * txtCnvtRate.value;
            txtCnvtAmt.value = DecRound(amt);
            //Call Grd Total 
            grdTotal()
        }


        function convertInRateBaseAmount(txtDBAmt, txtCnvtRate, txtCnvtAmt) {

            txtDBAmt = document.getElementById(txtDBAmt);
            txtCnvtRate = document.getElementById(txtCnvtRate);
            txtCnvtAmt = document.getElementById(txtCnvtAmt);

            if (trim(txtDBAmt.value) == '') { txtDBAmt.value = 0; }
            var amt = DecRound(txtDBAmt.value) / txtCnvtRate.value;
            txtCnvtAmt.value = DecRound(amt);
            //Call Grd Total 
            grdTotal()
        }

        //---------------------------
        function DecRoundEightPalces(amtToRound) {

            //var  txtdec=document.getElementById("<%=txtdecimal.ClientID%>");
            nodecround = Math.pow(10, parseInt(8));
            var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
            return parseFloat(rdamt);
        }

        function convertRateOnBaseCurrency(typ, txtdbamt, txtdebbaseamt, txtcramt, txtcrbaseamt, txtcnvRate, pstr) {
            var acctyp = document.getElementById(typ);
            var txtgrdDebAmt1 = document.getElementById(txtdbamt);
            var txtgrdDbBaseAmt1 = document.getElementById(txtdebbaseamt);
            var txtgrdCrAmt1 = document.getElementById(txtcramt);
            var txtgrdCrBaseAmt1 = document.getElementById(txtcrbaseamt);
            var txtCnvtRate1 = document.getElementById(txtcnvRate);

            if (trim(txtgrdDebAmt1.value) == '') { txtgrdDebAmt1.value = 0; }
            if (isNaN(txtgrdDebAmt1.value) == true) { txtgrdDebAmt1.value = 0; }
            if (trim(txtgrdCrAmt1.value) == '') { txtgrdCrAmt1.value = 0; }
            if (isNaN(txtgrdCrAmt1.value) == true) { txtgrdCrAmt1.value = 0; }


            if (pstr == 'Debit') {
                if (Number(txtgrdCrBaseAmt1.value) > 0) {

                    txtgrdDbBaseAmt1.value = 0;
                }
                else if (Number(txtgrdDebAmt1.value) == 0 || txtgrdDebAmt1.value == '') {
                    txtgrdDebAmt1.value = 0;
                    //txtgrdDebAmt1.readOnly=true;
                }

            }
            else if (pstr == 'Credit') {
                if (Number(txtgrdDbBaseAmt1.value) > 0) {
                    txtgrdCrBaseAmt1.value = 0;

                }
                else if (Number(txtgrdCrAmt1.value) == 0 || txtgrdCrAmt1.value == '') {
                    txtgrdCrBaseAmt1.value = 0;
                    // txtgrdCrAmt1.readOnly=true;
                }
            }

            if (acctyp.value != 'G') {
                if (pstr == 'Debit') {
                    if (txtgrdDebAmt1.value == '' || txtgrdDebAmt1.value == 0) {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdDebAmt1.value = DecRoundEightPalces(amt);
                    }
                    else {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtgrdDebAmt1.value);
                        txtCnvtRate1.value = DecRoundEightPalces(amt);
                    }
                }
                else if (pstr == 'Credit') {
                    if (txtgrdCrAmt1.value == '' || txtgrdCrAmt1.value == 0) {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdCrAmt1.value = DecRoundEightPalces(amt);
                    }
                    else {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtgrdCrAmt1.value);
                        txtCnvtRate1.value = DecRoundEightPalces(amt);
                    }
                }
            }
            else {
                if (pstr == 'Debit') {
                    if (parseFloat(txtgrdDbBaseAmt1.value) > 0) {
                        var amt = DecRound(txtgrdDbBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdDebAmt1.value = DecRoundEightPalces(amt);
                    }
                }
                else if (pstr == 'Credit') {
                    if (parseFloat(txtgrdCrBaseAmt1.value) > 0) {
                        var amt = DecRound(txtgrdCrBaseAmt1.value) / DecRound(txtCnvtRate1.value);
                        txtgrdCrAmt1.value = DecRoundEightPalces(amt);
                    }
                }
            }

            grdTotal()
        }

        //---------------------------------------------------

        function grdTotal() {
            var totCr = 0;
            var totDr = 0;
            var totbCr = 0;
            var totbDr = 0;

            var objGridView = document.getElementById('<%=grdReceipt.ClientID%>');

            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');

            intRows = txtrowcnt.value;

            for (j = 1; j <= intRows; j++) {


                var valDr = objGridView.rows[j].cells[9].children[0].value;

                var valCr = objGridView.rows[j].cells[9].children[0].value;

                var valbDr = objGridView.rows[j].cells[10].children[0].value;
                var valbCr = objGridView.rows[j].cells[11].children[0].value;

                if (valCr == '') { valCr = 0; }
                if (valDr == '') { valDr = 0; }
                if (valbCr == '') { valbCr = 0; }
                if (valbDr == '') { valbDr = 0; }

                if (isNaN(valCr) == true) { valCr = 0; }
                if (isNaN(valDr) == true) { valDr = 0; }

                if (isNaN(valbCr) == true) { valbCr = 0; }
                if (isNaN(valbDr) == true) { valbDr = 0; }


                totCr = DecRound(totCr) + DecRound(valCr);
                totDr = DecRound(totDr) + DecRound(valDr);

                totbCr = DecRound(totbCr) + DecRound(valbCr);
                totbDr = DecRound(totbDr) + DecRound(valbDr);


            }

            var txttotCr = document.getElementById('<%=txtTotalCredit.ClientID%>');
            var txttotDr = document.getElementById('<%=txtTotalDebit.ClientID%>');

            var txttotbCr = document.getElementById('<%=txtTotBaseCredit.ClientID%>');
            var txttotbDr = document.getElementById('<%=txtTotBaseDebit.ClientID%>');

            var txttotbDiff = document.getElementById('<%=txtTotBaseDiff.ClientID%>');

            txttotDr.value = DecRound(totDr);
            txttotCr.value = DecRound(totCr);

            txttotbDr.value = DecRound(totbDr);
            txttotbCr.value = DecRound(totbCr);

            txttotbDiff.value = DecRound(totbCr - totbDr);
            txttotbDiff.value = Math.abs(txttotbDiff.value);
        }
        //---------------------------------------------------

        var ACode = null;
        var conCode = null;
        var typ = null;
        var tranId = null;
        var lno = null;
        var currCode = null;
        var CurRate = null;
        var creditamt = null;
        var baseamt = null;
        var oldlno = null;
        var reqsid = null;

        function openAdjustBill(ddlACode, ddlconCode, ddltyp, txtTranId, txtlno, txtcurrCode, txtCurRate, txtcreditamt, txtcreditbaseamt, txtoldlno, txtdebitamt, txtdebitbaseamt, reqid) {
            var ddlACode = document.getElementById(ddlACode);
            var ddlContCode = document.getElementById(ddlconCode);
            ddltyp = document.getElementById(ddltyp);
            txtTranId = document.getElementById(txtTranId);
            var txtLineNo = document.getElementById(txtlno);
            var txtOLineNo = document.getElementById(txtoldlno);
            txtCurrsCode = document.getElementById(txtcurrCode);
            txtCurRate = document.getElementById(txtCurRate);
            //            txtBaseAmt = document.getElementById(txtbaseamt);
            //            txtCreditAmt = document.getElementById(txtcreditamt);
            txtrequestid = document.getElementById(reqid);

            ACode = ddlACode.value;
            conCode = ddlContCode.value;
            typ = ddltyp.value;
            tranId = txtTranId.value;
            lno = txtLineNo.value;
            oldlno = txtOLineNo.value;
            currCode = txtCurrsCode.value;
            CurRate = txtCurRate.value;
            //            creditamt = txtCreditAmt.value;
            //            baseamt = txtBaseAmt.value;
            if (txtrequestid != null) {
                if (txtrequestid != undefined) {
                    reqsid = txtrequestid.value;
                    if (reqsid == 0) { reqsid = ''; }
                } else {
                    reqsid = '';
                }
            } else {
                reqsid = '';
            }
            var sqlstr = null;

            txttrantype = document.getElementById("<%=txtTranType.ClientID%>");
            txtgrdtype = document.getElementById("<%=txtGridType.ClientID%>");

            var txtCrAmt = document.getElementById(txtcreditamt);
            var txtCrBaseAmt = document.getElementById(txtcreditbaseamt);
            var txtDRAmt = document.getElementById(txtdebitamt);
            var txtDRBaseAmt = document.getElementById(txtdebitbaseamt);

            var passBaseAmt = 0;
            var passAmt = 0;

            var valCr = DecRound(txtCrAmt.value);
            var valDr = DecRound(txtDRAmt.value);
            var valCrBase = DecRound(txtCrBaseAmt.value);
            var valDrBase = DecRound(txtDRBaseAmt.value);


            if (valCr == '' || valCr == 0) {
                passAmt = valDr;
                passBaseAmt = valDrBase;
                txtgrdtype.value = 'Debit';
            }
            if (valDr == '' || valDr == 0) {
                passAmt = valCr;
                passBaseAmt = valCrBase;
                txtgrdtype.value = 'Credit';
            }
            if (passAmt == 0) {
                alert("Please enter debit or credit amount.")
                return false;
            }



            //            if (txttrantype.value == 'RV') {
            //                txtgrdtype.value = 'Credit';
            //            }
            //            else //if (txttrantype.value=='PV' )
            //            {
            //                txtgrdtype.value = 'Debit';
            //            }

            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
            intRows = txtrowcnt.value;
            txtstate = document.getElementById("<%=txtMode.ClientID%>");
            txtrefcode = document.getElementById("<%=txtDocNo.ClientID%>");
            txtAdjcolcode = document.getElementById("<%=txtAdjcolno.ClientID%>");
            txtDate = document.getElementById("<%=txtDate.ClientID%>");

            var txtdiv = document.getElementById('<%=txtDivcode.ClientID%>');

            var pass;

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            if (reqsid != '') {
                if (ddltyp.value != 'G') {
                    if (ddlContCode.value == '[Select]') {
                        alert('Select Control Account..');
                        return false;
                    }

                    sqlstr = "select distinct acc_type,acc_code from reservation_invoice_detail where acc_type='" + typ + "' and requestid='" + reqsid + "' and acc_code='" + ddlACode.value + "'"
                    ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, 2, validate_supp, ErrorHandler, TimeOutHandler);
                }
                else {
                    alert('Account type ‘G’ doesn’t  adjust bill .');
                    return false;
                }
            }
            else {

                if (ddltyp.value != 'G') {
                    if (ddlContCode.value == '[Select]') {
                        alert('Select Control Account..');
                        return false;
                    }


                    if (txtrequestid.value == 0) { txtrequestid.value = ''; }

                    pass = "TranType=" + txttrantype.value + "&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.value + "&TranId=" + txtTranId.value + "&lineNo=" + txtLineNo.value + "&divid=" + txtdiv.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + passAmt + "&BaseAmount=" + passBaseAmt + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + txtOLineNo.value + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&trandate=" + txtDate.value + "&Requestid=" + txtrequestid.value;

                    window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,left=0,top=0,height=' + screen.height + ',width=' + screen.width);
                    return false;

                } else {
                    alert('Account type ‘G’ doesn’t  adjust bill .');
                    return false;
                }
            }
        }

        function validate_supp(result) {
            txttrantype = document.getElementById("<%=txtTranType.ClientID%>");
            txtgrdtype = document.getElementById("<%=txtGridType.ClientID%>");
            txtDate = document.getElementById("<%=txtDate.ClientID%>");

            if (txttrantype.value == 'RV' || txttrantype.value == 'CV') {
                txtgrdtype.value = 'Credit';
            }
            else //if (txttrantype.value=='PV' )
            {
                txtgrdtype.value = 'Debit';
            }

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value

            var txtdiv = document.getElementById('<%=txtDivcode.ClientID%>');

            var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');
            intRows = txtrowcnt.value;
            txtstate = document.getElementById("<%=txtMode.ClientID%>");
            txtrefcode = document.getElementById("<%=txtDocNo.ClientID%>");
            txtAdjcolcode = document.getElementById("<%=txtAdjcolno.ClientID%>");
            var pass;
            if (typ != 'G') {

                if (result[0] != typ || result[1] != ACode) {
                    alert('Booking no. does not matching for this account code');
                    return false;
                }

                //         pass = "TranType=" + txttrantype.value + "&AccCode=" + ddlACode.value + "&ControlCode=" + ddlContCode.value + "&AccType=" + ddltyp.value + "&TranId=" + txtTranId.value + "&lineNo=" + txtLineNo.value + "&currcode=" + txtCurrsCode.value + "&currrate=" + txtCurRate.value + "&Amount=" + txtCreditAmt.value + "&BaseAmount=" + txtBaseAmt.value + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + txtOLineNo.value + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&Requestid=" + txtrequestid.value;
                pass = "TranType=" + txttrantype.value + "&AccCode=" + ACode + "&ControlCode=" + conCode + "&AccType=" + typ + "&TranId=" + tranId + "&lineNo=" + lno + "&divid=" + txtdiv.value + "&currcode=" + currCode + "&currrate=" + CurRate + "&Amount=" + creditamt + "&BaseAmount=" + baseamt + "&Gridtype=" + txtgrdtype.value + "&MainGrdCount=" + intRows + "&OlineNo=" + oldlno + "&State=" + txtstate.value + "&RefCode=" + txtrefcode.value + "&AdjColno=" + txtAdjcolcode.value + "&trandate=" + txtDate.value + "&Requestid=" + reqsid;

                //window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,fullscreen=yes');

                window.open('ReceiptsAdjustBills.aspx?' + pass, 'ReceiptsAdjustBills', 'toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1,left=0,top=0,height=' + screen.height + ',width=' + screen.width);

                return false;

            } else {
                alert('Account type ‘G’ doesn’t  adjust bill .');
                return false;
            }
        }

        function validate_click() {
            var txtCredit = document.getElementById("<%=txtTotalCredit.ClientID%>");
            var txtBaseDebit = document.getElementById("<%=txtTotalDebit.ClientID%>");
            var txtBaseCredit = document.getElementById("<%=txtTotBaseCredit.ClientID%>");
            var txtBaseDebit = document.getElementById("<%=txtTotBaseDebit.ClientID%>");

            var chkBlank = document.getElementById("<%=chkBlank.ClientID%>");
            var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
            var btnss = document.getElementById("<%=btnsave.ClientID%>");

            hdnSS.value = 0;
            /*
            if ((txtBaseDebit.value == '') || (txtBaseDebit.value == '0')) {
            if (chkBlank.checked == false) {
            alert('Please select Allow blank');
            chkBlank.disabled = false;
            return false;
            }
            {
            return true;
            }
            }*/

            if (hdnSS.value == 0) {
                hdnSS.value = 1;
                btnss.style.visibility = "hidden";
                return true;
            }
            else {
                return false;
            }

            //return true;                

        }



 
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                &nbsp;&nbsp;
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    border-bottom: gray 2px solid">
                    <tbody>
                        <tr>
                            <td class="field_heading" align="center">
                                <asp:Label ID="lblHeading" runat="server" Text="Recepits" Width="896px" CssClass="field_heading"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="td_cell">
                                    <tbody>
                                        <tr style="height: 30px">
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Receipt No." Width="80px" CssClass="field_caption"></asp:Label>
                                                <input id="txtDocNo" class="field_input" tabindex="1" readonly="readonly" type="text"
                                                    maxlength="50" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Style="padding-left: 20px" Text="Receipt Date"
                                                    Width="80px" CssClass="field_caption"></asp:Label>
                                                <asp:TextBox ID="txtDate" TabIndex="2" runat="server" Width="75px" CssClass="fiel_input"
                                                    ValidationGroup="MKE"></asp:TextBox><asp:ImageButton ID="ImgBtnFrmDt" TabIndex="3"
                                                        runat="server" ImageUrl="..\Images\Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator
                                                            ID="MskVFromDt" runat="server" Width="23px" CssClass="field_error" ValidationGroup="MKE"
                                                            ControlExtender="MskFromDate" ControlToValidate="txtDate" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                <div style="padding-left: 100px;">
                                                    <asp:Label ID="lbldate" runat="server" Text="Posting date" Font-Size="X-Small" ForeColor="Red"
                                                        Width="70px" Style="display: none"></asp:Label>
                                                </div>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="Label7" Style="padding-left: 20px" runat="server" Text="Cash/Bank"
                                                    Width="100px" CssClass="field_caption"></asp:Label>
                                                <select style="width: 190px" id="ddlCashBank" class="field_input" tabindex="4" runat="server" >
                                                    <option value="Cash">Cash</option>                                                    
                                                    <option value="Bank" selected="selected">Bank</option>
                                                </select>

                                                 <asp:Label ID="Label1" Style="padding-left: 20px" runat="server" Text=" Balance (AED)"
                                                    Width="100px"></asp:Label>
                                                <input style="text-align: right;" id="txtBalance" class="field_input" disabled="disabled"
                                                    type="text" maxlength="50" runat="server" />&nbsp;<asp:Label ID="lblBalCrDr" runat="server"
                                                        Text="--" Width="18px" CssClass="field_caption"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 35px">
                                            <td colspan="2" style="width: 397px; color: #000000">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="Bank Name" Width="80px" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtBankName" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                MaxLength="500" TabIndex="5" Width="300px" Height="16px"></asp:TextBox>
                                                            <asp:TextBox ID="TxtBankCode" runat="server" Style="display: none"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtBankName" runat="server" CompletionInterval="10"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="10" DelimiterCharacters=""
                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                ServiceMethod="Getbankslist" TargetControlID="TxtBankName" ContextKey="True"
                                                                OnClientItemSelected="BankNameautocompleteselected" ServicePath="">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="2">
                                                

                                                        <%--Added by Natraj on 28/03/2021--%>
                                                        <asp:Label ID="Label11" Style="padding-left: 20px" runat="server" Text=" Balance"
                                                    Width="100px"></asp:Label>
                                                <input style="text-align: right;" id="txtBalanceOrig" class="field_input" disabled="disabled"
                                                    type="text" maxlength="50" runat="server" />&nbsp;<asp:Label ID="lblBalOrigCrDr" runat="server"
                                                        Text="--" Width="18px" CssClass="field_caption"></asp:Label>

                                                       


                                                <asp:Label ID="lblChB" Style="padding-left: 20px" runat="server" Text="CustomerBank"
                                                    Width="100px"></asp:Label>
                                                <asp:TextBox ID="txtCustBankName" runat="server" CssClass="field_input" MaxLength="500"
                                                    TabIndex="14" Width="170px" Height="16px"></asp:TextBox>
                                                <asp:TextBox ID="txtCustBankCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtCustBankName_AutoCompleteExtender" runat="server"
                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                    FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetCustbankslist"
                                                    TargetControlID="txtCustBankName" OnClientItemSelected="CustBankNameautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                                <select style="width: 136px" visible="false" id="ddlCustBank" class="field_input"
                                                    tabindex="15" runat="server">
                                                </select>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px">
                                            <td colspan="2">
                                                <asp:Label ID="Label6" runat="server" Text="Currency" Width="80px" CssClass="field_caption"
                                                    Font-Strikeout="False"></asp:Label>
                                                <input id="txtcurrency" class="field_input" tabindex="9" type="text" maxlength="50"
                                                    readonly="readonly" runat="server" />
                                                <asp:Label ID="Label4" Style="padding-left: 20px" runat="server" Width="80px" Text="Conv.Rate"
                                                    CssClass="field_caption"></asp:Label>
                                                <input id="txtConvRate" style="text-align: right;" class="field_input"
                                                    tabindex="10" type="text" maxlength="50" runat="server" onblur = "AssignConvRate();" />
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblRecfrom" Style="padding-left: 20px" runat="server" Text="ReceivedFrom "
                                                    Width="100px" CssClass="field_caption"></asp:Label>
                                                <input style="width: 410px" id="txtReceived" class="field_input" tabindex="11" type="text"
                                                    maxlength="100" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="height: 30px">
                                            <td colspan="2">
                                                <asp:Label ID="Label10" runat="server" Text="Amount " Width="80px" CssClass="field_caption"></asp:Label>
                                                <input style="text-align: right" id="txtAmount" class="field_input" type="text" maxlength="50"
                                                    tabindex="12" runat="server" />
                                                <asp:Label ID="lblBaseAmt" Style="padding-left: 20px" runat="server" Text="Base Amount"
                                                    Width="80px" CssClass="field_caption"></asp:Label>
                                                <input style="text-align: right" id="txtCnvAmount" readonly="readonly" class="field_input"
                                                    type="text" maxlength="50" tabindex="13" runat="server" />
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblChN" Style="padding-left: 20px" runat="server" Text="Cheque No"
                                                                Width="100px" CssClass="field_caption" Font-Strikeout="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <input id="txtChequeNo" class="field_input" tabindex="6" type="text" maxlength="40"
                                                                runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblChD" runat="server" Text="Cheque Date" CssClass="field_caption"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtChequeDate" TabIndex="7" runat="server" Width="70px" CssClass="fiel_input"
                                                                ValidationGroup="MKE"></asp:TextBox>&nbsp;<asp:ImageButton ID="ImageButton1" TabIndex="8"
                                                                    runat="server" ImageUrl="..\Images\Calendar_scheduleHS.png"></asp:ImageButton>&nbsp;<cc1:MaskedEditValidator
                                                                        ID="MaskedEditValidator1" runat="server" Width="23px" CssClass="field_error"
                                                                        ValidationGroup="MKE" ControlExtender="MskChequeDate" ControlToValidate="txtChequeDate"
                                                                        Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                                                        ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                                        TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                        </td>
                                                        <td align="left" style="padding-left: 5px">
                                                            <asp:Label ID="lblpostdatebank" runat="server" Text="Posting date" Font-Size="X-Small"
                                                                ForeColor="Red" Width="70px" Style="display: none"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px">
                                            <td colspan="4">
                                                <asp:Label ID="Label3" runat="server" Text="Narration" Width="80px" CssClass="field_caption"></asp:Label>
                                                <asp:TextBox ID="txtnarration" TabIndex="15" runat="server" Width="985px" CssClass="field_input"
                                                    MaxLength="5000" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grdReceipt" TabIndex="16" runat="server" Font-Size="10px" CssClass="td_cell"
                                    Width="100%" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999"
                                    BorderStyle="None" CellPadding="3" GridLines="Vertical">
                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText=" A/C Code <br/>  Control A/C Code" HeaderStyle-Wrap="true" ItemStyle-Width="0px" >
                                            <ItemTemplate>
                                                <div style="padding-top: 5px; ">
                                                    <asp:TextBox ID="txtGAccCode" runat="server" Width="100px" onkeyup="SetContextKey()"></asp:TextBox>
                                                    <asp:TextBox ID="txtConAccCode" runat="server" Width="100px"></asp:TextBox>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <select id="ddlType" runat="server" class="field_input MyAutoCompleteTypeClass" style="width: 75px"
                                                    tabindex="0">
                                                </select>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="middle" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" A/C Name <br/>  Control A/C Name" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <input type="text" visible="false" name="accSearch" class="field_input MyAutoCompleteClass"
                                                    style="width: 98%; font" id="accSearch" runat="server" />
                                                <asp:TextBox ID="txtGAccName" runat="server" CssClass="field_input" Width="220px"
                                                    Style="text-transform: uppercase;" onclick="javascript: this.select();"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtConAccName_AutoCompleteExtender" runat="server"
                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="10" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                    FirstRowSelected="false" MinimumPrefixLength="0" UseContextKey="True" ServiceMethod="GetGAcclist"
                                                    OnClientItemSelected="GAccautocompleteselectedControl" TargetControlID="txtGAccName"
                                                    ServicePath="">
                                                </asp:AutoCompleteExtender>
                                                <div style="padding-top: 5px">
                                                    <asp:TextBox ID="txtConAccName" runat="server" Width="220px" CssClass="field_input"
                                                        MaxLength="200"></asp:TextBox>
                                                </div>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="bottom" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <select style="width: 55px; display: none" id="ddlCostCode" class="field_input" runat="server">
                                                    <option selected="selected"></option>
                                                </select>
                                            </ItemTemplate>
                                            <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                            <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <select style="width: 60px; display: none" id="ddlCostName" class="field_input" runat="server">
                                                    <option selected="selected"></option>
                                                </select>
                                            </ItemTemplate>
                                            <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                            <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Narration">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <textarea style="width: 200px; height: 50px" id="txtgnarration" class="field_input"
                                                    maxlength="5000" runat="server"></textarea>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Currency">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input style="width: 40px" id="txtCurrency" class="field_input" type="text" maxlength="50"
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exchg. Rate">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <input id="txtConvRate" runat="server" class="field_input" maxlength="50" style="width: 45px"
                                                    type="text" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 70px; text-align: right" id="txtDebit" class="field_input" type="text"
                                                    maxlength="12" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 70px; text-align: right" id="txtCredit" class="field_input"
                                                    type="text" maxlength="12" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit(AED)">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 78px; text-align: right" id="txtBaseDebit" readonly="readonly"
                                                    class="field_input" type="text" maxlength="50" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit(AED)">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="width: 80px; text-align: right" id="txtBaseCredit" readonly="readonly"
                                                    class="field_input" type="text" maxlength="50" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Source Country">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSrcCtryCode" runat="server" onkeyup="SetContextKey()" Style="display: none"></asp:TextBox>
                                                <%--"--%>
                                                <asp:TextBox ID="txtSrcCtryName" runat="server" CssClass="field_input" Width="200px"
                                                    Style="text-transform: uppercase;" onclick="javascript: this.select();"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtSrcCtryName_AutoCompleteExtender" runat="server"
                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="10" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                    FirstRowSelected="false" MinimumPrefixLength="0" UseContextKey="True" ServiceMethod="GetSrcCtrylist"
                                                    TargetControlID="txtSrcCtryName" ServicePath="" OnClientItemSelected="SrcCtryautocompleteselectedControl">
                                                </asp:AutoCompleteExtender>
                                                <%-- <select style="width: 100px" id="ddldept" class="field_input" runat="server">
                                                        <option selected></option>
                                                    </select>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Adjust Bill">
                                            <ItemTemplate>
                                                <input style="width: 35px" id="btnAd" class="field_button" type="button" value="A.B"
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                            <ControlStyle BackColor="White" BorderColor="White" BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None" BorderColor="White"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtOldLineno" type="text" runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BackColor="White" BorderStyle="None" BorderColor="White"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="code" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                            <ControlStyle BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtctrolaccode" class="field_input" type="text"
                                                    maxlength="50" value=" " runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                            <EditItemTemplate>
                                                &nbsp;
                                            </EditItemTemplate>
                                            <ControlStyle BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None" Width="1px"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" Width="1px" BorderColor="White">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 44px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtlineno" class="field_input" readonly type="text"
                                                    maxlength="50" value='<%# Bind("LineNo") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                            <ControlStyle BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtcontrolacname" class="field_input" type="text"
                                                    maxlength="50" value=" " runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                            <ControlStyle BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtacctcode" class="field_input" type="text"
                                                    maxlength="50" value=" " runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ControlStyle BackColor="White" BorderStyle="None" />
                                            <ItemStyle BackColor="White" BorderStyle="None"></ItemStyle>
                                            <HeaderStyle BackColor="White" BorderStyle="None" BorderColor="White"></HeaderStyle>
                                            <ItemTemplate>
                                                <input style="visibility: hidden; border-bottom-color: #eeeeee; width: 1px; border-top-color: #eeeeee;
                                                    border-right-color: #eeeeee" id="txtacctname" class="field_input" type="text"
                                                    maxlength="50" value=" " runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle BorderStyle="None"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemStyle VerticalAlign="Top" CssClass="hiddencol"></ItemStyle>
                                            <HeaderStyle VerticalAlign="Top" CssClass="hiddencol" />
                                            <ItemTemplate>
                                                <input style="width: 75px; display: none" id="txtrequestid" class="field_input" type="text"
                                                    maxlength="20" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                    <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center">
                                    </PagerStyle>
                                    <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnAdd" TabIndex="17" runat="server" Text="Add Row" CssClass="field_button"
                                    Font-Bold="True"></asp:Button>&nbsp;
                                <asp:Button ID="btnDelLine" TabIndex="18" OnClick="btnDelLine_Click" runat="server"
                                    Text="DeleteRow" CssClass="field_button" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" style="font-weight: normal; font-family: 'Times New Roman';
                                padding-top: 15px; padding-left: 175px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Debit/Credit Total" CssClass="field_caption"
                                                Width="134px"></asp:Label>
                                        </td>
                                        <td>
                                            <input style="width: 80px; text-align: right" id="txtTotalDebit" class="field_input"
                                                readonly="readonly" type="text" maxlength="50" runat="server" tabindex="19" />
                                        </td>
                                        <td>
                                            <input style="width: 80px; text-align: right" id="txtTotalCredit" class="field_input"
                                                readonly="readonly" type="text" maxlength="50" runat="server" tabindex="20" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBaseTot" runat="server" CssClass="field_caption" Text="Base Total"
                                                Width="98px"></asp:Label>
                                        </td>
                                        <td>
                                            <input style="width: 80px; text-align: right" id="txtTotBaseDebit" class="field_input"
                                                readonly="readonly" type="text" maxlength="100" runat="server" tabindex="21" />
                                        </td>
                                        <td>
                                            <input style="width: 80px; text-align: right" id="txtTotBaseCredit" class="field_input"
                                                readonly="readonly" type="text" maxlength="100" runat="server" tabindex="22" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" CssClass="field_caption" Text=" Total" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <input style="width: 80px; text-align: right;" id="txtTotBaseDiff" class="field_input"
                                                readonly="readonly" type="text" maxlength="100" runat="server" tabindex="23" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp; &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr style="padding-top: 20px">
                            <td align="left" colspan="3" style="font-weight: normal; font-family: 'Times New Roman';
                                font-variant: normal">
                                &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3" style="font-weight: normal; font-family: 'Times New Roman';
                                font-variant: normal">
                                <input style="width: 29px; visibility: hidden;" id="txtAdjcolno" type="text" maxlength="20"
                                    runat="server" tabindex="24" />
                                <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                    height: 9px" type="text" />
                                <input style="visibility: hidden; width: 29px" id="txtOldAmount" type="text" maxlength="20"
                                    runat="server" tabindex="25" />
                                <input style="visibility: hidden; width: 29px" id="txtforex" type="text" maxlength="20"
                                    runat="server" tabindex="25" />
                                <input style="visibility: hidden; width: 29px" id="txtMode" type="text" maxlength="20"
                                    runat="server" tabindex="26" />
                                &nbsp;<input style="visibility: hidden; width: 29px" id="txtDivCode" class="field_input MyAutoCompletedivClass"
                                    type="text" tabindex="27" maxlength="20" runat="server" />
                                <input style="visibility: hidden; width: 29px" id="txtGridType" type="text" tabindex="28"
                                    maxlength="20" runat="server" />
                                &nbsp;&nbsp;<input style="visibility: hidden; width: 29px" id="txtTranType" tabindex="29"
                                    type="text" maxlength="20" runat="server" /><input style="visibility: hidden; width: 29px"
                                        id="txtbasecurr" tabindex="30" type="text" maxlength="20" runat="server" /><input
                                            style="visibility: hidden; width: 33px" id="txtdecimal" tabindex="31" type="text"
                                            maxlength="15" runat="server" /><asp:TextBox ID="txtpdate" runat="server" Visible="False"
                                                TabIndex="32" Width="1px"></asp:TextBox>
                                <input style="visibility: hidden; width: 137px" id="txtgridrows" tabindex="33" type="text"
                                    runat="server" />
                                &nbsp;<input id="chkBlank" runat="server" disabled="disabled" type="checkbox" tabindex="34"
                                    text="Allow Blank" visible="false" style="font-family: Arial,Verdana, Geneva, ms sans serif;
                                    font-size: 10pt; font-weight: normal;" />
                                &nbsp;<input id="chkPrntInclude" runat="server" tabindex="35" type="checkbox" visible="false" /><asp:Label
                                    ID="lblPrntInclude" runat="server" Text="Include 2nd page" CssClass="field_caption"
                                    Visible="false"></asp:Label>
                                &nbsp;
                                <asp:CheckBox ID="chkprclicurr" CssClass="field_caption" Text="Print in Client Currency Without GL A/C"
                                    runat="server" />
                                <asp:CheckBox ID="chkPost" TabIndex="36" runat="server" BackColor="#FFC0C0" Checked="true"
                                    CssClass="field_caption" Font-Bold="True" Font-Names="Verdana" Font-Size="10px"
                                    ForeColor="Black" Text="Post/UnPost" Width="103px" />
                                &nbsp;
                                <asp:Button ID="btnSave" TabIndex="37" runat="server" CssClass="field_button" Font-Bold="True"  OnClientClick="return ShowProgress();"
                                    OnClick="btnSave_Click" Text="Save" />
                                &nbsp;
                                <asp:Button ID="btnPrint" TabIndex="38" runat="server" CssClass="field_button" Text="Print" />
                                <asp:Button ID="PdfReport" TabIndex="38" runat="server" CssClass="field_button" Text="Pdf Report" />
                                <asp:Button ID="btnclientreceipt" TabIndex="39" runat="server" CssClass="field_button"
                                    Text="Client Receipt" />
                                &nbsp;
                                <asp:Button ID="btnExit" TabIndex="40" runat="server" CssClass="field_button" Font-Bold="True"
                                    OnClick="btnExit_Click" Text="Exit" />
                                &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" OnClick="btnhelp_Click"
                                    TabIndex="41" Text="Help" />
                                &nbsp; &nbsp;<asp:CheckBox ID="chkadjust" TabIndex="42" runat="server" Text="Allow any way"
                                    Visible="False" Width="121px" />
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                    TargetControlID="txtdate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                    ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                    TargetControlID="txtDate">
                                </cc1:MaskedEditExtender>
                                <cc1:CalendarExtender ID="ClExChequeDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImageButton1"
                                    TargetControlID="txtChequeDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
                                    ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                    TargetControlID="txtChequeDate">
                                </cc1:MaskedEditExtender>
                                <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                                    <Services>
                                        <asp:ServiceReference Path="~/clsServices.asmx" />
                                    </Services>
                                </asp:ScriptManagerProxy>
                                <asp:HiddenField ID="hdnss" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr visible="false">
                            <td colspan="2" style="height: 24px">
                                <select id="ddlRecveidfrom" runat="server" class="field_input" style="width: 172px;
                                    display: none;" tabindex="12">
                                </select>
                                <asp:Label ID="lblCurrCode" runat="server" CssClass="field_caption" Font-Strikeout="False"
                                    Text="Curr. code" Visible="false" Width="80px"></asp:Label>
                                <asp:Label ID="lblPostmsg" runat="server" BackColor="#E0E0E0" CssClass="field_caption"
                                    Font-Bold="True" Font-Names="Verdana" Font-Size="12px" ForeColor="Green" Text="UnPosted"
                                    Width="155px"></asp:Label>
                                <select id="ddlAccCode" runat="server" class="field_input" name="D1" style="width: 126px"
                                    tabindex="2" visible="false">
                                </select><select id="ddlAccName" runat="server" class="field_input" name="D2" style="width: 178px"
                                    tabindex="3" visible="false">
                                </select><select id="ddlCurrCode" runat="server" class="field_input" name="D3" style="width: 100px"
                                    tabindex="0" visible="false" visidisabled="disabled">
                                </select><input style="visibility: hidden; width: 168px" id="txtCurrCode" type="text"
                                    runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px">
                                &nbsp;
                            </td>
                            <td style="height: 22px">
                                &nbsp;
                            </td>
                            <td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Style="display: none" Text="M.RV" Width="80px"></asp:Label>
                                </td>
                                <td>
                                    <input id="txtMRV" style="display: none" class="field_input" tabindex="11" type="text"
                                        maxlength="50" runat="server" />
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input style="visibility: hidden;" id="txtAccCode" type="text" runat="server" />
                                &nbsp;
                                <input style="visibility: hidden;" id="txtAccName" type="text" runat="server" />
                                <asp:Button ID="hdnValidate" runat="server" OnClick="hdnValidate_Click" Style="display: none" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
          

            <asp:HiddenField ID="hdnConvRate" runat="server" />
                <center>
                <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                    <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
                    <h2 style="color: #06788B">
                        Processing please wait...</h2>
                </div>
            </center>

                <cc1:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
                TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
                BackgroundCssClass="ModalPopupBG">
            </cc1:ModalPopupExtender>
            <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
            <input id="btnCloseLoading" type="button" value="Cancel" style="display: none" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
