// JavaScript Document
$(document).ready(function(){
	//------- LOGIN PAGE ----------//
		function arrangeMap(){			
			var bWidth = $("body").innerWidth();
			var bHeight = $("body").innerHeight();
			var mapWidth = $("#map").width();
			var mapHeight = $("#map").height();
			if(mapWidth>bWidth){
				var diffW = mapWidth - bWidth;
				var diffH = mapHeight - bHeight;
				$("#map").css("left",0-Math.floor(diffW/2));
				$("#map").css("top",0-Math.floor(diffH/2));
			}
		}
		
		$(window).resize(function() {
			arrangeMap();
		});
		arrangeMap(); // initial setup
		
		// info
		$("#jorInfo").hide();
		$("#uaeInfo").hide();
		$(".pointer").click(function(){
			exchangeInfo($(this));
			return false;
		});
		
		function exchangeInfo(n){
			var nId = n.attr("id");
			//alert(nId);
			switch(nId){
				case "jordanPointer":
					$("#jorInfo").fadeIn().show("slow");
					$("#uaeInfo").fadeOut().hide("slow");
				break;
				
				case "uaePointer":
					$("#uaeInfo").fadeIn().show("slow");
					$("#jorInfo").fadeOut().hide("slow");
				break;
			}
		}
		
	});