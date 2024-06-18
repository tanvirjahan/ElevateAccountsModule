<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VehicleExpensesReportSearch.aspx.vb" Inherits="NewclientsSearch"  MasterPageFile="~/ExcursionMaster.master" Strict="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

 <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %><asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    
<script language="javascript" type="text/javascript" >
    
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
                
        case "fromsellcode":
                var select=document.getElementById("<%=ddlvehfromcode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlvehfromname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddlvehtocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddlvehtoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
               
                break;
        case "fromsellname":
            var select = document.getElementById("<%=ddlvehfromname.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlvehfromcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddlvehtocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlvehtoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tosellcode":
                var select = document.getElementById("<%=ddlvehtocode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlvehtoname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tosellname":
                var select = document.getElementById("<%=ddlvehtoname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlvehtocode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;


            case "fromclientcode":
                var select = document.getElementById("<%=ddlfromclientcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromclientname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddltoclientcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltoclientname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
               


                break;
            case "fromclientname":
                var select = document.getElementById("<%=ddlfromclientname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromclientcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //08092014
                var select1 = document.getElementById("<%=ddltoclientcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltoclientname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "toclientcode":
                var select = document.getElementById("<%=ddltoclientcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltoclientname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "toclientname":
                var select = document.getElementById("<%=ddltoclientname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltoclientcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "frompartycode":
                var select = document.getElementById("<%=ddlsupfromcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlsupfromname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //08092014
                var select1 = document.getElementById("<%=ddlsuptocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddlsuptoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;
               

                break;
            case "frompartyname":
                var select = document.getElementById("<%=ddlsupfromname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlsupfromcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddlsuptocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlsuptoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "topartycode":
                var select = document.getElementById("<%=ddlsuptocode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlsuptoname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "topartyname":
                var select = document.getElementById("<%=ddlsuptoname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlsuptocode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "fromairportcode":
                var select = document.getElementById("<%=ddldriverfromcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddldriverfromname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddldrivertocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddldrivertoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromairportname":
                var select = document.getElementById("<%=ddldriverfromname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddldriverfromcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddldrivertocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddldrivertoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;
            case "toairportcode":
                var select = document.getElementById("<%=ddldrivertocode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddldrivertoname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "toairportname":
                var select = document.getElementById("<%=ddldrivertoname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddldrivertocode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            case "fromairlinecode":
                var select = document.getElementById("<%=ddlcarfromcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlcarfromname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddlcartocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddlcartoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

//                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//                constr = connstr.value
//                // alert(select.options[select.selectedIndex].value);
//                ColServices.clsServices.getflightdetfromairlinecode(constr, cat, fillflightfrom, ErrorHandler, TimeOutHandler);
                //ColServices.clsServices.GetSellingCurrCodenew(constr, codeid, FillSupplierCurrCode, ErrorHandler, TimeOutHandler);

                break;
            case "fromairlinename":
                var select = document.getElementById("<%=ddlcarfromname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlcarfromcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //08092014
                var select1 = document.getElementById("<%=ddlcartocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlcartoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

//                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//                constr = connstr.value
//                ColServices.clsServices.getflightdetfromairlinecode(constr, cat, fillflightfrom, ErrorHandler, TimeOutHandler);
                break;

            case "toairlinecode":
                var select = document.getElementById("<%=ddlcartocode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlcartoname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

//                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//                constr = connstr.value
//                ColServices.clsServices.getflightdetfromairlinecode(constr, cat, fillflightto, ErrorHandler, TimeOutHandler);

                break;
            case "toairlinename":
                var select = document.getElementById("<%=ddlcartoname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlcartocode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

//                var connstr = document.getElementById("<%=txtconnection.ClientID%>");
//                constr = connstr.value
//                ColServices.clsServices.getflightdetfromairlinecode(constr, cat, fillflightto, ErrorHandler, TimeOutHandler);

                break;


            case "fromflightcode":
                var select = document.getElementById("<%=ddlexpfromcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexpfromname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //08092014
                var select1 = document.getElementById("<%=ddlexptocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddlexptoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromflightname":
                var select = document.getElementById("<%=ddlexpfromname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlexpfromcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //08092014
                var select1 = document.getElementById("<%=ddlexptocode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddlexptoname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;
            case "toflightcode":
                var select = document.getElementById("<%=ddlexptocode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlexptoname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "toflightname":
                var select = document.getElementById("<%=ddlexptoname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlexptocode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;
    


        }
        }
        
        
        



    
function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }


function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}

function ChangeDate()
{
   
     var txtfdate=document.getElementById("<%=txtexpdtfrom.ClientID%>");
     
       if (txtfdate.value==''){alert("Enter From Date.");txtfdate.focus();  }
     // else {ColServices.clsServices.GetQueryReturnFromToDate('FromDate',30,txtfdate.value,FillToDate,ErrorHandler,TimeOutHandler);}
}



    function radbtn_onclick(type) {

        switch (type)
        {
            case "selltypeall":

                var selectfromcode = document.getElementById("<%=ddlvehfromcode.ClientID%>");
                var selectfromname = document.getElementById("<%=ddlvehfromname.ClientID%>");
                var selecttocode = document.getElementById("<%=ddlvehtocode.ClientID%>");
                var selecttoname = document.getElementById("<%=ddlvehtoname.ClientID%>");
                selectfromcode.disabled = true;
                selectfromname.disabled = true;
                selecttocode.disabled = true;
                selecttoname.disabled = true;
                selectfromcode.value = "[Select]";
                selectfromname.value = "[Select]";
                selecttocode.value = "[Select]";
                selecttoname.value = "[Select]";
                break;

       case "selltyperange":

           var selectfromcode = document.getElementById("<%=ddlvehfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlvehfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlvehtocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlvehtoname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";


           break;

       case "clientsall":

           var selectfromcode = document.getElementById("<%=ddlfromclientcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlfromclientname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddltoclientcode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddltoclientname.ClientID%>");
           selectfromcode.disabled = true;
           selectfromname.disabled = true;
           selecttocode.disabled = true;
           selecttoname.disabled = true;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";


           break;

       case "clientsrange":

           var selectfromcode = document.getElementById("<%=ddlfromclientcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlfromclientname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddltoclientcode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddltoclientname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";


           break;

       case "partyall":

           var selectfromcode = document.getElementById("<%=ddlsupfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlsupfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlsuptocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlsuptoname.ClientID%>");
           selectfromcode.disabled = true;
           selectfromname.disabled = true;
           selecttocode.disabled = true;
           selecttoname.disabled = true;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;

       case "partyrange":

           var selectfromcode = document.getElementById("<%=ddlsupfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlsupfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlsuptocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlsuptoname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";


           break;

       case "airportall":

           var selectfromcode = document.getElementById("<%=ddldriverfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddldriverfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddldrivertocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddldrivertoname.ClientID%>");
           selectfromcode.disabled = true;
           selectfromname.disabled = true;
           selecttocode.disabled = true;
           selecttoname.disabled = true;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;

       case "airportrange":

           var selectfromcode = document.getElementById("<%=ddldriverfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddldriverfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddldrivertocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddldrivertoname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;


       case "airlineall":

           var selectfromcode = document.getElementById("<%=ddlcarfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlcarfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlcartocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlcartoname.ClientID%>");
           selectfromcode.disabled = true;
           selectfromname.disabled = true;
           selecttocode.disabled = true;
           selecttoname.disabled = true;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;


       case "airlinerange":

           var selectfromcode = document.getElementById("<%=ddlcarfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlcarfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlcartocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlcartoname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;

       case "flightall":

           var selectfromcode = document.getElementById("<%=ddlexpfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlexpfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlexptocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlexptoname.ClientID%>");
           selectfromcode.disabled = true;
           selectfromname.disabled = true;
           selecttocode.disabled = true;
           selecttoname.disabled = true;
           selectfromcode.value = "[Select]";
           selectfromname.value = "[Select]";
           selecttocode.value = "[Select]";
           selecttoname.value = "[Select]";
           break;

       case "flightrange":

           var selectfromcode = document.getElementById("<%=ddlexpfromcode.ClientID%>");
           var selectfromname = document.getElementById("<%=ddlexpfromname.ClientID%>");
           var selecttocode = document.getElementById("<%=ddlexptocode.ClientID%>");
           var selecttoname = document.getElementById("<%=ddlexptoname.ClientID%>");
           selectfromcode.disabled = false;
           selectfromname.disabled = false;
           selecttocode.disabled = false;
           selecttoname.disabled = false;

           //selectfromcode.value = "[Select]";
           //selectfromname.value = "[Select]";
           //selecttocode.value = "[Select]";
           //selecttoname.value = "[Select]";
           break;

       case "requestfromdate":

           var selectfromcode = document.getElementById("<%=txtexpdtfrom.ClientID%>");
         
           var selecttocode = document.getElementById("<%=txtexpdtto.ClientID%>");
         
           selectfromcode.disabled = true;
          
           selecttocode.disabled = true;
          
          
           break;

       case "requesttodate":

           var selectfromcode = document.getElementById("<%=txtexpdtfrom.ClientID%>");
         
           var selecttocode = document.getElementById("<%=txtexpdtto.ClientID%>");
          
           selectfromcode.disabled = false;
        
           selecttocode.disabled = false;
          
           break;





        }
    }

 



   

</script> <table>
        <tr>
            <td style="width: 362px">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center; width: 100%;">
                            Vehicle Expense Report</td>
                    </tr>
                   
                    <tr>
                        <td style="width: 100%; height: 375px;">
  
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY>
                      


      <table>
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel4" runat="server"  Width="679px" 
            GroupingText="Vehicle " Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="radvehall" onclick ="radbtn_onclick('selltypeall');" runat="server" checked name="Controlsell" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlvehfromcode" 
                onclick =" CallWebMethod('fromsellcode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell">
            <select 
            id="ddlvehfromname" onclick =" CallWebMethod('fromsellname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radvehrange" onclick ="radbtn_onclick('selltyperange');" runat="server" name="Controlsell" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell">
                <select id="ddlvehtocode" onclick =" CallWebMethod('tosellcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell">
            <select 
            id="ddlvehtoname" onclick =" CallWebMethod('tosellname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>

         <table>
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel1" runat="server"  Width="679px" 
            GroupingText="Clients" Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="Radio1" onclick ="radbtn_onclick('clientsall');" runat="server" checked name="Controlclient" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlfromclientcode" onclick =" CallWebMethod('fromclientcode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlfromclientname" onclick =" CallWebMethod('fromclientname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="Radio2" onclick ="radbtn_onclick('clientsrange');" runat="server" name="Controlclient" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell"><select id="ddltoclientcode" onclick =" CallWebMethod('toclientcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddltoclientname" onclick =" CallWebMethod('toclientname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>

          <table>
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel2" runat="server"  Width="679px" 
            GroupingText="Supplier" Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="radsupall" onclick ="radbtn_onclick('partyall');" runat="server" checked name="Controlparty" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlsupfromcode" 
                onclick =" CallWebMethod('frompartycode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell">
            <select 
            id="ddlsupfromname" onclick =" CallWebMethod('frompartyname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radsuprange" onclick ="radbtn_onclick('partyrange');" runat="server" name="Controlparty" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell">
                <select id="ddlsuptocode" onclick =" CallWebMethod('topartycode');" 
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell">
            <select 
            id="ddlsuptoname" onclick =" CallWebMethod('topartyname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>

       <table>
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel3" runat="server"  Width="679px" 
            GroupingText="Driver" Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="raddriverall" onclick ="radbtn_onclick('airportall');" runat="server" checked name="Controlairport" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddldriverfromcode" 
                onclick =" CallWebMethod('fromairportcode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddldriverfromname" onclick =" CallWebMethod('fromairportname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="raddriverrange" onclick ="radbtn_onclick('airportrange');" runat="server" name="Controlairport" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell">
                <select id="ddldrivertocode" onclick =" CallWebMethod('toairportcode');" 
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddldrivertoname" onclick =" CallWebMethod('toairportname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>
      
     <table>
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel5" runat="server"  Width="679px" 
            GroupingText="Cartype" Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="radcarall" onclick ="radbtn_onclick('airlineall');" runat="server" checked name="Controlairline" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlcarfromcode" 
                onclick =" CallWebMethod('fromairlinecode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlcarfromname" onclick =" CallWebMethod('fromairlinename');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radcarrange" onclick ="radbtn_onclick('airlinerange');" runat="server" name="Controlairline" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell">
                <select id="ddlcartocode" onclick =" CallWebMethod('toairlinecode');" 
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell">
            <select 
            id="ddlcartoname" onclick =" CallWebMethod('toairlinename');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>
     
       <table>
      <TR><TD style="WIDTH: 100px; height: 72px;">
        <asp:Panel id="Panel6" runat="server"  Width="679px" 
            GroupingText="Exptype" Font-Names="Arial" Font-Size="12px"><TABLE style="WIDTH: 676px"><TBODY>
        <tr><td class="td_cell">
            <input id="radexpall" onclick ="radbtn_onclick('flightall');" runat="server" checked name="Controlflight" 
            type="radio" /> All</td><td class="td_cell">From Code</td><td 
            class="td_cell"><select id="ddlexpfromcode" 
                onclick =" CallWebMethod('fromflightcode');" runat="server" class="drpdown" 
              style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">From Name</td><td class="td_cell"><select 
            id="ddlexpfromname" onclick =" CallWebMethod('fromflightname');"  
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radexprange" onclick ="radbtn_onclick('flightrange');" runat="server" name="Controlflight" type="radio" /> Range</td><td 
            class="td_cell">To Code</td><td class="td_cell">
                <select id="ddlexptocode" onclick =" CallWebMethod('toflightcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 200px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell">To Name</td><td class="td_cell"><select 
            id="ddlexptoname" onclick =" CallWebMethod('toflightname');"  
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD></TR>
   </table>

               <!-- requestdate -->
               <table>
               <tr>
              <TD style="WIDTH: 100px">
        <asp:Panel id="Panel15" runat="server"  Width="679px" 
            GroupingText="Expense. Date" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radreqall" onclick ="radbtn_onclick('requestfromdate');" runat="server" checked name="controlrequestdate" 
            type="radio" /> All</td>
            <TD><asp:Label id="Label3" runat="server" Text="From" CssClass="td_cell"></asp:Label></TD>
    <TD><asp:TextBox id="txtexpdtfrom" runat="server" CssClass="fiel_input" Width="80px" 
            AutoCompleteType="Cellular"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator1" runat="server" CssClass="field_input" Display="Dynamic" ControlToValidate="txtexpdtfrom" ControlExtender="MEFromDate" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
        </tr>
        <tr>
            <td class="td_cell">
            <input id="radreqrange" onclick ="radbtn_onclick('requesttodate');" runat="server" name="controlrequestdate" type="radio" /> Range</td>
            <TD><asp:Label id="Label4"  runat="server" Text="To" CssClass="td_cell"></asp:Label></TD>
     <TD><asp:TextBox id="txtexpdtto"   runat="server" CssClass="fiel_input" 
             Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate"  runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator2"  runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtexpdtto" ControlExtender="METoDate" InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
         </tr></TBODY></TABLE></asp:Panel></TD>
         </tr>
         </table>
   <table>
     <TR><TD style="width: 87px">&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD style="width: 83px">&nbsp;</TD>
     <TD>&nbsp;</TD>
   
    </TR>
    <tr>
    <TD style="WIDTH: 87px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Transfer Type</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddltrftype" runat="server" Width="100px" 
                CssClass="drpdown" >
                <asp:ListItem Value="0">Arrival</asp:ListItem>
                <asp:ListItem Value="1">Departure</asp:ListItem>
                <asp:ListItem Value="2">Shifting </asp:ListItem>
                <asp:ListItem Value="3">All</asp:ListItem>
               </asp:DropDownList>
        </TD>
        <TD style="WIDTH: 83px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Report Type</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddldet" runat="server" Width="100px" CssClass="drpdown" >
                <asp:ListItem Value="0">Detail</asp:ListItem>
                <asp:ListItem Value="1">Brief </asp:ListItem>
              
               </asp:DropDownList>
        </TD>

        <TD style="WIDTH: 91px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Expense Type</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddlexptype" runat="server" Width="100px" 
                CssClass="drpdown" >
                <asp:ListItem Value="0">Tansfer</asp:ListItem>
                <asp:ListItem Value="1">Safari</asp:ListItem>
                <asp:ListItem Value="2">Others</asp:ListItem>
                <asp:ListItem Value="3">All</asp:ListItem>
               </asp:DropDownList>
        </TD>
     </tr> 
    </table>  


      <TR><TD>&nbsp;</TD><TD colSpan=3>&nbsp;</TD></TR>

      <table><TR><TD style="TEXT-ALIGN: center" colSpan=4>
<asp:Button id="BtnClear" tabIndex=13 runat="server" Text="Clear" CssClass="field_button" Width="61px"></asp:Button>&nbsp;
 <asp:Button visible ="false" id="btndisplay" tabIndex=14 runat="server" Text="Display" CssClass="field_button"></asp:Button>&nbsp;
  <asp:Button id="BtnPrint" tabIndex=15 runat="server" Text="Load Report" CssClass="field_button" 
                            onclick="BtnPrint_Click"></asp:Button>&nbsp;
  <asp:Button id="btnhelp" tabIndex=16 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="field_button"></asp:Button>
    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" /><input id="txtCityCode" runat="server" style="visibility: hidden;
            width: 9px; height: 3px" type="text" /><input id="txtCityName" runat="server" style="visibility: hidden;
                width: 9px; height: 3px" type="text" /></TD>
                
                
<td style="display:none">
               <<asp:Button id="btnadd" tabIndex=16  runat="server"   CssClass="field_button"></asp:Button>
               <asp:Button id="Button1" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>
                   
                </TR>
        </table>

        <TR><TD>&nbsp;</TD><TD colSpan=3>&nbsp;</TD></TR>
                <TR><TD colSpan=4><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
 <asp:Label id="lblMsg" runat="server" 
            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE> <cc1:CalendarExtender id="CEFromDate" runat="server" TargetControlID="txtexpdtfrom" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender>
 <cc1:MaskedEditExtender id="MEFromDate" runat="server" TargetControlID="txtexpdtfrom" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> 
 <cc1:CalendarExtender id="CEToDate" runat="server" TargetControlID="txtexpdtto" PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
 <cc1:MaskedEditExtender id="METoDate" runat="server" TargetControlID="txtexpdtto" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>

     <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

     <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%>
    

</asp:Content>