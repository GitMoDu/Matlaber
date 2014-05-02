namespace Matlaber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.InputPLC = new System.Windows.Forms.GroupBox();
            this.ButtonInputFolder = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ListResultBox = new System.Windows.Forms.ListBox();
            this.FileListCheckedBox = new System.Windows.Forms.CheckedListBox();
            this.ButtonOutputFolder = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.InputPLC.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(190, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputPLC
            // 
            this.InputPLC.Controls.Add(this.ButtonInputFolder);
            this.InputPLC.Controls.Add(this.textBoxInput);
            this.InputPLC.Location = new System.Drawing.Point(12, 12);
            this.InputPLC.Name = "InputPLC";
            this.InputPLC.Size = new System.Drawing.Size(336, 49);
            this.InputPLC.TabIndex = 3;
            this.InputPLC.TabStop = false;
            this.InputPLC.Text = "Input PLC files directory";
            // 
            // ButtonInputFolder
            // 
            this.ButtonInputFolder.Location = new System.Drawing.Point(286, 19);
            this.ButtonInputFolder.Name = "ButtonInputFolder";
            this.ButtonInputFolder.Size = new System.Drawing.Size(44, 23);
            this.ButtonInputFolder.TabIndex = 1;
            this.ButtonInputFolder.Text = "Folder";
            this.ButtonInputFolder.UseVisualStyleBackColor = true;
            this.ButtonInputFolder.Click += new System.EventHandler(this.ButtonInputFolder_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(6, 19);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(274, 20);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 19);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(545, 10);
            this.progressBar1.TabIndex = 8;
            // 
            // ListResultBox
            // 
            this.ListResultBox.FormattingEnabled = true;
            this.ListResultBox.Location = new System.Drawing.Point(5, 35);
            this.ListResultBox.Name = "ListResultBox";
            this.ListResultBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ListResultBox.ScrollAlwaysVisible = true;
            this.ListResultBox.Size = new System.Drawing.Size(545, 69);
            this.ListResultBox.TabIndex = 7;
            // 
            // FileListCheckedBox
            // 
            this.FileListCheckedBox.CheckOnClick = true;
            this.FileListCheckedBox.FormattingEnabled = true;
            this.FileListCheckedBox.Location = new System.Drawing.Point(6, 19);
            this.FileListCheckedBox.Name = "FileListCheckedBox";
            this.FileListCheckedBox.ScrollAlwaysVisible = true;
            this.FileListCheckedBox.Size = new System.Drawing.Size(204, 124);
            this.FileListCheckedBox.TabIndex = 5;
            this.FileListCheckedBox.SelectedIndexChanged += new System.EventHandler(this.FileListCheckedBox_SelectedIndexChanged);
            // 
            // ButtonOutputFolder
            // 
            this.ButtonOutputFolder.Location = new System.Drawing.Point(285, 17);
            this.ButtonOutputFolder.Name = "ButtonOutputFolder";
            this.ButtonOutputFolder.Size = new System.Drawing.Size(44, 23);
            this.ButtonOutputFolder.TabIndex = 1;
            this.ButtonOutputFolder.Text = "Folder";
            this.ButtonOutputFolder.UseVisualStyleBackColor = true;
            this.ButtonOutputFolder.Click += new System.EventHandler(this.ButtonOutputFolder_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(6, 20);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(273, 20);
            this.textBoxOutput.TabIndex = 0;
            this.textBoxOutput.TextChanged += new System.EventHandler(this.textBoxOutput_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "© 2011";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FileListCheckedBox);
            this.groupBox1.Location = new System.Drawing.Point(354, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 152);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File selection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxOutput);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.ButtonOutputFolder);
            this.groupBox2.Location = new System.Drawing.Point(12, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 97);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output folder";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.ListResultBox);
            this.groupBox3.Location = new System.Drawing.Point(13, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(557, 125);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "André Pereira";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "andrep1333@hotmail.com";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(29, 307);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 45);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(518, 320);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 19);
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 364);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.InputPLC);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            //this.Icon = new System.Drawing.Icon("Resources/Matlaber.ico");

            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(601, 363);
            this.Name = "Form1";
            this.Text = "Matlaber";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.InputPLC.ResumeLayout(false);
            this.InputPLC.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox InputPLC;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button ButtonInputFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckedListBox FileListCheckedBox;
        private System.Windows.Forms.Button ButtonOutputFolder;
        private System.Windows.Forms.ListBox ListResultBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

