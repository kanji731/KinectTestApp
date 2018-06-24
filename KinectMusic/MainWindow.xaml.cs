using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectMusic
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, Context
    {
        private Pause pause = null; //ポーズ判定
        private SkeletonInfo skeletonInfo = null; //骨格情報判定

        KinectSensor kinect;
        //RGBカメラの解像度・フレームレート
        ColorImageFormat rgbFormat = ColorImageFormat.RgbResolution640x480Fps30;
        //Kinectセンサーからの画像情報を受け取るバッファ
        private byte[] pixelBuffer = null;
        //Kinectセンサーからの骨格情報を受け取るバッファ
        private Skeleton[] skeletonBuffer = null;
        //画面に表示するビットマップ
        private WriteableBitmap bmpBuffer = null;
        //顔のビットマップイメージ
        private BitmapImage maskImage = null;
        //ビットマップへの描画用DrawingVisual
        private DrawingVisual drawVisual = new DrawingVisual();
        private const DepthImageFormat depthFormat = DepthImageFormat.Resolution640x480Fps30;
        //Kinectセンサーからの深度情報を受け取るバッファ
        private short[] depthBuffer = null;
        //深度情報の各点に対する画像情報上の座標
        private ColorImagePoint[] clrPntBuffer = null;
        //深度情報で背景を覆う画像データ
        private byte[] depMaskBuffer = null;

        private bool do1 = true;
        private bool re = true;
        private bool mi = true;
        private bool fa = true;
        private bool so = true;
        private bool ra = true;
        private bool si = true;
        private bool do2 = true;
        private bool janp = true;

        public MainWindow()
        {
            InitializeComponent();

            pause = new Pause(this);
            skeletonInfo = new SkeletonInfo(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Kinectセンサーの取得(エラー処理など省略版)
                kinect = KinectSensor.KinectSensors[0];

                //カラー・骨格ストリームの有効化
                ColorImageStream clrStream = kinect.ColorStream;
                clrStream.Enable(rgbFormat);
                SkeletonStream skelStream = kinect.SkeletonStream;
                skelStream.Enable();
                DepthImageStream depStream = kinect.DepthStream;
                depStream.Enable(depthFormat);

                //バッファの初期化
                pixelBuffer = new byte[kinect.ColorStream.FramePixelDataLength];
                skeletonBuffer = new Skeleton[skelStream.FrameSkeletonArrayLength];
                bmpBuffer = new WriteableBitmap(kinect.ColorStream.FrameWidth,
                    kinect.ColorStream.FrameHeight,
                    96, 96, PixelFormats.Bgr32, null);
                depthBuffer = new short[depStream.FramePixelDataLength];
                clrPntBuffer = new ColorImagePoint[depStream.FramePixelDataLength];
                rgbImage.Source = bmpBuffer;

                //イベントハンドラの登録
                kinect.ColorFrameReady += ColorImageReady;
                kinect.AllFramesReady += AllFramesReady;

                //Kinectセンサーからのストリーム取得を開始
                kinect.Start();
            }
            catch
            {
                Console.WriteLine("kinectがありません。");
            }
        }

        //ColorFrameReadyイベントのハンドラ(画像情報を取得して描画)
        private void ColorImageReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            try
            {
                using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
                {
                    if (imageFrame != null)
                    {
                        //画像情報の幅・高さ取得※途中で変わらない想定!
                        int frmWidth = imageFrame.Width;
                        int frmHeight = imageFrame.Height;

                        //画像情報をバッファにコピー
                        imageFrame.CopyPixelDataTo(pixelBuffer);

                        //ビットマップに描画
                        Int32Rect src = new Int32Rect(0, 0, frmWidth, frmHeight);
                        bmpBuffer.WritePixels(src, pixelBuffer, frmWidth * 4, 0);
                    }
                }
            }
            catch
            {
                Console.WriteLine("エラーが発生しました。");
            }
        }

        //イベントハンドラー
        private void AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    // 骨格位置の表示
                    ShowSkeleton(skeletonFrame);
                }
            }
        }

        private void ShowSkeleton(SkeletonFrame skeletonFrame)
        {
            // キャンバスをクリアする
            canvasSkeleton.Children.Clear();

            // スケルトンデータを取得する
            Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeletonData);

            // プレーヤーごとのスケルトンを描画する
            foreach (var skeleton in skeletonData)
            {
                // 追跡されているプレイヤー
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    Joint handRight = skeleton.Joints[JointType.HandRight];
                    var p = handRight.Position;
                    Joint headA = skeleton.Joints[JointType.Head];
                    var headP = headA.Position;
                    textBlock1.Text = headP.X.ToString();
                    textBlock3.Text = p.Z.ToString() + "M";

                    Joint head = skeleton.Joints[JointType.Head];
                    var p2 = head.Position;
                    a.Text = p2.Y.ToString();

                    //ポーズのチェック
                    int pianoPause = pause.PianoPause(skeleton);

                    if (pianoPause != 0)
                    {
                        skeletonInfo.Piano(p.Y, p.Z); //ヒアノ判定
                    }
                    skeletonInfo.Janp(p2.Y); //ジャンプ

                    // 骨格を描画する
                    foreach (Joint joint in skeleton.Joints)
                    {
                        // 追跡されている骨格
                        if (joint.TrackingState != JointTrackingState.NotTracked)
                        {
                            // 骨格の座標をカラー座標に変換する
                            ColorImagePoint point = kinect.MapSkeletonPointToColor(joint.Position, kinect.ColorStream.Format);

                            // 円を書く
                            canvasSkeleton.Children.Add(new Ellipse()
                            {
                                Margin = new Thickness(point.X, point.Y, 0, 0),
                                Fill = new SolidColorBrush(Colors.Green),
                                Width = 20,
                                Height = 20,
                            });
                        }
                    }
                }
            }
        }

        //-----------------------------------------
        //UIオブジェクトを返す
        public MediaElement getMediaElement()
        {
            return this.mediaElement1;
        }
        public List<TextBlock> getTextBlocks()
        {
            List<TextBlock> data = new List<TextBlock>();
            data.Add(b);
            data.Add(textBlock2);
            return data;
        }
        //-----------------------------------------
    }
}
