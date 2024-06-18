
var Banner_vCurrent = 0;
var Banner_vTotal = 0;
var Banner_vDuration = 5000;
var Banner_intInterval = 0;
var Banner_vGo = 1;
var Banner_vIsPause = false;
var Banner_tmp = 20;
var Banner_title = "";
var Banner_imgW = 288;
var Banner_imgH = 209;

var Banner_vCurrentR = 0;
var Banner_vTotalR = 0;
var Banner_vDurationR = 5000;
var Banner_intIntervalR = 0;
var Banner_vGoR = 1;
var Banner_vIsPauseR = false;
var Banner_tmpR = 20;
var Banner_titleR = "";
var Banner_imgWR = 288;
var Banner_imgHR = 209;





jQuery(document).ready(function () {
    Banner_vTotal = $(".BannerSlides").children().size() - 1;   
    Banner_intInterval = setInterval(Banner_fnLoop, Banner_vDuration);
    //Horizontal
    $("#BannerHead").find(".BannerSlide").each(function (i) {
        Banner_tmp = ((i - 1) * Banner_imgW) - ((Banner_vCurrent - 1) * Banner_imgW);
        $(this).animate({ "left": Banner_tmp + "px" }, 500);
    });
    function Banner_fnChange() {
        clearInterval(Banner_intInterval);
        Banner_intInterval = setInterval(Banner_fnLoop, Banner_vDuration);
        Banner_fnLoop();
    }
    function Banner_fnLoop() {
        if (Banner_vGo == 1) {
            Banner_vCurrent == Banner_vTotal ? Banner_vCurrent = 0 : Banner_vCurrent++;
        } else {
            Banner_vCurrent == 0 ? Banner_vCurrent = Banner_vTotal : Banner_vCurrent--;
        }
        $("#BannerHead").find(".BannerSlide").each(function (i) {
            
            //Horizontal Scrolling
            Banner_tmp = ((i - 1) * Banner_imgW) - ((Banner_vCurrent - 1) * Banner_imgW);
            $(this).animate({ "left": Banner_tmp + "px" }, 500);
        });
    }
//});


//jQuery(document).ready(function () {

    Banner_vTotalR = $(".BannerSlidesR").children().size() - 1;
    Banner_intIntervalR = setInterval(Banner_fnLoopR, Banner_vDurationR);
    //Horizontal
    $("#BannerHeadRight").find(".BannerSlideR").each(function (i) {
        Banner_tmpR = ((i - 1) * Banner_imgWR) - ((Banner_vCurrentR - 1) * Banner_imgWR);
        $(this).animate({ "left": Banner_tmpR + "px" }, 500);
    });
    function Banner_fnChangeR() {
        clearInterval(Banner_intIntervalR);
        Banner_intIntervalR = setInterval(Banner_fnLoopR, Banner_vDurationR);
        Banner_fnLoopR();
    }

    function Banner_fnLoopR() {
        if (Banner_vGoR == 1) {
            Banner_vCurrentR == Banner_vTotalR ? Banner_vCurrentR = 0 : Banner_vCurrentR++;
        } else {
            Banner_vCurrentR == 0 ? Banner_vCurrentR = Banner_vTotalR : Banner_vCurrentR--;
        }
        $("#BannerHeadRight").find(".BannerSlideR").each(function (i) {

            //Horizontal Scrolling

            Banner_tmpR = ((i - 1) * Banner_imgWR) - ((Banner_vCurrentR - 1) * Banner_imgWR);
            $(this).animate({ "left": Banner_tmpR + "px" }, 500);
        });
    }
});
  













