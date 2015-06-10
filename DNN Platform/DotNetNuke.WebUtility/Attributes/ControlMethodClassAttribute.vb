Imports System

Namespace DotNetNuke.UI.Utilities
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class ControlMethodClassAttribute : Inherits Attribute

        ' Fields
        Private _friendlyNamespace As String = ""

        ' Methods
        Public Sub New()
        End Sub

        Public Sub New(ByVal FriendlyNamespace As String)
            _friendlyNamespace = FriendlyNamespace
        End Sub

        Public Property FriendlyNamespace() As String
            Get
                Return _friendlyNamespace
            End Get
            Set(ByVal value As String)
                _friendlyNamespace = value
            End Set
        End Property

    End Class

End Namespace

