namespace EdwardScissorhands
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
         this.m_generateButton = new System.Windows.Forms.Button();
         this.m_text = new System.Windows.Forms.TextBox();
         this.m_saveButton = new System.Windows.Forms.Button();
         this.m_loadButton = new System.Windows.Forms.Button();
         this.m_filenameTextbox = new System.Windows.Forms.TextBox();
         this.m_fileLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // m_generateButton
         // 
         this.m_generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.m_generateButton.Location = new System.Drawing.Point(457, 358);
         this.m_generateButton.Name = "m_generateButton";
         this.m_generateButton.Size = new System.Drawing.Size(75, 23);
         this.m_generateButton.TabIndex = 2;
         this.m_generateButton.Text = "Generate";
         this.m_generateButton.UseVisualStyleBackColor = true;
         this.m_generateButton.Click += new System.EventHandler(this.m_generateButton_Click);
         // 
         // m_text
         // 
         this.m_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.m_text.Location = new System.Drawing.Point(12, 32);
         this.m_text.Multiline = true;
         this.m_text.Name = "m_text";
         this.m_text.Size = new System.Drawing.Size(520, 320);
         this.m_text.TabIndex = 3;
         // 
         // m_saveButton
         // 
         this.m_saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.m_saveButton.Location = new System.Drawing.Point(376, 358);
         this.m_saveButton.Name = "m_saveButton";
         this.m_saveButton.Size = new System.Drawing.Size(75, 23);
         this.m_saveButton.TabIndex = 4;
         this.m_saveButton.Text = "Save";
         this.m_saveButton.UseVisualStyleBackColor = true;
         this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
         // 
         // m_loadButton
         // 
         this.m_loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.m_loadButton.Location = new System.Drawing.Point(295, 358);
         this.m_loadButton.Name = "m_loadButton";
         this.m_loadButton.Size = new System.Drawing.Size(75, 23);
         this.m_loadButton.TabIndex = 5;
         this.m_loadButton.Text = "Load";
         this.m_loadButton.UseVisualStyleBackColor = true;
         this.m_loadButton.Click += new System.EventHandler(this.m_loadButton_Click);
         // 
         // m_filenameTextbox
         // 
         this.m_filenameTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.m_filenameTextbox.Location = new System.Drawing.Point(136, 6);
         this.m_filenameTextbox.Name = "m_filenameTextbox";
         this.m_filenameTextbox.Size = new System.Drawing.Size(396, 20);
         this.m_filenameTextbox.TabIndex = 6;
         this.m_filenameTextbox.TextChanged += new System.EventHandler(this.m_filenameTextbox_TextChanged);
         // 
         // m_fileLabel
         // 
         this.m_fileLabel.AutoSize = true;
         this.m_fileLabel.Location = new System.Drawing.Point(12, 9);
         this.m_fileLabel.Name = "m_fileLabel";
         this.m_fileLabel.Size = new System.Drawing.Size(118, 13);
         this.m_fileLabel.TabIndex = 7;
         this.m_fileLabel.Text = "Instructions for Edward:";
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(544, 393);
         this.Controls.Add(this.m_fileLabel);
         this.Controls.Add(this.m_filenameTextbox);
         this.Controls.Add(this.m_loadButton);
         this.Controls.Add(this.m_saveButton);
         this.Controls.Add(this.m_text);
         this.Controls.Add(this.m_generateButton);
         this.Name = "Form1";
         this.Text = "Edward Scissorhands";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button m_generateButton;
      private System.Windows.Forms.TextBox m_text;
      private System.Windows.Forms.Button m_saveButton;
      private System.Windows.Forms.Button m_loadButton;
      private System.Windows.Forms.TextBox m_filenameTextbox;
      private System.Windows.Forms.Label m_fileLabel;
   }
}

