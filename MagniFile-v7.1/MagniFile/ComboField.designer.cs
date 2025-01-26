namespace MagniFile
{
    partial class ComboField
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
        this.combo = new System.Windows.Forms.ComboBox();
        this.SuspendLayout();
        // 
        // combo
        // 
        this.combo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.combo.Dock = System.Windows.Forms.DockStyle.Fill;
        this.combo.FormattingEnabled = true;
        this.combo.Location = new System.Drawing.Point(0, 0);
        this.combo.Margin = new System.Windows.Forms.Padding(0);
        this.combo.Name = "combo";
        this.combo.Size = new System.Drawing.Size(102, 21);
        this.combo.TabIndex = 0;
        this.combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
        // 
        // ComboField
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.BackColor = System.Drawing.SystemColors.Window;
        this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.ClientSize = new System.Drawing.Size(102, 25);
        this.ControlBox = false;
        this.Controls.Add(this.combo);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ComboField";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "FieldBox";
        this.TopMost = true;
        this.ResumeLayout(false);

	}

	#endregion

    private System.Windows.Forms.ComboBox combo;

    }
}
