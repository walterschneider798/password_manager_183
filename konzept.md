# Das Grundkonzept des Passwortsafes
Edo Husakovic und Walter Schneider
## Beschreibung
Der Zweck dieses Projekts besteht darin, eine Kennwortsicherheitsanwendung zu erstellen. Der Benutzer sollte sein Passwort haben
Möglichkeit, sie für verschiedene Konten sicher zu verschlüsseln. Benutzer sollten ein eigenes Passwort haben
Sie können immer noch zuschauen.
## Technologie

## Bestätigung
Der Benutzer erstellt ein Konto und kann sich später einloggen. Solange er ein Konto hat, kann er es verwenden
Es speichert auch die Passwörter seiner anderen Konten. Der Benutzer benötigt einen Benutzernamen und ein Passwort,
Er hat sich bei der Registrierung selbst definiert.
##Verschlüsselung
Das beim Erstellen des Benutzerkontos erstellte Passwort wird gehasht und gespeichert. Das beim Erstellen des Benutzerkontos erstellte Passwort wird gehasht und gespeichert.
Der Passwort-Hash wird später als symmetrischer Schlüssel für den Passwort-Safe verwendet. Das Passwort im Safe ist mit einem Hash verschlüsselt, bei der Anmeldung des Benutzers wird das Passwort für ihn entschlüsselt.
Ändert der Benutzer sein Passwort, werden alle PWs im Safe mit dem alten Passwort entschlüsselt und anschließend mit dem neuen Passwort verschlüsselt.
