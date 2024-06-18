#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
#End Region

Partial Class ExcursionsectorsuppliercostPriceList
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection

  
    Dim sqlTrans As SqlTransaction

    Dim gvRow1 As GridViewRow
    Dim myDataAdapter As SqlDataAdapter
    Dim otypecode1, otypecode2 As String
#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""


                If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then

                    strOption = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", Session("OthPListFilter"))

                    Dim sptype As String = ""
                    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", strSpType)
                    If sptype <> "" Then
                        ddlSPTypeName.Value = sptype
                        ddlSPType.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
                    End If

                    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                        " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

                ElseIf Session("OthPListFilter") = "OTH" Then
                    Dim sptypeQry As String = ""
                    sptypeQry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"
                    strtitle = "Other Service "
                    strqry = "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021)"

                End If
                grdDates.Visible = True
                fillDategrd(grdDates, True)
                If Request.QueryString("State") = "New" Then
                    Page.Title = Page.Title + " " + "New " + strtitle + " Price List"

                    btnsave.Style.Add("display", "none")
                    btncancel.Style.Add("display", "none")

                    txtCurrCode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =928")
                    txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", txtCurrCode.Text)
                   
                ElseIf Request.QueryString("State") = "Copy" Then
                    Page.Title = Page.Title + " " + "Copy " + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "Edit" Then
                    Page.Title = Page.Title + " " + "Edit " + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "View" Then
                    Page.Title = Page.Title + " " + "View" + strtitle + "  Price List"
                ElseIf Request.QueryString("State") = "Delete" Then
                    Page.Title = Page.Title + " " + "Delete " + strtitle + "  Price List"
                End If

                Dim s As String = ""
                ViewState.Add("OthpricelistState", Request.QueryString("State"))
                ViewState.Add("OthpricelistRefCode", Request.QueryString("RefCode"))

                txtconnection.Value = Session("dbconnectionName")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypcode,othtypname from othtypmast inner join othgrpmast on othgrpmast.othgrpcode =othtypmast.othgrpcode  where othtypmast.active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", "select othtypname,othtypcode from othtypmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othtypname", True)



                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname from othgrpmast  where active=1 and othmaingrpcode=(select option_selected from reservation_parameters where param_id ='" & Session("OthPListFilter") & "') order by othgrpname", True)
                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "') and active=1 order by othgrpname", True)



                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("OthpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption

                    SetFocus(ddlSPType)

                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    'check, becz it gets populated if any value persists in session /qs
                    If Session("ClearPrice") = "Yes" Then
                        If Request.QueryString("SptypeCode") <> Nothing And Request.QueryString("SptypeName") <> Nothing Then
                            s = ""
                            s = Request.QueryString("SptypeName")
                            ddlSPType.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("SptypeCode")
                            ddlSPTypeName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        End If

                        If Request.QueryString("Airportcd") <> Nothing And Request.QueryString("Airportnm") <> Nothing Then
                            s = ""
                            s = Request.QueryString("Airportnm")
                            ddlexccode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("Airportcd")
                            ddlexcname.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                        End If

                        If Request.QueryString("GroupCode") <> Nothing And Request.QueryString("GroupName") <> Nothing Then
                            s = ""
                            s = Request.QueryString("GroupName")
                            ddlGroupCode.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
                            s = ""
                            s = Request.QueryString("GroupCode")
                            ddlGroupName.Value = obj.Decrypt(s.Replace(" ", "+"), "&%#@?,:*")
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

                    End If



                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ShowRecord(ViewState("OthpricelistRefCode"))
                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf ViewState("OthpricelistState") = "View" Then
                    ShowRecord(ViewState("OthpricelistRefCode"))
                ElseIf ViewState("OthpricelistState") = "Delete" Then
                    ShowRecord(ViewState("OthpricelistRefCode"))
                End If
                If ViewState("OthpricelistState") = "Copy" Then
                    txtPlcCode.Value = ""
                End If
                DisableAllControls()
                TextLock(txtCurrCode)
                TextLock(txtCurrName)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlSPType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    'ddlexccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlexcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    'ddlGroupCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlGroupName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub DisableAllControls()"
    Private Sub DisableAllControls()
        If ViewState("OthpricelistState") = "New" Then
        ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
            ddlSPType.Disabled = False
            ddlSPTypeName.Disabled = False

            ddlGroupCode.Disabled = False
            ddlGroupName.Disabled = False

            ddlexccode.Disabled = False
            ddlexcname.Disabled = False

            txtRemark.Disabled = False


        ElseIf ViewState("OthpricelistState") = "Delete" Or ViewState("OthpricelistState") = "View" Then
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True

            ddlGroupCode.Disabled = True
            ddlGroupName.Disabled = True
            txtRemark.Disabled = True
            ddlSPType.Disabled = True
            ddlSPTypeName.Disabled = True

        End If

        'txtCurrCode.Enabled = False
        'txtCurrName.Enabled = False

    End Sub
#End Region


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excplist_header Where eplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then


                    If IsDBNull(mySqlReader("eplistcode")) = False Then
                        txtPlcCode.Value = mySqlReader("eplistcode")
                    End If

                    If IsDBNull(mySqlReader("esellcode")) = False Then
                        ddlSPTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "esellcode", "eplistcode", mySqlReader("eplistcode"))
                        ddlSPType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excsellmast", "excsellname", "excsellcode", ddlSPTypeName.Value)

                    End If

                    If IsDBNull(mySqlReader("gpcode")) = False Then
                        ddlGroupName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "gpcode", "eplistcode", mySqlReader("eplistcode"))
                        ddlGroupCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", ddlGroupName.Value)

                    End If




                    If mySqlReader("approved") = "1" Then
                        chkapprove.Checked = True
                    Else
                        chkapprove.Checked = False
                    End If



                    'check
                    'ddlexcname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "excplist_header", "airportcode", "tplistcode", mySqlReader("tplistcode"))
                    'ddlexccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "airportbordersmaster", "airportbordername", "airportbordercode", ddlexcname.Value)








                    If ddlexccode.Value <> "[Select]" Then


                    End If


                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrCode.Text = mySqlReader("currcode")
                        txtCurrName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
                    End If
                    If ViewState("OthsercostpricelistState") <> "Copy" Then

                    End If


                End If



            End If


        Catch ex As Exception
            objUtils.WritErrorLog("excPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try

            'If ddlSPType.Items(ddlSPType.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selling  Type can not be blank.');", True)
            '    '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSPTypeCD.ClientID + "');", True)
            '    Return False
            '    Exit Function
            'End If


            'If ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group Code can not be blank.');", True)
            '    '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlSupplierAgent.ClientID + "');", True)
            '    Return False
            '    Exit Function
            'End If

            If txtgroupname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Group  can not be blank.');", True)

                Return False
                Exit Function
            End If

            ValidatePage = True

        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            ValidatePage = False
        End Try

    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim obj As New EncryptionDecryption
        Try
            Dim myDS As New DataSet

            'If CType(ViewState("OthpricelistState"), String) = "New" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'End If


            strSqlQry = "select  h.exctypcode,h.exctypname,h.ratebasis,d.seniorallowed from excursiontypes h (nolock) left join excursiontypes_occupancy  d on h.exctypcode=d.exctypcode " _
                & " inner join  excursiontypes_suppliers es on h.exctypcode =es.exctypcode inner join exctypes_sectorgrp ec on h.exctypcode=ec.exctypcode  where  isnull(h.sectorwiserates,'')='YES' " _
                & " and active=1 and ec.sectorgrpcode='" & txtsectorcode.Text & "' and  h.classificationcode='" & txtgroupcode.Text & "' and es.partycode='" & txtsuppcode.Text & "'   and isnull(h.multicost,'') ='NO'"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdExrates.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                btnGenerate.Style.Add("display", "none")
                btnsave.Style.Add("display", "block")
                btncancel.Style.Add("display", "block")
                grdExrates.DataBind()
            Else
                btnGenerate.Style.Add("display", "block")
                btnsave.Style.Add("display", "none")
                btncancel.Style.Add("display", "none")

                grdExrates.PageIndex = 0
                grdExrates.DataBind()

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('For This Supplier There is No  Supplier Linked');", True)



            End If

            'Dim SptypeCode As String = obj.Encrypt(CType(ddlSPType.Items(ddlSPType.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim SptypeName As String = obj.Encrypt(CType(ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text, String), "&%#@?,:*")

            'Dim PlcCode As String = obj.Encrypt(CType(txtPlcCode.Value, String), "&%#@?,:*")
            'Dim Groupcd As String = obj.Encrypt(CType(ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim Groupnm As String = obj.Encrypt(CType(ddlGroupName.Items(ddlGroupName.SelectedIndex).Text, String), "&%#@?,:*")

            'Dim currcode As String = obj.Encrypt(CType(txtCurrCode.Text, String), "&%#@?,:*")
            'Dim currname As String = obj.Encrypt(CType(txtCurrName.Text, String), "&%#@?,:*")


            'Dim exccode As String = obj.Encrypt(CType(ddlexccode.Items(ddlexccode.SelectedIndex).Text, String), "&%#@?,:*")
            'Dim excname As String = obj.Encrypt(CType(ddlexcname.Items(ddlexcname.SelectedIndex).Text, String), "&%#@?,:*")


            ''"&SptypeCode=" & ViewState("TransferpricelistRefCode") &
            'Session("sessionRemark") = txtRemark.Value
            ''vij


            'Response.Redirect("OthPriceList2.aspx?&State=" & ViewState("OthpricelistState") & "&RefCode=" & ViewState("OthpricelistRefCode") &
            '                  "&SptypeCode=" & SptypeCode & "&SptypeName=" & SptypeName & "&PlcCode=" & PlcCode & "&currcode=" & currcode &
            '                  "&currname=" & currname & "&GroupCode=" & Groupcd & "&GroupName=" & Groupnm & "&exccode=" & exccode &
            '                  "&Airportnm=" & excname & "&Acvtive=" & IIf(ChkActive.Checked = True, 1, 0) & "&approved=" & IIf(chkapprove.Checked = True, 1, 0), False)


            '*********************************          **********************      ***************************
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getsectorlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim sectorname As New List(Of String)
        Try

            strSqlQry = "select othtypcode,othtypname  from  othtypmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1001) and  othtypname like  '" & Trim(prefixText) & "%' order by othtypname "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    sectorname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othtypname").ToString(), myDS.Tables(0).Rows(i)("othtypcode").ToString()))

                Next

            End If

            Return sectorname
        Catch ex As Exception
            Return sectorname
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppname As New List(Of String)
        Try

            strSqlQry = "select partycode,partyname  from  partymast where active=1 and sptypecode in (select option_selected from reservation_parameters where param_id=1033) and  partyname like  '" & Trim(prefixText) & "%' order by partyname "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    suppname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next

            End If

            Return suppname
        Catch ex As Exception
            Return suppname
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getcurrlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim currname As New List(Of String)
        Try

            strSqlQry = "select currcode,currname  from  currmast where active=1 and convrate <>0  and currname like  '" & Trim(prefixText) & "%' order by currname "

            '   strSqlQry = "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where  " _
            '      & " othmaingrpcode in(select option_selected from reservation_parameters where param_id in (1104,1105)) and active=1 and othgrpname like '" & Trim(prefixText) & "%' order by othgrpname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    currname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))

                Next

            End If

            Return currname
        Catch ex As Exception
            Return currname
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getgrouplist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim groupname As New List(Of String)
        Try

            strSqlQry = "select classificationname othgrpname, classificationcode othgrpcode from  excclassification_header where active=1 and classificationname like  '" & Trim(prefixText) & "%' order by classificationname "

            '   strSqlQry = "select rtrim(ltrim(othgrpcode))othgrpcode,rtrim(ltrim(othgrpname))othgrpname from othgrpmast where  " _
            '      & " othmaingrpcode in(select option_selected from reservation_parameters where param_id in (1104,1105)) and active=1 and othgrpname like '" & Trim(prefixText) & "%' order by othgrpname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    groupname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othgrpname").ToString(), myDS.Tables(0).Rows(i)("othgrpcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return groupname
        Catch ex As Exception
            Return groupname
        End Try

    End Function
    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("DELETE", row.RowIndex)
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        count = gvchildage.Rows.Count + 1

        Dim GVRow As GridViewRow
        '  count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdDates.Rows
                '  chkSelect = GVRow.FindControl("chkSelect")
                'If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

                k = k + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdDates.Rows.Count > 1 Then
                fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDates, False, grdDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)
                    'lblseason = GVRow.FindControl("lblseason")
                    'lblseason.Text = excl(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try


    End Sub
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        dt.Columns.Add(New DataColumn("SeasonName", GetType(String)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("ADD", 0)
        '    row.FindControl("imgStayAdd").Visible = False
        '    Dim txtfromdate As TextBox
        '    txtfromdate = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromdate"), TextBox)
        '    txtfromdate.Focus()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


        Dim count As Integer
        Dim GVRow As GridViewRow

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        count = gvchildage.Rows.Count + 1

        'count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Try
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                'lblseason = GVRow.FindControl("lblseason")
                'lblseason.Text = excl(n)

                n = n + 1
            Next

            'For Each GVRow In grdDates.Rows
            '    lblseason = GVRow.FindControl("lblseason")
            '    If lblseason.Text = "" Then
            '        lblseason.Text = txtseasonname.Text
            '    End If
            'Next

            Dim txtStayFromDt As TextBox
            txtStayFromDt = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("sessionRemark") = Nothing
        Session("GV_OthHotelData") = Nothing
        ViewState("OthpricelistRefCode") = Nothing
        ViewState("OthpricelistState") = Nothing
        ' Response.Redirect("OtherServicesCostPriceListSearch.aspx")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack() = False Then

            ViewState.Add("OthpricelistState", Request.QueryString("State"))
            ViewState.Add("OthpricelistRefCode", Request.QueryString("RefCode"))


            Dim supagentcode As String
            supagentcode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1021'")

        Else

            'If ddlexccode.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "airportbordercode", "airportbordername", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlexccode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "airportbordername", "airportbordercode", "select airportbordername , airportbordercode from airportbordersmaster where active=1 order by airportbordername", True, ddlexcname.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "airportbordercode", "airportbordername", "select airportbordercode , airportbordername from airportbordersmaster where active=1 order by airportbordercode", True, ddlexccode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "airportbordername", "airportbordercode", "select airportbordername , airportbordercode from airportbordersmaster where active=1 order by airportbordername", True, ddlexcname.Value)
            'End If



            'If ddlSPType.Value <> "[Select]" Then
            '    strSqlQry = "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", strSqlQry, True, ddlSPType.Value)

            '    strSqlQry = "select excsellcode,excsellname from excsellmast where active=1 order by excsellname "
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", strSqlQry, True, ddlSPTypeName.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "excsellcode", "excsellname", "select excsellcode,excsellname from excsellmast where active=1 order by excsellcode", True, ddlSPType.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "excsellname", "excsellcode", "select excsellcode,excsellname from excsellmast where active=1 order by excsellname", True, ddlSPTypeName.Value)
            'End If

            'If ddlGroupCode.Value <> "[Select]" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) and othgrpcode='" & ddlGroupCode.Items(ddlGroupCode.SelectedIndex).Text & "' order by othgrpname", True, ddlGroupCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpcode", True, ddlGroupName.Value)
            'Else
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpname", True, ddlGroupCode.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", "select othgrpcode , othgrpname  from othgrpmast where active=1 and othgrpcode in (select option_selected from reservation_parameters where param_id=1021) order by othgrpcode", True, ddlGroupName.Value)
            'End If

        End If

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OthPriceList1','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function getFillRateType(ByVal prefixText As String) As List(Of String)
        Dim promotionlist As New List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Try


            strSqlQry = "select distinct namecode from extracodes  where type=2  and namecode like '" & Trim(prefixText) & "%' order by namecode "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    promotionlist.Add(myDS.Tables(0).Rows(i)("namecode").ToString())
                Next

            End If



            Return promotionlist
          

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    Protected Sub grdExrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdExrates.RowDataBound

        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = grdExrates.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "2"
        '    e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblratebasis As Label = e.Row.FindControl("lblratebasis")
            Dim lblexctypecode As Label = e.Row.FindControl("lblexctypecode")
            Dim txtsenior As TextBox = e.Row.FindControl("txtsenior")
            Dim txtunit As TextBox = e.Row.FindControl("txtunit")
            Dim txthalf As TextBox = e.Row.FindControl("txthalf")
            Dim txtfull As TextBox = e.Row.FindControl("txtfull")
            Dim txtchild As TextBox = e.Row.FindControl("txtchild")
            Dim txtadult As TextBox = e.Row.FindControl("txtadult")
            Dim lblsenioryn As Label = e.Row.FindControl("lblsenioryn")

            If lblratebasis.Text = "ACS" Then
                txtfull.Enabled = False
                txtunit.Enabled = False
                txthalf.Enabled = False
                txtadult.Enabled = True
                txtchild.Enabled = True
                txtsenior.Enabled = True
            ElseIf lblratebasis.Text = "HALF" Then
                txtfull.Enabled = False
                txtunit.Enabled = False
                txthalf.Enabled = True
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            ElseIf lblratebasis.Text = "FULL" Then
                txtfull.Enabled = True
                txtunit.Enabled = False
                txthalf.Enabled = False
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            ElseIf lblratebasis.Text = "UNIT" Then
                txtfull.Enabled = False
                txtunit.Enabled = True
                txthalf.Enabled = False
                txtadult.Enabled = False
                txtchild.Enabled = False
                txtsenior.Enabled = False
            End If

            If lblsenioryn.Text = "Yes" And lblratebasis.Text = "ACS" Then
                txtsenior.Enabled = True
            Else
                txtsenior.Enabled = False
            End If

        End If
    End Sub
End Class
