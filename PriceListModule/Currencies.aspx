<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Currencies.aspx.vb"  Inherits="Currencies"  MasterPageFile="~/SubPageMaster.master" Strict ="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">
    //function checkNumber(e)
    //			{	    
    //			    	
    ////				if ( (event.keyCode < 47 || event.keyCode > 57) )
    ////				{
    ////					return false;
    ////	            }   
    //         return true;
    //      }
    // 
    function checkNumber(evt) {
        evt = (evt) ? evt : event;
        var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
        if (charCode != 47 && (charCode > 45 && charCode < 58)) {
            //alert("Enter numerals only in this field. "+ charCode);
            return true;
        }
        return false;
    }

    function compulsaryCode() {
        if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
            alert("Code feild can not be blank");
            document.getElementById("<%=txtCode.ClientID%>").focus();
            //                 txtCode.focus();
            return false;
        }
        else {
            document.getElementById("<%=txtName.ClientID%>").focus();
        }
    }
    function compulsaryName() {
        if (document.getElementById("<%=txtName.ClientID%>").value == "") {
            alert("Name feild can not be blank");
            document.getElementById("<%=txtName.ClientID%>").focus();
            return false;
        }
        else {
            document.getElementById("<%=txtCoin.ClientID%>").focus();
        }
    }
    function compulsaryCoin() {
        if (document.getElementById("<%=txtCoin.ClientID%>").value == "") {
            alert("Coin feild can not be blank");
            document.getElementById("<%=txtCoin.ClientID%>").focus();
            return false;
        }
        else {
            document.getElementById("<%=txtConversion.ClientID%>").focus();
        }
    }

    function ValidationForExchate() {
        if (document.getElementById("<%=txtConversion.ClientID%>").value <= 0) {

            alert("Conversion To kuwaiti Dinar must be greater than zero.");
            document.getElementById("<%=txtConversion.ClientID%>").focus();
            return false;
        }
    }


    function FormValidation(state) {
        var lblcur = document.getElementById("<%=lblcurr.ClientID%>");

        //    if ((document.getElementById("<%=txtCode.ClientID%>").value=="") || (document.getElementById("<%=txtName.ClientID%>").value=="") ||(document.getElementById("<%=txtCoin.ClientID%>").value=="")||(document.getElementById("<%=txtConversion.ClientID%>").value!=""))
        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=txtCoin.ClientID%>").value == "") || (document.getElementById("<%=txtConversion.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field  can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field  can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtCoin.ClientID%>").value == "") {
                document.getElementById("<%=txtCoin.ClientID%>").focus();
                alert("Coin field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtConversion.ClientID%>").value == "") {
                document.getElementById("<%=txtConversion.ClientID%>").focus();
                alert("Conversion To  " + lblcur.innerHTML + "  field can not be blank");
                return false;
            }
            //            else if (document.getElementById("<%=txtConversion.ClientID%>").value!="")
            //            {
            //                   if (document.getElementById("<%=txtConversion.ClientID%>").value<=0)
            //                       {
            //                        alert("Conversion To kuwaiti Dinar must be greater than zero.");
            //                        document.getElementById("<%=txtConversion.ClientID%>").focus();
            //                        return false;
            //             
            //             }          }     

        }
        //       else
        //       {       
        //       if (state=='New'){if(confirm('Are you sure you want to save currency Type?')==false)return false;}
        //       if (state=='Edit'){if(confirm('Are you sure you want to update currency Type?')==false)return false;}
        //       if (state=='Delete'){if(confirm('Are you sure you want to delete currency Type?')==false)return false;}
        //       }
        else {

            if (document.getElementById("<%=txtConversion.ClientID%>").value <= 0) {
                alert("Conversion To  " + lblcur.innerHTML + "  must be greater than zero.");
                document.getElementById("<%=txtConversion.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=txtCode.ClientID%>").value != "")
            //               if (document.getElementById("<%=txtCode.ClientID%>").value.match(!/^[a-z]+$/));
            {
                var val = document.getElementById("<%=txtCode.ClientID%>").value;

                if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
                    alert('Only alphabets,digits,-,_,/ are allowed');
                    return false
                }


                return true;
            }

            if (state == 'New') { if (confirm('Are you sure you want to save currency Type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update currency Type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete currency Type?') == false) return false; }
        }


    }



    function chgValue() {
        var lbl = document.getElementById("<%=lblText.ClientID%>");
        lbl.innerHTML = '1 ' + document.getElementById("<%=txtCode.ClientID%>").value + '=';

    }



    function CallWebMethod(methodType) {

        switch (methodType) {
            case "webcurrcode":
                var select = document.getElementById("<%=ddlwebcurrency.ClientID%>");
                var selectname = document.getElementById("<%=ddlwebCurrencyName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;


                document.getElementById("<%=currhdn.ClientID%>").value = select.value;

                var currhdn = document.getElementById("<%=currhdn.ClientID%>");
                currhdn.value = selectname.value;

                break;

            case "webcurrname":
                var select = document.getElementById("<%=ddlwebCurrencyName.ClientID%>");
                var selectname = document.getElementById("<%=ddlwebcurrency.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;

                var currhdn = document.getElementById("<%=currhdn.ClientID%>");
                currhdn.value = select.value;

                break;
        }
    }


</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">
<TBODY><TR><TD class="td_cell" align=center colSpan=3><asp:Label id="lblHeading" runat="server" Text="Add New Currency" __designer:wfdid="w14" Width="560px" CssClass="field_heading"></asp:Label></TD></TR><TR>
    <TD style="WIDTH: 114px" class="td_cell"><SPAN style="COLOR: black">Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; width: 177px;">
    <INPUT onblur="chgValue()" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
    <td style="COLOR: #000000; width: 194px;">
        &nbsp;</td>
    </TR><TR>
    <TD style="WIDTH: 114px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD style="width: 177px">
        <INPUT id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /> </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR><TR>
    <TD style="WIDTH: 114px; HEIGHT: 24px" class="td_cell">Coin <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD style="width: 177px">
        <INPUT style="TEXT-ALIGN: left" id="txtCoin" class="field_input" tabIndex=3 type=text maxLength=10 runat="server" /> </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR><TR>
    <TD class="td_cell" style="width: 114px">
    Conv. To  <asp:Label id="lblText" runat="server" Text=" " __designer:wfdid="w1"></asp:Label></TD>
        <TD style="width: 177px">
            <INPUT style="TEXT-ALIGN: right" id="txtConversion" class="field_input" tabIndex=4 onkeypress="return checkNumber(event)" type=text maxLength=31 runat="server" />
        <asp:Label ID="lblcurr" runat="server" Text=" "></asp:Label>
    </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR>
    <tr>
      <TD class="td_cell" style="width: 114px">
    Invoice Print. To  <asp:Label id="lblTextinv" runat="server" Text=" " __designer:wfdid="w1"></asp:Label></TD>
     <TD style="width: 177px">
            <INPUT style="TEXT-ALIGN: right" id="txtinvconversion" class="field_input" tabIndex=4 onkeypress="return checkNumber(event)" type=text maxLength=31 runat="server" />
        <asp:Label ID="lblcurrinv" runat="server" Text=" "></asp:Label>
    </TD>
    </tr>
    
    <TR><TD class="td_cell" style="width: 114px">
        &nbsp;</TD><TD style="width: 177px">
            <select id="ddlwebcurrency" runat="server" class="field_input" name="D1" 
                onchange="CallWebMethod('webcurrcode')" style="WIDTH: 102px" tabindex="12" 
                visible="False">
                <option selected=""></option>
            </select>
        </TD>
        <td style="width: 194px">
            <select id="ddlwebCurrencyName" runat="server" class="field_input" name="D2" 
                onchange="CallWebMethod('webcurrname')" style="WIDTH: 270px" tabindex="13" 
                visible="False">
                <option selected=""></option>
            </select>
        </td>
    </TR><TR>
    <TD class="td_cell" style="width: 114px; ">
        <asp:Label ID="Label1" runat="server" CssClass="td_ce" Text="Active" 
            ViewStateMode="Enabled" Width="44px"></asp:Label>
        </TD>
    <TD style="width: 177px;">
        <INPUT id="chkActive" tabIndex=5 type=checkbox 
                CHECKED runat="server" />
        </TD>
        <td style="width: 194px">
            &nbsp;</td>
    </TR><tr><td class="td_cell" style="width: 114px; height: 23px;">
        <asp:Button ID="btnSave" runat="server" __designer:wfdid="w15" CssClass="btn" 
            tabIndex="6" Text="Save" />
        </td>
        <td style="height: 23px; width: 177px;">
            <asp:Button ID="btnCancel" runat="server" __designer:wfdid="w16" CssClass="btn" 
                tabIndex="7" Text="Return to Search" />
            &nbsp;
            </td>
        <td style="height: 23px; ">
            <asp:Button ID="btnHelp" runat="server" __designer:dtid="1688858450198528" 
                __designer:wfdid="w17" CssClass="btn" tabIndex="8" Text="Help" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                Text="Webserviceerror"></asp:Label>
            <asp:HiddenField ID="currhdn" runat="server" />
        </td>
        <td style="width: 194px">
            &nbsp;</td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
 