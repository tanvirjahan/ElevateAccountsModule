$(document).ready(function(){
	$(".offerBlock a").each(function(){
		var mc = $(this).find("span").html()+"";
		$(this).qtip({
		  content: mc,
		  position: {
				corner: {
					 tooltip: "rightMiddle", // Use the corner...
					 target: "leftMiddle" // ...and opposite corner
				  }
				},
		   style: { 
			  border: {
				 width: 5,
					 radius: 10
				  },
				 padding: 10,
				 textAlign: 'center',
				 tip: true, // Give it a speech bubble tip with automatic corner detection
				 name: 'light' // Style it according to the preset 'cream' style
		   }
		});
	});
	
	$(".offerBlock a").click(function(){
		var imgPath =  $(this).attr("imagePath");
		var offerUrl =  $(this).attr("href");
		$('#loader').addClass('loading');
		$('#loader').html("");
		$(".imageHolder").show("fast");
		$('#loader').css("height",500);
		$(function () {
			var img = new Image();
			$(img).load(function () {
				//$(this).css('display', 'none'); // .hide() doesn't work in Safari when the element isn't on the DOM already
				$(this).hide();
				$('#loader').removeClass('loading').append(this);
				$('#loader').css("height",625);
				$('.topMenu').show();
				$(this).fadeIn();
				$("#goToOffer").attr("href",offerUrl);
				
			}).error(function () {
				// notify the user that the image could not be loaded
			}).attr('src',imgPath);
		});
		return false;
	});
	$('#backToOffers').click(function(){
		$(".imageHolder").hide("fast");
		$('.topMenu').hide();
	});
});