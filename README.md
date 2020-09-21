# QSE (Quest Song exporter)
A Tool to zip Beat Saber songs from Quest (custom_level_SongHash) to the right names. You can also make a List of all Songs you have.

# How-to (Quest)
### C# version (GUI):
#### Save Songs
Start Quest Song Exporter.exe. Connect your Quest to you PC. Check "Auto Mode (Quest only)". Then click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped.
#### Make Song List
Do everything as above and check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.
#### How to do Playlist Backups
Enter your Quests IP and click "Backup Playlists". It'll save a file named "Playlists.json" in the folder you specified in "Choose Source Folder". Notes: It Does NOT SAVE SONGS. ONLY PLAYLIST CONFIGURATION gets saved. You can only store one Playlist at once (unless you copy it).
#### How to restore Playlists
Enter Your Quests IP and then choose the Folder where "Playlists.json" is saved (It'll default to the programs location if you don't specify anything). Then press Restore Playlists. Note: No Songs get restored. Be sure that you have all Songs that you had when you made the Backup.


### Java version (discontinued!): 

Connect your Quest to you PC. Then copy all folders from "sd card\BMBFData\CustomSongs" to any empty folder on your PC. After it has finished copying start QSE.bat. It asks you to put in a Directory where your songs folders are. You have to put in the folder you just copied them. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# How-to (PC/Songs are already on PC)
### C# version (GUI):
#### Save Songs
Start Quest Song Exporter.exe . Click choose Song Folder and browse for the directory where your Song Folders are located (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Do the same with your destination directory. Click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped.
#### Make Song List
Do everything as above and check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.

### Java version (discontinued!):

Locate your Custom Songs Folder (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Then copy the directory Path and past it into QSE.bat when it askes you. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# Differences between the Java and C# version
- The Java Version got discontinued (is now outdated).
- The C# version has a GUI.
- C# Version is about 24% faster.
- The C# version is only for Windows. Java could work with Mac and Linux. But I'm not able to test it.
- Java Version requiers Java. The C# version should work on every Windows PC.

# Known Issues
- N/A

# To-Do
- Add native support for multiple playlists (ETA N/A)

- (Updating Java Version) (ETA Maybe never)
