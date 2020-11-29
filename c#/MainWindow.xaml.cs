using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using SimpleJSON;
using System;
using System.Collections;   
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace Quest_Song_Exporter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int MajorV = 3;
        int MinorV = 12;
        int PatchV = 3;
        Boolean Preview = false;

        String IP = "";
        String path;
        String dest;
        String keyName = "";
        Boolean debug = false;
        Boolean automode = false;
        Boolean copied = false;
        Boolean draggable = true;
        Boolean Running = false;
        Boolean OneClick = false;
        Boolean OneClickQSU = false;
        Boolean Converted = false;
        Boolean ComeFromUpdate = false;
        String exe = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 1);
        ArrayList P = new ArrayList();
        int Lists = 0;
        int Open = 0;
        String args = "";
        String Log = "";
        JSONNode UpdateJSON = JSON.Parse("{}");

        public void Changelog()
        {
            if (ComeFromUpdate)
            {
                String creators = "";
                foreach (JSONNode Creator in UpdateJSON["Updates"][0]["Creators"])
                {
                    creators = creators + Creator.ToString().Replace("\"", "") + ", ";
                }
                if (creators.Length >= 2)
                {
                    creators = creators.Substring(0, creators.Length - 2);
                }
                else
                {
                    creators = "ComputerElite";
                }
                txtbox.AppendText("\n\n\nYou installed a Update (Version: " + MajorV + "." + MinorV + "." + PatchV + ").\n\nUpdate posted by: " + creators + "\n\nChangelog:\n" + UpdateJSON["Updates"][0]["Changelog"]);
            }

        }

        public async Task KeyAsync(String key)
        {
            int c = 0;
            while (File.Exists(exe + "\\tmp\\Song" + c + ".zip")) {
                c++;
            }
            Open = c;

            StartBMBF();
            QuestIP();

            args = key;
            args = args.Replace("qsu://", "");
            args = args.Replace("beatsaver://", "");
            args = args.Replace("/", "");
            Uri keys = new Uri("https://beatsaver.com/api/download/key/" + args);
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                txtbox.AppendText("\n\nDownloading BeatMap " + args);
            }));
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate{ }));
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(finished_download);
                    client.DownloadFileAsync(keys, exe + "\\tmp\\Song" + Open + ".zip");
                }));
            }
            catch
            {
                txtbox.AppendText("\n\nAn Error Occured");
                return;
            }
        }

        public void upload(String path)
        {
            getQuestIP();

            WebClient client = new WebClient();

            txtbox.AppendText("\n\nUploading " + path + " to BMBF");
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate {
                    client.UploadFile("http://" + IP + ":50000/host/beatsaber/upload?overwrite", path);
                }));
                
            } catch
            {
                txtbox.AppendText("\n\nA error Occured (Code: BMBF100)");
            }
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    Sync();
                }));
                txtbox.AppendText("\n\nSong " + keyName + " (" + args + ") was synced to your Quest.");
                this.Close();
            } catch
            {
                txtbox.AppendText("\n\nCouldn't sync with BeatSaber. Needs to be done manually.");
            }
        }

        public void finished_download(object sender, AsyncCompletedEventArgs e)
        {
            txtbox.AppendText("\nDownloaded BeatMap " + args + "\n");
            upload(exe + "\\tmp\\Song" + Open + ".zip");
        }

        public MainWindow()
        {
            InitializeComponent();
            UpdateB.Visibility = Visibility.Hidden;
            txtbox.Text = "Output:\n";
            if(debug)
            {
                txtbox.AppendText(exe);
            }
            txtboxd.Text = "Please choose your destination folder.";
            txtboxs.Text = "Please choose your Song folder.";
            if(!Directory.Exists(exe + "\\tmp"))
            {
                Directory.CreateDirectory(exe + "\\tmp");
            }
            if (!Directory.Exists(exe + "\\CustomSongs"))
            {
                Directory.CreateDirectory(exe + "\\CustomSongs");
            }
            if (!Directory.Exists(exe + "\\Playlists"))
            {
                Directory.CreateDirectory(exe + "\\Playlists");
            }
            if (!Directory.Exists(exe + "\\BPLists"))
            {
                Directory.CreateDirectory(exe + "\\BPLists");
            }
            if (File.Exists(exe + "\\QSE_Update.exe"))
            {
                File.Delete(exe + "\\QSE_Update.exe");
            }
            if (File.Exists(exe + "\\QSU_Update.exe"))
            {
                File.Delete(exe + "\\QSU_Update.exe");
            }
            Update();
            Move();
            
            Backups.SelectedIndex = 0;
            getBackups(exe + "\\Playlists");

            Playlists.Items.Add("Load Playlists!");
            Playlists.SelectedIndex = 0;

            checks();
            QuestIP();
            Changelog();
            ComeFromUpdate = false;
        }

        public void checks()
        {
            if (!File.Exists(exe + "\\Info.json"))
            {
                txtbox.AppendText("\n\nIt's suggested to enable custom protocols by clicking the corresponding buttons.");
                return;
            }

            var json = JSON.Parse(File.ReadAllText(exe + "\\Info.json"));

            if(!json["Version"].ToString().Equals("\"" + MajorV.ToString() + MinorV.ToString() + PatchV.ToString() + "\""))
            {
                txtbox.AppendText("\n\nIt's suggested to enable/update custom protocols by clicking the corresponding buttons.");
            } else if (!json["NotFirstRun"].AsBool)
            {
                txtbox.AppendText("\n\nIt's suggested to enable/update custom protocols by clicking the corresponding buttons.");
            } else if(!json["Location"].Equals(System.Reflection.Assembly.GetEntryAssembly().Location))
            {
                txtbox.AppendText("\n\nIt's suggested to enable/update custom protocols by clicking the corresponding buttons.");
            }

            Quest.Text = json["IP"];
            Converted = json["pConverted"].AsBool;
            ComeFromUpdate = json["ComeFromUpdate"].AsBool;

            OneClick = json["OneClickInstalled"].AsBool;
            OneClickQSU = json["OneClickInstalledQSU"].AsBool;
            if (OneClick)
            {
                InstalledOneClick.Content = "Disable BeatSaver OneClick install";
            } else
            {
                InstalledOneClick.Content = "Enable BeatSaver OneClick install";
            }
            if (OneClickQSU)
            {
                InstalledOneClickQ.Content = "Disable QSU OneClick install";
            }
            else
            {
                InstalledOneClickQ.Content = "Enable QSU OneClick install";
            }
        }

        public void saveInfo()
        {
            getQuestIP();
            var json = JSON.Parse("{\"Version\":\"1\", \"NotFirstRun\": false}");
            json["Version"] = MajorV.ToString() + MinorV.ToString() + PatchV.ToString();
            json["NotFirstRun"] = true;
            json["Location"] = System.Reflection.Assembly.GetEntryAssembly().Location;
            json["OneClickInstalled"] = OneClick;
            json["OneClickInstalledQSU"] = OneClickQSU;
            json["IP"] = IP;
            json["pConverted"] = Converted;
            json["ComeFromUpdate"] = ComeFromUpdate;
            File.WriteAllText(exe + "\\Info.json", json.ToString());
        }

        public void enable_QSU(object sender, RoutedEventArgs e)
        {
            if (!OneClickQSU)
            {
                txtbox.AppendText("\n\nChanging Registry to enable QSU Custom protocols via QSU");
                String regFile = "Windows Registry Editor Version 5.00\n\n[HKEY_CLASSES_ROOT\\qsu]\n@=\"URL: qsu\"\n\"URL Protocol\"=\"qsu\"\n\n[HKEY_CLASSES_ROOT\\qsu]\n@=\"" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\", "\\\\") + "\"\n\n[HKEY_CLASSES_ROOT\\qsu\\shell]\n\n[HKEY_CLASSES_ROOT\\qsu\\shell\\open]\n\n[HKEY_CLASSES_ROOT\\qsu\\shell\\open\\command]\n@=\"" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\", "\\\\") + " \\\"%1\\\"\"";
                File.WriteAllText(exe + "\\registry.reg", regFile);
                try
                {
                    Process.Start(exe + "\\registry.reg");
                    txtbox.AppendText("\n\nOneClick Install via QSU enabled");
                }
                catch
                {
                    txtbox.AppendText("\n\nRegistry was unable to change... no Custom protocol enabled.");
                    return;
                }
                InstalledOneClickQ.Content = "Disable QSU OneClick install";
                OneClickQSU = true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("This will disable OneClick Install via Quest Song Utilities.\nDo you wish to continue?", "Quest Song Utilities OneClick QSU", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        txtbox.AppendText("\n\nOneClick disabeling enabeling aborted");
                        Running = false;
                        txtbox.ScrollToEnd();
                        return;
                }
                txtbox.AppendText("\n\nChanging Registry to disable QSU Custom protocols");
                String regFile = "Windows Registry Editor Version 5.00\n\n[-HKEY_CLASSES_ROOT\\qsu]";
                File.WriteAllText(exe + "\\registry.reg", regFile);
                try
                {
                    Process.Start(exe + "\\registry.reg");
                    txtbox.AppendText("\n\nOneClick Install via QSU Custom protocol disabled");
                }
                catch
                {
                    txtbox.AppendText("\n\nRegistry was unable to change.");
                    return;
                }
                InstalledOneClickQ.Content = "Enable QSU OneClick install";
                OneClickQSU = false;
            }
        }

        public void enable_BeatSaver(object sender, RoutedEventArgs e)
        {
            if(!OneClick)
            {
                MessageBoxResult result = MessageBox.Show("This will disable OneClick Install via Mod Assistent.\nDo you wish to continue?", "Quest Song Utilities OneClick BeatSaver", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        txtbox.AppendText("\n\nOneClick Install enabeling aborted");
                        Running = false;
                        txtbox.ScrollToEnd();
                        return;
                }
                txtbox.AppendText("\n\nChanging Registry to enable OneClick Custom protocols");
                String regFile = "Windows Registry Editor Version 5.00\n\n[HKEY_CLASSES_ROOT\\beatsaver]\n@=\"URL: beatsaver\"\n\"URL Protocol\"=\"beatsaver\"\n\n[HKEY_CLASSES_ROOT\\beatsaver]\n@=\"" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\", "\\\\") + "\"\n\n[HKEY_CLASSES_ROOT\\beatsaver\\shell]\n\n[HKEY_CLASSES_ROOT\\beatsaver\\shell\\open]\n\n[HKEY_CLASSES_ROOT\\beatsaver\\shell\\open\\command]\n@=\"" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace("\\", "\\\\") + " \\\"%1\\\"\"";
                File.WriteAllText(exe + "\\registry.reg", regFile);
                try
                {
                    Process.Start(exe + "\\registry.reg");
                    txtbox.AppendText("\n\nOneClick Install via BeatSaver enabled");
                }
                catch
                {
                    txtbox.AppendText("\n\nRegistry was unable to change... no Custom protocol disabled.");
                    return;
                }
                InstalledOneClick.Content = "Disable BeatSaver OneClick install";
                OneClick = true;
            } else
            {
                MessageBoxResult result = MessageBox.Show("This will disable OneClick Install via Quest Song Utilities.\nDo you wish to continue?", "Quest Song Utilities OneClick BeatSaver", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.No:
                        txtbox.AppendText("\n\nOneClick disabeling enabeling aborted");
                        Running = false;
                        txtbox.ScrollToEnd();
                        return;
                }
                txtbox.AppendText("\n\nChanging Registry to disable OneClick Custom protocols");
                String regFile = "Windows Registry Editor Version 5.00\n\n[-HKEY_CLASSES_ROOT\\beatsaver]";
                File.WriteAllText(exe + "\\registry.reg", regFile);
                try
                {
                    Process.Start(exe + "\\registry.reg");
                    txtbox.AppendText("\n\nOneClick Install via BeatSaver disabled");
                }
                catch
                {
                    txtbox.AppendText("\n\nRegistry was unable to change.");
                    return;
                }
                InstalledOneClick.Content = "Enable BeatSaver OneClick install";
                OneClick = false;
            }
            
        }

        public void Sync()
        {
            System.Threading.Thread.Sleep(2000);
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("foo", "foo");
                client.UploadValues("http://" + IP + ":50000/host/beatsaber/commitconfig", "POST", client.QueryString);
            }
        }

        public void QuestIP()
        {
            String IPS = adbS("shell ifconfig wlan0");
            int Index = IPS.IndexOf("inet addr:");
            Boolean space = false;
            String FIP = "";
            for (int i = 0; i < IPS.Length; i++)
            {
                if (i > (Index + 9) && i < (Index + 9 + 16))
                {
                    if (IPS.Substring(i, 1) == " ")
                    {
                        space = true;
                    }
                    if (!space)
                    {
                        FIP = FIP + IPS.Substring(i, 1);
                    }
                }
            }

            if (FIP == "" && IP == "Quest IP")
            {
                IP = "Quest IP";
                return;
            }
            if (FIP == "") return;
            IP = FIP;
            Quest.Text = IP;
        }

        public void StartBMBF()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                adb("shell am start -n com.weloveoculus.BMBF/com.weloveoculus.BMBF.MainActivity");
            }));
        }

        public Boolean adb(String Argument)
        {
            String User = System.Environment.GetEnvironmentVariable("USERPROFILE");
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = "adb.exe";
            s.WindowStyle = ProcessWindowStyle.Minimized;
            s.Arguments = Argument;
            s.RedirectStandardOutput = true;
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(s))
                {
                    String IPS = exeProcess.StandardOutput.ReadToEnd();
                    exeProcess.WaitForExit();
                    if (IPS.Contains("no devices/emulators found"))
                    {
                        txtbox.AppendText("\n\n\nAn error Occured (Code: ADB100). Check following");
                        txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                        txtbox.AppendText("\n\n- You have adb installed.");
                        txtbox.ScrollToEnd();
                        return false;
                    }
                    return true;
                }
            }
            catch
            {

                ProcessStartInfo se = new ProcessStartInfo();
                se.CreateNoWindow = false;
                se.UseShellExecute = false;
                se.FileName = User + "\\AppData\\Roaming\\SideQuest\\platform-tools\\adb.exe";
                se.WindowStyle = ProcessWindowStyle.Minimized;
                se.Arguments = Argument;
                se.RedirectStandardOutput = true;
                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(se))
                    {
                        String IPS = exeProcess.StandardOutput.ReadToEnd();
                        exeProcess.WaitForExit();
                        if (IPS.Contains("no devices/emulators found"))
                        {
                            txtbox.AppendText("\n\n\nAn error Occured (Code: ADB100). Check following");
                            txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                            txtbox.AppendText("\n\n- You have adb installed.");
                            txtbox.ScrollToEnd();
                            return false;
                        }
                        return true;
                    }
                }
                catch
                {
                    // Log error.
                    txtbox.AppendText("\n\n\nAn error Occured (Code: ADB100). Check following");
                    txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                    txtbox.AppendText("\n\n- You have adb installed.");
                    txtbox.ScrollToEnd();
                    return false;
                }
            }
        }

        public String adbS(String Argument)
        {
            String User = System.Environment.GetEnvironmentVariable("USERPROFILE");
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = "adb.exe";
            s.WindowStyle = ProcessWindowStyle.Minimized;
            s.RedirectStandardOutput = true;
            s.Arguments = Argument;
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(s))
                {
                    String IPS = exeProcess.StandardOutput.ReadToEnd();
                    exeProcess.WaitForExit();
                    return IPS;
                }
            }
            catch
            {

                ProcessStartInfo se = new ProcessStartInfo();
                se.CreateNoWindow = false;
                se.UseShellExecute = false;
                se.FileName = User + "\\AppData\\Roaming\\SideQuest\\platform-tools\\adb.exe";
                se.WindowStyle = ProcessWindowStyle.Minimized;
                se.RedirectStandardOutput = true;
                se.Arguments = Argument;
                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(se))
                    {
                        String IPS = exeProcess.StandardOutput.ReadToEnd();
                        exeProcess.WaitForExit();
                        return IPS;
                        
                    }
                }
                catch
                {
                    // Log error.
                    txtbox.AppendText("\n\n\nAn error Occured (Code: ADB100). Check following");
                    txtbox.AppendText("\n\n- Your Quest is connected and USB Debugging enabled.");
                    txtbox.AppendText("\n\n- You have adb installed.");
                }
            }
            return "";
        }

        public void getPlaylists(object sender, RoutedEventArgs e)
        {
            StartBMBF();
            if (!CheckIP())
            {
                txtbox.AppendText("\n\nChoose a valid IP!");
                return;
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("http://" + IP + ":50000/host/beatsaber/config", exe + "\\tmp\\Config.json");
                } catch
                {
                    txtbox.AppendText("\n\n\nError (Code: BMBF100). Couldn't acces BMBF Web Interface. Check Following:");
                    txtbox.AppendText("\n\n- You've put in the right IP");
                    txtbox.AppendText("\n\n- BMBF is opened");
                    return;
                }
            }
            Playlists.Items.Clear();
            Playlists.Items.Add("Playlists");

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://raw.githubusercontent.com/BMBF/resources/master/assets/beatsaber-knowns.json", exe + "\\tmp\\beatsaber-knowns.json");
                } catch
                {
                    txtbox.AppendText("\n\nCouldn't check for new Song Packs.");
                }
            }
            String knows = exe + "\\tmp\\beatsaber-knowns.json";

            var json2 = SimpleJSON.JSON.Parse(File.ReadAllText(knows));
            ArrayList known = new ArrayList();

            foreach(JSONNode pack in json2["knownLevelPackIds"])
            {
                known.Add(pack.ToString().Replace("\"", ""));
            }


            String Config = exe + "\\tmp\\config.json";
            P = new ArrayList();

            var json = SimpleJSON.JSON.Parse(File.ReadAllText(Config));
            int index = 0;

            ArrayList PN = new ArrayList();

            foreach (JSONNode Playlist in json["Config"]["Playlists"])
            {
                P.Add(Playlist["PlaylistID"].ToString().Replace("\"", ""));
                PN.Add(Playlist["PlaylistName"].ToString().Replace("\"", ""));
            }

            foreach (String c in P)
            {
                if (!known.Contains(c))
                {
                    Playlists.Items.Add(PN[index]);
                }
                else
                {
                    Lists++;
                }
                index++;
            }
            
            Playlists.SelectedIndex = 0;
            txtbox.AppendText("\n\nLoaded Playlists.");
        }

        public void BPList(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                return;
            }
            if (!CheckIP())
            {
                txtbox.AppendText("\n\nChoose a valid IP!");
                txtbox.ScrollToEnd();
                return;
            }
            Running = true;
            CheckIP();
            if (Playlists.SelectedIndex == 0)
            {
                txtbox.AppendText("\n\nChoose a Playlist!");
                txtbox.ScrollToEnd();
                Running = false;
                return;
            }
            
            String Config = exe + "\\tmp\\config.json";
            String txt = File.ReadAllText(Config);
            var json = JSON.Parse(txt);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                txtbox.AppendText("\n\nmaking BPList " + json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistName"]);
                txtbox.ScrollToEnd();
            }));
            var result = JSON.Parse("{}");
            result["playlistTitle"] = json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistName"];
            result["playlistAuthor"] = "Quest Song Utilities";
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                txtbox.AppendText("\nDownloading Playlist Cover");
                txtbox.ScrollToEnd();
            }));
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile("http://" + IP + ":50000/host/beatsaber/playlist/cover?PlaylistID=" + json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistID"], exe + "\\tmp\\Playlist.png");
                }
                catch
                {
                    txtbox.AppendText("\n\n\nError (Code: BMBF100). Couldn't acces BMBF Web Interface. Check Following:");
                    txtbox.AppendText("\n\n- You've put in the right IP");
                    txtbox.AppendText("\n\n- BMBF is opened");
                    txtbox.ScrollToEnd();
                    Running = false;
                    return;
                }
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                txtbox.AppendText("\nDownloaded Playlist Cover");
                txtbox.ScrollToEnd();
            }));
            result["image"] = "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(exe + "\\tmp\\Playlist.png"));

            int i = 0;
            foreach (JSONNode Song in json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["SongList"])
            {
                String SongHash = Song["SongID"];
                SongHash = SongHash.Replace("custom_level_", "");
                String SongName = Song["SongName"];
                result["songs"][i]["hash"] = SongHash;
                result["songs"][i]["songName"] = SongName;
                i++;
            }

            File.WriteAllText(exe + "\\BPLists\\" + json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistName"] + ".bplist", result.ToString());
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                txtbox.AppendText("\nBPList " + json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistName"] + " has been made at " + "BPLists\\" + json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["PlaylistName"] + ".bplist");
                txtbox.ScrollToEnd();
            }));
            Running = false;
        }

        public void DeleteP(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                return;
            }
            if (!CheckIP())
            {
                txtbox.AppendText("\n\nChoose a valid IP!");
                txtbox.ScrollToEnd();
                return;
            }
            Running = true;
            CheckIP();
            if (Playlists.SelectedIndex == 0)
            {
                txtbox.AppendText("\n\nChoose a Playlist!");
                txtbox.ScrollToEnd();
                Running = false;
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you Sure to delete the Playlists named \"" + Playlists.SelectedValue + "\"?\n\n THIS IS NOT UNDOABLE!!!", "Quest Song Utilities Playlist deleting", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.No:
                    txtbox.AppendText("\n\nDeleting aborted");
                    Running = false;
                    txtbox.ScrollToEnd();
                    return;
            }
            
            var json = SimpleJSON.JSON.Parse(File.ReadAllText(exe + "\\tmp\\config.json"));

            foreach (JSONNode song in json["Config"]["Playlists"][Playlists.SelectedIndex + Lists - 1]["SongList"])
            {
                if(!adb("shell rm -rR /sdcard/BMBFData/CustomSongs/" + song["SongID"].ToString().Replace("\"", "")))
                {
                    return;
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    txtbox.AppendText("\n\nDeleted " + song["SongID"]);
                    txtbox.ScrollToEnd();
                }));
            }
            json["Config"]["Playlists"].Remove(Playlists.SelectedIndex + Lists - 1);

            try
            {
                File.WriteAllText(exe + "\\tmp\\config.json", json["Config"].ToString());

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    postChanges(exe + "\\tmp\\config.json");
                }));
                txtbox.AppendText("\n\nDeleted Playlist with all Data");
                txtbox.ScrollToEnd();
            }
            catch
            {
                txtbox.AppendText("\n\n\nAn error occured (Code: BMBF100). Check following:");
                txtbox.AppendText("\n\n- Your Quest is on and BMBF opened");
                txtbox.AppendText("\n\n- You put in the Quests IP right.");
                txtbox.ScrollToEnd();
            }
            Running = false;
        }


        public void Move()
        {
            if (Converted) return;

            foreach (String file in Directory.GetFiles(exe + "\\CustomSongs"))
            {
                if (file.EndsWith(".json"))
                {
                    File.Move(file, exe + "\\Playlists\\" + System.IO.Path.GetFileName(file));
                }
            }

            foreach (String file in Directory.GetFiles(exe + "\\Playlists"))
            {
                if(file.EndsWith(".json"))
                {
                    String contents = File.ReadAllText(file);
                    if(contents.EndsWith(","))
                    {
                        contents = contents.Substring(0, contents.Length - 1) + "}}";
                        JSONNode c = JSON.Parse(contents);
                        File.Delete(file);
                        File.WriteAllText(file, c["Config"].ToString());
                    }
                }
            }
            
            Converted = true;
        }

        public Boolean CheckIP()
        {
            getQuestIP();
            if (IP == "Quest IP")
            {
                return false;
            }
            IP = IP.Replace(":5000000", "");
            IP = IP.Replace(":500000", "");
            IP = IP.Replace(":50000", "");
            IP = IP.Replace(":5000", "");
            IP = IP.Replace(":500", "");
            IP = IP.Replace(":50", "");
            IP = IP.Replace(":5", "");
            IP = IP.Replace(":", "");
            IP = IP.Replace("/", "");
            IP = IP.Replace("https", "");
            IP = IP.Replace("http", "");
            IP = IP.Replace("Http", "");
            IP = IP.Replace("Https", "");

            int count = IP.Split('.').Count();
            if (count != 4)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    Quest.Text = IP;
                }));
                return false;
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                Quest.Text = IP;
            }));
            return true;
        }

        public void getBackups(String Path)
        {
            ArrayList Jsons = new ArrayList();
            string[] Files = Directory.GetFiles(Path);
            Backups.Items.Clear();
            Backups.Items.Add("Backups");

            foreach (String cfile in Files)
            {
                if (cfile.EndsWith(".json"))
                {
                    Backups.Items.Add(System.IO.Path.GetFileNameWithoutExtension(cfile));
                }
            }
            Backups.SelectedIndex = 0;
        }

        public void Update()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        UpdateJSON = JSON.Parse(client.DownloadString("https://raw.githubusercontent.com/ComputerElite/QSE/master/update.json"));
                    }
                    catch
                    {
                        txtbox.AppendText("\n\n\nAn error Occured (Code: UD100). Couldn't check for Updates. Check following");
                        txtbox.AppendText("\n\n- Your PC has internet.");
                        return;
                    }
                }

                int MajorU = UpdateJSON["Updates"][0]["Major"];
                int MinorU = UpdateJSON["Updates"][0]["Minor"];
                int PatchU = UpdateJSON["Updates"][0]["Patch"];

                if (MajorU > MajorV || MinorU > MinorV || PatchU > PatchV)
                {
                    //Newer Version available
                    UpdateB.Visibility = Visibility.Visible;
                }

                String MajorVS = Convert.ToString(MajorV);
                String MinorVS = Convert.ToString(MinorV);
                String PatchVS = Convert.ToString(PatchV);
                String MajorUS = Convert.ToString(MajorU);
                String MinorUS = Convert.ToString(MinorU);
                String PatchUS = Convert.ToString(PatchU);

                String VersionVS = MajorVS + MinorVS + PatchVS;
                int VersionV = Convert.ToInt32(VersionVS);
                String VersionUS = MajorUS + MinorUS + PatchUS + " ";
                int VersionU = Convert.ToInt32(VersionUS);
                if (VersionV > VersionU)
                {
                    //Newer Version that hasn't been released yet
                    txtbox.AppendText("\n\nLooks like you have a preview version. Downgrade now from " + MajorV + "." + MinorV + "." + PatchV + " to " + MajorU + "." + MinorU + "." + PatchU + " xD");
                    UpdateB.Visibility = Visibility.Visible;
                    UpdateB.Content = "Downgrade Now xD";
                }
                if (VersionV == VersionU && Preview)
                {
                    //User has Preview Version but a release Version has been released
                    txtbox.AppendText("\n\nLooks like you have a preview version. The release version has been released. Please Update now. ");
                    UpdateB.Visibility = Visibility.Visible;
                }
            }
            catch
            {

            }
        }

        private void Start_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://github.com/ComputerElite/QSE/raw/master/QSE_Update.exe", exe + "\\QSU_Update.exe");
                }
                Process.Start(exe + "\\QSU_Update.exe");
                ComeFromUpdate = true;
                saveInfo();
                Process.GetCurrentProcess().Kill();
            }
            catch
            {
                // Log error.
                txtbox.AppendText("\n\n\nAn error Occured (Code: UD200). Couldn't download Update.");
                txtbox.ScrollToEnd();
            }
        }

        private void Mini(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void QuestIPCheck(object sender, RoutedEventArgs e)
        {
            if(Quest.Text == "")
            {
                Quest.Text = "Quest IP";
            }
        }

        private void BackupNameCheck(object sender, RoutedEventArgs e)
        {
            if(BName.Text == "")
            {
                BName.Text = "Backup Name";
            }
        }


        private void Backup(object sender, RoutedEventArgs e)
        {
            StartBMBF();
            if (Running)
            {
                return;
            }
            Boolean good = CheckIP();
            if (!good)
            {
                txtbox.AppendText("\n\nChoose a valid IP!");
                txtbox.ScrollToEnd();
                Running = false;
                return;
            }
            Running = true;
            try
            {
                CheckIP();
                if (dest == null)
                {
                    dest = exe + "\\Playlists";
                    if (!Directory.Exists(exe + "\\Playlists"))
                    {
                        Directory.CreateDirectory(exe + "\\Playlists");
                    }
                }

                BName.Text = BName.Text.Replace("/", "");
                BName.Text = BName.Text.Replace(":", "");
                BName.Text = BName.Text.Replace("*", "");
                BName.Text = BName.Text.Replace("?", "");
                BName.Text = BName.Text.Replace("\"", "");
                BName.Text = BName.Text.Replace("<", "");
                BName.Text = BName.Text.Replace(">", "");
                BName.Text = BName.Text.Replace("|", "");

                for (int f = 0; f < BName.Text.Length; f++)
                {
                    if (BName.Text.Substring(f, 1).Equals("\\"))
                    {
                        BName.Text = BName.Text.Substring(0, f - 1) + BName.Text.Substring(f + 1, BName.Text.Length - f - 1);
                    }
                }
                
                if (File.Exists(exe + "\\Playlists\\" + BName.Text + ".json"))
                {
                    txtbox.AppendText("\n\nThis Playlist Backup already exists!");
                    txtbox.ScrollToEnd();
                    Running = false;
                    return;
                }

                txtbox.AppendText("\n\nBacking up Playlist to " + exe + "\\Playlists\\" + BName.Text + ".json");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));

                adb("pull /sdcard/BMBFData/Playlists/ \"" + exe + "\\Playlists\"");

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("http://" + IP + ":50000/host/beatsaber/config", exe + "\\tmp\\Config.json");
                }


                String Config = exe + "\\tmp\\config.json";

                var json = JSON.Parse(File.ReadAllText(Config));
                json["IsCommitted"] = false;
                File.WriteAllText(exe + "\\Playlists\\" + BName.Text + ".json", json["Config"].ToString());

                txtbox.AppendText("\n\nBacked up Playlists to " + exe + "\\Playlists\\" + BName.Text + ".json");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            } catch
            {
                txtbox.AppendText("\n\n\nAn error occured (Code: PL100). Check following:");
                txtbox.AppendText("\n\n- You put in the Quests IP right.");
                txtbox.AppendText("\n\n- You've choosen a Backup Name.");
                txtbox.AppendText("\n\n- Your Quest is on.");
                txtbox.ScrollToEnd();

            }
            getBackups(exe + "\\Playlists");
            Running = false;

        }


        public void PushPNG(String Path)
        {
            String[] directories = Directory.GetFiles(Path);



            for (int i = 0; i < directories.Length; i++)
            {
                if (directories[i].EndsWith(".png"))
                {
                    txtbox.AppendText("\n\nPushing " + directories[i] + " to Quest");
                    adb("push \"" + directories[i] + "\" /sdcard/BMBFData/Playlists/");
                    txtbox.ScrollToEnd();
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                }
            }
        }

        public void postChanges(String Config)
        {
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("foo", "foo");
                client.UploadFile("http://" + IP + ":50000/host/beatsaber/config", "PUT", Config);
                client.UploadValues("http://" + IP + ":50000/host/beatsaber/commitconfig", "POST", client.QueryString);
            }
        }


        private void Restore(object sender, RoutedEventArgs e)
        {
            StartBMBF();
            if (Running)
            {
                return;
            }
            if (!CheckIP())
            {
                txtbox.AppendText("\n\nChoose a valid IP!");
                txtbox.ScrollToEnd();
                Running = false;
                return;
            }
            if (Backups.SelectedIndex == 0)
            {
                return;
            }
            Running = true;
            try
            {


                CheckIP();

                String Playlists;
                if (dest == null)
                {
                    dest = path;
                }

                txtbox.AppendText("\n\nRestoring Playlist from " + exe + "\\Playlists\\" + Backups.SelectedValue + ".json");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));



                if (!Directory.Exists(exe + "\\tmp"))
                {
                    Directory.CreateDirectory(exe + "\\tmp");
                }
                using (WebClient client = new WebClient())
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate {
                        client.DownloadFile("http://" + IP + ":50000/host/beatsaber/config", exe + "\\tmp\\OConfig.json");
                    }));
                    
                }

                String Config = exe + "\\tmp\\OConfig.json";

                Playlists = exe + "\\Playlists\\" + Backups.SelectedValue + ".json";

                var j = JSON.Parse(File.ReadAllText(Config));
                var p = JSON.Parse(File.ReadAllText(Playlists));


                j["Config"]["Playlists"] = p["Playlists"];
                File.WriteAllText(exe + "\\tmp\\config.json", j["Config"].ToString());

                PushPNG(exe + "\\Playlists\\Playlists");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate {
                    postChanges(exe + "\\tmp\\config.json");
                }));
                txtbox.AppendText("\n\nRestored old Playlists.");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            } catch
            {
                txtbox.AppendText("\n\n\nAn error occured (Code: BMBF100). Check following:");
                txtbox.AppendText("\n\n- Your Quest is on and BMBF opened");
                txtbox.AppendText("\n\n- You put in the Quests IP right.");
                txtbox.ScrollToEnd();
            }
            Running = false;
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            if(Quest.Text == "Quest IP")
            {
                Quest.Text = "";
            }
            
        }

        private void ClearTextN(object sender, RoutedEventArgs e)
        {
            if(BName.Text == "Backup Name")
            {
                BName.Text = "";
            }
        }


        private void Auto(object sender, RoutedEventArgs e)
        {
            if((bool)auto.IsChecked)
            {
                automode = true;
                txtboxs.Text = "Oculus Quest";
                txtboxs.Opacity = 0.6;
                sr.Opacity = 0.6;
                
                dest = exe + "\\CustomSongs";
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
            } else
            {
                automode = false;
                txtboxs.Text = "Please choose your Song Folder";
                txtboxs.Opacity = 0.9;
                sr.Opacity = 0.9;
            }
        }

        private void Drag(object sender, RoutedEventArgs e)
        {
            bool mouseIsDown = System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;
            
            
            if (mouseIsDown)
            {
                if(draggable)
                {
                    this.DragMove();
                }
                
            }
            
        }

        public void noDrag(object sender, MouseEventArgs e)
        {
            draggable = false;
        }

        public void doDrag(object sender, MouseEventArgs e)
        {
            draggable = true;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            saveInfo();
            try
            {
                if (Directory.Exists(exe + "\\tmp")) Directory.Delete(exe + "\\tmp", true);
            } catch
            {
            }
            this.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(automode)
            {
                return;
            }
            if(Running)
            {
                return;
            }
            CommonOpenFileDialog fd = new CommonOpenFileDialog();
            fd.IsFolderPicker = true;
            if (fd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = fd.FileName;
                if(path.Contains("Debug"))
                {
                    debug = true;
                    txtbox.Text = "Output (Debugging enabled):";
                } else
                {
                    debug = false;
                    txtbox.Text = "Output:";
                }
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                txtboxs.Text = path;

            }
        }

        public void getQuestIP()
        {
            IP = Quest.Text;
            return;
        }

        public void input(String Path, String dest)
        {
            if(Running)
            {
                return;
            }
            ArrayList list = new ArrayList();
            ArrayList content = new ArrayList();
            ArrayList over = new ArrayList();
            int overwritten = 0;
            int exported = 0;
            String Name = "";
            String Source = Path;
            Running = true;

            if((bool)auto.IsChecked)
            {
                txtbox.AppendText("\nAuto Mode enabled! Copying all Songs to " + exe + "\\tmp. Please be patient.");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                if (!copied)
                {
                    if(!adb("pull /sdcard/BMBFData/CustomSongs/ \"" + exe + "\\tmp\""))
                    {
                        return;
                    }
                    copied = true;
                }
                if (Directory.Exists(exe + "\\tmp\\CustomSongs"))
                {
                    Source = exe + "\\tmp\\CustomSongs";
                } else
                {
                    Source = exe + "\\tmp";
                }
                txtbox.AppendText("\n\nZipping Songs");
                txtbox.ScrollToEnd();

            }

            string[] directories = Directory.GetDirectories(Source);



            foreach (String CD in directories)
            {
                txtbox.AppendText("\n");
          

                if (!File.Exists(CD + "\\" + "Info.dat") && !File.Exists(CD + "\\" + "info.dat"))
                {
                    txtbox.AppendText("\n" + CD + " is no Song");
                    txtbox.ScrollToEnd();
                    continue;
                }
                String dat = "";
                if (File.Exists(CD + "\\" + "Info.dat"))
                {
                    dat = CD + "\\" + "Info.dat";

                }
                if (File.Exists(CD + "\\" + "info.dat"))
                {
                    dat = CD + "\\" + "info.dat";

                }
                try
                {
                    var json = SimpleJSON.JSON.Parse(File.ReadAllText(dat));
                    Name = json["_songName"].ToString();

                    Name = Name.Replace("/", "");
                    Name = Name.Replace(":", "");
                    Name = Name.Replace("*", "");
                    Name = Name.Replace("?", "");
                    Name = Name.Replace("\"", "");
                    Name = Name.Replace("<", "");
                    Name = Name.Replace(">", "");
                    Name = Name.Replace("|", "");

                    for (int f = 0; f < Name.Length; f++)
                    {
                        if (Name.Substring(f, 1).Equals("\\"))
                        {
                            Name = Name.Substring(0, f - 1) + Name.Substring(f + 1, Name.Length - f - 1);
                        }
                    }
                    int Time = 0;
                    while (Name.Substring(Name.Length - 1, 1).Equals(" "))
                    {
                        Name = Name.Substring(0, Name.Length - 1);
                    }

                    while (list.Contains(Name.ToLower()))
                    {
                        Time++;
                        if (Time > 1)
                        {
                            Name = Name.Substring(0, Name.Length - 1);
                            Name = Name + Time;
                        }
                        else
                        {
                            Name = Name + " " + Time;
                        }

                    }
                    list.Add(Name.ToLower());
                    txtbox.AppendText("\nSong Name: " + Name);
                    txtbox.AppendText("\nFolder: " + CD);

                    if ((bool)box.IsChecked)
                    {
                        bool v = File.Exists(dest + "\\" + Name + ".zip");
                        if (v)
                        {
                            File.Delete(dest + "\\" + Name + ".zip");
                            txtbox.AppendText("\noverwritten file: " + dest + "\\" + Name + ".zip");
                            if(debug)
                            {
                                over.Add("\nSong Name: " + Name);
                                over.Add("\nFolder: " + CD);
                                over.Add("\n");
                            }
                                    
                            overwritten++;
                        }
                    }
                    else
                    {
                        bool v = File.Exists(dest + "\\" + Name + ".zip");
                        if (v)
                        {
                            txtbox.AppendText("\nthis Song already exists");
                        }
                    }

                    zip(CD, dest + "\\" + Name + ".zip");
                    exported++;
                    Name = "";
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    txtbox.ScrollToEnd();
                }
                catch
                {

                }


            }


            txtbox.AppendText("\n");
            txtbox.AppendText("\n");
            txtbox.ScrollToEnd();
            if (exported == 0 && (bool)auto.IsChecked)
            {
                txtbox.AppendText("\nerror (Code: QSU110). No Songs were zipped.");
            } else if(exported == 0 && !(bool)auto.IsChecked)
            {
                txtbox.AppendText("\nerror (Code: QSU100). No Songs were zipped.");
            }
            else
            {
                txtbox.AppendText("\nFinished! Backed up " + exported + " Songs.");
            }
            if ((bool)auto.IsChecked && dest == exe + "\\CustomSongs")
            {
                txtbox.AppendText("\nAuto Mode was enabled. Your finished Songs are at the program location in a folder named CustomSongs.");
            }
            if ((bool)box.IsChecked)
            {
                txtbox.AppendText("\nOverwritten " + overwritten + " existing zips");
                if(debug)
                {
                    txtbox.AppendText("\nOverwritten Files:\n");
                    for (int cc = 0; cc < over.Count; cc++)
                    {
                        txtbox.AppendText(" " + over[cc]);
                    }
                }
            }
            txtbox.ScrollToEnd();
            Running = false;
        }

        public static void zip(String src, String Output)
        {
            ZipFile.CreateFromDirectory(src, Output);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                return;
            }
            CommonOpenFileDialog fd = new CommonOpenFileDialog();
            fd.IsFolderPicker = true;
            if (fd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                {
                    dest = fd.FileName;
                    if (!System.IO.Directory.Exists(dest))
                    {
                        System.IO.Directory.CreateDirectory(dest);
                    }
                    txtboxd.Text = dest;

                }
            }

        }


        private void Check(object sender, RoutedEventArgs e)
        {
            if(!(bool)index.IsChecked)
            {
                index.IsChecked = true;
            }
            if((bool)auto.IsChecked)
            {
                zips.IsChecked = false;
            }
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)box.IsChecked)
            {
                index.IsChecked = false;
            }
        }

        private void Overwrite(object sender, RoutedEventArgs e)
        {
            if((bool)index.IsChecked)
            {
                box.IsChecked = false;
            }
        }

        private void Uncheck(object sender, RoutedEventArgs e)
        {
            if ((bool)zips.IsChecked)
            {
                zips.IsChecked = false;
            }
        }

        private void AutoM(object sender, RoutedEventArgs e)
        {
            if((bool)zips.IsChecked)
            {
                zips.IsChecked = false;
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                return;
            }
            txtbox.Text = "Output:";
            if (!Directory.Exists(path))
            {
                if((bool)!auto.IsChecked)
                {
                    txtbox.AppendText("\n\nChoose a Source folder!");
                    txtbox.ScrollToEnd();
                    return;
                }
            }
            if (dest == null)
            {
                dest = path;
            }
            if ((bool)index.IsChecked)
            {
                //Index Songs
                if((bool)zips.IsChecked)
                {
                    unzip(path, false);
                    
                } else
                {
                    IndexSongs(path, dest, false, false);
                }
                
            }
            else
            {
                input(path, dest);
            }
        }

        public void unzip(String Path, Boolean download)
        {
            int end;
            String f;
            string[] directories = Directory.GetFiles(Path);
            if (Directory.Exists(exe + "\\tmp\\Zips"))
            {
                Directory.Delete(exe + "\\tmp\\Zips", true);
            }
            if (!Directory.Exists(exe + "\\tmp\\Zips"))
            {
                Directory.CreateDirectory(exe + "\\tmp\\Zips");
            }
            String dest = exe + "\\tmp\\Zips";
            txtbox.AppendText("\nUnziping files to temporary folder.");
            txtbox.ScrollToEnd();

            foreach (String CD in directories)
            {
                if(!CD.EndsWith(".zip"))
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    continue;
                }
                
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate {
                    f = System.IO.Path.GetFileNameWithoutExtension(CD);
                    ZipFile.ExtractToDirectory(CD, dest + "\\" + f);
                }));

            }
            txtbox.AppendText("\nUnziping complete.");
            txtbox.ScrollToEnd();
            IndexSongs(dest, Path, true, download);
        }


        public void IndexSongs(String Path, String dest, Boolean Zips, Boolean download)
        {
            if(Running)
            {
                return;
            }
            String zip = "";
            ArrayList list = new ArrayList();
            ArrayList Folder = new ArrayList();
            ArrayList BPM = new ArrayList();
            ArrayList Author = new ArrayList();
            ArrayList SubName = new ArrayList();
            ArrayList MAuthor = new ArrayList();
            ArrayList requierments = new ArrayList();
            ArrayList requiered = new ArrayList();
            int exported = 0;
            int end = 0;
            String Name = "";
            String Source = Path;
            String B = "";
            String A = "";
            String S = "";
            String M = "";
            Running = true;

            if ((bool)auto.IsChecked)
            {
                txtbox.AppendText("\nAuto Mode enabled! Copying all Songs to " + exe + "\\tmp. Please be patient.");
                txtbox.ScrollToEnd();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                if(!copied)
                {
                    if(!adb("pull /sdcard/BMBFData/CustomSongs/ \"" + exe + "\\tmp\""))
                    {
                        return;
                    }
                    copied = true;
                }
                if (Directory.Exists(exe + "\\tmp\\CustomSongs"))
                {
                    Source = exe + "\\tmp\\CustomSongs";
                }
                else
                {
                    Source = exe + "\\tmp";
                }

            }

            string[] directories = Directory.GetDirectories(Source);



            foreach (String CD in directories)
            {
                requierments.Clear();
                //Check if Folder is Valid Song
                txtbox.AppendText("\n");
                txtbox.ScrollToEnd();

                if (!File.Exists(CD + "\\" + "Info.dat") && !File.Exists(CD + "\\" + "info.dat"))
                {
                    txtbox.AppendText("\n" + CD + " is no Song");
                    txtbox.ScrollToEnd();
                    continue;
                }
                String dat = "";
                if (File.Exists(CD + "\\" + "Info.dat"))
                {
                    dat = CD + "\\" + "Info.dat";

                }
                if (File.Exists(CD + "\\" + "info.dat"))
                {
                    dat = CD + "\\" + "info.dat";

                }


                try
                {
                    var json = SimpleJSON.JSON.Parse(File.ReadAllText(dat));
                    Name = json["_songName"].ToString();

                    Name = Name.Substring(1, Name.Length - 2);
                    while (Name.Substring(Name.Length - 1, 1).Equals(" "))
                    {
                        Name = Name.Substring(0, Name.Length - 1);
                    }

                    list.Add(Name);
                    if(download)
                    {
                        keyName = Name;
                    }
                    txtbox.AppendText("\nSong Name: " + Name);

                    if (Zips)
                    {
                        zip = dest + "\\" + System.IO.Path.GetDirectoryName(CD) + ".zip";
                        txtbox.AppendText("\nZip: " + zip);
                        Folder.Add(zip);
                    }
                    else
                    {
                        txtbox.AppendText("\nFolder: " + CD);
                        Folder.Add(CD);
                    }


                    exported++;
                    Name = "";



                    /////////Requirements

                    foreach (JSONNode modes in json["_difficultyBeatmapSets"])
                    {
                        foreach (JSONNode diff in modes["_difficultyBeatmaps"])
                        {
                            Name = diff["_customData"]["_requirements"].ToString();
                            if (Name.Contains("GameSaber"))
                            {
                                if (!requierments.Contains("GameSaber, "))
                                {
                                    requierments.Add("GameSaber, ");
                                }
                            }


                            if (Name.Contains("Mapping Extensions"))
                            {
                                if (!requierments.Contains("Mapping Extensions, "))
                                {
                                    requierments.Add("Mapping Extensions, ");
                                }

                            }

                            if (Name.Contains("Noodle Extensions"))
                            {
                                if (!requierments.Contains("Noodle Extensions, "))
                                {
                                    requierments.Add("Noodle Extensions, ");
                                }

                            }

                            if (Name.Contains("Chroma"))
                            {
                                if (!requierments.Contains("Chroma, "))
                                {
                                    requierments.Add("Chroma, ");
                                }

                            }
                        }
                    }

                    /////////Song Sub Name

                    S = json["_songSubName"];
                    if (S.Equals(""))
                    {
                        S = "N/A";
                    }


                    SubName.Add(S);
                    txtbox.AppendText("\nSubName: " + S);
                    S = "";


                    /////////Song Author

                    A = json["_songAuthorName"];

                    if (A.Equals(""))
                    {
                        A = "N/A";
                    }

                    Author.Add(A);
                    txtbox.AppendText("\nSong author: " + A);
                    A = "";
                        

                    /////////Map Author

                    M = json["_levelAuthorName"];

                    if (M.Equals(""))
                    {
                       M = "N/A";
                    }

                    MAuthor.Add(M);
                    txtbox.AppendText("\nMap author: " + M);
                    M = "";
                        

                    /////////BPM

                    B = json["_beatsPerMinute"];

                    if (!B.Equals(""))
                    {
                        B = B.Replace(",", "");
                        B = B.Replace(" ", "");
                        B = B.Replace(":", "");
                    }
                    else
                    {
                        B = "N/A";
                    }

                    BPM.Add(B);
                    txtbox.AppendText("\nBPM: " + B);
                    B = "";
                     
                        

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    txtbox.ScrollToEnd();

                    String Content = "";
                    
                    for (int a = 0; a < requierments.Count; a++)
                    {
                        Content = Content + requierments[a];
                    }
                    if(Content.Equals(""))
                    {
                        Content = "N/A";
                    }
                    if(Content.EndsWith(", "))
                    {
                        Content = Content.Substring(0, Content.Count() - 2);
                    }
                    txtbox.AppendText("\nRequierments: " + Content);
                    requiered.Add(Content);
                    txtbox.ScrollToEnd();
                }
                catch
                {
                }
            }
            ArrayList txt = new ArrayList();
            //ArrayList list = new ArrayList();
            //ArrayList Folder = new ArrayList();
            //ArrayList Version = new ArrayList();
            //ArrayList BPM = new ArrayList();
            //ArrayList Author = new ArrayList();
            //ArrayList SubName = new ArrayList();
            //ArrayList MAuthor = new ArrayList();
            txt.Add("List of " + exported + " Songs");
            txt.Add("Use ctrl + f to search for Songs");
            txt.Add("");
            txt.Add("");
            for (int C = 0; C < list.Count; C++)
            {
                txt.Add("Name: " + list[C]);
                txt.Add("Song Sub Name: " + SubName[C]);
                txt.Add("BPM: " + BPM[C]);
                txt.Add("Song Author: " + Author[C]);
                txt.Add("Map Author: " + MAuthor[C]);
                txt.Add("Requiered mods: " + requiered[C]);
                if(Zips)
                {
                    txt.Add("Zip: " + Folder[C]);
                } else
                {
                    txt.Add("Folder: " + Folder[C]);
                }
                
                txt.Add("");
            }
            String[] output = (string[])txt.ToArray(typeof(string));
            if(!download)
            {
                File.WriteAllLines(dest + "\\" + "Songs.txt", output);
                txtbox.AppendText("\n");
                txtbox.AppendText("\n");
                txtbox.AppendText("Finished! Listed " + exported + " songs in Songs.txt");
            } else
            {
                txtbox.AppendText("\n");
                txtbox.AppendText("\n");
                txtbox.AppendText("Finished! See metadata of BeatMap " + args + " above.");
            }
            

            
            if(debug)
            {
                txtbox.AppendText("\n\n");
                txtbox.AppendText("\nNames: " + list.Count);
                txtbox.AppendText("\nSongSubNames: " + SubName.Count);
                txtbox.AppendText("\nBMPs: " + BPM.Count);
                txtbox.AppendText("\nSong Authors: " + Author.Count);
                txtbox.AppendText("\nMap Authors: " + MAuthor.Count);
                txtbox.AppendText("\nRequiered Mods: " + requiered.Count);
                if(Zips)
                {
                    txtbox.AppendText("\nZips: " + Folder.Count);
                } else
                {
                    txtbox.AppendText("\nFolders: " + Folder.Count);
                }
                
            }
            if ((bool)auto.IsChecked && dest == exe + "\\CustomSongs")
            {
                txtbox.AppendText("\nAuto Mode was enabled. Your finished Songs are at the program location in a folder named CustomSongs.");
            }
            txtbox.ScrollToEnd();
            Running = false;
        }
    }
}