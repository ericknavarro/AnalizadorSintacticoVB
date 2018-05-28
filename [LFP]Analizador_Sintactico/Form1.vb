' /******************************************
' * Ejemplo desarrollado por Erick Navarro *
' * Blog: e-navarro.blogspot.com           *
' * Octubre - 2015                         *
' ******************************************/

Imports _LFP_Analizador_Sintactico.Token

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtEntrada.Text = "(1+2+(5*(((7)))+2)+"
        'En este ejemplo se muestra una expresión incompleta, léxicamente es correcta
        'pero sintacticamente no, su estructura es incorrecta porque le hace falta un 
        'numero y un paréntesis derecho para estar completa. 

        'Cabe mencionar que en muchos casos este analizador sintáctico no funcionará como
        'se espera porque no cuenta con un sistema de recuperación de errores, este ejemplo
        'puede ampliarse con alguna de las estrategias de recuperación  de errores sintácticos 
        'existentes. 

        'El fundamento teórico que sirvio de soporte para el desarrollo de este ejemplo es el 
        'descrito en la sección 4.4.1 titulada Análisis sintáctico de descenso recursivo del libro:
        'Compiladores, principios, técnicas y herramientas. Aho, Lam, Sethi y Ullman. Segunda Edición.

        'Los errores identificados en el proceso de análisis sintáctico se muestran en consola, 
        'si en su entorno de visual studio no aparece, puede abrirla desde el menú ver, 
        'en la opción resultados o con Ctrl+Alt+O
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Creamos un analizador léxico, le pedimos que analice el texto y que nos de 
        'la lista de tokens resultante del análisis
        Dim scanner As AnalizadorLexico = New AnalizadorLexico()
        Dim listaTokens As List(Of Token) = scanner.escanear(txtEntrada.Text)
        'Agregamos a la lista de tokens uno de finalización, esto es necesario para 
        'que se ejecute adecuadamente el análisis sintáctico
        listaTokens.Add(New Token(Tipo.ULTIMO, ""))
        'Creamos un analizador sintáctico y le pedimos que analice la lista de tokens
        'que nos dio el analizador léxico como resultado
        Dim parser As AnalizadorSintactico = New AnalizadorSintactico()
        parser.parsear(listaTokens)
    End Sub
End Class
