<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintDocNew.aspx.vb" Inherits="PriceListModule_PrintDocNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

 <script type="text/javascript">
 function PrintWindow(){ 
if (navigator.appName == "Microsoft Internet Explorer") { 
     var PrintCommand = '< O B J E C T ID="PrintCommandObject" WIDTH=0 HEIGHT=0 ';
PrintCommand += 'CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></O B J E C T>';
     document.body.insertAdjacentHTML('beforeEnd', PrintCommand); 
     PrintCommandObject.ExecWB(6, -1); PrintCommandObject.outerHTML = ""; } 
else { window.print();} }  

var gAutoPrint = true; // Flag for whether or not to automatically call the print function
function printSpecial() {
if (document.getElementById != null) {
var html = '\n\n'; 
if (document.getElementsByTagName != null) { 
var headTags = document.getElementsByTagName("head"); 
if (headTags.length > 0) 
html += headTags[0].innerHTML;
} 
html += '\n< / H E A D >\n< B O D Y>\n';
var printReadyElem = document.getElementById("printReady"); 
if (printReadyElem != null) { 
html += printReadyElem.innerHTML; } 
else { 
alert("Could not find the printReady section in the HTML"); return; } 
html += '\n</ B O D Y >\n</ H T M L >'; 
var printWin = window.open("","printSpecial"); 
printWin.document.open(); 
printWin.document.write(html); 
printWin.document.close(); 
if (gAutoPrint) printWin.print(); 
}
else { 
alert("Sorry, the print ready feature is only available in modern browsers."); 
} }
function Button1_onclick() {
alert('test');
window.print();
//PrintWindow();
}
 

function ReprintDoc()
{if(confirm('Are you sure you want to reprint ?')==false)
{return false;}
else
{
window.print();
 return true;
 }
}
 

 </script>
 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
 <link rel="stylesheet" href= "../css/Styles.css" type="text/css" />
       <style type="text/css" media="print">
div.page	{ 
writing-mode: tb-rl;
height: 80%;
margin: 10% 0%;
}
</style>
</head>
<body onload="return window_onload()" >
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
