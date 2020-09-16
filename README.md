# QSE (Quest Song exporter)
A Tool to zip Beat Saber songs from Quest (custom_level_SongHash) to the right names. You can also make a List of all Songs you have.

# How-to (Quest)
### C# version (GUI):

Connect your Quest to you PC. Then copy all folders from "sd card\BMBFData\CustomSongs" to any empty folder on your PC. After it has finished copying start Quest Song Exporter.exe . Click choose Song Folder and browse for the directory where your Song Folders are located. Do the same with your destination directory. Click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped. Optionally you can check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.

### Java version (discontinued!): 

Connect your Quest to you PC. Then copy all folders from "sd card\BMBFData\CustomSongs" to any empty folder on your PC. After it has finished copying start QSE.bat. It asks you to put in a Directory where your songs folders are. You have to put in the folder you just copied them. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# How-to (PC)
### C# version (GUI):

Start Quest Song Exporter.exe . Click choose Song Folder and browse for the directory where your Song Folders are located (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Do the same with your destination directory. Click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped. Optionally you can check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.

### Java version (discontinued!):

Locate your Custom Songs Folder (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Then copy the directory Path and past it into QSE.bat when it askes you. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# Differences between the Java and C# version
- The Java Version got discontinued (is now outdated).
- The C# version has a GUI.
- C# Version is about 24% faster.
- The C# version is only for Windows. Java could work with Mac and Linux. But I'm not able to test it.
- Java Version requiers Java. The C# version should work on every Windows PC.

# Known Issues
- Exported Songs Counter may be wrong due to reconizing the Song but don't zipping them correctly. (I'm working on that; Could be fixed in the c# version)

# To-Do
- Adding Image to GUI (ETA when I get an Idea)
- Checking all Info.dat Files types possible (ETA N/A)

- (Updating Java Version) (ETA Maybe never)
