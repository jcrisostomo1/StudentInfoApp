Imports System.Data.SqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Imports System.Drawing.Text
Imports Accessibility

Public Class BaseForm

    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private id As Integer

    Private Sub LoadData()

        'Create Command Object 
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT * FROM Students"
        myCmd.ExecuteNonQuery()

        Dim myDataAdapter As New SqlDataAdapter(myCmd)
        Dim myDataTable As New DataTable
        myDataAdapter.Fill(myDataTable)
        DataGridView1.DataSource = myDataTable
    End Sub

    Private Sub Reset()
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtAddress.Text = ""
        txtPostCode.Text = ""
        txtPhone.Text = ""
    End Sub


    Private Sub BaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        myConn = New SqlConnection("Initial Catalog=myconnector;" &
            "Data Source=localhost;Integrated Security=SSPI;")

        If myConn.State = ConnectionState.Open Then
            myConn.Close()
        End If

        myConn.Open()
        LoadData()
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click

        myConn = New SqlConnection("Initial Catalog=myconnector;" &
            "Data Source=localhost;Integrated Security=SSPI;")
        myConn.Open()
        myCmd = myConn.CreateCommand()
        myCmd.CommandType = CommandType.Text
        myCmd.CommandText =
        "INSERT INTO Students (Firstname, Lastname, Address, PostCode, Phone)
        VALUES('" & txtFirstName.Text & "', '" & txtLastName.Text _
        & "', '" & txtAddress.Text & "', '" & txtPostCode.Text & "', '" & txtPhone.Text & "')"

        myCmd.ExecuteNonQuery()
        Reset()
        LoadData()
        MessageBox.Show("Record Sucessfully Inserted")
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Try
            If myConn.State = ConnectionState.Open Then
                myConn.Close()
            End If

            myConn.Open()

            id = Convert.ToInt32(DataGridView1.SelectedCells.Item(0).Value.ToString())

            myCmd = myConn.CreateCommand()
            myCmd.CommandType = CommandType.Text
            myCmd.CommandText = "SELECT * FROM Students where StudentID=" & id & ""
            myCmd.ExecuteNonQuery()

            Dim myDataAdapter As New SqlDataAdapter(myCmd)
            Dim myDataTable As New DataTable
            myDataAdapter.Fill(myDataTable)

            Dim myDataReader As SqlClient.SqlDataReader
            myDataReader = myCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While myDataReader.Read

                txtFirstName.Text = myDataReader.GetString(1).ToString()
                txtLastName.Text = myDataReader.GetString(2).ToString()
                txtAddress.Text = myDataReader.GetString(3).ToString()
                txtPostCode.Text = myDataReader.GetString(4).ToString()
                txtPhone.Text = myDataReader.GetString(5).ToString()

            End While
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If myConn.State = ConnectionState.Open Then
            myConn.Close()
        End If

        myConn.Open()

        myCmd = myConn.CreateCommand()
        myCmd.CommandType = CommandType.Text
        myCmd.CommandText = "UPDATE Students SET Firstname='" + txtFirstName.Text +
        "',Lastname='" + txtLastName.Text + "',Address='" +
        txtAddress.Text + "',PostCode='" + txtPostCode.Text +
        "', Phone='" + txtPhone.Text + "' WHERE StudentID=" & id & ""
        myCmd.ExecuteNonQuery()

        LoadData()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If myConn.State = ConnectionState.Open Then
            myConn.Close()
        End If

        myConn.Open()

        myCmd = myConn.CreateCommand()
        myCmd.CommandType = CommandType.Text
        myCmd.CommandText = "DELETE FROM Students WHERE StudentID=" & id & ""
        myCmd.ExecuteNonQuery()

        LoadData()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Reset()
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Dim iExit As DialogResult

        iExit = MessageBox.Show("Confirm if you want to exit.",
        "MySQL Connector", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If iExit = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub
End Class
