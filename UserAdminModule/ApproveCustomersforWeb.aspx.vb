'------------================--------------=======================------------------================
'   Module Name    :    ApproveCustomersforWeb.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
#End Region

Partial Class ApproveCustomersforWeb
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
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
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

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
            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            txtconnection.Value = Session("dbconnectionName")

            Try
                SetFocus(ddlOrderBy)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpcode", "plgrpname", "select * from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select * from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select * from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select * from citymast where active=1 order by cityname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellname", "select * from sellmast where active=1 order by sellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingTypeName, "sellname", "sellcode", "select * from sellmast where active=1 order by sellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", "select * from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", "select * from agentcatmast where active=1 order by agentcatname", True)

                FillGrid(ddlOrderBy.Value)
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        grdUploadClients.Visible = True
        lblMsg.Visible = False

        If grdUploadClients.PageIndex < 0 Then
            grdUploadClients.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "SELECT agentcode,agentname,webemail,webcontact,[webapprove]=case webapprove when 1 then 'Approved' else '' end,webpassword FROM agentmast where active = 1 "


            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            grdUploadClients.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdUploadClients.DataBind()
                Dim GVRow As GridViewRow
                'Dim chkApp As CheckBox
                Dim lblPwd As Label
                For Each GVRow In grdUploadClients.Rows
                    'If GVRow.Cells(7).Text.Trim = "1" Then
                    '    chkApp = GVRow.FindControl("chkApprove")
                    '    chkApp.Checked = True
                    'End If
                    lblPwd = GVRow.FindControl("lblWPassword")
                    If lblPwd.Text = "" Then
                        'Dim str As String

                        myCommand = New SqlCommand("GenerateRandomString", SqlConn)
                        myCommand.CommandType = CommandType.StoredProcedure

                        myCommand.Parameters.Add(New SqlParameter("@useNumbers", SqlDbType.Bit)).Value = 1
                        myCommand.Parameters.Add(New SqlParameter("@useLowerCase", SqlDbType.Bit)).Value = 0
                        myCommand.Parameters.Add(New SqlParameter("@useUpperCase", SqlDbType.Bit)).Value = 1
                        myCommand.Parameters.Add(New SqlParameter("@charactersToUse", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
                        myCommand.Parameters.Add(New SqlParameter("@passwordLength", SqlDbType.SmallInt, 9)).Value = 7

                        Dim param As SqlParameter
                        param = New SqlParameter
                        param.ParameterName = "@password"
                        param.Direction = ParameterDirection.Output
                        param.DbType = DbType.String
                        param.Size = 50
                        myCommand.Parameters.Add(param)
                        myDataAdapter = New SqlDataAdapter(myCommand)
                        myCommand.ExecuteNonQuery()
                        lblPwd.Text = param.Value
                        GVRow.Cells(9).Text = param.Value
                    End If
                Next
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            Else
                grdUploadClients.PageIndex = 0
                grdUploadClients.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Dim strWhereCond As String
        strWhereCond = ""
        If ddlMarket.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            End If
        End If

        If ddlSellingType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCategory.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCountry.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCity.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

    Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid(ddlOrderBy.Value)
    End Sub

    Protected Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strEmailText As String = ""
        Dim strSubject As String = ""
        Dim to_email As String = ""
        Dim flg As Boolean = False
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from email_text", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("emailtext")) = False Then
                        strEmailText = CType(mySqlReader("emailtext"), String)
                    End If
                    If IsDBNull(mySqlReader("subject")) = False Then
                        strSubject = CType(mySqlReader("subject"), String)
                    End If
                End If
            End If
            If strEmailText <> "" And strSubject <> "" Then
                Dim Mail_Message As New MailMessage()
                Dim msClient As New SmtpClient
                For Each GVRow In grdUploadClients.Rows
                    chkApp = GVRow.FindControl("chkSendMail")
                    If chkApp.Checked = True And GVRow.Cells(7).Text.Trim = "Approved" Then
                        to_email = GVRow.Cells(5).Text.Trim
                        Dim from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
                        Dim frmadd As New MailAddress(from_email)
                        msClient.Host = "localhost" '"127.0.0.1"
                        msClient.Port = 25
                        Mail_Message.From = frmadd
                        Mail_Message.To.Add(Trim(to_email))
                        Mail_Message.Subject = strSubject
                        Mail_Message.Body = strEmailText
                        Mail_Message.Priority = MailPriority.Normal
                        Mail_Message.IsBodyHtml = True
                        msClient.Send(Mail_Message)
                        Mail_Message = Nothing
                        to_email = ""
                        flg = True
                    End If
                Next
            End If
            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one approved customer.');", True)
                SetFocus(btnSendMail)
                Exit Sub
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('E-Mail sending successfully.');", True)
                SetFocus(btnSendMail)
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub

    Protected Sub btnSelectforApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        For Each GVRow In grdUploadClients.Rows
            chkApp = GVRow.FindControl("chkApprove")
            chkApp.Checked = True
        Next
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkApp As CheckBox
        Dim flg As Boolean = False
        Try
            For Each GVRow In grdUploadClients.Rows
                chkApp = GVRow.FindControl("chkApprove")
                If chkApp.Checked = True Then
                    If CType(GVRow.Cells(5).Text, String) = "&nbsp;" Or CType(GVRow.Cells(6).Text, String) = "&nbsp;" Or CType(GVRow.Cells(9).Text, String) = "&nbsp;" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact name, email-id and password should not be blank for approve customer.');", True)
                        SetFocus(btnApprove)
                        Exit Sub
                    End If
                End If
            Next
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each GVRow In grdUploadClients.Rows
                chkApp = GVRow.FindControl("chkApprove")
                If chkApp.Checked = True Then
                    myCommand = New SqlCommand("sp_updateweb_agentmast", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure

                    myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 9)).Value = 1
                    myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = CType(GVRow.Cells(9).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(GVRow.Cells(6).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(GVRow.Cells(5).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    myCommand.ExecuteNonQuery()
                    flg = True
                End If
            Next
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)               'sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)              'connection close
            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one customer for approve.');", True)
                SetFocus(btnApprove)
                Exit Sub
            Else
                FillGrid(ddlOrderBy.Value)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected customers are successfully approved for web.');", True)
                SetFocus(btnApprove)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)               'sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub

    Protected Sub btnSelectforRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkRem As CheckBox
        For Each GVRow In grdUploadClients.Rows
            chkRem = GVRow.FindControl("chkRemove")
            chkRem.Checked = True
        Next
    End Sub

    Protected Sub btnDeletefromWeb_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVRow As GridViewRow
        Dim chkRem As CheckBox
        Dim flg As Boolean = False
        Try
            For Each GVRow In grdUploadClients.Rows
                chkRem = GVRow.FindControl("chkRemove")
                If chkRem.Checked = True Then
                    If CType(GVRow.Cells(5).Text, String) = "&nbsp;" Or CType(GVRow.Cells(6).Text, String) = "&nbsp;" Or CType(GVRow.Cells(9).Text, String) = "&nbsp;" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Contact name, email-id and password should not be blank for remove customer.');", True)
                        SetFocus(btnApprove)
                        Exit Sub
                    End If
                End If
            Next
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
            For Each GVRow In grdUploadClients.Rows
                chkRem = GVRow.FindControl("chkRemove")
                If chkRem.Checked = True Then
                    myCommand = New SqlCommand("sp_updateweb_agentmast", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure

                    myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 9)).Value = 0
                    myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(GVRow.Cells(1).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = CType(GVRow.Cells(9).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(GVRow.Cells(6).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(GVRow.Cells(5).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    myCommand.ExecuteNonQuery()
                    
                    flg = True
                End If
            Next
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one customer for remove.');", True)
                SetFocus(btnDeletefromWeb)
                Exit Sub
            Else
                FillGrid(ddlOrderBy.Value)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected customers are successfully removed from web.');", True)
                SetFocus(btnDeletefromWeb)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)               'sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApproveCustomersforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("MainPage.aspx", False)
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = grdUploadClients.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            grdUploadClients.DataSource = dataView
            grdUploadClients.DataBind()
        End If
    End Sub
#End Region

    Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlMarket.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        ddlSellingType.Value = "[Select]"
        ddlSellingTypeName.Value = "[Select]"
        ddlCountry.Value = "[Select]"
        ddlCountryName.Value = "[Select]"
        ddlCity.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        ddlCategory.Value = "[Select]"
        ddlCategoryName.Value = "[Select]"
        FillGrid(ddlOrderBy.Value)
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid(ddlOrderBy.Value)
    End Sub
End Class
