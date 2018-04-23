@Html.DevExpress().GridView( _
    Sub(settings)
            settings.Name = "gvTypedListDataBinding"
            settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "TypedListDataBindingPartial"}

            settings.KeyFieldName = "ID"

            settings.Columns.Add("ID")
            settings.Columns.Add("Text")
            settings.Columns.Add("Quantity")
            settings.Columns.Add("Price")
        
            settings.Columns.Add( _
                Sub(column)
                        column.SetDataItemTemplateContent( _
                            Sub(c)
                                    Html.DevExpress().CheckBox( _
                                        Sub(checkboxSettings)
                                                checkboxSettings.Name = "cb_" + c.KeyValue.ToString()
                                                checkboxSettings.Properties.ClientSideEvents.CheckedChanged = "OnCheckedChanged"
                
                                                If (ViewData("items") IsNot Nothing) Then
                                                    Dim items As Dictionary(Of String, Boolean) = CType(ViewData("items"), Dictionary(Of String, Boolean))
                                                    checkboxSettings.Checked = items.ContainsKey(checkboxSettings.Name) AndAlso CBool(items(checkboxSettings.Name))
                                                End If
                
                                        End Sub).Render()
                            End Sub)
                End Sub)

            settings.ClientSideEvents.BeginCallback = "OnBeginCallback"

    End Sub).Bind(Model).GetHtml()