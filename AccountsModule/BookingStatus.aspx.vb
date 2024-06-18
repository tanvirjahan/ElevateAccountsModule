#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
Imports System.IO
Imports System.Web
Imports System.Net.Mail
Imports System.Web.HttpServerUtility
#End Region

Partial Class BookingStatus
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objectcl As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim SqlConn1 As SqlConnection
    Dim myDataAdapter As SqlDataAdapter
    Dim myDataAdapter1 As SqlDataAdapter
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try

                SetFocus(txtFromDate)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUser, "UserName", "UserCode", "select UserName, UserCode from UserMaster where active=1 order by UserName", True)


                Session.Add("strsortExpression", "tran_id")
                Session.Add("strsortdirection", SortDirection.Ascending)
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = Format(objectcl.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        'ddlType.Value = "[Select]"
        'ddlUser.Value = "[Select]"
        'txtdocno.Text = ""

        If txtFromDate.Text = "" Then
            txtFromDate.Text = Format(objectcl.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        End If
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Try
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(3) As SqlParameter
            If Not (txtFromDate.Text = "") Then
                parm(0) = New SqlParameter("@fromdate", objDateTime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtFromDate.Text, String)))
            Else
                parm(0) = New SqlParameter("@fromdate", "1900/01/01")
            End If
            If Not (txtToDate.Text = "") Then
                parm(1) = New SqlParameter("@todate", objDateTime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtToDate.Text, String)))
            Else
                parm(1) = New SqlParameter("@todate", "1900/01/01")
            End If
            parm(2) = New SqlParameter("@booktype", ddlbooktype.SelectedIndex)
            For i = 0 To 2
                parms.Add(parm(i))
            Next
            Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_booking_status", parms)

            objUtils.ExportToExcel(ds, Response)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' objUtils.WritErrorLog("BookingStatus.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Public Function ValidatePage() As Boolean

        Dim objDateTime As New clsDateTime
        Try
            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If


            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    'SetFocus(txtToDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingStatus.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Try
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If
    End Sub
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
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case 16 'changed by mohamed on 27/08/2018
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub
End Class
