

var popupStatus = 0;
var popupStatus1 = 0;
var popupStatus2 = 0;
var popupStatusForPromo = 0;
var LoadingPopUp = 0;



function loadPopup() {

   // alert('Hello');
	
	if(popupStatus==0){
		$("#MainPop").css({
		    "position":"absolute",
			"opacity": "0.9",
			"z-index":"5001"
			
		
		});
		
		 $("#NewLoad").css({
		  "position":"relative",
			"opacity": "0.9"
			
		});
		
		$("#MainPop").fadeIn("slow");
		$("#PopDiv").fadeIn("slow");
		popupStatus = 1;
	}
}


function disablePopup(){
	

		$("#MainPop").fadeOut("slow");
		$("#PopDiv").fadeOut("slow");
		popupStatus = 0;
	
}

function centerPopup() {

    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = $("#PopDiv").height();
    var popupWidth = $("#PopDiv").width();
   
     var windowWidth1 = $(document).width(); 

    var newleft = 0;
     newleft = windowWidth1 / 2 - popupWidth / 2

   //alert('I Am Heere Parent');
    $("#MainPop").css({
		"height": "2000px",
        "width": windowWidth,
        "z-index":"5001"
	});

 $("#PopDiv").css({
        "top": "300px" ,
        "left": newleft,
        "z-index":"5100"
       
    });
    newleft=newleft-160;
    
    
}

  
    $("#PopDivClose").live("click",function () {
        disablePopup();
    });
    

    $("#MainPop").live("click",function () {
      
        disablePopup();
        
    });

    $(document).keypress(function (e) {
        if (e.keyCode == 27 && popupStatus == 1 || popupStatus1 == 1 || popupStatus2 == 1 || popupStatusForPromo == 1) {
            disablePopup();

        }
        if (e.keyCode == 27 && popupStatus == 0 || popupStatus1 == 0 || popupStatus2 == 0 || popupStatusForPromo == 0) {
            disablePopup();

        }
        
    });




function disableLoadingPopup() {

   
        $("#PopDiv").fadeOut("slow");
      
        
}

$("#PopDiv").live("click",function () {

    disablePopup();

   });






   function ShowPanel(MyId) {

       var MyWidth = 700;

       $("#PopDiv").css({

           "width": MyWidth
           
         
       });

       $("#MyShowDivId").html('');

       $("#MyShowDivId").html($("#" + MyId).html());

       centerPopup();

       loadPopup()
      // alert($(window).scrollTop());
       var topheight = $(window).scrollTop() + 150;
       $("#PopDiv").css({

           "top": topheight +"px"


       });

   }


   $.getDocHeight = function () {
       var D = document;
       return Math.max(Math.max(D.body.scrollHeight, D.documentElement.scrollHeight), Math.max(D.body.offsetHeight, D.documentElement.offsetHeight), Math.max(D.body.clientHeight, D.documentElement.clientHeight));
   };
