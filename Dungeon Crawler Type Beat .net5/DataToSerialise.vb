Public Class DataToSerialise
    Public enemyentry As List(Of String)
    Public maginv As List(Of String)
    Public iteminv As List(Of String)
    Public inv As List(Of String)
    Public pmana As Integer
    Public maxpmana As Integer
    Public maxphealth As Integer
    Public phealth As Integer
    Public pname As String
    Public floor As Integer
    Public section As Integer

    Public Sub New(enemyentry As List(Of String), maginv As List(Of String), iteminv As List(Of String), inv As List(Of String), pmana As Integer, maxpmana As Integer, maxphealth As Integer, phealth As Integer, pname As String, floor As Integer, section As Integer)
        Me.enemyentry = enemyentry
        Me.maginv = maginv
        Me.iteminv = iteminv
        Me.inv = inv
        Me.pmana = pmana
        Me.maxpmana = maxpmana
        Me.maxphealth = maxphealth
        Me.phealth = phealth
        Me.pname = pname
        Me.floor = floor
        Me.section = section
    End Sub
End Class


'enemyentry maginv iteminv inv pmanabonus phealthbonus pdefencebonus pattackbonus defbonus
'dambonus pmana maxpmana maxphealth phealth pname floor section