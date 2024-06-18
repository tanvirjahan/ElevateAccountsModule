var ChangeImage = 1;
var MyLeft = 0;
var MyX = 0;
var Interval;
var Id = '';
jQuery(document).ready(function () {

    MyAutoCustomerFillArray();

     
});

  

function MyAutoCustomerFillArray() {
   
    var market = jQuery(".MyAutocompleteMarket").val();
 
    var sellcode = jQuery(".MyAutocompletesellcode").val();

    jQuery.ajax({
        url: "../ClsServices.asmx/CustomerAutoComplete",

        data: "{ para1:'" + market + "',para2:'" + sellcode + "'}",
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

                        jQuery(".MyAutoCompleteClassValue").val(ui.item.text);

                    

                        if (jQuery(".MyDropDownListCustValue").val() != null || jQuery(".MyDropDownListCustValue").val() != undefined) {
                            jQuery(".MyDropDownListCustValue").val(ui.item.text);

                            jQuery(".MyDropDownListCustValue").change();
                        }

                    }
                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('rr');
            alert(textStatus);
        }
    });

}





