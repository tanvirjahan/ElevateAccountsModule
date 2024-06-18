<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Registrationform.aspx.vb" Inherits="AgentModule_Registrationform" Title=":: ::" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

 <script language ="javascript" type="text/javascript" >
 function CallWebMethod(methodType)
 {
   switch(methodType)    
    {
        case "ctrycode":
                var select=document.getElementById("<%=ddlCountry.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCountryName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;

                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value   
                           
            if(codeid!='[Select]')
            {          

                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
            }
            break;
            case "ctryname":
                var select=document.getElementById("<%=ddlCountryName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCountry.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                var connstr=document.getElementById("<%=txtconnection.ClientID%>");  
                constr=connstr.value    
            if(codeid!='[Select]')
            {          

                
                ColServices.clsServices.GetCityCodeListnew(constr,codeid,FillCityCodes,ErrorHandler,TimeOutHandler);
                ColServices.clsServices.GetCityNameListnew(constr,codeid,FillCityNames,ErrorHandler,TimeOutHandler);
            }
                         
                break;
            case "citycode":
                var select=document.getElementById("<%=ddlCity.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCityName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break;
          case "cityname":
                var select=document.getElementById("<%=ddlCityName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCity.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                
                break; 
             
      }
}

function FillCityCodes(result)
    {
    	var ddl = document.getElementById("<%=ddlCity.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
    }

function FillCityNames(result)
    {
    	var ddl = document.getElementById("<%=ddlCityName.ClientID%>");
 		RemoveAll(ddl)
        for(var i=0;i<result.length;i++)
        {
            var option=new Option(result[i].ListText,result[i].ListValue);
            ddl.options.add(option);
        }
        ddl.value="[Select]";
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



</script>


    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 901px" class="td_cell_web"><TBODY><TR><TD style="TEXT-ALIGN: center" colSpan=4><TABLE style="WIDTH: 908px" class="td_cell_web"><TBODY>
    <tr>
        <td align="left" style="height: 17px; text-align: center">
            <asp:Label ID="lblavailablity" runat="server" CssClass="field_heading"
                Width="100%">Registration Form</asp:Label></td>
    </tr>
    <TR><TD style="HEIGHT: 17px; TEXT-ALIGN: left" align=left><TABLE style="WIDTH: 100%"><TBODY>
        <tr>
            <td class="td_cell" style="width: 6380px; color: #000000">
                Registration no.</td>
            <td class="td_cell" style="width: 466px">
                <input id="txtregno" runat="server" class="txtbox" maxlength="31" type="text" style="width: 228px" /></td>
            <td class="td_cell_web" style="width: 338px">
            </td>
        </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            User Id *</td>
        <td style="width: 466px;" class="td_cell">
            <input id="txtuserid" runat="server" class="txtbox" maxlength="31" type="text" style="width: 228px" />&nbsp;
            </td>
        <td class="td_cell_web" style="width: 338px">
            <asp:Label ID="lbluserid" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            User Password </td>
        <td style="width: 466px; height: 28px" class="td_cell">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="txtbox_agent" TextMode="Password"
                Width="226px"></asp:TextBox></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            First Name *</td>
        <td class="td_cell" style="width: 466px; height: 28px;"><input id="txtfname" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lblfname" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Last Name </td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtlname" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lbllname" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Designation *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtdesignation" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px"  /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lbldesignation" runat="server" Font-Bold="False" Font-Names="Verdana"
                Font-Size="9pt" ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Company Name *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtcompany" runat="server" class="txtbox" maxlength="31" type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lblcompany" runat="server" Font-Bold="False" Font-Names="Verdana"
                Font-Size="9pt" ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            IATA Number&nbsp;</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtiata" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 262px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Address 1 *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtadd1" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lbladd1" runat="server" Font-Bold="False" Font-Names="Verdana"
                Font-Size="9pt" ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; height: 30px; color: #000000;">
            Address 2</td>
        <td class="td_cell" style="width: 466px; height: 30px">
            <input id="txtadd2" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 30px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Address 3</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtadd3" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Country *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <select id="ddlCountryName" runat="server" class="drpdown" onchange="CallWebMethod('ctryname');"
                style="width: 303px" tabindex="1">
            </select>
        </td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lblctry" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; height: 30px; color: #000000;">
            City *</td>
        <td class="td_cell" style="width: 466px; height: 30px">
            <select id="ddlCityName" runat="server" class="drpdown"  onchange="CallWebMethod('cityname')"
                style="width: 303px" tabindex="2">
                <option selected="selected"></option>
            </select>
        </td>
        <td class="td_cell_web" style="width: 338px; height: 30px">
            <asp:Label ID="lblcity" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Phone 1 *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtphone1" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lblphone1" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Phone 2&nbsp;</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtphone2" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; height: 32px; color: #000000;">
            Fax
        </td>
        <td class="td_cell" style="width: 466px; height: 32px">
            <input id="txtfax" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 32px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Email *</td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtemail" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
            <asp:Label ID="lblemail" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                ForeColor="Red" Width="240px"></asp:Label></td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 6380px; color: #000000;">
            Web
        </td>
        <td class="td_cell" style="width: 466px; height: 28px">
            <input id="txtweb" runat="server" class="txtbox" maxlength="31"  type="text" style="width: 519px" /></td>
        <td class="td_cell_web" style="width: 338px; height: 28px">
        </td>
    </tr>
    <tr>
        <td class="td_cell_web" style="width: 6380px; height: 41px;">
                </td>
        <td class="td_cell_web" style="width: 466px; height: 41px;">
            <cc1:CaptchaControl ID="ccJoin" runat="server"   CaptchaBackgroundNoise="none" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"  />
            </td>
        <td class="td_cell_web" style="width: 338px; height: 41px">
            <input id="txtcaptcha" style="width: 197px" type="text" /></td>
    </tr>
</TBODY></TABLE>
    </TD></TR><TR><TD style="height: 13px"><asp:Label id="lblCountry" runat="server" Text=" " Font-Size="8pt" Font-Names="Verdana" ForeColor="Black" Font-Bold="True"></asp:Label>
        </TD></TR></TBODY></TABLE></TD></TR><TR><TD style="WIDTH: 94px">
            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                height: 9px" type="text" /></TD><TD style="WIDTH: 131px"></TD><TD style="WIDTH: 100px"></TD><TD style="WIDTH: 298px"><asp:Button id="btnCancel" runat="server" Text="<< Back" Font-Bold="False" CssClass="btn" Width="69px"></asp:Button>&nbsp;
    <asp:Button ID="btnsave" runat="server" CssClass="btn" Font-Bold="False"
         Text="<< Submit >>" OnClientClick="return validatepage();" 
            OnClick="btnsave_Click" /></TD></TR>
    <tr>
        <td style="width: 94px">
        </td>
        <td style="width: 131px">
            <select id="ddlCity" runat="server" class="drpdown" onchange="CallWebMethod('citycode')"
                style="width: 71px; visibility: hidden;" tabindex="19">
                <option selected="selected"></option>
            </select>
        </td>
        <td style="width: 100px">
            <select id="ddlCountry" runat="server" class="drpdown" onchange="CallWebMethod('ctrycode')"
                style="width: 59px; visibility: hidden;" tabindex="17">
                <option selected="selected"></option>
            </select>
        </td>
        <td style="width: 298px">
        </td>
    </tr>
</TBODY></TABLE>&nbsp; &nbsp; <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> &nbsp;&nbsp;
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>