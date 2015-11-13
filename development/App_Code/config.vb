Imports Microsoft.VisualBasic
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Public Module config
    Public enableCronTasks As Boolean = True
    Public dbVersion As String = "1.0.0"
    Public dbChecked As Boolean = False

    ' // ## VALUES LOADED IN APP.CONFIG!!
    Public pingdomEmail As String
    Public pingdomPassword As String
    Public pingdomAppKey As String
    Public dbConnection As String
    ' // ## END APP.CONFIG

    Public pingdomAPI As String = "https://api.pingdom.com"
    Public pingdomAPIVersion As String = "2.0"


    Public Function regenerateConfig() As Boolean
        Try
            Dim chk As System.Configuration.Configuration
            chk = OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
            If chk.AppSettings.Settings("config").Value = False Then
                Return False
            Else
                dbConnection = "Persist Security Info=False;server=" & chk.AppSettings.Settings("databaseAddress").Value & ";uid=" & chk.AppSettings.Settings("databaseUsername").Value & ";password=" & chk.AppSettings.Settings("databasePassword").Value & ";Connect Timeout=10;Database=" & chk.AppSettings.Settings("databaseName").Value
                pingdomEmail = chk.AppSettings.Settings("pingdomEmail").Value
                pingdomPassword = chk.AppSettings.Settings("pingdomPassword").Value
                pingdomAppKey = chk.AppSettings.Settings("pingdomAppKey").Value

            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
End Module
