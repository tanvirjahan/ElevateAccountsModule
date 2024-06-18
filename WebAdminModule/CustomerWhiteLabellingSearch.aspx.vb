#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
#End Region
Partial Class CustomerWhiteLabellingSearch
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
                objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustWhiteLabelWindowPostBack") Then
            btnFillList_Click(sender, e)
        End If
    End Sub
#End Region

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
            strSqlQry = "select a.agentcode as angentcodedis,a.agentname,case  isnull(w.whitelable,0) when 1 then 'Yes' when 0 then 'No' end whitelablesdis,w.* ,case isnull(w.active,0) when 0 then 'No' when 1 then 'Yes' end Activedis from agentmast a left outer join  agentmastwl w on a.agentcode=w.agentcode where a.active=1"


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
            Else
                grdUploadClients.PageIndex = 0
                grdUploadClients.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
       
        Dim strWhereCond As String
        strWhereCond = ""

        If txtCustomerCode.Value <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.agentcode LIKE '" & txtCustomerCode.Value & "%'"
            Else
                strWhereCond = strWhereCond & " AND a.agentcode LIKE '" & txtCustomerCode.Value & "%'"
            End If
        End If

        If txtcustomername.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.agentname LIKE '" & txtcustomername.Text & "%'"
            Else
                strWhereCond = strWhereCond & " AND a.agentname LIKE '" & txtcustomername.Text & "%'"
            End If
        End If

        If ddlMarket.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND a.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            End If
        End If

        If ddlSellingType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND a.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCategory.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND a.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCountry.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND a.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCity.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " a.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND a.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            End If
        End If


        If chkwhitelabelagent.Checked = True Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(a.whitelable, 0) = 1"
            Else
                strWhereCond = strWhereCond & " AND  isnull(a.whitelable, 0) = 1"
            End If
        End If
        If chkactive.Checked = True Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(w.active, 0) = 1"
            Else
                strWhereCond = strWhereCond & " AND  isnull(w.active, 0) = 1"
            End If
        End If
        If chkPwhitelabel.Checked = True Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(a.whitelable, 0) = 0"
            Else
                strWhereCond = strWhereCond & " AND  isnull(a.whitelable, 0) = 0"
            End If
        End If
        BuildCondition = strWhereCond
    End Function
#End Region

#Region "Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            FillGrid(ddlOrderBy.Value)
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

    Protected Sub grdUploadClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUploadClients.PageIndexChanging
        Try
            grdUploadClients.PageIndex = e.NewPageIndex
            FillGrid(ddlOrderBy.Value)
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillList.Click

        Try
            FillGrid(ddlOrderBy.Value)
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
       
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            txtCustomerCode.Value = ""
            txtcustomername.Text = ""
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
            chkwhitelabelagent.Checked = False
            chkactive.Checked = False
            chkPwhitelabel.Checked = False

            FillGrid(ddlOrderBy.Value)
        Catch ex As Exception
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub grdUploadClients_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUploadClients.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            Dim lblwhitelable As Label
            lblId = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagentCode")
            lblwhitelable = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblwhitelable")
            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                strpop = "window.open('CustomerWhiteLabelling.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','CustWhiteLablelling','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "ViewRow" Then
                Dim strpop As String = ""
                strpop = "window.open('CustomerWhiteLabelling.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','CustWhiteLablelling','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                strpop = "window.open('CustomerWhiteLabelling.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','CustWhiteLablelling','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "AddRow" Then

                If lblwhitelable.Text = "No" Then
                    Dim strpop As String = ""
                    strpop = "window.open('CustomerWhiteLabelling.aspx?State=New&RefCode=" + CType(lblId.Text.Trim, String) + "','CustWhiteLablelling','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                    'Else
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record already exist');", True)
                End If
                
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub grdUploadClients_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUploadClients.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblwhitelable As Label = CType(e.Row.FindControl("lblwhitelable"), Label)
                Dim lnkadd As LinkButton = CType(e.Row.FindControl("lnkadd"), LinkButton)
                Dim lnkedit As LinkButton = CType(e.Row.FindControl("lnkedit"), LinkButton)
                Dim lnkdelete As LinkButton = CType(e.Row.FindControl("lnkdelete"), LinkButton)

                If lblwhitelable.Text = "Yes" Then
                    lnkadd.Enabled = False
                Else
                    lnkedit.Enabled = False
                    lnkdelete.Enabled = False
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strwhitelabel As Integer
            Dim strwactive As Integer
            Dim strpwhitelabel As Integer
            Dim stragentname As String = ""


            If chkwhitelabelagent.Checked = True Then
                strwhitelabel = 1
            Else
                strwhitelabel = 0
            End If

            If chkactive.Checked = True Then
                strwactive = 1
            Else
                strwactive = 0
            End If


            If chkPwhitelabel.Checked = True Then
                strpwhitelabel = 1
            Else
                strpwhitelabel = 0
            End If

            If txtcustomername.Text.Trim <> "" Then
                stragentname = txtcustomername.Text.Trim
            End If

            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=CustWhiteLabel&BackPageName=CustomerWhiteLabellingSearch.aspx&agentcode=" & txtCustomerCode.Value.Trim() & "&agentname=" & txtcustomername.Text.Trim() & "&SellTypeName=" & Trim(ddlSellingTypeName.Items(ddlSellingTypeName.SelectedIndex).Text) & "&CatCode=" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "&CatName=" & Trim(ddlCategoryName.Items(ddlCategoryName.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlCountry.Items(ddlCountry.SelectedIndex).Text) & "&CtryName=" & Trim(ddlCountryName.Items(ddlCountryName.SelectedIndex).Text) & "&MktCode=" & Trim(ddlMarket.Items(ddlMarket.SelectedIndex).Text) & "&MktName=" & Trim(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCity.Items(ddlCity.SelectedIndex).Text) & "&CityName=" & Trim(ddlCityName.Items(ddlCityName.SelectedIndex).Text) & "&Whitelabel=" & strwhitelabel & "&Wactive=" & strwactive & "&Pwhitelabel=" & strpwhitelabel & "&Stragentname=" & stragentname & "','CustwhiteLabel','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerWhiteLabellingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class
