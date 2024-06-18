


 
 /*! Custom - 13/10/2013
* 
* Includes: Custom
*  Judson  */
 
 
 
   var timeToYield = 2000;
   var InterValUser=0;
   var InterValCom=0;
   var WindowList=[];
   var AbotLiveChat;
    

  jQuery(document).ready(function () {

  try{
   LoadingAllForPages();
   }
   catch(Error){

   }

});





function LoadingAllForPages() {

     
         jQuery('.LiveChatShow').click(function(){
         jQuery('.LiveChatShow').attr('disabled',true);
         jQuery('.CloseLiveChat').attr('disabled',false);
               jQuery('.UserChatTypeclass').css({'display':'block'});
             
                ShowCommunicationDetails();

             });

              jQuery('.CloseLiveChat').click(function(){

               jQuery('.LiveChatShow').attr('disabled',false);
               
                    jQuery('.UserChatTypeclass').css({'display':'none'});
                    AbotLiveChat.abort();
                    jQuery('.UserChatTypeclass').html('');
                    jQuery('.CloseLiveChat').attr('disabled',true);

              });

              if(jQuery(".AutoAgentClassValue").val()!=null || jQuery(".AutoAgentClassValue").val()!=undefined){
                MyAutoAgent();
                }


}





var RepeatOperation = function (anonymousOperation, whenToYield) {
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



var ShowCommunicationDetails = new RepeatOperation(function () {

 
  

  AbotLiveChat=  jQuery.ajax({
        type: "post",
        url: '../MyChatWebService.asmx/ShowCommunicationDetails',
        dataType: "json",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (status) {
            var result = status.d;
            var i = 0;

             

            for (i = 0; i < result.length; i++) {

            var MyMessage=result[i].Name;
            var j=0;
             if(MyMessage.length>0){
              for(j=0;j<MyMessage.length;j++){
             MyMessage=MyMessage.replace("DOUBLEQUOTESSTRING",'"');
             MyMessage=MyMessage.replace("SINGLEQUOTESSTRING","'");
             MyMessage=MyMessage.replace("LINEBREAK","<br/>");
             MyMessage=MyMessage.replace("FORWARDSLASH","/");
             MyMessage=MyMessage.replace("GREATERTHAN",">");
             MyMessage=MyMessage.replace("LESSTHAN","<");
             MyMessage=MyMessage.replace("BACKWARDSLASH","\\");
                
            }
            }
           var MyClass='table.ChatTableAddLive';
          
          // if(result[i].Extra!='')
              jQuery(MyClass).append('<tr><td style="width:100px">'+ result[i].Extra + '</td><td style="width:280px;text-align:left;">' + MyMessage + '</td></tr>');
            }
           ShowCommunicationDetails();
        },
        error: function (x) {

             /*
            alert(x.responseText);
            alert('error');

            */
        }
    });

   },timeToYield);






   function OpenNewWindow(Url,PageTitle,Left,Top,Width,height,Resizable,Scrollable){
      window.open(Url,PageTitle ,'left=' + Left +',top=' + Top + ',width=' + Width +',height=' + height + ',resizable=' + Resizable + ',scrollbars=' + Scrollable);
}
     


function encodeString(string,IgnoreSpecials) {   
 return string.replace(/([/\\&"'<>])/g, function(str, item) { return IgnoreSpecials[item]; });
 } 
 var IgnoreSpecials = {'&': 'AMDERSAND','"': 'DOUBLEQUOTESSTRING','<': 'LESSTHAN','>': 'GREATERTHAN',"'": 'SINGLEQUOTESSTRING',"\\":"BACKWARDSLASH","/":"FORWARDSLASH"};            



 function MyAutoAgent() {
    
      jQuery(".MyAutoCompleteClass").autocomplete({
        source: function(request, response) {
            jQuery.ajax({
                url: "../MyChatWebService.asmx/AgentAutoComplete",
                data: "{ para1:'" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { jQuery(".AutoAgentClassValue").val(''); return data; },
                success: function(data) {
                       data=data.d;
                if (data.length>0){
                
                             
                  
                 response(jQuery.map(data, function(item) {
                        return {
                            value:item.Name ,
                            text:item.Id
                        }
                    }))
                
                  } 
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        minLength: 1,
        select: function(event, ui) 
        {
        jQuery(".AutoAgentClassValue").val(ui.item.text);
                 
            
        }
    });

}