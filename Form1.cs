using System;
using System.Diagnostics;

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


        bool Operation = false;


        private async void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            Operation = true;

            Task t = new Task(async () =>
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

            });


            t.Start();
            t.ContinueWith((x) => { Operation = false; });

            timer1.Start();



        }



        async Task DeleteSecure(string filename)
        {

            FileInfo info = new System.IO.FileInfo(filename);
            long size = info.Length;

            char[] buffer = new char[500000];        //500 кб
            //заполняем 111111110
            for (int i = 0; i < buffer.Length; i++) buffer[i] = i % 2 == 0 ? '■' : '◘';

            //файл запишем длиннее
            long N = size / buffer.Length + 1;

            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < N; i++)
                { sw.Write(buffer); }


                sw.Close();
                sw.DisposeAsync();
            }
            catch { }
            //удалить реально

            try
            {
                string new_name = System.IO.Path.GetDirectoryName(filename) + System.IO.Path.DirectorySeparatorChar + "193292892";

                File.Move(filename, new_name);
                File.Delete(new_name);

            }
            catch
            {

                File.Delete(filename);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(folderBrowserDialog1.SelectedPath);

                foreach (FileInfo f in di.GetFiles("*.*", SearchOption.AllDirectories))
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
            PushSpace.BackColor = Color.Red;
            string msg = PushSpace.Text;



            string filename = "push.dat";

            System.IO.DriveInfo[] di = DriveInfo.GetDrives();

            int Runned = 0;
            int h = 0;
            foreach (DriveInfo d in di.AsParallel())
            {
                if ((d.DriveType == DriveType.Ram) || (d.DriveType == DriveType.CDRom) || (d.DriveType == DriveType.Network)) continue;

                h++;
                Task t = new Task(async () =>
                {

                    Form f = new Form();
                    f.StartPosition = FormStartPosition.CenterParent;
                    TextBox txt = new TextBox();
                    txt.Multiline = true;
                    txt.Parent = f;
                    txt.Dock = DockStyle.Fill;

                    f.Height = 100;

                    f.Top = (f.Height) * (h + 1) + 5;
                    f.Width = 700;
                    f.FormBorderStyle = FormBorderStyle.None;
                    f.Show();
                    f.TopMost = true;

                    bool BREAK_PROCESS = false;

                    List<string> done_files = new System.Collections.Generic.List<string>();
                    done_files.Clear();
                    DriveInfo DATA_DISK = d;
                    long Mosaic = 1000;

                    try { 
                        
                        string disk = DATA_DISK.Name;
                        f.Text = disk;

                        Mosaic = DATA_DISK.TotalFreeSpace / 4096 - 1; ;
                        long M_S = DATA_DISK.TotalFreeSpace / Mosaic - 1;

                        Runned++;

                        txt.Text = "Подготовка для " + disk;
                        txt.Refresh();

                        string DAT = "";
                        byte[] DX = new byte[M_S];
                        for (int i = 0; i < M_S; i++)
                        {
                            DX[i] = (byte)((254 + i) % 255);
                        }
                        DAT = System.Text.Encoding.Default.GetString(DX);


                       
                        string[] file_names = new string[Mosaic + 1];




                        //создаем массив
                        for (long j = 0; j < Mosaic; j++)
                        {
                            file_names[j] = disk + j.ToString().PadLeft(9, '0') + ".xxx-" + j.ToString().PadLeft(9, '0') + "";
                        }

                        for (long j = 0; j < Mosaic; j++)
                        {

                            if (j % 100 == 1)
                            {
                                txt.Text = "Загрузка на " + DATA_DISK.Name + " " + DATA_DISK.AvailableFreeSpace / 1000000 + " MB  / Потоков " + Runned.ToString() + " / " + ((int)((double)j * 100 / Mosaic)).ToString() + "%";
                                txt.Refresh();
                                if (j % 200 == 1) f.Refresh();


                                if (DATA_DISK.AvailableFreeSpace < 5024)
                                { BREAK_PROCESS = true; }
                            }



                            if (BREAK_PROCESS) break;

                            long index = (long)j + 0;


                            string tmp_filename2 = file_names[index];

                            if (File.Exists(tmp_filename2))
                                try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Hidden); } catch { }

                            if (!File.Exists(tmp_filename2))
                            {
                                try
                                {
                                    System.IO.File.WriteAllText(tmp_filename2, DAT);
                                    try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Hidden); } catch { }
                                    done_files.Add(tmp_filename2);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);

                                    if (DATA_DISK.AvailableFreeSpace < 5024)
                                    { BREAK_PROCESS = true; }

                                    if (ex.Message.ToLower().IndexOf("access to the path") > -1)
                                    {
                                        BREAK_PROCESS = true;

                                    }
                                }
                            }
                        }//for
                    }
                    catch (Exception e)
                    {
                        if (e.Message.ToLower().IndexOf("access to the path") > -1)
                        { BREAK_PROCESS = true; }


                    }
                    finally
                    {


                    }
                



                    int errors = 0;
                   

                    int ji = -1;
                    foreach (string fn in done_files.AsParallel())
                    {

                        ji++;

                        if (ji % 100 == 1)
                        {
                            txt.Text = "Разгрузка на " + DATA_DISK.Name + " " + DATA_DISK.AvailableFreeSpace / 1000000 
                            + " MB  / Потоков " + Runned.ToString() + " / " + ((int)((double)ji * 100 / Mosaic)).ToString() + "%";
                            txt.Refresh();
                            if (ji % 200 == 1) f.Refresh();
                        }


                        try
                        {

                            string tmp_filename2 = fn + "";



                            if (System.IO.File.Exists(tmp_filename2))
                            {

                                try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Normal); } catch { }

                                try
                                {

                                    System.IO.File.Delete(tmp_filename2);
                                    errors = 0;
                                }
                                catch { errors++; };


                            }
                            else
                            {
                                errors++;
                            }

                            //не ломай комедию
                            /*if (errors > 10)
                            {
                                BREAK_PROCESS = true;
                            }     */

                        }
                        catch (Exception e)
                        {
                            if (e.Message.ToLower().IndexOf("access to the path") > -1)
                            { BREAK_PROCESS = true; }

                        }
                        finally
                        {


                            if (!BREAK_PROCESS)
                            {

                            }

                        }

                    }

                   
                    txt.Dispose();
                    f.Dispose();
                });

               
                t.Start();
                t.ContinueWith((x) => { Runned = Runned - 1; });

                while (Runned > 0)
                {
                    lock (PushSpace)
                    {
                        PushSpace.Text = " Ждем очистку " + d.Name + " " + d.AvailableFreeSpace / 1000000 + " MB  / Потоков " + Runned.ToString();
                        PushSpace.Refresh();
                    }
                    Thread.Sleep(1000);

                }


        }   // foreach


            PushSpace.Text = msg;
            PushSpace.BackColor = SystemColors.ButtonFace;
            PushSpace.Enabled = !false;
        }

        private void button3_Click(object sender, EventArgs e)
        {


            FileLst.Items.Remove(FileLst.SelectedItem);

        }

        bool Clean_Pusher = false;
        private void CLean_Click(object sender, EventArgs e)
        {
            CLean.Enabled = false;
            CLean.BackColor = Color.DarkMagenta;
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            timer1.Start();
            Clean_Pusher = true;
            int h = 0;
            foreach (DriveInfo d in drives.AsParallel())
            {
                if ((d.DriveType == DriveType.Ram) || (d.DriveType == DriveType.CDRom) || (d.DriveType == DriveType.Network)) continue;

                Task t = new Task( async () =>
                {
                    string p = d.Name;
                    try
                    {
                        string[] lst_files = System.IO.Directory.GetFiles(p, "*.xxx*", SearchOption.TopDirectoryOnly);

                        h++;
                        foreach (string s in lst_files)
                            try
                            {
                                System.IO.File.SetAttributes(s, FileAttributes.Normal);
                                System.IO.File.Delete(s);
                            }
                            catch { }

                    }
                    catch { }

                });
                

                t.ContinueWith(async (t) =>
                {
                    h--;
                    if (h <= 0) {
                        Clean_Pusher = false;
                    }
                });

                t.Start();
            }







        }

        bool Cleaner_logs = false;
        private void button4_Click(object sender, EventArgs e)
        {
            CleanHDD.Enabled = false;
            CleanHDD.BackColor = Color.Cyan;
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            int h = 0;
            Cleaner_logs = true;
            timer1.Start();

            foreach (DriveInfo d in drives)
            {
                if ((d.DriveType == DriveType.Ram) || (d.DriveType == DriveType.CDRom) || (d.DriveType == DriveType.Network)) continue;

                List<string> lst_files = new List<string>();
                lst_files.Clear();

                Task t = new Task(async () =>
                {
                    
                    string p = d.Name + "";
                    try
                    {
                       
                       
                          h++;
                        foreach (string dirs in System.IO.Directory.GetDirectories(p, "*.*", SearchOption.TopDirectoryOnly))
                        {

                            try
                            {
                                string[] lst = System.IO.Directory.GetFiles(dirs, "*.*", SearchOption.AllDirectories);
                                
                                lst_files.AddRange(lst.Where(x => System.IO.Path.GetExtension(x).ToLower().Equals(".tmp")));
                                lst_files.AddRange(lst.Where(x => System.IO.Path.GetExtension(x).ToLower().Equals(".$$$")));
                                lst_files.AddRange(lst.Where(x => System.IO.Path.GetExtension(x).ToLower().Equals(".dmp")));
                                lst_files.AddRange(lst.Where(x => System.IO.Path.GetExtension(x).ToLower().Equals(".log")));
                            }
                            catch { }

                        }



                        foreach (string s in lst_files)
                           new Task( async()=>
                               {
                                   try
                                   {
                                      
                                       System.IO.File.SetAttributes(s, FileAttributes.Normal);
                                       //журналы удаляются безопасно
                                       DeleteSecure(s);
                                   }
                                   catch { }
                                   finally { }
                               }).Start();

                        

                    }
                    catch
                    { }

                });

                
                t.ContinueWith(async (t) =>
                {
                    h--;
                    if (h <= 0) 
                    {
                        Cleaner_logs = !true;
                        lst_files.Clear();
                    }
                });
                t.Start();

            }




        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Operation)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                FileLst.Items.Clear();
                
            }


            if (!Cleaner_logs)
            { 
                     CleanHDD.Enabled = !false;
                     CleanHDD.BackColor = SystemColors.ButtonFace;
                     
            }


            if (!Clean_Pusher)
            { 
                    CLean.Enabled = true;
                    CLean.BackColor = SystemColors.ButtonFace;
            }



            if ((!Operation) && (!Cleaner_logs) && (!Clean_Pusher))
                    {
                        timer1.Stop();
                    }

        }
    }

}