Option Explicit On
Option Strict On
Imports System.IO
Imports System.Xml.Serialization
Imports Emgu.CV
Imports Emgu.CV.CvEnum
Imports Emgu.CV.ML
Imports Emgu.CV.Structure
Imports Emgu.CV.UI
Imports Emgu.CV.Util

Public Class Form1
    Dim capWebcam As Capture
    Const RESIZED_IMAGE_WIDTH As Integer = 20
    Const RESIZED_IMAGE_HEIGHT As Integer = 30
    Dim img As Mat
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            capWebcam = New Capture(0)


        Catch ex As Exception
            MessageBox.Show("Unable to read from webcam, error: " + Environment.NewLine + Environment.NewLine + ex.Message _
                + Environment.NewLine + Environment.NewLine + "existing program")
            Environment.Exit(0)
        End Try
        AddHandler Application.Idle, New EventHandler(AddressOf Me.ProcessFrameAndUpdateGUI)
    End Sub

    Sub ProcessFrameAndUpdateGUI(sender As Object, arg As EventArgs)
        Dim img As Mat

        img = capWebcam.QueryFrame()
        imgbox.Image = img






    End Sub

    Private Sub btn_detect_webcam_Click(sender As Object, e As EventArgs) Handles btn_detect_webcam.Click
        Dim img As Mat

        img = capWebcam.QueryFrame()
        imgbox.Image = img
        OCRfunction(img)



    End Sub

    Private Sub btn_detect_image_Click(sender As Object, e As EventArgs) Handles btn_detect_image.Click
        Dim drChosenFile As DialogResult

        drChosenFile = ofdOpenFile.ShowDialog()

        If (drChosenFile <> DialogResult.OK Or ofdOpenFile.FileName = "") Then
            txtInfo.Text = "file not chosen"
            Return

        End If

        'declare the input image
        Dim imgTestingNumbers As Mat

        Try
            'open image
            imgTestingNumbers = CvInvoke.Imread(ofdOpenFile.FileName, LoadImageType.Color)
        Catch ex As Exception
            txtInfo.Text = "unable to open image, error: " + ex.Message
            Return
        End Try

        'if image could not be opened
        If (imgTestingNumbers Is Nothing) Then
            txtInfo.Text = "unable to open image"
            Return
        End If

        If (imgTestingNumbers.IsEmpty()) Then
            txtInfo.Text = "unable to open image"
            Return
        End If

        OCRfunction(imgTestingNumbers)
        CvInvoke.Imshow("imgTestingNumbers", imgTestingNumbers)

    End Sub

    Sub OCRfunction(img As Mat)
        Dim mtxClassifications As Matrix(Of Single) = New Matrix(Of Single)(1, 1)
        Dim mtxTrainingImages As Matrix(Of Single) = New Matrix(Of Single)(1, 1)

        Dim intValidChars As New List(Of Integer)(New Integer() {Asc("0"), Asc("1"), Asc("2"), Asc("3"), Asc("4"), Asc("5"), Asc("6"), Asc("7"), Asc("8"), Asc("9"),
                                                                  Asc("A"), Asc("B"), Asc("C"), Asc("D"), Asc("E"), Asc("F"), Asc("G"), Asc("H"), Asc("I"), Asc("J"),
                                                                  Asc("K"), Asc("L"), Asc("M"), Asc("N"), Asc("O"), Asc("P"), Asc("Q"), Asc("R"), Asc("S"), Asc("T"),
                                                                  Asc("U"), Asc("V"), Asc("W"), Asc("X"), Asc("Y"), Asc("Z"), Asc("."), Asc("("), Asc(")"),
                                                                  Asc("a"), Asc("b"), Asc("c"), Asc("d"), Asc("e"), Asc("f"), Asc("g"), Asc("h"), Asc("i"), Asc("j"),
                                                                  Asc("k"), Asc("l"), Asc("m"), Asc("n"), Asc("o"), Asc("p"), Asc("q"), Asc("r"), Asc("s"), Asc("t"),
                                                                  Asc("u"), Asc("v"), Asc("w"), Asc("x"), Asc("y"), Asc("z")})

        'these variables are for reading from the XML files
        Dim xmlSerializer As XmlSerializer = New XmlSerializer(mtxClassifications.GetType)
        Dim streamReader As StreamReader

        Try
            streamReader = New StreamReader("classifications.xml")
        Catch ex As Exception
            txtInfo.AppendText(vbCrLf + "unable to open 'classifications.xml', error: ")
            txtInfo.AppendText(ex.Message + vbCrLf)
            Return
        End Try
        'read from the classifications file the 1st time, this is only to get the number of rows, not the actual data
        mtxClassifications = CType(xmlSerializer.Deserialize(streamReader), Matrix(Of Single))

        streamReader.Close()

        'get the number of rows, i.e. the number of training samples
        Dim intNumberOfTrainingSamples As Integer = mtxClassifications.Rows

        'now that we know the number of rows, reinstantiate classifications Matrix and training images Matrix with 
        'the actual number of rows
        mtxClassifications = New Matrix(Of Single)(intNumberOfTrainingSamples, 1)
        mtxTrainingImages = New Matrix(Of Single)(intNumberOfTrainingSamples, RESIZED_IMAGE_WIDTH * RESIZED_IMAGE_HEIGHT)

        Try
            streamReader = New StreamReader("classifications.xml")
        Catch ex As Exception
            txtInfo.AppendText(vbCrLf + "unable to open 'classifications.xml', error:" + vbCrLf)
            txtInfo.AppendText(ex.Message + vbCrLf + vbCrLf)
            Return
        End Try
        'read from the classifications file again, this time we can get the actual data
        mtxClassifications = CType(xmlSerializer.Deserialize(streamReader), Matrix(Of Single))

        streamReader.Close()                'close the classifications XML file

        'reinstantiate file reading variable
        xmlSerializer = New XmlSerializer(mtxTrainingImages.GetType)

        Try
            streamReader = New StreamReader("images.xml")
        Catch ex As Exception
            txtInfo.AppendText("unable to open 'images.xml', error:" + vbCrLf)
            txtInfo.AppendText(ex.Message + vbCrLf + vbCrLf)
        End Try

        'read from training images file
        mtxTrainingImages = CType(xmlSerializer.Deserialize(streamReader), Matrix(Of Single))
        streamReader.Close()

        ' train '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim kNearest As New KNearest()

        kNearest.DefaultK = 1

        kNearest.Train(mtxTrainingImages, MlEnum.DataLayoutType.RowSample, mtxClassifications)

        ' test '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'declare the input image
        Dim imgTestingNumbers As Mat = img

        Dim imgGrayscale As New Mat()
        Dim imgBlurred As New Mat()
        Dim imgThresh As New Mat()
        Dim imgThreshCopy As New Mat()

        'convert to grayscale
        CvInvoke.CvtColor(imgTestingNumbers, imgGrayscale, ColorConversion.Bgr2Gray)
        'blur
        CvInvoke.GaussianBlur(imgGrayscale, imgBlurred, New Size(5, 5), 0)

        'threshold image from grayscale to black and white
        CvInvoke.AdaptiveThreshold(imgBlurred, imgThresh, 255.0, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 11, 2.0)
        'make a copy of the thresh image, this in necessary b/c findContours modifies the image
        imgThreshCopy = imgThresh.Clone()

        Dim contours As New VectorOfVectorOfPoint()

        'get external countours only
        CvInvoke.FindContours(imgThreshCopy, contours, Nothing, RetrType.External, ChainApproxMethod.ChainApproxSimple)
        'declare a list of contours with data
        Dim listOfContoursWithData As New List(Of ContourWithData)

        'populate list of contours with data
        For i As Integer = 0 To contours.Size - 1                   'for each contour
            Dim contourWithData As New ContourWithData                              'declare new contour with data
            contourWithData.contour = contours(i)                                                   'populate contour member variable
            contourWithData.boundingRect = CvInvoke.BoundingRectangle(contourWithData.contour)      'calculate bounding rectangle
            contourWithData.dblArea = CvInvoke.ContourArea(contourWithData.contour)                 'calculate area
            If (contourWithData.checkIfContourIsValid()) Then                                       'if contour with data is valis
                listOfContoursWithData.Add(contourWithData)                                         'add to list of contours with data
            End If
        Next
        'sort contours with data from left to right
        listOfContoursWithData.Sort(Function(oneContourWithData, otherContourWithData) oneContourWithData.boundingRect.X.CompareTo(otherContourWithData.boundingRect.X))

        Dim strFinalString As String = ""           'declare final string, this will have the final number sequence by the end of the program

        For Each contourWithData As ContourWithData In listOfContoursWithData               'for each contour in list of valid contours

            CvInvoke.Rectangle(imgTestingNumbers, contourWithData.boundingRect, New MCvScalar(0.0, 255.0, 0.0), 2)      'draw green rect around the current char

            Dim imgROItoBeCloned As New Mat(imgThresh, contourWithData.boundingRect)        'get ROI image of bounding rect

            Dim imgROI As Mat = imgROItoBeCloned.Clone()                'clone ROI image so we don't change original when we resize

            Dim imgROIResized As New Mat()

            'resize image, this is necessary for char recognition
            CvInvoke.Resize(imgROI, imgROIResized, New Size(RESIZED_IMAGE_WIDTH, RESIZED_IMAGE_HEIGHT))

            'declare a Matrix of the same dimensions as the Image we are adding to the data structure of training images
            Dim mtxTemp As Matrix(Of Single) = New Matrix(Of Single)(imgROIResized.Size())

            'declare a flattened (only 1 row) matrix of the same total size
            Dim mtxTempReshaped As Matrix(Of Single) = New Matrix(Of Single)(1, RESIZED_IMAGE_WIDTH * RESIZED_IMAGE_HEIGHT)

            imgROIResized.ConvertTo(mtxTemp, DepthType.Cv32F)           'convert Image to a Matrix of Singles with the same dimensions

            For intRow As Integer = 0 To RESIZED_IMAGE_HEIGHT - 1       'flatten Matrix into one row by RESIZED_IMAGE_WIDTH * RESIZED_IMAGE_HEIGHT number of columns
                For intCol As Integer = 0 To RESIZED_IMAGE_WIDTH - 1
                    mtxTempReshaped(0, (intRow * RESIZED_IMAGE_WIDTH) + intCol) = mtxTemp(intRow, intCol)
                Next
            Next

            Dim sngCurrentChar As Single

            sngCurrentChar = kNearest.Predict(mtxTempReshaped)              'finally we can call Predict !!!

            strFinalString = strFinalString + Chr(Convert.ToInt32(sngCurrentChar))          'append current char to full string of chars

        Next
        If (strFinalString <> "") Then
            'txtInfo.AppendText(vbCrLf + vbCrLf + "characters read from image = " + strFinalString + vbCrLf)
            txtInfo.Text = vbCrLf + vbCrLf + "characters read from image = " + strFinalString + vbCrLf

        End If



    End Sub

End Class
