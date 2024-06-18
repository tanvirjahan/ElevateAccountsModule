
jQuery(document).ready(function () {

    MyAutoRouteFillArray();


});



function MyAutoRouteFillArray() {
//        alert("hello");
        var strName = jQuery(".MyAutoCompleteClass").val();
//    var sellcode = jQuery(".MyAutocompletesellcode").val();
    //  alert(market);
    jQuery.ajax({
        url: "../ClsServices.asmx/RouteAutoComplete",
// ,para2:'" + sellcode + "'
        data: "{para1:'" + strName + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            // alert(data.d.length);
            if (data.d.length > 0) {

//                  alert("hello");


                var result = data.d;



                jQuery(".MyAutoCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name 
                        }
                    }),
                    minLength: 1,
                    select: function (event, ui) {
                        //                        alert(ui.item.text);
//                        jQuery(".MyAutoCompleteClassValue").val(ui.item.text);
//                        jQuery(".MyDropDownListCustValue").val(ui.item.text);

//                        jQuery(".MyDropDownListCustValue").change();

                    }
                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });





}