#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
#End Region

Partial Class WebAdminModule_ApprovedCustomer
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
    Dim objDate As New clsDateTime

    Dim sqlTrans As SqlTransaction
    Dim objdatetime As New clsDateTime
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then
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

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")


                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "WebAdminModule\ApprovedCustomer.aspx", btnadd, btnExportToExcel, _
                                                     btnPrint, grdUploadClients)
                End If
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end
                Session.Add("strappsortExpression", "agentcode")
                Session.Add("strappsortdirection", SortDirection.Ascending)

                ' FillGridNew()

                'SetFocus(ddlOrderBy)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpcode", "plgrpname", "select * from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select * from plgrpmast where active=1 order by plgrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select * from citymast where active=1 order by citycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select * from citymast where active=1 order by cityname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellname", "select * from sellmast where active=1 order by sellcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingTypeName, "sellname", "sellcode", "select * from sellmast where active=1 order by sellname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", "select * from agentcatmast where active=1 order by agentcatcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", "select * from agentcatmast where active=1 order by agentcatname", True)

                'ddlOrderBy.SelectedIndex = 2

                'FillGrid(ddlOrderBy.Value)
                'btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                'Dim typ As Type
                'typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                '    ddlMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlSellingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlSellingTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If
                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ApprovedCustomer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try


            'Else

            '    Dim strstring As String = "'"



            'If hdnmarket.Value <> "[Select]" And hdnmarket.Value <> "" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select  ctrycode,ctryname  from  ctrymast   where active=1 and plgrpcode='" & hdnmarket.Value & "' order by ctrycode", True, ddlCountry.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select  ctrycode,ctryname  from  ctrymast   where active=1 and plgrpcode='" & hdnmarket.Value & "' order by ctrycode", True, ddlCountryName.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellname", "select ltrim(rtrim(sellcode)) as sellcode ,ltrim(rtrim(sellname)) as sellname  from sellmast where active=1  and plgrpcode='" & hdnmarket.Value & "'order by sellcode", True, ddlSellingType.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingTypeName, "sellname", "sellcode", "select ltrim(rtrim(sellcode)) as sellcode,ltrim(rtrim(sellname)) as sellname  from sellmast where active=1 and plgrpcode='" & hdnmarket.Value & "'order by sellname", True, ddlSellingTypeName.Value)


            'Else

            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctrycode,ctryname from ctrymast where active=1 order by ctryname", True)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellname", "select ltrim(rtrim(sellcode)) as sellcode ,ltrim(rtrim(sellname)) as sellname  from sellmast where active=1 order by sellcode", True)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingTypeName, "sellname", "sellcode", "select ltrim(rtrim(sellcode)) as sellcode,ltrim(rtrim(sellname)) as sellname  from sellmast where active=1 order by sellname", True)

            'End If

            'ddlCountryName.Value = hdncountry.Value
            'ddlCountry.Value = ddlCountryName.Items(ddlCountryName.SelectedIndex).Text

            'ddlSellingTypeName.Value = hdnsellingtype.Value
            'ddlSellingType.Value = ddlSellingTypeName.Items(ddlSellingTypeName.SelectedIndex).Text

            'If ddlCountry.Value <> "[Select]" Or ddlCountry.Value = "" Then


            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select  citycode,cityname  from  citymast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' order by citycode", True, ddlCity.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select  citycode,cityname  from  citymast   where active=1 and ctrycode='" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "' order by cityname", True, ddlCityName.Value)
            '    ddlCityName.Value = hdncity.Value
            '    ddlCity.Value = ddlCityName.Items(ddlCityName.SelectedIndex).Text

            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select  citycode,cityname  from  citymast   where active=1  order by citycode", True)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select  citycode,cityname  from  citymast   where active=1 order by cityname", True)


            'End If

            'If ddlSellingType.Value <> "[Select]" Or ddlSellingType.Value = "" Then


            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", "select  agentcatcode,agentcatname  from  agentcatmast   where active=1 and sellcode='" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "' order by agentcatcode", True, ddlCategory.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", "select  agentcatcode,agentcatname  from  agentcatmast   where active=1 and sellcode='" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "' order by agentcatname", True, ddlCategoryName.Value)
            '    ddlCategoryName.Value = hdncategory.Value
            '    ddlCategory.Value = ddlCategoryName.Items(ddlCategoryName.SelectedIndex).Text

            'Else

            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", "select  agentcatcode,agentcatname  from  agentcatmast   where active=1  order by agentcatcode", True)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", "select  agentcatcode,agentcatname  from  agentcatmast   where active=1  order by agentcatname", True)


            'End If















        End If
    End Sub
#End Region

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Function BuildConditionNew(ByVal strCustomerGroupValue As String, ByVal strCountryGroupValue As String, ByVal strCustomerValue As String, ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strCategoryValue As String, ByVal strsectorvalue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCustomerGroupValue <> "" Then

            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            Else

                strWhereCond = strWhereCond & " AND agentmast.agentcode in (select customergroup_detail.agentcode  from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in  (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            End If
        End If
        If strCountryGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            Else
                strWhereCond = strWhereCond & "  AND ctrymast.ctrycode in (select countrygroup_detail.ctrycode    from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            End If
        End If

        If strCustomerValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            Else
                strWhereCond = strWhereCond & " and  upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            End If
        End If
        If strCityValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(citymast. cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCategoryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agentcatmast. agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strsectorvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agent_sectormaster. sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
            End If
        End If
        If strTextValue <> "" Then
            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = "upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    Else

                        strWhereCond1 = strWhereCond1 & " OR upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If
        If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

            'If ddlOrder.SelectedValue = "C" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " (CONVERT(datetime, convert(varchar(10),agentmast.appdate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            Else
                strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),agentmast.appdate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            End If
            'ElseIf ddlOrder.SelectedValue = "M" Then
            '    If Trim(strWhereCond) = "" Then

            '        strWhereCond = " (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            '    Else
            '        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
            '    End If
        End If
        'End If


        BuildConditionNew = strWhereCond
    End Function
    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strCustomerGroupValue As String = ""
        Dim strCustomerValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strSectorValue As String = ""

        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CITY" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMERGROUP" Then
                        If strCustomerGroupValue <> "" Then
                            strCustomerGroupValue = strCustomerGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strCustomerValue <> "" Then
                            strCustomerValue = strCustomerValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRYGROUP" Then
                        If strCountryGroupValue <> "" Then
                            strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                        If strCategoryValue <> "" Then
                            strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            strBindCondition = BuildConditionNew(strCustomerGroupValue, strCountryGroupValue, strCustomerValue, strCountryValue, strCityValue, strCategoryValue, strSectorValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            grdUploadClients.Visible = True
            lblMsg.Visible = False
            If grdUploadClients.PageIndex < 0 Then
                grdUploadClients.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strappsortExpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = "SELECT agentcode,agentname,shortname,bookingengineratetype,webusername,webemail,webcontact,[webapprove]=case webapprove when 1 then 'Approved' else '' end, " & _
                " dbo.pwddecript(webpassword) webpassword,webusername,appdate,appuser FROM agentmast " & _
                " left outer join ctrymast on agentmast.ctrycode=ctrymast.ctrycode left outer join citymast on agentmast.citycode=citymast.citycode  " & _
                " left outer join agent_sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode left outer join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode   where agentmast.active = 1 and agentmast.webapprove=1"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            grdUploadClients.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                grdUploadClients.PageSize = pagevaluecus
                grdUploadClients.DataBind()
            Else
                grdUploadClients.PageIndex = 0
                grdUploadClients.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

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
            strSqlQry = "SELECT agentcode,agentname,webusername,webemail,webcontact,[webapprove]=case webapprove when 1 then 'Approved' else '' end,dbo.pwddecript(webpassword) webpassword,webusername,appdate,appuser FROM agentmast where active = 1 and webapprove=1"

            'If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
            '    If IsDate(txtFromDate.Text) And IsDate(txtTodate.Text) Then
            '        If CDate(txtFromDate.Text) > CDate(txtTodate.Text) Then
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date should be less than to date')", True)
            '            Exit Sub
            '        End If
            '        strSqlQry += " and appdate between '" & CType(Format(CDate(txtFromDate.Text), "yyyy/MM/dd"), String) & "' and '" & CType(Format(CDate(txtTodate.Text), "yyyy/MM/dd"), String) & "'"
            '    End If
            'End If


            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

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
            objUtils.WritErrorLog("ApprovedCustomer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Sub
#End Region



    '#Region "Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub btnFillList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '        FillGrid(ddlOrderBy.Value)
    '    End Sub
    '#End Region

#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessCat As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "CUSTOMER"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCountry, "C")
                Case "CUSTOMERGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMERGROUP", lsProcessCountry, "CSG")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCountry, "CTY")
                Case "COUNTRYGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRYGROUP", lsProcessCountry, "CG")
                Case "SECTOR"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessCountry, "S")

                Case "CITY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITY", lsProcessCity, "CT")
                Case "CATEGORY"
                    lsProcessCat = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORY", lsProcessCat, "CTG")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strappsortExpression"), "")

        myDS = grdUploadClients.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strappsortExpression", objUtils.SwapSortDirection(Session("strappsortExpression")))
            dataView.Sort = Session("strappsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strappsortExpression"))
            grdUploadClients.DataSource = dataView
            grdUploadClients.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub grdUploadClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUploadClients.PageIndexChanging"
    Protected Sub grdUploadClients_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdUploadClients.PageIndexChanging
        grdUploadClients.PageIndex = e.NewPageIndex
        '  FillGrid(ddlOrderBy.Value)
        FillGridNew()
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

            Dim lblagentCode As Label = grdUploadClients.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblagentCode")
            Dim divcode As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select divcode conm from agentmast where agentcode='" & lblagentCode.Text & "'")
            Dim cumulative As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(bookingengineratetype,'')  from agentmast where agentcode='" & lblagentCode.Text & "'")

            If e.CommandName = "Login" Then

                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('http://AgentsOnline/AgentMainPage.aspx','_blank');", True)


                'Session.Add("CompanyName", CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 conm from columbusmaster"), String))
                'Session.Add("Type", "Main User")
                'Session.Add("GlobalAgentUserName", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentcode from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))
                'Session.Add("AgentUserpwd", CType(lblPassword.Text.Trim, String))
                'Session.Add("AgentCode", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentcode from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))
                'Session.Add("Name", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentname from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))
                'Session.Add("Code", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentcode from agentmast where webusername='" & lblUserName.Text.Trim & "' AND dbo.pwddecript(webpassword)='" & lblPassword.Text.Trim & "' AND active=1"))
                'Session("subusercode") = ""

                If cumulative = "CUMULATIVE" Then
                    ' Dim randno As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(randomnumber,'')  from agentmast_whitelabel where agentcode='" & lblagentCode.Text & "'")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('http://app.dubaionline-travel.com/agentsonline/login.aspx?comp=922658278609486','_blank');", True)

                Else
                    If divcode = "01" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('http://app.dubaionline-travel.com/agentsonline/login.aspx','_blank');", True)
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('http://app.dubaionline-travel.com/agentsonline/login.aspx?comp=675558760549078','_blank');", True)
                    End If
                End If
             


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApprovedCustomer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting"

    Protected Sub grdUploadClients_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdUploadClients.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblagentName As Label = e.Row.FindControl("lblagentName")

            Dim lsagentName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsagentName = ""

                        If "CUSTOMER" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsagentName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsagentName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsagentName.Trim <> "" Then
                            lblagentName.Text = Regex.Replace(lblagentName.Text.Trim, lsagentName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
    End Sub
    Protected Sub grdUploadClients_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdUploadClients.Sorting
        Session.Add("strappsortExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'txtCustomerCode.Value = ""
        'txtcustomername.Text = ""
        'ddlMarket.Value = "[Select]"
        'ddlMarketName.Value = "[Select]"
        'ddlSellingType.Value = "[Select]"
        'ddlSellingTypeName.Value = "[Select]"
        'ddlCountry.Value = "[Select]"
        'ddlCountryName.Value = "[Select]"
        'ddlCity.Value = "[Select]"
        'ddlCityName.Value = "[Select]"
        'ddlCategory.Value = "[Select]"
        'ddlCategoryName.Value = "[Select]"
        'FillGrid(ddlOrderBy.Value)
    End Sub
#End Region

#Region "Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' FillGrid(ddlOrderBy.Value)
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ApprovedCustomer','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        Dim strReportTitle As String = ""
    '        Dim strSelectionFormula As String = ""
    '        Dim strpop As String = ""
    '        Dim strcustomer, strfromdate, strtodate, strcustomercode As String
    '        Dim strstatus, strcitycode, strcountry, strcategory, strmarket, strsellcode As String

    '        strfromdate = ""
    '        strtodate = ""
    '        'If ValidatePage() = False Then
    '        '    Exit Sub
    '        'End If
    '        'If txtFromDate.Text <> "" Then
    '        '    strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy-MM-dd 00:00:00 ")
    '        'End If
    '        'If txtTodate.Text <> "" Then
    '        '    strtodate = Format(CType(txtTodate.Text, Date), "yyyy-MM-dd 00:00:00 ")
    '        'End If

    '        'strcustomercode = txtCustomerCode.Value
    '        'strcustomer = txtcustomername.Text.Trim
    '        'strstatus = IIf(UCase(ddlStatus.Items(ddlStatus.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlStatus.Value, "")
    '        'strcitycode = IIf(UCase(ddlCity.Items(ddlCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCity.Items(ddlCity.SelectedIndex).Text, "")
    '        'strcountry = IIf(UCase(ddlCountry.Items(ddlCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountry.Items(ddlCountry.SelectedIndex).Text, "")
    '        'strcategory = IIf(UCase(ddlCategory.Items(ddlCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategory.Items(ddlCategory.SelectedIndex).Text, "")
    '        'strmarket = IIf(UCase(ddlMarket.Items(ddlMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlMarket.Items(ddlMarket.SelectedIndex).Text, "")
    '        'strsellcode = IIf(UCase(ddlSellingType.Items(ddlSellingType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSellingType.Items(ddlSellingType.SelectedIndex).Text, "")
    '        ''strpop = "window.open('../WebAdminModule/rptReportNew.aspx?Pageame=webapproval&BackPageName=Approvedcustomer.aspx&custcode=" & strcustomercode & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&status=" & strstatus & "&CustName=" & strcustomer & "&sellcode=" & strsellcode & "&marketcode=" & strmarket & "&category=" & strcategory & "','RepSupAgent','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '        strpop = "window.open('../WebAdminModule/rptReportNew.aspx?Pageame=webapproval&BackPageName=Approvedcustomer.aspx','RepSupAgent');"
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try

    'End Sub

    Public Function ValidatePage() As Boolean
        Dim myfdate As Date
        Dim mytdate As Date
        'If txtFromDate.Text <> "" And txtTodate.Text <> "" Then
        '    myfdate = txtFromDate.Text
        '    mytdate = txtTodate.Text
        '    myfdate = myfdate.AddDays(31)

        '    If CType(objdatetime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objdatetime.ConvertDateromTextBoxToDatabase(txtTodate.Text), Date) Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
        '        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtTodate.ClientID + "');", True)
        '        ValidatePage = False
        '        Exit Function
        '    End If


        ' End If
        ValidatePage = True
    End Function

    Protected Sub btnFilter_Click(sender As Object, e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(sender As Object, e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As System.EventArgs)
        Try
            Dim strReportTitle As String = ""

            Dim strpop As String = ""
            strpop = "window.open('../WebAdminModule/rptReportNew.aspx?Pageame=webapproval&BackPageName=Approvedcustomer.aspx','RepSupAgent');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomersSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If grdUploadClients.Rows.Count <> 0 Then

                strSqlQry = "SELECT agentcode as [Customer Code],agentname as [Customer Name],webusername[Web User Name],webemail[Web Email],webcontact[Web Contact],[Status]=case webapprove when 1 then 'Approved' else '' end,dbo.pwddecript(webpassword) webpassword,appdate[Approved Date],appuser [Approved User] FROM agentmast where active = 1 and webapprove=1"


                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "currmast")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

End Class
