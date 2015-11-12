Imports Microsoft.VisualBasic
Imports Hangfire
Imports Hangfire.SqlServer
Imports Microsoft.Owin
Imports Owin

<Assembly: OwinStartup(GetType(scheduler.Startup))>
Namespace scheduler
    Public Class Startup
        Public Sub Configuration(app As IAppBuilder)
            If regenerateConfig() = False Then
                Exit Sub

            End If
            GlobalConfiguration.Configuration.UseSqlServerStorage(dbConnection)

            'Dim act = Sub(config As IBootstrapperConfiguration)
            '              config.UseSqlServerStorage("dbConnection")
            '              config.UseServer()
            'End Sub

            app.UseHangfireDashboard()
            app.UseHangfireServer()
        End Sub
    End Class

End Namespace