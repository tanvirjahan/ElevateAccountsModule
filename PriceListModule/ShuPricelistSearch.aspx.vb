﻿#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ShuPricelistSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim objDateTime As New clsDateTime
    Dim myDataAdapter As SqlDataAdapter

    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
#End Region

#Region "Enum GridCol"
    Enum GridCol
        PListCodeTCol = 0
        PListCode = 1
        '   Suppplier = 2
        Market = 2
        othgrpname = 3
        currcode = 4
        SubSeason = 5
        FromDate = 6
        ToDate = 7
        approve = 8
        Active = 9
        transfertype = 10
        DateCreated = 11
        UserCreated = 12
        DateModified = 13
        UserModified = 14
        approvet = 15
        Edit = 16
        View = 17
        Delete = 18
        Copy = 19
    End Enum
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
                hdpanelvalue.Value = 0

                SetFocus(TxtPLCD)
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                             CType(strappname, String), "PriceListModule\TrfPriceListSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If

                checkIsPrivilege()

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039))  order by sptypecode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by sptypename", True)


                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by partycode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by partyname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)


                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)

                ddlServerType.Items.Clear()
                ddlServerType.Items.Add("[Select]")
                ddlServerType.Items.Add("Arrival Borders")
                ddlServerType.Items.Add("Departure Borders")
                ddlServerType.Items.Add("Internal Transfer/Excursion")
                ddlServerType.Items.Add("Arrival/Departure Transfer Borders")


                Session.Add("strsortExpression", "trfplisth.tplistcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                charcters(TxtPLCD)
                btnAddNew.Visible = True
                btnExportToExcel.Visible = True
                'btnPrint.Visible = True


                ddlSubSeasCode.Visible = True
                ddlSubSeasName.Visible = True
                ddlGroupCode.Visible = True
                ddlGroupName.Visible = True
                ' txtfromDate.Visible = True
                ' txtToDate.Visible = True

                'FillGrid("ocplistcode")
                FillGridWithOrderByValues()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


                    ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    'ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    'ddlSupplierAgentCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSupplierAgentName.Attributes.Add("onkeyDown", "TADD_OnKeyDown(this);")

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                Else
                    'If ddlSPType.Value <> "[Select]" Then
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                    'Else
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

                    'End If
                    'If ddlSupplierCode.Value <> "[Select]" Then
                    '    strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                    '    "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

                    '    strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                    '    "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
                    'Else
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGroupName.Value)
                    'End If
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGroupName.Value)
                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ShuPriceListWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub

#End Region


#Region "Public Function checkIsPrivilege() As Boolean"
    Public Function checkIsPrivilege() As Boolean
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='8' and appid='1' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Statusapprove", "Yes")
            Else
                Session.Add("Statusapprove", "No")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try

    End Function
#End Region


#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String

        strWhereCond = ""
        If TxtPLCD.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(trfplisth.tplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(trfplisth.tplistcode) LIKE '" & Trim(TxtPLCD.Text.Trim.ToUpper) & "%'"
            End If
        End If

        'If ddlSPType.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " sptypemast.sptypecode= '" & Trim(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String)) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND partymast.sptypecode = '" & Trim(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String)) & "'"
        '    End If
        'End If
        'If ddlSupplierCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " trfplisth.partycode = '" & Trim(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND trfplisth.partycode = '" & Trim(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)) & "'"
        '    End If
        'End If
        'If ddlSupplierAgentCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " trfplisth.supagentcode = '" & Trim(CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String)) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND trfplisth.supagentcode = '" & Trim(CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String)) & "'"
        '    End If
        'End If
        'If ddlMarketCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " oplist_costh.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND oplist_costh.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '    End If
        'End If
        If ddlGroupCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (trfplisth.othgrpcode) = '" & Trim(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND (trfplisth.othgrpcode) = '" & Trim(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String)) & "'"
            End If
        End If
        If ddlSubSeasCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " trfplisth.subseascode = '" & Trim(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND trfplisth.subseascode = '" & Trim(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If ddlMarketCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " trfplisth_market.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND trfplisth_market.plgrpcode = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
            End If
        End If

        If DDLstatus.SelectedIndex = 1 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(trfplisth.approve,0) = 0"
            Else
                strWhereCond = strWhereCond & " and isnull(trfplisth.approve,0) = 0"
            End If
        ElseIf DDLstatus.SelectedIndex = 2 Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " isnull(trfplisth.approve,0) = 1"
            Else
                strWhereCond = strWhereCond & " and isnull(trfplisth.approve,0) = 1"
            End If
        End If

        If ddlServerType.SelectedValue <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " trfplisth.Transfertype= '" & CType(ddlServerType.SelectedIndex, String) & "'"
            Else
                strWhereCond = strWhereCond & " AND trfplisth.Transfertype = '" & CType(ddlServerType.SelectedIndex, String) & "'"
            End If
        End If

        If txtfromDate.Text <> "" And txtToDate.Text <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "( (convert(varchar(10),trfplisth_dates.frmdate,111) between convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),trfplisth_dates.todate,111)  between convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),trfplisth_dates.frmdate,111) <= convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),trfplisth_dates.todate,111) >= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"
            Else
                strWhereCond = strWhereCond & " and ( (convert(varchar(10),trfplisth_dates.frmdate,111) between convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) ) " _
                & "  or (convert(varchar(10),trfplisth_dates.todate,111)  between convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and  convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111) )" _
                & " or (convert(varchar(10),trfplisth_dates.frmdate,111) <= convert(varchar(10), '" & Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") & "',111) " _
                & "  and convert(varchar(10),trfplisth_dates.todate,111) >= convert(varchar(10), '" & Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") & "',111)) )"

            End If
        End If
        'strWhereCond = strWhereCond & " and trfplisth.shuttle=0 "

        BuildCondition = strWhereCond
    End Function

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select trfplisth.tplistcode, othgrpmast.othgrpname,dbo.fn_get_trfplist_market(trfplisth.tplistcode) plgrpcode," & _
                "trfplisth.currcode, trfplisth.subseascode,   min(trfplisth_dates.frmdate) frmdate, " & _
                " max(trfplisth_dates.todate) todate," & _
                " case when isnull(trfplisth.active,0)=1 then 'Active' when isnull(trfplisth.active,0)=0 then 'InActive' end active, " & _
                " case when isnull(trfplisth.transfertype,0)=0 then 'All' when   isnull(trfplisth.transfertype,0)=1 then 'Arrival Borders' when  isnull(trfplisth.transfertype,0)=2 then 'Departure Borders'   when isnull(trfplisth.transfertype,0)=3 then 'Internal Transfer/Excursion' when isnull(trfplisth.transfertype,0)=4 then 'Arrival/Departure Transfer Borders' end transfertype, " & _
                " trfplisth.adddate,  trfplisth.adduser, trfplisth.moddate, trfplisth.moduser,case isnull(trfplisth.approve,0) when 0 then 'Unapproved' else 'Approved' end approve    FROM trfplisth " & _
                " INNER JOIN othgrpmast on trfplisth.othgrpcode=othgrpmast.othgrpcode " & _
                " INNER JOIN trfplisth_dates on trfplisth.tplistcode=trfplisth_dates.tplistcode " & _
                " INNER JOIN trfplisth_market ON trfplisth_market.tplistcode =trfplisth.tplistcode"
           
            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition()
                strSqlQry = strSqlQry & " and  trfplisth.shuttle=1 group by  trfplisth.tplistcode, othgrpmast.othgrpname,trfplisth.currcode," & _
                    " trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser, trfplisth.moddate, trfplisth.moduser, trfplisth.approve,trfplisth.transfertype"
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder

            Else
                'strSqlQry = strSqlQry & " group by  trfplisth.tplistcode, partymast.partyname, supplier_agents.supagentname , othgrpmast.othgrpname,trfplisth.currcode," & _
                '    " trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser, trfplisth.moddate, trfplisth.moduser, trfplisth.approve,trfplisth.transfertype"

                strSqlQry = strSqlQry & " where  trfplisth.shuttle=1 group by  trfplisth.tplistcode, othgrpmast.othgrpname,trfplisth.currcode," & _
                  " trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser, trfplisth.moddate, trfplisth.moduser, trfplisth.approve,trfplisth.transfertype"

                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        Dim strpop As String = ""
        strpop = "window.open('ShuPriceList1.aspx?State=New','ShuPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("oplist_costh.ocplistcode")
        FillGridWithOrderByValues()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblocplistcode")

            Dim approve As Label
            approve = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapprove")

            If e.CommandName = "Editrow" Then
                ''Session.Add("State", "Edit")
                ''Session.Add("RefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("OtherServicesCostPriceList1.aspx", False)

                If approve.Text = "Approved" Then
                    If Session("Statusapprove") = "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Edit the Approved Booking' );", True)
                        Return
                    End If
                End If
                Dim strpop As String = ""
                strpop = "window.open('ShuPriceList1.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','ShuPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList2.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('ShuPriceList2.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','ShuPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "Deleterow" Then
                ''Session.Add("State", "Delete")
                ''Session.Add("RefCode", CType(lblId.Text.Trim, String))
                ''Response.Redirect("OtherServicesCostPriceList2.aspx", False)

                If approve.Text = "Approved" Then
                    If Session("Statusapprove") = "No" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Delete the Approved Booking' );", True)
                        Return
                    End If
                End If


                Dim strpop As String = ""
                strpop = "window.open('ShuPriceList2.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','ShuPriceList2','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "Copy" Then
                'Session.Add("State", "Copy")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServicesCostPriceList1.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('ShuPriceList1.aspx?State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','ShuPriceList1','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ShuPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        TxtPLCD.Text = ""

        ddlSPType.Value = "[Select]"
        ddlSPTypeName.Value = "[Select]"

        'ddlSupplierAgentCode.Value = "[Select]"
        'ddlSupplierAgentName.Value = "[Select]"

        ddlMarketCode.Value = "[Select]"
        ddlMarketName.Value = "[Select]"

        ddlGroupCode.Value = "[Select]"
        ddlGroupName.Value = "[Select]"

        'ddlSupplierCode.Value = "[Select]"
        'ddlSupplierName.Value = "[Select]"

        ddlSubSeasCode.Value = "[Select]"
        ddlSubSeasName.Value = "[Select]"

        txtfromDate.Text = ""
        txtToDate.Text = ""

        DDLstatus.SelectedIndex = 0
        ' FillGrid("oplist_costh.ocplistcode")
        ddlOrderBy.SelectedIndex = 0
        FillGridWithOrderByValues()
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region


#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                'strSqlQry = "select oplist_costh.ocplistcode AS [PList Code], partymast.partyname as  Suppplier, supplier_agents.supagentname as  [Suppplier Agent] , " & _
                '      " oplist_costh.plgrpcode AS Market , othgrpmast.othgrpname AS [Group] ,oplist_costh.currcode AS Currency , oplist_costh.subseascode AS [Sub Season] ,  " & _
                '      " convert(varchar(10),oplist_costh.frmdate,103)  as [From Date], convert(varchar(10),oplist_costh.todate,103) as [To Date], " & _
                '      " case when isnull(oplist_costh.active,0)=1 then 'Active' when isnull(oplist_costh.active,0)=0 then 'InActive' end Active,  " & _
                '      " oplist_costh.adddate as [Date Created],  oplist_costh.adduser AS [User Created], oplist_costh.moddate as [Date Modified], oplist_costh.moduser AS [User Modified] " & _
                '      " FROM oplist_costh  INNER JOIN partymast on oplist_costh.partycode=partymast.partycode  " & _
                '      " INNER JOIN othgrpmast on oplist_costh.othgrpcode=othgrpmast.othgrpcode  " & _
                '      " LEFT OUTER JOIN supplier_agents on oplist_costh.supagentcode=supplier_agents.supagentcode " & _
                '      " LEFT OUTER JOIN sptypemast on partymast.sptypecode=sptypemast.sptypecode"

                'strSqlQry = "select trfplisth.tplistcode AS [TPList Code], partymast.partyname as  Suppplier, supplier_agents.supagentname as  [Suppplier Agent] , " & _
                '    "  dbo.fn_get_trfplist_market(trfplisth.tplistcode) as [Market]  , othgrpmast.othgrpname AS [Group] ,trfplisth.currcode AS Currency ," & _
                '    "  trfplisth.subseascode AS [Sub Season] ,  min(trfplisth_dates.frmdate) frmdate,max(trfplisth_dates.todate) todate, " & _
                '    " case when isnull(trfplisth.active,0)=1 then 'Active' when isnull(trfplisth.active,0)=0 then 'InActive' end Active, " & _
                '    "  trfplisth.adddate as [Date Created],  trfplisth.adduser AS [User Created], trfplisth.moddate as [Date Modified], trfplisth.moduser AS [User Modified] " & _
                '    "  FROM trfplisth  INNER JOIN partymast on trfplisth.partycode=partymast.partycode  INNER JOIN othgrpmast on trfplisth.othgrpcode=othgrpmast.othgrpcode  " & _
                '    "  LEFT OUTER JOIN supplier_agents on trfplisth.supagentcode=supplier_agents.supagentcode  LEFT OUTER JOIN sptypemast on partymast.sptypecode=sptypemast.sptypecode" & _
                '    "  INNER JOIN trfplisth_dates on trfplisth.tplistcode=trfplisth_dates.tplistcode INNER JOIN trfplisth_market ON trfplisth_market.tplistcode =trfplisth.tplistcode "

                strSqlQry = "select trfplisth.tplistcode, othgrpmast.othgrpname,dbo.fn_get_trfplist_market(trfplisth.tplistcode) plgrpcode," & _
                "trfplisth.currcode, trfplisth.subseascode,   min(trfplisth_dates.frmdate) frmdate, " & _
                " max(trfplisth_dates.todate) todate," & _
                " case when isnull(trfplisth.active,0)=1 then 'Active' when isnull(trfplisth.active,0)=0 then 'InActive' end active, " & _
                " case when isnull(trfplisth.transfertype,0)=0 then 'All' when   isnull(trfplisth.transfertype,0)=1 then 'Arrival Borders' when  isnull(trfplisth.transfertype,0)=2 then 'Departure Borders'   when isnull(trfplisth.transfertype,0)=3 then 'Internal Transfer/Excursion' when isnull(trfplisth.transfertype,0)=4 then 'Arrival/Departure Transfer Borders' end transfertype, " & _
                " trfplisth.adddate,  trfplisth.adduser, trfplisth.moddate, trfplisth.moduser,case isnull(trfplisth.approve,0) when 0 then 'Unapproved' else 'Approved' end approve    FROM trfplisth " & _
                " INNER JOIN othgrpmast on trfplisth.othgrpcode=othgrpmast.othgrpcode " & _
                " INNER JOIN trfplisth_dates on trfplisth.tplistcode=trfplisth_dates.tplistcode " & _
                " INNER JOIN trfplisth_market ON trfplisth_market.tplistcode =trfplisth.tplistcode"

                ''group by  trfplisth.tplistcode, partymast.partyname, supplier_agents.supagentname , othgrpmast.othgrpname,
                '' trfplisth.currcode, trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser,
                ''  trfplisth.moddate, trfplisth.moduser, trfplisth.approve ORDER BY trfplisth.tplistcode DESC

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition()
                    strSqlQry = strSqlQry & " and trfplisth.shuttle=1 group by  trfplisth.tplistcode,  othgrpmast.othgrpname,trfplisth.currcode," & _
                        " trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser, trfplisth.moddate, trfplisth.moduser, trfplisth.approve,trfplisth.transfertype"
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()

                Else
                    strSqlQry = strSqlQry & " where trfplisth.shuttle=1 group by  trfplisth.tplistcode,  othgrpmast.othgrpname,trfplisth.currcode," & _
                        " trfplisth.subseascode,  trfplisth.active,trfplisth.adddate, trfplisth.adduser, trfplisth.moddate, trfplisth.moduser, trfplisth.approve,trfplisth.transfertype"
                    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                End If


              


                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "oplisth")

                objUtils.ExportToExcel(DS, Response)
                'con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        Finally
            clsDBConnect.dbAdapterClose(DA)                       'Close adapter
            clsDBConnect.dbConnectionClose(con)
        End Try
    End Sub

#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        'Response.Redirect("TrfPricelistSellingRates.aspx?State=New&RefCode=TRPL/000016&supplier=T-000002&suppliername=FADI ECRS&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)
        'Response.Redirect("TrfSellingRatePaxSlab.aspx?State=New&RefCode=TRPL/000016&supplier=T-000002&suppliername=FADI ECRS&SupplierType=Transfers&SupplierTypeName=Transfers&Market=CIS;&SuppierAgent=WONINF&SupplierAgentName=&CurrencyCode=AED&CurrencyName=DIRHAM&SubSeasonCode=ALL SEASONS&SubSeasonName=ALL", False)

        'Response.Redirect("TrfPaxSlab.aspx", False)

        'TrfSellingRatePaxSlab()



        'Try
        '    '  Session.Add("CurrencyCode", txtblocksale_header.blocksaleid.Text.Trim)
        '    '   Session.Add("CurrencyName", txtcityname.Text.Trim)
        '    '   Response.Redirect("rptCurrencies.aspx", False)

        '    Dim strReportTitle As String = ""
        '    Dim strSelectionFormula As String = ""

        '    Session.Add("Pageame", "Block Full Sales")
        '    Session.Add("BackPageName", "BlockFullSalesSearch.aspx")

        '    If txtTranId.Text.Trim <> "" Then
        '        strReportTitle = "Block Sale ID : " & txtTranId.Text.Trim
        '        strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & txtTranId.Text.Trim & "*'"
        '    End If


        '    If ddlPartyCode.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Supplier Code : " & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String))
        '            strSelectionFormula = "{blocksale_header.partycode} = '" & Trim(CType(ddlPartyCode.Items(ddlPartyCode.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If

        '    If ddlSupACode.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Supplier Agent Code : " & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String))
        '            strSelectionFormula = "{sptypemast.sptypecode} = '" & Trim(CType(ddlSupACode.Items(ddlSupACode.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If

        '    If ddlSupAName.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Supplier Agent Name : " & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String))
        '            strSelectionFormula = "{sptypemast.sptypename} = '" & Trim(CType(ddlSupAName.Items(ddlSupAName.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If

        '    If ddlMarketCode.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Market Code : " & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String))
        '            strSelectionFormula = "{blocksale_header.plgrpcode} = '" & Trim(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If
        '    If ddlMarketName.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Market Name : " & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String))
        '            strSelectionFormula = "{plgrpmast.plgrpname} = '" & Trim(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If
        '    If ddlSPTypeCode.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Supplier Type Code : " & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String))
        '            strSelectionFormula = "{blocksale_header.supagentcode} = '" & Trim(CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If
        '    If ddlSpTypeName.Value.Trim <> "[Select]" Then
        '        If strSelectionFormula <> "" Then
        '            strReportTitle = strReportTitle & " ; Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '            strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '        Else
        '            strReportTitle = "Supplier Type Name : " & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String))
        '            strSelectionFormula = "{supplier_agents.supagentname} = '" & Trim(CType(ddlSpTypeName.Items(ddlSpTypeName.SelectedIndex).Text, String)) & "'"
        '        End If
        '    End If


        '    Session.Add("SelectionFormula", strSelectionFormula)
        '    Session.Add("ReportTitle", strReportTitle)
        '    Response.Redirect("rptReport.aspx", False)
        'Catch ex As Exception
        '    objUtils.WritErrorLog("BlockFullSalesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try
    End Sub
#End Region


    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        PnlOtherServiceCost.Visible = False
        hdpanelvalue.Value = 0
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        PnlOtherServiceCost.Visible = True
        hdpanelvalue.Value = 1
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        'FillGrid("ocplistcode")
        FillGridWithOrderByValues()
    End Sub

    Private Sub FillGridWithOrderByValues()
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("trfplisth.tplistcode", "DESC")
            Case 1
                FillGrid("trfplisth.tplistcode", "ASC")
            Case 2
                FillGrid("partymast.partyname", "ASC")
            Case 3
                'FillGrid("oplist_costh.plgrpcode", "ASC")
            Case 4
                FillGrid("othgrpmast.othgrpname", "ASC")
            Case 5
                FillGrid("trfplisth.subseascode", "ASC")
        End Select
    End Sub

    Private Function ExportWithOrderByValues() As String
        ExportWithOrderByValues = ""
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                ExportWithOrderByValues = "trfplisth.tplistcode DESC"
            Case 1
                ExportWithOrderByValues = "trfplisth.tplistcode ASC"
            Case 2
                ExportWithOrderByValues = "partymast.partyname ASC"
            Case 3
                'ExportWithOrderByValues = "oplist_costh.plgrpcode ASC"
            Case 4
                ExportWithOrderByValues = "othgrpmast.othgrpname ASC"
            Case 5
                ExportWithOrderByValues = "trfplisth.subseascode ASC"
        End Select
    End Function

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TrfPriceListSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        FillGridWithOrderByValues()
    End Sub
End Class
