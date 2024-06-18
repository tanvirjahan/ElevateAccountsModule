
var ChangeImage = 1;
var MyLeft = 0;
var MyX = 0;
var Interval;
var Id = '';
jQuery(document).ready(function () {

    
     
});


function MyAutoHotelFillArray() {
   

   
   if (citycode == null)
       citycode = 'DXB';
//  alert(citycode);
//  alert('1');

  jQuery.ajax({
      url: "../ClsServices.asmx/GetQuotPartydetails",
      data: "{ para1:'" + citycode + "'}",
      dataType: "json",
      type: "POST",

      contentType: "application/json; charset=utf-8",
      dataFilter: function (data) { return data; },
      success: function (data) {
          alert('2');
          alert(data.d.length);
          if (data.d.length > 0) {

              alert(data.d.length);

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

                      //                      jQuery(".MyAutoCompleteHotelClassValue").val(ui.item.text);


//                      alert("Good morning");
                      //                      if (jQuery(".MyDropDownListHotelValue").val() != null || jQuery(".MyDropDownListHotelValue").val() != undefined) {
                      //                          jQuery(".MyDropDownListHotelValue").val(ui.item.text);

                      //                          jQuery(".MyDropDownListHotelValue").change();
                      //                      }

                  }
              });

          }
      },
      error: function (XMLHttpRequest, textStatus, errorThrown) {

      }
  });

}

function OnChangeType(TextBoxId, DropDownId, ValueId,ddlChoice,val) {


    
   

    
    ChangeType(TextBoxId, jQuery("#" + ValueId).text(), DropDownId, ddlChoice,val);

}




function ChangeType(TextBoxId, MyType, DropDownId, ddlChoice,val) {



    
    
    
    
    strchoicevalue = ddlChoice;
   
    jQuery.ajax({
        url: "../ClsServices.asmx/GetQuotPartydetails",


        

        data: "{ strCityCode:'" + MyType + "',strchoice:'" + strchoicevalue + "', sptypecode:'" + val + "'}",



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

