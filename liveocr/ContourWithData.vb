Option Explicit On
Option Strict On

Imports Emgu.CV.Util


Public Class ContourWithData


    Const MIN_CONTOUR_AREA As Integer = 100
    'contour
    Public contour As VectorOfPoint
    'bounding rect for contour
    Public boundingRect As Rectangle
    'area of contour
    Public dblArea As Double

    'this is oversimplified, for a production grade program better validity checking would be necessary
    Public Function checkIfContourIsValid() As Boolean
        If (dblArea < MIN_CONTOUR_AREA) Then
            Return False
        Else
            Return True
        End If
    End Function

End Class