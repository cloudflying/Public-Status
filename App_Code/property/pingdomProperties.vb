Imports Microsoft.VisualBasic

Public Class pingdomCheckProperties
    Public Property id As Integer
    Public Property created As Integer
    Public Property name As String
    Public Property hostname As String
    Public Property use_legacy_notifications As Boolean
    Public Property resolution As Integer
    Public Property type As String
    Public Property ipv6 As Boolean
    Public Property lasterrortime As Integer
    Public Property lasttesttime As Integer
    Public Property lastresponsetime As Integer
    Public Property status As String
    'Public Property alert_policy As Integer
    'Public Property alert_policy_name As String
    'Public Property acktimeout As Integer
    'Public Property autoresolve As Integer

End Class
Public Class pingdomCheckCounts
    Public Property total As Integer
    Public Property limited As Integer
    Public Property filtered As Integer
End Class
Public Class pingdomCheckResponse
    Public Property checks As New List(Of pingdomCheckProperties)
    Public Property counts As New pingdomCheckCounts
End Class
Public Class pingdomRawResults
    Public Property activeprobes As List(Of Integer)
    Public Property results As New List(Of pingdomRawResultsChecks)
End Class
Public Class pingdomRawResultsChecks
    Public Property probeid As Integer
    Public Property time As Integer
    Public Property [status] As String
    Public Property responsetime As Integer
    Public Property statusdesc As String
    Public Property statusdesclong As String
End Class