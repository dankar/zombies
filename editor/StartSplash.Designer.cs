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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public partial class StartSplash : System.Windows.Forms.Form
		{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
			{
			try
			{
				if (disposing && (components != null))
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		internal System.Windows.Forms.Label Version;
		internal System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
		internal System.Windows.Forms.TableLayoutPanel DetailsLayoutPanel;
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
			{
                this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.DetailsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.Version = new System.Windows.Forms.Label();
                this.MainLayoutPanel.SuspendLayout();
                this.SuspendLayout();
                // 
                // MainLayoutPanel
                // 
                this.MainLayoutPanel.BackgroundImage = global::My.Resources.Resources.org_gpme_splash;
                this.MainLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.MainLayoutPanel.ColumnCount = 2;
                this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
                this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 253F));
                this.MainLayoutPanel.Controls.Add(this.DetailsLayoutPanel, 1, 1);
                this.MainLayoutPanel.Controls.Add(this.Version, 1, 0);
                this.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
                this.MainLayoutPanel.Name = "MainLayoutPanel";
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
                this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
                this.MainLayoutPanel.Size = new System.Drawing.Size(496, 303);
                this.MainLayoutPanel.TabIndex = 0;
                this.MainLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainLayoutPanel_Paint);
                this.MainLayoutPanel.Click += new System.EventHandler(this.MainLayoutPanel_Click);
                // 
                // DetailsLayoutPanel
                // 
                this.DetailsLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.DetailsLayoutPanel.BackColor = System.Drawing.Color.Transparent;
                this.DetailsLayoutPanel.ColumnCount = 1;
                this.DetailsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247F));
                this.DetailsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
                this.DetailsLayoutPanel.Location = new System.Drawing.Point(246, 221);
                this.DetailsLayoutPanel.Name = "DetailsLayoutPanel";
                this.DetailsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
                this.DetailsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
                this.DetailsLayoutPanel.Size = new System.Drawing.Size(247, 79);
                this.DetailsLayoutPanel.TabIndex = 1;
                // 
                // Version
                // 
                this.Version.Anchor = System.Windows.Forms.AnchorStyles.Top;
                this.Version.BackColor = System.Drawing.Color.Transparent;
                this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.Version.ForeColor = System.Drawing.Color.White;
                this.Version.Location = new System.Drawing.Point(249, 0);
                this.Version.Name = "Version";
                this.Version.Size = new System.Drawing.Size(241, 20);
                this.Version.TabIndex = 1;
                this.Version.Text = "Version {0}.{1:00}";
                this.Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
                this.Version.Click += new System.EventHandler(this.Version_Click);
                // 
                // StartSplash
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(496, 303);
                this.ControlBox = false;
                this.Controls.Add(this.MainLayoutPanel);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "StartSplash";
                this.ShowInTaskbar = false;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Load += new System.EventHandler(this.StartSplash_Load);
                this.MainLayoutPanel.ResumeLayout(false);
                this.ResumeLayout(false);

		}
		
	}
	
}
