Imports System
Imports System.IO

Module Program
    Sub Main(args As String())

        Dim stopwatch As Stopwatch = Stopwatch.StartNew()

        ' make dummy data
        Console.WriteLine(vbCrLf + "Creating dummy data")
        stopwatch.Start()
        createDummyData()
        stopwatch.Stop()
        Console.WriteLine("Size: {0}", allData("Voltage1").Length)
        Console.WriteLine("Original code finished. ETA: " + stopwatch.Elapsed.TotalSeconds.ToString() + "s")

        ' Method Original
        Console.WriteLine(vbCrLf + "Ready to run the original code, enter y to continue")
        Dim name = Console.ReadLine()
        If name = "y" Then
            stopwatch.Start()
            originalSaveCsv()
            stopwatch.Stop()
            Console.WriteLine("Original code finished. ETA: " + stopwatch.Elapsed.TotalSeconds.ToString() + "s")
        Else
            Console.WriteLine("Original code test skipped.")
        End If

        'Method 1
        Console.WriteLine(vbCrLf + "Ready to run method #1, enter y to continue")
        name = Console.ReadLine()
        If name = "y" Then
            stopwatch.Start()
            newSaveCsv1()
            stopwatch.Stop()
            Console.WriteLine("Original code finiashed. ETA: " + stopwatch.Elapsed.TotalSeconds.ToString() + "s")
        Else
            Console.WriteLine("Original code test skipped.")
        End If

        'Method 2
        Console.WriteLine(vbCrLf + "Ready to run method #2, enter y to continue")
        name = Console.ReadLine()
        If name = "y" Then
            stopwatch.Start()
            newSaveCsv2()
            stopwatch.Stop()
            Console.WriteLine("Original code finiashed. ETA: " + stopwatch.Elapsed.TotalSeconds.ToString() + "s")
        Else
            Console.WriteLine("Original code test skipped.")
        End If
    End Sub

    Public buffersToRecord() As String = {"Voltage1", "Voltage2", "Voltage3",
        "Current1", "Current2", "Current3", "Speed"}
    Public allData As New Dictionary(Of String, Double())

    Sub createDummyData()
        Dim dummySignal1(10) As Double
        Dim dummySignal2(2) As Double
        Dim dummySignalTemp1 As List(Of Double) = New List(Of Double)
        Dim dummySignalTemp2 As List(Of Double) = New List(Of Double)

        dummySignal1 = {1 / 3, 2 / 3, 7 / 3, 1 / 7, 5 / 7, 2 / 19, 40 / 21, 8 / 7, 14 / 13, 5}
        dummySignal2 = {3.11, 3.12}
        'The actual data contains 3101130 points x 6. Took about 10 mins to write
        For i = 1 To 3101130 / 10
            dummySignalTemp1.AddRange(dummySignal1)
            dummySignalTemp2.AddRange(dummySignal2)
        Next

        Dim dummySignal3 As Double() = dummySignalTemp1.ToArray
        Dim dummySignal4 As Double() = dummySignalTemp2.ToArray

        allData.Add("Voltage1", dummySignal3)
        allData.Add("Voltage2", dummySignal3)
        allData.Add("Voltage3", dummySignal3)
        allData.Add("Current1", dummySignal3)
        allData.Add("Current2", dummySignal3)
        allData.Add("Current3", dummySignal3)
        allData.Add("Speed", dummySignal4)
    End Sub

    Sub newSaveCsv1()
        'Saving each row as a singal, like a regular csv transposed.
        'Since each row is too long, it is not usable.
        Dim sb As New Text.StringBuilder

        For i = 0 To buffersToRecord.Count - 1
            Dim doubleArray As Double() = allData(buffersToRecord(i))
            'Convert Double to String
            'Dim strArray As String() = Array.ConvertAll(Of Double, String)(doubleArray, Function(x) x.ToString())
            'String Array -> String
            sb.Append(String.Join(Environment.NewLine, doubleArray))
            sb.AppendLine("")
        Next

        Dim outputFile As String = Directory.GetCurrentDirectory() + "\Output_method1_" +
            DateTime.Now.ToString("yyyMMddhhmmss") + ".csv"
        File.WriteAllText(outputFile, sb.ToString())
    End Sub

    Sub newSaveCsv2()
        'Saving channel as a separate csv file.
        Dim sb As New Text.StringBuilder
        Dim outputFolder As String = Directory.GetCurrentDirectory() +
                "\Output_method2_" + DateTime.Now.ToString("yyyMMddhhmmss") + "\"
        'make folders for the files
        If Not System.IO.Directory.Exists(outputFolder) Then
            System.IO.Directory.CreateDirectory(outputFolder)
        End If

        For i = 0 To buffersToRecord.Count - 1

            Dim outputFile As String = outputFolder + buffersToRecord(i) + ".csv"

            Dim doubleArray As Double() = allData(buffersToRecord(i))
            'Convert Double to String
            'Dim strArray As String() = Array.ConvertAll(Of Double, String)(doubleArray, Function(x) x.ToString())
            File.WriteAllText(outputFile, String.Join(Environment.NewLine, doubleArray))
        Next

    End Sub

    Sub originalSaveCsv()
        Dim sb As New Text.StringBuilder
        Dim maxSamples As Integer
        maxSamples = allData("Voltage1").Count

        'Column Names & MAX Value, omitted

        'list of doubles
        For x = 0 To maxSamples - 1
            For y = 0 To buffersToRecord.Count - 1
                Dim data As Double() = allData(buffersToRecord(y))
                If y = buffersToRecord.Count - 1 Then
                    If x <= data.Count - 1 Then
                        sb.AppendLine(data(x))
                    Else
                        sb.AppendLine("")
                    End If
                Else
                    If x <= data.Count - 1 Then
                        sb.Append(data(x) & ",")
                    Else
                        sb.Append(",")
                    End If
                End If
            Next
        Next
        Dim outputFile As String = Directory.GetCurrentDirectory() + "\Output_original_" +
            DateTime.Now.ToString("yyyMMddhhmmss") + ".csv"
        File.WriteAllText(outputFile, sb.ToString)
    End Sub
End Module
