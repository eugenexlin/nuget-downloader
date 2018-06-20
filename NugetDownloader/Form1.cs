using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NugetDownloader
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			loadSettings();
		}

		private void loadSettings()
		{
			txtRemoteNugetPath.Text = Properties.Settings.Default.RemoteNugetPath;
			txtLocalNugetPath.Text = Properties.Settings.Default.LocalNugetPath;
			txtStagingNugetPath.Text = Properties.Settings.Default.StagingNugetPath;
			txtOutputReportPath.Text = Properties.Settings.Default.OutputReportPath;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			// validations
			string errors;
			
			errors = validatePath(txtLocalNugetPath.Text);
			if (errors.Length > 0)
			{
				MessageBox.Show(errors);
				return;
			}
			errors = validatePath(txtStagingNugetPath.Text);
			if (errors.Length > 0)
			{
				MessageBox.Show(errors);
				return;
			}
			errors = validatePath(txtOutputReportPath.Text);
			if (errors.Length > 0)
			{
				MessageBox.Show(errors);
				return;
			}

			Properties.Settings.Default.RemoteNugetPath = txtRemoteNugetPath.Text;
			Properties.Settings.Default.LocalNugetPath = txtLocalNugetPath.Text;
			Properties.Settings.Default.StagingNugetPath = txtStagingNugetPath.Text;
			Properties.Settings.Default.OutputReportPath = txtOutputReportPath.Text;
			Properties.Settings.Default.Save();
			loadSettings();
		}

		private void txtPath_Leave(object sender, EventArgs e)
		{
			textLeaveHandler(((TextBox)sender).Text);
		}

		private void textLeaveHandler(string text)
		{
			string errors = validatePath(text);
			if (errors.Length > 0)
			{
				MessageBox.Show(errors);
				return;
			}
			else
			{
				if (!Directory.Exists(text))
				{
					promptCreatePath(text);
					return;
				}
			}
		}

		private void promptCreatePath(string path)
		{
			var confirmResult = MessageBox.Show(
									String.Format(
										"Would you like to create the path '{0}' ?",
										path
									),
									"Auto-generate Path",
									MessageBoxButtons.YesNo
								);
			if (confirmResult == DialogResult.Yes)
			{
				Directory.CreateDirectory(path);
			}
			else
			{
				
			}
		}


		private string validatePath(string path)
		{
			StringBuilder errors = new StringBuilder();
			try
			{
				Path.GetFullPath(path);
			}
			catch
			{
				errors.AppendLine(
					String.Format(
						"Path '{0}' is invalid.",
						path
					)
				);
			}
			if (!Path.IsPathRooted(path))
			{
				errors.AppendLine(
					String.Format(
						"Path '{0}' needs to be an absolute path.",
						path
					)
				);
			}
			return errors.ToString();
		}

		private void btnDefault_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Reset();
			Properties.Settings.Default.Save();
			loadSettings();
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{

		}
	}
}
