# THIS TOOL DOES NOT RECIEVE UPDATES ATM PLEASE USE [BMBF MANAGER](https://github.com/ComputerElite/BM) instead
# QSU (Quest Song Utilities)
It was originally created just for the puprpose of creating nicely named zips out of the shitty named Beat Saber Songs on Quest (custom_level_songhash). Now it can also do a list of all songs you have woth Metadata like BPM, Song Artist, Mapper, ... And thanks to darkuni you can make Backups of your Playlists and Restore them without changing your mod configuration. OneClick Install via BeatSaver has been added and you can make a BPList out of your playlists in BMBf (The tool was originally named QSE Quest Song Exporter but got renamed due to these changes)

# How-to (Quest)
## C# version (GUI):
### Save Songs
Start Quest Song Utilities.exe. Connect your Quest to you PC. Check "Auto Mode (Quest only)" ([adb](https://developer.android.com/studio/releases/platform-tools) or [SideQuest](https://sidequestvr.com/setup-howto) required). Then click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped.
<br/>
<br/>
### Make Song List
Do everything as above and check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.
<br/>
<br/>
### how to make a BPList
Enter your Quests IP and click "Load Playlists". Then choose the Playlist you want and click "Make BPList". It'll save a .bplist file in the BPLists folder.
### How to do Playlist Backups
Enter your Quests IP and a Backup name and click "Backup Playlists". The It'll save your Playlists.

**Note**: It Does **Not save Songs. Only Playlist Configuration gets saved**.<br/><br/>
### How to restore Playlists
Enter Your Quests IP and then choose the Backup in the drop down menu. Then press Restore Playlists. 

**Note**: No Songs get restored. Be sure that you have all Songs that you had when you made the Backup.
### How to delete Playlists with the Songs
Enter your Quests IP and click "Load Playlists". Then choose the Playlist you want to delete and click "Delete selected Playlists". 
</br></br>
#### **THIS IS NOT UNDOABLE**
</br>

### How to enable OneClick Install
Connect your Quest to your PC and open "Quest Song Utilities.exe" everytime you install a Update/move the files you'll get a message that says to enable custom protocols. Click "Enable QSU OneClick install" and yes on everything that appears to enable one click install with the links qsu://[SongKey] e. g. [qsu://abcd](qsu://abcd). Click "Enable BeatSaver OneClick install" and again press yes on everything that appears. After that you will be able to use OneClick install links like [beatsaver://abcd](beatsaver://abcd) to install Songs to your quest. The custom Liks to open the programs don't work because of GitHub.

<br/>
<br/>
<br/>

## Java version (discontinued!): 

Connect your Quest to you PC. Then copy all folders from "sd card\BMBFData\CustomSongs" to any empty folder on your PC. After it has finished copying start QSE.bat. It asks you to put in a Directory where your songs folders are. You have to put in the folder you just copied them. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# How-to (PC/Songs are already on PC)
## C# version (GUI):
### Save Songs
Start Quest Song Utilities.exe . Click choose Song Folder and browse for the directory where your Song Folders are located (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Do the same with your destination directory. Click start and wait a few minutes (about 1 - 6). After it has finished it'll let you know how many Songs it zipped.
### Make Song List
Do everything as above and check "Make List of all songs". This will create a List of all Your Songs with information like Song name, BPM, Song sub name, song author, etc.

## Java version (discontinued!):

Locate your Custom Songs Folder (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Then copy the directory Path and past it into QSE.bat when it askes you. Then it askes you where you want to put the finished zips. Put there a Directory of your choice. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# Differences between the Java and C# version
- The Java Version got discontinued (is now outdated. No new features will be added).
- The C# version has a GUI.
- C# Version is about 24% faster.
- The C# version is only for Windows. Java could work with Mac and Linux. But I'm not able to test it.
- Java Version requiers Java. The C# version should work on every Windows PC.

# Contributers
- darkuni (helped me making the Playlist backuping and restoring possible. Check out his nice program: [Playlist Editor Pro](https://beatsaberquest.com/bmbf/my-tools/playlist-editor-pro/#:~:text=Playlist%20Editor%20Pro%20is%20a,details%20and%20download%20it%20here.))
- rui2015 (helped testing the tool and gave suggestions)
- Bunny83 with [SimpleJSON.cs](https://github.com/Bunny83/SimpleJSON/blob/master/SimpleJSON.cs)

# [wiki](https://github.com/ComputerElite/wiki)

# Known Issues
- N/A

# To-Do
- N/A
