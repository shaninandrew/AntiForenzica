﻿using System;
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
            long N = size / buffer.Length + 1;


            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < N; i++)
            { sw.Write(buffer); }


            sw.Close();
            sw.DisposeAsync();
            //удалить реально

            string new_name = System.IO.Path.GetDirectoryName(filename) + System.IO.Path.DirectorySeparatorChar + "193292892";

            File.Move(filename, new_name);
            File.Delete(new_name);

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
            PushSpace.BackColor = Color.Red;
            string msg = PushSpace.Text;



            string filename = "push.dat";

            System.IO.DriveInfo[] di = DriveInfo.GetDrives();

            int Runned = 0;

            foreach (DriveInfo d in di.AsParallel())
            {
                if ((d.DriveType == DriveType.Ram) || (d.DriveType == DriveType.CDRom) || (d.DriveType == DriveType.Network)) continue;

                Task t = new Task(() =>
                {

                Form f = new Form();
                TextBox txt = new TextBox();
                txt.Multiline = true;
                txt.Parent = f;
                txt.Dock = DockStyle.Fill;
                
                f.Top = 100*(Runned+1);
                f.Width = 700;
                f.FormBorderStyle = FormBorderStyle.None;
                f.Show();
                f.TopMost = true;


                DriveInfo DATA_DISK = d;
                string disk = DATA_DISK.Name;
                 f.Text = disk;

                long Mosaic = DATA_DISK.TotalFreeSpace / 4096 - 1; ;
                long M_S = DATA_DISK.TotalFreeSpace / Mosaic - 1;


               
                txt.Text = "Подготовка для " + disk;
                txt.Refresh();
                
                string DAT = "";
                byte[] DX = new byte[M_S];
                for (int i = 0; i < M_S; i++)
                {
                    DX[i] = (byte)((254 + i) % 255);
                }
                DAT = System.Text.Encoding.Default.GetString(DX);


                bool BREAK_PROCESS = false;
                string[] file_names = new string[Mosaic+1];

                
                if (disk == "Z:\\")
                    {
                        //for debug obly
                        int x = 0;
                    }

                List <string> done_files= new System.Collections.Generic.List<string>();
                done_files.Clear();

                //создаем массив
                for (long j = 0; j < Mosaic; j++)
                {
                    file_names[j] = disk + j + ".xxx-" + j + "";
                }

                for (long j = 0; j < Mosaic; j++)
                {

                        if (j % 100 == 1)
                        {
                            txt.Text = "Загрузка на " + DATA_DISK.Name + " " + DATA_DISK.AvailableFreeSpace / 1000000 + " MB  / Потоков " + Runned.ToString() + " / " + ((int)((double)j * 100 / Mosaic)).ToString() + "%";
                            txt.Refresh();
                            if (j%200==1) f.Refresh();


                            if (DATA_DISK.AvailableFreeSpace < 5024)
                            { BREAK_PROCESS = true; }
                        }



                        if (BREAK_PROCESS) break;

                        long index = (long)j + 0;

                        try
                        {
                               
                            string tmp_filename2 = file_names[index];

                            // if (File.Exists(tmp_filename2))
                            //     try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Normal); } catch { }

                            if (!File.Exists(tmp_filename2))
                            {
                                try
                                {
                                    System.IO.File.WriteAllText(tmp_filename2, DAT);
                                //     try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Hidden); } catch { }
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
                        }
                        catch (Exception e)
                        {
                            if (e.Message.ToLower().IndexOf("access to the path") > -1)
                            { BREAK_PROCESS = true; }


                        }
                        finally
                        {
                          

                        }
                } //for


                
                int errors = 0;
                Runned=0;

                int ji = -1;
                foreach (string fn in done_files)
                {

                        ji++;

                        if (ji % 100 == 1)
                        {
                            txt.Text = "Разгрузка на " + DATA_DISK.Name + " " + DATA_DISK.AvailableFreeSpace / 1000000 + " MB  / Потоков " + Runned.ToString() + " / " + ((int)((double)ji * 100 / Mosaic)).ToString() + "%";
                            txt.Refresh();
                            if (ji % 200 == 1) f.Refresh();
                        }
                            

                        try
                        {

                            string tmp_filename2 = fn + "";
                            


                            if (System.IO.File.Exists(tmp_filename2))
                            {

                                //    try { System.IO.File.SetAttributes(tmp_filename2, FileAttributes.Normal); } catch { }

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



                Runned = Runned - 1;
                txt.Dispose();
                f.Dispose();
            });

                Runned++;
                t.Start();

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
    }
}