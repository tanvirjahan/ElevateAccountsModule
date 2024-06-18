Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Linq
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class RptSalesRegister
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Web Methods"
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select agentcode,agentname from agentmast where active=1 and agentname like '%" & prefixText & "%' order by agentname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                Next
            End If
            Return customers
        Catch ex As Exception
            Return customers
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSectors(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select sectorcode,sectorname from agent_sectormaster where active=1 and sectorname like '%" & prefixText & "%' order by sectorname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sectorname").ToString(), myDS.Tables(0).Rows(i)("sectorcode").ToString()))
                Next
            End If
            Return customers
        Catch ex As Exception
            Return customers
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetCountry(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctry As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 and  ctryname like '%" & prefixText & "%' order by ctryname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    ctry.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If
            Return ctry
        Catch ex As Exception
            Return ctry
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountryGroup(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim countryGroup As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select countrygroupcode,countrygroupname from countrygroup(nolock) where active=1 and countrygroupname like '%" & prefixText & "%' " &
            "order by countrygroupname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    countryGroup.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("countrygroupname").ToString(), myDS.Tables(0).Rows(i)("countrygroupcode").ToString()))
                Next
            End If
            Return countryGroup
        Catch ex As Exception
            Return countryGroup
        End Try
    End Function

#End Region

#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsPostBack = False Then
            Dim appid As String = ""
            If Request.QueryString("appid") Is Nothing = False Then
                appid = CType(Request.QueryString("appid"), String)
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
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim strappname As String = ""
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
            ViewState("Appname") = strappname
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
            ViewState.Add("divcode", divid)
        End If
    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)

                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\RptSalesRegister.aspx?appid=" + strappid.Trim, btnAddNew, btnExportToExcel, _
                                                       btnLoadReport, gvSearch:=gvSearchResult)
                txtDivcode.Text = ViewState("divcode")

                ddlCurrency.Items.Add(New ListItem("Party Currency", 0))
                ddlCurrency.Items.Add(New ListItem("Base Currency", 1))
                ddlCurrency.SelectedIndex = 0

                'Added by Priyanka 20/01/2020

                ddlRptType.Items.Add(New ListItem("Detail", 0))
                ddlRptType.Items.Add(New ListItem("Summary", 1))
                ddlRptType.SelectedIndex = 0

                ddlRptOrder.Items.Add(New ListItem("Invoice No", 0))
                ddlRptOrder.Items.Add(New ListItem("Invoice Date", 1))
                ddlRptOrder.Items.Add(New ListItem("Agent", 2))
                ddlRptOrder.Items.Add(New ListItem("Sector", 3))
                ddlRptOrder.Items.Add(New ListItem("Country", 4))

                ddlRptOrder.SelectedIndex = 0

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSalesRegister.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Try
            If (Not IsDate(txtFromDt.Text)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select from date');", True)
                Exit Sub
            End If
            If (Not IsDate(txtToDt.Text)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select to date');", True)
                Exit Sub
            End If
            Dim strfromDate As String = txtFromDt.Text
            Dim strToDate As String = txtToDt.Text
            Dim divid As String = txtDivcode.Text
            Dim Agent As String = txtCustCode.Text
            Dim currType As Integer = Convert.ToInt32(ddlCurrency.SelectedValue)
            Dim reportsType As String = "excel"
            Dim Sector As String = txtSectorCode.Text
            Dim Ctry As String = txtCtryCode.Text
            Dim detailsummary As String = Convert.ToInt32(ddlRptType.SelectedValue)
            Dim reportorder As String = Convert.ToInt32(ddlRptOrder.SelectedValue)
            Dim bytes() As Byte
            bytes = {}
            Dim objSalesReg As clsSalesRegister = New clsSalesRegister()
            '  objSalesReg.GenerateReport(reportsType, strfromDate, strToDate, divid, Agent, currType, bytes, "download")
            'changed by Priyanka on 21/01/2020 to add summary report
            objSalesReg.GenerateReport(reportsType, strfromDate, strToDate, divid, Agent, currType, Sector, Ctry, detailsummary, reportorder, bytes, "download")
            'GenerateReport(reportsType, strfromDate, strToDate, divid, Agent, currType, bytes, "download")
            Response.Clear()
            Response.Buffer = True
            Dim filename As String = ""
            If detailsummary = "0" Then
                filename = "SalesRegister@"
            Else
                filename = "SalesRegisterSummary@"
            End If
            Dim FileNameNew As String = filename + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xlsx"
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", bytes.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.BinaryWrite(bytes)
            Response.Cookies.Add(New HttpCookie("DownloadSalesReg", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        Catch ex As Exception


            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSalesRegister.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

End Class
