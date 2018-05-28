' /******************************************
' * Ejemplo desarrollado por Erick Navarro *
' * Blog: e-navarro.blogspot.com           *
' * Octubre - 2015                         *
' ******************************************/

Imports _LFP_Analizador_Sintactico.Token
Public Class AnalizadorSintactico
    'Variable que se usa como índice para recorrer la lista de Tokens
    Dim numPreanalisis As Integer
    'Variable que representa el caracter de anticipación que posee el parser para realizar el análisis
    'en este caso se desarrolla un analizador sintáctico con un caracter (token) de anticipación
    Dim preanalisis As Token
    'Lista de Tokens que el parser recibe del analizador léxico
    Dim listaTokens As List(Of Token)
    'Lo primero que se tiene que hacer es generar una gramática que no sea ambigüa
    'vamos a utilizar la siguiente gramática que reconoce expresiones aritméticas, que 
    'respeta la precedencia de operadores:
    'E->E+T
    'E->E-T
    'E->T
    'T->T*F
    'T->T/F
    'T->F
    'F->(E)
    'F->numero
    '
    'Esta gramática es recursiva por la izquierda, pero para implementarla necesitamos 
    'que la gramática no tenga recursividad por la izuierda, entonces la transformamos: 
    '
    ' Gramática que resuelve el problema:     
    ' 
    ' E-> T EP
    ' EP-> + T EP
    '    | - T EP
    '    | EPSILON
    ' T->F TP
    ' TP-> * F TP
    '    | / F TP
    '    | EPSILON
    ' F->  (E)
    '    | NUMERO
    '

    'Para cada no terminal del lado izquierdo de las producciones, se crea un método
    'Para cada no terminal del lado derecho de las producciones, se hace una llamada 
    'al método que le corresponde, y para cada terminal del lado derecho se hace una 
    'llamada al método match enviando como parámetro el terminal.
    'Es muy importante mencionar que para hacer este analizador no se utiliza niunguna librería
    'auxiliar y ningun generador de analizadores sintácticos.
    Public Sub parsear(lst As List(Of Token))
        listaTokens = lst
        preanalisis = listaTokens.Item(0)
        numPreanalisis = 0
        E()
    End Sub
    Private Sub E()
        ' E-> T EP
        T()
        EP()
    End Sub

    Private Sub EP()
        If preanalisis.getTipo() = Tipo.SIGNO_MAS Then
            ' EP-> + T EP
            emparejar(Tipo.SIGNO_MAS)
            T()
            EP()
        ElseIf preanalisis.getTipo() = Tipo.SIGNO_MEN Then
            ' EP-> - T EP
            emparejar(Tipo.SIGNO_MEN)
            T()
            EP()
        End If
        ' EP-> EPSILON
        ' Para esta producción de EP en epsilon (cadena vacía), simplemente no se hace nada.
    End Sub

    Private Sub T()
        ' T->F TP
        F()
        TP()
    End Sub

    Private Sub TP()
        If preanalisis.getTipo() = Tipo.SIGNO_POR Then
            ' TP-> * F TP
            emparejar(Tipo.SIGNO_POR)
            F()
            TP()
        ElseIf preanalisis.getTipo() = Tipo.SIGNO_DIV Then
            ' TP-> / F TP
            emparejar(Tipo.SIGNO_DIV)
            F()
            TP()
        End If
        ' TP-> EPSILON
    End Sub

    Private Sub F()
        If preanalisis.getTipo() = Tipo.PARENTESIS_IZQ Then
            ' F->  (E)
            emparejar(Tipo.PARENTESIS_IZQ)
            E()
            emparejar(Tipo.PARENTESIS_DER)
        Else
            ' F->  NUMERO
            emparejar(Tipo.NUMERO)
        End If
    End Sub

    'A continuación se programa el método match, se le da el nombre en español de emparejar
    'pues el nombre "match", puede producir confusión, porque algunos pueden pensar que hago uso 
    'de una librería auxiliar, pero no es así.

    'Básicamente, el método match, compara la entrada en la lista de tokens, es decir, el actual
    'preanalisis con lo que debería venir, que es lo que se pasa como parámetro, es decir, "p".

    'Si "lo que viene" no es igual a "lo que debería venir", entonces despliegue un mensaje de error,
    'de lo contrario no haga nada.
    Private Sub emparejar(p As Tipo)
        If Not p = preanalisis.getTipo Then
            Console.WriteLine("Se esperaba " + getTipoParaError(p))
        End If
        'Si preanalisis no es el último token, entonces incremente en uno el índice de preanalisis y dé un 
        'nuevo valor a la variable preanalisis, asígnele el siguiente token en la lista.
        If Not preanalisis.getTipo = Tipo.ULTIMO Then
            numPreanalisis += 1
            preanalisis = listaTokens.Item(numPreanalisis)
        End If
    End Sub

    'Este método solo nos devuelve en texto, los diferentes tipos de tokens, para que podamos usarlo
    'a la  hora de desplegar el mensaje de error.
    Public Function getTipoParaError(p As Tipo) As String
        Select Case p
            Case Tipo.NUMERO
                Return "Número"
            Case Tipo.SIGNO_MAS
                Return "Signo Más"
            Case Tipo.SIGNO_MEN
                Return "Signo Menos"
            Case Tipo.SIGNO_POR
                Return "Signo Por"
            Case Tipo.SIGNO_DIV
                Return "Signo Division"
            Case Tipo.PARENTESIS_IZQ
                Return "Parentesis Izquierdo"
            Case Tipo.PARENTESIS_DER
                Return "Parentesis Derecho"
            Case Else
                Return "Desconocido  "
        End Select
    End Function

End Class

