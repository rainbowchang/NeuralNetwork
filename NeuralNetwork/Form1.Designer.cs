namespace NeuralNetwork
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_bptraining = new System.Windows.Forms.Button();
            this.btn_legendretraining = new System.Windows.Forms.Button();
            this.pic_box1 = new System.Windows.Forms.PictureBox();
            this.lbStocks = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_update = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_box1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_bptraining
            // 
            this.btn_bptraining.Location = new System.Drawing.Point(774, 466);
            this.btn_bptraining.Name = "btn_bptraining";
            this.btn_bptraining.Size = new System.Drawing.Size(75, 23);
            this.btn_bptraining.TabIndex = 0;
            this.btn_bptraining.Text = "BPTrain";
            this.btn_bptraining.UseVisualStyleBackColor = true;
            this.btn_bptraining.Click += new System.EventHandler(this.btn_training_Click);
            // 
            // btn_legendretraining
            // 
            this.btn_legendretraining.Location = new System.Drawing.Point(515, 467);
            this.btn_legendretraining.Name = "btn_legendretraining";
            this.btn_legendretraining.Size = new System.Drawing.Size(172, 23);
            this.btn_legendretraining.TabIndex = 4;
            this.btn_legendretraining.Text = "LegedreTraining";
            this.btn_legendretraining.UseVisualStyleBackColor = true;
            this.btn_legendretraining.Click += new System.EventHandler(this.btn_legendretraining_Click);
            // 
            // pic_box1
            // 
            this.pic_box1.Location = new System.Drawing.Point(220, 12);
            this.pic_box1.Name = "pic_box1";
            this.pic_box1.Size = new System.Drawing.Size(629, 448);
            this.pic_box1.TabIndex = 5;
            this.pic_box1.TabStop = false;
            // 
            // lbStocks
            // 
            this.lbStocks.FormattingEnabled = true;
            this.lbStocks.ItemHeight = 12;
            this.lbStocks.Location = new System.Drawing.Point(13, 13);
            this.lbStocks.Name = "lbStocks";
            this.lbStocks.Size = new System.Drawing.Size(201, 448);
            this.lbStocks.TabIndex = 6;
            this.lbStocks.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbStocks_MouseUp);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 467);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(497, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(693, 467);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(75, 23);
            this.btn_update.TabIndex = 8;
            this.btn_update.Text = "Update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 496);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(837, 115);
            this.tbLog.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 623);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbStocks);
            this.Controls.Add(this.pic_box1);
            this.Controls.Add(this.btn_legendretraining);
            this.Controls.Add(this.btn_bptraining);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_box1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_bptraining;
        private System.Windows.Forms.Button btn_legendretraining;
        private System.Windows.Forms.PictureBox pic_box1;
        private System.Windows.Forms.ListBox lbStocks;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.TextBox tbLog;
    }
}

