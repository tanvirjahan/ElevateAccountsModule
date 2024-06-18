Imports System.Data
Imports system.Data.SqlClient
Imports System.Net.Mail

Partial Class Login
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objEmail As New clsEmail
#End Region

#Region "Protected Sub btnLogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogIn.Click"
    Protected Sub btnLogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        Try

            clsDBConnect.webdb = ddlDbName.Items(ddlDbName.SelectedIndex).Value
            Session.Add("dbconnectionName", ddlDbName.Items(ddlDbName.SelectedIndex).Value)
            Session.Add("dbconnectionName1", "strDBConnection1")
            If objUser.ValidateUser(Session("dbconnectionName"), txtUserName.Text.Trim, txtPassword.Text.Trim) = True Then
                Session.Add("GlobalUserName", txtUserName.Text.Trim)
                Session.Add("Userpwd", txtPassword.Text.Trim)
                Session.Add("CompanyName", CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 conm from columbusmaster"), String))
                Session.Add("changeyear", Now.Year.ToString)
                ''          

                If chkRemember.Checked Then
                    addcookie()
                End If



                FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim, False)
                
                Response.Redirect("ModuleMainPage.aspx", False)

            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The username or password you entered is incorrect');", True)
                'objUtils.MessageBox("Please enter valid user name and password.", Me.Page)

                txtUserName.Text = ""
                txtPassword.Text = ""
                ' objUtils.MessageBox("Please enter valid user name and password.", Me.Page)
                ' SetFocus(txtUserName)

                'Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")
                'objUtils.MessageBox(ClientIP, Me.Page)
            End If

            ''Starts the changes for the crystal report according to the database selection
            If txtUserName.Text.Trim <> "" And txtPassword.Text.Trim <> "" Then
                Dim ds1 As DataSet
                Dim cmd1 As SqlCommand
                Dim dbSqlReader As SqlDataReader
                Dim scon1 As New SqlConnection(ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString)

                If scon1.State = Data.ConnectionState.Closed Then
                    scon1.Open()
                End If

                cmd1 = New SqlCommand("select top 1 username,password,servername,databasename from rpts_crystal_connection where conn_str='" & clsDBConnect.webdb & "'", scon1)
                dbSqlReader = cmd1.ExecuteReader(CommandBehavior.CloseConnection)
                cmd1 = Nothing

                If dbSqlReader.HasRows Then
                    If dbSqlReader.Read() = True Then

                        If IsDBNull(dbSqlReader("username")) = False Then
                            Session.Add("dbUserName", CType(dbSqlReader("username"), String))
                        End If

                        If IsDBNull(dbSqlReader("password")) = False Then
                            Session.Add("dbPassword", CType(dbSqlReader("password"), String))
                        End If

                        If IsDBNull(dbSqlReader("servername")) = False Then
                            Session.Add("dbServerName", CType(dbSqlReader("servername"), String))
                        End If



                        If IsDBNull(dbSqlReader("databasename")) = False Then
                            Session.Add("dbDatabaseName", CType(dbSqlReader("databasename"), String))
                        End If


                    End If
                End If

                ds1 = Nothing
                scon1.Close()
            End If

            'If txtUserName.Text.Trim = "admin" And txtPassword.Text.Trim = "admin" Then
            '    Session.Add("GlobalUserName", "Admin")
            '    Response.Redirect("ModuleMainPage.aspx", False)
            'Else
            '    txtUserName.Text = ""
            '    txtPassword.Text = ""
            '    objUtils.MessageBox("Please enter valid user name and password.", Me.Page)
            '    SetFocus(txtUserName)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Login.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

    Protected Sub addcookie()

        If Not (Request.Cookies("user") Is Nothing) Then

            Dim cookie As New HttpCookie("user")
            cookie.Values.Add("userId", txtUserName.Text)
            cookie.Expires = DateTime.Now.AddMonths(1)
            Response.Cookies.Add(cookie)


        Else
            If Response.Cookies("user").Item("userId") <> txtUserName.Text Then

                Response.Cookies.Remove("user")

                Dim cookie As New HttpCookie("user")
                cookie.Values.Add("userId", txtUserName.Text)
                cookie.Expires = DateTime.Now.AddMonths(1)
                Response.Cookies.Add(cookie)

            End If
        End If


    End Sub
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("DBName") = ""
        'Session("Userpwd") = ""

        ' FormsAuthentication.SignOut()
        If Page.IsPostBack = False Then
            Session.Abandon()

            SetFocus(txtUserName)

            If Not (Request.Cookies("user") Is Nothing) Then

                txtUserName.Text = Request.Cookies("user").Item("userId")

            End If



            Dim ds As DataSet
            Dim cmd As SqlCommand
            Dim da As SqlDataAdapter
            ds = New DataSet
            Dim scon As New SqlConnection(ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString)

            cmd = New SqlCommand("select top 1 table_desc,conn_str from rpts_logindb order by orderby", scon)
            da = New SqlDataAdapter(cmd)

            da.Fill(ds)
            cmd = Nothing

            ddlDbName.DataSource = ds
            ddlDbName.DataTextField = "table_desc"
            ddlDbName.DataValueField = "conn_str"
            ddlDbName.DataBind()

            ds = Nothing
            da = Nothing
            scon.Close()
            If Page.IsPostBack = False Then
                Session.Add("dbConnectionName", "strDBConnection")
                SetFocus(txtUserName)
                If Not (Request.Cookies("user") Is Nothing) Then
                    txtUserName.Text = Request.Cookies("user").Item("userId")
                End If

                clsDBConnect.webdb = Session("dbConnectionName") '"strDBConnection"
            End If
        End If

    End Sub
#End Region

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim mysqlreader As SqlDataReader
        Dim fromMail As String = ""
        Dim toMail As String = ""
        Dim strcust As String = ""
        Dim subject As String = ""

        'clsDBConnect.webdb = "strDBConnection"
        Session("dbconnectionName") = "strDBConnection"
        Try
            If txtFUserName.Text <> "" Then
                mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode,username,dbo.pwddecript(userpwd) userpwd ,usemail from usermaster where active=1 and  usercode='" & txtFUserName.Text & "'")

                If Not mysqlreader.HasRows Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid username.');", True)
                    '        '   
                    txtFUserName.Text = ""

                    Exit Sub
                End If


                mysqlreader.Read()


                If mysqlreader.Item("usemail") = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email Address  unavailable.');", True)

                    Exit Sub

                End If

                fromMail = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =1070")
                'objEmail.SendEmail(txtFrom.Value.Trim, txtTo.Value.Trim, txtSubject.Value.Trim, txtbody.Text.Trim)
                toMail = mysqlreader.Item("usemail")


                strcust = "<table style='font-family: Verdana'><tr><td > Dear " & mysqlreader.Item("username") & ", <br/><br/> Please  note  your following  account Info: "
                strcust += "<br/><br/>Username:" & mysqlreader.Item("usercode") & "<br/> "
                strcust += "Password:" & mysqlreader.Item("userpwd") & "<br/><br/>"
                strcust += " <br /><br />Thanks and Best Regards<br /><br /> </td></tr></table>"
                subject = "Password Recovery"
                If fromMail <> "" And toMail <> "" Then
                    If objEmail.SendEmailCC(fromMail, toMail, "", subject, strcust) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email has been successfully sent');", True)
                        txtFUserName.Text = ""
                    End If
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter the username');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try



    End Sub

    Protected Sub btnChangePwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePwd.Click
        Dim mysqlreader As SqlDataReader

        Session("dbconnectionName") = "strDBConnection"
        Try
            If txtUserName.Text <> "" Then
                mysqlreader = objUtils.GetDataFromReadernew(Session("dbconnectionName"), "Select usercode from usermaster where active=1 and  usercode='" & txtUserName.Text & "'")

                If Not mysqlreader.HasRows Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid username.');", True)
                    '        '   
                    txtUserName.Text = ""

                    Exit Sub
                End If

                'redirect to pwd change page
                Dim strpop As String = ""
                strpop = "window.open('ChangeUserPassword.aspx?UserCode=" & txtUserName.Text & "','ChangePassword','  left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Username.');", True)
                SetFocus(txtUserName.ClientID)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub
    
End Class
