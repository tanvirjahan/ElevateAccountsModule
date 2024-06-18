<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reservation_costamend.aspx.vb" Inherits="AgentsOnline_Reservation_costamend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Price Calculation</title>
    <link rel="shortcut icon" href="images/favicon.ico"/>
<link href="css/allStyles.css" rel="stylesheet" type="text/css" />

<script src="../TADDScript.js" type="text/javascript"></script>
<link rel="shortcut icon" href="images/favicon.ico"/>
<link href="../css/Styles.css" rel="stylesheet" type="text/css" />
<link href='http://fonts.googleapis.com/css?family=Cuprum' rel='stylesheet' type='text/css'/>

    <style type="text/css">
            

.field_heading_agent
{
	font-family: Arial,Verdana, Geneva, ms sans serif;
	font-size: 10pt;
	font-weight: bold;
	background-color: #06788B;
	color:  #ffffff;
}


.grdstyle
{
	border: 1px none #999999;
	font-family: Verdana, Arial, Geneva, ms sans serif;
	font-size :10px;
	background :White;
	color:#DDD9CF;
	font-weight: normal;
	margin-top: 0px;
}
.grdheader
{
	font-family: Verdana, Arial, Geneva, ms sans serif;
	font-size :10px;
	font-weight:bold;
	background-color:#06788B; 	
	color :White;

}
.grdRowstyle
{
	font-family: Verdana, Arial, Geneva, ms sans serif;
	background:white;
	font-weight:normal;
	color :black;
	Font-Size:10px;
}
.Label_heading
{
	font-family: Arial,Verdana, Geneva, ms sans serif;
	font-size: 8pt;
	font-weight: bold;
	color: #ffffff;
}


.grdAternaterow
{
	font-family: Verdana, Arial, Geneva, ms sans serif;
	Font-Size:10px;
	font-weight:normal;
	background:#DDD9CF;
	color :black;
	
}
      </style>

<script >

    function Changecost(costvalue) {
        var costval = document.getElementById(costvalue);
        alert(costval.value);
    }

    function btnclick(obj) {
        obj1 = document.getElementById(obj);
        obj1.click();
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
    <div>
                   <tr>
                              <td align="left"  bgcolor="Black"> 
                                   <asp:Label ID="lblroomtype" runat="server" CssClass="field_heading_agent"  Font-Bold="True"  
                                       Text="Change Reservation Cost"  Width="100%"></asp:Label>
                               </td>
                           </tr>


                                        <asp:GridView ID="gvRoomDetails" runat="server" CssClass="grdstyle"                  
             AutoGenerateColumns="False"   Width="850px"> 
          
                                            <RowStyle CssClass="grdRowstyle" ForeColor="Black" Wrap="False" />
                                            <Columns>
                                                <asp:TemplateField >
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="left" style="width: 100px; height: 15px; background-color: #AAc6c6; color: maroon;">
                                                                    <asp:Label ID="Label6" runat="server" Text="Room Type Code" ForeColor="Maroon" 
                                                                        CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="width: 150px; height: 15px; background-color: #AAc6c6;">
                                                                    <asp:Label ID="Label7" runat="server" Text="RoomType Name" Width="200px" 
                                                                        ForeColor="Maroon" CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="width: 100px; height: 15px; background-color: #AAc6c6;">
                                                                    <asp:Label ID="Label8" runat="server" Text="Meal Code" ForeColor="Maroon" 
                                                                        CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="width: 101px; height: 15px; background-color: #AAc6c6;">
                                                                    <asp:Label ID="Label3" runat="server" Text="Category" ForeColor="Maroon" 
                                                                        CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="width: 100px; height: 15px; background-color: #AAc6c6;">
                                                                    <asp:Label ID="Label5" runat="server" Text="No of Rooms" ForeColor="Maroon" 
                                                                        CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="height: 25px; background-color: #AAc6c6; width: 50;">
                                                                    <asp:Label ID="lblcvalue" runat="server" Text="CostValue" Width="75px" 
                                                                        ForeColor="Maroon" CssClass="Label_heading"></asp:Label></td>
                                                                <td align="left" style="width: 55px; height: 15px; background-color: #AAc6c6">
                                                                    <asp:Label ID="lblsvalue" runat="server" Text="SaleValue" Width="75px" 
                                                                        ForeColor="Maroon" CssClass="Label_heading"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 70px">
                                                                    <asp:Label ID="lblroomtype" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "rmtypcode") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 100px">
                                                                    <asp:Label ID="lblrmtypname" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "rmtypname") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 100px">
                                                                    <asp:Label ID="lblMCode" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "mealcode") %>'></asp:Label></td>
                                                                <td align="left" style="width: 101px">
                                                                    <asp:Label ID="lblCat" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "rmcatcode") %>'></asp:Label>
                                                                <td align="left" style="width: 100px">
                                                                    <asp:Label ID="lblNoroom" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "units") %>' Width="50px"></asp:Label>
                                                                    </td>
                                                                <td align="left" style="width: 101px">
                                                                    <asp:Label ID="lblcostvalue" runat="server" 
                                                                        Text='<%# DataBinder.Eval (Container.DataItem, "totsalevalue") %>' Width="50px"></asp:Label>
                                                                    </td>
                                                                 <td align="left" style="width: 100px">
                                                                     <asp:Label ID="lblsalevalue" runat="server" 
                                                                         Text='<%# DataBinder.Eval (Container.DataItem, "totcostvalue") %>' Width="50px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            <tr>
                                                                <td align="left" style="width: 70px">
                                                                    &nbsp;</td>
                                                                <td colspan="7">
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <div align="left">
                                                            <asp:GridView ID="gvPrice" runat="server" AutoGenerateColumns="False" 
                                                                CssClass="td_cell" Width="800px" onrowdatabound="gvPrice_RowDataBound">
                                                                <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                                                <HeaderStyle CssClass="grdheader" Wrap="False" />
                                                                <RowStyle CssClass="grdRowstyle" Wrap="False" />
                                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Valid From">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblValidform" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "fromdate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" Width="75px" />
                                                                        <ItemStyle Width="75px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Valid To">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblValidto" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "todate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="75px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Price">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpricenight" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "price") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cost Price">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtcostpricenight" runat="server" CssClass="txtbox" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "cprice") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Nights">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblnights" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "nights") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Free">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFree" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "freenights") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sale Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblvalue" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "value") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cost Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCostvalue" runat="server" 
                                                                                Text='<%# DataBinder.Eval (Container.DataItem, "costvalue") %>'></asp:Label>
                                                                            <asp:Button ID="btncostclick" runat="server" onclick="btncostclick_Click"  CssClass="field_button" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="td_cell" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle  CssClass="grdheader" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="grdpagerstyle" Wrap="False" />
                                            <HeaderStyle CssClass="grdheader" Wrap="False" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>

    <br />
    <br />
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="field_button" />
    </div>
    
    </center>
    </form>
</body>
</html>
