Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Main
    Public Sub New()
        InitializeComponent()
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        'My.Computer.Network.DownloadFile("http://download855.mediafire.com/inkmckd604qg/mdx989uyqe0jkeu/EquivalentExchange3-1.7.10-0.3.501.jar", "C:\Test\Mods\EquivalentExchange-1.7.10-0.3.501.jar")

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        ListBox3.Items.Clear()
        ListBox4.Items.Clear()

        Dim ds As DataTable = GetTable() 'Datatable with all information from the database
        Dim files As New List(Of String) 'List containing all the files in the mods folder (excluding mods inside of subfolders e.g. the 1.7.10 folder)
        Dim dbfiles As New List(Of String) 'List containing all the mods from the database with only their rawname and version stuck together (e.g. "buildcraft" and "1.1.0" --> "buildcraft-1.1.0.jar")

        For i = 0 To ds.Rows.Count - 1 'Add all the mods to the dbfiles (as mentioned above)
            dbfiles.Add(ds.Rows(i)(4) & "-" & ds.Rows(i)(3) & ".jar")
        Next

        Dim path As String = "C:\Test\All Mods" 'Path to the MultiMC mods folder (should be asked on first startup and stored in registry)
        Dim dir As New DirectoryInfo(path) 'The actual directory in variable format

        For Each fle In dir.GetFiles("*.jar") 'Getting all the files from the specified mods folder and adding them to their respective list (files)
            files.Add(fle.Name)
        Next

        MsgBox(files.Count & " | " & dbfiles.Count) 'Quick count comparison (debug)

        For k = 0 To ds.Rows.Count - 1
            ListBox3.Items.Add(ds.Rows(k)(4) & "-" & ds.Rows(k)(3) & ".jar")
        Next

        For Each item In files
            For j = 0 To dbfiles.Count - 1 'Go through every mod found in the database and see if any of the mods match, then compare their version
                Dim raw As String = ds.Rows(j)(4)

                'Hacky way to deal with items that also contains other items yet are not the same (hweh)
                If raw = "buildcraft" And item.Contains("buildcraftcompat") Or raw = "magicalcrops" And item.Contains("mfrmagicalcropscompat") Then
                    Exit For
                End If

                If item.Contains(raw) Then 'Column 4 in the db is the raw name of the mod
                    'MsgBox("Current mod: " & item)

                    ListBox1.Items.Add(item)

                    Dim modVersion As String = Split(item, "-", 2)(1)

                    If modVersion <> ds.Rows(j)(3) & ".jar" Then 'Compare the version
                        'Code if the version doesn't match
                    Else
                        'Code if the version matches
                    End If

                    Exit For
                End If
            Next
        Next
    End Sub

    Private Function GetTable() As DataTable
        Dim myConn As New MySqlConnection
        Dim sqlStr As String
        Dim ds As New DataSet
        Dim adp As MySqlDataAdapter

        sqlStr = "select * from Mods_List WHERE active = '1' ORDER BY modname"
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
