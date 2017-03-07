namespace TipCalculator
{
    partial class TipCalc
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
            this.CheckName = new System.Windows.Forms.Label();
            this.TipLabel = new System.Windows.Forms.Label();
            this.CheckBox = new System.Windows.Forms.TextBox();
            this.TipBox = new System.Windows.Forms.TextBox();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.TotalLabel = new System.Windows.Forms.Label();
            this.TotalBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CheckName
            // 
            this.CheckName.AutoSize = true;
            this.CheckName.Location = new System.Drawing.Point(12, 46);
            this.CheckName.Name = "CheckName";
            this.CheckName.Size = new System.Drawing.Size(99, 17);
            this.CheckName.TabIndex = 0;
            this.CheckName.Text = "Check Amount";
            this.CheckName.Click += new System.EventHandler(this.label1_Click);
            // 
            // TipLabel
            // 
            this.TipLabel.AutoSize = true;
            this.TipLabel.Location = new System.Drawing.Point(68, 113);
            this.TipLabel.Name = "TipLabel";
            this.TipLabel.Size = new System.Drawing.Size(44, 17);
            this.TipLabel.TabIndex = 1;
            this.TipLabel.Text = "Tip %";
            this.TipLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // CheckBox
            // 
            this.CheckBox.Location = new System.Drawing.Point(120, 43);
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Size = new System.Drawing.Size(100, 22);
            this.CheckBox.TabIndex = 2;
            this.CheckBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // TipBox
            // 
            this.TipBox.Location = new System.Drawing.Point(120, 113);
            this.TipBox.Name = "TipBox";
            this.TipBox.Size = new System.Drawing.Size(100, 22);
            this.TipBox.TabIndex = 3;
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(120, 155);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(100, 23);
            this.CalculateButton.TabIndex = 4;
            this.CalculateButton.Text = "Calculate";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            this.CalculateButton.MouseEnter += new System.EventHandler(this.CalculateButton_MouseEnter);
            this.CalculateButton.MouseLeave += new System.EventHandler(this.CalculateButton_MouseLeave);
            // 
            // TotalLabel
            // 
            this.TotalLabel.AutoSize = true;
            this.TotalLabel.Location = new System.Drawing.Point(68, 209);
            this.TotalLabel.Name = "TotalLabel";
            this.TotalLabel.Size = new System.Drawing.Size(40, 17);
            this.TotalLabel.TabIndex = 5;
            this.TotalLabel.Text = "Total";
            this.TotalLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // TotalBox
            // 
            this.TotalBox.Location = new System.Drawing.Point(120, 209);
            this.TotalBox.Name = "TotalBox";
            this.TotalBox.ReadOnly = true;
            this.TotalBox.Size = new System.Drawing.Size(100, 22);
            this.TotalBox.TabIndex = 6;
            // 
            // TipCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.TotalBox);
            this.Controls.Add(this.TotalLabel);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.TipBox);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.TipLabel);
            this.Controls.Add(this.CheckName);
            this.Name = "TipCalc";
            this.Text = "Tip Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CheckName;
        private System.Windows.Forms.Label TipLabel;
        private System.Windows.Forms.TextBox CheckBox;
        private System.Windows.Forms.TextBox TipBox;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.Label TotalLabel;
        private System.Windows.Forms.TextBox TotalBox;
    }
}

