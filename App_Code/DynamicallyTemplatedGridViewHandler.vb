Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.ComponentModel
Imports System.Collections

Public Class DynamicallyTemplatedGridViewHandler

    Dim ItemType As ListItemType
    Dim FieldName, InfoType As String
    Public Sub DynamicallyTemplatedGridViewHandler(ByVal item_type As ListItemType, ByVal field_name As String, ByVal info_type As String)
        ItemType = item_type
        FieldName = field_name
        InfoType = info_type
    End Sub


    Public Sub InstantiateIn(ByVal Container As System.Web.UI.Control)
        Select Case (ItemType)
            Case ListItemType.Header
                Dim header_ltrl As New Literal
                header_ltrl.Text = "<b>" + FieldName + "</b>"
                Container.Controls.Add(header_ltrl)
            Case ListItemType.Item
                Dim field_lbl As New Label()
                field_lbl.ID = FieldName
                field_lbl.Text = String.Empty ' //we will bind it later through 'OnDataBinding' event
                '' field_lbl.DataBind += New EventHandler(OnDataBinding)
                Container.Controls.Add(field_lbl)

        End Select
    End Sub
    Private Sub OnDataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim bound_value_obj As Object
        Dim ctrl As Control
        ctrl = CType(sender, Control)
        Dim data_item_container As IDataItemContainer
        data_item_container = CType(ctrl.NamingContainer, IDataItemContainer)
        bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName)
        Select Case ItemType
            Case ListItemType.Item
                Dim field_ltrl As Label
                field_ltrl = CType(sender, Label)
                field_ltrl.Text = bound_value_obj.ToString()

        End Select
    End Sub


End Class
