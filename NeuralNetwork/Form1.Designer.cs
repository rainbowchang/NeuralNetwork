﻿namespace NeuralNetwork
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
            this.btn_predict = new System.Windows.Forms.Button();
            this.btn_load = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_legendretraining = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_bptraining
            // 
            this.btn_bptraining.Location = new System.Drawing.Point(567, 284);
            this.btn_bptraining.Name = "btn_bptraining";
            this.btn_bptraining.Size = new System.Drawing.Size(75, 23);
            this.btn_bptraining.TabIndex = 0;
            this.btn_bptraining.Text = "BPTrain";
            this.btn_bptraining.UseVisualStyleBackColor = true;
            this.btn_bptraining.Click += new System.EventHandler(this.btn_training_Click);
            // 
            // btn_predict
            // 
            this.btn_predict.Location = new System.Drawing.Point(470, 284);
            this.btn_predict.Name = "btn_predict";
            this.btn_predict.Size = new System.Drawing.Size(75, 23);
            this.btn_predict.TabIndex = 1;
            this.btn_predict.Text = "Predict";
            this.btn_predict.UseVisualStyleBackColor = true;
            this.btn_predict.Click += new System.EventHandler(this.btn_predict_Click);
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(371, 284);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(75, 23);
            this.btn_load.TabIndex = 2;
            this.btn_load.Text = "Load";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(267, 284);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 3;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_legendretraining
            // 
            this.btn_legendretraining.Location = new System.Drawing.Point(470, 240);
            this.btn_legendretraining.Name = "btn_legendretraining";
            this.btn_legendretraining.Size = new System.Drawing.Size(172, 23);
            this.btn_legendretraining.TabIndex = 4;
            this.btn_legendretraining.Text = "LegedreTraining";
            this.btn_legendretraining.UseVisualStyleBackColor = true;
            this.btn_legendretraining.Click += new System.EventHandler(this.btn_legendretraining_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 319);
            this.Controls.Add(this.btn_legendretraining);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.btn_predict);
            this.Controls.Add(this.btn_bptraining);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_bptraining;
        private System.Windows.Forms.Button btn_predict;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_legendretraining;
    }
}

