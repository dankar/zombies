using System.Diagnostics;
using System;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Map_Editor
{
	public sealed partial class StartSplash
	{
		public StartSplash()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
		#region Default Instance
		
		private static StartSplash defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static StartSplash Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new StartSplash();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
				
				return defaultInstance;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
		
		#endregion
		
		public void MainLayoutPanel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
        public void Version_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

		private void ApplicationTitle_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		private void Copyright_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
        private void CloseForm(System.Object sender, System.EventArgs e)
        {

            this.Close();
        }
        private void StartSplash_Load(object sender, EventArgs e)
        {
            Version.Text = System.String.Format(Version.Text, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.Major, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.Minor);

            //- Close after maximum 3 seconds
            Timer tCloseTimer = new Timer();
            tCloseTimer.Interval =1500;
            tCloseTimer.Tick += new System.EventHandler(this.CloseForm);
            tCloseTimer.Start();
        }
        private void MainLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

	}
	
}
