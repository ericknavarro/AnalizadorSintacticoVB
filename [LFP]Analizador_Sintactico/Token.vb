' /******************************************
' * Ejemplo desarrollado por Erick Navarro *
' * Blog: e-navarro.blogspot.com           *
' * Octubre - 2015                         *
' ******************************************/
Public Class Token
    Enum Tipo
        NUMERO
        SIGNO_MAS
        SIGNO_MEN
        SIGNO_POR
        SIGNO_DIV
        PARENTESIS_IZQ
        PARENTESIS_DER
        ULTIMO
    End Enum

    Private tipoToken As Tipo
    Private valor As String
    Public Sub New(ByVal tipo As Tipo, ByVal auxLex As String)
        Me.tipoToken = tipo
        Me.valor = auxLex
    End Sub
    Public Function getTipo() As Tipo
        Return tipoToken
    End Function
    Public Function getValor() As String
        Return valor
    End Function
    Public Function getTipoEnString() As String
        Select Case tipoToken
            Case Tipo.NUMERO
                Return "Numero       "
            Case Tipo.SIGNO_MAS
                Return "SignoMas     "
            Case Tipo.SIGNO_MEN
                Return "SignoMenos   "
            Case Tipo.SIGNO_POR
                Return "SignoPor     "
            Case Tipo.SIGNO_DIV
                Return "SignoDivision"
            Case Tipo.PARENTESIS_IZQ
                Return "ParentesisIzq"
            Case Tipo.PARENTESIS_DER
                Return "ParentesisDer"
            Case Else
                Return "Desconocido  "
        End Select
    End Function
End Class
