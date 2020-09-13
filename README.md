# QSE (Quest Song exporter)
A Tool to zip Beat Saber songs from Quest (custom_level_SongHash) to the right names.

# How-to (Quest)
Connect your Quest to you PC. Then copy all folders from "sd card\BMBFData\CustomSongs" to any empty folder on your PC. After it has finished copying start QSE.bat. It asks you to put in a Directory where your songs folders are. You have to put in the folder you just copied them. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# How-to (PC)
Locate your Custom Songs Folder (probably at C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels). Then copy the directory Path and past it into QSE.bat when it askes you. It'll start zipping them right away. After it has finished it'll let you know how many Songs it (tried) zipped.

# Differences between the Java and C# version
- The c# version has a GUI.
- The Java version overwrites old zip files the c# version doesn't.
- The Java version outputs what it does in real time. The c# version only after it has finished.

Both should work the same and take about the same amount of time.

# Known Issues
- Exported Songs Counter may be wrong due to reconizing the Song but don't zipping them correctly. (I'm working on that; Could be fixed the c# version)
