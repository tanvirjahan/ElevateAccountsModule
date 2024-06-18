'------------================--------------=======================------------------================
'   Module Name    :    RptPaymentsRegister.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    01 OCT 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class RptPaymentsRegister
    Inherits System.Web.UI.Page


#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCD, "acctcode", "acctname", "select acctcode,acctname from acctmast where bankyn='Y'  order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccNM, "acctname", "acctcode", "select acctcode,acctname from acctmast where bankyn='Y'  order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoAccCD, "acctcode", "acctname", "select acctcode,acctname from acctmast where bankyn='Y'  order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoAccNM, "acctname", "acctcode", "select acctcode,acctname from acctmast where bankyn='Y'  order by acctname", True)
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlAccCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                rdbtnAll.Attributes.Add("onclick", "AllRange('" & rdbtnAll.ClientID & "','A','AC')")
                rdbtnRange.Attributes.Add("onclick", "AllRange('" & rdbtnRange.ClientID & "','R','AC')")


                txtFromDate.Text = ObjDate.GetSystemDateTime("dbconnectionName").Day & "/" & ObjDate.GetSystemDateTime("dbconnectionName").Month & "/" & ObjDate.GetSystemDateTime("dbconnectionName").Year
                txtToDate.Text = ObjDate.GetSystemDateTime("dbconnectionName").Day & "/" & ObjDate.GetSystemDateTime("dbconnectionName").Month & "/" & ObjDate.GetSystemDateTime("dbconnectionName").Year
                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                checkrb_status()
                SetFocus(ddlAccCD)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Public Sub checkrb_status()"
    Public Sub checkrb_status()
        If rdbtnAll.Checked = True Then
            ddlAccCD.Disabled = True
            ddlAccNM.Disabled = True
            ddltoAccCD.Disabled = True
            ddltoAccNM.Disabled = True
        ElseIf rdbtnRange.Checked = True Then
            ddlAccCD.Disabled = False
            ddlAccNM.Disabled = False
            ddltoAccCD.Disabled = False
            ddltoAccNM.Disabled = False
        End If
    End Sub
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptPaymentsRegister','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
