namespace StreamUpload.Winforms;

partial class Form1
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

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        imgCam = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)imgCam).BeginInit();
        SuspendLayout();
        // 
        // imgCam
        // 
        imgCam.Dock = DockStyle.Fill;
        imgCam.Location = new Point(0, 0);
        imgCam.Name = "imgCam";
        imgCam.Size = new Size(1080, 732);
        imgCam.SizeMode = PictureBoxSizeMode.Zoom;
        imgCam.TabIndex = 0;
        imgCam.TabStop = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1080, 732);
        Controls.Add(imgCam);
        Name = "Form1";
        Text = "Form1";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)imgCam).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox imgCam;
}
