


Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Partial Class ExcSectorMaster
    Inherits System.Web.UI.Page

    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
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

    <System.Web.Script.Services.ScriptMethod()> _
         <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Ctrynames As New List(Of String)
        Try
            strSqlQry = "select othtypcode,othtypname from othtypmast where active=1 and othgrpcode=(select option_selected from reservation_parameters where param_id=1001) and  othtypname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Ctrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othtypname").ToString(), myDS.Tables(0).Rows(i)("othtypcode").ToString()))

                Next
            End If
            Return Ctrynames
        Catch ex As Exception
            Return Ctrynames
        End Try

    End Function
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
 

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excursiontypes Where exctypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("exctypcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("exctypcode")
                        Me.txtCustomerName.Value = mySqlReader("exctypname")
                    End If
                End If
            End If
            mySqlCmd.Dispose()
            mySqlReader.Close()
            mySqlCmd.Dispose()
            mySqlReader.Close()
            Dim count As Long
            Dim GVRow As GridViewRow
            Dim txtsecgrpcode As TextBox
            Dim txtsecgrpname As TextBox
            Dim txtcityname As TextBox
            Dim txtcitycode As TextBox
            Dim txtoldsecgrpcode As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select count(*) from exctypes_sectorgrp Where exctypcode='" & RefCode & "'", mySqlConn)
            count = mySqlCmd.ExecuteScalar
            mySqlCmd.Dispose()
            mySqlConn.Close()
            If count > 0 Then
                fillDategrd(grdsectorgrps, False, count)
                'fillgrd(gv_Email, False, count)
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from exctypes_sectorgrp Where exctypcode='" & RefCode & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                For Each GVRow In grdsectorgrps.Rows
                    If mySqlReader.Read = True Then
                        If IsDBNull(mySqlReader("exctypcode")) = False Then


                            txtsecgrpcode = GVRow.FindControl("txtsecgrpcode")
                            txtsecgrpname = GVRow.FindControl("txtsecgrpname")
                            txtcityname = GVRow.FindControl("txtcityname")
                            txtoldsecgrpcode = GVRow.FindControl("txtoldsecgrpcode")
                            txtcitycode = GVRow.FindControl("txtcitycode")
                            txtsecgrpcode.Text = CType(mySqlReader("sectorgrpcode"), String)
                            txtoldsecgrpcode.Text = CType(mySqlReader("sectorgrpcode"), String)
                            txtsecgrpname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", CType(mySqlReader("sectorgrpcode"), String))
                            txtcitycode.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "citycode", "othtypcode", CType(mySqlReader("sectorgrpcode"), String))
                            txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", txtcitycode.Text)
                        End If
                    End If


                Next
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()

                DisableSectors()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

    Protected Sub btncityname_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex
        Dim secgrpcode As String = CType(grdsectorgrps.Rows(rowid).FindControl("txtsecgrpcode"), TextBox).Text

        hdnMainGridRowid.Value = rowid
        If hdnMainGridRowid.Value <> "" Then
            Dim txtcityname As TextBox = grdsectorgrps.Rows(hdnMainGridRowid.Value).FindControl("txtcityname")
            Dim txtcitycode As TextBox = grdsectorgrps.Rows(hdnMainGridRowid.Value).FindControl("txtcitycode")
            txtcitycode.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "citycode", "othtypcode", secgrpcode)
            txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", txtcitycode.Text)
        End If
   
    End Sub

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("ExcursionModule\ExcursionSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            Panelctry.Visible = True
            fillDategrd(grdsectorgrps, True)
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            'GetValuesForResvationDetails()

            If CType(Session("ExcTypesState"), String) = "New" Then
                '   SetFocus(txtCustomerCode)
                lblmainheading.Text = "Add New Excursion - Sector Details"
                Page.Title = Page.Title + " " + " New Excursion - Sector Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "Edit" Then

                BtnSave.Text = "Update"

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblmainheading.Text = "Edit Excursion - Sector Details"
                Page.Title = Page.Title + " " + "Edit Excursion - Sector Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "View" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdsectorgrps.Enabled = False
                btnaddrow.Enabled = False
                btnHelp.Enabled = False
                btndelrow.Enabled = False
                'DisableControl()
                lblmainheading.Text = "View Excursion - Sector Details"
                Page.Title = Page.Title + " " + "View Excursion - Sector Details"
                BtnSave.Visible = False
                BtnCancel.Text = "Return to Search"
                BtnCancel.Focus()

            ElseIf CType(Session("ExcTypesState"), String) = "Delete" Then

                RefCode = CType(Session("ExcTypesRefCode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                grdsectorgrps.Enabled = False
                btndelrow.Enabled = False
                lblmainheading.Text = "Delete Excursion - Sector Details"
                Page.Title = Page.Title + " " + "Delete Excursion - Sector Details"
                BtnSave.Text = "Delete"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                'BtnResSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;")
            ElseIf CType(Session("ExcTypesState"), String) = "Addclient" Then

                Dim clientname As String
                Dim webuser As String
                'SetFocus(txtCustomerCode)
                clientname = CType(Session("clientname"), String)
                webuser = CType(Session("webusername"), String)
          
                lblmainheading.Text = "Add New Customer - Main Details"
                Page.Title = Page.Title + " " + "New Excursion - SectorGroups Details"
                BtnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")


            End If
            BtnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

        End If
        ' Session.Add("submenuuser", "CustomersSearch.aspx")
        Session.Add("submenuuser", "ExSectorMaster.aspx")
    End Sub
#End Region

    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow
        Dim cnt As Integer = 0
        Dim txtsecgrpcode As TextBox
        Dim chkpreferred As CheckBox
        Dim txtsecgrpname As TextBox
        Dim txtoldsecgrpcode As TextBox



        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""

        Try

    
        For Each GvRow In grdsectorgrps.Rows
            txtsecgrpcode = gvRow.FindControl("txtsecgrpcode")
            txtsecgrpname = gvRow.FindControl("txtsecgrpname")
            txtoldsecgrpcode = gvRow.FindControl("txtoldsecgrpcode")

            If txtoldsecgrpcode.Text <> "" And txtoldsecgrpcode.Text <> txtsecgrpcode.Text And txtsecgrpcode.Text <> "" Then

                    strSqlQry = "select distinct h.eplistcode , 'Sector PriceList' Options from excplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and  isnull(h.sectoryesno,0)=1  and   esc.mktcode='" & txtoldsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                            & " select distinct h.eplistcode,'SectorCost Price List' Options from exccplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and  isnull(h.sectoryesno,0)=1  and   esc.mktcode='" & txtoldsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "'"


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                MyAdapter.Fill(ds, "showsectors")


                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                            Dim partyname As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", CType(txtoldsecgrpcode.Text, String))
                            strMsg = "For this " + partyname + "  Sector Already Entered Cost Price list You Can not Change  Details below" + "\n"

                            For i = 0 To ds.Tables(0).Rows.Count - 1

                                strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                            Next

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                            ValidateSave = False
                            Exit Function
                        End If
                    End If
                End If
            End If



        Next


            ValidateSave = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcsectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click


        Dim txtsecgrpcode As TextBox
        Dim txtsecgrpname As TextBox
        Dim txtcitycode As TextBox
        Dim GvRow As GridViewRow
        Try
            If Page.IsValid = True Then

                If ValidateSave() = False Then
                    Exit Sub
                End If


                If Session("ExcTypesState").ToString() = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("ExcTypesState").ToString() = "Edit" Then

                    '    'check  If ValidateEmail() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'mySqlCmd = New SqlCommand("delete from agentmast_countries where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd = New SqlCommand("sp_add_addmast_excsecgrpslog", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ' mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For Each GvRow In grdsectorgrps.Rows
                        txtsecgrpcode = GvRow.FindControl("txtsecgrpcode")
                        txtsecgrpname = GvRow.FindControl("txtsecgrpname")
                        txtcitycode = GvRow.FindControl("txtcitycode")
                        If CType(txtsecgrpname.Text, String) <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_exctypsecgrps", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@sectorgrpcode ", SqlDbType.VarChar, 20)).Value = CType(txtsecgrpcode.Text.Trim, String)
                            'mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 100)).Value = CType(txtcitycode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    Next
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                ElseIf Session("ExcTypesState") = "Delete" Then
                    'check If checkForDeletion() = False Then
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_exctypmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("delete from agentmast_countries where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close


                If Session("ExcTypesState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                ElseIf Session("State") = "Edit" Then
                    DisableSectors()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                End If
                If Session("ExcTypesState") = "Delete" Then

                    Dim strscript As String = ""

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully.');", True)

                    strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustBookEngCtry.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



    Protected Sub btnaddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddrow.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdsectorgrps.Rows.Count + 1
        Dim countrycode(count) As String
        Dim countryname(count) As String
        Dim cityname(count) As String
        Dim citycode(count) As String
        Dim oldcountrycode(count) As String

        Dim n As Integer = 0
        Dim txtsecgrpcode As TextBox
        Dim txtsecgrpname As TextBox
        Dim txtcityname As TextBox
        Dim txtcitycode As TextBox
        Dim txtoldsecgrpcode As TextBox

        Try
            For Each GVRow In grdsectorgrps.Rows
                txtsecgrpcode = GVRow.FindControl("txtsecgrpcode")
                countrycode(n) = CType(txtsecgrpcode.Text, String)
                txtsecgrpname = GVRow.FindControl("txtsecgrpname")
                countryname(n) = CType(txtsecgrpname.Text, String)
                txtcitycode = GVRow.FindControl("txtcitycode")
                citycode(n) = CType(txtcitycode.Text, String)
                txtcityname = GVRow.FindControl("txtcityname")
                cityname(n) = CType(txtcityname.Text, String)

                txtoldsecgrpcode = GVRow.FindControl("txtoldsecgrpcode")
                oldcountrycode(n) = CType(txtoldsecgrpcode.Text, String)

                n = n + 1
            Next
            fillDategrd(grdsectorgrps, False, grdsectorgrps.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdsectorgrps.Rows
                If n = i Then
                    Exit For
                End If
                txtsecgrpcode = GVRow.FindControl("txtsecgrpcode")
                txtsecgrpcode.Text = countrycode(n)
                txtsecgrpname = GVRow.FindControl("txtsecgrpname")
                txtsecgrpname.Text = countryname(n)
                txtcitycode = GVRow.FindControl("txtcitycode")
                txtcitycode.Text = citycode(n)
                txtcityname = GVRow.FindControl("txtcityname")
                txtcityname.Text = cityname(n)

                txtoldsecgrpcode = GVRow.FindControl("txtoldsecgrpcode")
                txtoldsecgrpcode.Text = oldcountrycode(n)
                n = n + 1
            Next

            DisableSectors()
             Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub



    Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursionsector','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As System.EventArgs) Handles BtnCancel.Click
        'Response.Redirect("CustomersSearch.aspx")
        'If Session("postback") <> "Addclient" Then
        '    Dim strscript As String = ""
        '    strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Else

        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        ' End If
    End Sub
    Private Sub DisableSectors()

        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Dim cnt As Integer = 0
        Try
            For Each GvRow In grdsectorgrps.Rows
                Dim txtsecgrpcode As TextBox = GvRow.FindControl("txtsecgrpcode")
                Dim txtsecgrpname As TextBox = GvRow.FindControl("txtsecgrpname")
                Dim chkSelect As CheckBox = GvRow.FindControl("chksectgrps")


         



                strSqlQry = "select sum(eplistcode) from (select count(distinct h.eplistcode) eplistcode from   excplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(h.sectoryesno,0)=1 and     esc.mktcode='" & txtsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                            & " select count(distinct h.eplistcode) eplistcode from exccplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(h.sectoryesno,0)=1 and     esc.mktcode='" & txtsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "')  rs"

                cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)

                If cnt > 0 Then
                    txtsecgrpname.Enabled = False
                End If




            Next



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcsectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Function CheckSectors() As Boolean
        CheckSectors = True
        Dim MyAdapter As SqlDataAdapter
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Try
            For Each GvRow In grdsectorgrps.Rows
                Dim txtsecgrpcode As TextBox = GvRow.FindControl("txtsecgrpcode")
                Dim chkSelect As CheckBox = GvRow.FindControl("chksectgrps")
                If txtsecgrpcode.Text <> "" And chkSelect.Checked = True Then

                    strSqlQry = "select distinct h.eplistcode , 'Sector PriceList' Options from excplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, excplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(h.sectoryesno,0)=1 and     esc.mktcode='" & txtsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "' union all  " _
                                & " select distinct h.eplistcode,' SectorCost Price List' Options from exccplist_header h(nolock) cross apply dbo.splitallotmkt(h.sectorgroupcode,',') esc, exccplist_detail d(nolock)  where h.eplistcode=d.eplistcode and isnull(h.sectoryesno,0)=1 and     esc.mktcode='" & txtsecgrpcode.Text & "' and d.exccode='" & txtCustomerCode.Value.Trim & "'"




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    MyAdapter.Fill(ds, "showsectors")


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("eplistcode")) = False Then
                                strMsg = "For this Sector Already Entered Excursion Price list " + "\n"

                                For i = 0 To ds.Tables(0).Rows.Count - 1

                                    strMsg += " Options -  " + ds.Tables(0).Rows(i)("Options") + " - Tran.ID  " + ds.Tables(0).Rows(i)("eplistcode") + "\n"
                                Next

                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                CheckSectors = False
                                Exit Function
                            End If
                        End If
                    End If
                End If

            Next



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcsectorMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
    Protected Sub btndelrow_Click(sender As Object, e As System.EventArgs) Handles btndelrow.Click


        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdsectorgrps.Rows.Count + 1
        Dim countrycode(count) As String
        Dim countryname(count) As String
        Dim cityname(count) As String
        Dim citycode(count) As String
        Dim oldcountrycode(count) As String

        Dim n As Integer = 0
        Dim txtsecgrpcode As TextBox
        Dim txtsecgrpname As TextBox
        Dim txtcityname As TextBox
        Dim txtcitycode As TextBox
        Dim txtoldsecgrpcode As TextBox
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try

            If CheckSectors() = False Then
                Exit Sub
            End If


            For Each GVRow In grdsectorgrps.Rows
                chkSelect = GVRow.FindControl("chksectgrps")
                If chkSelect.Checked = False Then
                    txtsecgrpcode = GVRow.FindControl("txtsecgrpcode")
                    countrycode(n) = CType(txtsecgrpcode.Text, String)
                    txtsecgrpname = GVRow.FindControl("txtsecgrpname")
                    countryname(n) = CType(txtsecgrpname.Text, String)
                    txtcitycode = GVRow.FindControl("txtcitycode")
                    citycode(n) = CType(txtcitycode.Text, String)
                    txtcityname = GVRow.FindControl("txtcityname")
                    cityname(n) = CType(txtcityname.Text, String)

                    txtoldsecgrpcode = GVRow.FindControl("txtoldsecgrpcode")
                    oldcountrycode(n) = CType(txtoldsecgrpcode.Text, String)

                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(grdsectorgrps, False, count)
            'If grdsectorgrps.Rows.Count > 1 Then
            '    fillDategrd(grdsectorgrps, False, grdsectorgrps.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(grdsectorgrps, False, grdsectorgrps.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdsectorgrps.Rows
                If GVRow.RowIndex < count Then
                    txtsecgrpcode = GVRow.FindControl("txtsecgrpcode")
                    txtsecgrpcode.Text = countrycode(n)
                    txtsecgrpname = GVRow.FindControl("txtsecgrpname")
                    txtsecgrpname.Text = countryname(n)
                    txtcitycode = GVRow.FindControl("txtcitycode")
                    txtcitycode.Text = citycode(n)
                    txtcityname = GVRow.FindControl("txtcityname")
                    txtcityname.Text = cityname(n)

                    txtoldsecgrpcode = GVRow.FindControl("txtoldsecgrpcode")
                    txtoldsecgrpcode.Text = oldcountrycode(n)

                    n = n + 1
                End If
            Next
            DisableSectors()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub


End Class

