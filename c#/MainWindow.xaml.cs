using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;   
using System.Collections.Generic;
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
        int MinorV = 7;
        int PatchV = 0;

        String IP = "";
        String path;
        String dest;
        Boolean debug = false;
        Boolean automode = false;
        Boolean copied = false;
        Boolean draggable = true;
        String exe = System.Reflection.Assembly.GetEntryAssembly().Location;


        public MainWindow()
        {
            InitializeComponent();
            UpdateB.Visibility = Visibility.Hidden;
            exe = exe.Replace("\\Quest Song Exporter.exe", "");
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
            if(File.Exists(exe + "\\QSE_Update.exe"))
            {
                File.Delete(exe + "\\QSE_Update.exe");
            }
            Update();
            

        }

        public void Update()
        {
            try
            {
                //Download Update.txt
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/ComputerElite/QSE/master/Update.txt", exe + "\\tmp\\Update.txt");
                }
                StreamReader VReader = new StreamReader(exe + "\\tmp\\Update.txt");

                String line;
                int l = 0;

                int MajorU = 0;
                int MinorU = 0;
                int PatchU = 0;
                while ((line = VReader.ReadLine()) != null)
                {
                    if (l == 0)
                    {
                        String URL = line;
                    }
                    if(l == 1)
                    {
                        MajorU = Convert.ToInt32(line);
                    }
                    if(l == 2)
                    {
                        MinorU = Convert.ToInt32(line);
                    }
                    if(l == 3)
                    {
                        PatchU = Convert.ToInt32(line);
                    }
                    l++;
                }

                if(MajorU > MajorV || MinorU > MinorV || PatchU > PatchV)
                {
                    UpdateB.Visibility = Visibility.Visible;
                }

            } catch
            {

            }
        }

        private void Start_Update(object sender, RoutedEventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://github.com/ComputerElite/QSE/raw/master/QSE_Update.exe", exe + "\\QSE_Update.exe");
            }
            //Process.Start(exe + "\\QSE_Update.exe");
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = exe + "\\QSE_Update.exe";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(s))
                {
                }
                this.Close();
            }
            catch
            {
                // Log error.
                txtbox.AppendText("\nAn Error Occured");
            }
        }

        private void Mini(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void Backup(object sender, RoutedEventArgs e)
        {
            try
            {
                txtbox.Text = "Output:";
                getQuestIP();
                if (dest == null)
                {
                    dest = exe + "\\CustomSongs";
                    if (!Directory.Exists(exe + "\\CustomSongs"))
                    {
                        Directory.CreateDirectory(exe + "\\CustomSongs");
                    }
                }

                txtbox.AppendText("\n\nBacking up Playlist to " + dest + "\\Playlists.json");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));



                if (!Directory.Exists(exe + "\\tmp"))
                {
                    Directory.CreateDirectory(exe + "\\tmp");
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("http://" + IP + ":50000/host/beatsaber/config", exe + "\\tmp\\Config.json");
                }


                String Config = exe + "\\tmp\\config.json";



                StreamReader reader = new StreamReader(@Config);
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    int Index = line.IndexOf("\"Mods\":[{", 0, line.Length);
                    String Playlists = line.Substring(0, Index);
                    File.WriteAllText(dest + "\\Playlists.json", Playlists);
                }
                txtbox.AppendText("\n\nBacked up Playlists to " + dest + "\\Playlists.json");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            } catch
            {
                txtbox.AppendText("\n\n\nAn error occured. Check following:");
                txtbox.AppendText("\n\n- You put in the Quests IP right.");
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
            try
            {


                getQuestIP();
                txtbox.Text = "Output:";

                if (dest == null)
                {
                    dest = path;
                }

                txtbox.AppendText("\n\nRestoring Playlist from " + dest + "\\Playlists.json");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));



                if (!Directory.Exists(exe + "\\tmp"))
                {
                    Directory.CreateDirectory(exe + "\\tmp");
                }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("http://" + IP + ":50000/host/beatsaber/config", exe + "\\tmp\\OConfig.json");
                }

                String Config = exe + "\\tmp\\OConfig.json";

                String Playlists;

                if (Directory.Exists(path))
                {
                    Playlists = path + "\\Playlists.json";
                }
                else
                {
                    Playlists = exe + "\\CustomSongs\\Playlists.json";
                }



                StreamReader reader = new StreamReader(@Config);
                String line;
                String CContent = "";
                int Index = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    Index = line.IndexOf("\"Mods\":", 0, line.Length);
                    CContent = line.Substring(Index, line.Length - Index);


                    //9
                }

                StreamReader Preader = new StreamReader(@Playlists);
                String Pline;
                String Content = "";
                while ((Pline = Preader.ReadLine()) != null)
                {
                    Content = Pline;
                }

                String finished = Content + CContent;

                JObject o = JObject.Parse(finished);
                o.Property("SyncConfig").Remove();
                o.Property("IsCommitted").Remove();
                o.Property("BeatSaberVersion").Remove();

                JProperty lrs = o.Property("Config");
                o.Add(lrs.Value.Children<JProperty>());
                lrs.Remove();

                String FConfig = o.ToString();
                File.WriteAllText(exe + "\\tmp\\config.json", FConfig);

                postChanges(exe + "\\tmp\\config.json");
                txtbox.AppendText("\n\nRestored old Playlists.");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            } catch
            {
                txtbox.AppendText("\n\n\nAn error occured. Check following:");
                txtbox.AppendText("\n\n- Your Quest is on and BMBF opened");
                txtbox.AppendText("\n\n- You put in the Quests IP right.");
                txtbox.AppendText("\n\n- You've choosen the right Source path");
            }
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            Quest.Text = "";
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
            if(Directory.Exists(exe + "\\tmp")) {
                Directory.Delete(exe + "\\tmp", true);
            }
            this.Close();
        }


            private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(automode)
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

        public void CopySongs(String Desktop)
        {
            if(copied)
            {
                return;
            }
            ProcessStartInfo s = new ProcessStartInfo();
            s.CreateNoWindow = false;
            s.UseShellExecute = false;
            s.FileName = "adb.exe";
            s.WindowStyle = ProcessWindowStyle.Minimized;
            s.Arguments = "pull /sdcard/BMBFData/CustomSongs/ \"" + Desktop + "\"";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(s))
                {
                    exeProcess.WaitForExit();
                    copied = true;
                }
            }
            catch
            {
                // Log error.
                txtbox.AppendText("\nAn Error Occured");
            }
        }

        public void input(String Path, String dest)
        {

            ArrayList list = new ArrayList();
            ArrayList content = new ArrayList();
            ArrayList over = new ArrayList();
            int overwritten = 0;
            int exported = 0;
            String Name = "";
            String Source = Path;

            if((bool)auto.IsChecked)
            {
                txtbox.AppendText("\nAuto Mode enabled! Copying all Songs to " + exe + "\\tmp. Please be patient.");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                CopySongs(exe + "\\tmp");
                Source = exe + "\\tmp";
                
            }

            string[] directories = Directory.GetDirectories(Source);



            for (int i = 0; i < directories.Length; i++)
            {
                txtbox.AppendText("\n");
                //System.out.println("");

                //if(directories[i].Substring(directories[i].Length-4, directories[i].Length).(".zip")) {
                //	System.out.println("File "+directories[i]+" is already zipped");
                //	continue;
                //}

                //File Path = new File(Source + File.separator + directories[i]);
                //if(!Path.isDirectory()) {
                //	System.out.println(directories[i]+" is no Song");
                //	continue;
                //}

                if (!File.Exists(directories[i] + "\\" + "Info.dat") && !File.Exists(directories[i] + "\\" + "info.dat"))
                {
                    txtbox.AppendText("\n" + directories[i] + " is no Song");
                    continue;
                }
                String dat = "";
                if (File.Exists(directories[i] + "\\" + "Info.dat"))
                {
                    dat = directories[i] + "\\" + "Info.dat";

                }
                if (File.Exists(directories[i] + "\\" + "info.dat"))
                {
                    dat = directories[i] + "\\" + "info.dat";

                }
                try
                {
                    StreamReader reader = new StreamReader(@dat);
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        if (line.Contains("_songName"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                Name = Strings(line, 7);

                                Name = Name.Substring(0, Name.Length - 1);

                                //Name = Name.replaceAll("[\\]", "");
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
                                txtbox.AppendText("\nFolder name: " + directories[i]);
                                //File sour = new File(Source);
                                //File src = new File(sour + File.separator + directories[i] + File.separator);
                                //ZipIt.zipDirectory(src, Source + File.separator + Name + ".zip");

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
                                            over.Add("\nFolder Name: " + directories[i]);
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

                                zip(directories[i], dest + "\\" + Name + ".zip");
                                exported++;
                                Name = "";
                                //src = new File("");

                            }
                            else
                            {
                                //normal Map
                                Name = Strings(line, 3);

                                Name = Name.Substring(0, Name.Length - 1);

                                //Name = Name.replaceAll("[\\]", "");
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
                                txtbox.AppendText("\nFolder name: " + directories[i]);
                                //File sour = new File(Source);
                                //File src = new File(sour + File.separator + directories[i] + File.separator);
                                //ZipIt.zipDirectory(src, Source + File.separator + Name + ".zip");

                                if ((bool)box.IsChecked)
                                {
                                    bool v = File.Exists(dest + "\\" + Name + ".zip");
                                    if (v)
                                    {
                                        File.Delete(dest + "\\" + Name + ".zip");
                                        txtbox.AppendText("\noverwritten file: " + dest + "\\" + Name + ".zip");
                                        if (debug)
                                        {
                                            over.Add("\nSong Name: " + Name);
                                            over.Add("\nFolder Name: " + directories[i]);
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

                                zip(directories[i], dest + "\\" + Name + ".zip");
                                exported++;
                                Name = "";
                                //src = new File("");

                            }
                        }
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                        txtbox.ScrollToEnd();

                    }
                    reader.Close();
                }
                catch
                {

                }


            }


            //ZipIt.zipDirectory(src, Output);;
            txtbox.AppendText("\n");
            txtbox.AppendText("\n");
            txtbox.AppendText("\nFinished! Exported " + exported + " Songs");
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
        }

        public static void zip(String src, String Output)
        {
            ZipFile.CreateFromDirectory(src, Output);

        }

        public String Strings(String line, int StartIndex)
        {
            int count = 0;
            String Name = "";
            for (int n = 0; n < line.Length; n++)
            {
                if (count == StartIndex)
                {

                    Name = Name + line.Substring(n, 1);
                }

                if (line.Substring(n, 1).Equals("\""))
                {
                    count++;
                }
            }
            return Name;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fd = new CommonOpenFileDialog();
            fd.IsFolderPicker = true;
            if (fd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                {
                    dest = fd.FileName;
                    dest = dest.Replace("\\select.directory", "");
                    if (!System.IO.Directory.Exists(dest))
                    {
                        System.IO.Directory.CreateDirectory(dest);
                    }
                    txtboxd.Text = dest;

                }
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            txtbox.Text = "Output:";
            if (dest == null)
            {
                dest = path;
            }
            if ((bool)index.IsChecked)
            {
                //Index Songs
                IndexSongs(path, dest);
            }
            else
            {
                input(path, dest);
            }
        }


        public void IndexSongs(String Path, String dest)
        {
            ArrayList list = new ArrayList();
            ArrayList Folder = new ArrayList();
            ArrayList Version = new ArrayList();
            ArrayList BPM = new ArrayList();
            ArrayList Author = new ArrayList();
            ArrayList SubName = new ArrayList();
            ArrayList MAuthor = new ArrayList();
            ArrayList requierments = new ArrayList();
            ArrayList requiered = new ArrayList();
            int exported = 0;
            String Name = "";
            String Source = Path;
            String V = "";
            String B = "";
            String A = "";
            String S = "";
            String M = "";
            Boolean custom = false;

            if ((bool)auto.IsChecked)
            {
                txtbox.AppendText("\nAuto Mode enabled! Copying all Songs to " + exe + "\\tmp. Please be patient.");
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                CopySongs(exe + "\\tmp");
                Source = exe + "\\tmp";

            }

            string[] directories = Directory.GetDirectories(Source);



            for (int i = 0; i < directories.Length; i++)
            {
                custom = false;
                requierments.Clear();
                //Check if Folder is Valid Song
                txtbox.AppendText("\n");

                if (!File.Exists(directories[i] + "\\" + "Info.dat") && !File.Exists(directories[i] + "\\" + "info.dat"))
                {
                    txtbox.AppendText("\n" + directories[i] + " is no Song");
                    continue;
                }
                String dat = "";
                if (File.Exists(directories[i] + "\\" + "Info.dat"))
                {
                    dat = directories[i] + "\\" + "Info.dat";

                }
                if (File.Exists(directories[i] + "\\" + "info.dat"))
                {
                    dat = directories[i] + "\\" + "info.dat";

                }


                try
                {
                    StreamReader reader = new StreamReader(@dat);
                    String line;
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        /////////CustomData

                        if (line.Contains("_customData"))
                        {
                            custom = true;
                        }

                        

                        /////////Song Name

                        if (line.Contains("_songName"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                Name = Strings(line, 7);

                                Name = Name.Substring(0, Name.Length - 1);

                                while (Name.Substring(Name.Length - 1, 1).Equals(" "))
                                {
                                    Name = Name.Substring(0, Name.Length - 1);
                                }

                                list.Add(Name);
                                Folder.Add(directories[i]);
                                txtbox.AppendText("\nSong Name: " + Name);
                                txtbox.AppendText("\nFolder name: " + directories[i]);

                                exported++;
                                Name = "";

                            }
                            else
                            {
                                //Normal Map
                                Name = Strings(line, 3);

                                Name = Name.Substring(0, Name.Length - 1);

                                while (Name.Substring(Name.Length - 1, 1).Equals(" "))
                                {
                                    Name = Name.Substring(0, Name.Length - 1);
                                }

                                list.Add(Name);
                                Folder.Add(directories[i]);
                                txtbox.AppendText("\nSong Name: " + Name);
                                txtbox.AppendText("\nFolder name: " + directories[i]);

                                exported++;
                                Name = "";
                            }
                        }

                        /////////Requirements

                        if (line.Contains("GameSaber"))
                        {
                            if(!requierments.Contains("GameSaber, "))
                            {
                                requierments.Add("GameSaber, ");
                            }

                        }

                        if (line.Contains("Mapping Extensions"))
                        {
                            if (!requierments.Contains("Mapping Extensions, "))
                            {
                                requierments.Add("Mapping Extensions, ");
                            }

                        }

                        if (line.Contains("Noodle Extensions"))
                        {
                            if (!requierments.Contains("Noodle Extensions, "))
                            {
                                requierments.Add("Noodle Extensions, ");
                            }

                        }

                        if (line.Contains("Chroma"))
                        {
                            if (!requierments.Contains("Chroma, "))
                            {
                                requierments.Add("Chroma, ");
                            }

                        }

                        /////////Version

                        if (line.Contains("_version"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                V = Strings(line, 3);

                                V = V.Substring(0, V.Length - 1);

                                Version.Add(V);
                                txtbox.AppendText("\nVersion: " + V);
                                V = "";
                            } 
                            else
                            {
                                if(!custom)
                                {
                                    //normal Map
                                    V = Strings(line, 3);

                                    V = V.Substring(0, V.Length - 1);

                                    Version.Add(V);
                                    txtbox.AppendText("\nVersion: " + V);
                                    V = "";
                                }
                            }

                                
                        }

                        /////////Song Sub Name

                        if (line.Contains("_songSubName"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                S = Strings(line, 11);
                                if (!S.Equals("\""))
                                {
                                    S = S.Substring(0, S.Length - 1);
                                }
                                else
                                {
                                    S = "N/A";
                                }


                                SubName.Add(S);
                                txtbox.AppendText("\nSubName: " + S);
                                S = "";
                            }
                            else
                            {
                                //normal map
                                S = Strings(line, 3);

                                if (!S.Equals("\""))
                                {
                                    S = S.Substring(0, S.Length - 1);
                                }
                                else
                                {
                                    S = "N/A";
                                }

                                SubName.Add(S);
                                txtbox.AppendText("\nSubName: " + S);
                                S = "";
                            }
                        }

                        /////////Song Author

                        if (line.Contains("_songAuthorName"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                A = Strings(line, 11);

                                if (!A.Equals("\""))
                                {
                                    A = A.Substring(0, A.Length - 1);
                                }
                                else
                                {
                                    A = "N/A";
                                }

                                Author.Add(A);
                                txtbox.AppendText("\nSong author: " + A);
                                A = "";
                            }
                            else
                            {
                                //normal map
                                A = Strings(line, 3);

                                if (!A.Equals("\""))
                                {
                                    A = A.Substring(0, A.Length - 1);
                                }
                                else
                                {
                                    A = "N/A";
                                }

                                Author.Add(A);
                                txtbox.AppendText("\nSong author: " + A);
                                A = "";
                            }
                        }

                        /////////Map Author

                        if (line.Contains("_levelAuthorName"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                M = Strings(line, 19);

                                if (!M.Equals("\""))
                                {
                                    M = M.Substring(0, M.Length - 1);
                                }
                                else
                                {
                                    M = "N/A";
                                }

                                MAuthor.Add(M);
                                txtbox.AppendText("\nMap author: " + M);
                                M = "";
                            }
                            else
                            {
                                //normal map
                                M = Strings(line, 3);

                                if (!M.Equals("\""))
                                {
                                    M = M.Substring(0, M.Length - 1);
                                }
                                else
                                {
                                    M = "N/A";
                                }

                                MAuthor.Add(M);
                                txtbox.AppendText("\nMap author: " + M);
                                M = "";
                            }
                        }

                        /////////BPM

                        if (line.Contains("_beatsPerMinute"))
                        {
                            if (line.Contains("_version") && line.Contains("songName"))
                            {
                                //BeatSage
                                B = Strings(line, 22);

                                if (!B.Equals("\""))
                                {
                                    B = B.Substring(1, B.Length - 2);
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
                            }
                            else
                            {
                                //normal map
                                B = Strings(line, 2);

                                if (!B.Equals("\""))
                                {
                                    B = B.Substring(1, B.Length - 2);
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
                            }
                        }
                        

                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                        txtbox.ScrollToEnd();

                    }
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
                    reader.Close();
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
                txt.Add("Map Version: " + Version[C]);
                txt.Add("Requiered mods: " + requiered[C]);
                txt.Add("Folder: " + Folder[C]);
                txt.Add("");
            }
            String[] output = (string[])txt.ToArray(typeof(string));
            File.WriteAllLines(dest + "\\" + "Songs.txt", output);

            txtbox.AppendText("\n");
            txtbox.AppendText("\n");
            txtbox.AppendText("Finished! Listed " + exported + " songs in Songs.txt");
            if ((bool)auto.IsChecked && dest == exe + "\\CustomSongs")
            {
                txtbox.AppendText("\nAuto Mode was enabled. Your finished Songs are at the program location in a folder named CustomSongs.");
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}