Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO

Partial Class UpdateSupplierBooking
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Web Services"
    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function GetSupplier(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliers As New List(Of String)
        Dim resvParamId As String = ""
        Try
            If prefixText = " " Then
                prefixText = ""
            End If

            Dim key As String = contextKey.Split(";")(0)
            Dim chk As String = contextKey.Split(";")(1)
            If key = "Tours" Then
                resvParamId = "1033,458" '458 is added / changed by mohamed on 27/12/2018 as sometime hotel will be service provider
            ElseIf key = "Transfers" Then
                resvParamId = "564,458" '458 is added / changed by mohamed on 27/12/2018 as sometime hotel will be service provider
            ElseIf key = "AiportMA" Then
                resvParamId = "564"
            ElseIf key = "Visa" Then
                resvParamId = "1032"
            ElseIf key = "Others" Then
                resvParamId = "1501,458" '458 is added / changed by mohamed on 27/12/2018 as sometime hotel will be service provider
            End If

            If chk = "true" Then
                'Show all suppliers - christo.A -30/04/19
                strSqlQry = "select p.partycode,p.partyname from partymast p(nolock) " &
                            " where p.active=1  and p.partyname like '%" & prefixText & "%' order by partyname "
            ElseIf chk = "false" Then
                strSqlQry = "select p.partycode,p.partyname from partymast p(nolock) inner join reservation_parameters r(nolock) " &
                            "on p.sptypecode=r.option_selected where p.active=1 and r.param_id in (" & resvParamId & ") and p.partyname like '%" & prefixText & "%' order by partyname "
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return suppliers
        Catch ex As Exception
            Return suppliers
        End Try
    End Function
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                Dim requestId As String = CType(Request.QueryString("ID"), String)
                Dim divCode As String = CType(Request.QueryString("divid"), String)

                txtBookingNo.Text = requestId
                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)

                If requestId <> "" Then
                    sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    mySqlCmd = New SqlCommand("sp_get_booking_costprovision", sqlConn)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = txtBookingNo.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
                    Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                        Using dt As New DataTable()
                            myDataAdapter.Fill(dt)
                            gvUpdateSupplier.DataSource = dt
                            gvUpdateSupplier.DataBind()
                        End Using
                    End Using
                    clsDBConnect.dbCommandClose(mySqlCmd)
                    clsDBConnect.dbConnectionClose(sqlConn)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierBooking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvUpdateSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUpdateSupplier.RowDataBound"
    Protected Sub gvUpdateSupplier_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUpdateSupplier.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim txtSupplier As TextBox = CType(e.Row.FindControl("txtSupplier"), TextBox)
            Dim txtSupplierCode As TextBox = CType(e.Row.FindControl("txtSupplierCode"), TextBox)
            txtSupplier.Attributes.Add("onkeyup", "ClearCode(this,'" + txtSupplierCode.ClientID + "')")
            Dim txtPaxorunitrate As TextBox = CType(e.Row.FindControl("txtPaxorunitrate"), TextBox)
            txtPaxorunitrate.Attributes.Add("onkeypress", "validateDecimalOnly(event,this)")



        
 
            Dim lblNoUnit As Label = CType(e.Row.FindControl("lblNoUnit"), Label)
            Dim txtCostValue As TextBox = CType(e.Row.FindControl("txtCostValue"), TextBox)

            txtPaxorunitrate.Attributes.Add("onchange", "fnFindCostValue('" + txtPaxorunitrate.ClientID + "','" + lblNoUnit.ClientID + "','" + txtCostValue.ClientID + "')")

            If IsNumeric(txtPaxorunitrate.Text) Then
                txtPaxorunitrate.Text = Math.Round(Convert.ToDecimal(txtPaxorunitrate.Text), decimalPlaces)
            End If
            ' Dim txtCostValue As TextBox = CType(e.Row.FindControl("txtCostValue"), TextBox)
            If IsNumeric(txtCostValue.Text) Then
                txtCostValue.Text = Math.Round(Convert.ToDecimal(txtCostValue.Text), decimalPlaces)
            End If
            Dim chkInHouseProvider As CheckBox = CType(e.Row.FindControl("chkInHouseProvider"), CheckBox)
            Dim chkComplimentary As CheckBox = CType(e.Row.FindControl("chkComplimentary"), CheckBox)
            If chkInHouseProvider.Checked Then
                txtSupplier.Enabled = False
                txtPaxorunitrate.Enabled = False
                chkComplimentary.Enabled = False
            Else
                txtSupplier.Enabled = True
                txtPaxorunitrate.Enabled = True
                chkComplimentary.Enabled = True
            End If
            If chkComplimentary.Checked Then
                txtPaxorunitrate.Enabled = False
                chkInHouseProvider.Enabled = False
            Else
                txtPaxorunitrate.Enabled = True
                chkInHouseProvider.Enabled = True
            End If


            chkComplimentary.Attributes.Add("onchange", "fnComplimentaryChange('" + chkInHouseProvider.ClientID + "','" + txtPaxorunitrate.ClientID + "','" + txtCostValue.ClientID + "','" + chkComplimentary.ClientID + "','" + txtSupplier.ClientID + "','" + txtSupplierCode.ClientID + "')")
            chkInHouseProvider.Attributes.Add("onchange", "fnInHouseProviderChange('" + chkInHouseProvider.ClientID + "','" + txtPaxorunitrate.ClientID + "','" + txtCostValue.ClientID + "','" + chkComplimentary.ClientID + "','" + txtSupplier.ClientID + "','" + txtSupplierCode.ClientID + "')")
            Dim lblServiceType As Label = CType(e.Row.FindControl("lblServiceType"), Label)
            Dim lblServiceName As Label = CType(e.Row.FindControl("lblServiceName"), Label)
            Dim lblRowId As Label = CType(e.Row.FindControl("lblRowId"), Label)
            Dim lblRownumber As Label = CType(e.Row.FindControl("lblRownumber"), Label)
            Dim lblPurchaseInvoiceNo As Label = CType(e.Row.FindControl("lblPurchaseInvoiceNo"), Label)
            Dim strInvNo As String = ""
            If lblServiceType.Text = "Tours" Then
                strInvNo = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select purchaseinvoiceno from PurchaseInvoiceToursdetail(nolock) where requestId='" & txtBookingNo.Text.Trim & "' and elineno='" & lblRowId.Text.Trim & "' and roomno='" & lblRownumber.Text.Trim & "' ") 'and servicedetails='" & lblServiceName.Text.Trim & "' 
                If strInvNo <> "" Then
                    txtSupplier.Enabled = False
                    lblPurchaseInvoiceNo.Text = strInvNo
                End If
            ElseIf lblServiceType.Text = "Transfers" Then
                strInvNo = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select purchaseinvoiceno from PurchaseInvoiceTransfersdetail(nolock) where requestId='" & txtBookingNo.Text.Trim & "' and rlineno='" & lblRowId.Text.Trim & "' and roomno='" & lblRownumber.Text.Trim & "' ")
                If strInvNo <> "" Then
                    txtSupplier.Enabled = False
                    lblPurchaseInvoiceNo.Text = strInvNo
                End If
            ElseIf lblServiceType.Text = "AiportMA" Then
                strInvNo = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select purchaseinvoiceno from PurchaseInvoice_Airportma_detail(nolock) where requestId='" & txtBookingNo.Text.Trim & "' and alineno='" & lblRowId.Text.Trim & "' and roomno='" & lblRownumber.Text.Trim & "' ")
                If strInvNo <> "" Then
                    txtSupplier.Enabled = False
                    lblPurchaseInvoiceNo.Text = strInvNo
                End If
            ElseIf lblServiceType.Text = "Others" Then
                strInvNo = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select purchaseinvoiceno from PurchaseInvoice_Others_detail(nolock) where requestId='" & txtBookingNo.Text.Trim & "' and olineno='" & lblRowId.Text.Trim & "' and roomno='" & lblRownumber.Text.Trim & "' ")
                If strInvNo <> "" Then
                    txtSupplier.Enabled = False
                    lblPurchaseInvoiceNo.Text = strInvNo
                End If
            ElseIf lblServiceType.Text = "Visa" Then
                strInvNo = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select purchaseinvoiceno from PurchaseInvoice_Visa_detail(nolock) where requestId='" & txtBookingNo.Text.Trim & "' and vlineno='" & lblRowId.Text.Trim & "'  and roomno='" & lblRownumber.Text.Trim & "' ")
                If strInvNo <> "" Then
                    txtSupplier.Enabled = False
                    lblPurchaseInvoiceNo.Text = strInvNo
                End If
            End If
     
            ''txtBookingNo


        End If
    End Sub
#End Region

#Region "Protected Sub txtPaxorunitrate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub txtPaxorunitrate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtPaxorunitrate As TextBox = CType(sender, TextBox)

        If IsNumeric(txtPaxorunitrate.Text) Then
            Dim row As GridViewRow = txtPaxorunitrate.NamingContainer
            Dim lblNoUnit As Label = CType(row.FindControl("lblNoUnit"), Label)
            Dim txtCostValue As TextBox = CType(row.FindControl("txtCostValue"), TextBox)
            Dim noUnits As Decimal
            If IsNumeric(lblNoUnit.Text) Then
                noUnits = Convert.ToDecimal(lblNoUnit.Text)
            Else
                noUnits = 0
            End If
            Dim calcost As Decimal = noUnits * Convert.ToDecimal(txtPaxorunitrate.Text)
            txtCostValue.Text = calcost
            txtCostValue.Focus()
        Else

        End If
    End Sub
#End Region

#Region "Protected Function validation() As Boolean"
    Protected Function validation() As Boolean
        Dim lblServiceType As Label
        Dim lblRowId As Label
        Dim lblRownumber As Label
        Dim lblRateType As Label
        Dim txtSupplier As TextBox
        Dim txtSupplierCode As TextBox
        Dim txtPaxorunitrate As TextBox
        Dim txtCostValue As TextBox
        Dim chkComplimentary As CheckBox
        Dim chkInHouseProvider As CheckBox
        Dim lblSublineno As Label

        For Each gvr As GridViewRow In gvUpdateSupplier.Rows
            lblServiceType = CType(gvr.FindControl("lblServiceType"), Label)
            lblRowId = CType(gvr.FindControl("lblRowId"), Label)
            lblRownumber = CType(gvr.FindControl("lblRownumber"), Label)
            lblRateType = CType(gvr.FindControl("lblRateType"), Label)
            txtSupplier = CType(gvr.FindControl("txtSupplier"), TextBox)
            txtSupplierCode = CType(gvr.FindControl("txtSupplierCode"), TextBox)
            txtPaxorunitrate = CType(gvr.FindControl("txtPaxorunitrate"), TextBox)
            txtCostValue = CType(gvr.FindControl("txtCostValue"), TextBox)
            chkComplimentary = CType(gvr.FindControl("chkComplimentary"), CheckBox)
            chkInHouseProvider = CType(gvr.FindControl("chkInHouseProvider"), CheckBox)
            lblSublineno = CType(gvr.FindControl("lblSublineno"), Label)

            If txtSupplier.Text.Trim = "" Then txtSupplierCode.Text = ""

            If Not IsNumeric(txtPaxorunitrate.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or Unit rate should be numeric' );", True)
                txtPaxorunitrate.Focus()
                validation = False
                Exit Function
            End If

            If Not IsNumeric(txtCostValue.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value should be numeric' );", True)
                txtPaxorunitrate.Focus()
                validation = False
                Exit Function
            End If

            If txtSupplierCode.Text.Trim <> "" Then
                If txtSupplierCode.Text.Trim <> "" And chkInHouseProvider.Checked = True Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Both Supplier and In house provider are selected' );", True)
                    txtSupplier.Focus()
                    validation = False
                    Exit Function
                End If

                If txtSupplierCode.Text.Trim <> "" And chkComplimentary.Checked = True And (Convert.ToDecimal(txtPaxorunitrate.Text) > 0 Or Convert.ToDecimal(txtCostValue.Text) > 0) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or unit rate and cost value should be zero for complimentary from supplier' );", True)
                    txtSupplier.Focus()
                    validation = False
                    Exit Function
                End If

                If Convert.ToDecimal(txtPaxorunitrate.Text) <= 0 And Convert.ToInt32(lblRownumber.Text) = 0 And chkComplimentary.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or Unit rate should be greater than zero' );", True)
                    txtPaxorunitrate.Focus()
                    validation = False
                    Exit Function
                End If

                If Convert.ToDecimal(txtCostValue.Text) <= 0 And Convert.ToInt32(lblRownumber.Text) = 0 And chkComplimentary.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value should be greater than zero' );", True)
                    txtPaxorunitrate.Focus()
                    validation = False
                    Exit Function
                End If
                If lblRateType.Text.Trim = "Combo" And Convert.ToInt32(lblSublineno.Text) = 1 And chkComplimentary.Checked = False Then
                    If Convert.ToDecimal(txtPaxorunitrate.Text) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or Unit rate should be greater than zero' );", True)
                        txtPaxorunitrate.Focus()
                        validation = False
                        Exit Function
                    End If
                    If Convert.ToDecimal(txtCostValue.Text) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value should be greater than zero' );", True)
                        txtPaxorunitrate.Focus()
                        validation = False
                        Exit Function
                    End If
                End If
                'If lblRateType.Text.Trim = "Combo" And Convert.ToInt32(lblSublineno.Text) > 1 And chkComplimentary.Checked = False Then
                '    If Convert.ToDecimal(txtPaxorunitrate.Text) > 0 Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or Unit rate should add for first service only in multi date or combo tours; Remaining services should be zero' );", True)
                '        txtPaxorunitrate.Focus()
                '        validation = False
                '        Exit Function
                '    End If
                '    If Convert.ToDecimal(txtCostValue.Text) > 0 Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value should add for first service in multi date or combo tours; Remaining services should be zero' );", True)
                '        txtPaxorunitrate.Focus()
                '        validation = False
                '        Exit Function
                '    End If
                'End If
            Else
                If chkInHouseProvider.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('select Supplier or In house provider' );", True)
                    txtSupplier.Focus()
                    validation = False
                    Exit Function
                Else
                    If Convert.ToDecimal(txtPaxorunitrate.Text) > 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pax or Unit rate should be zero for in house provider' );", True)
                        txtPaxorunitrate.Focus()
                        validation = False
                        Exit Function
                    End If
                    If Convert.ToDecimal(txtCostValue.Text) > 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost value should be zero for in house provider' );", True)
                        txtPaxorunitrate.Focus()
                        validation = False
                        Exit Function
                    End If
                End If
            End If
        Next
        validation = True
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If validation() = False Then
                Exit Sub
            End If

            Dim serviceDt As New DataTable
            serviceDt.Columns.Add(New DataColumn("servicetype", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("servicedate", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("elineno", GetType(Integer)))
            serviceDt.Columns.Add(New DataColumn("servicename", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("partycode", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("ratetype", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("paxtype", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("noofpaxorunit", GetType(Integer)))
            serviceDt.Columns.Add(New DataColumn("childno", GetType(Integer)))
            serviceDt.Columns.Add(New DataColumn("childage", GetType(Decimal)))
            serviceDt.Columns.Add(New DataColumn("paxorunitrate", GetType(Decimal)))
            serviceDt.Columns.Add(New DataColumn("costvalue", GetType(Decimal)))
            serviceDt.Columns.Add(New DataColumn("rownumber", GetType(Integer)))
            serviceDt.Columns.Add(New DataColumn("partyname", GetType(String)))
            serviceDt.Columns.Add(New DataColumn("supplierComplimentary", GetType(Boolean)))
            serviceDt.Columns.Add(New DataColumn("InHouseProvider", GetType(Boolean)))
            serviceDt.Columns.Add(New DataColumn("SubLineNo", GetType(Integer)))

            Dim lblServiceType As Label
            Dim lblServiceDate As Label
            Dim lblRowId As Label
            Dim lblRownumber As Label
            Dim lblServiceName As Label
            Dim lblRateType As Label
            Dim lblPaxType As Label
            Dim lblNoUnit As Label
            Dim lblChildNo As Label
            Dim lblChildAge As Label
            Dim txtSupplier As TextBox
            Dim txtSupplierCode As TextBox
            Dim txtPaxorunitrate As TextBox
            Dim txtCostValue As TextBox
            Dim chkComplimentary As CheckBox
            Dim chkInHouseProvider As CheckBox
            Dim lblSublineno As Label

            For Each gvr As GridViewRow In gvUpdateSupplier.Rows
                Dim dr As DataRow = serviceDt.NewRow
                lblServiceType = CType(gvr.FindControl("lblServiceType"), Label)
                lblServiceDate = CType(gvr.FindControl("lblServiceDate"), Label)
                lblRowId = CType(gvr.FindControl("lblRowId"), Label)
                lblRownumber = CType(gvr.FindControl("lblRownumber"), Label)
                lblServiceName = CType(gvr.FindControl("lblServiceName"), Label)
                lblRateType = CType(gvr.FindControl("lblRateType"), Label)
                lblPaxType = CType(gvr.FindControl("lblPaxType"), Label)
                lblNoUnit = CType(gvr.FindControl("lblNoUnit"), Label)
                lblChildNo = CType(gvr.FindControl("lblChildNo"), Label)
                lblChildAge = CType(gvr.FindControl("lblChildAge"), Label)
                txtSupplier = CType(gvr.FindControl("txtSupplier"), TextBox)
                txtSupplierCode = CType(gvr.FindControl("txtSupplierCode"), TextBox)
                txtPaxorunitrate = CType(gvr.FindControl("txtPaxorunitrate"), TextBox)
                txtCostValue = CType(gvr.FindControl("txtCostValue"), TextBox)
                chkComplimentary = CType(gvr.FindControl("chkComplimentary"), CheckBox)
                chkInHouseProvider = CType(gvr.FindControl("chkInHouseProvider"), CheckBox)
                lblSublineno = CType(gvr.FindControl("lblSublineno"), Label)

                dr("servicetype") = lblServiceType.Text.Trim
                If IsDate(lblServiceDate.Text) Then
                    dr("servicedate") = Convert.ToDateTime(lblServiceDate.Text).ToString("yyyy/MM/dd")
                Else
                    dr("servicedate") = ""
                End If
                dr("elineno") = Convert.ToInt32(lblRowId.Text)
                dr("servicename") = lblServiceName.Text.Trim
                dr("partycode") = txtSupplierCode.Text.Trim
                dr("ratetype") = lblRateType.Text.Trim
                dr("paxtype") = lblPaxType.Text.Trim
                dr("noofpaxorunit") = Convert.ToInt32(lblNoUnit.Text)
                If IsNumeric(lblChildNo.Text) Then
                    dr("childno") = Convert.ToInt32(lblChildNo.Text)
                End If
                If IsNumeric(lblChildAge.Text) Then
                    dr("childage") = Convert.ToDecimal(lblChildAge.Text)
                End If
                dr("paxorunitrate") = Convert.ToDecimal(txtPaxorunitrate.Text)
                dr("costvalue") = dr("noofpaxorunit") * dr("paxorunitrate") 'Convert.ToDecimal(txtCostValue.Text)
                dr("rownumber") = Convert.ToInt32(lblRownumber.Text)
                dr("partyname") = txtSupplier.Text
                dr("supplierComplimentary") = chkComplimentary.Checked
                dr("InHouseProvider") = chkInHouseProvider.Checked
                If IsNumeric(lblSublineno.Text) Then
                    dr("subLineNo") = Convert.ToInt32(lblSublineno.Text)
                Else
                    dr("subLineNo") = 0
                End If
                serviceDt.Rows.Add(dr)
            Next

            Dim xmlServices As String = ""
            If serviceDt.Rows.Count > 0 Then
                serviceDt.TableName = "Services"
                xmlServices = ConvertDatatableToXML(serviceDt)
            End If

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            sqlTrans = sqlConn.BeginTransaction
            mySqlCmd = New SqlCommand("sp_add_booking_services_cost", sqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar)).Value = txtBookingNo.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@xmlServices", SqlDbType.Xml)).Value = xmlServices
            mySqlCmd.ExecuteNonQuery()
            sqlTrans.Commit()
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
            Dim msg As String = "alert('Supplier details updated Successfully' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", msg, True)
            btnCancel_Click(sender, e)
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                If sqlConn.State = ConnectionState.Open Then
                    If Not sqlTrans Is Nothing Then
                        sqlTrans.Rollback()
                        clsDBConnect.dbSqlTransation(sqlTrans)
                    End If
                    clsDBConnect.dbConnectionClose(sqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierBooking.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String"
    Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String
        Dim mstr As New MemoryStream()
        dt.WriteXml(mstr, True)
        mstr.Seek(0, SeekOrigin.Begin)
        Dim sr As New StreamReader(mstr)
        Dim xmlstr As String
        xmlstr = sr.ReadToEnd()
        Return (xmlstr)
    End Function
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Protected Sub chkInHouseProvider_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkInHouseProvider_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkInHouseProvider As CheckBox = CType(sender, CheckBox)
        Dim gvr As GridViewRow = chkInHouseProvider.NamingContainer
        Dim txtPaxorunitrate As TextBox = CType(gvr.FindControl("txtPaxorunitrate"), TextBox)
        Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim chkComplimentary As CheckBox = CType(gvr.FindControl("chkComplimentary"), CheckBox)
        Dim txtSupplier As TextBox = CType(gvr.FindControl("txtSupplier"), TextBox)
        Dim txtSupplierCode As TextBox = CType(gvr.FindControl("txtSupplierCode"), TextBox)
        If chkInHouseProvider.Checked Then
            txtPaxorunitrate.Text = 0
            txtPaxorunitrate.Enabled = False
            txtCostValue.Text = 0
            chkComplimentary.Checked = False
            chkComplimentary.Enabled = False
            txtSupplier.Text = ""
            txtSupplierCode.Text = ""
            txtSupplier.Enabled = False
        Else
            txtSupplier.Enabled = True
            txtPaxorunitrate.Enabled = True
            chkComplimentary.Enabled = True
        End If
    End Sub
#End Region

#Region "Protected Sub chkComplimentary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub chkComplimentary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkComplimentary As CheckBox = CType(sender, CheckBox)
        Dim gvr As GridViewRow = chkComplimentary.NamingContainer
        Dim txtPaxorunitrate As TextBox = CType(gvr.FindControl("txtPaxorunitrate"), TextBox)
        Dim txtCostValue As TextBox = CType(gvr.FindControl("txtCostValue"), TextBox)
        Dim chkInHouseProvider As CheckBox = CType(gvr.FindControl("chkInHouseProvider"), CheckBox)
        If chkComplimentary.Checked Then
            txtPaxorunitrate.Text = 0
            txtPaxorunitrate.Enabled = False
            txtCostValue.Text = 0
            chkInHouseProvider.Checked = False
            chkInHouseProvider.Enabled = False
        Else
            txtPaxorunitrate.Enabled = True
            chkInHouseProvider.Enabled = True
        End If
    End Sub
#End Region

End Class
