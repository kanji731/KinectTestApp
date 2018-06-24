using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KinectMusic
{
    public class SkeletonInfo
    {
        private Context context;
        private Audio.Audio audio;

        private double depth = 2.5; //2.5m以内で反応

        private bool do1 = true;
        private bool re = true;
        private bool mi = true;
        private bool fa = true;
        private bool so = true;
        private bool ra = true;
        private bool si = true;
        private bool do2 = true;
        private bool janp = true;

        public SkeletonInfo(Context context)
        {
            this.context = context;
            audio = new Audio.Audio(context);
        }

        //ヒアノ判定
        public void Piano(double y, double z)
        {
            if (y > 0.1 && y <= 0.2 && z < depth && do1 != false)
            {
                do1 = false;

                re = true;
                mi = true;
                fa = true;
                so = true;
                ra = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_1do.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ド";
            }
            else if (y > 0.2 && y <= 0.3 && z < depth && re != false)
            {
                re = false;

                do1 = true;
                mi = true;
                fa = true;
                so = true;
                ra = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_2re.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "レ";
            }
            else if (y > 0.3 && y <= 0.4 && z < depth && mi != false)
            {
                mi = false;

                do1 = true;
                re = true;
                fa = true;
                so = true;
                ra = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_3mi.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ミ";
            }
            else if (y > 0.4 && y <= 0.5 && z < depth && fa != false)
            {
                fa = false;

                do1 = true;
                re = true;
                mi = true;
                so = true;
                ra = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_4fa.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ファ";
            }
            else if (y > 0.5 && y <= 0.6 && z < depth && so != false)
            {
                so = false;

                do1 = true;
                re = true;
                mi = true;
                fa = true;
                ra = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_5so.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ソ";
            }
            else if (y > 0.6 && y <= 0.7 && z < depth && ra != false)
            {
                ra = false;

                do1 = true;
                re = true;
                mi = true;
                fa = true;
                so = true;
                si = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_6ra.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ラ";
            }
            else if (y > 0.7 && y <= 0.8 && z < depth && si != false)
            {
                si = false;

                do1 = true;
                re = true;
                mi = true;
                fa = true;
                so = true;
                ra = true;
                do2 = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_7si.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "シ";
            }
            else if (y > 0.8 && z < depth && do2 != false)
            {
                do2 = false;

                do1 = true;
                re = true;
                mi = true;
                fa = true;
                so = true;
                ra = true;
                si = true;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\piano1_8do.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ド";
            }
        }

        //ジャンプ音
        public void Janp(double y)
        {
            if (y > 0.6 && janp != false)
            {
                janp = false;

                string stFilePath = System.IO.Path.GetFullPath(@"..\..\Resources\janp.wav");
                ((MainWindow)context).getTextBlocks()[0].Text = stFilePath;
                audio.MEPlaySound(stFilePath);
                ((MainWindow)context).getTextBlocks()[1].Text = "ジャンプ";
            }
            else if (y <= 0.6)
            {
                janp = true;
            }
        }
    }
}
