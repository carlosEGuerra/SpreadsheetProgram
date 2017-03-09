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
            this.cellNameReadOnly = new System.Windows.Forms.TextBox();
            this.ContentsLabel = new System.Windows.Forms.Label();
            this.ContentBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Location = new System.Drawing.Point(-2, 72);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(903, 305);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.DoubleClick += new System.EventHandler(this.spreadsheetPanel1_DoubleClick);
            this.spreadsheetPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.spreadsheetPanel1_MouseClick);
            // 
            // CellLabel
            // 
            this.CellLabel.AutoSize = true;
            this.CellLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CellLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellLabel.Location = new System.Drawing.Point(3, 47);
            this.CellLabel.Name = "CellLabel";
            this.CellLabel.Size = new System.Drawing.Size(42, 22);
            this.CellLabel.TabIndex = 1;
            this.CellLabel.Text = "Cell:";
            // 
            // cellNameReadOnly
            // 
            this.cellNameReadOnly.Location = new System.Drawing.Point(42, 47);
            this.cellNameReadOnly.Name = "cellNameReadOnly";
            this.cellNameReadOnly.ReadOnly = true;
            this.cellNameReadOnly.Size = new System.Drawing.Size(59, 22);
            this.cellNameReadOnly.TabIndex = 2;
            // 
            // ContentsLabel
            // 
            this.ContentsLabel.AutoSize = true;
            this.ContentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ContentsLabel.Location = new System.Drawing.Point(338, 47);
            this.ContentsLabel.Name = "ContentsLabel";
            this.ContentsLabel.Size = new System.Drawing.Size(81, 20);
            this.ContentsLabel.TabIndex = 3;
            this.ContentsLabel.Text = "Contents:";
            // 
            // ContentBox
            // 
            this.ContentBox.Location = new System.Drawing.Point(425, 47);
            this.ContentBox.Multiline = true;
            this.ContentBox.Name = "ContentBox";
            this.ContentBox.Size = new System.Drawing.Size(430, 22);
            this.ContentBox.TabIndex = 4;
            this.ContentBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContentBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(117, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Value:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(179, 47);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(153, 22);
            this.textBox2.TabIndex = 6;
            // 
            // SpreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 380);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContentBox);
            this.Controls.Add(this.ContentsLabel);
            this.Controls.Add(this.cellNameReadOnly);
            this.Controls.Add(this.CellLabel);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Name = "SpreadsheetWindow";
            this.Text = "Ghetto Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SSGui.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Label CellLabel;
        private System.Windows.Forms.TextBox cellNameReadOnly;
        private System.Windows.Forms.Label ContentsLabel;
        private System.Windows.Forms.TextBox ContentBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

