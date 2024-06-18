var ChangeImage = 1;
var MyLeft = 0;
var MyX = 0;
var Interval;
var Id = '';
jQuery(document).ready(function () {
 



    $('.SliderImageClass').mousemove(function (e) {

        MyLeft = e.pageX + 100;
        MyX = e.pageY;
    });

    jQuery('.SliderImageClass').hover(function () {
        var MyId = jQuery(this).attr("Id")
        jQuery('#LoadImageDiv').css({
            'display': 'block',
            "top": MyX + "px",
            "left": MyLeft + "px",
            "position": "absolute"

        });
        Id = MyId;
        RotateImage();


    });

    jQuery('.SliderImageClass').mouseout(function () {
        var MyId = jQuery(this).attr("Id")
        jQuery('#LoadImageDiv').css({
            'display': 'none'

        });
        clearInterval(Interval);

    });
});

RepeatOperation = function (anonymousOperation, whenToYield) {
    var count = 0;
    return function () {
        if (++count >= whenToYield) {
            count = 0;
            setTimeout(function () { anonymousOperation(); }, whenToYield);
        }
        else {
            anonymousOperation();
        }
    }
};
function RotateImage() {
    Interval = setInterval(function () {
        jQuery('#MySliderImage').attr("src", jQuery('#' + Id).attr("Image" + ChangeImage));
        ChangeImage++;
        if (ChangeImage == 6) ChangeImage = 1;
    }, 5000);


}



function LoadImage() {

}

function DisableImage() {

}


function MyAutogrouppackageFillArray() {
    //    alert("hello");
    jQuery.ajax({
        url: "../ClsServices.asmx/reservationgrppackageAutoComplete",

        data: "{ para1:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {



                var result = data.d;



                jQuery(".MyAutogrpCompleteClass").autocomplete({ source: jQuery.map(result, function (item) {
                    return {

                        value: item.Name,
                        text: item.Name
                    }
                })


                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });





}

function MyAutoCustomerFillArray() {
    // alert("hello");
    var market = jQuery(".MyAutocompleteMarket").val();
    var sellcode = jQuery(".MyAutocompletesellcode").val();
    //  alert(market);
    jQuery.ajax({
        url: "../ClsServices.asmx/CustomerAutoComplete",

        data: "{ para1:'" + market + "',para2:'" + sellcode + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            // alert(data.d.length);
            if (data.d.length > 0) {

                //  alert("hello");


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
                        //                        alert(ui.item.text);
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
            alert(textStatus);
        }
    });

}



function MyAutoCustomer() {
    // alert("hello");

    jQuery.ajax({
        url: "../ClsServices.asmx/CustomerSearchAutoComplete",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data.d.length > 0) {
                var result = data.d;
                jQuery(".MyAutoCompleteCustClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id
                        }
                    }),
                    minLength: 1,
                    select: function (event, ui) {

                        jQuery(".MyAutoCompleteCustClassValue").val(ui.item.text);
                        jQuery(".MyAutoCompleteCustClassName").val(ui.item.value);
                         
                        jQuery(".SearchButtonC").click();
                    }
                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
    });
}

