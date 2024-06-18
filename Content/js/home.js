//------- HOME PAGE ----------//
$(document).ready(function(){
	jQuery(function(){
		jQuery('ul.sf-menu').superfish();
	});
	
	$("#offers").fancybox({
				'width'				: 900,
				'height'			: 500,
				'autoScale'			: false,
				'transitionIn'		: 'none',
				'transitionOut'		: 'none',
				'type'				: 'iframe'
	});
});	
		