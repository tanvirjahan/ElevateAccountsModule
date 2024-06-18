#Region "NameSpace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic

#End Region

Partial Class PriceListModule_Othercostpricelist1
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

    Dim SqlCmd As SqlCommand
    Dim SqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim StrQry As String
    Dim SqlCon As SqlConnection

    Dim dpFDate As New TextBox
    Dim dpTDate As New TextBox

    Private dt As New DataTable
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String

#End Region

#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getgrouplist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim currname As New List(Of String)
        Try

            strSqlQry = "select othgrpcode,othgrpname  from  othgrpmast where active=1 and othgrpcode not in (select othgrpcode from view_system_othgrp) and othgrpname like  '" & Trim(prefixText) & "%' order by othgrpname "

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

                    currname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othgrpname").ToString(), myDS.Tables(0).Rows(i)("othgrpcode").ToString()))

                Next

            End If

            Return currname
        Catch ex As Exception
            Return currname
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
#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If IsPostBack = False Then
                'Session ("OthPLFilter")
                Dim strqry As String = ""
                Dim strOption As String = ""
                Dim strtitle As String = ""
                Dim strSpType As String = ""


                Session("GV_MAPLData") = Nothing

                grdDates.Visible = True
                fillDategrd(grdDates, True)


                'If (Session("OthPListFilter") <> Nothing And Session("OthPListFilter") <> "OTH") Then


                '    '

                '    Dim sptype As String = ""
                '    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", strSpType)


                '    strqry = "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters" & _
                '        " Where Param_Id='" & Session("OthPListFilter") & "') order by othgrpcode"

                'ElseIf Session("OthPListFilter") = "OTH" Then
                '    Dim sptypeQry As String = ""
                '    sptypeQry = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode not in (Select Option_Selected From Reservation_ParaMeters " & _
                '        "   Where Param_Id in (564,1031,1032,1033,1034,1035,1036,1037,1041,1042)) order by sptypecode "
                '    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPType, "sptypecode", "sptypename", sptypeQry, True)
                '    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", sptypeQry, True)


                '    strtitle = "Other Service "
                '    strqry = "select othgrpcode,othgrpname from othgrpmast,othmaingrpmast where othgrpmast.active=1 and othgrpmast.othgrpcode=othmaingrpmast.othmaingrpcode And othgrpmast.othmaingrpcode not in (Select Option_Selected From Reservation_ParaMeters" & _
                '        " Where Param_Id  in  (1001,1002,1105,1021,1027,1028)) order by othgrpcode"

                'End If

                If Request.QueryString("State") = "New" Then
                    Page.Title = Page.Title + " " + "New " + strtitle + " Price List"
                    txtCurrcodenew.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =457")
                    txtCurrNamenew.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", txtCurrcodenew.Text)

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
                ViewState.Add("pricelistState", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))

                txtconnection.Value = Session("dbconnectionName")

                btnclearprice.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear prices?')==false)return false;")
                'Dim supagentcode As String
                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 and sptypecode =(select option_selected  from " & _
                '" reservation_parameters where param_id=564)  order by sptypecode"

                'Dim strSPType As String = "select sptypecode,sptypename from sptypemast where active=1 "





                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupCode, "othgrpcode", "othgrpname", strqry, True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupName, "othgrpname", "othgrpcode", strqry, True)
                Dim default_group As String
                default_group = ""
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1109", String))


                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                If ViewState("OthpricelistState") = "New" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('New')")
                    Dim obj As New EncryptionDecryption



                    lblHeading.Text = "Add New " + strtitle + " Price List"

                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    chkPriceWithTax.Checked = True
                    txtVAT.Text = "5"
                ElseIf ViewState("OthpricelistState") = "Edit" Or ViewState("OthpricelistState") = "Copy" Then
                    btnGenerate.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'SetFocus(ddlMarketCode)
                    ' ShowRecord(ViewState("OthpricelistRefCode"))
                    'ShowMarket(CType(ViewState("OthpricelistRefCode"), String))
                    lblHeading.Text = "Edit " + strtitle + " Price List"
                ElseIf ViewState("OthpricelistState") = "View" Then
                ElseIf ViewState("OthpricelistState") = "Delete" Then
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



                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Else

                If Session("GV_MAPLData") Is Nothing = False Then
                    dt = Session("GV_MAPLData")


                    Dim fld2 As String = ""
                    Dim col As DataColumn
                    For Each col In dt.Columns
                        If col.ColumnName <> "Service_Type_Code" And col.ColumnName <> "Service_Type_Name" Then
                            Dim bfield As New TemplateField
                            'Call Function
                            bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                            bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)
                            grdtransferrates.Columns.Add(bfield)

                        End If

                    Next
                    grdtransferrates.Visible = True
                    grdtransferrates.DataSource = dt
                    'InstantiateIn Grid View
                    grdtransferrates.DataBind()
                End If

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
            ' ddlSPType.Disabled = True
            ' ddlSPTypeName.Disabled = True

            'If ViewState("OthpricelistState") = "Edit" Then
            '    ddlSupplierCode.Disabled = True
            '    ddlSupplierName.Disabled = True
            'End If


            'ddlGroupCode.Disabled = True
            'ddlGroupName.Disabled = True
        ElseIf ViewState("OthpricelistState") = "Delete" Or ViewState("OthpricelistState") = "View" Then
        End If


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
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtCurrcodenew.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode    from partymast where partycode ='" & txtsuppcode.Text & "'")
        txtCurrNamenew.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", txtCurrcodenew.Text)

        txtCurrNamenew.Enabled = False
       
    End Sub
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

                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        txtothgroupcode.Text = mySqlReader("othgrpcode")
                        txtothgroup.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", mySqlReader("othgrpcode"))
                    End If

                    If IsDBNull(mySqlReader("partycode")) = False Then
                        txtsuppcode.Text = mySqlReader("partycode")
                        txtsuppname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", mySqlReader("partycode"))
                    End If



                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrcodenew.Text = mySqlReader("currcode")
                        txtCurrNamenew.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If


                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Value = mySqlReader("remarks")
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



                chkConsdierForMarkUp.Visible = False

                'Added by abin on 20180604
                If IsDBNull(mySqlReader("VATPerc")) = False Then
                    txtVAT.Text = mySqlReader("VATPerc")
                End If
                If IsDBNull(mySqlReader("PriceWithTAX")) = False Then

                    If mySqlReader("PriceWithTAX") = "1" Then
                        chkPriceWithTax.Checked = True
                    Else
                        chkPriceWithTax.Checked = False
                    End If
                End If

            End If


        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

    '#Region "Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    '    Private Sub FillGridMarket(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
    '        Dim myDS As New DataSet
    '        gv_Market.Visible = True
    '        If gv_Market.PageIndex < 0 Then
    '            gv_Market.PageIndex = 0
    '        End If
    '        strSqlQry = ""
    '        Try
    '            strSqlQry = "select plgrpcode,plgrpname from plgrpmast where active=1 "
    '            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '            myDataAdapter.Fill(myDS)
    '            gv_Market.DataSource = myDS

    '            If myDS.Tables(0).Rows.Count > 0 Then
    '                gv_Market.DataBind()
    '                txtrowcnt.Value = gv_Market.Rows.Count
    '            Else
    '                gv_Market.DataBind()
    '            End If
    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        Finally
    '        End Try
    '    End Sub
    '#End Region

    '#Region "Private Sub ShowMarket(ByVal RefCode As String)"
    '    Private Sub ShowMarket(ByVal RefCode As String)
    '        Try
    '            Dim chkSel As CheckBox
    '            Dim lblcode As Label
    '            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '            mySqlCmd = New SqlCommand("Select * from othplisth_market  Where oplistcode='" & RefCode & "'", mySqlConn)
    '            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
    '            If mySqlReader.HasRows Then
    '                While mySqlReader.Read()
    '                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
    '                        For Each Me.gvRow1 In gv_Market.Rows
    '                            chkSel = gvRow1.FindControl("chkSelect")
    '                            lblcode = gvRow1.FindControl("lblcode")
    '                            If CType(mySqlReader("plgrpcode"), String) = CType(lblcode.Text, String) Then
    '                                chkSel.Checked = True
    '                                Exit For
    '                            End If
    '                        Next
    '                    End If
    '                End While
    '            End If

    '        Catch ex As Exception
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '            objUtils.WritErrorLog("OthPricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '        Finally
    '            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
    '            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
    '        End Try
    '    End Sub
    '#End Region

    '#Region "Public Sub FillMarket()"
    '    Public Sub FillMarket()
    '        Me.pnlMarket.Visible = True
    '        Me.Panel1.Visible = True
    '        FillGridMarket("plgrpcode")
    '        gv_Market.Visible = True
    '    End Sub
    '#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Dim strValue As String = ""
        Try


            If txtsuppname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Supplier.');", True)
                ValidatePage = False
                Exit Function
            End If

            If txtCurrcodenew.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Currency.');", True)
                ValidatePage = False
                Exit Function
            End If

            ''''''''''' Dates Overlapping

            Dim dtdatesnew As New DataTable
            Dim dsdates As New DataSet
            Dim dr As DataRow
            Dim xmldates As String = ""




            dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
            dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


            For Each gvRow1 In grdDates.Rows
                Dim txtfromdate As TextBox = gvRow1.FindControl("txtfromdate")
                Dim txttodate As TextBox = gvRow1.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    dr = dtdatesnew.NewRow

                    dr("fromdate") = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    dtdatesnew.Rows.Add(dr)

                End If

            Next
            dsdates.Clear()
            If dtdatesnew IsNot Nothing Then
                If dtdatesnew.Rows.Count > 0 Then
                    dsdates.Tables.Add(dtdatesnew)
                    xmldates = objUtils.GenerateXML(dsdates)
                End If
            Else
                xmldates = "<NewDataSet />"
            End If

            Dim strMsg As String = ""
            Dim ds As DataSet
            Dim parms As New List(Of SqlParameter)
            Dim parm(1) As SqlParameter

            parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
            'parm(1) = New SqlParameter("@contractfromdate", DBNull.Value)
            'parm(2) = New SqlParameter("@contracttodate", DBNull.Value)

            parms.Add(parm(0))

            'For i = 0 To 2
            '    parms.Add(parm(i))
            'Next

            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                        strMsg = "Dates Are Overlapping Please check " + "\n"
                        For i = 0 To ds.Tables(0).Rows.Count - 1

                            strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                        Next

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            End If


            '''''''''''''''''

            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim obj As New EncryptionDecryption
        Try
            Dim myDS As New DataSet

            If ValidatePage() = False Then
                Exit Sub
            End If

            strSqlQry = "select othtypcode,othtypname,0 Unit from othtypmast where active=1 and othgrpcode='" & txtothgroupcode.Text & "' order by othtypname "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdtransferrates.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then

                btnSave.Style.Add("display", "block")
                btnGenerate.Style.Add("display", "none")
                chkapprove.Style.Add("display", "block")
                btncopyrates.Style.Add("display", "block")
                btnclearprice.Style.Add("display", "block")

                grdtransferrates.PageIndex = 0
                grdtransferrates.DataBind()
            Else

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There is no records ');", True)

                btnSave.Style.Add("display", "none")
                btnGenerate.Style.Add("display", "block")
                chkapprove.Style.Add("display", "none")
                btncopyrates.Style.Add("display", "none")
                btnclearprice.Style.Add("display", "none")

                grdtransferrates.PageIndex = 0
                grdtransferrates.DataBind()

            End If

            'createdatatable()
            'createdatarows()

           

        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Private Sub createdatarows()"
    Private Sub createdatarows()
        Dim i As Long
        Dim k As Long = 0
        Try

            strSqlQry = "SELECT  count(othtypmast.othtypcode) FROM  othtypmast  WHERE othgrpcode='" & hdngroup.Value & "'  and othtypmast.active=1"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

            Dim arr_rnkorder(cnt + 1) As String
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "SELECT distinct othtypmast.othtypcode, othtypmast.othtypname,othtypmast.othgrpcode,isnull(rankorder,0) rankorder  FROM othtypmast  WHERE othgrpcode='" & hdngroup.Value & "'  and othtypmast.active=1 order by  rankorder"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rows(k) = mySqlReader("othtypcode")
                arr_rname(k) = mySqlReader("othtypname")
                '     arr_rname(k) = mySqlReader("othtypname")
                k = k + 1
            End While
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)


            Session("GV_MAPLData") = dt
            Dim dr As DataRow


            dr = Nothing

            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow


                dr("Service_Type_Code") = arr_rows(row)
                dr("Service_Type_Name") = arr_rname(row)



                'Next
                dt.Rows.Add(dr)
            Next


            grdtransferrates.Visible = True
            grdtransferrates.DataSource = dt
            'InstantiateIn Grid View
            grdtransferrates.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try


    End Sub
#End Region
#Region "Private Sub createdatatable()"
    Private Sub createdatatable()
        Try


            Session("GV_MAPLData") = Nothing


            cnt = 0
            'strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.othcatmast  WHERE othgrpcode in (select option_selected from reservation_parameters where param_id=1028) and othcatmast.active=1 "

            strSqlQry = "SELECT  count(dbo.othcatmast.othcatcode) FROM dbo.othcatmast  WHERE othgrpcode ='" & hdngroup.Value & "'   and othcatmast.active=1 "


            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt <= 0 Then
                Session("CheckGridColumn") = "Not Present"
            Else
                Session("CheckGridColumn") = ""
            End If


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0
            Dim Column As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            'strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM  dbo.othcatmast  WHERE othgrpcode in (select option_selected from reservation_parameters where param_id=1028)    and othcatmast.active=1 Order by othcatmast.grporder"

            strSqlQry = "SELECT  distinct dbo.othcatmast.othcatcode, dbo.othcatmast.othgrpcode,othcatmast.grporder FROM  dbo.othcatmast  WHERE othgrpcode ='" & hdngroup.Value & "'   and othcatmast.active=1 Order by othcatmast.grporder"

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(Column) = mySqlReader("othcatcode")
                Column = Column + 1
            End While
            mySqlReader.Close()
            mySqlConn.Close()


            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt = New DataTable

            dt.Columns.Add(New DataColumn("Service_Type_Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Service_Type_Name", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To Column - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next



            Session("GV_MAPLData") = dt

            For i = grdtransferrates.Columns.Count - 1 To 3 Step -1
                grdtransferrates.Columns.RemoveAt(i)
            Next


            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Service_Type_Code" And col.ColumnName <> "Service_Type_Name" Then


                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClsOtherServicePriceList(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClsOtherServicePriceList(ListItemType.Item, col.ColumnName, fld2)

                    grdtransferrates.Columns.Add(bfield)


                End If
            Next

            grdtransferrates.Visible = True
            grdtransferrates.DataSource = dt
            grdtransferrates.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthPricesList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region

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
        Dim RefCode As String
        Try
            If IsPostBack() = False Then

                ViewState.Add("pricelistState", Request.QueryString("State"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))

                btnSave.Style.Add("display", "none")
                btnGenerate.Style.Add("display", "block")

                '   wucCountrygroup.sbSetPageState("", "MAPLIST", CType(ViewState("pricelistState"), String))


                RefCode = CType(ViewState("RefCode"), String)
                hdngroup.Value = Request.QueryString("Group")
                Session("OthGroup") = hdngroup.Value

                hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")

                If CType(ViewState("pricelistState"), String) = "New" Then


                    'Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    'wucCountrygroup.sbShowCountry()

                    txtsuppname.Focus()

                    chkapprove.Style.Add("display", "none")
                    btncopyrates.Style.Add("display", "none")
                    btnclearprice.Style.Add("display", "none")
                ElseIf CType(ViewState("pricelistState"), String) = "Copy" Then
                    ShowRecord(RefCode)
                    'wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(ViewState("pricelistState"), String))
                    'wucCountrygroup.sbShowCountry()

                    ShowDates(CType(RefCode, String))
                    Showdetailsgrid(CType(RefCode, String))

                    txtPlcCode.Value = ""
                    btnGenerate.Style.Add("display", "none")
                    btnSave.Style.Add("display", "block")
                    chkapprove.Style.Add("display", "block")
                    btncopyrates.Style.Add("display", "block")
                    btnclearprice.Style.Add("display", "block")
                    TaxCalculation()
                ElseIf CType(ViewState("pricelistState"), String) = "Edit" Then

                    btnSave.Text = "Update"

                    RefCode = CType(ViewState("RefCode"), String)
                    ShowRecord(RefCode)
                    ShowDates(CType(RefCode, String))
                    Showdetailsgrid(CType(RefCode, String))

                    btnGenerate.Style.Add("display", "none")
                    btnSave.Style.Add("display", "block")
                    btnSave.Text = "Update"
                    chkapprove.Style.Add("display", "block")
                    btncopyrates.Style.Add("display", "block")
                    btnclearprice.Style.Add("display", "block")
                    TaxCalculation()
                ElseIf CType(ViewState("pricelistState"), String) = "View" Then

                    RefCode = CType(ViewState("RefCode"), String)

                    ShowRecord(RefCode)
                    ShowDates(CType(RefCode, String))
                    Showdetailsgrid(CType(RefCode, String))

                    btnGenerate.Style.Add("display", "none")
                    btnSave.Style.Add("display", "none")
                    chkapprove.Style.Add("display", "block")
                    btncopyrates.Style.Add("display", "block")
                    btnclearprice.Style.Add("display", "block")
                    TaxCalculation()
                ElseIf CType(ViewState("pricelistState"), String) = "Delete" Then

                    RefCode = CType(ViewState("RefCode"), String)

                    ShowRecord(RefCode)
                    ShowDates(CType(RefCode, String))
                    Showdetailsgrid(CType(RefCode, String))

                    btnGenerate.Style.Add("display", "none")
                    btnSave.Style.Add("display", "block")
                    chkapprove.Style.Add("display", "block")
                    btncopyrates.Style.Add("display", "block")
                    btnclearprice.Style.Add("display", "block")
                    btnSave.Text = "Delete"
                    TaxCalculation()
                End If

                DisableAllControls()

            Else


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdtransferrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0

            strQry = "select count(*) from othtypmast where active=1 and othgrpcode='" & txtothgroupcode.Text & "'"




            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            '  fillDategrd(grdtransferrates, False, cnt)

            If cnt > 0 Then



                strSqlQry = "select  * from (select  h.othtypcode,h.othtypname,0 Unit  from othtypmast h (nolock) where active=1 and othgrpcode='" & txtothgroupcode.Text & "' and othtypcode " _
                        & " not in (select othtypcode from   oplist_costd where  ocplistcode='" & RefCode & "') union all " _
                      & "  select  d.othtypcode,h.othtypname,  d.ocostprice  Unit from oplist_costd d, othtypmast h   where d.othtypcode=h.othtypcode and d.othgrpcode=h.othgrpcode and h.othgrpcode='" & txtothgroupcode.Text & "'    and " _
                      & "   d.ocplistcode='" & RefCode & "' group by d.othtypcode,h.othtypname,d.ocostprice) rs  order by  rs.othtypname "





                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                grdtransferrates.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then

                    ' grdtransferrates.PageIndex = 0
                    grdtransferrates.DataBind()
                Else

                    grdtransferrates.PageIndex = 0
                    grdtransferrates.DataBind()

                End If



            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Otherpricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub ShowDynamicGridnew(ByVal RefCode As String)
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long = 0
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        grdtransferrates.Visible = True
        cnt = grdtransferrates.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 4
            txt = grdtransferrates.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next
        Dim m As Long = 0
        Dim rmcatcode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim rmtypecode As String = ""
        Dim mealcode As String = ""
        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String
        Dim txttrftypecode As TextBox
        Dim sectorcode As String
        Try
            '    Dim extrapx As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1139")


            StrQry = "select distinct oplist_costd.oclineno,othtypcode from oplist_costd where oplist_costd.ocplistcode = '" & RefCode & "'"



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdtransferrates.Rows

                        txttrftypecode = gvrow.FindControl("txttrftypecode")

                        If IsDBNull(mySqlReader("othtypcode")) = False Then
                            sectorcode = mySqlReader("othtypcode")
                        End If
                        If txttrftypecode Is Nothing = False Then

                            StrQryTemp = "select ocplistcode  ,othtypcode,othcatcode,ocostprice, " & _
              " pkgnights from  oplist_costd  " & _
              " where   " & _
              " ocplistcode= '" & RefCode & "' and othtypcode = '" & txttrftypecode.Text & "'"



                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("othcatcode")) = False Then
                                        rmcatcode = myReader("othcatcode")
                                    End If
                                    If IsDBNull(myReader("ocostprice")) = False Then
                                        value = (myReader("ocostprice"))
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 4
                                        'If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                                        'Else
                                        For s = 0 To grdtransferrates.Columns.Count - 4
                                            headerlabel = grdtransferrates.HeaderRow.FindControl("txtHead" & s)
                                            Dim arrlist As String()
                                            If headerlabel.Text <> Nothing Then
                                                arrlist = headerlabel.Text.Split("-")
                                                If ((headerlabel.Text = rmcatcode)) Then
                                                    If gvrow.RowIndex = 0 Then
                                                        txt = gvrow.FindControl("txt" & s)
                                                    Else
                                                        txt = gvrow.FindControl("txt" & ((grdtransferrates.Columns.Count - 4) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                    End If
                                                    If txt Is Nothing Then
                                                    Else
                                                        If value = "" Then
                                                            txt.Text = ""
                                                        Else
                                                            Select Case value
                                                                Case "-4"
                                                                    value = "N/A"
                                                                Case "-5"
                                                                    value = "On Request"
                                                            End Select
                                                            If txt.Enabled = True Then
                                                                txt.Text = DecRound(value)
                                                            End If

                                                        End If
                                                    End If
                                                    GoTo go1
                                                End If
                                            End If
                                        Next
                                        'End If
                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                            ' End If '''
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)

        Catch ex As Exception
            objUtils.WritErrorLog("Othpricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
    Public Sub SetDate()
        Try
            Dim gvRow As GridViewRow
            Dim flgdt As Boolean = False
            Dim tempFrmDt As String = ""
            Dim tempToDt As String = ""
            ViewState("TempFrmDt") = ""
            ViewState("TempToDt") = ""
            For Each gvRow In grdDates.Rows
                dpFDate = gvRow.FindControl("txtfromDate")
                dpTDate = gvRow.FindControl("txttoDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If flgdt = False Then
                        'dpFromdate.txtDate.Text = dpFDate.txtDate.Text
                        tempFrmDt = dpFDate.Text
                        tempToDt = dpTDate.Text
                    Else
                        If CType(dpFDate.Text, Date) < CType(tempFrmDt, Date) Then
                            tempFrmDt = CType(dpFDate.Text, Date)
                        End If

                        If CType(dpTDate.Text, Date) > CType(tempToDt, Date) Then
                            tempToDt = CType(dpTDate.Text, Date)
                        End If
                    End If
                    flgdt = True
                End If
            Next
            ViewState("TempFrmDt") = tempFrmDt
            ViewState("TempToDt") = tempToDt


        Catch ex As Exception
            objUtils.WritErrorLog("OthPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppname As New List(Of String)
        Dim supgroup As String = ""

        Try

            supgroup = HttpContext.Current.Session("OthGroup")



            strSqlQry = "select partycode,partyname  from  partymast where active=1 and sptypecode='" & supgroup.ToString & "' and  partyname like  '" & Trim(prefixText) & "%' order by partyname "

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

    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try

            '   CopyRow = 0
            Dim chksel As CheckBox
            Dim weekdaystr As String = ""


            Dim othgrpcode As String = ""
            othgrpcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1028")


            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            'Session("CountryList") = wucCountrygroup.checkcountrylist
            'Session("AgentList") = wucCountrygroup.checkagentlist



            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(5) As SqlParameter

                    parm3(0) = New SqlParameter("@groupcode", CType(txtothgroupcode.Text, String))
                    parm3(1) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(2) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@plistcode", CType(txtPlcCode.Value, String))
                    parm3(4) = New SqlParameter("@partycode", CType(txtsuppcode.Text, String))
                    'parm3(5) = New SqlParameter("@agent", CType(Session("AgentList"), String))



                    For i = 0 To 4
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkduplicate_MAcostplist", parms3)


                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("ocplistcode")) = False Then
                             
                                strMsg = "OtherService Cost Pricelist already exists For this Supplier   " + ds1.Tables(0).Rows(0)("ocplistcode")


                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If

                End If
            Next



        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'If Not Session("PlistSaved") = True Then ''this variable because response.redirect is causing a postback and saving twice
            Dim vatPerc As Decimal
            If IsNumeric(txtVAT.Text) Then
                vatPerc = Convert.ToDecimal(txtVAT.Text)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vat Percentage can not be empty or zero.');", True)
                Exit Sub
            End If
            Dim GvRow As GridViewRow
            If Page.IsValid = True Then
                If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Edit" Or ViewState("pricelistState") = "Copy" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    Dim othgrpcode As String = ""
                    othgrpcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1028")

                    If FindDatePeriod() = False Then

                        Exit Sub
                    End If




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Copy" Then
                        Dim optionval As String
                        Dim optionName As String

                        'If hdngroup.Value = "VISA" Then
                        optionName = "OTHCPLIST"



                        optionval = objUtils.GetAutoDocNo(optionName, mySqlConn, sqlTrans)
                        txtPlcCode.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_OthcostPListh", mySqlConn, sqlTrans)
                    ElseIf ViewState("pricelistState") = "Edit" Then

                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_othercostpriceslist_logs", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd = New SqlCommand("sp_mod_OthcostPListh", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtothgroupcode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtsuppcode.Text, String)

                    If CType(txtCurrcodenew.Text.Trim, String) = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrcodenew.Text.Trim, String)
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1

                    If ViewState("pricelistState") = "New" Or ViewState("pricelistState") = "Copy" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If
                    If chkapprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@approve", SqlDbType.Int)).Value = 0
                    End If

                    'added by abn on 20180604 *************
                    mySqlCmd.Parameters.Add(New SqlParameter("@VatPercentage", SqlDbType.Decimal, 18, 3)).Value = CType(txtVAT.Text.Trim, String)
                    If chkPriceWithTax.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@PriceWithTAX", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@PriceWithTAX", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd.Dispose()
                    '-----------------------------------------------------------




                    mySqlCmd = New SqlCommand("DELETE FROM oplist_costd  Where ocplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM othcostplisth_dates  Where ocplistcode='" & CType(txtPlcCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    '----------------------------------- Inserting Data To Dates Table
                    For Each GvRow In grdDates.Rows
                        dpFDate = GvRow.FindControl("txtfromDate")
                        dpTDate = GvRow.FindControl("txtToDate")

                        If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_othcostplisth_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text)

                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next

                    Dim rx As Integer = 1
                    For Each GvRow In grdtransferrates.Rows
                        Dim lblothtypecode As Label = GvRow.FindControl("lblothtypecode")
                        Dim txtunit As TextBox = GvRow.FindControl("txtunit")

                        'Added by abin on 20180604 ************
                        Dim txtUnitTV As TextBox = CType(GvRow.FindControl("txtUnitTV"), TextBox)
                        Dim txtUnitNTV As TextBox = CType(GvRow.FindControl("txtUnitNTV"), TextBox)
                        Dim txtUnitVAT As TextBox = CType(GvRow.FindControl("txtUnitVAT"), TextBox)

                        If txtunit.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_Othplist_costd", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtothgroupcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@oclineno", SqlDbType.Int)).Value = rx
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(lblothtypecode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType("UNIT", String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@tcostprice", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtunit.Text.Trim), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@pknights", SqlDbType.Int)).Value = 1

                            'Added by abin on 20180604 ************
                            mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitTV.Text.Trim), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitNTV.Text.Trim), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@VatValue", SqlDbType.Decimal, 18, 3)).Value = CType(Val(txtUnitVAT.Text.Trim), Decimal)


                            mySqlCmd.ExecuteNonQuery()
                            rx = rx + 1
                        End If
                    Next


                  


                    '---------------------------------------------End of save/edit/copy------------------------
                ElseIf ViewState("pricelistState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    'Inserting Into Logs
                    mySqlCmd = New SqlCommand("sp_othercostpriceslist_logs", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    'delete of main tbl to be written
                    mySqlCmd = New SqlCommand("sp_del_Othcostplisth", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@oplistcode", SqlDbType.VarChar, 20)).Value = CType(txtPlcCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                End If

            End If
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('OtherserviceCostPriceListWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthercostPriceList1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub ShowDates(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            strSqlQry = "select count( distinct frmdate) from othcostplisth_dates(nolock) where ocplistcode='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            mySqlCmd = New SqlCommand("Select * from othcostplisth_dates(nolock) Where ocplistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("frmdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("frmdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Othpricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=othercostpricelist','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub btncopyrates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrates.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdtransferrates.Visible = True
        cnt = grdtransferrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 4
                txt = grdtransferrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try
            For Each GvRow In grdtransferrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If chk.Checked = True Then
                    cnt_checked = cnt_checked + 1
                End If
            Next
            If cnt_checked = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row');", True)
                SetFocus(cnt_checked)
                Exit Sub
            End If


            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0

            For Each GvRow In grdtransferrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            'If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                            'Else

                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    arr_room(room) = txt.Text
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                        End If
                        'End If
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    pkg = 0
                    For j = n To (m + n) - 1
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            'If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                            'Else

                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    arr_room(room) = txt.Text
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                            ' End If
                        End If
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next
            '--------------------------------------------------------------------------------------------
            'Noe Fill Record to Cell
            room = 0
            pkg = 0
            n = 0
            b = 0

            For Each GvRow In grdtransferrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 4
                        If GvRow.RowIndex = row_id + 1 Then
                            'If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                            'Else


                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Enabled = True Then
                                    txt.Text = arr_room(room)
                                End If
                            End If


                            room = room + 1
                            ' End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If GvRow.RowIndex = row_id + 1 Then
                            'If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                            'Else

                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Enabled = True Then
                                    txt.Text = arr_room(room)
                                End If
                            End If


                            room = room + 1
                            ' End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("OthPricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnclearprice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearprice.Click
        Try
            Dim GvRow As GridViewRow
            For Each GvRow In grdtransferrates.Rows
                Dim lblothtypecode As Label = GvRow.FindControl("lblothtypecode")
                Dim txtunit As TextBox = GvRow.FindControl("txtunit")
                Dim txtUnitTV As TextBox = CType(GvRow.FindControl("txtUnitTV"), TextBox) 'Added by abin on 20180406
                Dim txtUnitNTV As TextBox = CType(GvRow.FindControl("txtUnitNTV"), TextBox)
                Dim txtUnitVAT As TextBox = CType(GvRow.FindControl("txtUnitVAT"), TextBox)

                txtunit.Text = ""
                txtUnitTV.Text = ""
                txtUnitNTV.Text = ""
                txtUnitVAT.Text = ""
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("Othercostpricelist1.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

       
    End Sub
#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Private Function GridViewRowToDataRow(ByVal gvr As GridViewRow) As DataRow
        Dim di As Object = Nothing
        Dim drv As DataRowView = Nothing
        Dim dr As DataRow = Nothing

        If gvr IsNot Nothing Then
            di = TryCast(gvr.DataItem, System.Object)
            If di IsNot Nothing Then
                drv = TryCast(di, System.Data.DataRowView)
                If drv IsNot Nothing Then
                    dr = TryCast(drv.Row, System.Data.DataRow)
                End If
            End If
        End If

        Return dr
    End Function
    Protected Sub grdtransferrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdtransferrates.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim lblothtypecode As Label = e.Row.FindControl("lblothtypecode")
            Dim txtunit As TextBox = e.Row.FindControl("txtunit")
            Dim dtrow As DataRow = GridViewRowToDataRow(CType(e.Row, GridViewRow))
            txtunit.Text = IIf(dtrow("unit") = 0, "", DecRound(dtrow("unit")))


            Numberssrvctrl(txtunit)
            'Added by abin on 20180604 *********************
            Dim strUnit = "'" + txtunit.ClientID + "'"
            Dim txtUnitTV As TextBox = CType(e.Row.FindControl("txtUnitTV"), TextBox)
            strUnit = strUnit + ",'" + txtUnitTV.ClientID + "'"
            Dim txtUnitVAT As TextBox = CType(e.Row.FindControl("txtUnitVAT"), TextBox)
            strUnit = strUnit + ",'" + txtUnitVAT.ClientID + "'"
            Dim txtUnitNTV As TextBox = CType(e.Row.FindControl("txtUnitNTV"), TextBox)
            strUnit = strUnit + ",'" + txtUnitNTV.ClientID + "'"
            txtunit.Attributes.Add("onblur", "CalculateTax(" & strUnit & ")")




        End If

    End Sub

    'Added by abin on 20180604 *********************
    Protected Sub txtVAT_TextChanged(sender As Object, e As System.EventArgs) Handles txtVAT.TextChanged
        TaxCalculation()

    End Sub

    Protected Sub chkPriceWithTax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPriceWithTax.CheckedChanged
        TaxCalculation()
    End Sub

    Private Sub TaxCalculation()
        Try

            Dim vatPerc As Decimal
            If IsNumeric(txtVAT.Text) Then
                vatPerc = Convert.ToDecimal(txtVAT.Text)
            Else
                '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Vat Percentage can not be empty.');", True)
                '   Exit Sub
                txtVAT.Text = "0"
            End If
            For Each row As GridViewRow In grdtransferrates.Rows


                '*** >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Added by abin on 28/05/2018


                Dim txtunit As TextBox = row.FindControl("txtunit")


                Dim strUnit = txtunit.Text
                Dim txtUnitTV As TextBox = CType(row.FindControl("txtUnitTV"), TextBox)
                Dim txtUnitNTV As TextBox = CType(row.FindControl("txtUnitNTV"), TextBox)
                Dim txtUnitVAT As TextBox = CType(row.FindControl("txtUnitVAT"), TextBox)


                If txtVAT.Text.Trim = "" Then
                    txtUnitVAT.Text = ""
                ElseIf chkPriceWithTax.Checked = False Then
                    If txtunit.Text <> "" Then
                        txtUnitTV.Text = Math.Round(Convert.ToDecimal(txtunit.Text), 2)
                        txtUnitNTV.Text = "0"
                        txtUnitVAT.Text = Math.Round(Convert.ToDecimal(txtunit.Text) * (Convert.ToDecimal(txtVAT.Text) / 100), 2)
                    End If
                Else
                    If txtunit.Text <> "" Then
                        txtUnitTV.Text = Math.Round(Convert.ToDecimal(txtunit.Text) / (1 + (Convert.ToDecimal(txtVAT.Text) / 100)), 2)
                        txtUnitNTV.Text = "0"
                        txtUnitVAT.Text = Math.Round(Convert.ToDecimal(txtunit.Text) - Convert.ToDecimal(txtUnitTV.Text), 2)
                    End If

                End If

                '*** <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Next

        Catch ex As Exception

        End Try
    End Sub
End Class
