using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectMusic
{
    public class Pause
    {
        private Context context;
        private Audio.Audio audio;
        private bool flag = true;

        public Pause(Context context)
        {
            this.context = context;
            audio = new Audio.Audio(context);
        }

        //ピアノ
        public int PianoPause(Skeleton skeleton)
        {
            //必要な関節・方向を取得
            Joint head = skeleton.Joints[JointType.Head];
            Joint handLeft = skeleton.Joints[JointType.HandLeft]; //左手
            Joint handRight = skeleton.Joints[JointType.HandRight]; //右手

            //1つでも位置がとれない場合は処理しない
            if ((head.TrackingState != JointTrackingState.Tracked
                && head.TrackingState != JointTrackingState.Inferred)
                || (handLeft.TrackingState != JointTrackingState.Tracked
                    && handLeft.TrackingState != JointTrackingState.Inferred)
                || (handRight.TrackingState != JointTrackingState.Tracked
                    && handRight.TrackingState != JointTrackingState.Inferred))
                return 0;

            //左手が頭より上にある
            bool check1 = (handLeft.Position.Y > head.Position.Y - 0.3);

            return (check1) ? 1 : 0;
        }
    }
}
