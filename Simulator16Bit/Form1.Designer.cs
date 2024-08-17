namespace Simülator
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
            this.txt_input = new System.Windows.Forms.TextBox();
            this.txt_output = new System.Windows.Forms.TextBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.dtgrid_register = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_counter = new System.Windows.Forms.Label();
            this.dtgrid_memory = new System.Windows.Forms.DataGridView();
            this.btn_line = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btnAssembly = new System.Windows.Forms.Button();
            this.dtgrid_instMem = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hexValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_register)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_memory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_instMem)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(12, 12);
            this.txt_input.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_input.Multiline = true;
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(415, 370);
            this.txt_input.TabIndex = 0;
            // 
            // txt_output
            // 
            this.txt_output.Location = new System.Drawing.Point(12, 423);
            this.txt_output.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_output.Name = "txt_output";
            this.txt_output.Size = new System.Drawing.Size(415, 22);
            this.txt_output.TabIndex = 1;
            // 
            // btn_start
            // 
            this.btn_start.Enabled = false;
            this.btn_start.Location = new System.Drawing.Point(436, 52);
            this.btn_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(109, 34);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // dtgrid_register
            // 
            this.dtgrid_register.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrid_register.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dtgrid_register.Location = new System.Drawing.Point(558, 12);
            this.dtgrid_register.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtgrid_register.Name = "dtgrid_register";
            this.dtgrid_register.RowHeadersWidth = 51;
            this.dtgrid_register.RowTemplate.Height = 24;
            this.dtgrid_register.Size = new System.Drawing.Size(597, 502);
            this.dtgrid_register.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Registers";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Number";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Value";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 125;
            // 
            // lbl_counter
            // 
            this.lbl_counter.AutoSize = true;
            this.lbl_counter.Location = new System.Drawing.Point(433, 214);
            this.lbl_counter.Name = "lbl_counter";
            this.lbl_counter.Size = new System.Drawing.Size(54, 16);
            this.lbl_counter.TabIndex = 5;
            this.lbl_counter.Text = "counter:";
            // 
            // dtgrid_memory
            // 
            this.dtgrid_memory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrid_memory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.hexValue});
            this.dtgrid_memory.Location = new System.Drawing.Point(1170, 11);
            this.dtgrid_memory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtgrid_memory.Name = "dtgrid_memory";
            this.dtgrid_memory.RowHeadersWidth = 51;
            this.dtgrid_memory.RowTemplate.Height = 24;
            this.dtgrid_memory.Size = new System.Drawing.Size(366, 739);
            this.dtgrid_memory.TabIndex = 6;
            // 
            // btn_line
            // 
            this.btn_line.Enabled = false;
            this.btn_line.Location = new System.Drawing.Point(436, 130);
            this.btn_line.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_line.Name = "btn_line";
            this.btn_line.Size = new System.Drawing.Size(109, 34);
            this.btn_line.TabIndex = 7;
            this.btn_line.Text = "Run Step";
            this.btn_line.UseVisualStyleBackColor = true;
            this.btn_line.Click += new System.EventHandler(this.btn_line_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(437, 91);
            this.btn_clear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(109, 34);
            this.btn_clear.TabIndex = 8;
            this.btn_clear.Text = "Clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btnAssembly
            // 
            this.btnAssembly.Location = new System.Drawing.Point(437, 12);
            this.btnAssembly.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAssembly.Name = "btnAssembly";
            this.btnAssembly.Size = new System.Drawing.Size(109, 34);
            this.btnAssembly.TabIndex = 9;
            this.btnAssembly.Text = "Assembly";
            this.btnAssembly.UseVisualStyleBackColor = true;
            this.btnAssembly.Click += new System.EventHandler(this.btnAssembly_Click);
            // 
            // dtgrid_instMem
            // 
            this.dtgrid_instMem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrid_instMem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Code,
            this.Source});
            this.dtgrid_instMem.Location = new System.Drawing.Point(12, 562);
            this.dtgrid_instMem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtgrid_instMem.Name = "dtgrid_instMem";
            this.dtgrid_instMem.RowHeadersWidth = 51;
            this.dtgrid_instMem.RowTemplate.Height = 24;
            this.dtgrid_instMem.Size = new System.Drawing.Size(1143, 188);
            this.dtgrid_instMem.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Address";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 262;
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.MinimumWidth = 6;
            this.Code.Name = "Code";
            this.Code.Width = 262;
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.MinimumWidth = 6;
            this.Source.Name = "Source";
            this.Source.Width = 262;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Address";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 125;
            // 
            // hexValue
            // 
            this.hexValue.HeaderText = "Hex Value";
            this.hexValue.MinimumWidth = 6;
            this.hexValue.Name = "hexValue";
            this.hexValue.Width = 125;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1606, 767);
            this.Controls.Add(this.dtgrid_instMem);
            this.Controls.Add(this.btnAssembly);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_line);
            this.Controls.Add(this.dtgrid_memory);
            this.Controls.Add(this.lbl_counter);
            this.Controls.Add(this.dtgrid_register);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.txt_output);
            this.Controls.Add(this.txt_input);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_register)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_memory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrid_instMem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.TextBox txt_output;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.DataGridView dtgrid_register;
        private System.Windows.Forms.Label lbl_counter;
        private System.Windows.Forms.DataGridView dtgrid_memory;
        private System.Windows.Forms.Button btn_line;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btnAssembly;
        private System.Windows.Forms.DataGridView dtgrid_instMem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn hexValue;
    }
}

