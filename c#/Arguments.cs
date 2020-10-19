using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Quest_Song_Exporter
{
	public partial class App : Application
	{

		public void Application_StartupAsync(object sender, StartupEventArgs e)
		{
			MainWindow wnd = new MainWindow();

			if (e.Args.Length == 1)
            {
				wnd.KeyAsync(e.Args[0]);
			}
			wnd.Show();
		}
	}
}