<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ManualInvoiceFreeForm.aspx.vb" Inherits="AccountsModule_SalesInvoiceFreeForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="ews" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


  <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
 
<style type="text/css">
     .displaynonemhd 
     {
         display:none;
         }
</style>

 <script type="text/javascript" charset="utf-8">
     $(document).ready(function () {

         custAutoCompleteExtenderKeyUp();
         salesAutoCompleteExtenderKeyUp();
         sourcectryAutoCompleteExtenderKeyUp();


     });

        </script>




<script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    //prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);



    function EndRequestUserControl(sender, args) {

        salesAutoCompleteExtenderKeyUp();
        sourcectryAutoCompleteExtenderKeyUp();
        custAutoCompleteExtenderKeyUp();

        var curr12;
        curr12=document.getElementById("<%=txtcurr.ClientID%>").value;

        if (curr12 != '') {
            var objGridView = document.getElementById('<%=grdServiceInvoice.ClientID%>');
            objGridView.rows[0].cells[9].innerHTML = "Value(" + curr12  + ")";
        }

        // after update occur on UpdatePanel re-init the Autocomplete a

    }
</script>



<script language="javascript" type="text/javascript">


    function txtsourcectry_AutoCompleteExtender1_OnClientPopulating(sender, args) {


        sender.set_contextKey(document.getElementById('<%=TxtCustCode.ClientID%>').value);



    }

    //Tanvir 08112023 
    function ValidatePage() {
        var txtbasetaxableamttot = document.getElementById("<%=txtbasetaxableamttot.ClientID%>");
        var txtBasenontaxableamttot = document.getElementById("<%=txtBasenontaxableamttot.ClientID%>");

        var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");

        if ((txtbasetaxableamttot.value == '' && txtBasenontaxableamttot.value == '') || (txtbasetaxableamttot.value == '0' && txtBasenontaxableamttot.value == '0')) {

            return true;


        }

        hdnSS.value = 0;
        validate_click();
    }

    function validate_click() {
        var hdnSS = document.getElementById("<%=hdnSS.ClientID%>");
        var btnss = document.getElementById("<%=btnsave.ClientID%>");

        if (hdnSS.value == 0) {
            hdnSS.value = 1;
            // btnss.disabled=true;
            btnss.style.visibility = "hidden";
            return true;
        }
        else {
            return false;
        }
    }
    //Tanvir 08112023 

    function DecRound(amtToRound) {

        var txtdec = document.getElementById("<%=txtdecimal.ClientID%>");
        nodecround = Math.pow(10, parseInt(txtdec.value));

        var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
        // var rdamt = Math.round(parseFloat(Number(amtToRound)) * nodecround) / nodecround;
        return parseFloat(rdamt);
    }


    function FormValidation(state) {

        var dvType = document.getElementById('<%=ddltype.ClientID%>');

        //        if (document.getElementById("<%=txtsales.ClientID%>").value == '') {
        //            document.getElementById("<%=txtsales.ClientID%>").focus();
        //            alert("Select Sales Man ");
        //            return false;
        //        }
        if (document.getElementById("<%=txtsourcectry.ClientID%>").value == '') {
            document.getElementById("<%=txtsourcectry.ClientID%>").focus();
            alert("Select Sourcecountry ");
            return false;
        }
        else if (document.getElementById("<%=TxtcustName.ClientID%>").value == '') {
            document.getElementById("<%=TxtcustName.ClientID%>").focus();
            alert("Select " + dvType.value);
            return false;
        }
        else if (document.getElementById("<%=TxtCustCode.ClientID%>").value == '') {
            document.getElementById("<%=TxtCustCode.ClientID%>").focus();
            alert(+dvType.value + " customer cannot be blank ");
            return false;
        }
        else if (document.getElementById("<%=txtcurr.ClientID%>").value == '') {
            document.getElementById("<%=txtcurr.ClientID%>").focus();
            alert(+dvType.value + " currency cannot be blank ");
            return false;
        }
        else if (document.getElementById("<%=txtcontrolac%>").value == '') {
            document.getElementById("<%=txtcontrolac%>").focus();
            alert(+dvType.value + " Control A/c cannot be blank ");
            return false;
        }

        else if (Number(document.getElementById("<%=txtconvrate.ClientID%>").value) == 0) {
            document.getElementById("<%=txtconvrate.ClientID%>").focus();
            alert("Conversion Rate can not be 0");
            return false;
        }

        else if (Number(document.getElementById("<%=txtsalevaltot.ClientID%>").value) == 0) {
            document.getElementById("<%=txtsalevaltot.ClientID%>").focus();
            alert("Subtotal value cannot be 0");
            return false;
        }
        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
            if (state == 'Cancel') { if (confirm('Are you sure you want to Cancel ?') == false) return false; }
            if (state == 'UndoCancel') { if (confirm('Are you sure you want to UndoCancel ?') == false) return false; }

        }
    }



    function supautocompleteselected(source, eventArgs) {
        var contxt = '';
  
        if (eventArgs != null) {

            document.getElementById('<%=txtsuppcode.ClientID%>').value = eventArgs.get_value();

            contxt = document.getElementById('<%=txtsuppcode.ClientID%>').value;



            var sqlstr = "Select partymast.currcode,currmast.currname,partymast.controlacctcode,acctmast.acctname,'' spersoncode, trnno,partymast.ctrycode,ctrymast.ctryname,isnull(crdays,0) crdays ,currrates.convrate  convrate ,'',partymast.partycode,'S',partymast.hotelalias" +
                           " from partymast left join currmast on partymast.currcode= currmast.currcode" +
                            " left join  acctmast on partymast.controlacctcode=acctmast.acctcode " +
                            " left join ctrymast on partymast.ctrycode=ctrymast.ctrycode " +
                            " left join currrates on partymast.currcode=currrates.currcode  where partycode='" + document.getElementById('<%=txtsuppcode.ClientID%>').value + "' and currrates.tocurr='" + document.getElementById("<%=txtbase.ClientID%>").value + "' and acctmast.div_code='" + document.getElementById("<%=txtdiv.ClientID%>").value + "' "

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, "14", fnFillsupp, ErrorHandler, TimeOutHandler);
            
            
        }
        else {
            document.getElementById('<%=txtsuppcode.ClientID%>').value = '';
            contxt = '';
        }

    
    }



















    function custautocompleteselected(source, eventArgs) {
        var contxt = '';
        if (eventArgs != null) {

            document.getElementById('<%=Txtcustcode.ClientID%>').value = eventArgs.get_value();

            contxt = document.getElementById('<%=Txtcustcode.ClientID%>').value;
            if (document.getElementById('<%=ddltype.ClientID%>').value == "Customer") {
                var sqlstr = "Select agentmast.currcode,currmast.currname,agentmast.controlacctcode,acctmast.acctname,spersoncode,trnno,agentmast.ctrycode,ctrymast.ctryname,isnull(crdays,0) crdays ,currrates.convrate  convrate,usermaster.username,agentmast.agentcode,'C'" +
                                " from agentmast left join currmast on agentmast.currcode= currmast.currcode" +
                                " left join  acctmast on agentmast.controlacctcode=acctmast.acctcode and agentmast.divcode=acctmast.div_code" +
                                " left join ctrymast on agentmast.ctrycode=ctrymast.ctrycode " +
                                " left join currrates on agentmast.currcode=currrates.currcode left join usermaster on agentmast.spersoncode=usermaster.usercode where agentcode='" + document.getElementById('<%=Txtcustcode.ClientID%>').value + "' and agentmast.divcode='" + document.getElementById("<%=txtdiv.ClientID%>").value + "' and currrates.tocurr='" + document.getElementById("<%=txtbase.ClientID%>").value + "' ";
                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, "13", fnFillagent, ErrorHandler, TimeOutHandler);

            }

            else {


                var sqlstr = "Select partymast.currcode,currmast.currname,partymast.controlacctcode,acctmast.acctname,'' spersoncode, trnno,partymast.ctrycode,ctrymast.ctryname,isnull(crdays,0) crdays ,currrates.convrate  convrate ,'',partymast.partycode,'S'" +
                           " from partymast left join currmast on partymast.currcode= currmast.currcode" +
                            " left join  acctmast on partymast.controlacctcode=acctmast.acctcode " +
                            " left join ctrymast on partymast.ctrycode=ctrymast.ctrycode " +
                            " left join currrates on partymast.currcode=currrates.currcode  where partycode='" + document.getElementById('<%=Txtcustcode.ClientID%>').value + "' and currrates.tocurr='" + document.getElementById("<%=txtbase.ClientID%>").value + "' and acctmast.div_code='" + document.getElementById("<%=txtdiv.ClientID%>").value + "' "

                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
                constr = connstr.value
                ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, "13", fnFillagent, ErrorHandler, TimeOutHandler);

            }
        }
        else {
            document.getElementById('<%=Txtcustcode.ClientID%>').value = '';
            contxt = '';
        }

        $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(contxt);
    }
    function fnFillcustomer(result) {
        if (result[0] != 1) {
            document.getElementById('<%=txtsourcectry.ClientID%>').value = '';
            document.getElementById('<%=txtsourcecode.ClientID%>').value = '';

        }
    }






    function fnFillsupp(result) {

      
            document.getElementById('<%=txtPIxtrlacname.ClientID%>').value = result[3];
            document.getElementById('<%=txtPIxtrlaccode.ClientID%>').value = result[2];

     
        document.getElementById('<%=txtsuppconvrt.ClientID%>').value = DecRound(result[9]);
        document.getElementById('<%=txtsupptrnno.ClientID%>').value = result[5];
        document.getElementById('<%=txtPIcurrcurrcode.ClientID%>').value = result[0];
        document.getElementById('<%=txtPIcurrcurrname.ClientID%>').value = result[1];
       

        document.getElementById('<%=txtsalesPI.ClientID%>').value = result[10];
        document.getElementById('<%=txtsalesPIcode.ClientID%>').value = result[4];


        document.getElementById('<%=txtsuppcode.ClientID%>').value = result[11];

        document.getElementById('<%=txtsuppalias.ClientID%>').value = result[13];



    
        


    }



















    function fnFillagent(result) {

//        if (result[12] == 'C') {

//            var sqlstr = "select count(agentcode) From agentmast_countries where agentcode='" + document.getElementById('<%=Txtcustcode.ClientID%>').value + "' group by agentcode"

//            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//            constr = connstr.value
//            ColServices.clsServices.GetQueryReturnStringArraynew(constr, sqlstr, "1", fnFillcustomer, ErrorHandler, TimeOutHandler);
//            
//        }

        if (result[3] == '') {
            document.getElementById('<%=txtcontrolacname.ClientID%>').value = result[2];

        }
        else {
            document.getElementById('<%=txtcontrolacname.ClientID%>').value = result[3];

        }

        document.getElementById('<%=txtcontrolac.ClientID%>').value = result[2];
        document.getElementById('<%=txtcurr.ClientID%>').value = result[0];
        document.getElementById('<%=hdnCurrr.ClientID%>').value = result[0];
        document.getElementById('<%=txtcurrname.ClientID%>').value = result[1];

        document.getElementById('<%=txtsourcectry.ClientID%>').value = result[7];
        document.getElementById('<%=txtsourcecode.ClientID%>').value = result[6];

       




        document.getElementById('<%=txttrnno.ClientID%>').value = result[5];
        document.getElementById('<%=txtsales.ClientID%>').value = result[10];
        document.getElementById('<%=txtsalescode.ClientID%>').value = result[4];
        document.getElementById('<%=txtconvrate.ClientID%>').value = result[9];
        document.getElementById('<%=lblsubtotal_curr.ClientID%>').innerHTML = "SubTotal(" + result[0] + ")";

        var objGridView = document.getElementById('<%=grdServiceInvoice.ClientID%>');
        objGridView.rows[0].cells[9].innerHTML = "Value(" + result[0] + ")";

        document.getElementById('<%=Txtcustcode.ClientID%>').value = result[11];

        if (document.getElementById('<%=ddltype.ClientID%>').value == "Customer") {
            typ = "C";
        }
        else {
            typ = "S";
        }

        var crdays = result[8];
        var strdate = document.getElementById("<%=txtJDate.ClientID%>").value;

        sqlstr = "select isnull(crdays,0) from view_account where div_code='" + document.getElementById("<%=txtdiv.ClientID%>").value + "' and Code = '" + document.getElementById('<%=Txtcustcode.ClientID%>').value + "' and type= '" + typ + "'";

        ColServices.clsServices.GetQueryReturnDatenew(constr, sqlstr, strdate, FillDueDate, ErrorHandler, TimeOutHandler);

        grdTotal();


        //document.getElementById('<%=btnRefreshPageOnAccoutSelection.ClientID%>').click();
        
    }
    function assignGridValueColumn(currvalue) {
        var objGridView = document.getElementById('<%=grdServiceInvoice.ClientID%>');
        objGridView.rows[0].cells[9].innerHTML = "Value(" + currvalue + ")";
    }

    function FillDueDate(result) {

        var txtddate = document.getElementById("<%=txtTDate.ClientID%>");

        txtddate.value = result;

    }

    function custAutoCompleteExtenderKeyUp() {
        //            $("#<%=Txtcustname.ClientID %>").bind("change", function () {

        //                document.getElementById('<%=Txtcustcode.ClientID%>').value = '';
        ////                $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(document.getElementById('<%=Txtcustcode.ClientID%>').value);
        //            });


        //            $("#<%= Txtcustname.ClientID %>").keyup(function (event) {

        //                if (document.getElementById('<%=Txtcustname.ClientID%>').value == '') {

        //                    document.getElementById('<%=Txtcustcode.ClientID%>').value = '';
        ////                    $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(document.getElementById('<%=Txtcustcode.ClientID%>').value);
        //                }

        //            });

    }


    function salesAutoCompleteExtenderKeyUp() {
        $("#<%=txtsales.ClientID %>").bind("change", function () {

            document.getElementById('<%=txtsalescode.ClientID%>').value = '';

        });


        $("#<%= txtsales.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtsales.ClientID%>').value == '') {

                document.getElementById('<%=txtsalescode.ClientID%>').value = '';
            }

        });

    }
    function sourcecountryautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {

            document.getElementById('<%=txtsourcecode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtsourcecode.ClientID%>').value = '';
        }

        $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(document.getElementById('<%=Txtcustcode.ClientID%>').value);
    }

    function salesautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {

            document.getElementById('<%=txtsalescode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtsalescode.ClientID%>').value = '';
        }


    }

    function salesPIautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {

            document.getElementById('<%=txtsalesPIcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtsalesPIcode.ClientID%>').value = '';
        }


    }

    function acctPIautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {

            document.getElementById('<%=txtsuppacctCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
           document.getElementById('<%=txtsuppacctCode.ClientID%>').value = '';
        }


   }


   function acctcodePIautocompleteselected(source, eventArgs) {
       if (eventArgs != null) {

           document.getElementById('<%=txtsuppacct.ClientID%>').value = eventArgs.get_value();
       }
       else {
           document.getElementById('<%=txtsuppacct.ClientID%>').value = '';
       }


   }

   function PIcostcenterautocompleteselected(source, eventArgs) {
       if (eventArgs != null) {

           document.getElementById('<%=txtPIcostcentercode.ClientID%>').value = eventArgs.get_value();
       }
       else {
           document.getElementById('<%=txtPIcostcentercode.ClientID%>').value = '';
       }


   }
   function PIcostcentercodeautocompleteselected(source, eventArgs) {
       if (eventArgs != null) {

           document.getElementById('<%=txtPIcostcenter.ClientID%>').value = eventArgs.get_value();
       }
       else {
           document.getElementById('<%=txtPIcostcenter.ClientID%>').value = '';
       }


   }
    function sourcectryAutoCompleteExtenderKeyUp() {
        //            $("#<%=txtsourcectry.ClientID %>").bind("change", function () {

        //                document.getElementById('<%=txtsourcecode.ClientID%>').value = '';
        //                
        //                $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(document.getElementById('<%=Txtcustcode.ClientID%>').value);
        //            });


        //            $("#<%= txtsourcectry.ClientID %>").keyup(function (event) {

        //                if (document.getElementById('<%=txtsourcectry.ClientID%>').value == '') {

        //                    document.getElementById('<%=txtsourcecode.ClientID%>').value = '';
        //                    $find('<%=txtsourcectry_AutoCompleteExtender1.ClientID%>').set_contextKey(document.getElementById('<%=Txtcustcode.ClientID%>').value);

        //                }

        //            });

    }


    function acctautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("txtacct_AutoCompleteExtender", "txtacctCode");

        if (eventArgs != null) {
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        else {
            $get(hiddenfieldID).value = '';

        }

    }



    function salescodeacctautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("txtacctcode_AutoCompleteExtender", "txtacct");

        if (eventArgs != null) {
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        else {
            $get(hiddenfieldID).value = '';

        }

    }


    function salesnameacctautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("txtacct_AutoCompleteExtender", "txtacctCode");

        if (eventArgs != null) {
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        else {
            $get(hiddenfieldID).value = '';

        }

    }



    function costcenterautocompleteselected(source, eventArgs) {

        var hiddenfieldID = source.get_id().replace("txtcostcenter_AutoCompleteExtender", "txtcostcentercode");
        $get(hiddenfieldID).value = eventArgs.get_value();




        if (eventArgs != null) {
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        else {
            $get(hiddenfieldID).value = '';

        }

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




    function Calculate_GridvaluesPurchase(ddlvatType, txtvatperc, txtnontaxableamt, txttaxableamt, txtbasevalue, txtvalue, txtvatamount) {

        ddltyp = document.getElementById(ddlvatType);


        var strtp = ddltyp.value;

        vatperc = document.getElementById(txtvatperc);
        txtnontaxable = document.getElementById(txtnontaxableamt);
        txttaxable = document.getElementById(txttaxableamt);
        txtbase = document.getElementById(txtbasevalue);
        txtsaleval = document.getElementById(txtvalue);
        txtvatamt = document.getElementById(txtvatamount);
        var exchangerate = document.getElementById('<%=txtsuppconvrt.ClientID%>').value;
        var strvatdef = document.getElementById('<%=txtvatpercentage.ClientID%>').value;

        txtvatamt.disabled = true;
        var taxableval = txttaxable.value;
        var nontaxable = txtnontaxable.value;


        if (strtp == 'Taxable') {

            vatperc.disabled = false;
            if (vatperc.value == '') {
                vatperc.value = strvatdef;
            }

            txtvatamt.value = (txttaxable.value * (vatperc.value / 100));
            txtvatamt.value = DecRound(txtvatamt.value);
            var vatamt = txtvatamt.value;

            txtsaleval.value = Number(taxableval) + Number(vatamt) + Number(nontaxable);
            txtsaleval.value = DecRound(txtsaleval.value);
            var saleval = txtsaleval.value;



            txtbase.value = saleval * Number(exchangerate);

            txtbase.value = DecRound(txtbase.value);

        }
        else {
            vatperc.disabled = true;
            vatperc.value = "";
            txtvatamt.value = "";



            txtsaleval.value = Number(taxableval) + Number(nontaxable);
            txtsaleval.value = DecRound(txtsaleval.value);
            var saleval = txtsaleval.value;
            txtbase.value = saleval * Number(exchangerate);

            txtbase.value = DecRound(txtbase.value);

        }


    }

    function Calculate_Gridvalues(ddlvatType, txtvatperc, txtnontaxableamt, txttaxableamt, txtbasevalue, txtvalue, txtvatamount) {

        ddltyp = document.getElementById(ddlvatType);


        var strtp = ddltyp.value;

        vatperc = document.getElementById(txtvatperc);
        txtnontaxable = document.getElementById(txtnontaxableamt);
        txttaxable = document.getElementById(txttaxableamt);
        txtbase = document.getElementById(txtbasevalue);
        txtsaleval = document.getElementById(txtvalue);
        txtvatamt = document.getElementById(txtvatamount);
        var exchangerate = document.getElementById('<%=txtconvrate.ClientID%>').value;
        var strvatdef = document.getElementById('<%=txtvatpercentage.ClientID%>').value;

        txtvatamt.disabled = true;
        var taxableval = txttaxable.value;
        var nontaxable = txtnontaxable.value;


        if (strtp == 'Taxable') {

            vatperc.disabled = false;
            if (vatperc.value == '') {
                vatperc.value = strvatdef;
            }

            txtvatamt.value = (txttaxable.value * (vatperc.value / 100));
            txtvatamt.value = DecRound(txtvatamt.value);
            var vatamt = txtvatamt.value;

            txtsaleval.value = Number(taxableval) + Number(vatamt) + Number(nontaxable);
            txtsaleval.value = DecRound(txtsaleval.value);
            var saleval = txtsaleval.value;



            txtbase.value = saleval * Number(exchangerate);

            txtbase.value = DecRound(txtbase.value);

        }
        else {
            vatperc.disabled = true;
            vatperc.value = "";
            txtvatamt.value = "";



            txtsaleval.value = Number(taxableval) + Number(nontaxable);
            txtsaleval.value = DecRound(txtsaleval.value);
            var saleval = txtsaleval.value;
            txtbase.value = saleval * Number(exchangerate);

            txtbase.value = DecRound(txtbase.value);

        }


    }


    function displaynarration_default(txtparticulars) {

        if (txtparticulars.value == '') {
            txtparticulars.value = document.getElementById('<%=txtnarration.ClientID%>').value;
        }
    }

    function grdTotal() {



        //            alert(objGridView.rows[j].cells[4].children[1].value);
        //            alert(objGridView.rows[j].cells[5].children[2].value);
        //            alert(objGridView.rows[j].cells[6].children[0].value);

        var objGridView = document.getElementById('<%=grdServiceInvoice.ClientID%>');

        var TAtot = 0;
        var NTAtot = 0;
        var VATAmttot = 0;
        var Saletot = 0;
        var txtrowcnt = document.getElementById('<%=txtgridrows.ClientID%>');

        intRows = txtrowcnt.value;

        for (j = 1; j <= intRows; j++) {
            var Cell = objGridView.rows[j].cells[7].getElementsByTagName("input");  //3
            var TAval = Cell[1].value;
            var NTAval = Cell[0].value;
            var Cell = objGridView.rows[j].cells[8].getElementsByTagName("input"); //4
            var VATamtval = Cell[1].value;

            var saleval = objGridView.rows[j].cells[9].children[0].value; //fixed //5



            if (TAval == '') { TAval = 0; }
            if (NTAval == '') { NTAval = 0; }
            if (VATamtval == '') { VATamtval = 0; }
            if (saleval == '') { saleval = 0; }
            if (objGridView.rows[j].cells[9].children[1].value == '') { objGridView.rows[j].cells[9].children[1].value = 0; } //6

            if (isNaN(TAval) == true) { TAval = 0; }
            if (isNaN(NTAval) == true) { NTAval = 0; }
            if (isNaN(VATamtval) == true) { VATamtval = 0; }
            if (isNaN(saleval) == true) { saleval = 0; }
            if (isNaN(objGridView.rows[j].cells[9].children[1].value) == true) { objGridView.rows[j].cells[9].children[1].value = 0; } //6
            var exchangerate1 = document.getElementById('<%=txtconvrate.ClientID%>');
            if (exchangerate1.val == '') { exchangerate1.val = 0; }
            if (isNaN(exchangerate1.val) == true) { exchangerate1.val1 = 0; }

            objGridView.rows[j].cells[9].children[1].value = parseFloat(saleval) * parseFloat(exchangerate1.value); //6
            objGridView.rows[j].cells[9].children[1].value = DecRound(objGridView.rows[j].cells[9].children[1].value); //6
            TAtot = parseFloat(TAtot) + parseFloat(TAval);
            NTAtot = parseFloat(NTAtot) + parseFloat(NTAval);
            VATAmttot = parseFloat(VATAmttot) + parseFloat(VATamtval);
            Saletot = parseFloat(Saletot) + parseFloat(saleval);
        }
        var txttaxtot = document.getElementById('<%=txttaxableamttot.ClientID%>');
        var txtnontaxtot = document.getElementById('<%=txtnontaxableamttot.ClientID%>');
        var txtvatamttot = document.getElementById('<%=txtvatamttot.ClientID%>');
        var txtsalevaltot = document.getElementById('<%=txtsalevaltot.ClientID%>');

        txttaxtot.value = TAtot;
        txtnontaxtot.value = NTAtot;
        txtvatamttot.value = VATAmttot;
        txtsalevaltot.value = Saletot;

        txttaxtot.value = DecRound(txttaxtot.value);
        txtnontaxtot.value = DecRound(txtnontaxtot.value);
        txtvatamttot.value = DecRound(txtvatamttot.value);
        txtsalevaltot.value = DecRound(txtsalevaltot.value);




        var txtbasetaxtot = document.getElementById('<%=txtbasetaxableamttot.ClientID%>');
        var txtbasenontaxtot = document.getElementById('<%=txtBasenontaxableamttot.ClientID%>');
        var txtbasevatamttot = document.getElementById('<%=txtBasevatamttot.ClientID%>');
        var txtbasesalevaltot = document.getElementById('<%=txtBasesalevaltot.ClientID%>');



        //dgddg

        var exchangerate = document.getElementById('<%=txtconvrate.ClientID%>');
        if (exchangerate.val == '') { exchangerate.val = 0; }
        if (isNaN(exchangerate.val) == true) { exchangerate.val = 0; }


        txtbasetaxtot.value = parseFloat(TAtot) * parseFloat(exchangerate.value);
        txtbasenontaxtot.value = parseFloat(NTAtot) * parseFloat(exchangerate.value);
        txtbasevatamttot.value = parseFloat(VATAmttot) * parseFloat(exchangerate.value);
        txtbasesalevaltot.value = parseFloat(Saletot) * parseFloat(exchangerate.value);



        txtbasetaxtot.value = DecRound(txtbasetaxtot.value);
        txtbasenontaxtot.value = DecRound(txtbasenontaxtot.value);
        txtbasevatamttot.value = DecRound(txtbasevatamttot.value);
        txtbasesalevaltot.value = DecRound(txtbasesalevaltot.value);




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
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="1" style="height: 27px">
                            <asp:Label ID="lblHeading" runat="server" Text="Manual Invoice Free Form" Width="100%"  ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td> 
                            <table style="height: 108px">
                                <tbody>
                                    <tr>
                                        <td class="td_cell" colspan="6">
                                            <table>
                                                <tr>
                                                    <td style="width: 82px">
                                                        <asp:Label ID="lblInvNo" runat="server" Text="Invoice No." Width="84px" 
                                                            CssClass="field_Caption"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDocNo" TabIndex="1" runat="server" Width="120px" CssClass="field_Caption"
                                                            ReadOnly="True" Enabled="False" Style="margin-left: 0px" Height="22px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp
                                                        <asp:Label ID="LblInvoiceDate" runat="server" Text="Invoice Date" Width="92px" CssClass="field_Caption"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtJDate" TabIndex="2" runat="server" CssClass="fiel_input" Width="80px"
                                                            ValidationGroup="MKE"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnFrmDt" TabIndex="2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" CssClass="field_error" Width="23px"
                                                            ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                                            ControlToValidate="txtJDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator>
                                                        <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtJDate"
                                                            MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                            DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtJDate"
                                                            PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender>



                                                            

                                                    </td>
                                                    <td>
                                                        &nbsp&nbsp
                                                        <asp:Label ID="lblInvtype" runat="server" Text="Invoice Type" Width="95px" CssClass="field_Caption"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlinvoicetype" TabIndex="3" runat="server">
                                                            <asp:ListItem>Tax Invoice</asp:ListItem>
                                                            <asp:ListItem>Commercial Invoice</asp:ListItem>
                                                            <asp:ListItem>Select</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                       
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <table>
                                                <tr>
                                                    <td class="td_cell" style="width: 82px">
                                                         <asp:Label ID="lbltype" runat="server" CssClass="field_Caption" Height="16px" Text="Type"
                                                           ></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddltype" runat="server" TabIndex="4" AutoPostBack="True" Width="121px">
                                                            <asp:ListItem>Customer</asp:ListItem>
                                                            <asp:ListItem>Supplier</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="td_cell" style="width: 105px; text-align: center;">
                                                    <asp:Label ID="lblduedate" runat="server" Text="Due Date"  CssClass="field_Caption"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td class="td_cell" style="width: 267px">
                                                        <asp:TextBox ID="txtTDate" TabIndex="5" runat="server" CssClass="field_input" Width="80px"
                                                            ValidationGroup="MKE"></asp:TextBox>
                                                        <asp:ImageButton ID="ImageButton1" TabIndex="4" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MskVToDt" runat="server" CssClass="field_error" Width="23px"
                                                            ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate1" EmptyValueMessage="Date is required"
                                                            EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtTdate" ControlExtender="MskChequeDate"></cc1:MaskedEditValidator>
                                                        <cc1:CalendarExtender ID="ClExChequeDate" runat="server" TargetControlID="txtTdate"
                                                            PopupButtonID="ImageButton1" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" TargetControlID="txtTdate"
                                                            MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                            DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                                <tr><td style="width: 82px;"><asp:TextBox ID="txtvatpercentage"  runat="server" 
                                                             Style="margin-left: 0px;display:none"></asp:TextBox></td>
                                                              <td> <INPUT style="VISIBILITY: hidden; WIDTH: 5px" id="txtgridrows" type=text maxLength=15 runat="server" /> </td>
                                                             <td style="width: 105px">
                                                                 <input id="txtconnection" runat="server" style="visibility: hidden; width: 27px" type="text" /></td>
                                                             <td style="width: 267px"><asp:TextBox ID="txtpdate" runat="server" Visible="False" Width="16px"></asp:TextBox></td>
                                                            <td> <INPUT style="VISIBILITY: hidden; WIDTH: 29px" id="txtAdjcolno" type=text maxLength=20 runat="server" />
                                                             </td><td><INPUT style="VISIBILITY: hidden; WIDTH: 29px" id="txtbase" type=text maxLength=20 runat="server" /></td></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" class="td_cell">
                                            <asp:Panel  ID="PnlType" runat="server"  GroupingText="Customer" TabIndex="6" CssClass="field_Caption" Style="margin-bottom: 0px">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_cell" style="width: 93px; height: 32px;">
                                                                <asp:Label ID="lblcust" runat="server" Text="Customer" Width="109px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td style="height: 32px">
                                                                <asp:TextBox ID="TxtcustName" runat="server" TabIndex="7" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="200px" ></asp:TextBox>
                                                                <asp:TextBox ID="TxtCustCode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />
                                                                <asp:AutoCompleteExtender ID="TxtCustName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getcustlist" TargetControlID="TxtCustName" OnClientItemSelected="custautocompleteselected">
                                                                   
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td style="width: 93px; height: 32px;">
                                                                <asp:Label ID="lblcurr" runat="server" Text="Currency" Width="66px" 
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td style="height: 32px"> <asp:TextBox ID="txtcurrname" runat="server" Width="120px" CssClass="field_input"
                                                                  Enabled="False"></asp:TextBox>
                                                                <asp:TextBox ID="txtcurr"  runat="server" Width="120px" CssClass="field_input"
                                                                     style="display:none" Enabled="False"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnCurrr"  runat="server" />
                                                            </td>
                                                            <td style="height: 32px">
                                                                <asp:Label ID="lblcontrolac" runat="server" CssClass="field_Caption" Text="Control A/c "></asp:Label>
                                                            </td>
                                                            <td style="height: 32px">

                                                              <asp:TextBox ID="txtcontrolacname" runat="server" CssClass="field_input" Enabled="False"
                                                                   TabIndex="1" Width="270px"></asp:TextBox>


                                                              
 </td>
                                                              <td style="height: 32px">
                                                                <asp:TextBox ID="txtcontrolac" runat="server" CssClass="field_input" Enabled="False"
                                                                    style="VISIBILITY: hidden" TabIndex="1" Width="86px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsalesman" runat="server" CssClass="field_Caption" Text="Salesman "></asp:Label>
                                                            </td>
                                                            <td>
                                                                
<asp:TextBox ID="txtsales" runat="server" CssClass="field_input" Enabled="True" 
                                                                    TabIndex="8" Width="200px"></asp:TextBox>

                                                                    <asp:TextBox ID="txtsalescode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtsales_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getsalesuserlist" TargetControlID="txtsales" OnClientItemSelected="salesautocompleteselected">
                                                                </asp:AutoCompleteExtender>

                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblconvrrate" runat="server" Text="Conversion Rate" Width="96px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtconvrate"  runat="server" Width="120px" CssClass="field_input"
                                                                     Enabled="False"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbltrnno" runat="server" Text="TRNNo" Width="65px"
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txttrnno" runat="server" Width="269px" CssClass="field_input"
                                                                   Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblsourcectry" runat="server" Text="Source Country" Width="91px"
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>
                                                                
<asp:TextBox ID="txtsourcectry" runat="server" CssClass="field_input" Enabled="True"
                                                               TabIndex="9" Width="200px"></asp:TextBox>
                                                                     <asp:TextBox ID="txtsourcecode" Style="display: none" runat="server" ></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField4" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtsourcectry_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getctrylist" ContextKey ="True"  OnClientPopulating="txtsourcectry_AutoCompleteExtender1_OnClientPopulating" TargetControlID="txtsourcectry" OnClientItemSelected="sourcecountryautocompleteselected">
                                                                </asp:AutoCompleteExtender>

                                                            </td>
                                                             <td>
                                                               
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                             <td>
                                                                <asp:Label ID="lblGuestName" runat="server" Text="Guest Name" Width="65px"
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGuestName" runat="server" Width="269px" CssClass="field_input"
                                                                   ></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="width: 103px"  class="td_cell">
                                                        <asp:Label ID="lblreferno" runat="server" Text="Reference No " Width="113px" CssClass="field_Caption"></asp:Label>
                                                    </td>
                                                    <td style="width: 141px">
                                                        <asp:TextBox ID="txtrefno" TabIndex="10" runat="server" Width="203px" CssClass="field_input"
                                                            Enabled="True"></asp:TextBox>
                                                    </td>
                                                    <td  class="td_cell">
                                                        <asp:Label ID="lblbookingno" runat="server" Text="Booking No " Width="81px" CssClass="field_Caption"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtbookingno" TabIndex="11" runat="server" Width="179px" CssClass="field_input"
                                                            Enabled="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 103px"  class="td_cell">
                                                        <asp:Label ID="lblnarration" runat="server" CssClass="field_Caption" Text="Narration"
                                                            Width="98px"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtnarration" TextMode="MultiLine" runat="server" CssClass="field_input"
                                                            Enabled="True" Height="80px" TabIndex="12" Width="568px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                     <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdiv" type=text maxLength=15 runat="server" />
                                                             <INPUT style="VISIBILITY: hidden; WIDTH: 1px" id="txtdecimal" type=text maxLength=15 runat="server" />
                                                          
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
               
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Label ID="lblPostmsg" runat="server" Text="UnPosted" Font-Size="12px" Font-Names="Verdana"
                                Width="155px" CssClass="field_caption" ForeColor="Green" Font-Bold="True" BackColor="#E0E0E0"></asp:Label>&nbsp;<asp:Label
                                    ID="lblcrdrCaption" runat="server" Width="192px" CssClass="field_caption"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="td_cell">

                            <asp:Button ID="btnRefreshPageOnAccoutSelection" runat="server" style="display:none" />
                                <asp:GridView ID="grdServiceInvoice" TabIndex="13" runat="server" Font-Size="10px"
                                    Width="100%" CssClass="td_cell" BorderStyle="None" BorderColor="#999999" BackColor="White"
                                    AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3" BorderWidth="1px">
                                    <FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
                                    <Columns>
                                        <asp:BoundField DataField="SrNo" Visible="False" HeaderText="LineNo"></asp:BoundField>
                                        <asp:TemplateField Visible="False" HeaderText="LineID">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="HTextBox1" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineID" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField Visible="False" HeaderText="PIFields">                                         
                                            <ItemTemplate>
                                            <asp:Label ID="PIFlag" runat="server" Style="display: none" ></asp:Label>
                                                <asp:Label ID="PIdate" runat="server" Style="display: none" ></asp:Label>
                                                  <asp:Label ID="PIType" runat="server" Style="display: none" ></asp:Label>
                                                   <asp:Label ID="PIsuprefno" runat="server" Style="display: none" ></asp:Label>
                                                    <asp:Label ID="PIsupcode" runat="server" Style="display: none" ></asp:Label>
                                                      <asp:Label ID="PIsupcurcode" runat="server" Style="display: none" ></asp:Label>
                                                      <asp:Label ID="PIconvrate" runat="server" Style="display: none" ></asp:Label>
                                                      <asp:Label ID="PIsalesmancode" runat="server" Style="display: none" ></asp:Label>
                                                      <asp:Label ID="PIacctcode" runat="server" Style="display: none" ></asp:Label>
                                                       <asp:Label ID="PIcccode" runat="server"  Style="display: none"></asp:Label>
                                                       <asp:Label ID="PItxtPIParticulars" runat="server" Style="display: none"></asp:Label>
                                                    <asp:Label ID="PIvattype" runat="server"  Style="display: none"></asp:Label>
                                                    <asp:Label ID="PInontaxamt" runat="server"  Style="display: none"></asp:Label>
                                                     <asp:Label ID="PItaxamt" runat="server"  Style="display: none"></asp:Label>
                                                      <asp:Label ID="PIvatperc" runat="server"  Style="display: none"></asp:Label>
                                                      <asp:Label ID="PIvatamt" runat="server"  Style="display: none"></asp:Label>
                                                      <asp:Label ID="value" runat="server"  Style="display: none"></asp:Label>
                                                       <asp:Label ID="valuebase" runat="server"  Style="display: none"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CostCenter">
                                              <ItemTemplate>
                                                   <asp:TextBox ID="txtcostcenter" runat="server" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" Height="26px"></asp:TextBox>
                                                                <asp:TextBox ID="txtcostcentercode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtcostcenter_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getcostcenterlist" TargetControlID="txtcostcenter" OnClientItemSelected="costcenterautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                              </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtacctCode" runat="server" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" Height="26px"></asp:TextBox>
                                                                     <asp:AutoCompleteExtender ID="txtacctcode_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="GetacctSalesCodelist" TargetControlID="txtacctCode"  OnClientItemSelected="salescodeacctautocompleteselected">
                                                                </asp:AutoCompleteExtender>



                                                    
                                              <br />
                                                <asp:Label ID="lblaccname" runat="server" Text="Name:" CssClass="field_Caption"></asp:Label>
                                               <br />
                                               
                                               
                                           

                                          

                                          
 <asp:TextBox ID="txtacct" runat="server" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" Height="26px"></asp:TextBox>
                                                               
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtacct_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="GetacctSalesNamelist" TargetControlID="txtacct"  OnClientItemSelected="salesnameacctautocompleteselected">
                                                                </asp:AutoCompleteExtender>



                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="StartDate">
                                         <ItemTemplate>
                                           <asp:TextBox ID="txtstartdate" runat="server" CssClass="fiel_input" Width="80px"
                                                            ValidationGroup="MKE"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnstartDT"  runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidatorstart" runat="server" CssClass="field_error" Width="23px"
                                                            ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                                            ControlToValidate="txtstartdate" ControlExtender="Mskeditextsales"></cc1:MaskedEditValidator>
                                                        <cc1:MaskedEditExtender ID="Mskeditextsales" runat="server" TargetControlID="txtstartdate"
                                                            MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                            DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="startCalendarExtender1" runat="server" TargetControlID="txtstartdate"
                                                            PopupButtonID="ImgBtnstartDT" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                            </ItemTemplate>

                                                            

                                         </asp:TemplateField>
                                          <asp:TemplateField HeaderText="End/Operation Date">

                                          <ItemTemplate>
                                              <asp:TextBox ID="txtendtdate" runat="server" CssClass="fiel_input" Width="80px"
                                                            ValidationGroup="MKE"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnendDT"  runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidatorend" runat="server" CssClass="field_error" Width="23px"
                                                            ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                                            ControlToValidate="txtendtdate" ControlExtender="Mskeditextsalesend"></cc1:MaskedEditValidator>
                                                        <cc1:MaskedEditExtender ID="Mskeditextsalesend" runat="server" TargetControlID="txtendtdate"
                                                            MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                            DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="endCalendarExtender1" runat="server" TargetControlID="txtendtdate"
                                                            PopupButtonID="ImgBtnendDT" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                          
                                          </ItemTemplate>



                                         </asp:TemplateField>
                                           <asp:TemplateField HeaderText="No:Units">
                                           <ItemTemplate>
                                                <asp:TextBox Style="text-align: center"  ID="txtnoofunits" CssClass="field_input" runat="server" Width="96px"></asp:TextBox>
                                                </ItemTemplate>
                                         </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particulars">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox ID="txtParticulars"  TextMode="MultiLine" runat="server" CssClass="field_input"
                                                 height="50px"   Width="97%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="VATType" HeaderStyle-Width="5%" itemStyle-Width="5%" >
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlvattype"   runat="server">
                                                    <asp:ListItem>Taxable</asp:ListItem>
                                                    <asp:ListItem>ZeroRated</asp:ListItem>
                                                    <asp:ListItem>Exempt</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount"  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                            <asp:Label ID="Lblnontaxable" runat="server" Text="NonTaxableAmt:" CssClass="field_Caption"></asp:Label>
                                               <br /> <asp:TextBox Style="text-align: right" ID="txtnontaxableamt" runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox><br />
                                                    <asp:Label ID="Label1" runat="server" Text="TaxableAmt:" CssClass="field_Caption"></asp:Label>
                                               <br /> 
                                                <asp:TextBox Style="text-align: right"   ID="txttaxableamt" runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox><br />
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      
                                        <asp:TemplateField HeaderText="VAT %"  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:TextBox Style="text-align: right" ID="txtvatperc"  runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox><br />
                                                <asp:Label ID="lblvatamt" runat="server" Text="VATAmt:"  CssClass="field_Caption"></asp:Label>
                                                <br />
                                                <asp:TextBox Style="text-align: right" ID="txtvatamt"  runat="server" Width="96px"
                                                    CssClass="field_input" Enabled="False"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value()"  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                                &nbsp;<asp:TextBox Style="text-align: right" ID="txtCurrValue" runat="server" Width="96px"
                                                    CssClass="field_input" Enabled="False"></asp:TextBox>

                                                <asp:TextBox  ID="txtbaseValue" style="display:none" runat="server" CssClass="field_input"
                                                    Width="96px" Enabled="False" ></asp:TextBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Purchase"  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                               <asp:CheckBox ID="chckPurchase" AutoPostBack="true"  OnCheckedChanged="chckPurchase_CheckedChanged"   runat="server" Width="10px"></asp:CheckBox>
                                               </br>
                                               <asp:Button ID="btnpurchase" runat="server" enabled="false" Text="Purchase" OnClick="btnselect_Click" />
                                            </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                    
                                           <asp:TemplateField HeaderText=".."  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                               
                                            </ItemTemplate>
                                            <ItemStyle CssClass="displaynonemhd" />
                                            <HeaderStyle CssClass="displaynonemhd" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete"  HeaderStyle-Width="5%" itemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chckDeletion" runat="server" Width="10px"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle" ForeColor="Black" Font-Size="10px"></RowStyle>
                                    <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Size="10px" Font-Bold="True">
                                    </SelectedRowStyle>
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle CssClass="grdheader" Font-Bold="True"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                </asp:GridView>


                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAddLine" runat="server" CausesValidation="False" CssClass="field_button"
                                            TabIndex="23" Text="Add Row" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDelLine" TabIndex="24" runat="server" Text="DeleteRow" CssClass="field_button"
                                            CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="td_cell" align="right">
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="td_cell" align="center">
                                        <asp:Label ID="Label18" runat="server" CssClass="field_Caption" Text="Taxable Amt"
                                            Width="83px"></asp:Label>
                                    </td>
                                    <td class="td_cell">
                                        <asp:Label ID="Label19" runat="server" CssClass="field_Caption" Text="NonTaxable Amt"
                                            Width="127px"></asp:Label>
                                    </td>
                                    <td class="td_cell">
                                        <asp:Label ID="Label20" runat="server" CssClass="field_Caption" Text="VAT Amt" Width="83px"></asp:Label>
                                    </td>
                                    <td class="td_cell" align="center">
                                        <asp:Label ID="Label21" runat="server" CssClass="field_Caption" Text="Total " Width="83px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell" align="left">
                                        <asp:Label ID="lblsubtotal_curr" runat="server" autopostback=true CssClass="field_Caption" Text="SubTotal() "
                                            Width="102px"></asp:Label>
                                    </td>
                                    <td class="td_cell">
                                        <asp:TextBox ID="txttaxableamttot" runat="server" CssClass="field_input" Enabled="false"
                                             Width="86px" align="left"></asp:TextBox>
                                    </td>
                                    <td class="td_cell" align="center">
                                        <asp:TextBox ID="txtnontaxableamttot" runat="server" CssClass="field_input" Enabled="false"
                                            Width="73px" align="right"></asp:TextBox>
                                    </td>
                                    <td class="td_cell">
                                        <asp:TextBox ID="txtvatamttot" runat="server" CssClass="field_input" Enabled="false"
                                         Width="52px" align="right"></asp:TextBox>
                                    </td>
                                    <td class="td_cell">
                                        <asp:TextBox ID="txtsalevaltot" runat="server" CssClass="field_input" Enabled="false"
                                             Width="144px" align="right"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_cell" align="left">
                                        <asp:Label ID="lblsubtotal_base" runat="server" CssClass="field_Caption" Text="SubTotal() "
                                            Width="123px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbasetaxableamttot" runat="server" CssClass="field_input" Enabled="false"
                                             Width="86px" align="right"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtBasenontaxableamttot" runat="server" CssClass="field_input" Enabled="false"
                                            TabIndex="1" Width="73px" align="right"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasevatamttot" runat="server" CssClass="field_input" Enabled="false"
                                            Width="52px" align="right"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasesalevaltot" runat="server" CssClass="field_input" Enabled="false"
                                             Width="144px" align="right"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 59px">
                            <asp:CheckBox ID="chkPost" runat="server" BackColor="#FFC0C0" Checked="true" CssClass="field_caption"
                                Font-Bold="True" Font-Names="Verdana" Font-Size="10px" ForeColor="Black" TabIndex="19"
                                Text="Post/UnPost" Width="103px" />
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="25" Text="Save" OnClientClick="return ValidatePage();" />
                            &nbsp;
                            <asp:Button ID="btnPrint" runat="server" CssClass="field_button" TabIndex=
                            "26" Text="Print" />
                             <asp:Button ID="btnPdfReport" runat="server" CssClass="field_button" TabIndex=
                            "26" Text="Pdf Report" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="field_button"
                                TabIndex="27" Text="Exit" />
                            &nbsp;
                            <asp:Button ID="btnhelp" runat="server"  style="display:none"  CssClass="field_button" TabIndex="28" Text="Help" />
                        </td>
                    </tr>
                </tbody>
            </table>
            
            </td> </tr> 
            <tr>
             <td colspan="3">
                                            <div id="ShowPurchase" runat="server" style="overflow: scroll; height: 584px; width: 1000px;
                                                border: 3px solid green; background-color: White; ">
                                                <table  style="width: 100%;">
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                        


                                                         <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="4" style="height: 27px">
                            <asp:Label ID="Label2" runat="server" Text="Purchase Invoice" Width="100%"  ></asp:Label>
                        </td>
                    </tr>
                    <tr> <td align="left" style="width: 246px">
                                                        <asp:Label ID="lblPIdate" runat="server" Text="Purchase Invoice Date" Width="192px" CssClass="field_Caption"
                                                            Height="16px"></asp:Label>
                                                        &nbsp
                                                        </td>
                                                    <td align="left" style="width: 183px">
                                                        <asp:TextBox ID="txtPIdate" TabIndex="2" runat="server" CssClass="fiel_input" Width="80px"
                                                            ValidationGroup="MKE"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnPIDT" TabIndex="2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" CssClass="field_error" Width="23px"
                                                            ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"
                                                            EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                                            ControlToValidate="txtPIdate" ControlExtender="MskextPIDT"></cc1:MaskedEditValidator>
                                                        <cc1:MaskedEditExtender ID="MskextPIDT" runat="server" TargetControlID="txtPIdate"
                                                            MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                            DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                                                        <cc1:CalendarExtender ID="ClsExPIDate" runat="server" TargetControlID="txtPIdate"
                                                            PopupButtonID="ImgBtnPIDT" Format="dd/MM/yyyy"></cc1:CalendarExtender>



                                                            

                                                    </td></tr>
                                                    <tr>
                                                     <td align="left" style="width: 246px">
                                                  
                                                        <asp:Label ID="lblPItype" runat="server" Text="PI Type" Width="95px" CssClass="field_Caption"
                                                            Height="16px"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 183px">
                                                        <asp:DropDownList ID="ddlPItype" runat="server" TabIndex="3">
                                                            <asp:ListItem>Tax Invoice</asp:ListItem>
                                                            <asp:ListItem>Commercial Invoice</asp:ListItem>
                                                            <asp:ListItem>Select</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    </tr>
                                                    <tr>

 <td style="width: 246px"  class="td_cell" align="left">
                                                        <asp:Label ID="lblsupprefno" runat="server" Text="Supplier Invoice No " 
                                                            Width="193px" CssClass="field_Caption"></asp:Label>
                                                    </td>
                                                    <td style="width: 183px" align="left">
                                                        <asp:TextBox ID="txtsupprefno" TabIndex="10" runat="server" Width="203px" CssClass="field_input"
                                                            Enabled="True"></asp:TextBox>
                                                    </td>                                                    </tr>
                                                    <tr>
                                               
                                                      

                                                        <td colspan="5" class="td_cell">
                                            <asp:Panel  ID="Panel1" runat="server"  GroupingText="Supplier" TabIndex="6" CssClass="field_Caption" Style="margin-bottom: 0px">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_cell" style="width: 93px; height: 32px;">
                                                                <asp:Label ID="lblpisupp" runat="server" Text="Supplier" Width="109px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td style="height: 32px">
                                                                <asp:TextBox ID="txtsuppname" runat="server" TabIndex="7" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="200px" ></asp:TextBox>
                                                                <asp:TextBox ID="txtsuppcode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField5" runat="server" Visible="False" />
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getsupplist" TargetControlID="txtsuppname" OnClientItemSelected="supautocompleteselected">
                                                                   
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td style="height: 32px;">
                                                                <asp:Label ID="lblPIcurrname" runat="server" Text="Currency" Width="66px" 
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td style="height: 32px"> <asp:TextBox ID="txtPIcurrcurrname" runat="server" Width="120px" CssClass="field_input"
                                                                  Enabled="False"></asp:TextBox>
                                                                <asp:TextBox ID="txtPIcurrcurrcode"  runat="server" Width="120px" CssClass="field_input"
                                                                     style="VISIBILITY: hidden" Enabled="False"></asp:TextBox>
                                                            </td>
                                                          
                                                              <td style="height: 32px">
                                                              
                                                            </td>
                                                        </tr>
                                                        <tr>                                                         
                                                        
                                                            <td>
                                                                <asp:Label ID="lblPIconvrate" runat="server" Text="Conversion Rate" Width="96px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtsuppconvrt"  runat="server" Width="120px" CssClass="field_input"
                                                                     Enabled="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPIsupptrnno" runat="server" Text="TRNNo" Width="65px"
                                                                    CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtsupptrnno" runat="server" Width="269px" CssClass="field_input"
                                                                   Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                              
                                                                <asp:Label ID="lblPIctrlacct" runat="server" CssClass="field_Caption" 
                                                                    Text="Control A|C" Width="96px"></asp:Label>
                                                              
                                                            </td>
                                                            <td colspan="3" align="left">
        <asp:TextBox ID="txtPIxtrlaccode" runat="server" CssClass="field_input" Enabled="False"
                                                                   TabIndex="1" Width="73px"></asp:TextBox>
                                                       <asp:TextBox ID="txtPIxtrlacname" runat="server" CssClass="field_input" Enabled="False"
                                                                     TabIndex="1" Width="158px"></asp:TextBox>
                                                            </td>
                                                              <td>
       
                                                        

                                                            </td>
                                                        </tr>
                                                        <tr><td>
                                                        <asp:Label ID="lblPIsales" runat="server" CssClass="field_Caption" Text="Salesman "></asp:Label>
                                                        </td>
                                                        <td>
                                                        <asp:TextBox ID="txtsalesPI" runat="server" CssClass="field_input" Enabled="True" 
                                                                    TabIndex="8" Width="200px"></asp:TextBox>

                                                                    <asp:TextBox ID="txtsalesPIcode" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField7" runat="server" />
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getsalesuserlist" TargetControlID="txtsalesPI" OnClientItemSelected="salesPIautocompleteselected">
                                                                </asp:AutoCompleteExtender>

                                                        </td>
                                                        <td> <asp:Label ID="Label6" runat="server" CssClass="field_Caption" Text="Legal Name of Entity "></asp:Label></td>
                                                        <td>       <asp:TextBox ID="txtsuppalias" runat="server" CssClass="field_input" Enabled="False"
                                                                     TabIndex="1" Width="158px"></asp:TextBox></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>




                                                    <td align="left">
                                                       
                                                        </td>
                                                    </tr>
                                                        <tr>
                                               
                                                      

                                                        <td colspan="5" class="td_cell">
                                            <asp:Panel  ID="Panel2" runat="server"  GroupingText="Accounts" TabIndex="6" CssClass="field_Caption" Style="margin-bottom: 0px">
                                         <table>
                                                    <tbody>
                                                    <tr>
                                                     <td class="td_cell" style="width: 93px; height: 32px;">
                                                                <asp:Label ID="lblsuppacct" runat="server" Text="Account Code/Name" Width="109px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>         
                                                           <asp:TextBox ID="txtsuppacctCode" runat="server" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" ></asp:TextBox>
                                                              <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="GetPurchaseacctcodelist" TargetControlID="txtsuppacctCode"  OnClientItemSelected="acctcodePIautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                            
                                                             <asp:TextBox ID="txtsuppacct" runat="server" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" ></asp:TextBox>
                                                       
                                                                <asp:HiddenField ID="HiddenField6" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtsuppacct_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="GetPurchaseacctlist" TargetControlID="txtsuppacct"  OnClientItemSelected="acctPIautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                            
                                                            
                                                            
                                                            </td>
                                                            </tr>
                                                              <tr>
                                                     <td class="td_cell" style="width: 93px; height: 32px;">
                                                                <asp:Label ID="Label3" runat="server" Text="CostCenter Code/Name" Width="109px" CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td>  
                                                                     <asp:TextBox ID="txtPIcostcentercode" runat="server" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" ></asp:TextBox>
                                                                       <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getcostcentercodelist" TargetControlID="txtPIcostcentercode" OnClientItemSelected="PIcostcentercodeautocompleteselected">
                                                                </asp:AutoCompleteExtender>

                                                              
                                                                       </td>
                                                                       <td>
                                               <asp:TextBox ID="txtPIcostcenter" runat="server" AutoPostBack="false" CssClass="field_input"
                                                                    MaxLength="500"  Width="97%" ></asp:TextBox>
                                                                
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                <asp:AutoCompleteExtender ID="txtcostcenter_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    ServiceMethod="Getcostcenterlist" TargetControlID="txtPIcostcenter" OnClientItemSelected="PIcostcenterautocompleteselected">
                                                                </asp:AutoCompleteExtender>

  
                                                            </td>

                                                            </tr>
                                                            <tr>
                                                            <td> <asp:Label ID="Label4" runat="server" Text="Particulars" Width="109px" CssClass="field_Caption"></asp:Label></td>
                                                            <td colspan="2"><asp:TextBox ID="txtPIParticulars" TextMode="MultiLine" 
                                                                    runat="server" CssClass="field_input"
                                                 height="50px"   Width="700px"></asp:TextBox></td>


                                                            </tr>
                                                            
                                                            <tr>
                                                            <td> <asp:Label ID="Label5" runat="server" Text="VAT Type" Width="109px" CssClass="field_Caption"></asp:Label></td>
                                                            <td>

  <asp:DropDownList ID="ddlvattype"   runat="server">
                                                    <asp:ListItem>Taxable</asp:ListItem>
                                                    <asp:ListItem>ZeroRated</asp:ListItem>
                                                    <asp:ListItem>Exempt</asp:ListItem>
                                                      </asp:DropDownList>

</td>


                                                            </tr>
                                                                     <tr>
                                                            <td> <asp:Label ID="LblnontaxablePI" runat="server" Text="NonTaxableAmt:" CssClass="field_Caption"></asp:Label> </td>
                                                            <td><asp:TextBox Style="text-align: right" ID="txtPInontaxableamt" runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox></td>
                                                            <td> <asp:Label ID="LbltaxablePI" runat="server" Text="TaxableAmt:" CssClass="field_Caption"></asp:Label></td>
                                                              <td>  <asp:TextBox Style="text-align: right"   ID="txtPItaxableamt" runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox></td>

                                                            </tr>
                                                            <tr>
                                                            <td> <asp:Label ID="lblvatpercPI" runat="server" Text="VAT%:"  CssClass="field_Caption"></asp:Label></td>
                                                            <td> <asp:TextBox Style="text-align: right" ID="txtPIvatperc"  runat="server" Width="96px"
                                                    CssClass="field_input"></asp:TextBox><br /></td>
                                                              <td>
                                                                    <asp:Label ID="lblvatamtPI" runat="server" CssClass="field_Caption" Text="VATAmt:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                        <asp:TextBox Style="text-align: right" ID="txtPIvatamt"  runat="server" Width="96px"
                                                    CssClass="field_input" Enabled="False"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            
                                                              
                                                            <tr>
                                                            <td>
                                                            <asp:Label ID="lblPIvalue" runat="server" Text="Value:"  CssClass="field_Caption"></asp:Label>
                                                            </td>
                                                            <td><asp:TextBox Style="text-align: right" ID="txtPICurrValue" runat="server" Width="96px"
                                                    CssClass="field_input" Enabled="False"></asp:TextBox>
</td>
                                                            <td><asp:Label ID="lblPIBAsevalue" runat="server" Text="Value:"  CssClass="field_Caption"></asp:Label></td>
                                                            <td><asp:TextBox Style="text-align: right" ID="txtPIbaseValue" runat="server" CssClass="field_input"
                                                    Width="96px" Enabled="False"></asp:TextBox></td>
                                                            </tr>
                                                     </tbody>
                                                </table>
                                         </asp:Panel>
                                        </td>
                                           </tr>
                    </tbody>
                    </table>


                                                    <tr><td colspan="2" ;align="center" align="center">
                                                      <asp:Button ID="btnPIUpdatetogrid" runat="server" CssClass="field_button" 
                                                            Text="Update" Width="75px" />
                                                   
                                                        <asp:Button ID="btnPIclose" runat="server" CssClass="field_button" Text="Close" 
                                                            Width="75px" />
                                                   
                                                    </td></tr>
                                                </table>
                                                <input id="btnInvisiblePurchase" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                <input id="btnOkayPI" type="button" value="OK" style="visibility: hidden" />
                                                <input id="btnCancelPI" type="button" value="Cancel" style="visibility: hidden" />
                                            </div>
                                            <asp:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                CancelControlID="btnCancelPI" OkControlID="btnOkayPI" TargetControlID="btnInvisiblePurchase"
                                                PopupControlID="ShowPurchase" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                BackgroundCssClass="ModalPopupBG">
                                            </asp:ModalPopupExtender>
                                           <asp:HiddenField ID="HFPI" runat="server" />
                                        </td>
            </tr></tbody> </table> 
                 <asp:HiddenField ID="hdnSS" runat="server" Value="0" /> <%-- Tanvir 08112023--%>
   </ContentTemplate>
    </asp:UpdatePanel>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>

</asp:Content>

