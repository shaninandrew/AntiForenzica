namespace AntiForenzica
{
    public partial class FormDel : Form
    {
        public FormDel()
        {
            InitializeComponent();
            this.AllowDrop = true;

        }

        private void FileLst_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data;
            if (data != null)
            {
                e.Effect = DragDropEffects.Move;
                FileLst.Items.AddRange((string[])data.GetData(DataFormats.FileDrop));
            }

        }

        private void FileLst_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data == null) || e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }





        private async void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            Label label = new Label();
            label.Parent = f;

            f.Text = "Удаление в прогрессе";
            label.Text = "Идет удаление файлов"; ;
            label.ForeColor = Color.White;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.CenterScreen;
            // f.Parent = this;

            label.Top = 30;
            label.Left = 30;
            f.Width = 70;
            f.Height = 50;
            f.BackColor = Color.Red;

            f.Show();
            f.Refresh();


            foreach (string s in FileLst.Items)
            {

                try { await DeleteSecure(s); } catch { }
            }


            f.Close();
            f.Dispose();
            FileLst.Items.Clear();

        }



        async Task DeleteSecure(string filename)
        {

            FileInfo info = new System.IO.FileInfo(filename);
            long size = info.Length;

            char[] buffer = new char[500000];        //500 кб
            //заполняем 111111110
            for (int i = 0; i < buffer.Length; i++) buffer[i] = i % 2 == 0 ? '■' : '◘';

            //файл запишем длиннее
            long N = size / buffer.Length+1;
           

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < N; i++)
            { sw.Write(buffer); }


            sw.Close();
            sw.DisposeAsync();
            //удалить реально

            File.Delete(filename);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(folderBrowserDialog1.SelectedPath);

                foreach (FileInfo f in di.GetFiles("*.*"))
                {
                    string s = f.FullName;
                    FileLst.Items.Add(s);
                }


            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private async void PushSpace_Click(object sender, EventArgs e)
        {
            PushSpace.Enabled = false;
            PushSpace.BackColor= Color.Red;
            string msg = PushSpace.Text;
            

            


            string filename = "push.dat";

            System.IO.DriveInfo[] di = DriveInfo.GetDrives();

            foreach (DriveInfo d in di.AsParallel())
            {
                if ((d.DriveType==DriveType.Ram) || (d.DriveType == DriveType.CDRom) || (d.DriveType==DriveType.Network)) continue;

                    long Mosaic = 10000;
                    long M_S = d.TotalFreeSpace / Mosaic-1;

                    PushSpace.Text = "Подготовка для " +d.Name;
                    PushSpace.Refresh();
                    string DAT = "";
                    byte[] DX  =  new  byte[M_S];
                    for (int i = 0; i < M_S; i++)
                    {
                        DX[i] =  (byte) ( (254 + i)% 255 );
                    }
                    DAT = System.Text.Encoding.Default.GetString(DX);


                    for (int j = 0; j < Mosaic; j++)
                    {
                        try
                        {
                            string tmp_filename2 = d.Name + j + ".xxx-"+j+".dat";
                            await System.IO.File.WriteAllTextAsync(tmp_filename2, DAT);

                            if (j % 50 == 1)
                            {
                                PushSpace.Text = d.Name + " " + d.AvailableFreeSpace / 1000000 + " MB";
                                PushSpace.Refresh();
                                this.Refresh();
                            }


                    }
                    catch
                        { break;  }

                    }


                    for (int j = 0; j < Mosaic; j++)
                    {
                    try
                    {
                        string tmp_filename2 = d.Name + j + ".xxx-" + j + ".dat";
                        System.IO.File.Delete(tmp_filename2);


                    }
                    catch
                    { break; }
                    finally
                    {
                        if (j % 50 == 1)
                        {
                            PushSpace.Text = d.Name + " " + d.AvailableFreeSpace / 1000000 + " MB";
                            PushSpace.Refresh();
                        }

                    }

                    }


                    /*

                long size = d.AvailableFreeSpace;
                    string tmp_filename = d.Name + filename;

                    char[] buffer = new char[1000000];        //1000 кб
                                                              //заполняем 111111110
                    for (int i = 0; i < buffer.Length; i++) buffer[i] = i % 2 == 0 ? '■' : '◘';

                    //файл запишем длиннее
                    long N = size / buffer.Length-1;
                    

                    StreamWriter sw = null;


                    try
                    {
                        FileStream fs = new FileStream(tmp_filename, FileMode.OpenOrCreate, FileAccess.Write);
                        sw = new StreamWriter(fs);


                        for (int i = 0; i < N; i++)
                            try
                            {
                                sw.Write(buffer);

                            }
                            catch { }
                    }

                    catch
                    {

                    }
                    finally
                    {

                        
                        if (sw != null)
                        {
                            try { sw.Close(); } catch { }
                            try { sw.DisposeAsync(); } catch { }
                        }
                    }
              
              

                for (int i = 0; i<5;i++)
                    try
                    {
                            Thread.Sleep(100);
                            File.Delete(tmp_filename);
                    }
                    catch { }

                               */
                
            }       // foreach

            PushSpace.Text = msg;
            PushSpace.BackColor = SystemColors.ButtonFace;
            PushSpace.Enabled = !false;
        }
    }
}