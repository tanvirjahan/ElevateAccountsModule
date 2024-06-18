'------------================--------------=======================------------------================
'   Module Name    :    PackagePerRoom.aspx
'   Developer Name :    Jaffer
'   Date           :    08-06-2011
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
#End Region
Partial Class PriceListModule_ViewCalculation
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim ds As New DataSet
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Dim pkgid As String = ""
            If Request.QueryString("PkgId") Is Nothing = False Then
                pkgid = Request.QueryString("PkgId")
            End If
            'pkgid = "000004"
            'Session.Add("dbconnectionName", "strDBConnection")
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "sp_get_view_packageprices '" + pkgid + "'")
            gv_PkgMain.DataSource = Nothing
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    gv_PkgMain.DataSource = ds.Tables(0)
                End If
            End If
            gv_PkgMain.DataBind()

            gv_Additional.DataSource = Nothing
            If ds.Tables.Count > 2 Then
                If ds.Tables(2).Rows.Count > 0 Then
                    gv_Additional.DataSource = ds.Tables(2)
                End If
            End If
            gv_Additional.DataBind()
        End If
    End Sub

    Protected Sub gv_PkgMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_PkgMain.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblsellcode As Label
            Dim gv_PkgMain As GridView
            lblsellcode = e.Row.FindControl("lblsellcode")
            gv_PkgMain = e.Row.FindControl("gv_PkgMain")
            If ds.Tables.Count > 1 Then
                ds.Tables(1).DefaultView.RowFilter = "sellcode='" + lblsellcode.Text + "'"
            End If
            gv_PkgMain.DataSource = ds.Tables(1).DefaultView
            gv_PkgMain.DataBind()
        End If
    End Sub

    Protected Sub gv_Additional_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Additional.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblsellcode As Label
            Dim gv_AdChild As GridView
            lblsellcode = e.Row.FindControl("lblsellcode")
            gv_AdChild = e.Row.FindControl("gv_AdChild")
            If ds.Tables.Count > 3 Then
                ds.Tables(3).DefaultView.RowFilter = "sellcode='" + lblsellcode.Text + "'"
            End If
            gv_AdChild.DataSource = ds.Tables(3).DefaultView
            gv_AdChild.DataBind()
        End If
    End Sub
End Class
