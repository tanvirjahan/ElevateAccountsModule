<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptExcursionReportSearch.aspx.vb" Inherits="NewclientsSearch"  MasterPageFile="~/ExcursionMaster.master" Strict="true"  %>

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

            case "frommaingpcode":
                var select=document.getElementById("<%=ddlfrommaingpcode.ClientID%>");                
                var cat=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlfrommaingpname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014
                var select1 = document.getElementById("<%=ddltomaingpcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltomaingpname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

               
                break;
            case "frommaingpname":
                var select=document.getElementById("<%=ddlfrommaingpname.ClientID%>");                
                var cat=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlfrommaingpcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014
                var select1 = document.getElementById("<%=ddltomaingpcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltomaingpname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                
                break;

            case "tomaingpcode":
                var select = document.getElementById("<%=ddltomaingpcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltomaingpname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tomaingpname":
                var select = document.getElementById("<%=ddltomaingpname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltomaingpcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

                //excursin Group

            case "fromexcgpcode":
                var select = document.getElementById("<%=ddlfromexcgpcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromexcgpname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                //07092014
                var select1 = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltoexcgpname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromexcgpname":
                var select = document.getElementById("<%=ddlfromexcgpname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromexcgpcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014
                var select1 = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltoexcgpname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "toexcgpcode":
                var select = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltoexcgpname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "toexcgpname":
                var select = document.getElementById("<%=ddltoexcgpname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;


            //City

            case "fromcitycode":
                var select = document.getElementById("<%=ddlfromcitycode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromcityname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltocitycode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltocityname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromcityname":
                var select = document.getElementById("<%=ddlfromcityname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromcitycode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //07092014 to 
                var select1 = document.getElementById("<%=ddltocitycode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltocityname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tocitycode":
                var select = document.getElementById("<%=ddltocitycode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltocityname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tocityname":
                var select = document.getElementById("<%=ddltocityname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltocitycode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;


            //excursin Provider  

            case "fromexcprovider":
                var select = document.getElementById("<%=ddlfromexcprovider.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromexcprovidername.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltoexcprovider.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromexcprovidername":
                var select = document.getElementById("<%=ddlfromexcprovidername.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromexcprovider.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //07092014 to 
                var select1 = document.getElementById("<%=ddltoexcprovider.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "toexcprovider":
                var select = document.getElementById("<%=ddltoexcprovider.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "toexcprovidername":
                var select = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltoexcprovider.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //Hotel

            case "fromhotelcode":
                var select = document.getElementById("<%=ddlfromhotelcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromhotelname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltohotelcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltohotelname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromhotelname":
                var select = document.getElementById("<%=ddlfromhotelname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromhotelcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;



                //07092014 to 
                var select1 = document.getElementById("<%=ddltohotelcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltohotelname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;
                break;

            case "tohotelcode":
                var select = document.getElementById("<%=ddltohotelcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltohotelname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tohotelname":
                var select = document.getElementById("<%=ddltohotelname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltohotelcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //Client

            case "fromclientcode":
                var select = document.getElementById("<%=ddlfromclientcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromclientname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                
                

                //07092014 from 
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

                //07092014 to 
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

            //Payment Terms

            case "frompaytermscode":
                var select = document.getElementById("<%=ddlfrompaytermscode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfrompaytermsname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //07092014 from 
                var select1 = document.getElementById("<%=ddltopaytermscode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltopaytermsname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "frompaytermsname":
                var select = document.getElementById("<%=ddlfrompaytermsname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfrompaytermscode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltopaytermscode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltopaytermsname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;


                break;

            case "topaytermscode":
                var select = document.getElementById("<%=ddltopaytermscode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltopaytermsname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "topaytermsname":
                var select = document.getElementById("<%=ddltopaytermsname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltopaytermscode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //Service Provider

            case "fromsp":
                var select = document.getElementById("<%=ddlfromsp.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromspname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosp.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltospname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromspname":
                var select = document.getElementById("<%=ddlfromspname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromsp.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //07092014 from 
                var select1 = document.getElementById("<%=ddltosp.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltospname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tosp":
                var select = document.getElementById("<%=ddltosp.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltospname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tospname":
                var select = document.getElementById("<%=ddltospname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltosp.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //Driver

            case "fromdrivercode":
                var select = document.getElementById("<%=ddlfromdcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromdname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltodcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltodname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromdrivername":
                var select = document.getElementById("<%=ddlfromdname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromdcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltodcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltodname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "todrivercode":
                var select = document.getElementById("<%=ddltodcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltodname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "todrivername":
                var select = document.getElementById("<%=ddltodname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltodcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //office Sales

            case "fromsalescode":
                var select = document.getElementById("<%=ddlfromsalescode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromsalesname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                //07092014 from 
                var select1 = document.getElementById("<%=ddltosalescode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltosalesname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromsalesname":
                var select = document.getElementById("<%=ddlfromsalesname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromsalescode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosalescode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltosalesname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tosalescode":
                var select = document.getElementById("<%=ddltosalescode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltosalesname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tosalesname":
                var select = document.getElementById("<%=ddltosalesname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltosalescode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //sales person

            case "fromsapcode":
                var select = document.getElementById("<%=ddlfromsapcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromsapname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosapcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltosapname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;

                break;
            case "fromsapname":
                var select = document.getElementById("<%=ddlfromsapname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromsapcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosapcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltosapname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;


                break;

            case "tosapcode":
                var select = document.getElementById("<%=ddltosapcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltosapname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tosapname":
                var select = document.getElementById("<%=ddltosapname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltosapcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //collected by

            case "fromcollectcode":
                var select = document.getElementById("<%=ddlfromcolcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromcolname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltocolcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltocolname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromcollectname":
                var select = document.getElementById("<%=ddlfromcolname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromcolcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltocolcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltocolname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tocollectcode":
                var select = document.getElementById("<%=ddltocolcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltocolname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tocollectname":
                var select = document.getElementById("<%=ddltocolname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltocolcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;


            //market

            case "frommarketcode":
                var select = document.getElementById("<%=ddlfrommarketcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfrommarketname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                  


                //07092014 from 
                var select1 = document.getElementById("<%=ddltomarketcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltomarketname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "frommarketname":
                var select = document.getElementById("<%=ddlfrommarketname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfrommarketcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

            
              

                //07092014 from 
                var select1 = document.getElementById("<%=ddltomarketcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltomarketname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tomarketcode":
                var select = document.getElementById("<%=ddltomarketcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltomarketname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

               

                break;
            case "tomarketname":
                var select = document.getElementById("<%=ddltomarketname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltomarketcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

               
                break;


            //Operator  

            case "fromoperatorcode":
                var select = document.getElementById("<%=ddlfromoperatorcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromoperatorname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltooperatorcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltooperatorname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromoperatorname":
                var select = document.getElementById("<%=ddlfromoperatorname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromoperatorcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltooperatorcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltooperatorname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tooperatorcode":
                var select = document.getElementById("<%=ddltooperatorcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltooperatorname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tooperatorname":
                var select = document.getElementById("<%=ddltooperatorname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltooperatorcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;




            //tourguide   

            case "fromtgcode":
                var select = document.getElementById("<%=ddlfromtgcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromtgname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltotgcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltotgname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromtgname":
                var select = document.getElementById("<%=ddlfromtgname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromtgcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltotgcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltotgname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "totgcode":
                var select = document.getElementById("<%=ddltotgcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltotgname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "totgname":
                var select = document.getElementById("<%=ddltotgname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltotgcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;

            //selling     

            case "fromsellcode":
                var select = document.getElementById("<%=ddlfromsellcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlfromsellname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosellcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].value;
                var selectname1 = document.getElementById("<%=ddltosellname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].text;


                break;
            case "fromsellname":
                var select = document.getElementById("<%=ddlfromsellname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlfromsellcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                //07092014 from 
                var select1 = document.getElementById("<%=ddltosellcode.ClientID%>");
                select1.value = select.options[select.selectedIndex].text;
                var selectname1 = document.getElementById("<%=ddltosellname.ClientID%>");
                selectname1.value = select.options[select.selectedIndex].value;

                break;

            case "tosellcode":
                var select = document.getElementById("<%=ddltosellcode.ClientID%>");
                var cat = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddltosellname.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                break;
            case "tosellname":
                var select = document.getElementById("<%=ddltosellname.ClientID%>");
                var cat = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddltosellcode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                break;



        }
        }


        function loadgridexc(type)
         {
            switch (type)
            {
            case "all":
                var mytextbox = document.getElementById("<%=txtexctype.ClientID%>");
                
                var mybtn = document.getElementById("<%=btnloadgrid.ClientID%>").style.visibility="hidden";
                mytextbox.value = ""
                mytextbox.style.visibility="hidden";
                //mytextbox.style.visibility = "hidden";
               // mybtn.disabled =true;
                break;
            case "range":
                var mytextbox = document.getElementById("<%=txtexctype.ClientID%>");
                mytextbox.style.visibility = "visible";
                var mybtn = document.getElementById("<%=btnloadgrid.ClientID%>");
                mybtn.style.visibility = "visible"
                mytextbox.value = ""
                mytextbox.style.display = "block";
                mybtn.style.display = "block";
                //mytextbox.style.visibility = "visible";
                // mybtn.disabled =false;
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




			function radbtn_onclick(type) {

			    switch (type) {
			        case "frommaingpcode":

			            var selectfromcode = document.getElementById("<%=ddlfrommaingpcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrommaingpname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltomaingpcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltomaingpname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tomaingpcode":

			            var selectfromcode = document.getElementById("<%=ddlfrommaingpcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrommaingpname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltomaingpcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltomaingpname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";


			            break;

			        case "fromexcgpcode":

			            var selectfromcode = document.getElementById("<%=ddlfromexcgpcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromexcgpname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltoexcgpname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "toexcgpcode":

			            var selectfromcode = document.getElementById("<%=ddlfromexcgpcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromexcgpname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltoexcgpcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltoexcgpname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";


			            break;

			        case "fromcitycode":

			            var selectfromcode = document.getElementById("<%=ddlfromcitycode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromcityname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltocitycode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltocityname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tocitycode":

			            var selectfromcode = document.getElementById("<%=ddlfromcitycode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromcityname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltocitycode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltocityname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";


			            break;


			        case "fromexcprovider":

			            var selectfromcode = document.getElementById("<%=ddlfromexcprovider.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromexcprovidername.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltoexcprovider.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "toexcprovider":

			            var selectfromcode = document.getElementById("<%=ddlfromexcprovider.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromexcprovidername.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltoexcprovider.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltoexcprovidername.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromhotelcode":

			            var selectfromcode = document.getElementById("<%=ddlfromhotelcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromhotelname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltohotelcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltohotelname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tohotelcode":

			            var selectfromcode = document.getElementById("<%=ddlfromhotelcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromhotelname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltohotelcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltohotelname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromclientcode":

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

			        case "toclientcode":

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

			        case "frompaytermscode":

			            var selectfromcode = document.getElementById("<%=ddlfrompaytermscode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrompaytermsname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltopaytermscode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltopaytermsname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "topaytermscode":

			            var selectfromcode = document.getElementById("<%=ddlfrompaytermscode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrompaytermsname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltopaytermscode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltopaytermsname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromsp":

			            var selectfromcode = document.getElementById("<%=ddlfromsp.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromspname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosp.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltospname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tosp":

			            var selectfromcode = document.getElementById("<%=ddlfromsp.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromspname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosp.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltospname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromdrivercode":

			            var selectfromcode = document.getElementById("<%=ddlfromdcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromdname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltodcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltodname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "todrivercode":

			            var selectfromcode = document.getElementById("<%=ddlfromdcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromdname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltodcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltodname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;


			        case "fromsalescode":

			            var selectfromcode = document.getElementById("<%=ddlfromsalescode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsalesname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosalescode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosalesname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tosalescode":

			            var selectfromcode = document.getElementById("<%=ddlfromsalescode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsalesname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosalescode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosalesname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;


			        case "fromsapcode":

			            var selectfromcode = document.getElementById("<%=ddlfromsapcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsapname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosapcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosapname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tosapcode":

			            var selectfromcode = document.getElementById("<%=ddlfromsapcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsapname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosapcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosapname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;


			        case "fromcollectcode":

			            var selectfromcode = document.getElementById("<%=ddlfromcolcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromcolname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltocolcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltocolname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tocollectcode":

			            var selectfromcode = document.getElementById("<%=ddlfromcolcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromcolname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltocolcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltocolname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;


			        case "frommarketcode":

			            var selectfromcode = document.getElementById("<%=ddlfrommarketcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrommarketname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltomarketcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltomarketname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            //10112014
//			            var tourdet = document.getElementById("<%=panel19.ClientID%>");
//			            var tourdet1 = document.getElementById("<%=ddlfromsellcode.ClientID%>");
//			            var tourdet2 = document.getElementById("<%=ddlfromsellname.ClientID%>");
//			            var tourdet3 = document.getElementById("<%=ddltosellcode.ClientID%>");
//			            var tourdet4 = document.getElementById("<%=ddltosellname.ClientID%>");
//			            tourdet1.value = "[Select]"
//			            tourdet2.value = "[Select]"
//			            tourdet3.value = "[Select]"
//			            tourdet4.value = "[Select]"
//			            tourdet.style.display = "none";
			            break;

			        case "tomarketcode":

			            var selectfromcode = document.getElementById("<%=ddlfrommarketcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfrommarketname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltomarketcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltomarketname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
                        //10112014
//			            var tourdet = document.getElementById("<%=panel19.ClientID%>");
//			            tourdet.style.display = "block";

//			            var tourdet1 = document.getElementById("<%=ddlfromsellcode.ClientID%>");
//			            var tourdet2 = document.getElementById("<%=ddlfromsellname.ClientID%>");
//			            var tourdet3 = document.getElementById("<%=ddltosellcode.ClientID%>");
//			            var tourdet4 = document.getElementById("<%=ddltosellname.ClientID%>");
//			            tourdet1.value = "[Select]"
//			            tourdet2.value = "[Select]"
//			            tourdet3.value = "[Select]"
//			            tourdet4.value = "[Select]"

			            break;

			        case "fromoperatorcode":

			            var selectfromcode = document.getElementById("<%=ddlfromoperatorcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromoperatorname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltooperatorcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltooperatorname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tooperatorcode":

			            var selectfromcode = document.getElementById("<%=ddlfromoperatorcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromoperatorname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltooperatorcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltooperatorname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromtgcode":

			            var selectfromcode = document.getElementById("<%=ddlfromtgcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromtgname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltotgcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltotgname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "totgcode":

			            var selectfromcode = document.getElementById("<%=ddlfromtgcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromtgname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltotgcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltotgname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "fromsellcode":

			            var selectfromcode = document.getElementById("<%=ddlfromsellcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsellname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosellcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosellname.ClientID%>");
			            selectfromcode.disabled = true;
			            selectfromname.disabled = true;
			            selecttocode.disabled = true;
			            selecttoname.disabled = true;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "tosellcode":

			            var selectfromcode = document.getElementById("<%=ddlfromsellcode.ClientID%>");
			            var selectfromname = document.getElementById("<%=ddlfromsellname.ClientID%>");
			            var selecttocode = document.getElementById("<%=ddltosellcode.ClientID%>");
			            var selecttoname = document.getElementById("<%=ddltosellname.ClientID%>");
			            selectfromcode.disabled = false;
			            selectfromname.disabled = false;
			            selecttocode.disabled = false;
			            selecttoname.disabled = false;
			            selectfromcode.value = "[Select]";
			            selectfromname.value = "[Select]";
			            selecttocode.value = "[Select]";
			            selecttoname.value = "[Select]";
			            break;

			        case "requestfromdate":

			            var selectfromcode = document.getElementById("<%=txtreqfrom.ClientID%>");

			            var selecttocode = document.getElementById("<%=txtreqto.ClientID%>");

			            selectfromcode.disabled = true;

			            selecttocode.disabled = true;

			            break;

			        case "requesttodate":

			            var selectfromcode = document.getElementById("<%=txtreqfrom.ClientID%>");

			            var selecttocode = document.getElementById("<%=txtreqto.ClientID%>");

			            selectfromcode.disabled = false;

			            selecttocode.disabled = false;


			            break;
			           


			        case "tourfromdate":

			            var selectfromcode = document.getElementById("<%=txttourfrom.ClientID%>");

			            var selecttocode = document.getElementById("<%=txttourto.ClientID%>");

			            selectfromcode.disabled = true;

			            selecttocode.disabled = true;

			              break;

			        case "tourtodate":

			            var selectfromcode = document.getElementById("<%=txttourfrom.ClientID%>");

			            var selecttocode = document.getElementById("<%=txttourto.ClientID%>");

			            selectfromcode.disabled = false;

			            selecttocode.disabled = false;

			             break;

			    }


			}

   

</script> <table>
        <tr>
            <td style="width:100%">
                <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                    width: 100%; border-bottom: gray 2px solid">
                    <tr>
                        <td class="field_heading" style="text-align: center; width: 100%;">
                            Report - Excursion  Report</td>
                    </tr>
                   
                    <tr>
                        <td style="width: 100%; height: 375px;">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY>
                      


      <table>
      <!-- maingroup -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel4" runat="server"  Width="528px" 
            GroupingText="Main Group" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbmaingpall" onclick ="radbtn_onclick('frommaingpcode');" runat="server" checked name="controlmaingrp" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlFrommaingpcode" 
                onchange ="CallWebMethod('frommaingpcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlFrommaingpName" onchange ="CallWebMethod('frommaingpname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbmaingprange" onclick ="radbtn_onclick('tomaingpcode');" runat="server" name="controlmaingrp" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddlTomaingpcode" onchange ="CallWebMethod('tomaingpcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlTomaingpname" onchange ="CallWebMethod('tomaingpname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- excgroup -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel7" runat="server"  Width="528px" 
            GroupingText="Excursion Group" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbexcgpall" onclick ="radbtn_onclick('fromexcgpcode');" runat="server" checked name="controlexcgroup" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromexcgpcode" 
                onchange ="CallWebMethod('fromexcgpcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromexcgpname" onchange =" CallWebMethod('fromexcgpname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbexcgprange" onclick ="radbtn_onclick('toexcgpcode');" runat="server" name="controlexcgroup" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltoexcgpcode" onchange =" CallWebMethod('toexcgpcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltoexcgpname" onchange =" CallWebMethod('toexcgpname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>

    

    <table>
      <!-- City -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel3" runat="server"  Width="528px" 
            GroupingText="City" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radcityall" onclick ="radbtn_onclick('fromcitycode');" runat="server" checked name="controlcity" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromcitycode" 
                onchange =" CallWebMethod('fromcitycode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromcityname" onchange =" CallWebMethod('fromcityname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radcityrange" onclick ="radbtn_onclick('tocitycode');" runat="server" name="controlcity" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltocitycode" onchange =" CallWebMethod('tocitycode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltocityname" onchange =" CallWebMethod('tocityname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- Exc Provider -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel5" runat="server"  Width="528px" 
            GroupingText="Excursion Provider" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radexcproviderall" onclick ="radbtn_onclick('fromexcprovider');" runat="server" checked name="controlexcprovider" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromexcprovider" 
                onchange =" CallWebMethod('fromexcprovider');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromexcprovidername" onclick =" CallWebMethod('fromexcprovidername');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radexcproviderto" onclick ="radbtn_onclick('toexcprovider');" runat="server" name="controlexcprovider" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltoexcprovider" onclick =" CallWebMethod('toexcprovider');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddltoexcprovidername" onchange =" CallWebMethod('toexcprovidername');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>

   <table>
      <!-- hotel -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel6" runat="server"  Width="528px" 
            GroupingText="Hotel" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radhotelall" onclick ="radbtn_onclick('fromhotelcode');" runat="server" checked name="controlhotel" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromhotelcode" 
                onchange =" CallWebMethod('fromhotelcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromhotelname" onchange =" CallWebMethod('fromhotelname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radhotelrange" onclick ="radbtn_onclick('tohotelcode');" runat="server" name="controlhotel" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltohotelcode" onchange =" CallWebMethod('tohotelcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltohotelname" onchange =" CallWebMethod('tohotelname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- client -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel8" runat="server"  Width="528px" 
            GroupingText="Client" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radclientall" onclick ="radbtn_onclick('fromclientcode');" runat="server" checked name="controlclient" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromclientcode" 
                onchange =" CallWebMethod('fromclientcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromclientname" onchange =" CallWebMethod('fromclientname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radclientrange" onclick ="radbtn_onclick('toclientcode');" runat="server" name="controlclient" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltoclientcode" onchange =" CallWebMethod('toclientcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddltoclientname" onchange =" CallWebMethod('toclientname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>
    

     <table>
      <!-- Payment Terms -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel1" runat="server"  Width="528px" 
            GroupingText="Payment Terms" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radpaytermsall" onclick ="radbtn_onclick('frompaytermscode');" runat="server" checked name="controlpayterms" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfrompaytermscode" 
                onchange =" CallWebMethod('frompaytermscode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfrompaytermsname" onchange =" CallWebMethod('frompaytermsname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radpaytermsrange" onclick ="radbtn_onclick('topaytermscode');" runat="server" name="controlpayterms" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltopaytermscode" onchange =" CallWebMethod('topaytermscode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddltopaytermsname" onchange =" CallWebMethod('topaytermsname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- Service Provider -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel2" runat="server"  Width="528px" 
            GroupingText="Service Provider" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radspall" onclick ="radbtn_onclick('fromsp');" runat="server" checked name="controlsp" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromsp" 
                onchange =" CallWebMethod('fromsp');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddlfromspname" onclick =" CallWebMethod('fromspname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radsprange" onclick ="radbtn_onclick('tosp');" runat="server" name="controlsp" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltosp" onclick =" CallWebMethod('tosp');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltospname" onchange =" CallWebMethod('tospname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>


    <table>
      <!-- Driver -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel9" runat="server"  Width="528px" 
            GroupingText="Driver" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="raddall" onclick ="radbtn_onclick('fromdrivercode');" runat="server" checked name="controldriver" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromdcode" 
                onchange =" CallWebMethod('fromdrivercode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddlfromdname" onchange =" CallWebMethod('fromdrivername');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="raddrange" onclick ="radbtn_onclick('todrivercode');" runat="server" name="controldriver" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltodcode" onchange =" CallWebMethod('todrivercode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltodname" onchange =" CallWebMethod('todrivername');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- Office Sales -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel10" runat="server"  Width="528px" 
            GroupingText="Office Sales" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radsalesall" onclick ="radbtn_onclick('fromsalescode');" runat="server" checked name="controlsales" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromsalescode" 
                onchange =" CallWebMethod('fromsalescode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromsalesname" onclick =" CallWebMethod('fromsalesname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radsalesrange" onclick ="radbtn_onclick('tosalescode');" runat="server" name="controlsales" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltosalescode" onclick =" CallWebMethod('tosalescode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltosalesname" onchange =" CallWebMethod('tosalesname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>

   <table>
      <!-- Sales Person -->
      <TR><TD style="WIDTH: 100px">
        <asp:Panel id="Panel11" runat="server"  Width="528px" 
            GroupingText="Sales Person" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radsapall" onclick ="radbtn_onclick('fromsapcode');" runat="server" checked name="controlsales1" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromsapcode" 
                onchange =" CallWebMethod('fromsapcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromsapname" onchange =" CallWebMethod('fromsapname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radsaprange" onclick ="radbtn_onclick('tosapcode');" runat="server" name="controlsales1" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltosapcode" onchange =" CallWebMethod('tosapcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltosapname" onchange =" CallWebMethod('tosapname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

            <!-- Collectedby -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel12" runat="server"  Width="528px" 
            GroupingText="Collected By" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radcolall" onclick ="radbtn_onclick('fromcollectcode');" runat="server" checked name="controlcollect" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromcolcode" 
                onchange =" CallWebMethod('fromcollectcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromcolname" onclick =" CallWebMethod('fromcollectname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radcolrange" onclick ="radbtn_onclick('tocollectcode');" runat="server" name="controlcollect" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltocolcode" onclick =" CallWebMethod('tocollectcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltocolname" onchange =" CallWebMethod('tocollectname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
            </TR>
   </table>

   <table>
     
      <TR>
      
       <!-- Market -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel14" runat="server"  Width="528px" 
            GroupingText="Market" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radmarketall" onclick ="radbtn_onclick('frommarketcode');" runat="server" checked name="controlmarket" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfrommarketcode" 
                onchange =" CallWebMethod('frommarketcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfrommarketname" onclick =" CallWebMethod('frommarketname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radmarketrange" onclick ="radbtn_onclick('tomarketcode');" runat="server" name="controlmarket" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltomarketcode" onclick =" CallWebMethod('tomarketcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltomarketname" onchange =" CallWebMethod('tomarketname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
           <!-- Operator -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel13" runat="server"  Width="528px" 
            GroupingText="Operator" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="radoperatorall" onclick ="radbtn_onclick('fromoperatorcode');" runat="server" checked name="controloperator" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromoperatorcode" 
                onchange =" CallWebMethod('fromoperatorcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell"><select 
            id="ddlfromoperatorname" onclick =" CallWebMethod('fromoperatorname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="radoperatorrange" onclick ="radbtn_onclick('tooperatorcode');" runat="server" name="controloperator" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltooperatorcode" onclick =" CallWebMethod('tooperatorcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltooperatorname" onchange =" CallWebMethod('tooperatorname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
            
          
     

           
            </TR>
   </table>

   <table>
     
      <TR>
      
              <!-- requestdate -->
              <TD style="WIDTH: 100px">
        <asp:Panel id="Panel15" runat="server"  Width="528px" 
            GroupingText="Request Date" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell" style="width: 247px">
            <input id="radreqall" onclick ="radbtn_onclick('requestfromdate');" runat="server" checked name="controlrequestdate" 
            type="radio" /> All</td>
            <TD><asp:Label id="Label3" runat="server" Text="From" CssClass="td_cell"></asp:Label></TD>
    <TD><asp:TextBox id="txtreqfrom" runat="server" CssClass="fiel_input" Width="80px" 
            AutoCompleteType="Cellular"></asp:TextBox><asp:ImageButton id="ImgBtnFrmDt1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator3" runat="server" CssClass="field_input" Display="Dynamic" ControlToValidate="txtreqfrom" ControlExtender="MaskedEditExtender3" InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
        </tr>
        <tr>
            <td class="td_cell" style="width: 247px">
            <input id="radreqrange" onclick ="radbtn_onclick('requesttodate');" runat="server" name="controlrequestdate" type="radio" /> Range</td>
            <TD><asp:Label id="Label4"  runat="server" Text="To" CssClass="td_cell"></asp:Label></TD>
     <TD><asp:TextBox id="txtreqto"   runat="server" CssClass="fiel_input" 
             Width="80px"></asp:TextBox><asp:ImageButton id="ImgBtnToDate1"  runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton><cc1:MaskedEditValidator id="MaskedEditValidator4"  runat="server" CssClass="field_error" Display="Dynamic" ControlToValidate="txtreqto" ControlExtender="MaskedEditExtender4" InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator></TD>
         </tr></TBODY></TABLE></asp:Panel></TD>

          
            <!-- Tour Date -->
            <td style="WIDTH: 100px">
                <asp:Panel ID="Panel16" runat="server" Font-Names="Arial" Font-Size="12px" 
                    GroupingText="Tour Date" Width="528px">
                    <table style="WIDTH: 524px">
                        <tbody>
                            <tr>
                                <td class="td_cell" style="width: 247px">
                                    <input id="radtourall" onclick ="radbtn_onclick('tourfromdate');" runat="server" checked name="controltourdate" 
            type="radio" />
                                    All</td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" CssClass="td_cell" Text="From"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttourfrom" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                    <asp:ImageButton ID="ImgBtnFrmDt2" runat="server" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator5" runat="server" 
                                        ControlExtender="MaskedEditExtender5" ControlToValidate="txttourfrom" 
                                        CssClass="field_input" Display="Dynamic" 
                                        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a date in dd/mm/yyyy format">
                                    </cc1:MaskedEditValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell" style="width: 247px">
                                    <input id="radtourrange" onclick ="radbtn_onclick('tourtodate');" runat="server" name="controltourdate" type="radio" />
                                    Range</td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="td_cell" Text="To"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttourto" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                    <asp:ImageButton ID="ImgBtnToDate2" runat="server" 
                                        ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator6" runat="server" 
                                        ControlExtender="MaskedEditExtender6" ControlToValidate="txttourto" 
                                        CssClass="field_error" Display="Dynamic" 
                                        EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                                        InvalidValueBlurredMessage=" Invalid Date" InvalidValueMessage="Invalid Date" 
                                        TooltipMessage="Input a date in dd/mm/yyyy format">
                                    </cc1:MaskedEditValidator>
                                </td>
                            </tr>

                           
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
      
   
          
            </TR>
   </table>

   <table>
   <tr>
      <!-- Exctype -->
      <TD style="WIDTH: 100px">
        <asp:Panel id="Panel17" runat="server"  Width="528px" 
            GroupingText="Exc Type" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbexctypeall" onclick ="loadgridexc('all');" runat="server" checked name="controlexctype" 
            type="radio" /> All</td>
            <td style="WIDTH: 100px">
            <asp:TextBox ID="txtexctype" ReadOnly ="true"  runat="server"></asp:TextBox> </td>

            <td> <asp:Button id="btnloadgrid" tabIndex=15 runat="server"  Text="Load Grid" CssClass="field_button" 
                            onclick="Btnloadgrid_Click"></asp:Button></td>
            </tr>
        <tr><td class="td_cell">
            <input id="rbexctyperange" onclick ="loadgridexc('range');" runat="server" name="controlexctype" type="radio" /> Range</td>
          
            </tr></TBODY></TABLE></asp:Panel></TD>


            <!-- tourguide 
                  -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel18" runat="server"  Width="528px" 
            GroupingText="Tour Guide" Font-Names="Arial" Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbtgall" onclick ="radbtn_onclick('fromtgcode');" runat="server" checked name="controltg" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromtgcode" 
                onchange =" CallWebMethod('fromtgcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddlfromtgname" onclick =" CallWebMethod('fromtgname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbtgrange" onclick ="radbtn_onclick('totgcode');" runat="server" name="controltg" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltotgcode" onclick =" CallWebMethod('totgcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltotgname" onchange =" CallWebMethod('totgname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>

   
   </tr>

   <tr >
    
       <!-- selling Code  -->
            <TD style="WIDTH: 100px">
        <asp:Panel id="Panel19" runat="server"  Width="528px" 
            GroupingText="Selling Type Code" Font-Names="Arial"  Font-Size="12px">
            <TABLE style="WIDTH: 524px"><TBODY>
        <tr><td class="td_cell">
            <input id="rbsellall" onclick ="radbtn_onclick('fromsellcode');" runat="server" checked name="controlsellcode" 
            type="radio" /> All</td><td class="td_cell">From </td><td 
            class="td_cell">
            <select id="ddlfromsellcode" 
                onchange =" CallWebMethod('fromsellcode');" runat="server" class="drpdown" 
              style="WIDTH: 139px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddlfromsellname" onclick =" CallWebMethod('fromsellname');" 
                runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr>
        <tr><td class="td_cell">
            <input id="rbsellrange" onclick ="radbtn_onclick('tosellcode');" runat="server" name="controlsellcode" type="radio" /> Range</td><td 
            class="td_cell">To </td><td class="td_cell">
                <select id="ddltosellcode" onclick =" CallWebMethod('tosellcode');"
            runat="server" class="drpdown"  
             style="WIDTH: 140px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td>
            <td class="td_cell"> Name</td><td class="td_cell">
            <select 
            id="ddltosellname" onchange =" CallWebMethod('tosellname');" runat="server" class="drpdown"  
             style="WIDTH: 225px"> <option 
            selected="" value="[Select]">[Select]</option></select> </td></tr></TBODY></TABLE></asp:Panel></TD>
   </tr>


   </table>


       <!-- gridview -->
 
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px"    GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>



<asp:TemplateField Visible="False" HeaderText="Excursion Type">          

<ItemTemplate>
      <asp:Label ID="lblothtypcode" runat="server" Text='<%# Bind("othtypcode") %>'></asp:Label>

    
</ItemTemplate>
</asp:TemplateField>
  <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <input id="chkcode" type="checkbox" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
<asp:BoundField  DataField="othtypcode" SortExpression="othtypcode" HeaderText="Excursion Code">
<ItemStyle HorizontalAlign="Left" CssClass="NoDisplay"  ></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText="Excursion Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>


</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>


</asp:GridView>

 <table>
   
   <tr>
       <td style="height: 27px;">
           <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="6" Text="Save" 
               Width="50px" />
       </td>
      
           <td style="width: 504px; height: 27px;">
               <asp:Button ID="btnCancel" runat="server" CssClass="btn" 
                    tabIndex="7" Text="Cancel" />
                          
               <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                   Text="Webserviceerror"></asp:Label>
           </td>
      
    </tr>
    </table>


    <table>
<tr><td></td></tr>

</table>

   <table>
     
    <tr>
    <TD style="WIDTH: 136px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Group By</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddlgroupby" runat="server" Width="150px" CssClass="drpdown" >
                <asp:ListItem Value="0">None</asp:ListItem>
                <asp:ListItem Value="1">ExclD</asp:ListItem>
                <asp:ListItem Value="2">Excursion Type</asp:ListItem>
                <asp:ListItem Value="3">Hotels</asp:ListItem>
                <asp:ListItem Value="4">Tour Date </asp:ListItem>
                <asp:ListItem Value="5">Clients</asp:ListItem>
                <asp:ListItem Value="6">Sectorwise</asp:ListItem>
                <asp:ListItem Value="7">Driverwise</asp:ListItem>
                <asp:ListItem Value="8">Sellcode</asp:ListItem>
                <asp:ListItem Value="9">Provider</asp:ListItem>
                <asp:ListItem Value="10">Monthwise</asp:ListItem>
               </asp:DropDownList>
        </TD>

        <TD style="WIDTH: 132px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Language</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddllang" runat="server" Width="150px" CssClass="drpdown" >
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">English</asp:ListItem>
                <asp:ListItem Value="2">German</asp:ListItem>
                <asp:ListItem Value="3">Russian</asp:ListItem>
                <asp:ListItem Value="4">Italy</asp:ListItem>
                <asp:ListItem Value="5">French</asp:ListItem>
                <asp:ListItem Value="6">Spanish</asp:ListItem>
               
               </asp:DropDownList>
        </TD>
     </tr> 

     <tr>
    <TD style="WIDTH: 136px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">Report Type</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddlreptype" runat="server" Width="150px" CssClass="drpdown" >
                <asp:ListItem Value="0">Normal</asp:ListItem>
                <asp:ListItem Value="1">Witout Rate</asp:ListItem>
                <asp:ListItem Value="2">Cost Not Entered</asp:ListItem>
                <asp:ListItem Value="3">Excursion Details</asp:ListItem>
                </asp:DropDownList>
        </TD>

        <TD style="WIDTH: 132px" class="td_cell" align=left><SPAN style="FONT-FAMILY: Arial">D.M.C</SPAN> <SPAN style="COLOR: #ff0000"></SPAN></TD>
        <td><asp:DropDownList id="ddldmc" runat="server" Width="150px" CssClass="drpdown" >
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="2">Others</asp:ListItem>
                               
               </asp:DropDownList>
        </TD>
     </tr> 
     <tr>
     <td align="left" class="td_cell" style="WIDTH: 136px">
                   <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                   <INPUT id="chkcost"  tabIndex=19  type=checkbox CHECKED 
           runat="server" />
           
<asp:Label ID='lblactive' Text='Show Cost' runat="server" ></asp:Label></td>
<td style="WIDTH: 145px"> </td>
     
     <td align="left" class="td_cell" style="WIDTH: 132px">
                   <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                   <INPUT id="chkcommission"  tabIndex=19  type=checkbox CHECKED 
           runat="server" />
                   
<asp:Label ID='Label1' Text='Show Commission'  runat="server" ></asp:Label></td>
     </tr>

     <tr>
      <td align="left" class="td_cell" style="WIDTH: 136px">
      <asp:Label ID='Label2' Text='Trf.Required' runat="server" ></asp:Label>
                   <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                   <INPUT id="chktrfreq"  tabIndex=19  type=checkbox CHECKED 
           runat="server" />
        </td>
     </tr>

      <tr>
      <td align="left" class="td_cell" style="WIDTH: 136px">
      <asp:Label ID='Label7' Text='Send Email' runat="server" ></asp:Label>
                  
                 
        </td>
         <td align="left" class="td_cell" style="WIDTH: 156px">
        <asp:TextBox ID="txtemail"   runat="server" Width="320px" TextMode="MultiLine"></asp:TextBox>
        </td>
        <td>
        <asp:Button id="btnemail" tabIndex=13 runat="server" Text="Email to Send" 
                CssClass="field_button" Width="115px"></asp:Button>
        </td>
     </tr>

    </table>  

    <table>
      <TR><TD>&nbsp;</TD><TD colSpan=3>&nbsp;</TD></TR>
      </table>

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
               <asp:Button id="btnadd" tabIndex=16  runat="server"   CssClass="field_button"></asp:Button>
               <asp:Button id="Button1" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
       
   </td>



        </table>

        <TR><TD>&nbsp;</TD><TD colSpan=3>&nbsp;</TD></TR>
                <TR><TD colSpan=4><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
 <asp:Label id="lblMsg" runat="server" 
            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE> 

<cc1:CalendarExtender id="CalendarExtender3" runat="server" TargetControlID="txtreqfrom" PopupButtonID="ImgBtnFrmDt1" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MaskedEditExtender3" runat="server" TargetControlID="txtreqfrom" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CalendarExtender4" runat="server" TargetControlID="txtreqto" PopupButtonID="ImgBtnToDate1" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MaskedEditExtender4" runat="server" TargetControlID="txtreqto" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
<cc1:CalendarExtender id="CalendarExtender5" runat="server" TargetControlID="txttourfrom" PopupButtonID="ImgBtnFrmDt2" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MaskedEditExtender5" runat="server" TargetControlID="txttourfrom" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender> <cc1:CalendarExtender id="CalendarExtender6" runat="server" TargetControlID="txttourto" PopupButtonID="ImgBtnToDate2" Format="dd/MM/yyyy"></cc1:CalendarExtender> <cc1:MaskedEditExtender id="MaskedEditExtender6" runat="server" TargetControlID="txttourto" Mask="99/99/9999" MaskType="Date" Enabled="True"></cc1:MaskedEditExtender>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                </table>
                </td>
        </tr>
    </table>
    <table>
     <tr>
         <td>
         <asp:HiddenField id="hdnfromclientcode" runat="server"></asp:HiddenField>
         <asp:HiddenField id="hdntoclientcode" runat="server"></asp:HiddenField>
          <asp:HiddenField ID="hdnfromclientname" runat="server" />
           <asp:HiddenField ID="hdntoclientname" runat="server" />
         </td>
         </tr>
    </table>

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>

     <%--<asp:HiddenField ID="hdncategoryname" runat="server"/>--%>
    

</asp:Content>