namespace FzBusSystem
{
    partial class FzbussysForm
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
            this.textBox_searchresult = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_line = new System.Windows.Forms.ComboBox();
            this.btn_linesearch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_stop = new System.Windows.Forms.ComboBox();
            this.btn_stopsearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_stoptostop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_end = new System.Windows.Forms.ComboBox();
            this.comboBox_start = new System.Windows.Forms.ComboBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbBusLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.labelCount = new System.Windows.Forms.Label();
            this.lbCurrentLine = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_searchresult
            // 
            this.textBox_searchresult.Location = new System.Drawing.Point(12, 447);
            this.textBox_searchresult.Multiline = true;
            this.textBox_searchresult.Name = "textBox_searchresult";
            this.textBox_searchresult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_searchresult.Size = new System.Drawing.Size(598, 111);
            this.textBox_searchresult.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_line);
            this.groupBox1.Controls.Add(this.btn_linesearch);
            this.groupBox1.Location = new System.Drawing.Point(634, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 112);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "公交车次查询";
            // 
            // comboBox_line
            // 
            this.comboBox_line.FormattingEnabled = true;
            this.comboBox_line.Location = new System.Drawing.Point(39, 30);
            this.comboBox_line.Name = "comboBox_line";
            this.comboBox_line.Size = new System.Drawing.Size(138, 20);
            this.comboBox_line.TabIndex = 2;
            // 
            // btn_linesearch
            // 
            this.btn_linesearch.Location = new System.Drawing.Point(63, 72);
            this.btn_linesearch.Name = "btn_linesearch";
            this.btn_linesearch.Size = new System.Drawing.Size(83, 24);
            this.btn_linesearch.TabIndex = 1;
            this.btn_linesearch.Text = "查询";
            this.btn_linesearch.UseVisualStyleBackColor = true;
            this.btn_linesearch.Click += new System.EventHandler(this.btn_linesearch_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_stop);
            this.groupBox2.Controls.Add(this.btn_stopsearch);
            this.groupBox2.Location = new System.Drawing.Point(636, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 116);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "公交站点查询";
            // 
            // comboBox_stop
            // 
            this.comboBox_stop.FormattingEnabled = true;
            this.comboBox_stop.Location = new System.Drawing.Point(37, 37);
            this.comboBox_stop.Name = "comboBox_stop";
            this.comboBox_stop.Size = new System.Drawing.Size(138, 20);
            this.comboBox_stop.TabIndex = 3;
            // 
            // btn_stopsearch
            // 
            this.btn_stopsearch.Location = new System.Drawing.Point(61, 75);
            this.btn_stopsearch.Name = "btn_stopsearch";
            this.btn_stopsearch.Size = new System.Drawing.Size(83, 23);
            this.btn_stopsearch.TabIndex = 0;
            this.btn_stopsearch.Text = "查询";
            this.btn_stopsearch.UseVisualStyleBackColor = true;
            this.btn_stopsearch.Click += new System.EventHandler(this.btn_stopsearch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 432);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "查询结果：";
            // 
            // btn_stoptostop
            // 
            this.btn_stoptostop.Location = new System.Drawing.Point(60, 106);
            this.btn_stoptostop.Name = "btn_stoptostop";
            this.btn_stoptostop.Size = new System.Drawing.Size(83, 23);
            this.btn_stoptostop.TabIndex = 0;
            this.btn_stoptostop.Text = "查询";
            this.btn_stoptostop.UseVisualStyleBackColor = true;
            this.btn_stoptostop.Click += new System.EventHandler(this.btn_stoptostop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "起点：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "终点：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_end);
            this.groupBox3.Controls.Add(this.comboBox_start);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btn_stoptostop);
            this.groupBox3.Location = new System.Drawing.Point(637, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(209, 150);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "公交站站查询";
            // 
            // comboBox_end
            // 
            this.comboBox_end.FormattingEnabled = true;
            this.comboBox_end.Location = new System.Drawing.Point(53, 65);
            this.comboBox_end.Name = "comboBox_end";
            this.comboBox_end.Size = new System.Drawing.Size(138, 20);
            this.comboBox_end.TabIndex = 5;
            // 
            // comboBox_start
            // 
            this.comboBox_start.FormattingEnabled = true;
            this.comboBox_start.Location = new System.Drawing.Point(53, 27);
            this.comboBox_start.Name = "comboBox_start";
            this.comboBox_start.Size = new System.Drawing.Size(138, 20);
            this.comboBox_start.TabIndex = 4;
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(598, 417);
            this.webBrowser.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbBusLine);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btnGo);
            this.groupBox4.Controls.Add(this.labelCount);
            this.groupBox4.Controls.Add(this.lbCurrentLine);
            this.groupBox4.Location = new System.Drawing.Point(636, 409);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(210, 159);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "公交数据更新";
            // 
            // tbBusLine
            // 
            this.tbBusLine.Location = new System.Drawing.Point(61, 23);
            this.tbBusLine.Name = "tbBusLine";
            this.tbBusLine.Size = new System.Drawing.Size(87, 21);
            this.tbBusLine.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "起始线路:";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(61, 64);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(83, 23);
            this.btnGo.TabIndex = 13;
            this.btnGo.Text = "更新";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(7, 128);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(83, 12);
            this.labelCount.TabIndex = 12;
            this.labelCount.Text = "抓取的站点有:";
            // 
            // lbCurrentLine
            // 
            this.lbCurrentLine.AutoSize = true;
            this.lbCurrentLine.Location = new System.Drawing.Point(7, 101);
            this.lbCurrentLine.Name = "lbCurrentLine";
            this.lbCurrentLine.Size = new System.Drawing.Size(119, 12);
            this.lbCurrentLine.TabIndex = 11;
            this.lbCurrentLine.Text = "当前正在解析的线路:";
            // 
            // FzbussysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(853, 570);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_searchresult);
            this.MaximizeBox = false;
            this.Name = "FzbussysForm";
            this.Text = "福州公交查询系统";
            this.Load += new System.EventHandler(this.FzbussysForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_searchresult;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_linesearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_stopsearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_stoptostop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox_line;
        private System.Windows.Forms.ComboBox comboBox_stop;
        private System.Windows.Forms.ComboBox comboBox_end;
        private System.Windows.Forms.ComboBox comboBox_start;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label lbCurrentLine;
        private System.Windows.Forms.TextBox tbBusLine;
        private System.Windows.Forms.Label label4;
    }
}

