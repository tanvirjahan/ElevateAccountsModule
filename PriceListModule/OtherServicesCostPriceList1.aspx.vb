'
'   Form Name       :       Other Services Cost Price List
'   Developer Name  :       Nilesh Sawant
'   Date            :       2 July 2008
'
'
'
'
#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class OtherServicesCostPriceList1
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

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
                Page.Title = Page.Title + " " + "New Other Services Cost Price List"
            ElseIf Request.QueryString("State") = "Copy" Then
                Page.Title = Page.Title + " " + "Copy Other Services Cost Price List"
            ElseIf Request.QueryString("State") = "Edit" Then
                Page.Title = Page.Title + " " + "Edit Other Services Cost Price List"
            ElseIf Request.QueryString("State") = "View" Then
                Page.Title = Page.Title + " " + "View Other Services Cost Price List"
            ElseIf Request.QueryString("State") = "Delete" Then
                Page.Title = Page.Title + " " + "Delete Other Services Cost Price List"
            End If

            Dim s As String = ""
            ViewState.Add("OthsercostpricelistState", Request.QueryString("State"))
            ViewState.Add("OthsercostpricelistRefCode", Request.QueryString("RefCode"))

            If IsPostBack = False Then
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                txtconnection.Value = Session("dbconnectionName")
                Dim supagentcode As String

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasCode, "subseascode", "subseasname", "select subseascode,subseasname from subseasmast where active=1 order by subseascode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubSeasName, "subseasname", "subseascode", "select subseasname,subseascode from subseasmast where active=1 order by subseasname", True)



                supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='520'")
                If supagentcode <> "" Then
                    ddlSupplierAgentName.Value = CType(supagentcode, String)
                    ddlSupplierAgentCode.Value = ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text
                End If

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


                If ViewState("OthsercostpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New Other Services Cost Price List"

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
                    If Request.QueryString("MarketCode") <> Nothing And Request.QueryString("MarketName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("MarketName")
                        ddlMarketCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("MarketCode")
                        ddlMarketName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                        s = ""
                        s = Request.QueryString("GroupName")
                        ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        s = ""
                        s = Request.QueryString("GroupCode")
                        ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

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
                    If Request.QueryString("frmdate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("frmdate")
                        dpFromdate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If
                    If Request.QueryString("todate") <> Nothing Then
                        s = ""
                        s = Request.QueryString("todate")
                        dpToDate.txtDate.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                    End If

                    If ddlSPType.Value <> "[Select]" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

                    End If
                    If ddlSupplierCode.Value <> "[Select]" Then
                        strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

                        strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGroupName.Value)
                    End If

                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthsercostpricelistState") = "Edit" Or ViewState("OthsercostpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("OthsercostpricelistRefCode"))
                    lblHeading.Text = "Edit Other Services Cost Price List"
                ElseIf ViewState("OthsercostpricelistState") = "View" Then
                ElseIf ViewState("OthsercostpricelistState") = "Delete" Then
                End If
                If ViewState("OthsercostpricelistState") = "Copy" Then
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

                    ddlMarketCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSubSeasCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSubSeasName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")



                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServicesPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("OthsercostpricelistState") = "New" Then
        ElseIf ViewState("OthsercostpricelistState") = "Edit" Or ViewState("OthsercostpricelistState") = "Copy" Then
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True

            ddlSupplierCode.Disabled = True
            ddlSupplierName.Disabled = True

            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True
        ElseIf ViewState("OthsercostpricelistState") = "Delete" Or ViewState("OthsercostpricelistState") = "View" Then
        End If


    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from oplist_costh Where ocplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("ocplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("ocplistcode")
                    End If

                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "sptypecode", "partycode", mySqlReader("partycode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", ddlSPTypeName.Value)

                        If ddlSPType.Value <> "[Select]" Then
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
                        End If
                        ddlSupplierName.Value = mySqlReader("partycode")
                        ddlSupplierCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))
                    End If
                    If ddlSupplierCode.Value <> "[Select]" Then
                        strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

                        strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                        "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
                    End If
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        ddlGroupName.Value = mySqlReader("othgrpcode")
                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    End If

                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        ddlMarketName.Value = mySqlReader("plgrpcode")
                        ddlMarketCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", mySqlReader("plgrpcode"))
                    End If


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
                        If IsDBNull(mySqlReader("frmdate")) = False Then
                            dpFromdate.txtDate.Text = Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy")
                            Session("sessionFrmdate") = dpFromdate.txtDate.Text
                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then
                            dpToDate.txtDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                            Session("sessionTodate") = dpToDate.txtDate.Text
                        End If
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


            End If


        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As New EncryptionDecryption
        Try


            If dpFromdate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field should not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpFromdate.ClientID + "');", True)
                Exit Sub
            End If
            If dpToDate.txtDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field should not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpToDate.ClientID + "');", True)
                Exit Sub
            End If
            If dpFromdate.txtDate.Text <> "" And dpToDate.txtDate.Text <> "" Then
                If ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(dpToDate.txtDate.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be grater than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + dpToDate.ClientID + "');", True)
                    Exit Sub
                End If
            End If
            '*********************************          **********************      ***************************

            Dim SptypeCode As String = obj.Encrypt(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SptypeName As String = obj.Encrypt(CType(ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text, String), "&%#@?,:*")


            Dim MarketCode As String = obj.Encrypt(CType(ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim MarketName As String = obj.Encrypt(CType(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierCode As String = obj.Encrypt(CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierName As String = obj.Encrypt(CType(ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            Dim GroupCode As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim GroupName As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")

            Dim SubSeasCode As String = obj.Encrypt(CType(ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SubSeasName As String = obj.Encrypt(CType(ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierAgentCode As String = obj.Encrypt(CType(ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text, String), "&%#@?,:*")
            Dim SupplierAgentName As String = obj.Encrypt(CType(ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text, String), "&%#@?,:*")

            Dim frmdate As String = obj.Encrypt(dpFromdate.txtDate.Text, "&%#@?,:*")
            Dim todate As String = obj.Encrypt(dpToDate.txtDate.Text, "&%#@?,:*")
            Session("sessionRemark") = txtRemark.Value
            Response.Redirect("OtherServicesCostPriceList2.aspx?&State=" & ViewState("OthsercostpricelistState") & "&RefCode=" & ViewState("OthsercostpricelistRefCode") & "&SptypeCode=" & ViewState("OthsercostpricelistRefCode") & "&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SupplierCode=" & SupplierCode & "&SupplierName=" & SupplierName & "&SupplierAgentCode=" & SupplierAgentCode & "&SupplierAgentName=" & SupplierAgentName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0) & "&frmdate=" & frmdate & "&todate=" & todate, False)
            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("sessionRemark") = Nothing
        Session("GV_HotelData") = Nothing
        ViewState("OthsercostpricelistRefCode") = Nothing
        ViewState("OthsercostpricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack() = False Then

            ViewState.Add("OthsercostpricelistState", Request.QueryString("State"))
            ViewState.Add("OthsercostpricelistRefCode", Request.QueryString("RefCode"))


        Else
            If ddlSPType.Value <> "[Select]" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partycode", True, ddlSupplierCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 and sptypecode='" & ddlSPType.Items(ddlSPType.SelectedIndex).Text & "' order by partyname", True, ddlSupplierName.Value)
            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partycode, partyname from partymast where active=1 order by partycode", True, ddlSupplierCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True, ddlSupplierName.Value)

            End If
            If ddlSupplierCode.Value <> "[Select]" Then
                strSqlQry = "select othgrpmast.othgrpcode,othgrpmast.othgrpname  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "' order by othgrpcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strSqlQry, True, ddlGroupCode.Value)

                strSqlQry = "select othgrpmast.othgrpname,othgrpmast.othgrpcode  from  othgrpmast left outer join  partyothgrp on othgrpmast.othgrpcode=partyothgrp.othgrpcode  where active=1 " & _
                "and partyothgrp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by othgrpname"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strSqlQry, True, ddlGroupName.Value)
            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True, ddlGroupCode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpname,othgrpcode from othgrpmast where active=1 order by othgrpname", True, ddlGroupName.Value)
            End If
        End If
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesCostPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub
End Class
'#Region "Private Sub ShowDynamicGrid()"
'    Private Sub ShowDynamicGrid(ByVal RefCode As String)
'        Try
'            cnt = 0
'            Dim StrQry As String = ""
'            '--------------------------------------------------------
'            ' Show Records From Details Table 
'            Dim GVROW As GridViewRow
'            cnt = gv_SearchResult.Columns.Count
'            Dim i As Long = 0
'            Dim n As Long = 0
'            Dim m As Long = 0
'            Dim txt1 As TextBox
'            Dim header As Long = 0
'            Dim heading(cnt + 1) As String
'            Try
'                For header = 0 To cnt - 4
'                    txt1 = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'                    heading(header) = txt1.Text
'                Next
'            Catch ex As Exception

'            End Try

'            StrQry = "select * from oplist_costd where ocplistcode='" & RefCode & "' order by oclineno,othtypcode"
'            Dim txt As TextBox
'            Dim headerlabel As New TextBox
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            Dim s As Long
'            Dim k As Long = 0
'            Dim a As Long = cnt - 7
'            Dim b As Long = 0
'            Dim othtypcode As String
'            Dim value As String

'            For Each GVROW In gv_SearchResult.Rows
'                '  If mySqlReader.Read = True Then
'                If n = 0 Then
'                    For i = 0 To cnt - 4
'                        If heading(i) = "From Date" Or heading(i) = "To Date" Or heading(i) = "Pkg" Then
'                        Else
'                            If mySqlReader.Read = True Then
'                                othtypcode = mySqlReader("othcatcode")
'                                If IsDBNull(mySqlReader("ocostprice")) = False Then
'                                    value = mySqlReader("ocostprice")
'                                Else
'                                    value = ""
'                                End If

'                                For s = 0 To cnt - 4
'                                    headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                    If headerlabel.Text = othtypcode Then
'                                        txt = GVROW.FindControl("txt" & i)
'                                        If value = "" Then
'                                            txt.Text = ""
'                                        Else
'                                            txt.Text = value
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 1)
'                                        If IsDBNull(mySqlReader("frmdate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If

'                                        txt = GVROW.FindControl("txt" & b + a + 2)
'                                        If IsDBNull(mySqlReader("todate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 3)
'                                        If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                            txt.Text = mySqlReader("pkgnights")
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        GoTo go1
'                                    End If

'                                Next
'                            End If
'                        End If

'go1:                Next
'                    m = i
'                Else
'                    k = 0
'                    For i = n To (m + n) - 1
'                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                        Else
'                            k = k + 1
'                            If mySqlReader.Read = True Then
'                                othtypcode = mySqlReader("othcatcode")
'                                If IsDBNull(mySqlReader("ocostprice")) = False Then
'                                    value = mySqlReader("ocostprice")
'                                Else
'                                    value = ""
'                                End If

'                                For s = 0 To cnt - 4
'                                    headerlabel = gv_SearchResult.HeaderRow.FindControl("txtHead" & s)
'                                    If headerlabel.Text = othtypcode Then
'                                        txt = GVROW.FindControl("txt" & i)
'                                        If value = "" Then
'                                            txt.Text = ""
'                                        Else
'                                            txt.Text = value
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 1)
'                                        If IsDBNull(mySqlReader("frmdate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If

'                                        txt = GVROW.FindControl("txt" & b + a + 2)
'                                        If IsDBNull(mySqlReader("todate")) = False Then
'                                            txt.Text = ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate"))
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        txt = GVROW.FindControl("txt" & b + a + 3)
'                                        If IsDBNull(mySqlReader("pkgnights")) = False Then
'                                            txt.Text = mySqlReader("pkgnights")
'                                        Else
'                                            txt.Text = ""
'                                        End If
'                                        GoTo goto2
'                                    End If

'                                Next
'                            End If

'                        End If

'goto2:              Next
'                End If
'                n = i
'                b = i
'                ' End If
'            Next

'            mySqlReader.Close()
'            mySqlConn.Close()
'            '  gv_SearchResult.Enabled = False
'            '-------------------------------------------
'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Sub
'#End Region

'#Region " Private Sub createdatatable()"
'    Private Sub createdatatable()
'        Try

'            cnt = 0

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            'strSqlQry = "select count(othcatcode) from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 "
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            cnt = mySqlCmd.ExecuteScalar
'            mySqlConn.Close()
'            If cnt > 0 Then
'                Session("CheckGridColumn") = ""
'            Else
'                Session("CheckGridColumn") = "Not Present"
'            End If


'            ReDim arr(cnt + 1)
'            ReDim arrRName(cnt + 1)
'            Dim i As Long = 0
'            Dim Column As Long = 0
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            'strSqlQry = "select othcatcode from  othcatmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and active=1"
'            strSqlQry = "SELECT  distinct  dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode FROM dbo.partyothcat INNER JOIN  dbo.othcatmast ON dbo.partyothcat.othcatcode = dbo.othcatmast.othcatcode WHERE (dbo.othcatmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and (partyothcat.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "') and othcatmast.active=1 Order by (othcatmast.othcatcode)"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            While mySqlReader.Read = True
'                arr(Column) = mySqlReader("othcatcode")
'                Column = Column + 1
'            End While
'            mySqlReader.Close()
'            mySqlConn.Close()
'            'select rmcatcode from partyrmcat where partycode='3'
'            'Here in Array store room types
'            '-------------------------------------
'            Dim tf As New TemplateField
'            dt.Columns.Add(New DataColumn("Sr No", GetType(String)))
'            dt.Columns.Add(New DataColumn("Service Type Code", GetType(String)))
'            dt.Columns.Add(New DataColumn("Service Type Name", GetType(String)))

'            'create columns of this room types in data table
'            For i = 0 To Column - 1
'                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
'            Next
'            dt.Columns.Add(New DataColumn("From Date", GetType(String)))
'            dt.Columns.Add(New DataColumn("To Date", GetType(String)))
'            dt.Columns.Add(New DataColumn("Pkg", GetType(String)))
'            Session("GV_HotelData") = dt

'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Sub
'#End Region


'#Region "Private Sub createdatarows()"
'    Private Sub createdatarows()
'        Dim i As Long = 0
'        Dim k As Long = 0
'        Try
'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            'strSqlQry = "select count(othtypcode) from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and inactive=0"
'            strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "') and  (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "')  and othtypmast.active=1"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            cnt = mySqlCmd.ExecuteScalar
'            mySqlConn.Close()

'            Dim arr_rows(cnt + 1) As String
'            Dim arr_rname(cnt + 1) As String

'            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
'            'strSqlQry = "select othtypcode,othtypname from othtypmast where othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' and inactive=0"
'            strSqlQry = "SELECT  distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode FROM partyothtyp INNER JOIN othtypmast ON partyothtyp.othtypcode = othtypmast.othtypcode WHERE (othtypmast.othgrpcode = '" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "')  and (partyothtyp.partycode='" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "')  and othtypmast.active=1 order by othtypmast.othtypcode"
'            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
'            mySqlReader = mySqlCmd.ExecuteReader()
'            While mySqlReader.Read = True
'                arr_rows(k) = mySqlReader("othtypcode")
'                arr_rname(k) = mySqlReader("othtypname")
'                k = k + 1
'            End While
'            mySqlReader.Close()
'            mySqlConn.Close()


'            'Here add rows.....
'            'Now Get Rows From sellmast Based on SPType
'            Session("GV_HotelData") = dt
'            Dim dr As DataRow

'            Dim row As Long

'            For row = 0 To k - 1
'                dr = dt.NewRow
'                'For i = 1 To cnt - 1
'                dr("Sr No") = row + 1   ' 
'                dr("Service Type Code") = arr_rows(row)
'                dr("Service Type Name") = arr_rname(row)
'                'dr(arr(i)) = ""
'                'Next
'                dt.Rows.Add(dr)
'            Next


'            gv_SearchResult.Visible = True
'            gv_SearchResult.DataSource = dt
'            'InstantiateIn Grid View
'            gv_SearchResult.DataBind()
'        Catch ex As Exception
'            If mySqlConn.State = ConnectionState.Open Then
'                mySqlConn.Close()
'            End If
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try

'    End Sub
'#End Region

'#Region "Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub BtnClearFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClearFormula.Click
'        Try
'            gv_SearchResult.Visible = False
'            BtnClearFormula.Visible = False
'            btnGenerate.Visible = True
'            Panel1.Visible = False
'            EnableAllControls()
'            lblError.Visible = False
'        Catch ex As Exception
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Sub
'#End Region

'#Region "Private Function ValidatePage() As Boolean"
'    Private Function ValidatePage() As Boolean
'        Dim j As Long = 0
'        Dim txt As TextBox
'        Dim cnt As Long
'        Dim srno As Long = 0
'        Dim hotelcategory As String = ""
'        Dim n As Long = 0
'        Dim k As Long = 0
'        Dim a As Long = cnt - 7
'        Dim b As Long = 0
'        Dim header As Long = 0
'        Dim GvRow As GridViewRow
'        Dim FrmDate As Date = ObjDate.ConvertDateromTextBoxToDatabase(dpFromdate.txtDate.Text)
'        Dim GrdFrmDate As Date
'        Dim Flag As Boolean = False
'        Try
'            gv_SearchResult.Visible = True
'            cnt = gv_SearchResult.Columns.Count
'            Dim heading(cnt + 1) As String
'            '----------------------------------------------------------------------------
'            '           Stoaring heading column values in the array
'            For header = 0 To cnt - 4
'                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'                heading(header) = txt.Text
'            Next
'            '----------------------------------------------------------------------------
'            Dim m As Long = 0
'            For Each GvRow In gv_SearchResult.Rows
'                If n = 0 Then
'                    For j = 0 To cnt - 4
'                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
'                        Else
'                            txt = GvRow.FindControl("txt" & j)
'                            If txt.Text <> "" Then
'                                Flag = True
'                                GoTo Flag
'                            End If
'                        End If
'                    Next
'                    m = j
'                Else
'                    k = 0
'                    For j = n To (m + n) - 1
'                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                        Else

'                            txt = GvRow.FindControl("txt" & j)
'                            If txt.Text <> "" Then
'                                Flag = True
'                                GoTo Flag
'                            End If
'                            k = k + 1
'                        End If
'                    Next
'                End If
'                b = j
'                n = j
'            Next
'Flag:       If Flag = False Then
'                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Grid Rates can not be left blank.');", True)
'                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
'                ValidatePage = False
'                Exit Function
'            End If


'            '-----------------------------------------------------------------------------
'            a = cnt - 7
'            j = 0
'            b = 0
'            m = 0
'            n = 0
'            For Each GvRow In gv_SearchResult.Rows
'                If n = 0 Then
'                    For j = 0 To cnt - 4
'                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
'                        Else
'                            'txt = GvRow.FindControl("txt" & j)
'                            txt = GvRow.FindControl("txt" & b + a + 1)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < FrmDate Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the From dates  Should be Greater Than or Equal to Header From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If
'                            GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'                            txt = GvRow.FindControl("txt" & b + a + 2)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(GrdFrmDate, Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the To dates  Should be Greater Than or Equal to Grids From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If

'                            txt = GvRow.FindControl("txt" & b + a + 3)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            End If
'                        End If
'                    Next

'                    m = j
'                Else
'                    k = 0
'                    For j = n To (m + n) - 1
'                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                        Else

'                            txt = GvRow.FindControl("txt" & j)

'                            txt = GvRow.FindControl("txt" & b + a + 1)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates From date can not be left blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) < FrmDate Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the From dates  Should be Greater Than or Equal to Header From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If

'                            GrdFrmDate = CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date)
'                            txt = GvRow.FindControl("txt" & b + a + 2)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates to dates can not be blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            Else
'                                If CType(GrdFrmDate, Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txt.Text), Date) Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In Grid Rates all the To dates  Should be Greater Than or Equal to Grids From Date.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                    ValidatePage = False
'                                    Exit Function
'                                End If
'                            End If
'                            txt = GvRow.FindControl("txt" & b + a + 3)
'                            If txt.Text = "" Then
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Package Cannot be Blank.');", True)
'                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txt.ClientID + "');", True)
'                                ValidatePage = False
'                                Exit Function
'                            End If
'                            k = k + 1
'                        End If
'                    Next
'                End If
'                b = j
'                n = j
'            Next
'            '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
'            ValidatePage = True
'        Catch ex As Exception
'            Response.Write(ex)
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Function
'#End Region


'#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
'        Try
'            If Page.IsValid = True Then
'                If Session("State") = "Delete" Then
'                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

'                    mySqlCmd = New SqlCommand("sp_del_oplist_costh", mySqlConn, sqlTrans)
'                    mySqlCmd.CommandType = CommandType.StoredProcedure
'                    mySqlCmd.Parameters.Add(New SqlParameter("@ocplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'                    mySqlCmd.ExecuteNonQuery()
'                    mySqlCmd = New SqlCommand("sp_del_oplist_costd", mySqlConn, sqlTrans)
'                    mySqlCmd.CommandType = CommandType.StoredProcedure
'                    mySqlCmd.Parameters.Add(New SqlParameter("@ocplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
'                    mySqlCmd.ExecuteNonQuery()
'                    sqlTrans.Commit()    'SQl Tarn Commit
'                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
'                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
'                    Session("sessionRemark") = ""
'                    Session("GV_HotelData") = ""
'                    Session("RefCode") = ""
'                    Session("State") = ""
'                    Response.Redirect("OtherServicesCostPriceListSearch.aspx", False)
'                End If
'            End If
'        Catch ex As Exception
'            sqlTrans.Rollback()
'            mySqlConn.Close()
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Sub
'#End Region


'#Region "Private Function CheckDecimalInGrid() As Boolean"
'    Private Function CheckDecimalInGrid() As Boolean
'        Dim j As Long = 0
'        Dim txt As TextBox
'        Dim cnt As Long
'        Dim srno As Long = 0
'        Dim hotelcategory As String = ""
'        Dim n As Long = 0
'        Dim k As Long = 0
'        Dim a As Long = cnt - 7
'        Dim b As Long = 0
'        Dim header As Long = 0
'        Dim arr() As String
'        Dim GvRow As GridViewRow
'        Try
'            gv_SearchResult.Visible = True
'            cnt = gv_SearchResult.Columns.Count
'            Dim heading(cnt + 1) As String
'            '----------------------------------------------------------------------------
'            '           Stoaring heading column values in the array
'            For header = 0 To cnt - 4
'                txt = gv_SearchResult.HeaderRow.FindControl("txtHead" & header)
'                heading(header) = txt.Text
'            Next
'            '----------------------------------------------------------------------------
'            Dim m As Long = 0
'            For Each GvRow In gv_SearchResult.Rows
'                If n = 0 Then
'                    For j = 0 To cnt - 4
'                        If heading(j) = "From Date" Or heading(j) = "To Date" Or heading(j) = "Pkg" Then
'                        Else
'                            txt = GvRow.FindControl("txt" & j)
'                            If txt.Text <> "" Then
'                                arr = txt.Text.Split(".")
'                                If arr.Length > 2 Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
'                                    CheckDecimalInGrid = False
'                                    Exit Function
'                                End If
'                            End If
'                        End If
'                    Next
'                    m = j
'                Else
'                    k = 0
'                    For j = n To (m + n) - 1
'                        If heading(k) = "From Date" Or heading(k) = "To Date" Or heading(k) = "Pkg" Then
'                        Else
'                            txt = GvRow.FindControl("txt" & j)
'                            If txt.Text <> "" Then
'                                arr = txt.Text.Split(".")
'                                If arr.Length > 2 Then
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Valid decimal values in Grid Rates.');", True)
'                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + gv_SearchResult.ClientID + "');", True)
'                                    CheckDecimalInGrid = False
'                                    Exit Function
'                                End If
'                            End If
'                            k = k + 1
'                        End If
'                    Next
'                End If
'                b = j
'                n = j
'            Next
'            CheckDecimalInGrid = True
'        Catch ex As Exception
'            Response.Write(ex)
'            objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'        End Try
'    End Function
'#End Region

'#Region "Numbers"
'    Public Sub Numbers(ByVal txtbox As TextBox)
'        Try
'            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
'        Catch ex As Exception

'        End Try
'    End Sub
'#End Region
'#Region "HTMLNumbers"
'    Public Sub HTMLNumbers(ByVal txtbox As HtmlInputText)
'        Try
'            txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
'        Catch ex As Exception

'        End Try
'    End Sub
'#End Region
'#Region "Characters"
'    Public Sub Characters(ByVal txtbox As HtmlInputText)
'        Try
'            txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
'        Catch ex As Exception

'        End Try
'    End Sub
'#End Region

'#Region "Private Sub EnableAllControls()"
'    Private Sub EnableAllControls()
'        ddlMarketCode.Disabled = False
'        ddlMarketName.Disabled = False
'        ddlGroupCode.Disabled = False
'        ddlGroupName.Disabled = False
'        ddlCurrencyCode.Disabled = False
'        ddlCurrencyName.Disabled = False
'        ddlSupplierCode.Disabled = False
'        ddlSupplierName.Disabled = False
'        ddlSubSeasCode.Disabled = False
'        ddlSubSeasName.Disabled = False
'        txtRemark.Disabled = False
'        ChkActive.Disabled = False
'        dpFromdate.Enabled = True
'        dpToDate.Enabled = True
'        ddlSupplierAgentCode.Disabled = False
'        ddlSupplierAgentName.Disabled = False
'    End Sub
'#End Region
'Dim currcode As String = obj.Encrypt(CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String), "&%#@?,:*")
'Dim currname As String = obj.Encrypt(CType(ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text, String), "&%#@?,:*")


'sp_chkopldate

'mySqlCmd = New SqlCommand("sp_chkopldate", mySqlConn)
'mySqlCmd.CommandText = "sp_chkopldate"
'mySqlCmd.CommandType = CommandType.StoredProcedure
'mySqlCmd.Connection = mySqlConn
'mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar))
'mySqlCmd.Dispose()
'mySqlReader.Close()
'ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = mySqlReader("currcode")
'                      ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", mySqlReader("currcode"))


'If mySqlConn.State = ConnectionState.Open Then
'End If

'Dim SqlCmd As SqlCommand
'Dim SqlReader As SqlDataReader
'Dim sqlTrans As SqlTransaction
'Dim StrQry As String
'Dim SqlCon As SqlConnection
'Dim mySqlAdapter As SqlDataAdapter
'Dim ObjCon As New clsDBConnect

''Dim MyDS As New DataSet
'Dim Table As New DataTable()
'Dim ParameterArray As New ArrayList()
'Private dt As New DataTable
'Private cnt As Long
'Private arr(1) As String
'Private arrRName(1) As String
'Dim GvRow As String

'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)


'If Request.QueryString("SellTypeName") <> Nothing Then
'    s = ""
'    s = Request.QueryString("SellTypeName")
'    ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'End If

'If Request.QueryString("remarks") <> Nothing Then
'End If

'ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                       ddlCurrencyName.Items(ddlCurrencyName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")

' Response.Redirect("OtherServicesCostPriceList2.aspx?&MarketCode=" & MarketCode & "&MarketName=" & MarketName & "&SupplierCode=" & SupplierCode & "&SupplierName=" & SupplierName & "&SupplierAgentCode=" & SupplierAgentCode & "&SupplierAgentName=" & SupplierAgentName & "&PlcCode=" & PlcCode & "&GroupCode=" & GroupCode & "&GroupName=" & GroupName & "&currcode=" & currcode & "&currname=" & currname & "&SubSeasCode=" & SubSeasCode & "&SubSeasName=" & SubSeasName & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&frmdate=" & frmdate & "&todate=" & todate, False)

'createdatatable()

''fill controls from previous form
'' Now  Bind Column Dynamically 
'dt = Session("GV_HotelData")
'Dim fld2 As String = ""
'Dim col As DataColumn
'For Each col In dt.Columns
'    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'        Dim bfield As New TemplateField
'        'Call Function
'        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'        gv_SearchResult.Columns.Add(bfield)

'    End If

'Next
'gv_SearchResult.Visible = True
'gv_SearchResult.DataSource = dt
''InstantiateIn Grid View
'gv_SearchResult.DataBind()

'' dt = Session("GV_HotelData")
'' createdatatable()

''fill controls from previous form
'' Now  Bind Column Dynamically 

'Dim fld2 As String = ""
'Dim col As DataColumn
'For Each col In dt.Columns
'    If col.ColumnName <> "Sr No" And col.ColumnName <> "Service Type Code" And col.ColumnName <> "Service Type Name" Then
'        Dim bfield As New TemplateField
'        'Call Function
'        bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
'        bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
'        gv_SearchResult.Columns.Add(bfield)

'    End If

'Next
'gv_SearchResult.Visible = True
'gv_SearchResult.DataSource = dt
''InstantiateIn Grid View
'gv_SearchResult.DataBind()
'BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Cleare formula?')==false)return false;")

' ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
'ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

'btnGenerate.Visible = False
'SetFocus(ddlMarketCode)
'BtnClearFormula.Visible = False
'ShowRecord(Session("RefCode"))
'DisableAllControls()
'lblHeading.Text = "View Other Services Cost Price List"

'btnSave.Visible = True
'btnGenerate.Visible = False
'btnGenerate.Attributes.Add("onclick", "return FormValidation('Delete')")
'lblHeading.Text = "Delete Other Services Cost Price List"
''btnSave.Text = "Delete"
'ShowRecord(Session("RefCode"))
'BtnClearFormula.Visible = False
'DisableAllControls()

' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete other service cost price list?')==false)return false;")

'gv_SearchResult.Visible = True
'BtnClearFormula.Visible = False

'ddlMarketCode.Disabled = True
'ddlMarketName.Disabled = True
'ddlGroupCode.Disabled = True
'ddlGroupName.Disabled = True
'txtCurrCode.Enabled = False
'txtCurrName.Enabled = False

'ddlSubSeasCode.Disabled = True
'ddlSubSeasName.Disabled = True
'txtRemark.Disabled = True
'ChkActive.Disabled = True
'dpFromdate.txtDate.Enabled = False
'dpToDate.txtDate.Enabled = False
'ddlSupplierAgentCode.Disabled = True
'ddlSupplierAgentName.Disabled = True

'btnGenerate.Visible = False
' dpFromdate.txtDate.Text = Session("sessionFrmdate")
'/'dpTodate.txtDate.Text = Session("sessionTodate")


'Try
'    If IsPostBack = False Then
'        'If Session("CheckGridColumn") = "Not Present" Then
'        '    Panel1.Visible = False
'        '    lblError.Visible = True
'        '    lblError.Text = "Rates are not present of this selected Group Code. Click Clear Prices for Changing Group."
'        'Else
'        '    Panel1.Visible = True
'        '    'createdatarows()
'        '    lblError.Visible = False
'        'End If
'        If Session("State") <> "New" Then
'            '    'ShowDynamicGrid(Session("RefCode"))
'            '    gv_SearchResult.Visible = True
'            '    gv_SearchResult.Enabled = False
'            '    If gv_SearchResult.Rows.Count > 0 Then
'            '        Panel1.Visible = True
'            '    Else
'            '        Panel1.Visible = False
'            '    End If

'            'Else
'            lblError.Visible = False
'            Panel1.Visible = False
'            gv_SearchResult.Visible = False
'        End If
'    End If

'Catch ex As Exception
'    objUtils.WritErrorLog("OtherServicesCostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'End Try


'lblError.Visible = False
'Panel1.Visible = False
'gv_SearchResult.Visible = False

' BtnClearFormula.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear prices?')==false)return false;")

'If Request.QueryString("SptypeCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SptypeCode")
'                      ddlSPType.Items(ddlSPType.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SptypeName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SptypeName")
'                      ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  If Request.QueryString("SupplierCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SupplierCode")
'                      ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SupplierName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SupplierName")
'                      ddlSupplierName.Items(ddlSupplierName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("SupplierAgentCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SupplierAgentCode")
'                      ddlSupplierAgentCode.Items(ddlSupplierAgentCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  If Request.QueryString("SupplierAgentName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SupplierAgentName")
'                      ddlSupplierAgentName.Items(ddlSupplierAgentName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("MarketCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("MarketCode")
'                      ddlMarketCode.Items(ddlMarketCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("MarketName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("MarketName")
'                      ddlMarketName.Items(ddlMarketName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  txtRemark.Value = CType(Session("sessionRemark"), String)

'                  If Request.QueryString("PlcCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("PlcCode")
'                      txtPlcCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("GroupCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("GroupCode")
'                      ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("GroupName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("GroupName")
'                      ddlGroupName.Items(ddlGroupName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("currcode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("currcode")
'                      txtCurrCode.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
'                  If Request.QueryString("currname") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("currname")
'                      txtCurrName.Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  If Request.QueryString("SubSeasCode") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SubSeasCode")
'                      ddlSubSeasCode.Items(ddlSubSeasCode.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If

'                  If Request.QueryString("SubSeasName") <> Nothing Then
'                      s = ""
'                      s = Request.QueryString("SubSeasName")
'                      ddlSubSeasName.Items(ddlSubSeasName.SelectedIndex).Text = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
'                  End If
