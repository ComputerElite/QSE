﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using System.Windows.Threading;

namespace Quest_Song_Exporter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		String path;
		String dest;

        public MainWindow()
        {
            InitializeComponent();
			txtbox.Text = "Output:";
			txtboxd.Text = "Please choose your destination folder.";
			txtboxs.Text = "Please choose your Song folder.";

		}

		private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Directory|*this.directory";
            fd.FileName = "select";
            if (fd.ShowDialog() == true)
            {
                path = fd.FileName;
                path = path.Replace("\\select.directory", "");
				if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
				txtboxs.Text = path;


			}
        }

        public void input(String Path, String dest)
        {

            ArrayList list = new ArrayList();
            ArrayList content = new ArrayList();
            int count = 0;
			int overwritten = 0;
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

								if ((bool)box.IsChecked)
								{
									bool v = File.Exists(dest + "\\" + Name + ".zip");
									if (v)
									{
										File.Delete(dest + "\\" + Name + ".zip");
										txtbox.AppendText("\noverwritten file: " + dest + "\\" + Name + ".zip");
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

                                if ((bool)box.IsChecked)
                                {
                                    bool v = File.Exists(dest + "\\" + Name + ".zip");
                                    if (v)
                                    {
										File.Delete(dest + "\\" + Name + ".zip");
										txtbox.AppendText("\noverwritten file: " + dest + "\\" + Name + ".zip");
										overwritten++;
									}
                                } else
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
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,new Action(delegate { }));
						txtbox.ScrollToEnd();

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
			if((bool)box.IsChecked) {
				txtbox.AppendText("\nOverwritten " + overwritten + " existing zips");
			}
			txtbox.ScrollToEnd();
		}

		public static void zip(String src, String Output)
        {
			ZipFile.CreateFromDirectory(src, Output);

		}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
			SaveFileDialog fd = new SaveFileDialog();
			fd.Filter = "Directory|*this.directory";
			fd.FileName = "select";
			if (fd.ShowDialog() == true)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
			txtbox.Text = "Output:";
			if(dest == null)
            {
				dest = path;
            }
			input(path, dest);
        }
    }
}
