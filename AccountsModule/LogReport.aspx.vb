#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class AccountsModule_LogReport
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


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUser, "UserName", "UserCode", "select UserName, UserCode from UserMaster where active=1 order by UserName", True)


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
        ddlType.Value = "[Select]"
        ddlUser.Value = "[Select]"
        txtdocno.Text = ""

        If txtFromDate.Text = "" Then
            txtFromDate.Text = Format(objectcl.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objectcl.GetSystemDateOnly(Session("dbconnectionName")))
        End If
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Session("ColReportParams") = Nothing
            If ValidatePage() = False Then
                Exit Sub
            End If

           
            Dim strpop As String = ""

            strpop = "window.open('rptReportNew.aspx?Pageame=Logdetails&BackPageName=LogReport.aspx&Tid=" & txtdocno.Text.Trim & "&User=" & ddlUser.Value & "&Ttype1=" & ddlType.Value & "&Fromdate=" & Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") & "&Todate=" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "','Logdetails','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"



            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception

            objUtils.WritErrorLog("ViewLog.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


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
            objUtils.WritErrorLog("RptBillStatusQuery.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
