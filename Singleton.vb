Public Class Singleton

  Private Shared FInstance As Singleton = Nothing

  Private Sub New()

  End Sub

  Public Shared ReadOnly Property Instance()
    Get
      If (FInstance Is Nothing) Then
        FInstance = New Singleton()
      End If

      Return FInstance
    End Get
  End Property

End Class
