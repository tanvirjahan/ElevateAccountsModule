Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Net.Mail
Imports System.Net
Imports System.Collections.Generic

Partial Class AgentModule_Registrationform
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Public dt1 As DataTable
    Public dt2 As DataTable
    Public dt3 As DataTable
    Dim dsdata As DataSet
    Dim objEmail As New clsEmail
    Dim requestid As String = ""
    Dim strMessage As String = ""
    Dim strSubject As String = ""
    Dim strSqlQry As String
    Dim mySqlReader As SqlDataReader
#End Region




#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Page.IsPostBack = False Then

                txtconnection.Value = "strDBConnection"

                visibility()
                'objUtils.FillDropDownListHTMLNEW(txtconnection.Value, ddlCountry, "ctrycode", "ctryname", "select  ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(txtconnection.Value, ddlCountryName, "ctryname", "ctrycode", "select  ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)
                ' '' ddlCountry.Value = objUtils.ExecuteQueryReturnStringValuenew(txtconnection.Value, "select option_selected from reservation_parameters where param_id='459'")
                'objUtils.FillDropDownListHTMLNEW(txtconnection.Value, ddlCity, "citycode", "cityname", "select citycode,cityname from citymast where active='1' and ctrycode='" + ddlCountry.Value + "'order by cityname", True)
                'objUtils.FillDropDownListHTMLNEW(txtconnection.Value, ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active='1' and ctrycode='" + ddlCountryName.Value + "'order by cityname", True)

                FillDDL()
                If CType(Session("CustState"), String) = "View" Then
                    If CType(Request.QueryString("regno"), String) = "" = False Then
                        txtregno.Value = CType(Request.QueryString("regno"), String)
                    End If
                    ShowRecord()
                End If

                dissabled()
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Else
                'objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountry, "ctrycode", "ctryname", "select  ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True, ddlCountry.Value)
                'objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountryName, "ctryname", "ctrycode", "select  ctryname,ctrycode from ctrymast where active=1 order by ctryname", True, ddlCountryName.Value)
                ' '' ddlCountry.Value = objUtils.ExecuteQueryReturnStringValuenew(txtconnection.Value, "select option_selected from reservation_parameters where param_id='459'")
                'objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCity, "citycode", "cityname", "select citycode,cityname from citymast where active='1' and ctrycode='" + ddlCountry.Value + "'order by cityname", True, ddlCity.Value)
                'objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active='1' and ctrycode='" + ddlCountry.Value + "'order by cityname", True, ddlCityName.Value)
                FillDDLPostBack()
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Registrationform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalAgentUserName"))
        End Try
    End Sub
   
#End Region

#Region "Private Sub FillDDL()"
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountry, "ctrycode", "ctryname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select ctryname,ctrycode from ctrymast where active=1 order by ctryname"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountryName, "ctryname", "ctrycode", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select citycode,cityname from citymast where active=1  order by citycode"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCity, "citycode", "cityname", strSqlQry, True)
        strSqlQry = ""
        strSqlQry = "select cityname,citycode from citymast where active=1  order by cityname"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCityName, "cityname", "citycode", strSqlQry, True)
    End Sub
#End Region

    Protected Sub FillDDLPostBack()
        strSqlQry = ""
        strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountry, "ctrycode", "ctryname", strSqlQry, True, ddlCountry.Value)
        strSqlQry = ""
        strSqlQry = "select ctryname,ctrycode from ctrymast where active=1 order by ctryname"
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCountryName, "ctryname", "ctrycode", strSqlQry, True, ddlCountryName.Value)
        If ddlCountry.Value <> "[Select]" Then
            strSqlQry = "select citycode,cityname from citymast where active=1 and ctrycode='" & ddlCountryName.Value & "' order by citycode"
        Else
            strSqlQry = "select citycode,cityname from citymast where active=1  order by citycode"
        End If
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCity, "citycode", "cityname", strSqlQry, True, ddlCity.Value)
        strSqlQry = ""
        If ddlCountryName.Value <> "[Select]" Then
            strSqlQry = "select cityname,citycode from citymast where active=1 and ctrycode='" & ddlCountryName.Value & "' order by cityname"
        Else
            strSqlQry = "select cityname,citycode from citymast where active=1  order by cityname"
        End If
        objUtils.FillDropDownListHTMLNEW("strDBConnection", ddlCityName, "cityname", "citycode", strSqlQry, True, ddlCityName.Value)

    End Sub


#Region "Public Function visibility()"
    Public Function visibility()
        lbluserid.Style("visibility") = "hidden"
        lblfname.Style("visibility") = "hidden"
        lbllname.Style("visibility") = "hidden"
        lbldesignation.Style("visibility") = "hidden"
        lblcompany.Style("visibility") = "hidden"
        lbladd1.Style("visibility") = "hidden"
        lblctry.Style("visibility") = "hidden"
        lblcity.Style("visibility") = "hidden"
        lblphone1.Style("visibility") = "hidden"
        lbladd1.Style("visibility") = "hidden"
        lblemail.Style("visibility") = "hidden"

        visibility = ""
    End Function
#End Region


#Region "Public Function dissabled()"
    Public Function dissabled()
        txtregno.Disabled = True
        txtuserid.Disabled = True
        txtPassword.Enabled = False
        txtfname.Disabled = True
        txtlname.Disabled = True
        txtdesignation.Disabled = True
        txtcompany.Disabled = True
        txtiata.Disabled = True
        txtadd1.Disabled = True
        txtadd2.Disabled = True
        txtadd3.Disabled = True
        ddlCountryName.Disabled = True
        ddlCityName.Disabled = True
        txtphone1.Disabled = True
        txtphone2.Disabled = True
        txtfax.Disabled = True
        txtemail.Disabled = True
        txtweb.Disabled = True
        btnsave.Visible = False
        dissabled = ""
    End Function
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region




    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then

                If checkForDuplicate() = False Then
                    Exit Sub
                End If

                mySqlConn = clsDBConnect.dbConnectionnew("strDBConnection")           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                mySqlCmd = New SqlCommand("sp_registration", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure

                If (txtuserid.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(txtuserid.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = String.Empty
                End If
                If (txtPassword.Text = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = CType(txtPassword.Text.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@webpassword", SqlDbType.VarChar, 10)).Value = String.Empty
                End If
                If (txtfname.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = CType(txtfname.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact1", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtlname.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = CType(txtlname.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@contact2", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtdesignation.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@designation", SqlDbType.VarChar, 100)).Value = CType(txtdesignation.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@designation", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtcompany.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 100)).Value = CType(txtcompany.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtiata.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@iatano", SqlDbType.VarChar, 100)).Value = CType(txtiata.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@iatano", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtadd1.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 100)).Value = CType(txtadd1.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtadd2.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = CType(txtadd2.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtadd3.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = CType(txtadd3.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If CType(ddlCountry.Items(ddlCountry.SelectedIndex).Text, String) = "" Or CType(ddlCountry.Items(ddlCountry.SelectedIndex).Text, String) <> "[Select]" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(ddlCountry.Items(ddlCountry.SelectedIndex).Text, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = String.Empty
                End If
                If CType(ddlCity.Items(ddlCity.SelectedIndex).Text, String) = "" Or CType(ddlCity.Items(ddlCity.SelectedIndex).Text, String) <> "[Select]" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(ddlCity.Items(ddlCity.SelectedIndex).Text, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = String.Empty
                End If
                If (txtphone1.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = CType(txtphone1.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel1", SqlDbType.VarChar, 50)).Value = String.Empty
                End If
                If (txtphone2.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = CType(txtphone2.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@tel2", SqlDbType.VarChar, 50)).Value = String.Empty
                End If
                If (txtfax.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = CType(txtfax.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@fax", SqlDbType.VarChar, 50)).Value = String.Empty
                End If
                If (txtemail.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = CType(txtemail.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100)).Value = String.Empty
                End If
                If (txtweb.Value = "") = False Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.VarChar, 100)).Value = CType(txtweb.Value.Trim, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.VarChar, 100)).Value = String.Empty
                End If


                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                sendMail()


                btnsave.Style("visibility") = "hidden"
                ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Registered Successfully.');", True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Registrationform.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim strValue As String = ""

        strValue = objUtils.ExecuteQueryReturnStringValuenew("strDBConnection", "select top 1 't' from registration where  agentname='" + txtcompany.Value + "' and webusername='" + txtuserid.Value + "'")
        If strValue <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('User Id already registered for this Company');", True)
            checkForDuplicate = False
            Exit Function
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "Public Function sendMail()"
    Public Function sendMail()
        Dim mysqlreader As SqlDataReader
        Dim cmd3 As SqlCommand
        Dim toadd As String
        Dim strmsg As String
        If txtemail.Value <> "" Then
            mysqlreader = Nothing

            Dim scon3 As New SqlConnection(ConfigurationManager.ConnectionStrings("strDBConnection").ConnectionString)

            If scon3.State = Data.ConnectionState.Closed Then
                scon3.Open()
            End If


            cmd3 = New SqlCommand("Select option_selected from reservation_parameters where param_id =547   ", scon3)
            mysqlreader = cmd3.ExecuteReader(CommandBehavior.CloseConnection)
            cmd3 = Nothing


            If Not mysqlreader.HasRows Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter a valid username.');", True)
                sendMail = ""
                Exit Function
            End If

            mysqlreader.Read()

            If mysqlreader.Item("option_selected") = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email address unavailable.');", True)
                sendMail = ""
                Exit Function
            Else
                toadd = mysqlreader.Item("option_selected") + "," + txtemail.Value
            End If


            Dim message As String = "  <table><tr><td style='width: 100%; font-family: Verdana; height: 50px' valign='top'>"
            message += "Dear Mr. " + txtfname.Value.Trim + " , </td></tr><tr>"
            message += "<td style='width: 100%; font-family: Verdana; height:50px' valign='top'> "
            message += "Your Request has been registered We will proceed shortly.</td></tr><tr><td style='width: 100%; font-family: Verdana; height: 50px' valign='top'>"
            message += "Thanks and best regards</td><tr><tr>"
            message += "<td style='width: 100%; font-family: Verdana; height:50px' valign='top'> "
            message += " </td></tr></table>"



            'strmsg = "<html>Dear Mr." & txtfname.Value.Trim & ", </br>"
            'strmsg += "</br></br> Your Request has been regigtered We will proceed shortly.</br>"
            'strmsg += "</br></br> Thanks & Regards"


            'objEmail.SendEmail(txtFrom.Value.Trim, txtTo.Value.Trim, txtSubject.Value.Trim, txtbody.Text.Trim)
            Dim from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
            If objEmail.SendEmail(from_email, toadd, "Client Registration", message) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Registered Sucessfully  ');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Register');", True)
            End If

            scon3.Close()

        End If
        sendMail = ""
    End Function
#End Region

#Region "Private Sub ShowRecord()"
    Private Sub ShowRecord()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from registration where regno='" + CType(Request.QueryString("regno"), String) + "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("webusername")) = False Then
                        Me.txtuserid.Value = mySqlReader("webusername")
                    End If
                    If IsDBNull(mySqlReader("webpassword")) = False Then
                        Me.txtPassword.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwddecript('" & mySqlReader("webpassword") & "')")
                    End If
                    If IsDBNull(mySqlReader("contact1")) = False Then
                        Me.txtfname.Value = mySqlReader("contact1")
                    End If
                    If IsDBNull(mySqlReader("contact2")) = False Then
                        Me.txtlname.Value = mySqlReader("contact2")
                    End If
                    If IsDBNull(mySqlReader("designation")) = False Then
                        Me.txtdesignation.Value = mySqlReader("designation")
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtcompany.Value = mySqlReader("agentname")
                    End If
                    If IsDBNull(mySqlReader("iatano")) = False Then
                        Me.txtiata.Value = mySqlReader("iatano")
                    End If
                    If IsDBNull(mySqlReader("add1")) = False Then
                        Me.txtadd1.Value = mySqlReader("add1")
                    End If
                    If IsDBNull(mySqlReader("add2")) = False Then
                        Me.txtadd2.Value = mySqlReader("add2")
                    End If
                    If IsDBNull(mySqlReader("add3")) = False Then
                        Me.txtadd3.Value = mySqlReader("add3")
                    End If
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        ddlCountryName.Value = mySqlReader("ctrycode")
                        ddlCountry.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                    End If
                    strSqlQry = ""
                    strSqlQry = "select citycode,cityname from citymast where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'  order by citycode"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", strSqlQry, True)
                    strSqlQry = ""
                    strSqlQry = "select cityname,citycode from citymast where active=1  and ctrycode='" & mySqlReader("ctrycode") & "'  order by cityname"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", strSqlQry, True)
                    If IsDBNull(mySqlReader("citycode")) = False Then
                        ddlCityName.Value = mySqlReader("citycode")
                        ddlCity.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                    End If
                    If IsDBNull(mySqlReader("tel1")) = False Then
                        Me.txtphone1.Value = mySqlReader("tel1")
                    End If
                    If IsDBNull(mySqlReader("tel2")) = False Then
                        Me.txtphone2.Value = mySqlReader("tel2")
                    End If
                    If IsDBNull(mySqlReader("fax")) = False Then
                        Me.txtfax.Value = mySqlReader("fax")
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        Me.txtemail.Value = mySqlReader("email")
                    End If
                    If IsDBNull(mySqlReader("web")) = False Then
                        Me.txtweb.Value = mySqlReader("web")
                    End If


                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustMainDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
End Class
