namespace MobiDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonDownload = new System.Windows.Forms.Button();
            this.textboxMain = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDownloadLoc = new System.Windows.Forms.TextBox();
            this.labelDownloadLoc = new System.Windows.Forms.Label();
            this.buttonEmail = new System.Windows.Forms.Button();
            this.labelFromEmail = new System.Windows.Forms.Label();
            this.textBoxFromEmail = new System.Windows.Forms.TextBox();
            this.textBoxToEmail = new System.Windows.Forms.TextBox();
            this.labelToEmail = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.labelFileSave = new System.Windows.Forms.Label();
            this.textBoxFileSave = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(446, 237);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 0;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // textboxMain
            // 
            this.textboxMain.AccessibleDescription = "Main Textbox";
            this.textboxMain.AccessibleName = "textboxMain";
            this.textboxMain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textboxMain.Location = new System.Drawing.Point(195, 239);
            this.textboxMain.Name = "textboxMain";
            this.textboxMain.Size = new System.Drawing.Size(245, 20);
            this.textboxMain.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(195, 266);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(245, 23);
            this.progressBar.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(620, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter the name of a book followed by the author name (Outliers - Malcolm Gladwell" +
    ") and the program will seach for and download it";
            // 
            // textBoxDownloadLoc
            // 
            this.textBoxDownloadLoc.Location = new System.Drawing.Point(77, 92);
            this.textBoxDownloadLoc.Name = "textBoxDownloadLoc";
            this.textBoxDownloadLoc.Size = new System.Drawing.Size(203, 20);
            this.textBoxDownloadLoc.TabIndex = 4;
            // 
            // labelDownloadLoc
            // 
            this.labelDownloadLoc.AutoSize = true;
            this.labelDownloadLoc.Location = new System.Drawing.Point(77, 73);
            this.labelDownloadLoc.Name = "labelDownloadLoc";
            this.labelDownloadLoc.Size = new System.Drawing.Size(111, 13);
            this.labelDownloadLoc.TabIndex = 5;
            this.labelDownloadLoc.Text = "Download Folder path";
            // 
            // buttonEmail
            // 
            this.buttonEmail.Location = new System.Drawing.Point(447, 267);
            this.buttonEmail.Name = "buttonEmail";
            this.buttonEmail.Size = new System.Drawing.Size(75, 23);
            this.buttonEmail.TabIndex = 6;
            this.buttonEmail.Text = "Email";
            this.buttonEmail.UseVisualStyleBackColor = true;
            this.buttonEmail.Click += new System.EventHandler(this.buttonEmail_Click);
            // 
            // labelFromEmail
            // 
            this.labelFromEmail.AutoSize = true;
            this.labelFromEmail.Location = new System.Drawing.Point(300, 73);
            this.labelFromEmail.Name = "labelFromEmail";
            this.labelFromEmail.Size = new System.Drawing.Size(58, 13);
            this.labelFromEmail.TabIndex = 7;
            this.labelFromEmail.Text = "From Email";
            // 
            // textBoxFromEmail
            // 
            this.textBoxFromEmail.Location = new System.Drawing.Point(303, 92);
            this.textBoxFromEmail.Name = "textBoxFromEmail";
            this.textBoxFromEmail.Size = new System.Drawing.Size(203, 20);
            this.textBoxFromEmail.TabIndex = 8;
            // 
            // textBoxToEmail
            // 
            this.textBoxToEmail.Location = new System.Drawing.Point(303, 134);
            this.textBoxToEmail.Name = "textBoxToEmail";
            this.textBoxToEmail.Size = new System.Drawing.Size(203, 20);
            this.textBoxToEmail.TabIndex = 9;
            // 
            // labelToEmail
            // 
            this.labelToEmail.AutoSize = true;
            this.labelToEmail.Location = new System.Drawing.Point(303, 118);
            this.labelToEmail.Name = "labelToEmail";
            this.labelToEmail.Size = new System.Drawing.Size(48, 13);
            this.labelToEmail.TabIndex = 10;
            this.labelToEmail.Text = "To Email";
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(195, 296);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(21, 13);
            this.labelProgress.TabIndex = 11;
            this.labelProgress.Text = "0%";
            // 
            // labelFileSave
            // 
            this.labelFileSave.AutoSize = true;
            this.labelFileSave.Location = new System.Drawing.Point(80, 119);
            this.labelFileSave.Name = "labelFileSave";
            this.labelFileSave.Size = new System.Drawing.Size(82, 13);
            this.labelFileSave.TabIndex = 12;
            this.labelFileSave.Text = "File Save Name";
            // 
            // textBoxFileSave
            // 
            this.textBoxFileSave.Location = new System.Drawing.Point(77, 136);
            this.textBoxFileSave.Name = "textBoxFileSave";
            this.textBoxFileSave.Size = new System.Drawing.Size(203, 20);
            this.textBoxFileSave.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 333);
            this.Controls.Add(this.textBoxFileSave);
            this.Controls.Add(this.labelFileSave);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.labelToEmail);
            this.Controls.Add(this.textBoxToEmail);
            this.Controls.Add(this.textBoxFromEmail);
            this.Controls.Add(this.labelFromEmail);
            this.Controls.Add(this.buttonEmail);
            this.Controls.Add(this.labelDownloadLoc);
            this.Controls.Add(this.textBoxDownloadLoc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.textboxMain);
            this.Controls.Add(this.buttonDownload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.TextBox textboxMain;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDownloadLoc;
        private System.Windows.Forms.Label labelDownloadLoc;
        private System.Windows.Forms.Button buttonEmail;
        private System.Windows.Forms.Label labelFromEmail;
        private System.Windows.Forms.TextBox textBoxFromEmail;
        private System.Windows.Forms.TextBox textBoxToEmail;
        private System.Windows.Forms.Label labelToEmail;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label labelFileSave;
        private System.Windows.Forms.TextBox textBoxFileSave;
    }
}

