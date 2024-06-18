'------------================--------------=======================------------------================
'   Module Name    :    MarkPreferredSuppliers.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
#End Region

Partial Class MarkPreferredExcursions
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chkSel As CheckBox
#End Region


    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
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

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        'gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGridNew()
        ' FillGridWithOrderByValues()
    End Sub
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

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
                ' SetFocus(txtBanktypecode)
                '  RowsPerPageMS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub

                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "WebAdminModule\MarkPreferredExcursions.aspx", btnAddNew, BtnExportToExcel, _
                                                     btnPrint, gv_SearchResult)
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
                Session.Add("strsortExpression", "exctypcode")
                Session.Add("strsortdirection", SortDirection.Ascending)



                FillGridNew()

                'Else
                '    txtconnection.Value = Session("dbconnectionName")
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "DoPostBack", "__doPostBack('dummybtnPivotGridupdate', 'dummybtnPivotGridupdate');", True)
            Else
                If Request("__EVENTARGUMENT") = "dummybtnPivotGridupdate" Then
                    dummybtnPivotGridupdate.Text = dummybtnPivotGridupdate.Text
                    FillGridNew()
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkPreferredExcursions.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub


    Private Function BuildConditionNew(ByVal strSectorValue As String, ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strCategoryValue As String, ByVal strHotelstatusValue As String, ByVal strHotelChainValue As String, ByVal strHotelGroupValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(excursiontypes.exctypname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(excursiontypes.exctypname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strSectorValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(excursiongroup.excursiongroupname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(excursiongroup.excursiongroupname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If


        End If
        If strCityValue.Trim <> "" Then

            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(excclassification_header.classificationname) IN( " & Trim(strCityValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(excclassification_header.classificationname) IN (" & Trim(strCityValue) & ")"
            End If



        End If
        If strCategoryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(excursiontypes.ratebasis) IN (" & IIf(Trim(strCategoryValue) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strCategoryValue)) & ")"

            Else
                strWhereCond = strWhereCond & " AND  upper(excursiontypes.ratebasis) IN (" & IIf(Trim(strCategoryValue) = "'ADULT/CHILD/SENIOR'", "'ACS'", Trim(strCategoryValue)) & ")"
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
                        strWhereCond1 = "upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excclassification_header.classificationname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excursiontypes.ratebasis) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excursiongroup. excursiongroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR upper(excursiontypes.exctypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excclassification_header.classificationname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excursiontypes.ratebasis) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(excursiongroup. excursiongroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        BuildConditionNew = strWhereCond
    End Function

    'Protected Sub RowsPerPagemS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageMS.SelectedIndexChanged
    '    '   RowsPerPageMS.SelectedIndex = e.
    '    FillGridNew()
    'End Sub
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strSectorValue As String = ""
        Dim strHotelstatusValue As String = ""
        Dim strHotelchainValue As String = ""
        Dim strHotelGroupValue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "EXCURSIONTYPE" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CLASSIFICATION" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "RATEBASIS" Then
                        If strCategoryValue <> "" Then
                            strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "EXCURSIONGROUP" Then
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
            ' Dim pagevaluems = RowsPerPageMS.SelectedValue
            strBindCondition = BuildConditionNew(strSectorValue, strCountryValue, strCityValue, strCategoryValue, strHotelstatusValue, strHotelchainValue, strHotelGroupValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            'If gv_SearchResult.PageIndex < 0 Then
            '    gv_SearchResult.PageIndex = 0
            'End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"

            strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,isnull(excursiongroup.excursiongroupcode,'') excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,(select case when excursiontypes.ratebasis='ACS' then'Adult/Child/Senior' else excursiontypes.ratebasis end) ratebasis  ,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active  " _
                & " , excursiontypes.preferred from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode "

            'strSqlQry = " select p.partycode,p.partyname,p.Preferred,ca.catname,ct.cityname,c.ctryname,s.sectorname,isnull(dbo.fn_get_hotelgroup(p.partycode),'')hotelgroup,hotelchainmaster.hotelchainname, p.contact1 from partymast p  left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =p.hotelstatuscode left outer JOIN  hotelchainmaster ON p.hotelchaincode = hotelchainmaster.hotelchaincode,ctrymast c,sectormaster s,citymast ct,catmast ca " & _
            '" where(p.ctrycode = c.ctrycode And p.citycode = ct.citycode)" & _
            '         "      and p.catcode = ca.catcode and p.active=1 and p.sectorcode = s.sectorcode"



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
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                ' gv_SearchResult.PageSize = pagevaluems
                gv_SearchResult.DataBind()
            Else
                ' gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            Dim gvrow As GridViewRow
            For i = 0 To myDS.Tables(0).Rows.Count - 1
                For Each gvrow In gv_SearchResult.Rows
                    If myDS.Tables(0).Rows(i)("exctypcode").ToString = gvrow.Cells(1).Text.ToString Then
                        chkSel = gvrow.FindControl("chkSelect")
                        If Val(myDS.Tables(0).Rows(i).Item("Preferred").ToString) = 1 Then
                            chkSel.Checked = True
                            Exit For
                        Else
                            chkSel.Checked = False
                            Exit For
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkPreferredExcursions.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Private Sub fillgridcheckbox()
        strSqlQry = "select partycode,preferred from partymast"
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        Dim myDS As New DataSet 'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        Dim gvrow As GridViewRow
        For i = 0 To myDS.Tables(0).Rows.Count - 1
            For Each gvrow In gv_SearchResult.Rows
                If myDS.Tables(0).Rows(i)("partycode").ToString = gvrow.Cells(1).Text.ToString Then
                    chkSel = gvrow.FindControl("chkSelect")
                    If Val(myDS.Tables(0).Rows(i).Item("Preferred").ToString) = 1 Then
                        chkSel.Checked = True
                        Exit For
                    Else
                        chkSel.Checked = False
                        Exit For
                    End If
                End If
            Next
        Next
    End Sub
#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim i As Integer

        lblMsg.Visible = False

        If gv_Searchresult.PageIndex < 0 Then
            gv_Searchresult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = " select partycode,partyname,Preferred,ctryname,cityname,catname from partymast,ctrymast,citymast,catmast " & _
                        " where partymast.ctrycode = ctrymast.ctrycode And partymast.citycode = citymast.citycode " & _
                        " and partymast.catcode = catmast.catcode and partymast.active=1 "

            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_Searchresult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Searchresult.DataBind()
            Else
                gv_Searchresult.PageIndex = 0
                gv_Searchresult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

            '----------------------------------Show Preferred Status 
            For i = 0 To myDS.Tables(0).Rows.Count - 1
                For Each gvRow In gv_Searchresult.Rows
                    If myDS.Tables(0).Rows(i)("partycode").ToString = gvRow.Cells(0).Text.ToString Then
                        chkSel = gvRow.FindControl("chkSelect")
                        If Val(myDS.Tables(0).Rows(i).Item("Preferred").ToString) = 1 Then
                            chkSel.Checked = True
                            Exit For
                        Else
                            chkSel.Checked = False
                            Exit For
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkPreferredSuppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Validate Page()"
    Public Function ValidatePage() As Boolean
        Try
            Dim dt As DataTable
            dt = Session("sDtDynamic")
            If dt.Rows.Count = 0 Then
                Dim flgCheck As Boolean = False

                For Each gvRow In gv_SearchResult.Rows
                    chkSel = gvRow.FindControl("chkSelect")
                    If chkSel.Checked = True Then
                        flgCheck = True
                        Exit For
                    End If
                Next
                If flgCheck = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast one supplier should be selected.');", True)
                    SetFocus(gv_SearchResult)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            ValidatePage = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '  FillGrid("partycode")
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblcountry As Label = e.Row.FindControl("lblcountry")
            Dim lblCity As Label = e.Row.FindControl("lblCity")
            Dim lblCategory As Label = e.Row.FindControl("lblCategory")
            Dim lblSector As Label = e.Row.FindControl("lblSector")
            Dim lblhotelgroup As Label = e.Row.FindControl("lblhotelgroup")
            Dim lblhotelchainname As Label = e.Row.FindControl("lblhotelchainname")
            Dim lblpartyname As Label = e.Row.FindControl("lblpartyname")
            Dim lsCityName As String = ""
            Dim lsCategoryName As String = ""
            Dim lsTextName As String = ""
            Dim lsCtryName As String = ""
            Dim lsSectorName As String = ""
            Dim lsCtryGrpName As String = ""
            Dim lsCtrychainname As String = ""
            Dim lsHotelName As String = ""
            Dim lshotelchainname As String = ""
            Dim lsPartyname As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCityName = ""

                        If "EXCURSIONTYPE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsPartyname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CLASSIFICATION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSectorName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper

                        End If
                        If "EXCURSIONGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCategoryName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        'If "HOTELGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lsHotelName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        'End If
                        If "RATEBASIS" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCityName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        'If "COUNTRYGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lsCityName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        'End If
                        'If "HOTELCHAIN" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lshotelchainname = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        'End If
                        'If "CATEGORY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lsCategoryName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        'End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsTextName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsCityName.Trim <> "" Then
                            lblCity.Text = Regex.Replace(lblCity.Text.Trim, lsCityName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        'If lsCtryName.Trim <> "" Then
                        '    lblcountry.Text = Regex.Replace(lblcountry.Text.Trim, lsCtryName.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If
                        If lsPartyname.Trim <> "" Then
                            lblpartyname.Text = Regex.Replace(lblpartyname.Text.Trim, lsPartyname.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        'If lshotelchainname.Trim <> "" Then
                        '    lblhotelchainname.Text = Regex.Replace(lblhotelchainname.Text.Trim, lshotelchainname.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '        RegexOptions.IgnoreCase)
                        'End If
                        If lsCategoryName.Trim <> "" Then
                            lblCategory.Text = Regex.Replace(lblCategory.Text.Trim, lsCategoryName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSectorName.Trim <> "" Then
                            lblSector.Text = Regex.Replace(lblSector.Text.Trim, lsSectorName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsTextName.Trim <> "" Then
                            'lblcountry.Text = Regex.Replace(lblcountry.Text.Trim, lsTextName.Trim(), _
                            '  Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                            '              RegexOptions.IgnoreCase)
                            lblCity.Text = Regex.Replace(lblCity.Text.Trim, lsTextName.Trim(), _
                             Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                         RegexOptions.IgnoreCase)
                            lblpartyname.Text = Regex.Replace(lblpartyname.Text.Trim, lsTextName.Trim(), _
         Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                     RegexOptions.IgnoreCase)
                            lblCategory.Text = Regex.Replace(lblCategory.Text.Trim, lsTextName.Trim(), _
                      Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                  RegexOptions.IgnoreCase)
                            lblSector.Text = Regex.Replace(lblSector.Text.Trim, lsTextName.Trim(), _
              Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                          RegexOptions.IgnoreCase)
                        End If


                    Next
                End If
            End If



        End If
    End Sub


    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx", False)
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If gv_SearchResult.Rows.Count <> 0 Then



                strSqlQry = "select excursiontypes.exctypcode,excursiontypes.exctypname,excursiongroup.excursiongroupcode,isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'') excursiongroup,excclassification_header.classificationname,(select case when excursiontypes.ratebasis='ACS' then'Adult/Child/Senior' else excursiontypes.ratebasis end) ratebasis  ,(select case when excursiontypes.active='1' then 'Yes' else 'No'end) as Active  " _
              & " , excursiontypes.preferred from excursiontypes left join excursiongroup on  excursiongroup.excursiongroupname= isnull(dbo.fn_get_excursiongroup( excursiontypes.exctypcode),'')  inner join excclassification_header on excursiontypes.classificationcode=excclassification_header.classificationcode where excursiontypes.Preferred=1 "


                'strSqlQry = " select p.partycode as [Supplier Code],p.partyname as [Supplier Name],p.Preferred,ca.catname as Category,ct.cityname as City,s.sectorname as Sector,c.ctryname as Country ,isnull(dbo.fn_get_hotelgroup(p.partycode),'') as [Hotel Group],hotelchainmaster.hotelchainname as [Hotel Chain], p.contact1 as[Contact] from partymast p  left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =p.hotelstatuscode left outer JOIN  hotelchainmaster ON p.hotelchaincode = hotelchainmaster.hotelchaincode,ctrymast c,sectormaster s,citymast ct,catmast ca " & _
                '" where(p.ctrycode = c.ctrycode And p.citycode = ct.citycode)" & _
                '         "      and p.catcode = ca.catcode and p.active=1 and p.sectorcode = s.sectorcode  and p.Preferred=1 "

                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY partycode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY partycode "
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)

                DA.Fill(DS, "PreferredExcursions")

                If DS.Tables(0).Rows.Count > 0 Then

                    objUtils.ExportToExcel(DS, Response)
                    con.Close()
                Else
                    objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "PreferredSupplier")
            'Session.Add("BackPageName", "MarkPreferredSuppliers.aspx")
            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=PreferredSupplier&BackPageName=MarkPreferredSuppliers.aspx&SupTypeCode=" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "&SupTypeName=" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "&CatCode=" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "&CatName=" & Trim(ddlCategoryName.Items(ddlCategoryName.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlCountry.Items(ddlCountry.SelectedIndex).Text) & "&CtryName=" & Trim(ddlCountryName.Items(ddlCountryName.SelectedIndex).Text) & "&SectCode=" & Trim(ddlSector.Items(ddlSector.SelectedIndex).Text) & "&SectName=" & Trim(ddlSectorName.Items(ddlSectorName.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCity.Items(ddlCity.SelectedIndex).Text) & "&CityName=" & Trim(ddlCityName.Items(ddlCityName.SelectedIndex).Text) & "','PrefSup','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=PreferredExcursions&BackPageName=MarkPreferredExcursions.aspx','PrefSup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MarkPreferredSuppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MarkPreferredSuppliers','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub




    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MarkPreferredSuppliers','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    'End Sub

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
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
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
                Case "EXCURSIONGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("EXCURSIONGROUP", lsProcessCountry, "CTY")

                Case "CLASSIFICATION"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CLASSIFICATION", lsProcessCity, "CT")
                Case "EXCURSIONTYPE"
                    lsProcessCat = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("EXCURSIONTYPE", lsProcessCat, "CTG")

                Case "RATEBASIS"
                    lsProcessSector = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("RATEBASIS", lsProcessSector, "S")

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

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        If Page.IsPostBack = False Then
            If AppId.Value Is Nothing = False Then
                'Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case AppId.Value
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
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"

                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                If ValidatePage() = False Then
                    Exit Sub
                End If

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start


                '----------------------------------- Updating Data To partymast Table
                For Each gvRow In gv_SearchResult.Rows
                    chkSel = gvRow.FindControl("chkSelect")
                    myCommand = New SqlCommand("sp_update_excursion", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(1).Text, String)
                    If chkSel.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int, 9)).Value = 1
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@preferred", SqlDbType.Int, 9)).Value = 0
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                Next
                '-----------------------------------------------------------
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                'Response.Redirect("BankTypesSearch.aspx", False)
                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('BanktypeWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("MarkPreferredSuppliers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
End Class
