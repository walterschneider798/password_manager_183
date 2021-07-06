# Das Grundkonzept des Passwortsafes
Edo Husakovic und Walter Schneider
## Beschreibung
Der Zweck dieses Projekts besteht darin, eine Kennwortsicherheitsanwendung zu erstellen. Der Benutzer sollte sein Passwort haben
Möglichkeit, sie für verschiedene Konten sicher zu verschlüsseln. Benutzer sollten ein eigenes Passwort haben
Sie können immer noch zuschauen.

## Technologie
Wir haben eine .NET 5.0 Installation und unsere Frontend wird mit Razor von .NET gemacht.

## Bestätigung
Der Benutzer erstellt ein Konto und kann sich später einloggen. Solange er ein Konto hat, kann er es verwenden
Es speichert auch die Passwörter seiner anderen Konten. Der Benutzer benötigt einen Benutzernamen und ein Passwort,
Er hat sich bei der Registrierung selbst definiert.
##Verschlüsselung
Das beim Erstellen des Benutzerkontos erstellte Passwort wird gehasht und gespeichert. Das beim Erstellen des Benutzerkontos erstellte Passwort wird gehasht und gespeichert.
Der Passwort-Hash wird später als symmetrischer Schlüssel für den Passwort-Safe verwendet. Das Passwort im Safe ist mit einem Hash verschlüsselt, bei der Anmeldung des Benutzers wird das Passwort für ihn entschlüsselt.
Ändert der Benutzer sein Passwort, werden alle PWs im Safe mit dem alten Passwort entschlüsselt und anschließend mit dem neuen Passwort verschlüsselt.

## Security Konzept
Um Users zu überprüfen wird ein HMACSHA1 Hash benutzt. Nach der Eingabe werden der E-Mail und Password an den Server gesendet. 
Dort wird der Password mit dem Salt gehashed und mit dem Hash in der Datenbank vergliechen. Nach der Bestätigung werden die Login Informationen auf dem Client gespeichert.
Der Client sendet die Login Informationen bei jedem Request mit und wird nachher im Backend bentutzt um Passwords zu entschlüsseln.
Die gespeicherte Passwords haben eine andere Art Verschlüsselung als die User Passwords. Diese nehmen den Password der User, überprüfen ob die Login Daten stimmen und entschlüsseln dann den Password. Der SHA512CryptoServiceProvider wird angewendet um diese Verschlüsselung aus zu führen.
