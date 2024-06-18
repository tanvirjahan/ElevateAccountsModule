#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
#End Region

Partial Class WebAdminModule_AgentloginInformation
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim sqlTrans As SqlTransaction
    Dim myDS As New DataSet
    Dim objdatetime As New clsDateTime
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
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

            txtconnection.Value = Session("dbconnectionName")


            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If

            Try
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserCode, "AGENT_SUB_CODE", "Sub_User_Name", "select AGENT_SUB_CODE,Sub_User_Name from agents_subusers where AGENTCODE='" + ddlCustName.Value + "' order by AGENT_SUB_CODE ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserName, "Sub_User_Name", "AGENT_SUB_CODE", "select Sub_User_Name,AGENT_SUB_CODE from agents_subusers where AGENTCODE='" + ddlCustName.Value + "' order by AGENT_SUB_CODE ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select * from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select * from citymast where active=1 order by cityname", True)

                myDS = FillGrid(0)
                grdUploadClients.DataSource = myDS
                If myDS.Tables(0).Rows.Count > 0 Then
                    grdUploadClients.DataBind()
                Else
                    grdUploadClients.PageIndex = 0
                    grdUploadClients.DataBind()
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found, Please redefine search criteria."
                End If

                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCustCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCustName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubUserCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubUserName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ApprovedCustomer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else




            ddlCountryName.Value = hdncountry.Value
            ddlCountry.Value = ddlCountryName.Items(ddlCountryName.SelectedIndex).Text

            If ddlCountry.Value <> "[Select]" Or ddlCountry.Value = "" Then


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select  citycode,cityname  from  citymast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' order by citycode", True, ddlCity.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select  citycode,cityname  from  citymast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' order by cityname", True, ddlCityName.Value)
                ddlCityName.Value = hdncity.Value
                ddlCity.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text

            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select  citycode,cityname  from  citymast   where active=1  order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select  citycode,cityname  from  citymast   where active=1 order by cityname", True)


            End If

            If ddlCity.Value <> "[Select]" Or ddlCity.Value = "" Then


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustCode, "agentcode", "agentname", "select  agentcode,agentname  from  agentmast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' and citycode='" & ddlCity.Items(ddlCity.SelectedIndex).Text & "' order by agentcode", True, ddlCustCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustName, "agentname", "agentcode", "select  agentcode,agentname  from  agentmast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' and citycode='" & ddlCity.Items(ddlCity.SelectedIndex).Text & "' order by agentname", True, ddlcustname.Value)
                ddlCustName.Value = hdncustomer.Value
                ddlCustCode.Value = ddlCustName.Items(ddlCustName.SelectedIndex).Text

            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustCode, "agentcode", "agentname", "select  agentcode,agentname  from  agentmast   where active=1  order by agentcode", True, ddlCustCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustName, "agentname", "agentcode", "select  agentcode,agentname  from  agentmast   where active=1  order by agentname", True, ddlCustName.Value)


            End If




            If ddlCustCode.Value <> "[Select]" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserName, "Sub_User_Name", "AGENT_SUB_CODE", "select Sub_User_Name,AGENT_SUB_CODE from agents_subusers where AGENTCODE='" + ddlCustName.Value + "' order by AGENT_SUB_CODE ", True, txtsubuser.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserCode, "AGENT_SUB_CODE", "Sub_User_Name", "select AGENT_SUB_CODE,Sub_User_Name from agents_subusers where AGENTCODE='" + ddlCustName.Value + "' order by AGENT_SUB_CODE ", True, ddlSubUserName.Items(ddlSubUserName.SelectedIndex).Text)
                ddlSubUserName.Value = hdnsubcustomer.Value
                ddlSubUserCode.Value = ddlSubUserName.Items(ddlSubUserName.SelectedIndex).Text

            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserCode, "AGENT_SUB_CODE", "Sub_User_Name", "select AGENT_SUB_CODE,Sub_User_Name from agents_subusers order by AGENT_SUB_CODE ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserName, "Sub_User_Name", "AGENT_SUB_CODE", "select Sub_User_Name,AGENT_SUB_CODE from agents_subusers order by AGENT_SUB_CODE ", True)
            End If

        End If


    End Sub
#End Region

#Region "Private function FillGrid(byval rpttype as integer)"
    Private Function FillGrid(ByVal rpttype As Integer)
        Dim parms As New List(Of SqlParameter)
        Dim parm(7) As SqlParameter
        Dim i As Integer

        grdUploadClients.Visible = True
        lblMsg.Visible = False

        If grdUploadClients.PageIndex < 0 Then
            grdUploadClients.PageIndex = 0
        End If
        strSqlQry = ""

        If Not (ddlCustCode.Items(ddlCustCode.SelectedIndex).Text = "" Or ddlCustCode.Items(ddlCustCode.SelectedIndex).Text = "[Select]") Then
            parm(0) = New SqlParameter("@agentcode", CType(ddlCustCode.Items(ddlCustCode.SelectedIndex).Text, String))
        Else
            parm(0) = New SqlParameter("@agentcode", String.Empty)
        End If

        If Not (ddlSubUserCode.Items(ddlSubUserCode.SelectedIndex).Text = "" Or ddlSubUserCode.Items(ddlSubUserCode.SelectedIndex).Text = "[Select]") Then
            parm(1) = New SqlParameter("@subusercode", CType(ddlSubUserCode.Items(ddlSubUserCode.SelectedIndex).Text, String))
        Else
            parm(1) = New SqlParameter("@subusercode", String.Empty)
        End If

        If Not (ddlCountry.Items(ddlCountry.SelectedIndex).Text = "" Or ddlCountry.Items(ddlCountry.SelectedIndex).Text = "[Select]") Then
            parm(2) = New SqlParameter("@ctrycode", CType(ddlCountry.Items(ddlCountry.SelectedIndex).Text, String))
        Else
            parm(2) = New SqlParameter("@ctrycode", String.Empty)
        End If
        If Not (ddlCity.Items(ddlCity.SelectedIndex).Text = "" Or ddlCity.Items(ddlCity.SelectedIndex).Text = "[Select]") Then
            parm(3) = New SqlParameter("@citycode", CType(ddlCity.Items(ddlCity.SelectedIndex).Text, String))
        Else
            parm(3) = New SqlParameter("@citycode", String.Empty)
        End If

        If txtFromDate.Text <> "" Then
            parm(4) = New SqlParameter("@loginfrmdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtFromDate.Text, String)), String))
        Else
            parm(4) = New SqlParameter("@loginfrmdate", String.Empty)
        End If

        If txtTodate.Text <> "" Then
            parm(5) = New SqlParameter("@logintodate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtTodate.Text, String)), String))
        Else
            parm(5) = New SqlParameter("@logintodate", String.Empty)
        End If

        parm(6) = New SqlParameter("@list", rpttype)

        For i = 0 To 6
            parms.Add(parm(i))
        Next

        myDS = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_login_infomation", parms)

        Return myDS

    End Function
#End Region



#Region "Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ValidatePage() = False Then
            Exit Sub
        End If


        myDS = FillGrid(0)
        grdUploadClients.DataSource = myDS
        If myDS.Tables(0).Rows.Count > 0 Then
            grdUploadClients.DataBind()
        Else
            grdUploadClients.PageIndex = 0
            grdUploadClients.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found, Please redefine search criteria."
        End If

    End Sub
#End Region

    Public Function ValidatePage() As Boolean
        Dim myfdate As Date
        Dim mytdate As Date
        If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
            myfdate = txtFromDate.Text
            mytdate = txtTodate.Text
            myfdate = myfdate.AddDays(31)

            If CType(objdatetime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objdatetime.ConvertDateromTextBoxToDatabase(txtTodate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtTodate.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If


            If CType(mytdate, Date) > CType(myfdate, Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date diffrence in between 31 days only.' );", True)
                ValidatePage = False
                Exit Function
            End If
        End If
        ValidatePage = True
    End Function

#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region


#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet

        myDS = FillGrid(0)
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

#Region "Protected Sub grdUploadClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUploadClients.PageIndexChanging"
    Protected Sub grdUploadClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUploadClients.PageIndexChanging
        grdUploadClients.PageIndex = e.NewPageIndex

        myDS = FillGrid(0)
        grdUploadClients.DataSource = myDS
        If myDS.Tables(0).Rows.Count > 0 Then
            grdUploadClients.DataBind()
        Else
            grdUploadClients.PageIndex = 0
            grdUploadClients.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found, Please redefine search criteria."
        End If

    End Sub
#End Region

#Region "Protected Sub grdUploadClients_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUploadClients.RowCommand"
    Protected Sub grdUploadClients_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUploadClients.RowCommand
        Try
            Dim lblUserName As Label
            Dim lblPassword As Label
            Dim lblName As Label

            lblUserName = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblWebUserName")
            lblPassword = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblWPassword")
            ''        lblName = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblContactPerson")


            If e.CommandName = "Login" Then
                Session.Add("Type", "Main User")
                Session.Add("GlobalAgentUserName", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentcode from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))
                Session.Add("AgentUserpwd", CType(lblPassword.Text.Trim, String))
                Session.Add("Name", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentname from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))

                'Response.Write("<script language='javascript'> nw=window.open('../AgentModule/AgentMainPage.aspx','_blank');</script>")

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../AgentModule/AgentMainPage.aspx','_blank');", True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApprovedCustomer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting"
    Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlCustCode.Value = "[Select]"
        ddlCustName.Value = "[Select]"
        ddlSubUserCode.Value = "[Select]"
        ddlSubUserName.Value = "[Select]"
        ddlCountry.Value = "[Select]"
        ddlCountryName.Value = "[Select]"
        ddlCity.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        txtFromDate.Text = ""
        txtTodate.Text = ""

        myDS = FillGrid(0)
        grdUploadClients.DataSource = myDS
        If myDS.Tables(0).Rows.Count > 0 Then
            grdUploadClients.DataBind()
        Else
            grdUploadClients.PageIndex = 0
            grdUploadClients.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found, Please redefine search criteria."
        End If

    End Sub
#End Region



    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=agentlogin_information','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

#Region "Public Function FormatDate()"
    Public Function FormatDate(ByVal obj As Object) As String
        If Not (obj Is Nothing) Then
            If (obj.ToString() = "") = False Then
                Return CType(obj.ToString(), Date).ToShortDateString()
            End If
        Else
            Return ""
        End If
    End Function
#End Region

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            Dim strcustomer, strsubuser, strfromdate, strtodate As String
            Dim strcitycode, strcountry, strrepfilter As String
            Dim strrpttype As Integer


            strfromdate = ""
            strtodate = ""
            strrepfilter = ""
            If ValidatePage() = False Then
                Exit Sub
            End If
            strcustomer = IIf(UCase(ddlCustCode.Items(ddlCustCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCustCode.Items(ddlCustCode.SelectedIndex).Text, "")
            If strcustomer <> "" Then
                strrepfilter = strrepfilter + ddlCustName.Items(ddlCustName.SelectedIndex).Text
            End If

            strsubuser = IIf(UCase(ddlSubUserCode.Items(ddlSubUserCode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSubUserCode.Items(ddlSubUserCode.SelectedIndex).Text, "")
            If strsubuser <> "" Then
                strrepfilter = strrepfilter + " ; " + ddlSubUserName.Items(ddlSubUserName.SelectedIndex).Text
            End If

            If txtFromDate.Text <> "" Then
                strfromdate = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtFromDate.Text, String)), String)
                strrepfilter = strrepfilter + " ; " + Format(CType(txtFromDate.Text, Date), "dd/MM/yyyy").ToString
            End If
            If txtTodate.Text <> "" Then
                strtodate = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtTodate.Text, String)), String)
                strrepfilter = strrepfilter + " ; " + Format(CType(txtTodate.Text, Date), "dd/MM/yyyy").ToString
            End If

            strcountry = IIf(UCase(ddlCountry.Items(ddlCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountry.Items(ddlCountry.SelectedIndex).Text, "")
            If strcountry <> "" Then
                strrepfilter = strrepfilter + ddlCountryName.Items(ddlCountryName.SelectedIndex).Text
            End If

            strcitycode = IIf(UCase(ddlCity.Items(ddlCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCity.Items(ddlCity.SelectedIndex).Text, "")
            If strcitycode <> "" Then
                strrepfilter = strrepfilter + ddlCityName.Items(ddlCityName.SelectedIndex).Text
            End If



            strrpttype = 1
            ' strpop = "window.open('../WebAdminModule/rptAgentloginInformationReport.aspx?Pageame=Agentlogininfo&BackPageName=AgentloginInformation.aspx&custcode=" & strcustomer & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&subuser=" & strsubuser & "&CtryCode=" & strcountry & "&CityCode=" & strcitycode & "&rpttype=" & strrpttype & "&rptrepfilter=" & strrepfilter & "','RepSupAgent','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('../WebAdminModule/rptAgentloginInformationReport.aspx?Pageame=Agentlogininfo&BackPageName=AgentloginInformation.aspx&custcode=" & strcustomer & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&subuser=" & strsubuser & "&CtryCode=" & strcountry & "&CityCode=" & strcitycode & "&rpttype=" & strrpttype & "&rptrepfilter=" & strrepfilter & "','RepSupAgent');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
End Class
