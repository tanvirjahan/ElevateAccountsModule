Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class PriceListModule_ContractMainNew
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Dim myDataAdapter As SqlDataAdapter

#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then

            '   PanelMain.Visible = True
            'charcters(txtCode)
            'charcters(txtName)

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))


            'If Session("ContractState") Is Nothing Then
            '    Session("ContractState") = CType(Request.QueryString("State"), String)

            'End If
           


            ' If CType(Session("ContractState"), String) <> "New" Then
            'Session("ContractRefCode") = CType(Request.QueryString("contractid"), String)
            'Session("contractid") = CType(Request.QueryString("contractid"), String)
            'End If

            'txtconnection.Value = Session("dbconnectionName")
            'txtfromDate.Text = Now.ToString("dd/MM/yyyy")
            'txtToDate.Text = Now.ToString("dd/MM/yyyy")
            'hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")

            'ViewState("partycode") = CType(Request.QueryString("partycode"), String)
            'hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            'txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast(nolock) where partycode='" & ViewState("partycode") & "'")
            'txthotelname.Enabled = False

            'If CType(Session("ContractState"), String) = "New" Then
            '    SetFocus(txtName)

            '    Page.Title = Page.Title + " " + "New Contracts"
            '    txtCode.Disabled = True
            '    ' wucCountrygroup.clearsessions()
            '    Session("isAutoTick_wuccountrygroupusercontrol") = 1
            '    wucCountrygroup.sbShowCountry()

            '    txthotelname.Focus()
            '    lblHeading.Text = "Add New Contracts - " + txthotelname.Text


            '    'txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
            '    btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
            '    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")
            'End If
       


        End If


        'btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        'Dim typ As Type
        'typ = GetType(DropDownList)

        'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        'End If
        'Session.Add("submenuuser", "ContractsSearch.aspx")

    End Sub
End Class
