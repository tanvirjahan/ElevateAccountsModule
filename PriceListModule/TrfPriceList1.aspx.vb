
#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_TrfPriceList1
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

    Dim gvRow1 As GridViewRow
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Request.QueryString("State") = "New" Then
                Page.Title = Page.Title + " " + "New Transfer Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy Transfer Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit Transfer Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View Transfer Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete Transfer Price List"
            End If

            Dim s As String = ""
            ViewState.Add("TransferpricelistState", Request.QueryString("State"))
            ViewState.Add("TransferpricelistRefCode", Request.QueryString("RefCode"))

            If IsPostBack = False Then
                ''If Not Session("CompanyName") Is Nothing Then
                ''    Me.Page.Title = CType(Session("CompanyName"), String)
                ''End If

                txtconnection.Value = Session("dbconnectionName")
                Dim supagentcode As String
                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode =(select option_selected  from " & _
                '" reservation_parameters where param_id=564)  order by sptypecode"

                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 "

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by sptypecode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by sptypename ", True)

                Dim sptype As String = ""
                sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "564")
                If sptype <> "" Then
                    ddlSPTypeName.Value = sptype
                    ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1  and sptypecode='" & sptype & "' order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1   and sptypecode='" & sptype & "'order by partyname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)
                
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)

                ddlServerType.Items.Clear()
                ddlServerType.Items.Add("[Select]")
                ddlServerType.Items.Add("Arrival Borders")
                ddlServerType.Items.Add("Departure Borders")
                ddlServerType.Items.Add("Internal Transfer/Excursion")
                ddlServerType.Items.Add("Arrival/Departure Transfer Borders")


                FillGridMarket("plgrpcode")

                'supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='520'")
                'If supagentcode <> "" Then
                '    ddlSupplierAgentName.Value = CType(supagentcode, String)
                '    ddlSupplierAgentCode.Value = ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text
                'End If

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("TransferpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New Transfer Price List"

                    If Request.QueryString("SptypeCode") <> Nothing And Request.QueryString("SptypeName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SptypeName")
                        ddlSPType.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SptypeCode")
                        ddlSPTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("SupplierCode") <> Nothing And Request.QueryString("SupplierName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SupplierName")
                        ddlSupplierCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SupplierCode")
                        ddlSupplierName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("SupplierAgentCode") <> Nothing And Request.QueryString("SupplierAgentName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SupplierAgentName")
                        ddlSupplierAgentCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SupplierAgentCode")
                        ddlSupplierAgentName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    'If Request.QueryString("MarketCode") <> Nothing And Request.QueryString("MarketName") <> Nothing Then
                    '    s = ""
                    '    s = Request.QueryString("MarketName")
                    '    ddlMarketCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    '    s = ""
                    '    s = Request.QueryString("MarketCode")
                    '    ddlMarketName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    'End If

                    'If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                    '    s = ""
                    '    s = Request.QueryString("GroupName")
                    '    ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    '    s = ""
                    '    s = Request.QueryString("GroupCode")
                    '    ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    'End If

                    If Request.QueryString("SubSeasCode") <> Nothing And Request.QueryString("SubSeasName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("SubSeasName")
                        ddlSubSeasCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("SubSeasCode")
                        ddlSubSeasName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    txtRemark.Value = CType(Session("sessionRemark"), String)

                    If Request.QueryString("PlcCode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("PlcCode")
                        txtPlcCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("currcode") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currcode")
                        txtCurrCode.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("currname") <> Nothing Then
                        s = ""
                        s = Request.QueryString("currname")
                        txtCurrName.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If


                    If Request.QueryString("Acvtive") <> Nothing Then
                        If Request.QueryString("Acvtive") = "1" Then
                            ChkActive.Checked = True
                        ElseIf Request.QueryString("Acvtive") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If

                    If Request.QueryString("transfertype") <> Nothing Then
                        ddlServerType.SelectedIndex = CType(Request.QueryString("transfertype"), Integer)
                    End If

                    'If ddlSPType.Value <> "[Select]" Then
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                    'Else
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

                    'End If
                  

                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("TransferpricelistState") = "Edit" Or ViewState("TransferpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("TransferpricelistRefCode"))
                    ShowMarket(CType(ViewState("TransferpricelistRefCode"), String))
                    lblHeading.Text = "Edit Transfer Price List"
                ElseIf ViewState("TransferpricelistState") = "View" Then
                ElseIf ViewState("TransferpricelistState") = "Delete" Then
                End If
                If ViewState("TransferpricelistState") = "Copy" Then
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
                TextLock(txtCurrCode)
                TextLock(txtCurrName)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSupplierAgentCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierAgentName.Attributes.Add("onkeyDown", "TADD_OnKeyDown(this);")

                    'ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    'ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")



                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("TransferpricelistState") = "New" Then
        ElseIf ViewState("TransferpricelistState") = "Edit" Or ViewState("TransferpricelistState") = "Copy" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True

            If ViewState("TransferpricelistState") = "Edit" Then
                ddlSupplierCode.Disabled = True
                ddlSupplierName.Disabled = True
                ddlServerType.Enabled = False
            End If


            'ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
        ElseIf ViewState("TransferpricelistState") = "Delete" Or ViewState("TransferpricelistState") = "View" Then
        End If


    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("tplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("tplistcode")
                    End If

                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", mySqlReader("partycode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlSPTypeName.Value)

                        If ddlSPType.Value <> "[Select]" Then
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                        End If
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by sptypecode ", True)
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 and sptypecode  in (Select Option_Selected From Reservation_ParaMeters Where Param_Id in (564,1039)) order by sptypename ", True)


                        ddlSupplierName.Value = mySqlReader("partycode")
                        ddlSupplierCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))
                    End If
                    If ddlSupplierCode.Value <> "[Select]" Then
                        strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

                        'strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        '"and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
                    End If
                    'If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    '    ddlGroupName.Value = mySqlReader("othgrpcode")
                    '    ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    'End If

                    'If IsDBNull(mySqlReader("plgrpcode")) = False Then
                    '    ddlMarketName.Value = mySqlReader("plgrpcode")
                    '    ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
                    'End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text = mySqlReader("supagentcode")
                        ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "supplier_agents", "supagentname", "supagentcode", mySqlReader("supagentcode"))
                    End If


                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        ddlSubSeasName.Value = mySqlReader("subseascode")
                        ddlSubSeasCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "subseasmast", "subseasname", "subseascode", mySqlReader("subseascode"))
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If ViewState("OthsercostpricelistState") <> "Copy" Then
                        'If IsDBNull(mySqlReader("frmdate")) = False Then
                        '    dpFromdate.txtDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                        '    Session("sessionFrmdate") = dpFromdate.txtDate.Text
                        'End If
                        'If IsDBNull(mySqlReader("todate")) = False Then
                        '    dpToDate.txtDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                        '    Session("sessionTodate") = dpToDate.txtDate.Text
                        'End If
                    End If



                    If IsDBNull(mySqlReader("active")) = False Then
                        If mySqlReader("active") = "1" Then
                            ChkActive.Checked = True
                        ElseIf mySqlReader("active") = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If
                End If

                If IsDBNull(mySqlReader("approve")) = False Then
                    If mySqlReader("approve") = 1 Then
                        chkapprove.Checked = True
                    ElseIf mySqlReader("approve") = 0 Then
                        chkapprove.Checked = False
                    End If
                End If

                If IsDBNull(mySqlReader("considerformarkup")) = False Then
                    If mySqlReader("considerformarkup") = 1 Then
                        chkConsdierForMarkUp.Checked = True
                    ElseIf mySqlReader("considerformarkup") = 0 Then
                        chkConsdierForMarkUp.Checked = False
                    End If
                End If

                If IsDBNull(mySqlReader("Transfertype")) = False Then
                    ddlServerType.SelectedIndex = mySqlReader("Transfertype")
                End If
            End If


        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        gv_Market.Visible = True
        If gv_Market.PageIndex < 0 Then
            gv_Market.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Market.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Market.DataBind()
                txtrowcnt.Value = gv_Market.Rows.Count
            Else
                gv_Market.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("trfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try
    End Sub
#End Region

#Region "Private Sub ShowMarket(ByVal RefCode As String)"

    Private Sub ShowMarket(ByVal RefCode As String)
        Try
            Dim chkSel As CheckBox
            Dim lblcode As Label
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from trfplisth_market  Where tplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        For Each Me.gvRow1 In gv_Market.Rows
                            chkSel = gvRow1.FindControl("chkSelect")
                            lblcode = gvRow1.FindControl("lblcode")
                            If CType(mySqlReader("plgrpcode"), String) = CType(lblcode.Text, String) Then
                                chkSel.Checked = True
                                Exit For
                            End If
                        Next
                    End If
                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Public Sub FillMarket()"
    Public Sub FillMarket()
        Me.pnlMarket.Visible = True
        Me.Panel1.Visible = True
        FillGridMarket("plgrpcode")
        gv_Market.Visible = True
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try

            If ddlSPType.Items(ddlSPType.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Type can not be blank.');", True)
                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCD.ClientID + "');", True)
                Exit Function
            End If
            If ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier can not be blank.');", True)
                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSuppierCD.ClientID + "');", True)
                Exit Function
            End If

            If ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier Agent can not be blank.');", True)
                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSupplierAgent.ClientID + "');", True)
                Exit Function
            End If

            If ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sub season can not be blank.');", True)
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSubSeas.ClientID + "');", True)
                Exit Function
            End If


            Dim chksel As CheckBox
            Dim flg As Boolean
            flg = False
            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                If chksel.Checked = True Then
                    flg = True
                End If
            Next

            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
                ValidatePage = False
                Exit Function
            End If

            If ViewState("TransferpricelistState") = "New" Or ViewState("TransferpricelistState") = "Copy" Then
                If ddlServerType.SelectedValue = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Transfer Type');", True)
                    ValidatePage = False
                    Exit Function
                End If
            End If


            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("TrfPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As New EncryptionDecryption
        Try

            '  If CType(ViewState("TransferpricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            ' End If


            Dim SptypeCode As String = obj.Encrypt(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SptypeName As String = obj.Encrypt(CType(ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text, String), "&%#@?,:*")


            'Dim MarketCode As String = obj.Encrypt(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim MarketName As String = obj.Encrypt(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierCode As String = obj.Encrypt(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierName As String = obj.Encrypt(CType(ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")

            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierAgentCode As String = obj.Encrypt(CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierAgentName As String = obj.Encrypt(CType(ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text, String), "&%#@?,:*")


            Dim chksel As CheckBox
            Dim marketstr As String = ""
            Dim lblcode As Label

            For Each Me.gvRow1 In gv_Market.Rows
                chksel = gvRow1.FindControl("chkSelect")
                lblcode = gvRow1.FindControl("lblcode")
                If chksel.Checked = True Then
                    marketstr = marketstr + ";" + lblcode.Text
                End If
            Next


            If marketstr.Length > 0 Then
                marketstr = marketstr.Substring(1, marketstr.Length - 1)
            End If
            'Dim frmdate As String = obj.Encrypt(dpFromdate.txtDate.Text, "&%#@?,:*")
            'Dim todate As String = obj.Encrypt(dpToDate.txtDate.Text, "&%#@?,:*")
            '"&SptypeCode=" & ViewState("TransferpricelistRefCode") &
            Session("sessionRemark") = txtRemark.Value
            Response.Redirect("TrfPriceList2.aspx?&State=" & ViewState("TransferpricelistState") & "&RefCode=" & ViewState("TransferpricelistRefCode") &
                               "&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&SupplierCode=" & SupplierCode &
                              "&SupplierName=" & SupplierName & "&SupplierAgentCode=" & SupplierAgentCode & "&SupplierAgentName=" & SupplierAgentName & "&PlcCode=" & PlcCode &
                              "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode &
                              "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0) &
                              "&Market=" & marketstr & "&GroupCode=" & GroupCode & "&transfertype=" & ddlServerType.SelectedIndex & "&conMarkUp=" & IIf(chkConsdierForMarkUp.Checked = True, 1, 0), False)
            '"&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName &"&frmdate=" & frmdate & "&todate=" & todate
            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("TrfPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("sessionRemark") = Nothing
        Session("GV_HotelData") = Nothing
        ViewState("TransferpricelistRefCode") = Nothing
        ViewState("TransferpricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack() = False Then

            ViewState.Add("TransferpricelistState", Request.QueryString("State"))
            ViewState.Add("TransferpricelistRefCode", Request.QueryString("RefCode"))

            'Dim sptype As String = ""
            'sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "564")
            'If sptype <> "" Then
            '    ddlSPTypeName.Value = sptype
            '    ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
            'End If
            ddlGroupCode.SelectedIndex = 0
            Dim supagentcode As String
            supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='520'")
            If supagentcode <> "" Then
                ddlSupplierAgentName.Value = CType(supagentcode, String)
                ddlSupplierAgentCode.Value = ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text
            End If

        Else
            If ddlSPType.Value <> "[Select]" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

            End If
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
        End If
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TrfPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        Dim chksel As CheckBox
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click
        Dim chksel As CheckBox
        For Each Me.gvRow1 In gv_Market.Rows
            chksel = gvRow1.FindControl("chkSelect")
            chksel.Checked = False
        Next
    End Sub

    
End Class
