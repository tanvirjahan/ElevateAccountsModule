Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Linq

Partial Class RptSuppConsolidateCommInvoice
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
#End Region

#Region "Web Services"
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliernames As New List(Of String)
        Try
            strSqlQry = "select partyname,partycode from partymast where partyname like  '%" & Trim(prefixText) & "%'  "
            strSqlQry = strSqlQry & "  order by partyname"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliernames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return suppliernames
        Catch ex As Exception
            Return suppliernames
        End Try
    End Function

#End Region

#Region "Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
        ViewState.Add("appid", appid)
    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                '                                       CType(strappname, String), "AccountsModule\SalesInvoiceSearchNew.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                '                                       btnPrint, gvSearch:=gvSalesInvoice, ViewColumnNo:=GridCol.view, PrintColumnNo:=GridCol.print)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SalesInvoiceSearchNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region "Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click"
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Dim fromDate As String = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
        Dim toDate As String = Convert.ToDateTime(txtToDate.Text).ToString("yyyy/MM/dd")
        Dim acctCode As String = txtsuppliercode.Text.Trim
        Dim ScriptStr As String
        ScriptStr = "<script language=""javascript"">var win=window.open('../AccountsModule/TransactionReports.aspx?BackPageName=~\AccountsModule\RptSuppConsolidateCommInvoice.aspx?appid=" & ViewState("appid") & "&printId=suppConsolidateInvoice&acctType=S&acctCode=" & acctCode & "&TranType=IN&divid=" & ViewState("divcode") & "&fromDate=" & fromDate & "&toDate=" & toDate & "');</script>"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "consolidateInvoice", ScriptStr, False)
    End Sub
#End Region

End Class
