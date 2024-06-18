#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Drawing.Color
#End Region
Partial Class AgentsOnline_Reservation_costamend
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim requestid As String
    Dim rlineno As String
    Dim supagentcode As String
    Dim partycode As String
    Dim datein As String
    Dim dateout As String
    Dim rmtypcode As String
    Dim mealcode As String
    Dim norooms As String
    Dim hotellineno As String
    Dim ds As DataSet
    Dim objdatetime As New clsDateTime
    Dim objUtils As New clsUtils
    Dim dt As DataSet
#End Region
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim strscript As String = "window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            If Not (Request.QueryString("requestid") Is Nothing) = True Then
                requestid = Request.QueryString("requestid")
            End If
            If Not (Request.QueryString("rlineno") Is Nothing) = True Then
                rlineno = Request.QueryString("rlineno")
            End If
            If Not (Request.QueryString("supagentcode") Is Nothing) = True Then
                supagentcode = Request.QueryString("supagentcode")
            End If
            If Not (Request.QueryString("partycode") Is Nothing) = True Then
                partycode = Request.QueryString("partycode")
            End If
            If Not (Request.QueryString("datein") Is Nothing) = True Then
                datein = Request.QueryString("datein")
            End If
            If Not (Request.QueryString("dateout") Is Nothing) = True Then
                dateout = Request.QueryString("dateout")
            End If
            If Not (Request.QueryString("rmtypcode") Is Nothing) = True Then
                rmtypcode = Request.QueryString("rmtypcode")
            End If
            If Not (Request.QueryString("mealcode") Is Nothing) = True Then
                mealcode = Request.QueryString("mealcode")
            End If
            If Not (Request.QueryString("norooms") Is Nothing) = True Then
                norooms = Request.QueryString("norooms")
            End If
            If Not (Request.QueryString("hotellineno") Is Nothing) = True Then
                hotellineno = Request.QueryString("hotellineno")
            End If
            ds = GetData()

            If ds.Tables.Count > 0 Then
                gvRoomDetails.DataSource = ds.Tables(0)
                gvRoomDetails.DataBind()
            End If

        End If
    End Sub

#Region "Public Function GetData()"
    Public Function GetData() As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(4) As SqlParameter

        If requestid <> "" Then
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If
        If rlineno <> "" Then
            parm(1) = New SqlParameter("@rlineno", CType(rlineno, String))
        Else
            parm(1) = New SqlParameter("@rlineno", String.Empty)
        End If
        If partycode <> "" Then
            parm(2) = New SqlParameter("@partycode", CType(partycode, String))
        Else
            parm(2) = New SqlParameter("@partycode", String.Empty)
        End If
        If supagentcode <> "" Then
            parm(3) = New SqlParameter("@supagentcode", CType(supagentcode, String))
        Else
            parm(3) = New SqlParameter("@supagentcode", String.Empty)

        End If

        For i = 0 To 3
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_amendcost_price", parms)
        Return ds
    End Function
#End Region

    Protected Sub gvRoomDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRoomDetails.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblroomtype As Label
            Dim lblrtypeName As Label
            Dim lblMCode As Label
            Dim lblCat As Label
            Dim lblsval As Label
            Dim lblcvalue As Label
            Dim lblNoroom As Label

            lblsval = CType(e.Row.FindControl("lblsval"), Label)
            lblcvalue = CType(e.Row.FindControl("lblcvalue"), Label)
            lblroomtype = CType(e.Row.FindControl("lblroomtype"), Label)
            lblrtypeName = CType(e.Row.FindControl("lblrmtypname"), Label)
            lblMCode = CType(e.Row.FindControl("lblMCode"), Label)
            lblCat = CType(e.Row.FindControl("lblCat"), Label)
            lblNoroom = CType(e.Row.FindControl("lblNoroom"), Label)

            Dim gv As GridView
            gv = CType(e.Row.FindControl("gvPrice"), GridView)
            If ds.Tables.Count > 1 Then
                ds.Tables(1).DefaultView.RowFilter = "rmtypcode='" + lblroomtype.Text + "'" + " and mealcode='" + lblMCode.Text + "'" + " and rmcatcode='" + lblCat.Text + "'"
                gv.DataSource = ds.Tables(1).DefaultView.ToTable()
                gv.DataBind()
            End If
            Dim gvrow As GridViewRow
            For Each gvrow In gv.Rows
                Dim txtcprice As TextBox = CType(gvrow.FindControl("txtcostpricenight"), TextBox)
            Next
        End If
    End Sub

    Protected Sub gvPrice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim txtcostpricenight As TextBox
            Dim btncostclick As Button
            txtcostpricenight = e.Row.FindControl("txtcostpricenight")
            btncostclick = e.Row.FindControl("btncostclick")
            btncostclick.Style("visibility") = "hidden"
            txtcostpricenight.Attributes.Add("onchange", "javascript:Changecost('" + CType(txtcostpricenight.ClientID, String) + "')")
            btncostclick.Attributes.Add("onclick", "Javascript:btnclick('" + CType(btncostclick.ClientID, String) + "')")
        End If
    End Sub

    Protected Sub btncostclick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

    End Sub
End Class
