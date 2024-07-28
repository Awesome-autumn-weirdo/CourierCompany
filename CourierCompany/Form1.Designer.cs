namespace CourierCompany
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.txtNumCouriers = new System.Windows.Forms.TextBox();
            this.txtSimulationTime = new System.Windows.Forms.TextBox();
            this.txtCourierSpeed = new System.Windows.Forms.TextBox();
            this.txtOrderRate = new System.Windows.Forms.TextBox();
            this.lblNumCouriers = new System.Windows.Forms.Label();
            this.lblSimulationTime = new System.Windows.Forms.Label();
            this.lblCourierSpeed = new System.Windows.Forms.Label();
            this.lblOrderRate = new System.Windows.Forms.Label();
            this.btnSimulate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDeltaT = new System.Windows.Forms.TextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNumCouriers
            // 
            this.txtNumCouriers.Location = new System.Drawing.Point(355, 32);
            this.txtNumCouriers.Margin = new System.Windows.Forms.Padding(4);
            this.txtNumCouriers.Name = "txtNumCouriers";
            this.txtNumCouriers.Size = new System.Drawing.Size(132, 22);
            this.txtNumCouriers.TabIndex = 0;
            // 
            // txtSimulationTime
            // 
            this.txtSimulationTime.Location = new System.Drawing.Point(355, 62);
            this.txtSimulationTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtSimulationTime.Name = "txtSimulationTime";
            this.txtSimulationTime.Size = new System.Drawing.Size(132, 22);
            this.txtSimulationTime.TabIndex = 1;
            // 
            // txtCourierSpeed
            // 
            this.txtCourierSpeed.Location = new System.Drawing.Point(355, 92);
            this.txtCourierSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.txtCourierSpeed.Name = "txtCourierSpeed";
            this.txtCourierSpeed.Size = new System.Drawing.Size(132, 22);
            this.txtCourierSpeed.TabIndex = 2;
            // 
            // txtOrderRate
            // 
            this.txtOrderRate.Location = new System.Drawing.Point(355, 122);
            this.txtOrderRate.Margin = new System.Windows.Forms.Padding(4);
            this.txtOrderRate.Name = "txtOrderRate";
            this.txtOrderRate.Size = new System.Drawing.Size(132, 22);
            this.txtOrderRate.TabIndex = 3;
            // 
            // lblNumCouriers
            // 
            this.lblNumCouriers.AutoSize = true;
            this.lblNumCouriers.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNumCouriers.Location = new System.Drawing.Point(24, 28);
            this.lblNumCouriers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNumCouriers.Name = "lblNumCouriers";
            this.lblNumCouriers.Size = new System.Drawing.Size(152, 26);
            this.lblNumCouriers.TabIndex = 4;
            this.lblNumCouriers.Text = "Число курьеров";
            // 
            // lblSimulationTime
            // 
            this.lblSimulationTime.AutoSize = true;
            this.lblSimulationTime.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSimulationTime.Location = new System.Drawing.Point(24, 58);
            this.lblSimulationTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSimulationTime.Name = "lblSimulationTime";
            this.lblSimulationTime.Size = new System.Drawing.Size(229, 26);
            this.lblSimulationTime.TabIndex = 5;
            this.lblSimulationTime.Text = "Время симуляции (мин)";
            // 
            // lblCourierSpeed
            // 
            this.lblCourierSpeed.AutoSize = true;
            this.lblCourierSpeed.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCourierSpeed.Location = new System.Drawing.Point(24, 88);
            this.lblCourierSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCourierSpeed.Name = "lblCourierSpeed";
            this.lblCourierSpeed.Size = new System.Drawing.Size(292, 26);
            this.lblCourierSpeed.TabIndex = 6;
            this.lblCourierSpeed.Text = "Средняя скорость курьера (км/ч)";
            // 
            // lblOrderRate
            // 
            this.lblOrderRate.AutoSize = true;
            this.lblOrderRate.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOrderRate.Location = new System.Drawing.Point(24, 118);
            this.lblOrderRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderRate.Name = "lblOrderRate";
            this.lblOrderRate.Size = new System.Drawing.Size(288, 26);
            this.lblOrderRate.TabIndex = 7;
            this.lblOrderRate.Text = "Интенсивность потока (1/мин)";
            // 
            // btnSimulate
            // 
            this.btnSimulate.BackColor = System.Drawing.Color.Thistle;
            this.btnSimulate.Font = new System.Drawing.Font("Sylfaen", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSimulate.Location = new System.Drawing.Point(354, 283);
            this.btnSimulate.Margin = new System.Windows.Forms.Padding(4);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(133, 35);
            this.btnSimulate.TabIndex = 8;
            this.btnSimulate.Text = "Симулировать";
            this.btnSimulate.UseVisualStyleBackColor = false;
            this.btnSimulate.Click += new System.EventHandler(this.btnSimulate_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResult.Location = new System.Drawing.Point(24, 13);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(104, 26);
            this.lblResult.TabIndex = 9;
            this.lblResult.Text = "Результат:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LemonChiffon;
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Location = new System.Drawing.Point(12, 340);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 343);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LemonChiffon;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtDeltaT);
            this.panel2.Controls.Add(this.btnSimulate);
            this.panel2.Controls.Add(this.txtNumCouriers);
            this.panel2.Controls.Add(this.txtSimulationTime);
            this.panel2.Controls.Add(this.txtCourierSpeed);
            this.panel2.Controls.Add(this.lblOrderRate);
            this.panel2.Controls.Add(this.txtOrderRate);
            this.panel2.Controls.Add(this.lblCourierSpeed);
            this.panel2.Controls.Add(this.lblNumCouriers);
            this.panel2.Controls.Add(this.lblSimulationTime);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(517, 322);
            this.panel2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sylfaen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(24, 148);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Шаг времени";
            // 
            // txtDeltaT
            // 
            this.txtDeltaT.Location = new System.Drawing.Point(355, 152);
            this.txtDeltaT.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeltaT.Name = "txtDeltaT";
            this.txtDeltaT.Size = new System.Drawing.Size(132, 22);
            this.txtDeltaT.TabIndex = 9;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Lavender;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(535, 12);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(749, 671);
            this.chart1.TabIndex = 12;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(1271, 695);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Курьерская Компания";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TextBox txtNumCouriers;
        private System.Windows.Forms.TextBox txtSimulationTime;
        private System.Windows.Forms.TextBox txtCourierSpeed;
        private System.Windows.Forms.TextBox txtOrderRate;
        private System.Windows.Forms.Label lblNumCouriers;
        private System.Windows.Forms.Label lblSimulationTime;
        private System.Windows.Forms.Label lblCourierSpeed;
        private System.Windows.Forms.Label lblOrderRate;
        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDeltaT;
    }
}
