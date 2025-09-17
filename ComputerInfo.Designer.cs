using ComputerInfo.Properties;
using System.Collections;
using System.Configuration;
using System.Resources;
using System.Windows.Forms;

namespace ComputerInfo
{
    partial class ComputerInfoBox
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent(Dictionary<string, string> DisplayList, Dictionary<string, string> InformationList)
        {
            splitContainer1 = new SplitContainer();
            btnExit = new Button();
            btnCopy = new Button();
            btnSave = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            statusStrip1.SuspendLayout();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.Icon = new Icon("computer.ico");
            SuspendLayout();
            
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";

            splitContainer1.Panel2.Controls.Add(btnExit);
            splitContainer1.Panel1.Controls.Add(btnCopy);
            splitContainer1.Panel1.Controls.Add(btnSave);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.TabIndex = 0;

            int LabelLocation = 9;
            int TextLocation = 3;
            int TabIndex = 0;

            foreach (string key in DisplayList.Keys)
            {
                Label = new Label();
                Text = new TextBox();

                splitContainer1.Panel1.Controls.Add(Label);
                splitContainer1.Panel2.Controls.Add(Text);
                // 
                // label
                // 
                Label.Anchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right));
                Label.AutoSize = true;
                Label.Location = new Point(12, LabelLocation);
                Label.Name = "lblContents";
                Label.Size = new Size(136, 25);
                Label.TabIndex = TabIndex;
                Label.Text = key;
                // 
                // text
                // 
                Text.Anchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right));
                Text.Enabled = true;
                Text.Location = new Point(3, TextLocation);
                Text.Name = "txtContents";
                Text.ReadOnly = true;
                Text.Size = new Size(459, 31);
                Text.TabIndex = TabIndex;
                Text.Text = DisplayList[key].ToString();

                LabelLocation = LabelLocation + 32;
                TextLocation = TextLocation + 32;
                TabIndex++;
            }

            foreach (string key in InformationList.Keys.Where(k => k.Split('.')[0] == "1"))
            {
                infoBoxPanel1 = new TextBox();
                splitContainer1.Panel1.Controls.Add(infoBoxPanel1);
                infoBoxPanel1.Anchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right));
                infoBoxPanel1.Enabled = true;
                infoBoxPanel1.Location = new Point(12, LabelLocation);
                infoBoxPanel1.Name = "infoBoxPanel1";
                infoBoxPanel1.ReadOnly = true;
                int lines = (InformationList[key].Split('\n').Length);
                infoBoxPanel1.Size = new Size(250, (lines * 32));
                infoBoxPanel1.TabIndex = ++TabIndex;
                infoBoxPanel1.Multiline = true;
                infoBoxPanel1.Text = InformationList[key];

                LabelLocation = LabelLocation + (lines * 32);
            }

            foreach (string key in InformationList.Keys.Where(k => k.Split('.')[0] == "2"))
            {
                infoBoxPanel2 = new TextBox();
                splitContainer1.Panel2.Controls.Add(infoBoxPanel2);
                infoBoxPanel2.Anchor = ((AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right));
                infoBoxPanel2.Enabled = true;
                infoBoxPanel2.Location = new Point(3, TextLocation);
                infoBoxPanel2.Name = "infoBoxPanel2";
                infoBoxPanel2.ReadOnly = true;
                int lines = (InformationList[key].Split('\n').Length);
                infoBoxPanel2.Size = new Size(459, (lines * 32));
                infoBoxPanel2.TabIndex = ++TabIndex;
                infoBoxPanel2.Multiline = true;
                infoBoxPanel2.Text = InformationList[key];

                TextLocation = TextLocation + (lines * 32);
            }

            //
            // StatusStrip
            //

            statusStrip1.AutoSize = false;
            statusStrip1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusStrip1.Location = new Point(0, 450);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = ToolStripRenderMode.Professional;
            statusStrip1.Size = new Size(800, 30);
            statusStrip1.TabIndex = 0;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });

            toolStripStatusLabel1.TextAlign = ContentAlignment.TopLeft;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(790, 29);

            // 
            // btnSave
            // 
            btnSave.Anchor = ((AnchorStyles)(AnchorStyles.Bottom | AnchorStyles.Left));
            btnSave.Location = new Point(9, 404);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 40);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // 
            // btnCopy
            // 
            btnCopy.Anchor = ((AnchorStyles)(AnchorStyles.Bottom | AnchorStyles.Right));
            btnCopy.Location = new Point(188, 404);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(112, 40);
            btnCopy.TabIndex = 1;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            
            // 
            // btnExit
            // 
            btnExit.Anchor = ((AnchorStyles)(AnchorStyles.Bottom | AnchorStyles.Right));
            btnExit.Location = new Point(350, 404);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(112, 40);
            btnExit.TabIndex = 1;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;

            // 
            // ComputerInfoBox
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 480);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Name = "ComputerInfoBox";
            base.Text = "Computer information";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }


        private SplitContainer splitContainer1;
        private Label Label;
        private TextBox Text;
        private Button btnExit;
        private Button btnCopy;
        private Button btnSave;
        private TextBox infoBoxPanel1;
        private TextBox infoBoxPanel2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
