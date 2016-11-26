namespace RWD.Toolbox.ExceptionHelper.WinForm
{
    partial class ExceptionDisplay
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
            this.btnReport = new System.Windows.Forms.Button();
            this.tre_ErrorMessages = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.White;
            this.btnReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ForeColor = System.Drawing.Color.Blue;
            this.btnReport.Location = new System.Drawing.Point(21, 11);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(155, 33);
            this.btnReport.TabIndex = 3;
            this.btnReport.Text = "Report Error";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // tre_ErrorMessages
            // 
            this.tre_ErrorMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tre_ErrorMessages.Location = new System.Drawing.Point(21, 70);
            this.tre_ErrorMessages.Name = "tre_ErrorMessages";
            this.tre_ErrorMessages.Size = new System.Drawing.Size(672, 133);
            this.tre_ErrorMessages.TabIndex = 2;
            // 
            // ExceptionDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 257);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.tre_ErrorMessages);
            this.Name = "ExceptionDisplay";
            this.Text = "Display";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnReport;
        internal System.Windows.Forms.TreeView tre_ErrorMessages;
    }
}