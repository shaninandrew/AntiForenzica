namespace AntiForenzica
{
    partial class FormDel
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            FileLst = new ListBox();
            label2 = new Label();
            button1 = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button2 = new Button();
            PushSpace = new Button();
            button3 = new Button();
            CLean = new Button();
            CleanHDD = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(180, 15);
            label1.TabIndex = 0;
            label1.Text = "Список уничтожаемых файлов";
            // 
            // FileLst
            // 
            FileLst.AllowDrop = true;
            FileLst.FormattingEnabled = true;
            FileLst.ItemHeight = 15;
            FileLst.Location = new Point(12, 44);
            FileLst.Name = "FileLst";
            FileLst.Size = new Size(946, 274);
            FileLst.TabIndex = 1;
            FileLst.DragDrop += FileLst_DragDrop;
            FileLst.DragEnter += FileLst_DragEnter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 321);
            label2.Name = "label2";
            label2.Size = new Size(180, 15);
            label2.TabIndex = 2;
            label2.Text = "Перетащите удаляемые файлы";
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Popup;
            button1.Location = new Point(12, 349);
            button1.Name = "button1";
            button1.Size = new Size(946, 40);
            button1.TabIndex = 3;
            button1.Text = "Удалить с перепиской содержимого";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.HelpRequest += folderBrowserDialog1_HelpRequest;
            // 
            // button2
            // 
            button2.Location = new Point(867, 12);
            button2.Name = "button2";
            button2.Size = new Size(91, 26);
            button2.TabIndex = 4;
            button2.Text = "+Папка";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // PushSpace
            // 
            PushSpace.FlatStyle = FlatStyle.Popup;
            PushSpace.Location = new Point(12, 395);
            PushSpace.Name = "PushSpace";
            PushSpace.Size = new Size(946, 42);
            PushSpace.TabIndex = 5;
            PushSpace.Text = "Запушить место (долго!)";
            PushSpace.UseVisualStyleBackColor = true;
            PushSpace.Click += PushSpace_Click;
            // 
            // button3
            // 
            button3.Location = new Point(676, 12);
            button3.Name = "button3";
            button3.Size = new Size(166, 26);
            button3.TabIndex = 6;
            button3.Text = "Убрать из списка";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // CLean
            // 
            CLean.FlatStyle = FlatStyle.Popup;
            CLean.Location = new Point(12, 443);
            CLean.Name = "CLean";
            CLean.Size = new Size(946, 41);
            CLean.TabIndex = 7;
            CLean.Text = "Очистить диски от следов пуша";
            CLean.UseVisualStyleBackColor = true;
            CLean.Click += CLean_Click;
            // 
            // CleanHDD
            // 
            CleanHDD.FlatStyle = FlatStyle.Popup;
            CleanHDD.Location = new Point(12, 523);
            CleanHDD.Name = "CleanHDD";
            CleanHDD.Size = new Size(946, 41);
            CleanHDD.TabIndex = 8;
            CleanHDD.Text = "Очистить диски от журналов";
            CleanHDD.UseVisualStyleBackColor = true;
            CleanHDD.Click += button4_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // FormDel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(970, 576);
            Controls.Add(CleanHDD);
            Controls.Add(CLean);
            Controls.Add(button3);
            Controls.Add(PushSpace);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(FileLst);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormDel";
            Text = "Программный шредер";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox FileLst;
        private Label label2;
        private Button button1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button2;
        private Button PushSpace;
        private Button button3;
        private Button CLean;
        private Button CleanHDD;
        private System.Windows.Forms.Timer timer1;
    }
}