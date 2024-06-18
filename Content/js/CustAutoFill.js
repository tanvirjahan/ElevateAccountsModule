
jQuery(document).ready(function () {


    
     MyAutoCustomerFillArray();


});



function MyAutoCustomerFillArray() {
    var market = jQuery(".MyAutocompleteMarket").val();
  
    jQuery.ajax({
        url: "../ClsServices.asmx/CustAutoComplete",

        data: "{para1:'" + market + "'}",
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