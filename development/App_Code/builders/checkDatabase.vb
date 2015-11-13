Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Public Class checkDatabase
    Public Shared Function checkDB() As Boolean
        Try
            Dim version As String
            Using conn As New SqlConnection(config.dbConnection)
                Using cmd As New SqlCommand
                    With cmd
                        .Connection = conn
                        .Connection.Open()
                        .CommandText = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PS_VERSION')) BEGIN SELECT version FROM PS_VERSION WHERE _id=1; END ELSE BEGIN 	SELECT 'no' as version; END"
                        .Parameters.Clear()
                        version = .ExecuteScalar
                        .Connection.Close()
                    End With
                End Using
            End Using


            If version = config.dbVersion Then
                Return True
            Else
                Return False
            End If


        Catch ex As Exception
            Throw New ApplicationException("ERROR CHECKING DATABASE: " & ex.Message)
        End Try

    End Function
End Class
