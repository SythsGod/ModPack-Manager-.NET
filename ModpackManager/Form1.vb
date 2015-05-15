Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Main
    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        'My.Computer.Network.DownloadFile("http://download855.mediafire.com/inkmckd604qg/mdx989uyqe0jkeu/EquivalentExchange3-1.7.10-0.3.501.jar", "C:\Test\Mods\EquivalentExchange-1.7.10-0.3.501.jar")

        Dim ds As DataTable = GetTable() 'Datatable with all information from the database
        Dim files As New List(Of String) 'List containing all the files in the mods folder (excluding mods inside of subfolders e.g. the 1.7.10 folder)
        Dim dbfiles As New List(Of String) 'List containing all the mods from the database with only their rawname and version stuck together (e.g. "buildcraft" and "1.1.0" --> "buildcraft-1.1.0.jar")

        For i = 0 To ds.Rows.Count - 1 'Add all the mods to the dbfiles (as mentioned above)
            dbfiles.Add(ds.Rows(i)(4) & "-" & ds.Rows(i)(3) & ".jar")
        Next

        Dim path As String = "C:\Users\Public\Documents\MultiMC\instances\Itvara Server\minecraft\mods" 'Path to the MultiMC mods folder (should be asked on first startup and stored in registry)
        Dim dir As New DirectoryInfo(path) 'The actual directory in variable format

        For Each fle In dir.GetFiles("*.jar") 'Getting all the files from the specified mods folder and adding them to their respective list (files)
            files.Add(fle.Name)
        Next

        MsgBox(files.Count & " | " & dbfiles.Count) 'Quick count comparison (debug)

        Dim found As Boolean

        For Each item In files
            Dim i As Integer = 0

            For i = 0 To dbfiles.Count - 1
                If item.ToString = dbfiles(i).ToString Then
                    Exit For
                Else
                    found = False
                End If
            Next

            If Not found Then
                MsgBox("Error: Mismatch! " & item.ToString & "|" & dbfiles(i).ToString)
            End If
        Next

        ListBox1.DataSource = files
        ListBox2.DataSource = dbfiles
    End Sub

    Private Function GetTable() As DataTable
        Dim myConn As New MySqlConnection
        Dim sqlStr As String
        Dim ds As New DataSet
        Dim adp As MySqlDataAdapter

        sqlStr = "select * from Mods_List ORDER BY modname"
        myConn.ConnectionString = "server=" & My.Resources.Ip & ";User id=" & My.Resources.User & ";password=" & My.Resources.Pass & ";database=" & My.Resources.Db

        Try
            myConn.Open()
            adp = New MySqlDataAdapter(sqlStr, myConn)
            adp.FillSchema(ds, SchemaType.Source, "Mods_List")
            adp.Fill(ds, "Mods_List")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        myConn.Close()

        Return ds.Tables(0)

        myConn.Dispose()
        adp.Dispose()
        ds.Dispose()
    End Function

    Private Function NewRecord(ByVal modname As String, ByVal version As String, ByVal filename As String, ByVal download_url As String, ByVal website_url As String)
        Dim cmdInsert As New MySqlCommand

        cmdInsert.Parameters.AddWithValue("@value1", modname)
        cmdInsert.Parameters.AddWithValue("@value2", version)
        cmdInsert.Parameters.AddWithValue("@value3", filename)
        cmdInsert.Parameters.AddWithValue("@Value4", download_url)

        Dim sqlStr As String = "INSERT INTO Mods_List (modname, version, filename, download_url"
    End Function
End Class
