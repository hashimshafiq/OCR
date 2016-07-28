<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.imgbox = New Emgu.CV.UI.ImageBox()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.btn_detect_webcam = New System.Windows.Forms.Button()
        Me.btn_detect_image = New System.Windows.Forms.Button()
        Me.ofdOpenFile = New System.Windows.Forms.OpenFileDialog()
        CType(Me.imgbox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'imgbox
        '
        Me.imgbox.Location = New System.Drawing.Point(13, 13)
        Me.imgbox.Name = "imgbox"
        Me.imgbox.Size = New System.Drawing.Size(605, 339)
        Me.imgbox.TabIndex = 2
        Me.imgbox.TabStop = False
        '
        'txtInfo
        '
        Me.txtInfo.Location = New System.Drawing.Point(13, 387)
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.Size = New System.Drawing.Size(605, 20)
        Me.txtInfo.TabIndex = 3
        '
        'btn_detect_webcam
        '
        Me.btn_detect_webcam.Location = New System.Drawing.Point(13, 358)
        Me.btn_detect_webcam.Name = "btn_detect_webcam"
        Me.btn_detect_webcam.Size = New System.Drawing.Size(299, 23)
        Me.btn_detect_webcam.TabIndex = 4
        Me.btn_detect_webcam.Text = "Detect from Webcam"
        Me.btn_detect_webcam.UseVisualStyleBackColor = True
        '
        'btn_detect_image
        '
        Me.btn_detect_image.Location = New System.Drawing.Point(318, 358)
        Me.btn_detect_image.Name = "btn_detect_image"
        Me.btn_detect_image.Size = New System.Drawing.Size(300, 23)
        Me.btn_detect_image.TabIndex = 5
        Me.btn_detect_image.Text = "Detect from Images"
        Me.btn_detect_image.UseVisualStyleBackColor = True
        '
        'ofdOpenFile
        '
        Me.ofdOpenFile.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(636, 419)
        Me.Controls.Add(Me.btn_detect_image)
        Me.Controls.Add(Me.btn_detect_webcam)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.imgbox)
        Me.Name = "Form1"
        Me.Text = "OCR"
        CType(Me.imgbox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents imgbox As Emgu.CV.UI.ImageBox
    Friend WithEvents txtInfo As TextBox
    Friend WithEvents btn_detect_webcam As Button
    Friend WithEvents btn_detect_image As Button
    Friend WithEvents ofdOpenFile As OpenFileDialog
End Class
