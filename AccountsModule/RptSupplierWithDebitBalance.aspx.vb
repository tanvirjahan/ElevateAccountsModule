'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierWithDebitBalance.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class RptSupplierWithDebitBalance
    Inherits System.Web.UI.Page


#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategorycode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryname, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategorycodeto, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategorynameto, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityname, "cityname", "citycode", "select cityname,citycode from citymast where active=1 order by cityname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitycodeto, "citycode", "cityname", "select citycode,cityname from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCitynameto, "cityname", "citycode", "select cityname,citycode from citymast where active=1 order by cityname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppliercode, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppliername, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppliercodeto, "partycode", "partyname", "select partycode,partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppliernameto, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountrycode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountrycodeto, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountrynameto, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", True)

                ddlSupType.Attributes.Add("onchange", "fillSup('" & ddlSupType.ClientID & "')")

                '----------------------------- Default Dates
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCategorycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategoryname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategorycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategorynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCountrycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountrycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountrynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCitycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCitycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCitynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSuppliercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSuppliername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSuppliercodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSuppliernameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                End If

                rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                rbCityall.Attributes.Add("onclick", "rbevent(this,'" & rbCityrange.ClientID & "','A','City')")
                rbCityrange.Attributes.Add("onclick", "rbevent(this,'" & rbCityall.ClientID & "','R','City')")
                rbSupall.Attributes.Add("onclick", "rbevent(this,'" & rbSuprange.ClientID & "','A','Supplier')")
                rbSuprange.Attributes.Add("onclick", "rbevent(this,'" & rbSupall.ClientID & "','R','Supplier')")
                rbcountryall.Attributes.Add("onclick", "rbevent(this,'" & rbcountryrange.ClientID & "','A','Country')")
                rbcountryrange.Attributes.Add("onclick", "rbevent(this,'" & rbcountryall.ClientID & "','R','Country')")


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog(" ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("MainPage.aspx")
    End Sub



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierwithDebitBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
