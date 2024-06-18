
var ChangeImage = 1;
var MyLeft = 0;
var MyX = 0;
var Interval;
var Id = '';
jQuery(document).ready(function () {

  


});


function MyAutoRoomFillArray() {

  

    //if (citycode == null)
    var sptypecode = jQuery(".MyAutocompleteRoom").val();
    //  alert(citycode);
    //  alert('1');
    
    jQuery.ajax({
        url: "../ClsServices.asmx/GetRoomdetails",
        data: "{ para1:'" + sptypecode + "'}",
        dataType: "json",
        type: "POST",

        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            alert('2');
            alert(data.d.length);
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


                    }
                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });

}

function OnChangeType(TextBoxId, DropDownId, val) {
ChangeType(TextBoxId, DropDownId, val);
}

function ChangeType(TextBoxId, DropDownId, val) {
    

    jQuery.ajax({
        url: "../ClsServices.asmx/GetRoomdetails",
        data: "{sptypecode:'" + val + "'}",
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











