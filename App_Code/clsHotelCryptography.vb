Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class clsHotelCryptography
    Public Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "MOHDALHUMAIDICOMPUTERDUBAI082017"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H4D, &H61, &H68, &H63, &H65, &H20, &H56, &H65, &H6E, &H6B, &H61, &H74, &H72, _
                                                                         &H61, &H6D, &H61, &H6E, &H20, &H44, &H75, &H62, &H61, &H69})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
                clearText = HttpUtility.UrlEncode(clearText)
            End Using
        End Using
        Return clearText
    End Function

    Public Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "MOHDALHUMAIDICOMPUTERDUBAI082017"
        cipherText = HttpUtility.UrlDecode(cipherText)
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H4D, &H61, &H68, &H63, &H65, &H20, &H56, &H65, &H6E, &H6B, &H61, &H74, &H72, _
                                                                         &H61, &H6D, &H61, &H6E, &H20, &H44, &H75, &H62, &H61, &H69})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
End Class
