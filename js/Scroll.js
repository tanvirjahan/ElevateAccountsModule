// JScript File

function setScroll(val, posId) {
    posId.value = val.scrollTop;
}

 
//only required for w/o ajax page ScrollWOAjax.
function scrollTo(what, posId) {
      if (what != "0")  
      document.getElementById(what).scrollTop = document.getElementById(posId).value;
      

}
