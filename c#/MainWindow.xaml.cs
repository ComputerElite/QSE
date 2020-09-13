using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
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

namespace Quest_Song_Exporter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			txtbox.Text = "After Selecting the Song Folder the program is working. The duration without any output depends on how many songs you have.";


		}

		private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Directory|*this.directory";
            fd.FileName = "select";
            if (fd.ShowDialog() == true)
            {
                String path = fd.FileName;
                path = path.Replace("\\select.directory", "");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
				input(path);

            }
        }

        public void input(String Path)
        {
            ArrayList list = new ArrayList();
            ArrayList content = new ArrayList();
            int count = 0;
            int exported = 0;
            String Name = "";
            String Source = Path;

            string[] directories = Directory.GetDirectories(Path);



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
				if(File.Exists(directories[i] + "\\" + "Info.dat"))
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
						
						if (line.Contains("songName"))
						{
							if (line.Contains("version") && line.Contains("songName"))
							{
								//BeatSage
								count = 0;
								Name = "";
								for (int n = 0; n < line.Length; n++)
								{
									if (count == 7)
									{

										Name = Name + line.Substring(n, 1);
									}

									if (line.Substring(n, 1).Equals("\""))
									{
										count++;
									}
								}

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

								while (list.Contains(Name))
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
								list.Add(Name);
								txtbox.AppendText("\nSong Name: " + Name);
								txtbox.AppendText("\nFolder name: " + directories[i]);
								//File sour = new File(Source);
								//File src = new File(sour + File.separator + directories[i] + File.separator);
								//ZipIt.zipDirectory(src, Source + File.separator + Name + ".zip");
								zip(directories[i], Source + "\\" + Name + ".zip");
								exported++;
								Name = "";
								//src = new File("");

							}
							else
							{
								//normal Map
								count = 0;
								Name = "";
								for (int n = 0; n < line.Length; n++)
								{
									if (count == 3)
									{
										
										Name = Name + line.Substring(n, 1);
									}
									
									if (line.Substring(n, 1).Equals("\""))
									{
										count++;
									}
								}
								
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
								
								while (list.Contains(Name))
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
								list.Add(Name);
								txtbox.AppendText("\nSong Name: " + Name);
								txtbox.AppendText("\nFolder name: " + directories[i]);
								//File sour = new File(Source);
								//File src = new File(sour + File.separator + directories[i] + File.separator);
								//ZipIt.zipDirectory(src, Source + File.separator + Name + ".zip");
								zip(directories[i], Source + "\\" + Name + ".zip");
								exported++;
								Name = "";
								//src = new File("");

							}
						}
						

					}
					reader.Close();
				} catch
                {

                }


			}


			//ZipIt.zipDirectory(src, Output);;
			txtbox.AppendText("\n");
			txtbox.AppendText("\n");
			txtbox.AppendText("\nFinished! Exported " + exported + " Songs");
			txtbox.ScrollToEnd();
		}

		public static void zip(String src, String Output)
        {
			ZipFile.CreateFromDirectory(src, Output);

		}


	}
}
