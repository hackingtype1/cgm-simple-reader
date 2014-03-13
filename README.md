cgm-simple-reader
=================
Using Windows 8 compatible DLLs from Dexcom Studio 12.0.4.6 will allow this application to work with Windows 8,
no other changes have been required.

Makes a mobile windows 8 option a possibility with newer 8"  (win 8 "pro") tablets. any LTE pro tablets out there?

Reads from a connected receiver and generates a CSV for easy reuse. The file contains headers, so it should be pretty straightforward.

EGVDownloader Project is missing the Dexcom DLLs... You will need to install Dexcom Studio and manually fix/add the references to the following DLLs:
Aspose.Words
Dexcom.Client.Common
Dexcom.Common.Data
Dexcom.Common
Dexcom.FileTransfer
Dexcom.R2ReceiverApi
Dexcom.ReceiverCommercialApi
ICSharpCode.SharpZipLib

I'm fairly certain that we don't actually depend on all of these files.


Basics to get it running:
1. Connect G4
2. Open windows command line (cmd.exe)
3. Go to path of compiled EXE (DexcomCMD.exe)
4. You can simply run DexcomCMD - it will display the csv save file (and path) as well as the interval at which it will poll the G4. ---- The defaults are c:\G4Output.csv and 30000 milliseconds.
5. You can also set the output file and interval like this: > DexcomCMD -t 20000 -f C:/Example.csv     [ '-t' is the time in milliseconds between downloads from G4, '-f' is the path and file-name to save to]
