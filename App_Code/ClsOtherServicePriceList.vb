Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class ClsOtherServicePriceList
    Implements ITemplate

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

    Sub New(ByVal type As ListItemType, ByVal fld1 As String, ByVal fld2 As String) ', ByVal control_type As String)
        i = 0
        k = 0
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
                lbl.Columns = 15
                lbl.Height = 18
                lbl.BorderStyle = BorderStyle.None
                lbl.BackColor = Drawing.Color.Transparent
                lbl.ReadOnly = True
                Container.Controls.Add(lbl)
                k = k + 1
            Case ListItemType.Item

                If Field1 = "Service Type Code" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.ReadOnly = True
                    txt.Columns = 15
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Service Type Name" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.ReadOnly = True
                    txt.Columns = 15
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.ReadOnly = True
                    txt.Columns = 4
                    txt.BorderStyle = BorderStyle.None
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "Pkg" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 5
                    txt.Text = 0
                    txt.MaxLength = 5
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "From Date" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10
                    txt.MaxLength = 10
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "To Date" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10
                    txt.MaxLength = 10
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                ElseIf Field1 = "VISACHG" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10
                    txt.MaxLength = 10
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                    Dim lt As New LiteralControl("<br/>")
                    Container.Controls.Add(lt)
                    Dim txt1 As New TextBox
                    txt1.ID = "txtTax"
                    txt1.Columns = 10
                    txt1.MaxLength = 10
                    txt1.Width = 60
                    txt1.ToolTip = "Taxable Value"
                    txt1.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt1)
                    Dim txt2 As New TextBox
                    txt2.ID = "txtNonTax"
                    txt2.Columns = 10
                    txt2.MaxLength = 10
                    txt2.Width = 60
                    txt2.ToolTip = "Non Taxable Value"
                    txt2.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt2)
                    Dim txt3 As New TextBox
                    txt3.ID = "txtVat"
                    txt3.Columns = 10
                    txt3.MaxLength = 10
                    txt3.Width = 60
                    txt3.ToolTip = "Vat Value"
                    txt3.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt3)


                ElseIf Field1 = "VISACHG-CH" Then


                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10
                    txt.MaxLength = 10
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)
                    Dim lt As New LiteralControl("<br/>")
                    Container.Controls.Add(lt)
                    Dim txt1 As New TextBox
                    txt1.ID = "txtTax1"
                    txt1.Columns = 10
                    txt1.MaxLength = 10
                    txt1.Width = 60
                    txt1.ToolTip = "Taxable Value"
                    txt1.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt1)
                    Dim txt2 As New TextBox
                    txt2.ID = "txtNonTax1"
                    txt2.Columns = 10
                    txt2.MaxLength = 10
                    txt2.Width = 60
                    txt2.ToolTip = "Non Taxable Value"
                    ' txt2.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt2)
                    Dim txt3 As New TextBox
                    txt3.ID = "txtVat1"
                    txt3.Columns = 10
                    txt3.MaxLength = 10
                    txt3.Width = 60
                    txt3.ToolTip = "Vat Value"
                    txt3.Enabled = False
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt3)


                    'Dim txt_CH As New TextBox
                    'txt_CH.ID = "txt_CH" & i
                    'txt_CH.Columns = 10
                    'txt_CH.MaxLength = 10
                    'AddHandler txt_CH.DataBinding, AddressOf BindStringColumn
                    'Container.Controls.Add(txt_CH)
                    'Dim ltt As New LiteralControl("<br/>")
                    'Container.Controls.Add(ltt)
                    'Dim txt6 As New TextBox
                    'txt6.ID = "txtTax_CH"
                    'txt6.Columns = 10
                    'txt6.MaxLength = 10
                    'txt6.Width = 60
                    'txt6.ToolTip = "Taxable Value"
                    'txt6.Enabled = False
                    'AddHandler txt_CH.DataBinding, AddressOf BindStringColumn
                    'Container.Controls.Add(txt6)
                    'Dim txt4 As New TextBox
                    'txt4.ID = "txtNonTax_CH"
                    'txt4.Columns = 10
                    'txt4.MaxLength = 10
                    'txt4.Width = 60
                    'txt4.ToolTip = "Non Taxable Value"
                    'txt4.Enabled = False
                    'AddHandler txt_CH.DataBinding, AddressOf BindStringColumn
                    'Container.Controls.Add(txt4)
                    'Dim txt5 As New TextBox
                    'txt5.ID = "txtVat_CH"
                    'txt5.Columns = 10
                    'txt5.MaxLength = 10
                    'txt5.Width = 60
                    'txt5.ToolTip = "Vat Value"
                    'txt5.Enabled = False
                    'AddHandler txt_CH.DataBinding, AddressOf BindStringColumn
                    'Container.Controls.Add(txt5)
                ElseIf Field1 <> "Sr No" Then
                    Dim txt As New TextBox
                    txt.ID = "txt" & i
                    txt.Columns = 10
                    txt.MaxLength = 10
                    txt.Style.Add("FieldHeader", Field1)
                    AddHandler txt.DataBinding, AddressOf BindStringColumn
                    Container.Controls.Add(txt)

                End If

                '---------------------------------
                'If Field1 <> "Sr No" Then
                '    Dim txt As New TextBox
                '    txt.ID = "txt" & i
                '    txt.Columns = 15
                '    txt.MaxLength = 10
                '    AddHandler txt.DataBinding, AddressOf BindStringColumn
                '    Container.Controls.Add(txt)

                'End If

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
