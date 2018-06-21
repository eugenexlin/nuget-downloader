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
	public partial class DownloadInputForm : Form
	{

		public DownloadInputForm()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			LoadSettings();
		}

		private void LoadSettings()
		{
			txtRemoteNugetPath.Text = Properties.Settings.Default.RemoteNugetPath;
			txtLocalNugetPath.Text = Properties.Settings.Default.LocalNugetPath;
			txtStagingNugetPath.Text = Properties.Settings.Default.StagingNugetPath;
			txtOutputReportPath.Text = Properties.Settings.Default.OutputReportPath;
			txtFramework.Text = Properties.Settings.Default.TargetFramework;
			txtFrameworkVersion.Text = Properties.Settings.Default.TargetFrameworkVersion;
		}

		private bool ValidateAll()
		{
			StringBuilder errors = new StringBuilder();
			String error;

			error = ValidatePath(txtLocalNugetPath.Text);
			if (error.Length > 0)
			{
				errors.AppendLine(error);
			}
			error = ValidatePath(txtStagingNugetPath.Text);
			if (error.Length > 0)
			{
				errors.AppendLine(error);
			}
			error = ValidatePath(txtOutputReportPath.Text);
			if (error.Length > 0)
			{
				errors.AppendLine(error);
			}
			if (errors.Length > 0)
			{
				MessageBox.Show(errors.ToString());
				return false;
			}
			return true;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			// validations
			if(!ValidateAll())
			{
				return;
			}

			Properties.Settings.Default.RemoteNugetPath = txtRemoteNugetPath.Text;
			Properties.Settings.Default.LocalNugetPath = txtLocalNugetPath.Text;
			Properties.Settings.Default.StagingNugetPath = txtStagingNugetPath.Text;
			Properties.Settings.Default.OutputReportPath = txtOutputReportPath.Text;
			Properties.Settings.Default.TargetFramework = txtFramework.Text;
			Properties.Settings.Default.TargetFrameworkVersion = txtFrameworkVersion.Text;
			Properties.Settings.Default.Save();
			LoadSettings();
		}

		private void txtPath_Leave(object sender, EventArgs e)
		{
			TextLeaveHandler(((TextBox)sender).Text);
		}

		private void TextLeaveHandler(string text)
		{
			string errors = ValidatePath(text);
			if (errors.Length > 0)
			{
				MessageBox.Show(errors);
				return;
			}
			else
			{
				if (!Directory.Exists(text))
				{
					PromptCreatePath(text);
					return;
				}
			}
		}

		private bool PromptCreatePath(string path)
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
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool ValidatePathExists(string path)
		{
			if (!Directory.Exists(path))
			{
				MessageBox.Show(String.Format("Path '{0}' does not exist.", path));
				return false;
			}
			return true;
		}

		private string ValidatePath(string path)
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
			LoadSettings();
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			if (!ValidateAll())
			{
				return;
			}
			if (!ValidatePathExists(txtLocalNugetPath.Text))
			{
				if (!PromptCreatePath(txtLocalNugetPath.Text)){
					return;
				}
			}
			if (!ValidatePathExists(txtStagingNugetPath.Text))
			{
				if (!PromptCreatePath(txtStagingNugetPath.Text))
				{
					return;
				}
			}
			if (!ValidatePathExists(txtOutputReportPath.Text))
			{
				if (!PromptCreatePath(txtOutputReportPath.Text))
				{
					return;
				}
			}

			NugetManagerParams p = new NugetManagerParams();
			p.localNugetPath = txtLocalNugetPath.Text;
			p.stagingNugetPath = txtStagingNugetPath.Text;
			p.remoteNugetPath = txtRemoteNugetPath.Text;
			p.outputReportPath = txtOutputReportPath.Text;

			p.framework = txtFramework.Text;
			p.frameworkVersion = txtFrameworkVersion.Text;

			char[] split = { '\n' };
			Nuget nuget;
			foreach (string sNuget in txtNugets.Text.Split(split, StringSplitOptions.RemoveEmptyEntries)) {
				if (Nuget.TryParse(sNuget, out nuget)) {
					p.nugetsToDownload.Add(nuget);
				}
			}

			DownloadDashboard form = new DownloadDashboard(p);
			form.Show();
		}
	}
}
