
$(document).ready(function () {

   // MyAutoCustomer();
    MyAutohotel_rptFillArray();
    MyAutoShifthotel_rptFillArray();

});

function MyAutoCustomer() {
    var type = jQuery(".MyAutoCompleteTypeClass").val();
    jQuery.ajax({
        url: "../ClsServices.asmx/CustomerAutoCompleteExcursionRequest",
        data: "{ para1:'" + type + "',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;
                // alert(result.length);

                if (result.length == 0)
                    return;

                jQuery(".MyAutoCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {

                        jQuery(".MyDropDownListCustValue").val(ui.item.text);
                        jQuery(".MyDropDownListCustValue").change();

                    }


                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function MyAutohotel_rptFillArray() {
  var type = jQuery(".MyAutoCompleteHotelTypeClass").val();
  jQuery.ajax({
    url: "../ClsServices.asmx/hotelAutoComplete",        
      data: "{ para1:'" + type + "',para2:''}",
      dataType: "json",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataFilter: function (data) { return data; },


      success: function (data) {

          if (data.d.length > 0) {
              var result = data.d;

              jQuery(".MyAutoCompleteHotelClass").autocomplete({
                  source: jQuery.map(result, function (item) {
                      return {
                          value: item.Name,
                          text: item.Id

                      }

                  }),
                  minLength: 1,
                  select: function (event, ui) {

                      jQuery(".MyDropDownListsuppValue").val(ui.item.text);
                      jQuery(".MyDropDownListsuppValue").change();


                      if (jQuery('.MyAutoCompleteHotelValueClass').val() != null || jQuery('.MyAutoCompleteHotelValueClass').val() != undefined) {
                          jQuery('.MyAutoCompleteHotelValueClass').val(ui.item.text)
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


function MyAutoShifthotel_rptFillArray() {
   var type = jQuery(".MyAutoCompleteShiftTypeClass").val();
    jQuery.ajax({
        url: "../ClsServices.asmx/hotelAutoComplete",
        data: "{ para1:'" + type + "',para2:''}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {

            if (data.d.length > 0) {
                var result = data.d;

                jQuery(".MyAutoShiftsupplierCompleteClass").autocomplete({
                    source: jQuery.map(result, function (item) {
                        return {
                            value: item.Name,
                            text: item.Id

                        }

                    }),
                    minLength: 1,
                    select: function (event, ui) {

                        jQuery(".MyDropDownListShiftsuppValue").val(ui.item.text);
                        jQuery(".MyDropDownListShiftsuppValue").change();

                    }


                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus);
        }
    });
}



function ChangeType1(TextBoxId, TextBoxValueId,hdnfield) {

    var type = jQuery(".MyAutoCompleteHotelTypeClass").val();
    jQuery.ajax({
        url: "../ClsServices.asmx/hotelAutoComplete",
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

                        jQuery("#" + TextBoxValueId).val(ui.item.text);
                        jQuery("#" + hdnfield).val(ui.item.text);
                       
//                        jQuery("#" + DropDownId).change();
                    }



                });

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            alert(textStatus);
        }
    });
}

