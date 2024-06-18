
jQuery(document).ready(function () {



    MyAutoCustomerFillArray();

    MyAutosupplier_rptFillArray();

    MyAutoSupp_rptFillArray();

    //Added MyAutoSupp_rptFillArray() by Archana on 11/04/2015

});



function MyAutoCustomerFillArray() {
    var type = jQuery(".MyAutoCompleteTypeClass").val();
    var div = jQuery(".MyAutoCompletedivClass").val();
    jQuery.ajax({
        url: "../ClsServices.asmx/AccountsAutoCompletenew",

        data: "{ para1:'" + type + "',para2:'',para3:'" + div + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery(".MyAutoCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery(".MyDropDownListCustValue").val(ui.item.text);
                        jQuery(".MyDropDownListCustValue").change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}

/*
function OnChangeType(TextBoxId, DropDownId, ValueId) {

   

    ChangeType(TextBoxId, jQuery("#" + ValueId).val(), DropDownId);

}
*/

function OnChangeType(TextBoxId, DropDownId, ValueId, relTextBoxId, IsCode) {
    if (IsCode == undefined) { IsCode = '0' } else { IsCode = '1' };
    ChangeType(TextBoxId, jQuery("#" + ValueId).val(), DropDownId, relTextBoxId, IsCode);
}


/*

function ChangeType(TextBoxId, MyType, DropDownId) {
   
    jQuery.ajax({
        url: "../ClsServices.asmx/AccountsAutoComplete",

        data: "{ para1:'" + MyType + "',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery("#" + TextBoxId).autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery("#" + DropDownId).val(ui.item.text);
                        jQuery("#" + DropDownId).change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}


*/


function ChangeType(TextBoxId, MyType, DropDownId, relTextBoxId, IsCode) {
    var div = jQuery(".MyAutoCompletedivClass").val();
    var param = '';
    if (IsCode == '1') {
        param = "{ para1:'" + MyType + "',para2:'1',para3:'" + div + "'}";
    } else {
        param = "{ para1:'" + MyType + "',para2:'',para3:'" + div + "'}";
    }
    jQuery.ajax({
        url: "../ClsServices.asmx/AccountsAutoCompletenew",
        data: param,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data.d.length > 0) {
                BindAutoCompleteValues(data, TextBoxId, DropDownId, relTextBoxId);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}

function BindAutoCompleteValues(data, TextBoxId, DropDownId, relTextBoxId) {
    var result = data.d;
    jQuery("#" + TextBoxId).autocomplete({
        source: jQuery.map(result, function (item) {
            if (item.IsCode == "1") {
                return {
                    value: item.Id,
                    text: item.Name
                }
            }
            else {
                return {
                    value: item.Name,
                    text: item.Id
                }
            }
        }),
        minLength: 1,
        select: function (event, ui) {
            jQuery("#" + relTextBoxId).val(ui.item.text);
            jQuery("#" + DropDownId).val(ui.item.text);
            jQuery("#" + DropDownId).change();
        }
    });
}

function MyAutoaccountsFillArray(TextBoxId,DropDownId) {

  
    jQuery.ajax({
        url: "../ClsServices.asmx/AccountsallAutoComplete",

        data: "{ para1:'',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery("#" + TextBoxId).autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery("#" + DropDownId).val(ui.item.text);
                        jQuery("#" + DropDownId).change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}





function MyAutodebitsFillArray(TextBoxId, DropDownId) {
    var type = jQuery(".MyAutoCompletedivClass").val();

    jQuery.ajax({
        url: "../ClsServices.asmx/AccountsdebitnoteAutoComplete",

        data: "{ para1:'" + type + "',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery("#" + TextBoxId).autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery("#" + DropDownId).val(ui.item.text);
                        jQuery("#" + DropDownId).change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}


function MyAutoCustomer_rptFillArray() {
   
    jQuery.ajax({
        url: "../ClsServices.asmx/CustomerAutoComplete",

        data: "{ para1:'',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery(".MyAutoCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery(".MyDropDownListCustValue").val(ui.item.text);
                        jQuery(".MyDropDownListCustValue").change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}



function MyAutoSupp_rptFillArray() {

    jQuery.ajax({
        url: "../ClsServices.asmx/supplierAutoComplete",

        data: "{ para1:'',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery(".MyAutosupplierCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                     alert(ui.item.text);
                        jQuery(".MyDropDownListSuppValue").val(ui.item.text);
                        jQuery(".MyDropDownListSuppValue").change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}


// Added function MyAutoSupp_rptFillArray by Archana on 11/04/2014 for cost register in Accounts module


function MyAutosupplier_rptFillArray() {
   // alert("Test");

    jQuery.ajax({
        url: "../ClsServices.asmx/supplierAutoComplete",

        data: "{ para1:'',para2:'HOT'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;



                jQuery(".MyAutosupplierCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                      //  alert(ui.item.text);
                        jQuery(".MyDropDownListsuppValue").val(ui.item.text);
                     //   alert(jQuery(".MyDropDownListsuppValue").val());
                        // jQuery(".MyDropDownListsuppValue").change();
                    fillvalue();
                    }
                   


                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}



function MyAutosupplier_rptFillArray1(ctrycode, citycode, category, ddlhotel) {


    jQuery.ajax({
        url: "../ClsServices.asmx/supplierAutoCompletefilter",

        data: "{ 'para1':'','para2':'HOT','para3':'" + ctrycode + "','para4':'" + citycode + "','para5':'" + category + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;

                /*
                var i = 0;
                jQuery('.HotelFillSelect').empty();

                for (i = 0; i < result.length; i++) {
                jQuery('.HotelFillSelect').append("<option value='" + result[i].Id + "'>" + result[i].Name + "</option>")
                }
                jQuery('.HotelFillSelect').append("<option selected='selected' value='[Select]'>[Select]</option>")

                */


                jQuery(".MyAutosupplierCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id


                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        jQuery(".MyDropDownListsuppValue").val(ui.item.text);

                        // alert(jQuery(".MyDropDownListsuppValue").val());
                      fillvalue();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });


}


