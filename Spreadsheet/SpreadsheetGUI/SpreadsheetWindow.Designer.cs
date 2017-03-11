namespace SpreadsheetGUI
{
    partial class SpreadsheetWindow
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
            this.spreadsheetPanel1 = new SSGui.SpreadsheetPanel();
            this.CellLabel = new System.Windows.Forms.Label();
            this.ContentsLabel = new System.Windows.Forms.Label();
            this.ContentBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellNameReadOnly = new System.Windows.Forms.TextBox();
            this.cellValReadOnly = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(-3, 75);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1242, 490);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            this.spreadsheetPanel1.DoubleClick += new System.EventHandler(this.spreadsheetPanel1_DoubleClick);
            this.spreadsheetPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.spreadsheetPanel1_MouseClick);
            // 
            // CellLabel
            // 
            this.CellLabel.AutoSize = true;
            this.CellLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CellLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellLabel.Location = new System.Drawing.Point(16, 42);
            this.CellLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CellLabel.Name = "CellLabel";
            this.CellLabel.Size = new System.Drawing.Size(58, 30);
            this.CellLabel.TabIndex = 1;
            this.CellLabel.Text = "Cell:";
            // 
            // ContentsLabel
            // 
            this.ContentsLabel.AutoSize = true;
            this.ContentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ContentsLabel.Location = new System.Drawing.Point(478, 42);
            this.ContentsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ContentsLabel.Name = "ContentsLabel";
            this.ContentsLabel.Size = new System.Drawing.Size(114, 29);
            this.ContentsLabel.TabIndex = 3;
            this.ContentsLabel.Text = "Contents:";
            // 
            // ContentBox
            // 
            this.ContentBox.Location = new System.Drawing.Point(597, 42);
            this.ContentBox.Margin = new System.Windows.Forms.Padding(4);
            this.ContentBox.Multiline = true;
            this.ContentBox.Name = "ContentBox";
            this.ContentBox.Size = new System.Drawing.Size(590, 31);
            this.ContentBox.TabIndex = 4;
            this.ContentBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContentBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(175, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Value:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1255, 38);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(156, 34);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(68, 34);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // cellNameReadOnly
            // 
            this.cellNameReadOnly.Location = new System.Drawing.Point(69, 39);
            this.cellNameReadOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cellNameReadOnly.Name = "cellNameReadOnly";
            this.cellNameReadOnly.ReadOnly = true;
            this.cellNameReadOnly.Size = new System.Drawing.Size(77, 29);
            this.cellNameReadOnly.TabIndex = 8;
            // 
            // cellValReadOnly
            // 
            this.cellValReadOnly.Location = new System.Drawing.Point(248, 39);
            this.cellValReadOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cellValReadOnly.Name = "cellValReadOnly";
            this.cellValReadOnly.ReadOnly = true;
            this.cellValReadOnly.Size = new System.Drawing.Size(221, 29);
            this.cellValReadOnly.TabIndex = 9;
            // 
            // SpreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 570);
            this.Controls.Add(this.cellValReadOnly);
            this.Controls.Add(this.cellNameReadOnly);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContentBox);
            this.Controls.Add(this.ContentsLabel);
            this.Controls.Add(this.CellLabel);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SpreadsheetWindow";
            this.Text = "Ghetto Spreadsheet";
            this.Load += new System.EventHandler(this.SpreadsheetWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SSGui.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Label CellLabel;
        private System.Windows.Forms.Label ContentsLabel;
        private System.Windows.Forms.TextBox ContentBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox cellNameReadOnly;
        private System.Windows.Forms.TextBox cellValReadOnly;
    }
}

