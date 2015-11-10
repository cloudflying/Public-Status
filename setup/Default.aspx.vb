Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Partial Class setup_Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                form1.Action = Request.RawUrl


                If regenerateConfig() = True Then
                    Dim bldr As New StringBuilder
                    bldr.AppendLine("<div class=""row row-pad""><div class=""col-md-12""><div class=""alert alert-danger"" role=""alert"">Configuration already completed, re-entering data will overwrite current configuration!</div></div></div>")
                    plWarning.Controls.Add(New Literal With {.Text = bldr.ToString})

                    Dim chk As System.Configuration.Configuration
                    chk = OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

                    txtDatabaseHost.Text = chk.AppSettings.Settings("databaseAddress").Value
                    txtDatabaseName.Text = chk.AppSettings.Settings("databaseName").Value
                    txtDatabaseUsername.Text = chk.AppSettings.Settings("databaseUsername").Value
                    txtpingdomEmail.Text = chk.AppSettings.Settings("pingdomEmail").Value
                    txtPingdomPassword.Text = chk.AppSettings.Settings("pingdomPassword").Value
                    txtPingdomAppKey.Text = chk.AppSettings.Settings("pingdomAppKey").Value
                End If

            End If



        Catch ex As Exception
            Response.Write("Load Error" & ex.Message)
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try

            Dim config As System.Configuration.Configuration
            config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

            If Len(txtDatabaseHost.Text) > 0 And Len(txtDatabaseUsername.Text) > 0 And Len(txtDatabasePassword.Text) > 0 And Len(txtDatabaseName.Text) > 0 Then
                config.AppSettings.Settings.Remove("databaseAddress")
                config.AppSettings.Settings.Add("databaseAddress", txtDatabaseHost.Text)
                config.AppSettings.Settings.Remove("databaseName")
                config.AppSettings.Settings.Add("databaseName", txtDatabaseName.Text)
                config.AppSettings.Settings.Remove("databaseUserName")
                config.AppSettings.Settings.Add("databaseUserName", txtDatabaseUsername.Text)
                config.AppSettings.Settings.Remove("databasePassword")
                config.AppSettings.Settings.Add("databasePassword", txtDatabasePassword.Text)

            End If

            config.AppSettings.Settings.Remove("pingdomEmail")
            config.AppSettings.Settings.Add("pingdomEmail", txtpingdomEmail.Text)
            config.AppSettings.Settings.Remove("pingdomPassword")
            config.AppSettings.Settings.Add("pingdomPassword", txtPingdomPassword.Text)
            config.AppSettings.Settings.Remove("pingdomAppKey")
            config.AppSettings.Settings.Add("pingdomAppKey", txtPingdomAppKey.Text)
            config.AppSettings.Settings("config").Value = True
            config.Save(ConfigurationSaveMode.Minimal)
            Response.Redirect("/", False)
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
