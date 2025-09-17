using Humanizer;
using Microsoft.VisualBasic.Devices;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ComputerInfo
{
    public partial class ComputerInfoBox : Form
    {
        private string CallPS(string CommandLine)
        {
            var PSInstance = PowerShell.Create();

            // This is a wrapper to automatically load the PowerShell 7 built in modules. We can not always find PowerShell 7 modules from local installed hosts, so include our own.
            DirectoryInfo moduleRoot = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string[] moduleFiles = [
                "Microsoft.Management.Infrastructure.CimCmdlets.dll",
                "Microsoft.PowerShell.Commands.Diagnostics.dll",
                "Microsoft.PowerShell.Commands.Management.dll",
                "Microsoft.PowerShell.Commands.Utility.dll"
            ];

            foreach (string file in moduleFiles)
            {
                PSInstance.AddScript($"Import-Module \"{moduleRoot.FullName}\\{file}\" -ErrorAction SilentlyContinue");
            }
            

            PSInstance.AddScript(CommandLine);

            Collection<PSObject> psOutput = PSInstance.Invoke();
            StringBuilder stringBuilder = new();
            foreach (PSObject outputItem in psOutput)
            {
                if (!(string.IsNullOrEmpty(outputItem.BaseObject.ToString())))
                {
                    stringBuilder.AppendLine(outputItem.BaseObject.ToString());
                }
                else if (ConfigurationManager.AppSettings["IncludeDebugOutput"] == "true")
                {
                    foreach (DebugRecord dbg in PSInstance.Streams.Debug)
                    {
                        stringBuilder.AppendLine("Debug");
                        stringBuilder.AppendLine(dbg.ToString());
                    }
                    foreach (ErrorRecord err in PSInstance.Streams.Error)
                    {
                        stringBuilder.AppendLine("Error");
                        stringBuilder.AppendLine(err.ToString());
                    }
                    foreach (InformationRecord info in PSInstance.Streams.Information)
                    {
                        stringBuilder.AppendLine("Information");
                        stringBuilder.AppendLine(info.ToString());
                    }
                    foreach (ProgressRecord prog in PSInstance.Streams.Progress)
                    {
                        stringBuilder.AppendLine("Progress");
                        stringBuilder.AppendLine(prog.ToString());
                    }
                    foreach (VerboseRecord vrb in PSInstance.Streams.Verbose)
                    {
                        stringBuilder.AppendLine("Verbose");
                        stringBuilder.AppendLine(vrb.ToString());
                    }
                    foreach (WarningRecord wrn in PSInstance.Streams.Warning)
                    {
                        stringBuilder.AppendLine("Warning");
                        stringBuilder.AppendLine(wrn.ToString());
                    }
                }
            }

            string output = stringBuilder.ToString();
            return output;
        }

        public ComputerInfoBox()
        {
            Dictionary<string, string> psScriptDict = new Dictionary<string, string>();
            Dictionary<string, string> informationDict = new Dictionary<string, string>();

            string filepath = ConfigurationManager.AppSettings["PowerShellPath"] ?? @".\Scripts";
            DirectoryInfo d = new DirectoryInfo(filepath);
            var orderedPowerShellFiles = d.GetFiles("*.ps1").OrderBy(f => f.Name);

            foreach (var file in orderedPowerShellFiles)
            {
                StreamReader reader = new(file.FullName.ToString());
                string scriptText = reader.ReadToEnd();
                string scriptOutput = CallPS(scriptText);
                string headerText = Path.GetFileNameWithoutExtension(file.Name);

                string fileNameNoNumber = Regex.Replace(headerText,"^\\d+\\.", "");

                psScriptDict.Add(fileNameNoNumber, scriptOutput);
            }

            var orderedTextFiles = d.GetFiles("*.txt").OrderBy(f => f.Name);

            foreach (var file in orderedTextFiles)
            {
                StreamReader reader = new(file.FullName.ToString());
                string infoText = reader.ReadToEnd();

                string informationFileName = Path.GetFileNameWithoutExtension(file.Name);
                
                informationDict.Add(informationFileName, infoText);
            }

            InitializeComponent(psScriptDict, informationDict);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new();
            Control[] lblControls = this.Controls.Find("lblContents", true);
            Control[] txtControls = this.Controls.Find("txtContents", true);
            for (int i = 0; i < lblControls.Length; i++)
            {
                string line;
                if (string.IsNullOrEmpty(txtControls[i].Text))
                {
                    line = $"{lblControls[i].Text}; " + Environment.NewLine;
                }
                else
                {
                    line = $"{lblControls[i].Text}; {txtControls[i].Text}";
                }
                stringBuilder.Append(line);
            }

            string copyOutput = stringBuilder.ToString();
            Clipboard.SetText(copyOutput);

            toolStripStatusLabel1.Text = "Clipboard set with information";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new();
            Control[] lblControls = this.Controls.Find("lblContents", true);
            Control[] txtControls = this.Controls.Find("txtContents", true);
            for (int i = 0; i < lblControls.Length; i++)
            {
                string line = $"{lblControls[i].Text}; {txtControls[i].Text}";
                stringBuilder.Append(line);
            }

            string saveOutput = stringBuilder.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file|*.txt";
            saveFileDialog.Title = "Save info to file";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                byte[] data = Encoding.UTF8.GetBytes(saveOutput);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }

            toolStripStatusLabel1.Text = $"File saved: {saveFileDialog.FileName}";
        }
    }
}
