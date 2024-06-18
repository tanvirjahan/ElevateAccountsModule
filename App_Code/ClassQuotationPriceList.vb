Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class ClassQuotationPriceList
    Implements ITemplate
    Dim Container As Control
    Dim myDataReader As SqlDataReader
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim TemplateType As ListItemType
    Private htControls As New System.Collections.Hashtable()
    Private htBindPropertiesNames As New System.Collections.Hashtable()
    Private htBindExpression As New System.Collections.Hashtable()

    Dim Field1 As String

    Dim Field2 As String
    Dim infotype As String
    'Dim i As Long = 0
    Shared i = 0
    Shared k = 0
    Shared n = 0
    Sub New(ByVal type As ListItemType, ByVal fld1 As String, ByVal fld2 As String) ', ByVal control_type As String)
        i = 0
        k = 0
        n = 0
        TemplateType = type

        Field1 = fld1

        Field2 = fld2

        ' infotype = control_type

    End Sub

    Sub InstantiateIn(ByVal Container As Control) Implements ITemplate.InstantiateIn

        Dim lbl1 As DropDownList = New DropDownList()
        Dim strSqlQry As String = ""
        Dim lc1 As LiteralControl = New LiteralControl()
        Dim lc2 As LiteralControl = New LiteralControl()

        Select Case TemplateType
            Case ListItemType.Header
                
                Dim lbl As New TextBox


                lbl.Text = Field1
                lbl.ID = "txtHead" & k
                lbl.ForeColor = Drawing.Color.White
                'lbl.Font.Bold = True
                lbl.Columns = 10 ' 15
                lbl.Height = 18
                lbl.BorderStyle = BorderStyle.None
                lbl.BackColor = Drawing.Color.Transparent
                lbl.CssClass = "field_caption"

                lbl.ReadOnly = True
                Container.Controls.Add(lbl)
                k = k + 1
                If Field1 = "P.P" Then
                    Dim lblpp As New Label
                    lblpp.Text = Field2
                    lblpp.ID = "lblhd" & n
                 
                    lblpp.Height = 18
                    lblpp.BorderStyle = BorderStyle.None
                    lblpp.BackColor = Drawing.Color.Transparent
                    lblpp.CssClass = "field_caption"
                    lblpp.Visible = False
                    Container.Controls.Add(lblpp)
                    n = n + 1
                End If
            Case ListItemType.Item

                'If Field1 = "Room Type" Then
                '    Dim txt As New TextBox
                '    txt.ID = "txt" & i
                '    txt.ReadOnly = True
                '    txt.Columns = 15
                '    txt.BorderStyle = BorderStyle.None
                '    AddHandler txt.DataBinding, AddressOf BindStringColumn
                '    Container.Controls.Add(txt)
                'ElseIf Field1 = "Room Type Name" Then
                '    Dim txt As New TextBox
                '    txt.ID = "txt" & i
                '    txt.ReadOnly = True
                '    txt.Columns = 15
                '    txt.BorderStyle = BorderStyle.None
                '    AddHandler txt.DataBinding, AddressOf BindStringColumn
                '    Container.Controls.Add(txt)
                If Field1 = "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.ReadOnly = True
                    txt.Columns = 4
                    txt.BorderStyle = BorderStyle.None
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt) '
                ElseIf Field1 = "Pkg" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 6 '15
                    'txt.Text = 0
                    txt.MaxLength = 10
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Remark" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 50
                    txt.MaxLength = 50
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                Else
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10 '15
                    txt.MaxLength = 10
                    txt.CssClass = "field_input"
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                End If

                i = i + 1
        End Select
    End Sub

    Sub BindOperatorColumn(ByVal Sender As Object, ByVal e As EventArgs)
        'Dim strSqlQry As String
        Dim ddl As DropDownList = CType(Sender, DropDownList)

        Dim Container As GridViewRow = CType(ddl.NamingContainer, GridViewRow)

        'ddl.SelectedValue = DataBinder.Eval(Container.DataItem, "[Select]")

    End Sub

    Sub BindStringColumn(ByVal sender As Object, ByVal e As EventArgs)

        Dim lbl2 As TextBox = CType(sender, TextBox)

        Dim Container As GridViewRow = CType(lbl2.NamingContainer, GridViewRow)
        Try
            lbl2.Text = DataBinder.Eval(Container.DataItem, Field1)
            Dim t As Type = lbl2.[GetType]()
            Dim pi As System.Reflection.PropertyInfo = t.GetProperty("Text")

            pi.SetValue(lbl2, lbl2.Text.ToString(), Nothing)
        Catch ex As Exception

        End Try

        'For i As Integer = 0 To htControls.Count - 1
        '    If htBindPropertiesNames(i).ToString().Length > 0 Then
        '        Dim tmpCtrl As Control = DirectCast(htControls(i), Control)
        '        Dim item1Value As String = DirectCast(DataBinder.Eval(Container.DataItem, htBindExpression(i).ToString()), String)
        '        Dim ctrl As Control = sender.FindControl(tmpCtrl.ID)
        '        Dim t As Type = ctrl.[GetType]()
        '        Dim pi As System.Reflection.PropertyInfo = t.GetProperty(htBindPropertiesNames(i).ToString())

        '        pi.SetValue(ctrl, item1Value.ToString(), Nothing)

        '    End If
        'Next


    End Sub
    Public Sub Item_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ph As PlaceHolder = DirectCast(sender, PlaceHolder)
        Dim ri As GridViewRow = DirectCast(ph.NamingContainer, GridViewRow)
        For i As Integer = 0 To htControls.Count - 1
            If htBindPropertiesNames(i).ToString().Length > 0 Then
                Dim tmpCtrl As Control = DirectCast(htControls(i), Control)
                Dim item1Value As String = DirectCast(DataBinder.Eval(ri.DataItem, htBindExpression(i).ToString()), String)
                Dim ctrl As Control = ph.FindControl(tmpCtrl.ID)
                Dim t As Type = ctrl.[GetType]()
                Dim pi As System.Reflection.PropertyInfo = t.GetProperty(htBindPropertiesNames(i).ToString())
                pi.SetValue(ctrl, item1Value.ToString(), Nothing)
            End If
        Next
    End Sub
End Class
