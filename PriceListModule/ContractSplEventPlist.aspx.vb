Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing

Imports ColServices

Partial Class ContractSplEventPlist
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime


    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim blankrow As Integer = 0
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim txtrmtypcodenew As New ArrayList
    Dim txtrmtypnamenew As New ArrayList
    Dim txtmealcodenew As New ArrayList
    Dim txtrmcatcodenew As New ArrayList
    Dim txtdetailremarksnew As New ArrayList
    Dim txtminnightsnew As New ArrayList
    Dim txtSplEventsnamenew As New ArrayList
    Dim txtSplEventscodenew As New ArrayList
    Dim txtadultratenew As New ArrayList

    Dim rmcatcodenew As New ArrayList
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList
    Dim Exhnamenew As New ArrayList
    Dim Roomtypenew As New ArrayList
    Dim Mealplannew As New ArrayList
    Dim Suppamountnew As New ArrayList
    Dim adultratenew As New ArrayList
    Dim withdrawnnew As New ArrayList

    Dim Exhicodenew As New ArrayList
    Dim CopyRowlist As New ArrayList
    Dim childCopyRow As New ArrayList


#End Region
#Region "Enum GridCol"
    Enum GridCol
        splistcode = 1
        Fromdate = 2
        Todate = 3
        applicableto = 4

        Edit = 5
        View = 6
        Delete = 7
        Copy = 8
        DateCreated = 11
        UserCreated = 12
        DateModified = 13
        UserModified = 14


    End Enum
#End Region
    '*** Danny 20/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    Protected Sub CheckBox1MarkupRequired_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1MarkupRequired.CheckedChanged
        txtMarkupAdultOp.Text = ""
        txtMarkupAdultVal.Text = "0"
        txtMarkupChildOp.Text = ""
        txtMarkupChildVal.Text = "0"
        If CheckBox1MarkupRequired.Checked = True Then
            txtMarkupAdultOp.Enabled = True
            txtMarkupAdultVal.Enabled = True
            txtMarkupChildOp.Enabled = True
            txtMarkupChildVal.Enabled = True
        Else
            txtMarkupAdultOp.Enabled = False
            txtMarkupAdultVal.Enabled = False
            txtMarkupChildOp.Enabled = False
            txtMarkupChildVal.Enabled = False
        End If
        'Call fnCalculateVATValue()
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetOperators(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lstOperrator As New List(Of String)
        Try
            If prefixText = "" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            ElseIf prefixText = "*" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
            ElseIf prefixText = "/" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
            ElseIf prefixText = "+" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
            ElseIf prefixText = "-" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
            ElseIf prefixText = "%" Then
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            Else
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("*", "*"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("/", "/"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("+", "+"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("-", "-"))
                lstOperrator.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("%", "%"))
            End If

            Return lstOperrator
        Catch ex As Exception
            Return lstOperrator
        End Try

    End Function
    '*** Danny 20/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


    Protected Sub btnrmcat_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        'btnmealok.Style("display") = "none"
        'btnOk1.Style("display") = "block"

        Dim strrmcatname As String = CType(gv_Filldata.Rows(rowid).FindControl("txtrmcatcode"), TextBox).Text


        Dim strRoomcat As String = CType(strrmcatname, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        If Session("Calledfrom") = "Offers" Then

            strSqlQry = "select distinct p.rmcatcode as rmtypcode,m.rmcatname as rmtypname from  partyrmcat(nolock) p, rmcatmast m,view_offers_accomodation v(nolock) where p.rmcatcode=m.rmcatcode  " _
  & "and p.rmcatcode=v.rmcatcode and v.rmcatcode=m.rmcatcode and v.promotionid='" & hdnpromotionid.Value & "' and m.active=1 and m.allotreqd ='Yes' and m.accom_extra ='A' and  partycode='" & hdnpartycode.Value & "' order by p.rmcatcode"

         
        Else


            strSqlQry = "select distinct p.rmcatcode as rmtypcode,m.rmcatname as rmtypname from  partyrmcat(nolock) p, rmcatmast m where p.rmcatcode=m.rmcatcode  " _
     & "and m.active=1 and m.allotreqd ='Yes' and m.accom_extra ='A' and  partycode='" & hdnpartycode.Value & "' order by p.rmcatcode"
        End If

        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If

        gv_Showroomtypes.HeaderRow.Cells(3).Text = "RoomCategory Name"

        ChkExistingRoomtypes(strRoomcat)


        ModalExtraPopup.Show()


    End Sub
    Protected Sub btnrmtyp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        'btnmealok.Style("display") = "none"
        'btnOk1.Style("display") = "block"

        Dim strrmtypename As String = CType(gv_Filldata.Rows(rowid).FindControl("txtrmtypcode"), TextBox).Text


        Dim strRoomType As String = CType(strrmtypename, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        If Session("Calledfrom") = "Offers" Then
            strSqlQry = "select v.rmtypcode,p.rmtypname from  partyrmtyp p(nolock),view_offers_rmtype v where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.inactive=0 and p.partycode='" & hdnpartycode.Value & "' order by isnull(p.rankord,999)"
        Else
            strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"
        End If

        'strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"

        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If

        gv_Showroomtypes.HeaderRow.Cells(3).Text = "RoomType Name"

        ChkExistingRoomtypes(strrmtypename)


        ModalExtraPopup.Show()


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


        Catch ex As Exception
            Return promotionlist
        End Try

    End Function
    Private Sub ChkExistingRoomtypes(ByVal prm_strChkRoomtypes As String)
        Dim chkSelect As CheckBox
        Dim txtrmtypcode As Label


        Dim arrString As String()


        If prm_strChkRoomtypes <> "" Then
            arrString = prm_strChkRoomtypes.Split(",") 'spliting for ';' 1st

            For k = 0 To arrString.Length - 1
                'If arrString(k) <> "" Then

                '  Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                For Each grdRow In gv_Showroomtypes.Rows
                    chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                    txtrmtypcode = CType(grdRow.FindControl("txtrmtypcode"), Label)
                    If arrString(k) = txtrmtypcode.Text Then
                        chkSelect.Checked = True

                    End If

                Next
                'End If
            Next
            'Else
            '    'first case when not selected , chk all
            '    For Each grdRow In gvOccupancy.Rows
            '        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
            '        chkSelect.Checked = True

            '    Next


        End If
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnmeal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(gv_Filldata.Rows(rowid).FindControl("txtmealcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        If Session("Calledfrom") = "Offers" Then
            strSqlQry = "select v.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock),view_offers_meal v(nolock)  " _
                & " where v.mealcode=p.mealcode and v.mealcode=m.mealcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
        Else
            strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
        End If


        ' strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Meal Plan"

        ' ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)


        ModalExtraPopup.Show()


    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub fillheader()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,c.fromdate,c.todate from view_contracts_search c(nolock) Where c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)

                    End If

                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        hdnconfromdate.Value = Format(mySqlReader("fromdate"), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        hdncontodate.Value = Format(mySqlReader("todate"), "dd/MM/yyyy")
                    End If

                End If




                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub


    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ViewState("State") = "New"


        If Session("Calledfrom") = "Offers" Then

            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

            txtpromotionid.Text = hdnpromotionid.Value
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,pm.mktcode  from view_offers_header h (nolock) cross apply dbo.splitallotmkt(h.promotiontypes,',')  pm  where   pm.mktcode ='Special Rates' and promotionid='" & hdnpromotionid.Value & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                hdncommtype.Value = ds.Tables(0).Rows(0).Item("mktcode")
            End If

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
            End If




            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            DisableControl()

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
           
            fillheader()

            lblstatus.Visible = False
            lblstatustext.Visible = False

            btnSave.Text = "Save"
            lblHeading.Text = "New Special Events - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "New Special Events -" + ViewState("hotelname") + "-" + hdnpromotionid.Value

        Else

            PanelMain.Visible = True
            PanelMain.Style("display") = "block"
            Panelsearch.Enabled = False
            Session("contractid") = hdncontractid.Value
            wucCountrygroup.Visible = True
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            fillheader()
            DisableControl()
            lblstatus.Visible = False
            lblstatustext.Visible = False

            btnSave.Text = "Save"
            lblHeading.Text = "New Special Events - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "New Special Events -" + ViewState("hotelname")



            ' divuser.Style("display") = "none"
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
        End If

      






        wucCountrygroup.Visible = True

        'divcopy1.Style("display") = "none"



        btncopyratesnextrow.Visible = True

      
        fillDategrd(gv_Filldata, True)

        'createdatacolumns()
        ' FillRoomdetails()

        btnSave.Visible = True



     

        chkVATCalculationRequired.Checked = True
        Call sbFillTaxDetail() 'changed by mohamed on 21/02/2018

    End Sub


#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        dt.Columns.Add(New DataColumn("Agefrom", GetType(String)))
        dt.Columns.Add(New DataColumn("AgeTo", GetType(String)))
        dt.Columns.Add(New DataColumn("childrate", GetType(String)))

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
            lngcnt = 3
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True

    End Sub
#End Region
    Public Sub fillsubDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
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




    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim gvchildage As GridView

            gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

            count = gvchildage.Rows.Count + 1

            Dim childfrom(count) As String
            Dim childto(count) As String
            Dim childrate(count) As String
            Dim txtchildfrom As TextBox
            Dim txtchildto As TextBox
            Dim txtchildrate As TextBox
            '  Dim gvchildage As GridView

            Dim n As Integer = 0
            ' For Child Age Grid


            For Each GVchildRow In gvchildage.Rows
                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                txtchildto = GVchildRow.FindControl("txtchildageto")
                txtchildrate = GVchildRow.FindControl("txtchildrate")

                childfrom(n) = CType(txtchildfrom.Text, String)
                childto(n) = CType(txtchildto.Text, String)
                childrate(n) = CType(txtchildrate.Text, String)
                n = n + 1
            Next

            fillsubDategrd(gvchildage, False, gvchildage.Rows.Count + 1)

            Dim i As Integer = n
            n = 0
            For Each GVchildRow In gvchildage.Rows
                If n = i Then
                    Exit For
                End If
                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                txtchildto = GVchildRow.FindControl("txtchildageto")
                txtchildrate = GVchildRow.FindControl("txtchildrate")

                txtchildfrom.Text = CType(childfrom(n), String)
                txtchildto.Text = childto(n)
                txtchildrate.Text = childrate(n)

                n = n + 1
            Next


            ' gvchildage = TryCast(gv_Filldata.Rows(gv_Filldata.Rows.Count).FindControl("gvchildage"), GridView)
            txtchildfrom = TryCast(gvchildage.Rows(gvchildage.Rows.Count - 1).FindControl("txtchildagefrm"), TextBox)
            txtchildfrom.Focus()

            Call fnCalculateVATValue() 'changed by mohamed on 21/02/2018
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)


        Dim gvchildage As GridView

        Dim imgSclose As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gvchildage.Rows.Count + 1


        Dim childfrom(count) As String
        Dim childto(count) As String
        Dim childrate(count) As String
        Dim txtchildfrom As TextBox
        Dim txtchildto As TextBox
        Dim txtchildrate As TextBox
        Dim lblCRowId As Label

        Dim deletedrow As Integer = 0
        Dim n As Integer = 0
        ' For Child Age Grid

        Try


            For Each GVchildRow In gvchildage.Rows

                lblCRowId = GVchildRow.FindControl("lblCRowId")

                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                txtchildto = GVchildRow.FindControl("txtchildageto")
                txtchildrate = GVchildRow.FindControl("txtchildrate")

                If CType(Val(lblCRowId.Text), Integer) <> row.RowIndex + 1 Then
                    childfrom(n) = CType(txtchildfrom.Text, String)
                    childto(n) = CType(txtchildto.Text, String)
                    childrate(n) = CType(txtchildrate.Text, String)
                Else
                    deletedrow = deletedrow + 1

                End If

                n = n + 1
            Next

            If gvchildage.Rows.Count > 1 Then
                fillsubDategrd(gvchildage, False, gvchildage.Rows.Count - 1)
            Else
                fillsubDategrd(gvchildage, True)
            End If



            Dim i As Integer = n
            n = 0

            For Each GVchildRow In gvchildage.Rows
                If n = i Then
                    Exit For
                End If
                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                txtchildto = GVchildRow.FindControl("txtchildageto")
                txtchildrate = GVchildRow.FindControl("txtchildrate")

                txtchildfrom.Text = CType(childfrom(n), String)
                txtchildto.Text = childto(n)
                txtchildrate.Text = childrate(n)

                n = n + 1
            Next

            'count = n
            'If count = 0 Then
            '    count = 1
            'End If
            'fillroomgrid(grdRoomrates, False, count)
            ''If gv_Filldata.Rows.Count > 1 Then
            ''    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count - deletedrow)
            ''Else
            ''    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count)
            ''End If


            'Dim i As Integer = n
            'n = 0

            'For Each GVRow In grdRoomrates.Rows
            '    If n = i Then
            '        Exit For
            '    End If
            '    If GVRow.RowIndex < count Then


            '        txtrmtypcode = GVRow.FindControl("txtrmtypcode")
            '        txtrmtypname = GVRow.FindControl("txtrmtypname")
            '        txtmealcode = GVRow.FindControl("txtmealcode")
            '        txtMinnights = GVRow.FindControl("txtminnights")
            '        ddloptions = GVRow.FindControl("ddloptions")
            '        dpFDate = GVRow.FindControl("txtfromDate")
            '        dpTDate = GVRow.FindControl("txtToDate")



            '        txtrmtypcode.Text = rmtypcode(n)
            '        txtrmtypname.Text = rmtypname(n)
            '        txtmealcode.Text = mealcode(n)
            '        txtMinnights.Text = minnights(n)
            '        ddloptions.Value = options(n)
            '        dpFDate.Text = fDate(n)
            '        dpTDate.Text = tDate(n)


            '        n = n + 1
            '    End If
            'Next
            ''chek Enablegrid()

            Call fnCalculateVATValue() 'changed by mohamed on 21/02/2018
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub



    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click

        ' Createdatacolumns("add")

        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("MainrowId", GetType(String))
        dt.Columns.Add("SrNo", GetType(String))
        dt.Columns.Add("Agefrom", GetType(String))
        dt.Columns.Add("AgeTo", GetType(String))
        dt.Columns.Add("childrate", GetType(String))


        Session("childdata") = Nothing

        ' Addlines()
        Dim count As Integer
        ' Dim chcount As Integer
        Dim GVRow As GridViewRow
        ' Dim GVchildRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1

        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim SplEventsname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim roomocc(count) As String
        Dim adultrate(count) As String
        Dim active(count) As String
        Dim SplEventscode(count) As String
        Dim detailremarks(count) As String

        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtSplEventsname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtrmcatcode As TextBox

        Dim txtadultrate As TextBox
        Dim chkActive As CheckBox
        Dim chkSelect As CheckBox
        Dim txtSplEventscode As TextBox
        Dim txtdetailremarks As TextBox

        ' Dim Gvchildage As GridView
        '   CopyRow = 0
        Dim gvchildage As GridView
        Dim GVchildRow As GridViewRow
        Dim txtchildfrom As TextBox
        Dim txtchildto As TextBox
        Dim txtchildrate As TextBox
        Dim lblMRowId As Label
        Dim lblRowId As Label
        Dim lblCRowId As Label
        Dim nchild As Integer = 0
        Dim chcount As Integer
        Dim childfrom(chcount) As String
        Dim childto(chcount) As String
        Dim childrate(chcount) As String
        Dim maingridid(chcount) As String
        Dim childgridid(chcount) As String
        Dim rowid(count) As String
        Dim grdrowid(count) As String
        Dim arrcount As Integer = 0
        Dim ch As Integer = 0
        Try
            'For Main Grid
            For Each GVRow In gv_Filldata.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtadultrate = GVRow.FindControl("txtadultrate")
                chkActive = GVRow.FindControl("chkActive")
                chkSelect = GVRow.FindControl("chkSelect")
                txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                lblRowId = GVRow.FindControl("lblRowId")
                txtdetailremarks = GVRow.FindControl("txtdetailremarks")

                lblMRowId = GVRow.FindControl("lblMRowId")
                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                fDate(n) = CType(dpFDate.Text, String)
                tDate(n) = CType(dpTDate.Text, String)

                Roomtype(n) = CType(txtRoomtype.Text, String)
                Mealplan(n) = CType(txtMealplan.Text, String)
                SplEventscode(n) = CType(txtSplEventscode.Text, String)
                SplEventsname(n) = CType(txtSplEventsname.Text, String)
                detailremarks(n) = CType(txtdetailremarks.Text, String)

                roomocc(n) = CType(txtrmcatcode.Text, String)
                adultrate(n) = CType(txtadultrate.Text, String)
                rowid(n) = CType(lblMRowId.Text, String)
                grdrowid(n) = CType(lblRowId.Text, String)

                If chkActive.Checked = True Then
                    active(n) = 1
                Else
                    active(n) = 0
                End If

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtadultrate IsNot Nothing Then
                    Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                End If


                'Dim gvchildage As GridView
                'Dim GVchildRow As GridViewRow
                gvchildage = GVRow.FindControl("Gv_childage")
                'nchild = chcount
                'chcount = chcount + gvchildage.Rows.Count
                'ReDim Preserve childfrom(chcount)
                'ReDim Preserve childto(chcount)
                'ReDim Preserve childrate(chcount)
                'ReDim Preserve maingridid(chcount)
                'ReDim Preserve childgridid(chcount)

                ' For Child Age Grid


                For Each GVchildRow In gvchildage.Rows
                    txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                    txtchildto = GVchildRow.FindControl("txtchildageto")
                    txtchildrate = GVchildRow.FindControl("txtchildrate")
                    lblCRowId = GVchildRow.FindControl("lblCRowId")

                    dr = dt.NewRow

                    dr("MainrowId") = CType(lblRowId.Text, String)
                    dr("SrNo") = lblCRowId.Text
                    dr("Agefrom") = txtchildfrom.Text
                    dr("AgeTo") = txtchildto.Text
                    dr("childrate") = txtchildrate.Text
                    dt.Rows.Add(dr)

                    'changed by mohamed on 21/02/2018
                    Dim txtCh2 As TextBox = GVchildRow.FindControl("txtchildrate")
                    Dim txtTVCh2 As TextBox = Nothing
                    Dim txtNTVCh2 As TextBox = Nothing
                    Dim txtVATCh2 As TextBox = Nothing
                    txtTVCh2 = GVchildRow.FindControl("txtTV")
                    txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                    txtVATCh2 = GVchildRow.FindControl("txtVAT")

                    If txtCh2 IsNot Nothing Then
                        Call assignVatValueToTextBox(txtCh2, txtTVCh2, txtNTVCh2, txtVATCh2)
                    End If

                    'maingridid(nchild) = CType(lblMRowId.Text, String)
                    'childgridid(nchild) = CType(lblCRowId.Text, String)
                    'childfrom(nchild) = CType(txtchildfrom.Text, String)
                    'childto(nchild) = CType(txtchildto.Text, String)
                    'childrate(nchild) = CType(txtchildrate.Text, String)
                    'nchild = nchild + 1


                Next



                '  fillDategrd(gvchildage, False, gvchildage.Rows.Count + 1)
                n = n + 1

            Next
            '   Session("childdatacopy") = Nothing
            Session("childdata") = dt
            '   Session("childdatacopy") = Session("childdata")
            fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count + 1)




            Dim i As Integer = n
            n = 0

            For Each GVRow In gv_Filldata.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtadultrate = GVRow.FindControl("txtAdultRate")
                chkActive = GVRow.FindControl("chkActive")
                txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                lblMRowId = GVRow.FindControl("lblMRowId")
                lblRowId = GVRow.FindControl("lblRowId")
                txtdetailremarks = GVRow.FindControl("txtdetailremarks")


                gvchildage = GVRow.FindControl("Gv_childage")


                chcount = nchild + gvchildage.Rows.Count

                dpFDate.Text = fDate(n)
                dpTDate.Text = tDate(n)
                txtSplEventscode.Text = SplEventscode(n)
                txtSplEventsname.Text = SplEventsname(n)
                txtRoomtype.Text = Roomtype(n)
                txtMealplan.Text = Mealplan(n)
                txtrmcatcode.Text = roomocc(n)
                txtdetailremarks.Text = detailremarks(n)

                txtadultrate.Text = adultrate(n)
                lblMRowId.Text = n + 1 'rowid(n)
                lblRowId.Text = grdrowid(n)

                If active(n) = 1 Then
                    chkActive.Checked = True
                Else
                    chkActive.Checked = False
                End If

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtadultrate IsNot Nothing Then
                    Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                End If


                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try



    End Sub
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            ' Dim txtbox As TextBox
            Dim roomtypes As String = ""

            '   roomtypes = getroomtypes()

            Dim chkSelect As CheckBox

            Dim strChkdStringVal As StringBuilder = New StringBuilder()
            Dim tickedornot As Boolean

            tickedornot = False
            For Each grdRow In gv_Showroomtypes.Rows
                chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                chkSelect = grdRow.FindControl("chkrmtype")
                If chkSelect.Checked = True Then
                    tickedornot = True
                    Exit For
                End If
            Next



            Dim chk2 As CheckBox
            Dim txtrmtypcode1 As Label


            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode1 = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode1.Text + ","

                End If
            Next

            If roomtypes Is Nothing Then
                roomtypes = "All"

            Else
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
            End If

            If gv_Showroomtypes.HeaderRow.Cells(3).Text = "Meal Plan" Then
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtmealcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                    If roomtypes.ToString = "" And tickedornot = False Then
                        txtmealcode.Text = "All"
                    Else
                        txtmealcode.Text = roomtypes.ToString
                    End If

                End If
            ElseIf gv_Showroomtypes.HeaderRow.Cells(3).Text = "RoomCategory Name" Then
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtrmcatcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtrmcatcode")
                    If roomtypes.ToString = "" And tickedornot = False Then
                        txtrmcatcode.Text = "All"
                    Else
                        txtrmcatcode.Text = roomtypes.ToString
                    End If

                End If
            Else
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtrmtypcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                    If roomtypes.ToString = "" And tickedornot = False Then
                        txtrmtypcode.Text = "All"
                    Else
                        txtrmtypcode.Text = roomtypes.ToString
                    End If

                End If
            End If





            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSplEventslist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim spleventslist As New List(Of String)
        Dim conid As String


        Try
            If Convert.ToString(HttpContext.Current.Session("Calledfrom").ToString()) = "Offers" Then

                strSqlQry = "select spleventcode,inactive,spleventname from party_splevents p where  p.inactive=0   and p.partycode='" & Convert.ToString(HttpContext.Current.Session("Offerparty").ToString()) & "' and   p.spleventname like  '" & Trim(prefixText) & "%'  order by p.spleventname  "

            Else
                conid = Convert.ToString(HttpContext.Current.Session("contractid").ToString())
                strSqlQry = "select spleventcode,inactive,spleventname from party_splevents p, view_contracts_search c where  p.inactive=0 and c.contractid ='" & conid & "'  and p.partycode=c.partycode and   p.spleventname like  '" & Trim(prefixText) & "%'  order by p.spleventname  "
            End If



            'strSqlQry = " select h.exhibitionname,h.exhibitioncode  from exhibition_master h,exhibition_detail d,view_contracts_search c(nolock)   where h.active=1 and  h.exhibitioncode =d.exhibitioncode  and c.contractid ='" & conid & "' and  ((convert(varchar(10),d.fromdate,111) between c.fromdate   and c.todate )   " _
            '    & "  or   (convert(varchar(10),d.todate,111) between c.fromdate   and c.todate)  or (convert(varchar(10),d.fromdate,111) < c.fromdate  and  convert(varchar(10),d.todate,111)>c.todate))  and  h.exhibitionname like  '" & Trim(prefixText) & "%'  order by h.exhibitionname "




            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'roomclasslist.Add(myDS.Tables(0).Rows(i)("roomclassname").ToString())
                    spleventslist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("spleventname").ToString(), myDS.Tables(0).Rows(i)("spleventcode").ToString()))
                Next

            End If

            Return spleventslist
        Catch ex As Exception
            Return spleventslist
        End Try

    End Function
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentListSearch(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim lsAgentNames As New List(Of String)
        Dim lsCountryList As String
        Try

            'strSqlQry = "select agentname from agentmast where active=1 and agentname like  '" & prefixText & "%'"
            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList
            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
                End If

            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'lsAgentNames.Add(myDS.Tables(0).Rows(i)("agentname").ToString())
                    lsAgentNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next

            End If

            Return lsAgentNames
        Catch ex As Exception

            Return lsAgentNames
        End Try

    End Function
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_specialevents_header Where splistcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("splistcode")) = False Then
                        txttranid.Text = CType(mySqlReader("splistcode"), String)
                    End If

                    If IsDBNull(mySqlReader("contractid")) = False And ViewState("CopyFrom") Is Nothing = True Then
                        hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    End If

                    'If IsDBNull(mySqlReader("contractid")) = False Then
                    '    hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    'End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtremarks.Text = CType(mySqlReader("remarks"), String)
                    End If

                    If IsDBNull(mySqlReader("compulsory")) = False Then
                        ddlcompoption.SelectedIndex = CType(mySqlReader("compulsory"), String)
                    End If


                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_specialevents_header(nolock) where  splistcode ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
                    End If

                    '------------------------------
                    'Changed by mohamed on 21/02/2018
                    If IsDBNull(mySqlReader("VATCalculationRequired")) = False Then
                        chkVATCalculationRequired.Checked = IIf(CType(mySqlReader("VATCalculationRequired"), String) = "1", True, False)
                    Else
                        chkVATCalculationRequired.Checked = False
                    End If
                    txtServiceCharges.Text = "0"
                    TxtMunicipalityFees.Text = "0"
                    txtTourismFees.Text = "0"
                    txtVAT.Text = "0"

                    If IsDBNull(mySqlReader("ServiceChargePerc")) = False Then
                        txtServiceCharges.Text = mySqlReader("ServiceChargePerc")
                    End If
                    If IsDBNull(mySqlReader("MunicipalityFeePerc")) = False Then
                        TxtMunicipalityFees.Text = mySqlReader("MunicipalityFeePerc")
                    End If
                    If IsDBNull(mySqlReader("TourismFeePerc")) = False Then
                        txtTourismFees.Text = mySqlReader("TourismFeePerc")
                    End If
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        txtVAT.Text = mySqlReader("VATPerc")
                    End If
                    '------------------------------

                    '*** Danny 20/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    If CType(mySqlReader("MarkupAdultOp"), String).Trim().Length > 0 Then
                        CheckBox1MarkupRequired.Checked = True
                        txtMarkupAdultOp.Enabled = True
                        txtMarkupAdultVal.Enabled = True
                        txtMarkupChildOp.Enabled = True
                        txtMarkupChildVal.Enabled = True
                    Else
                        CheckBox1MarkupRequired.Checked = False
                        txtMarkupAdultOp.Enabled = False
                        txtMarkupAdultVal.Enabled = False
                        txtMarkupChildOp.Enabled = False
                        txtMarkupChildVal.Enabled = False
                    End If
                    txtMarkupAdultOp.Text = CType(mySqlReader("MarkupAdultOp"), String).Trim()
                    txtMarkupAdultVal.Text = CType(mySqlReader("MarkupAdultVal"), String).Trim()
                    txtMarkupChildOp.Text = CType(mySqlReader("MarkupChildOp"), String).Trim()
                    txtMarkupChildVal.Text = CType(mySqlReader("MarkupChildVal"), String).Trim()
                    '*** Danny 20/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                End If
            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    Private Sub ShowRoomdetails(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_Filldata.Visible = True
            strSqlQry = ""

123:
            Dim strQry As String
            Dim cnt As Integer = 0


            strQry = "select count( distinct splineno) from view_contracts_specialevents_detail(nolock) where splistcode='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            fillDategrd(gv_Filldata, False, cnt)

            If cnt > 0 Then

                ' Rosalin 2019-11-20
                'strSqlQry = "select d.splistcode,d.spleventcode, h.spleventname,d.fromdate,d.todate,d.roomtypes,d.mealplans,d.roomcategory,d.adultrate,d.childdetails,d.active,d.Detailremarks " _
                '    & " from view_contracts_specialevents_detail d(nolock),party_splevents h where d.spleventcode=h.spleventcode   and h.partycode ='" & hdnpartycode.Value & "'  and   d.splistcode='" & RefCode & "' order by d.splineno"


                strSqlQry = " exec new_contracts_specialevents_detail '" & hdnpartycode.Value & "' , '" & RefCode & "' "

                'strSqlQry = "select d.splistcode,d.spleventcode, h.spleventname,d.fromdate,d.todate,d.roomtypes,d.mealplans,d.roomcategory,d.adultrate,d.childdetails,d.active,d.Detailremarks " _
                '& " from view_contracts_specialevents_detail d(nolock),party_splevents h(nolock),view_contracts_search v(nolock) ,view_contracts_specialevents_header vh(nolock) where d.spleventcode=h.spleventcode  and  " _
                '& "   vh.splistcode =d.splistcode  and vh.contractid =v.contractid  and   v.contractid ='" & hdncontractid.Value & "'  and v.partycode =h.partycode  and d.splistcode='" & RefCode & "' order by d.splineno"


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                myCommand.CommandTimeout = 0
                mySqlReader = myCommand.ExecuteReader


                Dim dpFDate As TextBox
                Dim dpTDate As TextBox
                Dim txtSplEventsname As TextBox
                Dim txtRoomtype As TextBox
                Dim txtMealplan As TextBox
                Dim txtrmcatcode As TextBox

                Dim txtadultrate As TextBox
                Dim chkActive As CheckBox
                Dim chkSelect As CheckBox
                Dim txtSplEventscode As TextBox


                Dim gvchildage As GridView
                Dim GVchildRow As GridViewRow
                Dim txtchildfrom As TextBox
                Dim txtchildto As TextBox
                Dim txtchildrate As TextBox
                Dim lblMRowId As Label
                Dim lblRowId As Label
                Dim lblCRowId As Label
                Dim childratesstring As String = ""

                For Each GvRow In gv_Filldata.Rows

                    dpFDate = GvRow.FindControl("txtfromDate")
                    dpTDate = GvRow.FindControl("txtToDate")
                    txtSplEventsname = GvRow.FindControl("txtSplEventsname")
                    txtRoomtype = GvRow.FindControl("txtrmtypcode")
                    txtMealplan = GvRow.FindControl("txtmealcode")
                    txtrmcatcode = GvRow.FindControl("txtrmcatcode")
                    txtadultrate = GvRow.FindControl("txtadultrate")
                    chkActive = GvRow.FindControl("chkActive")
                    chkSelect = GvRow.FindControl("chkSelect")
                    txtSplEventscode = GvRow.FindControl("txtSplEventscode")
                    lblRowId = GvRow.FindControl("lblRowId")
                    Dim txtdetailremarks As TextBox = GvRow.findcontrol("txtdetailremarks")

                    lblMRowId = GvRow.FindControl("lblMRowId")

                    gvchildage = GvRow.FindControl("Gv_childage")

                    If mySqlReader.Read = True Then


                        If IsDBNull(mySqlReader("spleventcode")) = False Then
                            txtSplEventscode.Text = mySqlReader("spleventcode")
                            txtSplEventsname.Text = mySqlReader("spleventname")


                        End If

                        If IsDBNull(mySqlReader("roomtypes")) = False Then
                            txtRoomtype.Text = mySqlReader("roomtypes")

                        End If
                        If IsDBNull(mySqlReader("mealplans")) = False Then
                            txtMealplan.Text = mySqlReader("mealplans")

                        End If
                        If IsDBNull(mySqlReader("roomcategory")) = False Then
                            txtrmcatcode.Text = mySqlReader("roomcategory")


                            Dim strrmcat As String = ""
                            Dim strrmcatCondition As String = ""
                            strrmcat = mySqlReader("roomcategory")
                            If strrmcat.Length > 0 Then
                                Dim mString As String() = strrmcat.Split(",")
                                For k As Integer = 0 To mString.Length - 1
                                    If strrmcatCondition = "" Then
                                        strrmcatCondition = "'" & mString(k) & "'"
                                    Else
                                        strrmcatCondition &= ",'" & mString(k) & "'"
                                    End If
                                Next
                            End If
                            strQry = "select distinct stuff((select  ',' + u.rmcatcode   from partyrmcat u(nolock),rmcatmast r  where u.rmcatcode=r.rmcatcode and   u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmcatcode in (" & strrmcatCondition & ")   group by u.rmcatcode    for xml path('')),1,1,'')  rmcatcode " _
                                & " from partyrmcat where partycode='" & hdnpartycode.Value & "' and partyrmcat.rmcatcode in (" & strrmcatCondition & ") "

                            txtrmcatcode.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry) '"select rmtypname from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") ")

                            If txtrmcatcode.Text = "" Then txtrmcatcode.Text = "All"


                        End If



                        If IsDBNull(mySqlReader("adultrate")) = False Then
                            txtadultrate.Text = DecRound(mySqlReader("adultrate"))

                        End If


                        If IsDBNull(mySqlReader("active")) = False Then
                            If mySqlReader("active") = 1 Then
                                chkActive.Checked = True
                            Else
                                chkActive.Checked = False
                            End If
                        End If

                        If IsDBNull(mySqlReader("Detailremarks")) = False Then
                            txtdetailremarks.Text = mySqlReader("Detailremarks")
                        End If


                        If IsDBNull(mySqlReader("fromdate")) = False Then


                            dpFDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then


                            dpTDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                        End If

                        'changed by mohamed on 21/02/2018
                        Dim txtTV1 As TextBox = Nothing
                        Dim txtNTV1 As TextBox = Nothing
                        Dim txtVAT1 As TextBox = Nothing
                        txtTV1 = GvRow.FindControl("txtTV")
                        txtNTV1 = GvRow.FindControl("txtNTV")
                        txtVAT1 = GvRow.FindControl("txtVAT")
                        If txtadultrate IsNot Nothing Then
                            Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                        End If

                        Dim strchilds As String = ""
                        Dim arrlist As String()
                        Dim childdetails As String()
                        Dim rate As String = ""
                        If IsDBNull(mySqlReader("childdetails")) = False Then


                            strchilds = mySqlReader("childdetails")
                            childdetails = strchilds.ToString.Trim.Split(";")

                        End If
                        fillsubDategrd(gvchildage, False, childdetails.Length)

                        Dim i As Integer = 0
                        For Each GVchildRow In gvchildage.Rows



                            txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                            txtchildto = GVchildRow.FindControl("txtchildageto")
                            txtchildrate = GVchildRow.FindControl("txtchildrate")
                            lblCRowId = GVchildRow.FindControl("lblCRowId")


                            If childdetails(i) <> "" Then

                                arrlist = childdetails(i).Split(",")

                                txtchildfrom.Text = CType(arrlist(0), Decimal)
                                txtchildto.Text = CType(arrlist(1), Decimal)
                                rate = CType(arrlist(2), Decimal)
                                Select Case DecRound(arrlist(2))
                                    Case "-3"
                                        rate = "Free"
                                    Case "-1"
                                        rate = "Incl"
                                    Case "-2"
                                        rate = "N.Incl"
                                    Case "-4"
                                        rate = "N/A"
                                    Case "-5"
                                        rate = "On Request"
                                End Select


                                txtchildrate.Text = rate


                                Dim txtTVCh2 As TextBox = Nothing
                                Dim txtNTVCh2 As TextBox = Nothing
                                Dim txtVATCh2 As TextBox = Nothing
                                txtTVCh2 = GVchildRow.FindControl("txtTV")
                                txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                                txtVATCh2 = GVchildRow.FindControl("txtVAT")

                                If txtchildrate IsNot Nothing Then
                                    Call assignVatValueToTextBox(txtchildrate, txtTVCh2, txtNTVCh2, txtVATCh2)
                                End If
                            End If

                            i += 1


                        Next




                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        ' FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub

#End Region
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltran")
            If lbltran.Text.Trim = "" Then Exit Sub

            If e.CommandName <> "View" Then

                If Session("Calledfrom") = "Offers" Then
                    Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdnpromotionid.Value & "'")

                    If offerexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Offer Main Details First');", True)
                        Exit Sub

                    End If
                Else
                    Dim contexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_contracts(nolock) where contractid='" & hdncontractid.Value & "'")

                    If contexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Contract Main Details First');", True)
                        Exit Sub

                    End If
                End If
            End If


            If e.CommandName = "EditRow" Then
                ViewState("State") = "Edit"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))

                ' Showdetailsgrid(CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit  SplEvents PriceList  - " + ViewState("hotelname") + " - " + IIf(Session("Calledfrom") = "Offers", hdnpromotionid.Value, hdncontractid.Value)
                Page.Title = Page.Title + " " + "  SplEvents PriceList   "
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View  SplEvents PriceList  - " + ViewState("hotelname") + " - " + IIf(Session("Calledfrom") = "Offers", hdnpromotionid.Value, hdncontractid.Value)
                Page.Title = Page.Title + " " + "  SplEvents PriceList  "
            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete  SplEvents PriceList  - " + ViewState("hotelname") + " - " + IIf(Session("Calledfrom") = "Offers", hdnpromotionid.Value, hdncontractid.Value)
                Page.Title = Page.Title + " " + "  SplEvents PriceList  "
            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                txttranid.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy SplEvents PriceList - " + ViewState("hotelname") + " - " + IIf(Session("Calledfrom") = "Offers", hdnpromotionid.Value, hdncontractid.Value)
                Page.Title = Page.Title + " " + "  SplEvents PriceList  "
            End If




        Catch ex As Exception
            objUtils.WritErrorLog("ContractSplEventPList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub FillGrid(ByVal strsortby As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            If Session("Calledfrom") = "Offers" Then
                If strsortby = "fromdate" Or strsortby = "todate" Then
                    strSqlQry = "With ctee as (SELECT h.splistcode,convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,h.applicableto, h.adddate,h.adduser,h.moddate,h.moduser FROM view_contracts_specialevents_header h(nolock),view_contracts_specialevents_detail d(nolock) " _
                        & " where h.splistcode=d.splistcode  and  h.promotionid='" & contractid & "' group by h.splistcode,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee  order by convert(datetime," & strsortby & "  ,103)  " & strsortorder & " "
                Else

                    strSqlQry = "SELECT h.splistcode,convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,h.applicableto, h.adddate,h.adduser,h.moddate,h.moduser FROM view_contracts_specialevents_header h(nolock),view_contracts_specialevents_detail d(nolock) " _
                        & " where h.splistcode=d.splistcode  and  h.promotionid='" & contractid & "' group by h.splistcode,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & " " & strsortorder & " "

                End If
            Else
                If strsortby = "fromdate" Or strsortby = "todate" Then
                    strSqlQry = "With ctee as (SELECT h.splistcode,convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,h.applicableto, h.adddate,h.adduser,h.moddate,h.moduser FROM view_contracts_specialevents_header h(nolock),view_contracts_specialevents_detail d(nolock) " _
                        & " where h.splistcode=d.splistcode  and  h.contractid='" & hdncontractid.Value & "' group by h.splistcode,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee  order by convert(datetime," & strsortby & "  ,103)  " & strsortorder & " "
                Else

                    strSqlQry = "SELECT h.splistcode,convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,h.applicableto, h.adddate,h.adduser,h.moddate,h.moduser FROM view_contracts_specialevents_header h(nolock),view_contracts_specialevents_detail d(nolock) " _
                        & " where h.splistcode=d.splistcode  and  h.contractid='" & hdncontractid.Value & "' group by h.splistcode,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & " " & strsortorder & " "

                End If
            End If
         
            'strSqlQry = "select genpolicyid,fromdate,todate,applicableto,adddate,adduser,moddate,moduser  from view_contracts_genpolicy_header h " _
            '        & " where contractid='" & hdncontractid.Value & "'"



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1

        Dim n As Integer = 0


        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("MainrowId", GetType(String))
        dt.Columns.Add("SrNo", GetType(String))
        dt.Columns.Add("Agefrom", GetType(String))
        dt.Columns.Add("AgeTo", GetType(String))
        dt.Columns.Add("childrate", GetType(String))

        Session("childdata") = Nothing

        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim spleventscode(count) As String
        Dim spleventsname(count) As String
        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim adultrate(count) As String
        Dim detailremarks(count) As String
        Dim roomocc(count) As String
        Dim GVchildRow As GridViewRow
        Dim txtchildfrom As TextBox
        Dim txtchildto As TextBox
        Dim txtchildrate As TextBox
        Dim txtdetailremarks As TextBox

        Dim lblCRowId As Label
        Dim rowid(count) As String
        Dim fDate(count) As String
        Dim tDate(count) As String

        Session("copyrow") = Nothing
        Session("newcopyrow") = Nothing

        '   CopyRow = 0
        ClearArray()
        blankrow = 0

        Try

            For Each GVRow In gv_Filldata.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")
                Dim lblMRowId As Label = GVRow.FindControl("lblMRowId")
                Dim txtspleventscode As TextBox = GVRow.FindControl("txtSplEventscode")
                Dim txtSplEventsname As TextBox = GVRow.FindControl("txtSplEventsname")
                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtAdultRate As TextBox = GVRow.FindControl("txtAdultRate")
                Dim gvchildage As GridView = GVRow.FindControl("gv_childage")
                Dim lblRowId As Label = GVRow.FindControl("lblRowId")

                txtdetailremarks = GVRow.FindControl("txtdetailremarks")
                '      Dim ddloptions As HtmlSelect = GVRow.FindControl("ddloptions")


                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtAdultRate IsNot Nothing Then
                    Call assignVatValueToTextBox(txtAdultRate, txtTV1, txtNTV1, txtVAT1)
                End If


                If chkSelect.Checked = True Then
                    CopyRowlist.Add(n)
                    CopyRow = n
                    Session("copyrow") = n + 1
                    Session("newcopyrow") = lblRowId.Text
                    childCopyRow.Add(lblRowId.Text)

                    rowid(n) = CType(lblMRowId.Text, String)

                    spleventscode(n) = CType(txtspleventscode.Text, String)
                    spleventsname(n) = CType(txtSplEventsname.Text, String)
                    rmtypcode(n) = CType(txtrmtypcode.Text, String)
                    roomocc(n) = CType(txtrmcatcode.Text, String)
                    'rmtypname(n) = CType(txtrmtypname.Text, String)
                    mealcode(n) = CType(txtmealcode.Text, String)
                    adultrate(n) = CType(txtAdultRate.Text, String)

                    fDate(n) = CType(dpFDate.Text, String)
                    tDate(n) = CType(dpTDate.Text, String)


                    txtSplEventscodenew.Add(CType(txtspleventscode.Text, String))
                    txtSplEventsnamenew.Add(CType(txtSplEventsname.Text, String))
                    txtrmtypcodenew.Add(CType(txtrmtypcode.Text, String))
                    txtrmcatcodenew.Add(CType(txtrmcatcode.Text, String))
                    txtmealcodenew.Add(CType(txtmealcode.Text, String))
                    txtadultratenew.Add(CType(txtAdultRate.Text, String))

                    txtdetailremarksnew.Add(CType(txtdetailremarks.Text, String))
                    '  ddloptionsnew.Add(CType(ddloptions.Value, String))
                    fDatenew.Add(CType(dpFDate.Text, String))
                    tDatenew.Add(CType(dpTDate.Text, String))
                End If
                For Each GVchildRow In gvchildage.Rows
                    txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                    txtchildto = GVchildRow.FindControl("txtchildageto")
                    txtchildrate = GVchildRow.FindControl("txtchildrate")
                    lblCRowId = GVchildRow.FindControl("lblCRowId")

                    'changed by mohamed on 21/02/2018
                    Dim txtTVCh2 As TextBox = Nothing
                    Dim txtNTVCh2 As TextBox = Nothing
                    Dim txtVATCh2 As TextBox = Nothing
                    txtTVCh2 = GVchildRow.FindControl("txtTV")
                    txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                    txtVATCh2 = GVchildRow.FindControl("txtVAT")

                    If txtchildrate IsNot Nothing Then
                        Call assignVatValueToTextBox(txtchildrate, txtTVCh2, txtNTVCh2, txtVATCh2)
                    End If

                    dr = dt.NewRow

                    dr("MainrowId") = CType(lblMRowId.Text, String)
                    dr("SrNo") = lblCRowId.Text
                    dr("Agefrom") = txtchildfrom.Text
                    dr("AgeTo") = txtchildto.Text
                    dr("childrate") = txtchildrate.Text
                    dt.Rows.Add(dr)



                Next

                If chkSelect.Checked = False And (txtSplEventsname.Text = "") Then
                    blankrow = blankrow + 1
                End If

                n = n + 1
            Next
            Session("childdata") = dt

            Dim k As Integer
            k = blankrow + CopyRowlist.Count
            Do While blankrow < (CopyRowlist.Count)

                btnAddrow_Click(Nothing, Nothing)


                blankrow = blankrow + 1
            Loop


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub
    Protected Sub btncopyratesnextrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyratesnextrow.Click

        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = gv_Filldata.Rows.Count '+ 1


            Dim n As Integer = 0
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            Dim k As Integer = 0

            For Each GVRow In gv_Filldata.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")
                Dim txtspleventscode As TextBox = GVRow.FindControl("txtspleventscode")
                Dim txtspleventsname As TextBox = GVRow.FindControl("txtspleventsname")
                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                'Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtadultrate As TextBox = GVRow.FindControl("txtadultrate")

                Dim txtdetailremarks As TextBox = GVRow.FindControl("txtdetailremarks")

                Dim gvchildage As GridView = GVRow.FindControl("gv_childage")
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")

                Dim lblMainRowId As Label
                ' Dim dpFDate As TextBox
                gvchildage = GVRow.FindControl("Gv_childage")
                lblMainRowId = GVRow.FindControl("lblMRowId")

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtadultrate IsNot Nothing Then
                    Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                End If



                'If n > CopyRow And txtrmtypname.Text = "" Then

                'If n > CopyRow And txtrmtypcode.Text <> "" And txtspleventsname.Text = "" Then
                If txtspleventsname.Text = "" Then

                    txtspleventscode.Text = txtSplEventscodenew.Item(k)
                    txtspleventsname.Text = txtSplEventsnamenew.Item(k)
                    txtrmtypcode.Text = txtrmtypcodenew.Item(k)
                    txtrmcatcode.Text = txtrmcatcodenew.Item(k)
                    'txtrmtypname.Text = txtrmtypnamenew.Item(CopyRow)
                    txtmealcode.Text = txtmealcodenew.Item(k)
                    txtadultrate.Text = txtadultratenew.Item(k)

                    txtdetailremarks.Text = txtdetailremarksnew.Item(k)
                    'txtroomrates.Text = txtminnightsnew.Item(CopyRow)
                    'ddloptions.Value = ddloptionsnew.Item(CopyRow)



                    If CType(fDatenew.Item(k), String) <> "" Then
                        dpFDate.Text = Format(CType(fDatenew.Item(k), Date), "dd/MM/yyyy")
                        dpTDate.Text = Format(CType(tDatenew.Item(k), Date), "dd/MM/yyyy")
                    End If
                    Dim dt As DataTable, dv As DataView
                    dt = Session("childdata")




                    If dt IsNot Nothing Then
                        dv = New DataView(dt)
                        'dv.RowFilter = "mainrowid=" & CType(Session("newcopyrow"), Integer)
                        dv.RowFilter = "mainrowid=" & CType(childCopyRow(k), Integer)
                        If dv.ToTable.Rows.Count > 0 Then
                            gvchildage.DataSource = dv.ToTable
                            gvchildage.DataBind()


                        End If
                    End If



                    'Exit For
                    k = k + 1
                End If

                If k >= CopyRowlist.Count Then
                    Exit For
                End If
                n = n + 1

            Next

            CopyClick = 0
            ClearArray()
            '  Enablegrid()
            ' setdynamicvalues()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


    End Sub
    Protected Sub grdviewrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdviewrates.RowCommand

        Try

            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            Dim lblcontract As Label
            lbltran = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            lblcontract = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then
                hdncopycontractid.Value = CType(lblcontract.Text, String)
                PanelMain.Visible = True
                ViewState("CopyFrom") = "CopyFrom"
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"

                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                wucCountrygroup.sbSetPageState(CType(Session("contractid"), String), Nothing, "Edit")

                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                fillheader()
                DisableControl()
                btnSave.Visible = True
                txttranid.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy SplEvents PriceList - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + "  SplEvents PriceList  "


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub ReadMoreLinkButtoncopycont_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalViewrates.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Function Limitcopy(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Sub ClearArray()


        txtSplEventscodenew.Clear()
        txtrmtypnamenew.Clear()
        txtrmtypcodenew.Clear()
        txtrmtypnamenew.Clear()
        txtmealcodenew.Clear()
        txtadultratenew.Clear()
        txtdetailremarksnew.Clear()
        ' ddloptionsnew.Clear()
        fDatenew.Clear()
        tDatenew.Clear()

        'For Each GVRow In gv_Filldata.Rows
        '    ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

        '    Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")
        '    chkSelect.Checked = False
        'Next


    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Dim RefCode As String
        Dim CalledfromValue As String = ""

        Dim Minappid As String = ""
        Dim Minappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        If IsPostBack = False Then
            Minappid = 1
            Minappname = objUser.GetAppName(Session("dbconnectionName"), Minappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(Minappname, String), "ContractSplEventPlist.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then

                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(Minappname, String), "ContractSplEventPlist.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If


        



            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractSplEventPlist.aspx", Minappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, Minappid, intMenuID)

            If functionalrights <> "" Then
                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "07" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnselect.Visible = False
                    If Count = 1 Then
                        btncopycontract.Visible = True
                    Else
                        btncopycontract.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    btncopycontract.Visible = False
                    If Count = 1 Then
                        btnselect.Visible = True
                        btncopycontract.Visible = True
                    Else
                        btnselect.Visible = False
                        btncopycontract.Visible = False
                    End If
                End If

            

            Else

                btncopycontract.Visible = False

            End If


            Session("childdata") = Nothing

            If Session("Calledfrom") = "Offers" Then

              
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)


                txtconnection.Value = Session("dbconnectionName")


                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Session("partycode") = hdnpartycode.Value
                divoffer.Style.Add("display", "block")


                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                '  wucCountrygroup.Visible = False

                '  wucCountrygroup.sbSetPageState("", "CONTRACTSPLEVENTPLIST", CType(Session("OfferState"), String))
                wucCountrygroup.sbSetPageState("", "OFFERSSPLEVENTPLIST", CType(Session("OfferState"), String))

                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionid.Text = Session("OfferRefCode")
                    Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,commissiontype  from view_offers_header (nolock) where  promotionid='" & hdnpromotionid.Value & "'")
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                        txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                        hdncommtype.Value = ds.Tables(0).Rows(0).Item("commissiontype")
                    End If

                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                        hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                    End If


                End If


                '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Promotion Special Events"

                'lblbookingvaltype.Visible = False
                'ddlBookingValidity.Visible = False

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                FillGrid("h.splistcode", hdnpromotionid.Value, hdnpartycode.Value, "Desc")


            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))


                txtconnection.Value = Session("dbconnectionName")
                'txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                'txtToDate.Text = Now.ToString("dd/MM/yyyy")


                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
                'Session("contractid") = hdncontractid.Value
                'Session("partycode") = hdnpartycode.Value

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                '  wucCountrygroup.Visible = False

                wucCountrygroup.sbSetPageState("", "CONTRACTSPLEVENTPLIST", CType(Session("ContractState"), String))
                divoffer.Style.Add("display", "none")

                '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdncontractid.Value

                Page.Title = "Contract Special Events"

                'lblbookingvaltype.Visible = False
                'ddlBookingValidity.Visible = False

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                FillGrid("h.splistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")
            End If

         
            '  PanelMain.Visible = False

            'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If
        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If

        hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")

        chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

        If Session("Calledfrom") <> "Offers" Then
            btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        End If

        '*** Danny 21/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        'Session.Add("1002", objUtils.GetString("strDBConnection", "select option_selected from reservation_parameters where param_id=3")) '*** Danny Added 19/03/2018
        'If IsNothing(Session("1002")) Then ''*** Danny 1002 is Excel Document number 
        '    Session("1002") = String.Empty
        'End If
        'If Session("1002").ToString() <> "SHOW" Then
        '    PnlMarkUp.Visible = False
        'End If
        '*** Danny 21/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
    Protected Sub ReadMoreLinkButtonpromotion_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If
            ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            grdpromotion.Visible = True


            Dim MyDs As New DataTable
            Dim countryList As String = ""
            Dim agentList As String = ""
            Dim filterCond As String = ""
            If wucCountrygroup.checkcountrylist.ToString().Trim <> "" Then
                countryList = wucCountrygroup.checkcountrylist.ToString().Trim.Replace(",", "','")
                filterCond = "h.promotionid  in (select promotionid from view_offers_countries where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            filterCond = ""
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = " select c.cancelpolicyid plistcode, h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_cancelpolicy_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by h.promotionid,h.approved,h.promotionname,c.cancelpolicyid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                grdpromotion.DataSource = MyDs
                grdpromotion.DataBind()
                grdpromotion.Visible = True
            Else
                grdpromotion.Visible = False
            End If

            ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub
    Protected Sub gv_childage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtchildrate As TextBox = e.Row.FindControl("txtchildrate")
            '------------------------------------------------
            'changed by mohamed on 21/02/2018

            Dim txtTV1 As TextBox = Nothing
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing
            txtTV1 = e.Row.FindControl("txtTV")
            txtNTV1 = e.Row.FindControl("txtNTV")
            txtVAT1 = e.Row.FindControl("txtVAT")
            txtchildrate.Attributes.Add("onchange", "javascript:calculateVAT('" & txtchildrate.ClientID & "','" & txtTV1.ClientID & "','" & txtNTV1.ClientID & "','" & txtVAT1.ClientID & "');")
            If txtchildrate IsNot Nothing Then
                Call assignVatValueToTextBox(txtchildrate, txtTV1, txtNTV1, txtVAT1)
            End If
            '------------------------------------------------
        End If
    End Sub

    Protected Sub gv_Filldata_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Filldata.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
            Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
            Dim txtAdultRate As TextBox = e.Row.FindControl("txtAdultRate")
            Dim lblRowId As Label

            lblRowId = e.Row.FindControl("lblRowId")
            lblRowId.Text = e.Row.RowIndex + 1
            'txtFromDate.Text = Now.ToString("dd/MM/yyyy") ' Format(Now.Date, "dd/MM/yyyy")
            'txtToDate.Text = Now.ToString("dd/MM/yyyy") 'Format(Now.Date, "dd/MM/yyyy")
            txtFromDate.Attributes.Add("onchange", "setdate();")
            txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
            txtFromDate.Attributes.Add("onchange", "checkfromdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

            Numberssrvctrl(txtAdultRate)

            '------------------------------------------------
            'changed by mohamed on 21/02/2018

            Dim txtTV1 As TextBox = Nothing
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing
            txtTV1 = e.Row.FindControl("txtTV")
            txtNTV1 = e.Row.FindControl("txtNTV")
            txtVAT1 = e.Row.FindControl("txtVAT")
            txtAdultRate.Attributes.Add("onchange", "javascript:calculateVAT('" & txtAdultRate.ClientID & "','" & txtTV1.ClientID & "','" & txtNTV1.ClientID & "','" & txtVAT1.ClientID & "');")

            'If txtAdultRate IsNot Nothing Then
            '    Call assignVatValueToTextBox(txtAdultRate, txtTV1, txtNTV1, txtVAT1)
            'End If
            '------------------------------------------------


            If Session("childdata") Is Nothing Then

                Dim Gvchildage As GridView

                Gvchildage = e.Row.FindControl("Gv_childage")


                fillsubDategrd(Gvchildage, True)




            Else

                Dim Gvchildage As GridView, lblMainRowId As Label
                ' Dim dpFDate As TextBox
                Gvchildage = e.Row.FindControl("Gv_childage")
                lblMainRowId = e.Row.FindControl("lblMRowId")
                lblRowId = e.Row.FindControl("lblRowId")

                Dim dt As DataTable, dv As DataView
                dt = Session("childdata")

                If dt IsNot Nothing Then
                    dv = New DataView(dt)
                    dv.RowFilter = "mainrowid=" & lblRowId.Text
                    If dv.ToTable.Rows.Count > 0 Then
                        Gvchildage.DataSource = dv.ToTable
                        Gvchildage.DataBind()
                    Else
                        fillsubDategrd(Gvchildage, True)
                    End If
                Else
                    fillsubDategrd(Gvchildage, True)
                End If
                ' dpFDate = e.Row.FindControl("txtfromDate")
                ' fillDategrd(Gvchildage, False, CType(Session("subrowcount"), Integer))

            End If

        End If



        ''Dim chcount As Integer
        ''Dim ch As Integer
        ''Gvchildage = e.Row.FindControl("Gv_childage")
        ''chcount = Gvchildage.Rows.Count + 1

        ''Dim childfrom(chcount) As String
        ''Dim childto(chcount) As String
        ''Dim childrate(chcount) As String
        ''Dim txtchildfrom As TextBox
        ''Dim txtchildto As TextBox
        ''Dim txtchildrate As TextBox

        ' '' For Child Age Grid

        ''
        ''For Each GVchildRow In Gvchildage.Rows
        ''    txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
        ''    txtchildto = GVchildRow.FindControl("txtchildageto")
        ''    txtchildrate = GVchildRow.FindControl("txtchildrate")


        ''    childfrom(ch) = CType(txtchildfrom.Text, String)
        ''    childto(ch) = CType(txtchildto.Text, String)
        ''    childrate(ch) = CType(txtchildrate.Text, String)
        ''    ch = ch + 1
        ''Next

        ''fillDategrd(Gvchildage, False, Gvchildage.Rows.Count + 1)

    End Sub
    Private Sub DisableControl()
        If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

            'txtpolicy.Enabled = True
            'txtfromDate.Enabled = True
            'txtToDate.Enabled = True

            txtApplicableTo.Enabled = True
            'txtplistcode.Text = ""
            If ViewState("State") = "New" Then
                'txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                'txtToDate.Text = Now.ToString("dd/MM/yyyy")
                'txtpolicy.Text = ""
            End If
            wucCountrygroup.Disable(True)
        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then



            'dpFromDate.Enabled = False
            'dpToDate.Enabled = False
            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False

            txtremarks.Enabled = False
            'txtfromDate.Enabled = False
            'txtToDate.Enabled = False


        ElseIf ViewState("State") = "Edit" Then

            'dpFromDate.Enabled = True
            'dpToDate.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            'txtpolicy.Enabled = True
            'txtfromDate.Enabled = True
            'txtToDate.Enabled = True


        End If
    End Sub
    Protected Sub btnreset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset1.Click
        Session("childdata") = Nothing

        ' clearall()
        Panelsearch.Enabled = True

        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")

        lblHeading.Text = "Special Events-PriceList  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "ContractSplEventPlist", CType(Session("ContractState"), String))
        Response.Redirect(Request.RawUrl)
    End Sub





    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        Dim lblCRowId As Label
        count = gv_Filldata.Rows.Count + 1
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim GVchildRow As GridViewRow
        Dim txtchildfrom As TextBox
        Dim txtchildto As TextBox
        Dim txtchildrate As TextBox
        dt.Columns.Add("MainrowId", GetType(String))
        dt.Columns.Add("SrNo", GetType(String))
        dt.Columns.Add("Agefrom", GetType(String))
        dt.Columns.Add("AgeTo", GetType(String))
        dt.Columns.Add("childrate", GetType(String))


        Session("childdata") = Nothing


        Dim n As Integer = 0

        Dim chkSelect As CheckBox
        Dim chkactive As CheckBox

        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0


        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtMinnights As TextBox
        Dim ddloptions As HtmlSelect

        Dim txtSplEventscode As TextBox
        Dim txtSplEventsname As TextBox
        Dim txtdetailremarks As TextBox
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim SplEventsname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim roomocc(count) As String
        Dim adultrate(count) As String
        Dim active(count) As String
        Dim SplEventscode(count) As String
        Dim detailremarks(count) As String
        Dim rowid(count) As String
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtrmcatcode As TextBox
        Dim lblMRowId As Label
        Dim txtadultrate As TextBox
        Dim gvchildage As GridView

        Try
            For Each GVRow In gv_Filldata.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                    txtRoomtype = GVRow.FindControl("txtrmtypcode")
                    txtMealplan = GVRow.FindControl("txtmealcode")
                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                    txtadultrate = GVRow.FindControl("txtAdultRate")
                    chkactive = GVRow.FindControl("chkActive")
                    txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                    txtdetailremarks = GVRow.FindControl("txtdetailremarks")
                    lblMRowId = GVRow.FindControl("lblMRowId")

                    gvchildage = GVRow.FindControl("Gv_childage")

                    fDate(n) = CType(dpFDate.Text, String)
                    tDate(n) = CType(dpTDate.Text, String)
                    SplEventscode(n) = CType(txtSplEventscode.Text, String)
                    SplEventsname(n) = CType(txtSplEventsname.Text, String)
                    Roomtype(n) = CType(txtRoomtype.Text, String)
                    Mealplan(n) = txtMealplan.Text
                    roomocc(n) = txtrmcatcode.Text

                    detailremarks(n) = txtdetailremarks.Text

                    adultrate(n) = txtadultrate.Text
                    rowid(n) = lblMRowId.Text

                    'changed by mohamed on 21/02/2018
                    Dim txtTV1 As TextBox = Nothing
                    Dim txtNTV1 As TextBox = Nothing
                    Dim txtVAT1 As TextBox = Nothing
                    txtTV1 = GVRow.FindControl("txtTV")
                    txtNTV1 = GVRow.FindControl("txtNTV")
                    txtVAT1 = GVRow.FindControl("txtVAT")

                    If txtadultrate IsNot Nothing Then
                        Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                    End If

                    For Each GVchildRow In gvchildage.Rows
                        txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                        txtchildto = GVchildRow.FindControl("txtchildageto")
                        txtchildrate = GVchildRow.FindControl("txtchildrate")
                        lblCRowId = GVchildRow.FindControl("lblCRowId")

                        Dim txtTVCh2 As TextBox = Nothing
                        Dim txtNTVCh2 As TextBox = Nothing
                        Dim txtVATCh2 As TextBox = Nothing
                        txtTVCh2 = GVchildRow.FindControl("txtTV")
                        txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                        txtVATCh2 = GVchildRow.FindControl("txtVAT")

                        If txtchildrate IsNot Nothing Then
                            Call assignVatValueToTextBox(txtchildrate, txtTVCh2, txtNTVCh2, txtVATCh2)
                        End If

                        dr = dt.NewRow

                        dr("MainrowId") = n + 1 'CType(lblMRowId.Text, String)
                        dr("SrNo") = lblCRowId.Text
                        dr("Agefrom") = txtchildfrom.Text
                        dr("AgeTo") = txtchildto.Text
                        dr("childrate") = txtchildrate.Text
                        dt.Rows.Add(dr)

                        'maingridid(nchild) = CType(lblMRowId.Text, String)
                        'childgridid(nchild) = CType(lblCRowId.Text, String)
                        'childfrom(nchild) = CType(txtchildfrom.Text, String)
                        'childto(nchild) = CType(txtchildto.Text, String)
                        'childrate(nchild) = CType(txtchildrate.Text, String)
                        'nchild = nchild + 1


                    Next

                    n = n + 1

                Else
                    deletedrow = deletedrow + 1

                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If

            Session("childdata") = dt
            fillsubDategrd(gv_Filldata, False, count)
            'If gv_Filldata.Rows.Count > 1 Then
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0

            For Each GVRow In gv_Filldata.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then




                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                    txtRoomtype = GVRow.FindControl("txtrmtypcode")
                    txtMealplan = GVRow.FindControl("txtmealcode")
                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                    txtadultrate = GVRow.FindControl("txtAdultRate")
                    chkactive = GVRow.FindControl("chkActive")
                    txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                    txtdetailremarks = GVRow.FindControl("txtdetailremarks")
                    lblMRowId = GVRow.FindControl("lblMRowId")



                    dpFDate.Text = fDate(n)
                    dpTDate.Text = tDate(n)
                    txtSplEventscode.Text = SplEventscode(n)
                    txtSplEventsname.Text = SplEventsname(n)
                    txtRoomtype.Text = Roomtype(n)
                    txtMealplan.Text = Mealplan(n)
                    txtrmcatcode.Text = roomocc(n)
                    txtdetailremarks.Text = detailremarks(n)

                    txtadultrate.Text = adultrate(n)
                    lblMRowId.Text = rowid(n)

                    'changed by mohamed on 21/02/2018
                    Dim txtTV1 As TextBox = Nothing
                    Dim txtNTV1 As TextBox = Nothing
                    Dim txtVAT1 As TextBox = Nothing
                    txtTV1 = GVRow.FindControl("txtTV")
                    txtNTV1 = GVRow.FindControl("txtNTV")
                    txtVAT1 = GVRow.FindControl("txtVAT")

                    If txtadultrate IsNot Nothing Then
                        Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
                    End If

                    n = n + 1
                End If
            Next
            ' Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click

    End Sub

    Protected Sub btnfillremarks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillremarks.Click
        Dim mealcode As String = ""


        For Each gvrow In gv_Filldata.Rows
            Dim txtdetailremarks As TextBox = gvrow.FindControl("txtdetailremarks")
            Dim txtSplEventsname As TextBox = gvrow.FindControl("txtSplEventsname")
            If txtSplEventsname.Text <> "" Then
                txtdetailremarks.Text = txtremarks.Text
            End If
        Next
    End Sub
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        Dim lnCnt As Integer = 0

        Dim txtchild As TextBox

        Dim ToDt As Date = Nothing
        Dim flg As Boolean = False


        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtSplEventsname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtrmcatcode As TextBox

        Dim txtadultrate As TextBox
        Dim chkActive As CheckBox
        Dim chkSelect As CheckBox
        Dim txtSplEventscode As TextBox


        Dim gvchildage As GridView
        Dim GVchildRow As GridViewRow
        Dim txtchildfrom As TextBox
        Dim txtchildto As TextBox
        Dim txtchildrate As TextBox
        Dim lblMRowId As Label
        Dim lblRowId As Label
        Dim lblCRowId As Label



        '*** Danny 20/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        If CheckBox1MarkupRequired.Checked = True Then
            If txtMarkupAdultOp.Text = String.Empty Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Markup Adult Operator' );", True)
                ValidateSave = False
                SetFocus(txtMarkupAdultOp)
                Exit Function
            End If
            If txtMarkupChildOp.Text = String.Empty Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Markup Child Operator' );", True)
                ValidateSave = False
                SetFocus(txtMarkupChildOp)
                Exit Function
            End If
            If txtMarkupAdultVal.Text = String.Empty Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Markup Adult Value' );", True)
                ValidateSave = False
                SetFocus(txtMarkupAdultVal)
                Exit Function
            End If
            If txtMarkupChildVal.Text = String.Empty Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Markup Child Value' );", True)
                ValidateSave = False
                SetFocus(txtMarkupChildVal)
                Exit Function
            End If
        Else
            txtMarkupAdultOp.Text = String.Empty
            txtMarkupAdultVal.Text = "0"

            txtMarkupChildOp.Text = String.Empty
            txtMarkupChildVal.Text = "0"
        End If

        '*** Danny 20/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Applicable To ');", True)
            ValidateSave = False
            Exit Function
        End If


        For Each gvRow In gv_Filldata.Rows
            lnCnt += 1


            dpFDate = gvRow.FindControl("txtfromDate")
            dpTDate = gvRow.FindControl("txtToDate")
            txtSplEventsname = gvRow.FindControl("txtSplEventsname")
            txtRoomtype = gvRow.FindControl("txtrmtypcode")
            txtMealplan = gvRow.FindControl("txtmealcode")
            txtrmcatcode = gvRow.FindControl("txtrmcatcode")
            txtadultrate = gvRow.FindControl("txtadultrate")
            chkActive = gvRow.FindControl("chkActive")
            chkSelect = gvRow.FindControl("chkSelect")
            txtSplEventscode = gvRow.FindControl("txtSplEventscode")
            lblRowId = gvRow.FindControl("lblRowId")

            lblMRowId = gvRow.FindControl("lblMRowId")

            gvchildage = gvRow.FindControl("Gv_childage")

            'changed by mohamed on 21/02/2018
            Dim txtTV1 As TextBox = Nothing
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing
            txtTV1 = gvRow.FindControl("txtTV")
            txtNTV1 = gvRow.FindControl("txtNTV")
            txtVAT1 = gvRow.FindControl("txtVAT")

            If txtadultrate IsNot Nothing Then
                Call assignVatValueToTextBox(txtadultrate, txtTV1, txtNTV1, txtVAT1)
            End If

            If txtSplEventsname.Text <> "" Then
                flg = True
            End If


            If txtSplEventsname.Text <> "" Then


                If txtRoomtype.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Room Type Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If
                If txtMealplan.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan  Row No:" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If
                If txtrmcatcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Room Category  Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If Val(txtadultrate.Text) < 1 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Adult Price Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If dpFDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(dpFDate)
                    Exit Function
                End If


                If dpFDate.Text <> "" Then

                    'If Format(CType(dpTDate.Text, Date), "yyyy/MM/dd") < Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Date.Row No :" & lnCnt & "');", True)
                    '    SetFocus(dpFDate)
                    '    ValidateSave = False
                    '    Exit Function
                    'End If

                    If Session("Calledfrom") = "Offers" Then


                        If Not (Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            SetFocus(dpFDate)
                            ValidateSave = False
                            Exit Function

                        End If


                        If Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            SetFocus(dpTDate)
                            ValidateSave = False
                            Exit Function
                        End If

                    Else
                        If Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period.Row No :" & lnCnt & "');", True)
                            SetFocus(dpFDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If (Format(CType(dpFDate.Text, Date), "yyyy/MM/dd") < ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value)) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period.Row No :" & lnCnt & "');", True)
                            SetFocus(dpTDate)
                            ValidateSave = False
                            Exit Function
                        End If
                    End If



                End If

                    For Each GVchildRow In gvchildage.Rows
                        txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                        txtchildto = GVchildRow.FindControl("txtchildageto")
                        txtchildrate = GVchildRow.FindControl("txtchildrate")
                        lblCRowId = GVchildRow.FindControl("lblCRowId")

                        If txtchildfrom.Text = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Please Enter Child Age From .Row No :" & lnCnt & "');", True)
                            SetFocus(txtchildfrom)
                            ValidateSave = False
                            Exit Function
                        End If

                        If txtchildto.Text = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Please Enter Child Age To .Row No :" & lnCnt & "');", True)
                            SetFocus(txtchildto)
                            ValidateSave = False
                            Exit Function
                        End If

                        If txtchildrate.Text = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Child Rate   can not be blank. .Row No :" & lnCnt & "');", True)
                            SetFocus(txtchildrate)
                            ValidateSave = False
                            Exit Function
                        End If

                        'changed by mohamed on 21/02/2018
                        Dim txtTVCh2 As TextBox = Nothing
                        Dim txtNTVCh2 As TextBox = Nothing
                        Dim txtVATCh2 As TextBox = Nothing
                        txtTVCh2 = GVchildRow.FindControl("txtTV")
                        txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                        txtVATCh2 = GVchildRow.FindControl("txtVAT")

                        If txtchildrate IsNot Nothing Then
                            Call assignVatValueToTextBox(txtchildrate, txtTVCh2, txtNTVCh2, txtVATCh2)
                        End If

                    Next

                End If
        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one  Special Events :" & lnCnt & "');", True)
            ValidateSave = False
            Exit Function
        End If

        ''''''''''' child age Overlapping

        Dim dtdatesnew As New DataTable
        Dim dsdates As New DataSet
        Dim dr As DataRow
        Dim xmldates As String = ""

        dtdatesnew.Columns.Add(New DataColumn("specialeventcode", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("specialeventname", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("Rmtypcode", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("mealcode", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("rmcatcode", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("fromage", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("toage", GetType(String)))

        For Each gvRow In gv_Filldata.Rows
            chkSelect = gvRow.FindControl("chkSelect")
            txtSplEventscode = gvRow.FindControl("txtSplEventscode")
            txtSplEventsname = gvRow.FindControl("txtSplEventsname")
            lblRowId = gvRow.FindControl("lblRowId")
            txtRoomtype = gvRow.FindControl("txtrmtypcode")
            txtMealplan = gvRow.FindControl("txtmealcode")
            txtrmcatcode = gvRow.FindControl("txtrmcatcode")

            lblMRowId = gvRow.FindControl("lblMRowId")

            gvchildage = gvRow.FindControl("Gv_childage")

            For Each GVchildRow In gvchildage.Rows
                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                txtchildto = GVchildRow.FindControl("txtchildageto")
                txtchildrate = GVchildRow.FindControl("txtchildrate")
                lblCRowId = GVchildRow.FindControl("lblCRowId")



                If txtchildfrom.Text <> "" And txtchildto.Text <> "" Then

                    dr = dtdatesnew.NewRow
                    dr("specialeventcode") = CType(txtSplEventscode.Text, String)
                    dr("specialeventname") = CType(txtSplEventsname.Text, String)
                    dr("Rmtypcode") = CType(txtRoomtype.Text, String)
                    dr("mealcode") = CType(txtMealplan.Text, String)
                    dr("rmcatcode") = CType(txtrmcatcode.Text, String)
                    dr("fromage") = CType(txtchildfrom.Text, String)
                    dr("toage") = CType(txtchildto.Text, String)
                    dtdatesnew.Rows.Add(dr)


                End If
            Next
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
        Dim parm(2) As SqlParameter

        parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
        parm(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))


        parms.Add(parm(0))
        parms.Add(parm(1))

        'For i = 0 To 2
        '    parms.Add(parm(i))
        'Next

        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapchildages", parms)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("spleventname")) = False Then
                    strMsg = "Special Events  Are Child Ages Overlapping Please check " + "\n"
                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        strMsg += "  Special Events -  " + ds.Tables(0).Rows(i)("spleventname") + "\n"
                    Next

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    ValidateSave = False
                    Exit Function
                End If
            End If
        End If



        


        'For Each dtrow As DataRow In dtdatesnew.Rows
        '    Dim dtSearchedRows As DataRow() = dtdatesnew.Select("fromage='" & dtrow("fromage") & "' and toage='" & dtrow("toage") & "'")
        '    If dtSearchedRows.Length > 0 Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Child Ages Are Overlapping Please check ');", True)
        '        ValidateSave = False
        '        Exit Function
        '    End If
        'Next


        ValidateSave = True
    End Function
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try


            '   CopyRow = 0

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            For Each GVRow In gv_Filldata.Rows

                Dim dpFDate As TextBox = GVRow.FindControl("txtfromDate")
                Dim dpTDate As TextBox = GVRow.FindControl("txtToDate")
                Dim txtSplEventsname As TextBox = GVRow.FindControl("txtSplEventsname")
                Dim txtRoomtype As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtMealplan As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                Dim txtadultrate As TextBox = GVRow.FindControl("txtadultrate")
                Dim chkActive As CheckBox = GVRow.FindControl("chkActive")

                Dim txtSplEventscode As TextBox = GVRow.FindControl("txtSplEventscode")
                Dim lblRowId As Label = GVRow.FindControl("lblRowId")

                Dim lblMRowId As Label = GVRow.FindControl("lblMRowId")

                'gvchildage = GVRow.FindControl("Gv_childage")


                Dim ds As DataSet
                Dim parms2 As New List(Of SqlParameter)
                Dim parm2(12) As SqlParameter


                If txtSplEventsname.Text <> "" Then
                    parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                    parm2(1) = New SqlParameter("@fromdate", Format(CType(dpFDate.Text, Date), "yyyy/MM/dd"))
                    parm2(2) = New SqlParameter("@todate", Format(CType(dpFDate.Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@mode", CType(ViewState("State"), String))
                    parm2(4) = New SqlParameter("@tranid", CType(txttranid.Text.Trim, String))
                    parm2(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(7) = New SqlParameter("@promotionid", "")
                    parm2(8) = New SqlParameter("@rmtypcode", CType(txtRoomtype.Text, String))
                    parm2(9) = New SqlParameter("@mealcode", CType(txtMealplan.Text, String))
                    parm2(10) = New SqlParameter("@rmcatcode", CType(txtrmcatcode.Text, String))
                    parm2(11) = New SqlParameter("@spleventcode", CType(txtSplEventscode.Text, String))



                    For i = 0 To 11
                        parms2.Add(parm2(i))
                    Next

                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_duplicate_splevents", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("splistcode")) = False Then
                                strMsg = "Contract Special Events already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("splistcode") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
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
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Function checknewevents() As String

        checknewevents = ""
        Dim strmsg1 As String = ""

        For Each gvrow In gv_Filldata.Rows
            Dim txtSplEventsname As TextBox = gvrow.FindControl("txtSplEventsname")
            Dim txtSplEventscode As TextBox = gvrow.FindControl("txtSplEventscode")

            If txtSplEventsname.Text <> "" And txtSplEventscode.Text = "" Then
                strmsg1 = txtSplEventsname.Text + "  You have Entered  New Special Events Are sure want to continue save ?"
                checknewevents = strmsg1
            End If

        Next





        '''''''''''''

    End Function
    Protected Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ViewState("chksplevents") = Nothing

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strMsg As String = ""
            Dim lsChildVATDetail As String, liChildVATDetailSlNo As Integer

            If Page.IsValid = True Then

                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then

                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                    If checkforexisting() = False Then
                        Exit Sub
                    End If
                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    If ViewState("chksplevents") <> 1 Then
                        Dim strmsg4 As String = checknewevents()
                        If strmsg4 <> "" Then

                            ViewState("chksplevents") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg4 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub

                        End If
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    ' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_splevents", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    ' '''''''''''''''''''''''

                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("SPLIST", mySqlConn, sqlTrans)
                        txttranid.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_specialevents_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = IIf(Session("Calledfrom") = "Offers", DBNull.Value, CType(hdncontractid.Value, String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 5000)).Value = CType(txtremarks.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)

                        '*** Danny 20/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                        '*** Danny 20/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_specialevents_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0

                        mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = IIf(Session("Calledfrom") = "Offers", DBNull.Value, CType(hdncontractid.Value, String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 5000)).Value = CType(txtremarks.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)


                        '*** Danny 20/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                        '*** Danny 20/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()





                    End If

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_Events Where  EventList_ID='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_EventRates Where  eventid='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_specialevents_detail Where  splistcode='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    ' '''''''
                    Dim sqlstr As String = ""
                    Dim dpFDate As TextBox
                    Dim dpTDate As TextBox
                    Dim txtSplEventsname As TextBox
                    Dim txtRoomtype As TextBox
                    Dim txtMealplan As TextBox
                    Dim txtrmcatcode As TextBox

                    Dim txtadultrate As TextBox
                    Dim chkActive As CheckBox
                    Dim chkSelect As CheckBox
                    Dim txtSplEventscode As TextBox


                    Dim gvchildage As GridView
                    Dim GVchildRow As GridViewRow
                    Dim txtchildfrom As TextBox
                    Dim txtchildto As TextBox
                    Dim txtchildrate As TextBox
                    Dim lblMRowId As Label
                    Dim lblRowId As Label
                    Dim lblCRowId As Label
                    Dim childratesstring As String = ""
                    Dim arrOcpncy As String()
                    Dim Mealplan As String()
                    Dim roomcategory As String()

                    Dim ix As Integer
                    Dim ds1 As DataSet

                    Dim spleventcode As String = ""

                    Dim i As Integer = 1
                    Dim rates As String = ""

                    Dim txtTV1 As TextBox = Nothing
                    Dim txtNTV1 As TextBox = Nothing
                    Dim txtVAT1 As TextBox = Nothing

                    Dim txtTVCh2 As TextBox = Nothing
                    Dim txtNTVCh2 As TextBox = Nothing
                    Dim txtVATCh2 As TextBox = Nothing

                    Dim txtdetailremarks As TextBox

                    Dim k As Integer = 1
                    Dim oldeventcode As String = ""
                    Dim oldrmtype As String = ""
                    Dim oldmeal As String = ""

                    For Each GVRow In gv_Filldata.Rows

                        dpFDate = GVRow.FindControl("txtfromDate")
                        dpTDate = GVRow.FindControl("txtToDate")
                        txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                        txtRoomtype = GVRow.FindControl("txtrmtypcode")
                        txtMealplan = GVRow.FindControl("txtmealcode")
                        txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                        txtadultrate = GVRow.FindControl("txtadultrate")
                        chkActive = GVRow.FindControl("chkActive")
                        chkSelect = GVRow.FindControl("chkSelect")
                        txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                        lblRowId = GVRow.FindControl("lblRowId")
                        txtdetailremarks = GVRow.findcontrol("txtdetailremarks")
                        lblMRowId = GVRow.FindControl("lblMRowId")

                    

                        txtTV1 = GVRow.FindControl("txtTV")
                        txtNTV1 = GVRow.FindControl("txtNTV")
                        txtVAT1 = GVRow.FindControl("txtVAT")

                        gvchildage = GVRow.FindControl("Gv_childage")
                        childratesstring = ""
                        If txtSplEventsname.Text <> "" Then
                            liChildVATDetailSlNo = 0 'changed by mohamed on 25/02/2018
                            lsChildVATDetail = ""

                            For Each GVchildRow In gvchildage.Rows

                                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                                txtchildto = GVchildRow.FindControl("txtchildageto")
                                txtchildrate = GVchildRow.FindControl("txtchildrate")
                                lblCRowId = GVchildRow.FindControl("lblCRowId")

                                rates = txtchildrate.Text
                                Select Case txtchildrate.Text
                                    Case "Free"
                                        rates = "-3"
                                    Case "Incl"
                                        rates = "-1"
                                    Case "N.Incl"
                                        rates = "-2"
                                    Case "N/A"
                                        rates = "-4"
                                    Case "On Request"
                                        rates = "-5"

                                End Select

                              
                                txtTVCh2 = GVchildRow.FindControl("txtTV")
                                txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                                txtVATCh2 = GVchildRow.FindControl("txtVAT")

                                childratesstring = childratesstring + ";" + CType(txtchildfrom.Text, String) + "," + CType(txtchildto.Text, String) + "," + CType(CType(rates, Decimal), String)

                                liChildVATDetailSlNo += 1
                                lsChildVATDetail += IIf(lsChildVATDetail = "", "", ";") + CType(liChildVATDetailSlNo, String) + "," + CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTVCh2.Text)), Decimal), String)
                                lsChildVATDetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTVCh2.Text)), Decimal), String)
                                lsChildVATDetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVATCh2.Text)), Decimal), String)
                            Next

                            If childratesstring.Length > 0 Then
                                childratesstring = Right(childratesstring, Len(childratesstring) - 1)
                            Else
                                childratesstring = ""
                            End If

                            If Session("CountryList") <> "" Then

                                If oldrmtype <> txtRoomtype.Text And oldrmtype <> "" And oldmeal <> txtMealplan.Text And oldmeal <> "" Then

                                    If oldeventcode <> CType(txtSplEventscode.Text.Trim, String) And oldeventcode <> "" Then
                                        k += 1
                                    Else
                                        k += 1
                                    End If

                                End If


                              


                                ''Value in hdn variable , so splting to get string correctly
                                Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                                For m = 0 To arrcountry.Length - 1

                                    If arrcountry(m) <> "" Then

                                        ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                                        If ds1.Tables(0).Rows.Count > 0 Then

                                            For ix = 0 To ds1.Tables(0).Rows.Count - 1


                                                mySqlCmd = New SqlCommand("sp_mod_edit_contracts_specialevents_detail", mySqlConn, sqlTrans)
                                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                                mySqlCmd.CommandTimeout = 0


                                                mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@splineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) ' i
                                                mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(txtSplEventscode.Text.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtRoomtype.Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtMealplan.Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 8000)).Value = CType(txtrmcatcode.Text, String)
                                                'mySqlCmd.Parameters.Add(New SqlParameter("@adultrate", SqlDbType.Decimal)).Value = CType(Val(txtadultrate.Text), Decimal)
                                                'mySqlCmd.Parameters.Add(New SqlParameter("@childdetails", SqlDbType.VarChar, 8000)).Value = CType(childratesstring, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = CType(IIf(chkActive.Checked = True, 1, 0), Integer)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@Detailremarks", SqlDbType.VarChar, 8000)).Value = CType(txtdetailremarks.Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtSplEventsname.Text.Trim.ToUpper, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = ""
                                                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                                mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = CType(arrcountry(m), String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                                                mySqlCmd.Parameters.Add(New SqlParameter("@spolineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) ' ix + 1

                                                mySqlCmd.ExecuteNonQuery()
                                                mySqlCmd.Dispose() 'command disposed
                                            Next



                                        Else

                                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_specialevents_detail", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure
                                            mySqlCmd.CommandTimeout = 0

                                            mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@splineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) ' k
                                            mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(txtSplEventscode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtRoomtype.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtMealplan.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 8000)).Value = CType(txtrmcatcode.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = CType(IIf(chkActive.Checked = True, 1, 0), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Detailremarks", SqlDbType.VarChar, 8000)).Value = CType(txtdetailremarks.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtSplEventsname.Text.Trim.ToUpper, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = ""
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = CType(arrcountry(m), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@spolineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) 'k

                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                            oldeventcode = CType(txtSplEventscode.Text.Trim, String)
                                            oldmeal = txtMealplan.Text
                                            oldrmtype = txtRoomtype.Text
                                        End If




                                    End If  '' Country

                                Next           '' Country
                            End If   '' Country
                        End If
                    Next

                    k = 1
                    oldeventcode = ""
                    Dim x As Integer = 1
                    For Each GVRow In gv_Filldata.Rows

                        dpFDate = GVRow.FindControl("txtfromDate")
                        dpTDate = GVRow.FindControl("txtToDate")
                        txtSplEventsname = GVRow.FindControl("txtSplEventsname")
                        txtRoomtype = GVRow.FindControl("txtrmtypcode")
                        txtMealplan = GVRow.FindControl("txtmealcode")
                        txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                        txtadultrate = GVRow.FindControl("txtadultrate")
                        chkActive = GVRow.FindControl("chkActive")
                        chkSelect = GVRow.FindControl("chkSelect")
                        txtSplEventscode = GVRow.FindControl("txtSplEventscode")
                        lblRowId = GVRow.FindControl("lblRowId")
                        txtdetailremarks = GVRow.findcontrol("txtdetailremarks")
                        lblMRowId = GVRow.FindControl("lblMRowId")


                        txtTV1 = GVRow.FindControl("txtTV")
                        txtNTV1 = GVRow.FindControl("txtNTV")
                        txtVAT1 = GVRow.FindControl("txtVAT")

                        gvchildage = GVRow.FindControl("Gv_childage")
                        childratesstring = ""
                        If txtSplEventsname.Text <> "" Then
                            liChildVATDetailSlNo = 0 'changed by mohamed on 25/02/2018
                            lsChildVATDetail = ""

                            For Each GVchildRow In gvchildage.Rows

                                txtchildfrom = GVchildRow.FindControl("txtchildagefrm")
                                txtchildto = GVchildRow.FindControl("txtchildageto")
                                txtchildrate = GVchildRow.FindControl("txtchildrate")
                                lblCRowId = GVchildRow.FindControl("lblCRowId")

                                rates = txtchildrate.Text
                                Select Case txtchildrate.Text
                                    Case "Free"
                                        rates = "-3"
                                    Case "Incl"
                                        rates = "-1"
                                    Case "N.Incl"
                                        rates = "-2"
                                    Case "N/A"
                                        rates = "-4"
                                    Case "On Request"
                                        rates = "-5"

                                End Select



                                txtTVCh2 = GVchildRow.FindControl("txtTV")
                                txtNTVCh2 = GVchildRow.FindControl("txtNTV")
                                txtVATCh2 = GVchildRow.FindControl("txtVAT")

                                childratesstring = childratesstring + ";" + CType(txtchildfrom.Text, String) + "," + CType(txtchildto.Text, String) + "," + CType(CType(rates, Decimal), String)

                                liChildVATDetailSlNo += 1
                                lsChildVATDetail += IIf(lsChildVATDetail = "", "", ";") + CType(liChildVATDetailSlNo, String) + "," + CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTVCh2.Text)), Decimal), String)
                                lsChildVATDetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTVCh2.Text)), Decimal), String)
                                lsChildVATDetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVATCh2.Text)), Decimal), String)
                            Next

                            If childratesstring.Length > 0 Then
                                childratesstring = Right(childratesstring, Len(childratesstring) - 1)
                            Else
                                childratesstring = ""
                            End If

                            If CType(txtRoomtype.Text, String) = "All" Then
                                Dim rmtypcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + rmtypcode from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                                arrOcpncy = rmtypcode.ToString.Trim.Split(",")
                            Else
                                arrOcpncy = txtRoomtype.Text.ToString.Trim.Split(",")
                            End If
                            spleventcode = txtSplEventscode.Text.Trim
                            If spleventcode = "" Then
                                spleventcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select event_code from  New_edit_Events(nolock) where    line_no=" & i & " and EventList_ID='" & CType(txttranid.Text.Trim, String) & "'")
                            End If


                            If oldrmtype <> txtRoomtype.Text And oldrmtype <> "" And oldmeal <> txtMealplan.Text And oldmeal <> "" Then

                                If oldeventcode <> CType(spleventcode, String) And oldeventcode <> "" Then
                                    x += 1
                                Else
                                    x += 1
                                End If

                            End If

                            'If oldeventcode <> CType(spleventcode, String) And oldeventcode <> "" And oldrmtype = txtRoomtype.Text And oldrmtype <> "" And oldmeal = txtMealplan.Text And oldmeal <> "" Then
                            '    x += 1
                            'End If



                            ' arrOcpncy = txtRoomtype.Text.ToString.Trim.Split(",")
                            For i = 0 To arrOcpncy.Length - 1

                                If arrOcpncy(i) <> "" Then

                                    '     Dim mealcode As String() = txtMealplan.Text.ToString.Trim.Split(",")


                                    If CType(txtMealplan.Text, String) = "All" Then
                                        Dim mealcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + mealcode from  partymeal(nolock) where   partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                                        Mealplan = mealcode1.ToString.Trim.Split(",")
                                    Else
                                        Mealplan = txtMealplan.Text.ToString.Trim.Split(",")
                                    End If

                                    For j = 0 To Mealplan.Length - 1

                                        If Mealplan(j) <> "" Then

                                            If CType(txtrmcatcode.Text, String) = "All" Then
                                                Dim rmcatcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_partymaxaccomodation  u(nolock) where u.rmtypcode='" & CType(arrOcpncy(i), String) & "' and  u.partycode ='" & CType(hdnpartycode.Value.Trim, String) & "'  for xml path('')),1,1,'' ) ")

                                                roomcategory = rmcatcode1.ToString.Trim.Split(",")
                                            Else
                                                roomcategory = txtrmcatcode.Text.ToString.Trim.Split(",")
                                            End If

                                            For k = 0 To roomcategory.Length - 1

                                                If roomcategory(k) <> "" Then

                                                    mySqlCmd = New SqlCommand("sp_add_new_edit_sp_eventrates", mySqlConn, sqlTrans)
                                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                                    mySqlCmd.CommandTimeout = 0

                                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@splistcode ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@splineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) ' x ' i + 1
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = IIf(CType(txtSplEventscode.Text.Trim, String) = "", spleventcode, CType(txtSplEventscode.Text.Trim, String))
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 100)).Value = CType(arrOcpncy(i), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(roomcategory(k), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(Mealplan(j), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@adultrate", SqlDbType.Decimal)).Value = CType(Val(txtadultrate.Text), Decimal)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@childdetails", SqlDbType.VarChar, 8000)).Value = CType(childratesstring, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildTaxable", SqlDbType.VarChar, 8000)).Value = lsChildVATDetail
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtdetailremarks.Text, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@prlineno", SqlDbType.Int)).Value = CType(Val(lblMRowId.Text), Integer) ' x ' ix
                                                    'mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtSplEventsname.Text.Trim.ToUpper, String)

                                                    mySqlCmd.ExecuteNonQuery()
                                                    mySqlCmd.Dispose() 'command disposed
                                                    oldeventcode = CType(txtSplEventscode.Text.Trim, String)
                                                    oldmeal = txtMealplan.Text
                                                    oldrmtype = txtRoomtype.Text

                                                End If

                                            Next

                                        End If

                                    Next

                                End If


                            Next  '''



                        End If
                    Next

                    If Session("AgentList") <> "" Then
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For kl = 0 To arragents.Length - 1

                            If arragents(kl) <> "" Then

                                ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                                If ds1.Tables(0).Rows.Count > 0 Then

                                    For ix = 0 To ds1.Tables(0).Rows.Count - 1


                                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_specialevents_detail", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure
                                        mySqlCmd.CommandTimeout = 0


                                        mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@splineno", SqlDbType.Int)).Value = i
                                        mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(txtSplEventscode.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtRoomtype.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtMealplan.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 8000)).Value = CType(txtrmcatcode.Text, String)
                                        'mySqlCmd.Parameters.Add(New SqlParameter("@adultrate", SqlDbType.Decimal)).Value = CType(Val(txtadultrate.Text), Decimal)
                                        'mySqlCmd.Parameters.Add(New SqlParameter("@childdetails", SqlDbType.VarChar, 8000)).Value = CType(childratesstring, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = CType(IIf(chkActive.Checked = True, 1, 0), Integer)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@Detailremarks", SqlDbType.VarChar, 8000)).Value = CType(txtdetailremarks.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtSplEventsname.Text.Trim.ToUpper, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(kl), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(kl), String) & "'")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = DBNull.Value
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@spolineno", SqlDbType.Int)).Value = ix + 1

                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose() 'command disposed
                                    Next
                                Else

                                    mySqlCmd = New SqlCommand("sp_mod_edit_contracts_specialevents_detail", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.CommandTimeout = 0

                                    mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@splineno", SqlDbType.Int)).Value = i
                                    mySqlCmd.Parameters.Add(New SqlParameter("@spleventcode", SqlDbType.VarChar, 20)).Value = CType(txtSplEventscode.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtRoomtype.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtMealplan.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomcategory", SqlDbType.VarChar, 8000)).Value = CType(txtrmcatcode.Text, String)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@adultrate", SqlDbType.Decimal)).Value = CType(Val(txtadultrate.Text), Decimal)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@childdetails", SqlDbType.VarChar, 8000)).Value = CType(childratesstring, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = CType(IIf(chkActive.Checked = True, 1, 0), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Detailremarks", SqlDbType.VarChar, 8000)).Value = CType(txtdetailremarks.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@spleventname", SqlDbType.VarChar, 200)).Value = CType(txtSplEventsname.Text.Trim.ToUpper, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(kl), String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(kl), String) & "'")
                                    mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = DBNull.Value
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupAdultOp.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MarkupAdultVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupAdultVal.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildOp", SqlDbType.VarChar, 10)).Value = CType(txtMarkupChildOp.Text, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MarkupChildVal", SqlDbType.Decimal, 10, 3)).Value = Val(txtMarkupChildVal.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@compulsory", SqlDbType.Int)).Value = CType(ddlcompoption.SelectedIndex, Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@spolineno", SqlDbType.Int)).Value = 1

                                    mySqlCmd.ExecuteNonQuery()
                                    mySqlCmd.Dispose() 'command disposed

                                End If

                            End If
                        Next
                    End If



                    '    i = i + 1

                    'Next




                    '''  User cotrol country saving
                    ''' 

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_specialevents_countries Where splistcode='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_specialevents_agents Where splistcode='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    '    For i = 0 To arrcountry.Length - 1

                    '        If arrcountry(i) <> "" Then

                    '            mySqlCmd = New SqlCommand("sp_add_contracts_specialevents_countries", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    'If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then

                    '            mySqlCmd = New SqlCommand("sp_add_contracts_specialevents_agents", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    strMsg = "Saved Succesfully!!"

                ElseIf ViewState("State") = "Delete" Then

                    '------------------------for delete contracts_special events
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_splevents", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    '''''''''''''''''''''''


                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_contracts_specialevents_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@splistcode", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Succesfully!!"
                End If


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                ViewState("State") = ""
                btnreset1_Click(sender, e)
                FillGrid(" h.splistcode", hdncontractid.Value, "Desc")

                ModalPopupDays.Hide()  '' Added shahul 08/08/18

            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ").Replace(vbCr, " ").Replace(vbLf, " ") & "' );", True)
            objUtils.WritErrorLog("ContractSplEventPlist.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Function Limit(ByVal desc As Object, ByVal maxlen As Integer) As String

        If desc.ToString = "" Then
            Return ""
        Else
            If desc.ToString.Length > maxlen Then
                desc = desc.Substring(0, maxlen)
            Else

                desc = desc
            End If
        End If

        Return desc


    End Function
    Function SetVisibility(ByVal desc As Object, ByVal maxlen As Integer) As Boolean

        If desc.ToString = "" Then
            Return False
        Else
            If desc.ToString.Length > maxlen Then
                Return True
            Else
                Return False
            End If
        End If


    End Function
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try
            'strSqlQry = "select h.contractid,h.childpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            '     " h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek   from view_contracts_childpolicy_header h(nolock),view_contracts_search d(nolock)  where h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"


            strSqlQry = "select h.contractid as contractid,h.splistcode plistcode,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
   & "   from view_contracts_specialevents_header h(nolock),view_contracts_specialevents_detail d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.splistcode=d.splistcode and     h.contractid<>'" & hdncontractid.Value & "'  group by h.contractid, h.splistcode,h.applicableto  "




            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myds)
            grdviewrates.DataSource = myds

            If myds.Tables(0).Rows.Count > 0 Then
                grdviewrates.DataBind()
            Else
                grdviewrates.PageIndex = 0
                grdviewrates.DataBind()

            End If


            ModalViewrates.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Private Sub sortgvsearch()
        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("h.splistcode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("fromdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 7
                FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select
    End Sub
    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub

    'changed by mohamed on 21/02/2018
    Function fnCalculateVATValue() As Boolean
        For Each grdRow As GridViewRow In gv_Filldata.Rows
            If (grdRow.RowType = DataControlRowType.DataRow) Then
                Dim txt1 As TextBox = grdRow.FindControl("txtAdultRate")
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = grdRow.FindControl("txtTV")
                txtNTV1 = grdRow.FindControl("txtNTV")
                txtVAT1 = grdRow.FindControl("txtVAT")

                If txt1 IsNot Nothing Then
                    Call assignVatValueToTextBox(txt1, txtTV1, txtNTV1, txtVAT1)
                End If

                Dim Gv_childage As GridView
                Gv_childage = grdRow.FindControl("Gv_childage")
                For Each grdChildRow As GridViewRow In Gv_childage.Rows
                    If (grdChildRow.RowType = DataControlRowType.DataRow) Then
                        Dim txtCh2 As TextBox = grdChildRow.FindControl("txtchildrate")
                        Dim txtTVCh2 As TextBox = Nothing
                        Dim txtNTVCh2 As TextBox = Nothing
                        Dim txtVATCh2 As TextBox = Nothing
                        txtTVCh2 = grdChildRow.FindControl("txtTV")
                        txtNTVCh2 = grdChildRow.FindControl("txtNTV")
                        txtVATCh2 = grdChildRow.FindControl("txtVAT")

                        If txtCh2 IsNot Nothing Then
                            Call assignVatValueToTextBox(txtCh2, txtTVCh2, txtNTVCh2, txtVATCh2)
                        End If
                    End If
                Next
            End If
        Next
                Return True
    End Function

    'changed by mohamed on 21/02/2018
    Sub assignVatValueToTextBox(ByRef txt1 As TextBox, ByRef txtTV1 As TextBox, ByRef txtNTV1 As TextBox, ByRef txtVAT1 As TextBox)
        Dim clsServ As New clsServices
        Dim lRetValue As clsMaster()
        Dim lsSqlQry As String = ""

        If txt1.Text.Trim = "" Or chkVATCalculationRequired.Checked = False Then
            txtTV1.Text = IIf(txt1.Text.Trim = "", "", Val(txt1.Text))
            txtNTV1.Text = ""
            txtVAT1.Text = ""
        Else
            lsSqlQry = "execute sp_calculate_taxablevalue_onlycost " & Val(txt1.Text) & ","
            lsSqlQry += Val(txtServiceCharges.Text) & "," & Val(TxtMunicipalityFees.Text) & ","
            lsSqlQry += Val(txtVAT.Text) & "," & Val(txtTourismFees.Text)
            lRetValue = clsServ.getCommonArrayOfCodeAndNameFromSqlQuery(Session("dbConnectionName"), lsSqlQry)
            For li As Integer = lRetValue.GetLowerBound(0) To lRetValue.GetUpperBound(0)
                If lRetValue(li).ListText = "taxablevalue" Then
                    txtTV1.Text = Val(lRetValue(li).ListValue)
                End If
                If lRetValue(li).ListText = "nontaxablevalue" Then
                    txtNTV1.Text = Val(lRetValue(li).ListValue)
                End If
                If lRetValue(li).ListText = "vatvalue" Then
                    txtVAT1.Text = Val(lRetValue(li).ListValue)
                End If
            Next
        End If
    End Sub

    'changed by mohamed on 21/02/2018
    Protected Sub VATTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Call fnCalculateVATValue()
    End Sub

    'changed by mohamed on 21/02/2018
    Protected Sub chkVATCalculationRequired_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVATCalculationRequired.CheckedChanged
        If chkVATCalculationRequired.Checked = True Then
            If Val(txtServiceCharges.Text) + Val(TxtMunicipalityFees.Text) + Val(txtTourismFees.Text) + Val(txtVAT.Text) = 0 Then
                sbFillTaxDetail()
            End If
        Else
            txtServiceCharges.Text = "0"
            TxtMunicipalityFees.Text = "0"
            txtTourismFees.Text = "0"
            txtVAT.Text = "0"
        End If
        Call fnCalculateVATValue()
    End Sub

    'changed by mohamed on 21/02/2018
    Sub sbFillTaxDetail()
        'chkVATCalculationRequired.Checked = False
        txtServiceCharges.Text = ""
        TxtMunicipalityFees.Text = ""
        txtTourismFees.Text = ""
        txtVAT.Text = ""

        Dim dsVatDet As DataSet
        strSqlQry = "execute sp_get_taxdetail_frommaster '" & hdnpartycode.Value & "',5101" '"select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'"
        dsVatDet = objUtils.GetDataFromDatasetnew(Session("dbConnectionName"), strSqlQry)

        If dsVatDet.Tables(0).Rows.Count > 0 Then
            With dsVatDet.Tables(0).Rows(0)
                'chkVATCalculationRequired.Checked = True
                txtServiceCharges.Text = .Item("servicechargeperc").ToString
                TxtMunicipalityFees.Text = .Item("municipalityfeeperc").ToString
                txtTourismFees.Text = .Item("tourismfeeperc").ToString
                txtVAT.Text = .Item("vatperc").ToString
            End With
        End If
    End Sub
End Class
