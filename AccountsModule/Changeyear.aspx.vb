#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_Changeyear
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime
    Dim strSqlQry As String
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
#Region "Global Declarations"
    Dim msealdate As Date
#End Region

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                '----------------------------- Default Dates
                Dim acccode As String
                Dim desc As String
                Dim fdate As String = ""
                Dim endmonth As String
                Dim enddate As String
                Dim yearend As String = ""
                Dim i As Integer


                txtDivCode.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)


                Dim ds As DataSet

                'ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select year(enddate) listyear from closeyear union all select year(getdate()) listyear   order by  listyear")
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select  distinct year(invoicedate) listyear from reservation_invoice_header  order by  listyear")

                If ds.Tables.Count > 0 Then

                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                            For i = 0 To ds.Tables(0).Rows.Count - 1
                                ddlyear.Items.Add(New ListItem(ds.Tables(0).Rows(i)("listyear"), ds.Tables(0).Rows(i)("listyear")))
                            Next
                        End If
                    End If
                End If
                If (Session("changeyear") Is Nothing) Then
                    ddlyear.SelectedValue = Year(Now)
                Else
                    ddlyear.SelectedValue = Session("changeyear")
                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlyear.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("sealdate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub ddlyear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Add("changeyear", ddlyear.SelectedValue)
    End Sub
End Class
