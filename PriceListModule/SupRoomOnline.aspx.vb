
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class PriceListModule_SupRoomOnline
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Dim objdatetime As New clsDateTime
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim ObjDate As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As New SqlDataAdapter
    Dim ddlPcode As DropDownList
    Dim strWhereCond As String
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then

            If Session("SupState") = "New" Then
                Response.Redirect("SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)

                Exit Sub
            End If

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If


            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)



            PanelVisit.Visible = True
            charcters(txtCode)
            charcters(txtName)
            If CType(Session("SupState"), String) = "New" Then
                SetFocus(txtCode)
                lblHeading.Text = "Add New Supplier Room Types Online"
            ElseIf CType(Session("SupState"), String) = "Edit" Then

                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                fillgrid(RefCode)

                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "Edit Supplier Room Types Online"
            ElseIf CType(Session("SupState"), String) = "View" Then

                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                fillgrid(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier Room Types Online"
                BtnVisitCancel.Text = "Return to Search"
                BtnVisitCancel.Focus()

            ElseIf CType(Session("SupState"), String) = "Delete" Then

                RefCode = CType(Session("SupRefCode"), String)
                ShowRecord(RefCode)
                fillgrid(RefCode)
                txtCode.Disabled = True
                txtName.Disabled = True
                lblHeading.Text = "Delete Supplier Room Types Online"
            End If
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If

        Session.Add("submenuuser", "SupplierSearch.aspx")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RoomOnlineWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If


    End Sub
#End Region

#Region " Protected Sub BtnVisitCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnVisitCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("CustomersSearch.aspx")

        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

#Region "Public Function Dateformat()"
    Public Function Dateformat(ByVal obj As Object) As String
        If Not (obj Is Nothing) Then
            Return Format(CType(obj.ToString(), Date), "dd/MM/yyyy")
        Else
            Return ""
        End If
    End Function
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("custstate") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentcode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("custstate") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from partymast Where partycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        Me.txtCode.Value = mySqlReader("partycode")
                    End If
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        Me.txtName.Value = mySqlReader("partyname")
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustVisitFollUp.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
    Private Sub fillgrid(ByVal RefCode As String)
        Dim myDS As New DataSet

        gv_VisitFollow.Visible = True

        If gv_VisitFollow.PageIndex < 0 Then
            gv_VisitFollow.PageIndex = 0
        End If
        strSqlQry = ""
        'strSqlQry = "select  visitid,visitdate,remarks,cperson,reqaction from agentmast_visit where agentcode='" & RefCode & "' and spersoncode='" & CType(Session("GlobalUserName"), String) & "' "
        strSqlQry = "select po.rmtypcode,r.rmtypname, po.roomdescription from Party_Room_Details po,partyrmtyp pm,rmtypmast r  where po.rmtypcode = pm.rmtypcode and po.partycode=pm.partycode and pm.rmtypcode = r.rmtypcode and pm.partycode='" & RefCode & "'"

        'If Trim(BuildCondition) <> "" Then
        '    strSqlQry = strSqlQry & BuildCondition()
        strSqlQry = strSqlQry & " order by po.rmtypcode desc"
        'Else
        'strSqlQry = strSqlQry & " order by po.rmtypcode desc"
        'End If

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(myDS)
        gv_VisitFollow.DataSource = myDS

        If myDS.Tables(0).Rows.Count > 0 Then
            gv_VisitFollow.DataBind()
        Else
            gv_VisitFollow.PageIndex = 0
            gv_VisitFollow.DataBind()
        End If


    End Sub
#End Region
#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If txtvisitid.Value.Trim <> "" Then
            strWhereCond = " and agentmast_visit.visitid LIKE '" & Trim(txtvisitid.Value.Trim) & "%'"
        End If


        If txtPfromdate.Text <> "" And txtPtodate.Text <> "" Then
            strWhereCond = " and convert(varchar(10),agentmast_visit.visitdate,111) between convert(varchar(10),'" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) " & _
                           " and convert(varchar(10),'" & Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") & "',111) "
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        btnVisitFollo.Enabled = False
        gv_VisitFollow.Enabled = False

    End Sub
#End Region

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)

    End Sub
#End Region

#Region "Protected Sub btnVisitFollo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnVisitFollow.Click"
    Protected Sub btnVisitFollo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVisitFollo.Click 'Handles btnVisitFollo.Click
        Dim strpop As String = ""
        Dim mode As String
        mode = "New"
        strpop = "window.open('RoomOnline.aspx?State=" + mode + "&partycode=" + txtCode.Value + "&partyname=" + txtName.Value + "','RoomOnline','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        'AddLinesVisit()
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustVisitFollUp','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub gv_VisitFollow_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        '  FillGrid(e.SortExpression, direction)
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()

    End Sub
#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        fillgrid(txtCode.Value)
    End Sub
#End Region

    Protected Sub gv_VisitFollow_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Try
            Dim lblvisitid As Label
            Dim rmtypcode As String
            lblvisitid = gv_VisitFollow.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblvisitid")
            rmtypcode = Replace(lblvisitid.Text.Trim.ToString, "+", "$")
            If e.CommandName = "Editrow" Then
                Dim strpop As String = ""
                strpop = "window.open('RoomOnline.aspx?State=Edit&RefCode=" + CType(rmtypcode, String) + "&partycode=" + txtCode.Value + "&partyname=" + txtName.Value + "','HeaderInfo','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                strpop = "window.open('RoomOnline.aspx?State=View&RefCode=" + CType(rmtypcode, String) + "&partycode=" + txtCode.Value + "&partyname=" + txtName.Value + "','HeaderInfo','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Deleterow" Then
                Dim strpop As String = ""
                strpop = "window.open('RoomOnline.aspx?State=Delete&RefCode=" + CType(rmtypcode, String) + "&partycode=" + txtCode.Value + "&partyname=" + txtName.Value + "','HeaderInfo','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupRoomOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'If ValidatePage() = False Then
        '    Exit Sub
        'End If

        fillgrid(CType(Session("SupRefCode"), String))
    End Sub

    Public Function ValidatePage() As Boolean
        Dim myfdate As Date
        Dim mytdate As Date
        If txtPfromdate.Text <> "" And txtPtodate.Text <> "" Then
            myfdate = txtPfromdate.Text
            mytdate = txtPtodate.Text

            If CType(objdatetime.ConvertDateromTextBoxToDatabase(txtPfromdate.Text), Date) > CType(objdatetime.ConvertDateromTextBoxToDatabase(txtPtodate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtPtodate.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If
        End If

        ValidatePage = True
    End Function
End Class
