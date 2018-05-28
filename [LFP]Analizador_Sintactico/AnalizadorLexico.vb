' /******************************************
' * Ejemplo desarrollado por Erick Navarro *
' * Blog: e-navarro.blogspot.com           *
' * Octubre - 2015                         *
' ******************************************/

Imports _LFP_Analizador_Sintactico.Token

Public Class AnalizadorLexico
    'Variable que representa la lista de tokens
    Private salida As List(Of Token)
    'Variable que representa el estado actual
    Private estado As Integer
    'Variable que representa el lexema que actualmente se esta acumulando
    Private auxLex As String
    Public Function escanear(ByVal entrada As String) As List(Of Token)
        'Le agrego caracter de fin de cadena porque hay lexemas que aceptan con 
        'el primer caracter del siguiente lexema y si este caracter no existe entonces
        'perdemos el lexema
        entrada = entrada + "#"
        salida = New List(Of Token)
        estado = 0
        auxLex = ""
        Dim c As Char
        'Ciclo que recorre de izquierda a derecha caracter por caracter la cadena de entrada
        For i As Integer = 0 To entrada.Length - 1 Step 1
            c = entrada.Chars(i)
            'Select en el que cada caso representa cada uno de los estados del conjunto de estados
            Select Case estado
                Case 0
                    'Para cada caso (o estado) hay un if elseif elseif ... else que representan el conjunto de transiciones que 
                    'salen de dicho estado, por ejemplo, estando en el estado 0 si el caracter reconocido es un dígito entonces, 
                    'pasamos al estado 1 y acumulamos el caracter reconocido en auxLex, que es el auxiliar de lexemas.
                    If Char.IsDigit(c) Then
                        estado = 1
                        auxLex += c
                    ElseIf (c = "+") Then
                        auxLex += c
                        addToken(Tipo.SIGNO_MAS)
                    ElseIf (c = "-") Then
                        auxLex += c
                        addToken(Tipo.SIGNO_MEN)
                    ElseIf (c = "*") Then
                        auxLex += c
                        addToken(Tipo.SIGNO_POR)
                    ElseIf (c = "/") Then
                        auxLex += c
                        addToken(Tipo.SIGNO_DIV)
                    ElseIf (c = "(") Then
                        auxLex += c
                        addToken(Tipo.PARENTESIS_IZQ)
                    ElseIf (c = ")") Then
                        auxLex += c
                        addToken(Tipo.PARENTESIS_DER)
                    Else
                        If (c = "#" And i = entrada.Length() - 1) Then
                            'Hemos concluido el análisis léxico.
                            'Console.WriteLine("Hemos concluido el análisis léxico satisfactoriamente")
                        Else
                            Console.WriteLine("Error léxico con: " + c)
                            estado = 0
                        End If
                    End If
                Case 1
                    If (Char.IsDigit(c)) Then
                        estado = 1
                        auxLex += c
                    ElseIf (c = ".") Then
                        estado = 2
                        auxLex += c
                    Else
                        addToken(Tipo.NUMERO)
                        i -= 1
                    End If
                Case 2
                    If (Char.IsDigit(c)) Then
                        estado = 3
                        auxLex += c
                    Else
                        Console.WriteLine("Error léxico con: " & c & " despues del punto decimal se espera uno o más números.")
                        estado = 0
                    End If
                Case 3
                    If (Char.IsDigit(c)) Then
                        estado = 3
                        auxLex += c
                    Else
                        addToken(Tipo.NUMERO)
                        i -= 1
                    End If
            End Select
        Next
        Return salida
    End Function
    Private Sub addToken(ByVal tipo As Tipo)
        salida.Add(New Token(tipo, auxLex))
        auxLex = ""
        estado = 0
    End Sub
    Public Sub imprimirLista(ByVal l As List(Of Token))
        For Each t As Token In l
            Console.WriteLine(t.getTipoEnString() & "<-->" & t.getValor())
        Next
    End Sub
End Class

