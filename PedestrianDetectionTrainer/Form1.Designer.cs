namespace PedestrianDetectionTrainer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора — не изменяйте
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadImagesButton = new System.Windows.Forms.Button();
            this.loadTrainFileButton = new System.Windows.Forms.Button();
            this.trainButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadImagesButton
            // 
            this.loadImagesButton.Location = new System.Drawing.Point(16, 15);
            this.loadImagesButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loadImagesButton.Name = "loadImagesButton";
            this.loadImagesButton.Size = new System.Drawing.Size(171, 39);
            this.loadImagesButton.TabIndex = 0;
            this.loadImagesButton.Text = "Загрузить изображения";
            this.loadImagesButton.UseVisualStyleBackColor = true;
            this.loadImagesButton.Click += new System.EventHandler(this.AddImagesButton_Click);
            // 
            // loadTrainFileButton
            // 
            this.loadTrainFileButton.Location = new System.Drawing.Point(16, 62);
            this.loadTrainFileButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loadTrainFileButton.Name = "loadTrainFileButton";
            this.loadTrainFileButton.Size = new System.Drawing.Size(171, 39);
            this.loadTrainFileButton.TabIndex = 1;
            this.loadTrainFileButton.Text = "Загрузить файл обучения";
            this.loadTrainFileButton.UseVisualStyleBackColor = true;
            this.loadTrainFileButton.Click += new System.EventHandler(this.LoadTrainFileButton_Click);
            // 
            // trainButton
            // 
            this.trainButton.Location = new System.Drawing.Point(16, 108);
            this.trainButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trainButton.Name = "trainButton";
            this.trainButton.Size = new System.Drawing.Size(171, 39);
            this.trainButton.TabIndex = 2;
            this.trainButton.Text = "Начать обучение";
            this.trainButton.UseVisualStyleBackColor = true;
            this.trainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 169);
            this.Controls.Add(this.trainButton);
            this.Controls.Add(this.loadTrainFileButton);
            this.Controls.Add(this.loadImagesButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TrainingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadImagesButton;
        private System.Windows.Forms.Button loadTrainFileButton;
        private System.Windows.Forms.Button trainButton;
    }
}
