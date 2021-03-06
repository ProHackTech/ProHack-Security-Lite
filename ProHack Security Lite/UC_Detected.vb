﻿Imports System.IO

Public Class UC_Detected

    Private Sub toggle_btnHover(btn As Control, isHover As Boolean)
        If isHover = True Then
            btn.Size = New Size(btn.Size.Width + 4, btn.Size.Height + 4)
            btn.Location = New Point(btn.Location.X - 4, btn.Location.Y - 4)
        Else
            btn.Size = New Size(btn.Size.Width - 4, btn.Size.Height - 4)
            btn.Location = New Point(btn.Location.X + 4, btn.Location.Y + 4)
        End If
    End Sub

    Private Sub picTrash_MouseEnter(sender As Object, e As EventArgs) Handles picTrash.MouseEnter
        toggle_btnHover(picTrash, True)
    End Sub

    Private Sub picTrash_MouseLeave(sender As Object, e As EventArgs) Handles picTrash.MouseLeave
        toggle_btnHover(picTrash, False)
    End Sub

    Private Sub picInfo_MouseEnter(sender As Object, e As EventArgs) Handles picInfo.MouseEnter
        toggle_btnHover(picInfo, True)
    End Sub

    Private Sub picInfo_MouseLeave(sender As Object, e As EventArgs) Handles picInfo.MouseLeave
        toggle_btnHover(picInfo, False)
    End Sub

    Private Sub picIgnore_MouseEnter(sender As Object, e As EventArgs) Handles picIgnore.MouseEnter
        toggle_btnHover(picIgnore, True)
    End Sub

    Private Sub picIgnore_MouseLeave(sender As Object, e As EventArgs) Handles picIgnore.MouseLeave
        toggle_btnHover(picIgnore, False)
    End Sub

    Private Sub Malware_Trash()
        utils.tempStr = txtFile.Text
        ' find index of hash value (filename will be the same index)
        Dim index As Integer = utils.detected_filepath.FindIndex(Function(x As String) x.Contains(utils.tempStr))
        ' try deleting the infected file from the system
        Try
            File.Delete(utils.detected_filepath(index))
            ' remove filename and hash at stored index
            utils.detected_filepath.RemoveAt(index)
            utils.detected_filehash.RemoveAt(index)
            Try
                ' refresh detected file
                File.Create(utils.dectected_malware_file).Dispose()
                ' save list
                utils.writer = New StreamWriter(utils.dectected_malware_file)
                Dim lenDetections As Integer = utils.detected_filehash.Count
                For x As Integer = 0 To lenDetections - 1
                    Dim tempString As String = utils.detected_filepath(x) & " |+| " & utils.detected_filehash(x)
                    utils.writer.WriteLine(tempString)
                Next
                utils.writer.Close()
                utils.writer.Dispose()
                utils.invoke_msg(1, "Success!", "Infected file was deleted from system successfully!")
                malware_informer.refresher()
                Me.Dispose() : GC.Collect()
            Catch ex As Exception
                utils.invoke_msg(3, "File Delete Error", ex.Message.ToString)
            End Try
        Catch ex1 As Exception
            ' try to force delete using cmd -> 'DEL [args]'
            Dim del_command = "/c DEL /F /Q /A " & utils.detected_filepath(index)
            Try
                ' run the cmd process with arguments
                Process.Start("cmd", del_command)
            Catch ex2 As Exception
                utils.invoke_msg(2, "Remove Error", ex2.Message.ToString)
            End Try
        End Try
    End Sub

    Private Sub Malware_Info()
        ' get malware information
        ' by default browser
        Dim searchString = "https://www.virustotal.com/#/search/" & txtHash.Text
        Try
            Process.Start(searchString)
        Catch ex As Exception
            utils.invoke_msg(2, "Process Error0", ex.Message.ToString)
        End Try
    End Sub

    Private Sub Malware_Ignore()
        Dim tempfile As String = txtFile.Text
        ' find index of hash value (filename will be the same index)
        Dim index As Integer = utils.detected_filepath.FindIndex(Function(x As String) x.Contains(tempfile))
        Try
            utils.detected_filepath.RemoveAt(index)
            utils.detected_filehash.RemoveAt(index)
            ' refresh detected file
            File.Create(utils.dectected_malware_file).Dispose()
            ' save list
            utils.writer = New StreamWriter(utils.dectected_malware_file)
            Dim lenDetections As Integer = utils.detected_filehash.Count
            For x As Integer = 0 To lenDetections - 1
                Dim tempString As String = utils.detected_filepath(x) & " |+| " & utils.detected_filehash(x)
                utils.writer.WriteLine(tempString)
            Next
            utils.writer.Close()
            utils.writer.Dispose()
            utils.invoke_msg(1, "Ignored!", "Selected file has been ignored for now..")
            malware_informer.refresher()
            Me.Dispose() : GC.Collect()
        Catch ex As Exception
            utils.invoke_msg(3, "File Delete Error", ex.Message.ToString)
        End Try
    End Sub

    Private Sub picTrash_Click(sender As Object, e As EventArgs) Handles picTrash.Click
        Malware_Trash()
    End Sub

    Private Sub picInfo_Click(sender As Object, e As EventArgs) Handles picInfo.Click
        Malware_Info()
    End Sub

    Private Sub picIgnore_Click(sender As Object, e As EventArgs) Handles picIgnore.Click
        Malware_Ignore()
    End Sub

    Private Sub TxtHash_DoubleClick(sender As Object, e As EventArgs) Handles txtHash.DoubleClick
        ' copy the hash to clipboard
        Clipboard.SetText(txtHash.Text)
    End Sub

    Private Sub TxtFile_DoubleClick(sender As Object, e As EventArgs) Handles txtFile.DoubleClick
        ' copy the filepath to clipboard
        Clipboard.SetText(txtFile.Text)
    End Sub
End Class
