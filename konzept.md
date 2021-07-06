# Das Grundkonzept des Passwortsafes
Edo Husakovic und Walter Schneider
## Beschreibung
Der Zweck dieses Projekts besteht darin, eine Kennwortsicherheitsanwendung zu erstellen. Der Benutzer sollte die Möglichkeit haben verschiedene Passwörter, 
für verschiedene Konten sicher zu verschlüsseln. Jeder Benutzer sollte ein eigenes Passwort auch ansehen können.

## Technologie
Wir haben eine .NET 5.0 Installation und unser Frontend wird mit Razor von .NET gemacht.

## Bestätigung
Der Benutzer erstellt ein Konto und kann sich später einloggen. Solange er ein Konto hat, kann er es verwenden.
Es speichert auch die Passwörter seiner anderen Konten. Der Benutzer benötigt einen Benutzernamen und ein Passwort, welche er bei der Registrierung selber definiert hat.
##Verschlüsselung
Das beim Erstellen des Benutzerkontos erstellte Passwort wird gehasht und gespeichert.
Der Passwort-Hash wird später als symmetrischer Schlüssel für den Passwort-Safe verwendet. Die Passwörter im Safe sind mit einem Hash verschlüsselt, sobald sich der Benutzer anmeldet, werden diese für ihn mithilfe des passwort hashes entschlüsselt. 
Ändert der Benutzer sein Passwort, werden alle PWs im Safe mit dem alten Passwort entschlüsselt und anschließend mit dem neuen Passwort verschlüsselt.

## Security Konzept
Um einen Benutzer zu überprüfen wird ein HMACSHA1 Hash benutzt. Nach der Eingabe werden die E-Mail und das Password an den Server gesendet. 
Dort wird das Password mit dem Salt gehashed und mit dem Hash in der Datenbank verglichen. Nach der Bestätigung werden die Login Informationen auf dem Client gespeichert.
Der Client sendet die Login Informationen bei jedem Request mit und wird nachher im Backend bentutzt um die Passwörter zu entschlüsseln.
Die gespeicherten Passwörter haben eine andere Verschlüsselungs Art als das Benutzer Passwort. Das Passwort des Benutzers wird überprüft, ob die Login Daten stimmen und falls ja, werden die Passwörter entschlüsselt. Der SHA512CryptoServiceProvider wird angewendet um diese Verschlüsselung auszuführen.
