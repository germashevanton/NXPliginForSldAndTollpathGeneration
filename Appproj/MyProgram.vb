Option Strict Off
Imports System
Imports System.Windows
Imports NXOpen
Imports NXOpen.BlockStyler
Imports WpfApp1

Public Class dialog
    'class members
    Private Shared theSession As Session
    Private Shared theUI As UI
    Private theDlxFileName As String
    Private theDialog As NXOpen.BlockStyler.BlockDialog
    Private group0 As NXOpen.BlockStyler.Group ' Block type: Group
    Private enum0 As NXOpen.BlockStyler.Enumeration ' Block type: Enumeration
    Private face_select0 As NXOpen.BlockStyler.FaceCollector ' Block type: Face Collector
    Private enum01 As NXOpen.BlockStyler.Enumeration ' Block type: Enumeration
    Private toggle0 As NXOpen.BlockStyler.Toggle ' Block type: Toggle
    Private separator0 As NXOpen.BlockStyler.Separator ' Block type: Separator
    Private label0 As NXOpen.BlockStyler.Label ' Block type: Label
    Private double0 As NXOpen.BlockStyler.DoubleBlock ' Block type: Double
    Private double01 As NXOpen.BlockStyler.DoubleBlock ' Block type: Double
    Private double02 As NXOpen.BlockStyler.DoubleBlock ' Block type: Double

    Public Sub New()
        Try

            theSession = Session.GetSession()
            theUI = UI.GetUI()
            theDlxFileName = "dialog.dlx"
            theDialog = theUI.CreateDialog(theDlxFileName)
            theDialog.AddApplyHandler(AddressOf apply_cb)
            theDialog.AddOkHandler(AddressOf ok_cb)
            theDialog.AddUpdateHandler(AddressOf update_cb)
            theDialog.AddInitializeHandler(AddressOf initialize_cb)
            theDialog.AddDialogShownHandler(AddressOf dialogShown_cb)

        Catch ex As Exception

            Throw ex
        End Try
    End Sub
    Public Shared Sub Main()
        Dim thedialog As dialog = Nothing
        Try

            thedialog = New dialog()

            thedialog.Show()

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        Finally
            If thedialog IsNot Nothing Then
                thedialog.Dispose()
                thedialog = Nothing
            End If
        End Try
    End Sub

    Public Shared Function GetUnloadOption(ByVal arg As String) As Integer

        Return CType(Session.LibraryUnloadOption.Immediately, Integer)

    End Function

    Public Shared Sub UnloadLibrary(ByVal arg As String)
        Try


        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
    End Sub

    Public Sub Show()
        Try

            theDialog.Show()

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
    End Sub

    Public Sub Dispose()
        If theDialog IsNot Nothing Then
            theDialog.Dispose()
            theDialog = Nothing
        End If
    End Sub
    Public Sub initialize_cb()
        Try

            group0 = CType(theDialog.TopBlock.FindBlock("group0"), NXOpen.BlockStyler.Group)
            enum0 = CType(theDialog.TopBlock.FindBlock("enum0"), NXOpen.BlockStyler.Enumeration)
            face_select0 = CType(theDialog.TopBlock.FindBlock("face_select0"), NXOpen.BlockStyler.FaceCollector)
            enum01 = CType(theDialog.TopBlock.FindBlock("enum01"), NXOpen.BlockStyler.Enumeration)
            toggle0 = CType(theDialog.TopBlock.FindBlock("toggle0"), NXOpen.BlockStyler.Toggle)
            separator0 = CType(theDialog.TopBlock.FindBlock("separator0"), NXOpen.BlockStyler.Separator)
            label0 = CType(theDialog.TopBlock.FindBlock("label0"), NXOpen.BlockStyler.Label)
            double0 = CType(theDialog.TopBlock.FindBlock("double0"), NXOpen.BlockStyler.DoubleBlock)
            double01 = CType(theDialog.TopBlock.FindBlock("double01"), NXOpen.BlockStyler.DoubleBlock)
            double02 = CType(theDialog.TopBlock.FindBlock("double02"), NXOpen.BlockStyler.DoubleBlock)

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
    End Sub


    Public Sub dialogShown_cb()
        Try

            double0.Show = False
            double01.Show = False
            double02.Show = False
            separator0.Show = False
            label0.Show = False

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
    End Sub


    Public Function apply_cb() As Integer
        Dim errorCode As Integer = 0
        Try


            Dim workPart As Part = theSession.Parts.Work
            Dim diam As Integer
            Dim TeethNumber As Integer
            Dim SelectedMaterial As String = enum0.ValueAsString
            Dim SelectedTool As String = enum01.ValueAsString
            Dim Stiffness As Double = double0.Value
            Dim Damping As Double = double01.Value
            Dim Frequency As Double = double02.Value

            'set tool diametr and number of teeth
            If SelectedTool = "Square End Mill D16 z5" Then
                diam = 16
                TeethNumber = 5
            ElseIf SelectedTool = "Square End Mill D20 z6" Then
                diam = 20
                TeethNumber = 6
            ElseIf SelectedTool = "Square End Mill D12 z4" Then
                diam = 12
                TeethNumber = 4
            End If

            If toggle0.Value = True Then
                ' constructor params: diameter, radial depth of cut, teeth nimber, 
                ' stiffness, damping, natural frequency, material
                Dim view As Window = New MainWindow(diam, diam * 0.75, TeethNumber, Stiffness,
                                                    Damping, Frequency, SelectedMaterial)
                view.ShowDialog()
            End If

            'getting faces/edges
            Dim i As Integer
            Dim one(10) As Point3d
            Dim two(10) As Point3d
            For Each tempObj As TaggedObject In face_select0.GetSelectedObjects

                Dim tempFace As Face = CType(tempObj, Face)

                For Each tempEdg As Edge In tempFace.GetEdges

                    tempEdg.GetVertices(one(i), two(i))
                    i = i + 1
                Next

                tempFace.GetEdges()

            Next

            'getting points

            Dim A As Point3d
            Dim B As Point3d
            Dim C As New Point3d
            Dim D As Point3d





            Dim maxX(2) As Point3d
            Dim minX(2) As Point3d
            Dim indMax As Integer = 0
            Dim indMin As Integer = 0
            'x axis
            If two(0).X >= two(1).X And two(0).X >= two(2).X And two(0).X >= two(3).X Then
                maxX(indMax) = two(0)
                indMax = +1
            Else
                minX(indMin) = two(0)
                indMin = +1
            End If

            If two(1).X >= two(0).X And two(1).X >= two(2).X And two(1).X >= two(3).X Then
                maxX(indMax) = two(1)
                indMax = +1
            Else
                minX(indMin) = two(1)
                indMin = +1
            End If

            If two(2).X >= two(0).X And two(2).X >= two(1).X And two(2).X >= two(3).X Then
                maxX(indMax) = two(2)
                indMax = +1
            Else
                minX(indMin) = two(2)
                indMin = +1
            End If

            If two(3).X >= two(0).X And two(3).X >= two(2).X And two(3).X >= two(1).X Then
                maxX(indMax) = two(3)
                indMax = +1
            Else
                minX(indMin) = two(3)
                indMin = +1
            End If
            'y axis

            Dim maxY(2) As Point3d
            Dim minY(2) As Point3d
            Dim indMaxY As Integer = 0
            Dim indMinY As Integer = 0
            If two(0).Y >= two(1).Y And two(0).Y >= two(2).Y And two(0).Y >= two(3).Y Then
                maxY(indMaxY) = two(0)
                indMaxY = +1
            Else
                minY(indMinY) = two(0)
                indMinY = +1
            End If

            If two(1).Y >= two(0).Y And two(1).Y >= two(2).Y And two(1).Y >= two(3).Y Then
                maxY(indMaxY) = two(1)
                indMaxY = +1
            Else
                minY(indMinY) = two(1)
                indMinY = +1
            End If

            If two(2).Y >= two(0).Y And two(2).Y >= two(1).Y And two(2).Y >= two(3).Y Then
                maxY(indMaxY) = two(2)
                indMaxY = +1
            Else
                minY(indMinY) = two(2)
                indMinY = +1
            End If

            If two(3).Y >= two(0).Y And two(3).Y >= two(2).Y And two(3).Y >= two(1).Y Then
                maxY(indMaxY) = two(3)
                indMaxY = +1
            Else
                minX(indMinY) = two(3)
                indMinY = +1
            End If

            'point A
            If minX(0).X = maxY(0).X And minX(0).Y = maxY(0).Y Then
                A = minX(0)
            End If
            If minX(1).X = maxY(1).X And minX(1).Y = maxY(1).Y Then
                A = minX(1)
            End If
            If minX(0).X = maxY(1).X And minX(0).Y = maxY(1).Y Then
                A = minX(0)
            End If
            If minX(1).X = maxY(0).X And minX(1).Y = maxY(0).Y Then
                A = minX(1)
            End If

            'point B
            If minX(0).X = minY(0).X And minX(0).Y = minY(0).Y Then
                B = minX(0)
            End If
            If minX(1).X = minY(1).X And minX(1).Y = minY(1).Y Then
                B = minX(1)
            End If
            If minX(0).X = minY(1).X And minX(0).Y = minY(1).Y Then
                B = minX(0)
            End If
            If minX(1).X = minY(0).X And minX(1).Y = minY(0).Y Then
                minX(1) = B
            End If

            'point C
            If maxX(0).X = minY(0).X And maxX(0).Y = minY(0).Y Then
                C = maxX(0)
            End If
            If maxX(1).X = minY(1).X And maxX(1).Y = minY(1).Y Then
                C = maxX(1)
            End If
            If maxX(0).X = minY(1).X And maxX(0).Y = minY(1).Y Then
                C = maxX(0)
            End If
            If maxX(1).X = minY(0).X And maxX(1).Y = minY(0).Y Then
                C = maxX(1)
            End If

            'point D
            If maxX(0).X = maxY(0).X And maxX(0).Y = maxY(0).Y Then
                D = maxX(0)
            End If
            If maxX(1).X = maxY(1).X And maxX(1).Y = maxY(1).Y Then
                D = maxX(1)
            End If
            If maxX(0).X = maxY(1).X And maxX(0).Y = maxY(1).Y Then
                D = maxX(0)
            End If
            If maxX(1).X = maxY(0).X And maxX(1).Y = maxY(0).Y Then
                D = maxX(1)
            End If


            Dim EndTr As Double
            Dim EndTr1 As Double
            Dim index As Integer = 0
            Dim k As Integer = 1
            Dim kk As Double
            If SelectedMaterial = "Titanium." Then

                'Building trajectory
                'begin cutting
                Dim pt33 As New Point3d(C.X + (1.5 * diam), C.Y + (1.5 * diam), C.Z + 7)
                Dim pt44 As New Point3d(C.X, C.Y + (0.15 * diam), C.Z + 7)
                Dim l11 As NXOpen.Line = workPart.Curves.CreateLine(pt33, pt44)
                l11.SetVisibility(SmartObject.VisibilityOption.Visible)
                Dim S11 As New Point3d(C.X, C.Y + (0.2 * diam), C.Z + 7)
                Dim F11 As New Point3d(C.X - (0.2 * diam), C.Y, C.Z + 7)
                Dim centr11 As New Point3d(C.X - (0.365 * diam), C.Y + (0.365 * diam), C.Z + 7)
                Dim arc11 As NXOpen.Arc = workPart.Curves.CreateArc(S11, centr11, F11, True, False)

                Dim psi As Integer

                'cycle

                While k = 1
                    kk = ((diam / 2) * index)
                    Dim l(10) As NXOpen.Line
                    Dim cur(10) As NXOpen.Arc
                    If index = 0 Then
                        psi = 0
                    ElseIf index > 0 Then
                        psi = 1
                    End If

                    Dim pt3 As New Point3d(C.X - (0.2 * diam) - kk + ((diam / 2 * psi)), C.Y + kk, C.Z + 7)
                    Dim pt4 As New Point3d(B.X + (0.2 * diam) + kk, B.Y + kk, B.Z + 7)
                    l(1) = workPart.Curves.CreateLine(pt3, pt4)
                    l(1).SetVisibility(SmartObject.VisibilityOption.Visible)
                    Dim S1 As New Point3d(B.X + (0.2 * diam) + kk, B.Y + kk, B.Z + 7)
                    Dim F1 As New Point3d(B.X + kk, B.Y + (0.2 * diam) + kk, B.Z + 7)
                    Dim centr1 As New Point3d(B.X + (0.365 * diam) + kk, B.Y + (0.365 * diam) + kk, B.Z + 7)
                    cur(1) = workPart.Curves.CreateArc(S1, centr1, F1, True, False)


                    Dim pt5 As New Point3d(B.X + kk, B.Y + (0.2 * diam) + kk, B.Z + 7)
                    Dim pt6 As New Point3d(A.X + kk, A.Y - (0.2 * diam) - kk, A.Z + 7)
                    l(2) = workPart.Curves.CreateLine(pt5, pt6)
                    l(2).SetVisibility(SmartObject.VisibilityOption.Visible)
                    Dim S2 As New Point3d(A.X + kk, A.Y - (0.2 * diam) - kk, A.Z + 7)
                    Dim F2 As New Point3d(A.X + (0.2 * diam) + kk, A.Y - kk, A.Z + 7)
                    Dim centr2 As New Point3d(A.X + (0.365 * diam) + kk, A.Y - (0.365 * diam) - kk, A.Z + 7)
                    cur(3) = workPart.Curves.CreateArc(S2, centr2, F2, True, False)

                    Dim pt7 As New Point3d(A.X + (0.2 * diam) + kk, A.Y - kk, A.Z + 7)
                    Dim pt8 As New Point3d(D.X - (0.2 * diam) - kk, D.Y - kk, D.Z + 7)
                    l(3) = workPart.Curves.CreateLine(pt7, pt8)
                    l(3).SetVisibility(SmartObject.VisibilityOption.Visible)
                    Dim S3 As New Point3d(D.X - (0.2 * diam) - kk, D.Y - kk, D.Z + 7)
                    Dim F3 As New Point3d(D.X - kk, D.Y - (0.2 * diam) - kk, D.Z + 7)
                    Dim centr3 As New Point3d(D.X - (0.365 * diam) - kk, D.Y - (0.365 * diam) - kk, D.Z + 7)
                    cur(4) = workPart.Curves.CreateArc(S3, centr3, F3, True, False)


                    Dim pt9 As New Point3d(D.X - kk, D.Y - (0.2 * diam) - kk, D.Z + 7)
                    Dim off As New Point3d(C.X - kk, C.Y + (0.2 * diam) + (0.365 * diam) + kk + 1.35, C.Z + 7)
                    l(4) = workPart.Curves.CreateLine(pt9, off)
                    l(4).SetVisibility(SmartObject.VisibilityOption.Visible)
                    Dim S As New Point3d(C.X - kk, C.Y + (0.2 * diam) + (0.365 * diam) + kk + 1.35, C.Z + 7)
                    Dim F As New Point3d(C.X - (0.2 * diam) - kk, C.Y + (diam / 2) + kk, C.Z + 7)
                    Dim centr As New Point3d(C.X - (0.365 * diam) - kk, C.Y + (0.865 * diam) + kk, C.Z + 7)
                    cur(0) = workPart.Curves.CreateArc(S, centr, F, True, False)

                    EndTr = l(4).GetLength
                    EndTr1 = l(3).GetLength

                    If EndTr < 8 Or EndTr1 < 8 Then Exit While
                    index = index + 1.0

                End While

            ElseIf SelectedMaterial = "Aluminum." Then

                While index < 10

                    kk = diam * index
                    Dim l(10) As NXOpen.Line

                    Dim pt1 As New Point3d(C.X + diam + 2, C.Y + (diam / 2) + kk, C.Z + 7)
                    Dim pt2 As New Point3d(B.X - diam - 1, B.Y + (diam / 2) + kk, B.Z + 7)
                    l(0) = workPart.Curves.CreateLine(pt1, pt2)
                    l(0).SetVisibility(SmartObject.VisibilityOption.Visible)

                    Dim pt3 As New Point3d(B.X - diam - 1, B.Y + (diam / 2) + kk, B.Z + 15)
                    l(1) = workPart.Curves.CreateLine(pt2, pt3)
                    l(1).SetVisibility(SmartObject.VisibilityOption.Visible)

                    index = index + 1
                    kk = diam * index
                    Dim pt4 As New Point3d(C.X + diam + 2, C.Y + (diam / 2) + kk, C.Z + 15)
                    l(2) = workPart.Curves.CreateLine(pt3, pt4)
                    l(2).SetVisibility(SmartObject.VisibilityOption.Visible)

                    Dim pt5 As New Point3d(C.X + diam + 2, C.Y + (diam / 2) + kk, C.Z + 7)
                    l(3) = workPart.Curves.CreateLine(pt4, pt5)
                    l(3).SetVisibility(SmartObject.VisibilityOption.Visible)

                    Dim pt11 As New Point3d(two(2).X + diam + 2, two(2).Y, two(2).Z + 7)
                    l(4) = workPart.Curves.CreateLine(pt1, pt11)
                    l(4).SetVisibility(SmartObject.VisibilityOption.Invisible)
                    EndTr = l(4).GetLength

                    If EndTr < diam Then Exit While


                End While

            End If


        Catch ex As Exception
            errorCode = 1
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
        apply_cb = errorCode
    End Function

    Public Function update_cb(ByVal block As NXOpen.BlockStyler.UIBlock) As Integer
        Try

            If block Is enum0 Then

            ElseIf block Is face_select0 Then

            ElseIf block Is enum01 Then

            ElseIf block Is toggle0 Then
                If toggle0.Value = True Then
                    double0.Show = True
                ElseIf toggle0.Value = False Then
                    double0.Show = False
                End If

                If toggle0.Value = True Then
                    double01.Show = True
                ElseIf toggle0.Value = False Then
                    double01.Show = False
                End If

                If toggle0.Value = True Then
                    double02.Show = True
                ElseIf toggle0.Value = False Then
                    double02.Show = False
                End If

                If toggle0.Value = True Then
                    label0.Show = True
                ElseIf toggle0.Value = False Then
                    label0.Show = False
                End If

                If toggle0.Value = True Then
                    separator0.Show = True
                ElseIf toggle0.Value = False Then
                    separator0.Show = False
                End If

            ElseIf block Is separator0 Then

            ElseIf block Is label0 Then

            ElseIf block Is double0 Then

            ElseIf block Is double01 Then

            ElseIf block Is double02 Then

            End If

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
        update_cb = 0
    End Function

    Public Function ok_cb() As Integer
        Dim errorCode As Integer = 0
        Try

            errorCode = apply_cb()

        Catch ex As Exception

            errorCode = 1
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
        ok_cb = errorCode
    End Function

    Public Function GetBlockProperties(ByVal blockID As String) As PropertyList
        GetBlockProperties = Nothing
        Try

            GetBlockProperties = theDialog.GetBlockProperties(blockID)

        Catch ex As Exception

            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString)
        End Try
    End Function

End Class
