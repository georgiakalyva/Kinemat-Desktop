using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using Kinemat.Models.Games;

namespace Kinemat.Controls.Boards
{
	/// <summary>
	/// Interaction logic for VirtualDrumKitGameBoard.xaml
	/// </summary>
	public partial class VirtualDrumKitGameBoard : UserControl
	{
		int sec = 0;

		DispatcherTimer fakeRecognition = new DispatcherTimer();

		private int CurrentRegion = 0;
		private int index = 0;

		private int maxIndex = 5;
		private int[] sequence = new int[500];
		ImageSource[] src = new ImageSource[4];

		Random rand = new Random();
		private bool CanPlay = false;
		Storyboard Drum1, Drum2, Drum3, Drum4, Hint;
		MediaPlayer drum1, drum2, drum3, drum4, correctSequence, WIN;

		private int FailCount;
		private int correct;
		private int maxGameIndex = 8;
		Storyboard s, feedstory;

		private VirtualDrumKitGame game;

		private Action<object, RoutedEventArgs> action;

		public VirtualDrumKitGameBoard()
		{
			InitializeComponent();
		}

		        private void SetNumbers()
        {
            for (int i = 0; i < 4; i++)
            {
                if (sequence[0] == i)
                {
                    SequenceIMG1.Source = src[i];
                }
                if (sequence[1] == i)
                {
                    SequenceIMG2.Source = src[i];
                }
                if (sequence[2] == i)
                {
                    SequenceIMG3.Source = src[i];
                }
                if (sequence[3] == i)
                {
                    SequenceIMG4.Source = src[i];
                }
                if (sequence[4] == i)
                {
                    SequenceIMG5.Source = src[i];
                }
                if (sequence[5] == i)
                {
                    SequenceIMG6.Source = src[i];
                }
                if (sequence[6] == i)
                {
                    SequenceIMG7.Source = src[i];
                }
                if (sequence[7] == i)
                {
                    SequenceIMG8.Source = src[i];
                }
                if (sequence[8] == i)
                {
                    SequenceIMG9.Source = src[i];
                }
                if (sequence[9] == i)
                {
                    SequenceIMG10.Source = src[i];
                }
                if (sequence[10] == i)
                {
                    SequenceIMG11.Source = src[i];
                }
                if (sequence[11] == i)
                {
                    SequenceIMG12.Source = src[i];
                }
                if (sequence[12] == i)
                {
                    SequenceIMG13.Source = src[i];
                }
                if (sequence[13] == i)
                {
                    SequenceIMG14.Source = src[i];
                }
            }


            if (maxIndex == 14)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = SequenceIMG13.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = SequenceIMG14.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 13)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = SequenceIMG13.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 12)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 11)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 10)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility =
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 9)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility =
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 8)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility =
                    SequenceIMG5.Visibility =
                    SequenceIMG6.Visibility =
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 7)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility =
                    SequenceIMG5.Visibility =
                    SequenceIMG6.Visibility =
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 6)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility =
                    SequenceIMG5.Visibility =
                    SequenceIMG6.Visibility = Visibility.Visible;
            }
            if (maxIndex == 5)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility =
                    SequenceIMG5.Visibility = Visibility.Visible;
            }
        }


        private void Randomizer(int MAX)
        {
            if (maxIndex == 6)
                SequenceIMG6.Visibility = Visibility.Visible;
            if (maxIndex == 7)
                SequenceIMG7.Visibility = Visibility.Visible;
            if (maxIndex == 8)
                SequenceIMG8.Visibility = Visibility.Visible;
            if (maxIndex == 9)
                SequenceIMG9.Visibility = Visibility.Visible;
            if (maxIndex == 10)
                SequenceIMG10.Visibility = Visibility.Visible;
            if (maxIndex == 11)
                SequenceIMG11.Visibility = Visibility.Visible;
            if (maxIndex == 12)
                SequenceIMG12.Visibility = Visibility.Visible;
            if (maxIndex == 13)
                SequenceIMG13.Visibility = Visibility.Visible;
            if (maxIndex == 14)
                SequenceIMG14.Visibility = Visibility.Visible;

            sequence[0] = rand.Next(0, 4);
            for (int i = 1; i < MAX; i++)
            {
                sequence[i] = rand.Next(0, 4);
            }
            SetNumbers();
        }

        #region Event handlers

        private void GameWindowLoaded(object sender, RoutedEventArgs e)
        {
            Drum1 = (Storyboard)Resources["Drum1"];
            Drum2 = (Storyboard)Resources["Drum2"];
            Drum3 = (Storyboard)Resources["Drum3"];
            Drum4 = (Storyboard)Resources["Drum4"];
            Hint = (Storyboard)Resources["LetterFade"];
            s = (Storyboard)Resources["MEDIA"];
            feedstory = (Storyboard)Resources["Feedback"];
            //msg = (Storyboard)Resources["MessageBlink"];
           // msg.Begin();
        }

        private void GameWindowContentRendered(object sender, EventArgs e)
        {
            // Set the hot spot regions
            KinectRegion.AddHandPointerEnterHandler(RECT1, SetHandEntersHotspot1);
            KinectRegion.AddHandPointerLeaveHandler(RECT1, SetHandLeavesHotspot1);

            KinectRegion.AddHandPointerEnterHandler(RECT2, SetHandEntersHotspot2);
            KinectRegion.AddHandPointerLeaveHandler(RECT2, SetHandLeavesHotspot2);

            KinectRegion.AddHandPointerEnterHandler(RECT3, SetHandEntersHotspot3);
            KinectRegion.AddHandPointerLeaveHandler(RECT3, SetHandLeavesHotspot3);

            KinectRegion.AddHandPointerEnterHandler(RECT4, SetHandEntersHotspot4);
            KinectRegion.AddHandPointerLeaveHandler(RECT4, SetHandLeavesHotspot4);

            KinectRegion.AddHandPointerEnterHandler(PlayGameBTN, SetHandEntersHotspot5);
            KinectRegion.AddHandPointerLeaveHandler(PlayGameBTN, SetHandLeavesHotspot5);
        }

        private void SetHandLeavesHotspot5(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = -1;
            sec = 0;
            fakeRecognition.Stop();
        }

        private void SetHandEntersHotspot5(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = 5;
            fakeRecognition.Start();
        }

        private void SetHandEntersHotspot4(object sender, HandPointerEventArgs e)
        {
            if (CanPlay)
            {
                CurrentRegion = 3;
                fakeRecognition.Start();
            }
        }

        private void SetHandLeavesHotspot4(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = -1;
            sec = 0;
            fakeRecognition.Stop();
        }

        private void SetHandLeavesHotspot3(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = -1;
            sec = 0;
            fakeRecognition.Stop();
        }

        private void SetHandEntersHotspot3(object sender, HandPointerEventArgs e)
        {
            if (CanPlay)
            {
                CurrentRegion = 2;
                fakeRecognition.Start();
            }
        }

        private void SetHandLeavesHotspot2(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = -1;
            sec = 0;
            fakeRecognition.Stop();
        }

        private void SetHandEntersHotspot2(object sender, HandPointerEventArgs e)
        {
            if (CanPlay)
            {
                CurrentRegion = 1;
                fakeRecognition.Start();
            }
        }

        private void SetHandLeavesHotspot1(object sender, HandPointerEventArgs e)
        {
            CurrentRegion = -1;
            sec = 0;
            fakeRecognition.Stop();
        }

        private void SetHandEntersHotspot1(object sender, HandPointerEventArgs e)
        {
            if (CanPlay)
            {
                CurrentRegion = 0;
                fakeRecognition.Start();
            }
        }

		public void Start(VirtualDrumKitGame game, Action<object,RoutedEventArgs> action)
		{
			this.game = game;
			this.action = action;
		}

        private void AnswerCountdown(object sender, EventArgs e)
        {
            sec++;
            if (sec == 2)
            {
                if (CurrentRegion == 5)
                {
                    CanPlay = true;
                    Hint.Begin();
                    SequenceIMG1.Visibility = SequenceIMG2.Visibility = 
                        SequenceIMG3.Visibility = SequenceIMG4.Visibility =
                        SequenceIMG5.Visibility = sequence_Required.Visibility = 
                        failed.Visibility = Visibility.Visible;
                    PlayGameBTN.Visibility = Visibility.Collapsed;
                    Randomizer(maxIndex);
                    MSG.Visibility = Visibility.Visible;
                    return;
                    
                }
                if (CurrentRegion == 3)
                {
                    drum4 = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
                    drum4.Open(new Uri(System.IO.Path.GetFullPath("sfx/Crash.wav"))); //Open the file for a media playback
                    drum4.Play();
                    Drum4.Begin();
                }
                if (CurrentRegion == 2)
                {

                    drum3 = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
					drum3.Open(new Uri(System.IO.Path.GetFullPath("sfx/Tom-05.wav"))); //Open the file for a media playback
                    drum3.Play();
                    Drum3.Begin();
                }
                if (CurrentRegion == 1)
                {
                    drum2 = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
					drum2.Open(new Uri(System.IO.Path.GetFullPath("sfx/Tom H.wav"))); //Open the file for a media playback
                    drum2.Play();
                    Drum2.Begin();
                }
                if (CurrentRegion == 0)
                {
                    drum1 = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
					drum1.Open(new Uri(System.IO.Path.GetFullPath("sfx/Tom L.wav"))); //Open the file for a media playback

                    Drum1.Begin();
                    drum1.Play();
                }

                if (maxIndex == maxGameIndex) return;

                if (CurrentRegion == sequence[correct])
                {
                    //Swsto



                    correct++;
                    // HideBasedOnCorrect();

                    if (correct == maxIndex)
                    {
                        correctSequence = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
						correctSequence.Open(new Uri(System.IO.Path.GetFullPath("sfx/choice2.wav"))); //Open the file for a media playback
                        correctSequence.Play();
                        index = 0;
                        correct = 0;
                        maxIndex += 1;
                        if (maxIndex == maxGameIndex)
                        {

                            s.Stop();
                            WIN = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
							WIN.Open(new Uri(System.IO.Path.GetFullPath("sfx/Hotel 1 Level Complete.mp3"))); //Open the file for a media playback
                            WIN.Play();
                            Hotel_1_3___New_mp3.Volume = 0;
                            Hotel_1_3___New_mp3.IsMuted = true;
                            MSG.Text = "You Win!";
                            failed.Visibility = LETTERSTACKPANEL.Visibility = sequence_Required.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ReturnFeedback();
                            MSG.Text = "Round " + (maxIndex - 4).ToString();
                            Randomizer(maxIndex);
                        }
                    }
                    HideBasedOnCorrect();




                }
                else
                {
                    if (maxIndex != maxGameIndex)
                    {
                        drum1 = new MediaPlayer(); //Initialize a new instance of MediaPlayer of name countingSound
						drum1.Open(new Uri(System.IO.Path.GetFullPath("sfx/failure1.wav"))); //Open the file for a media playback
                        drum1.Play();
                        ResetSequence();
                    }
                }
            }
        }

        private void HideBasedOnCorrect()
        {
            if (correct == 1)
                SequenceIMG1.Visibility = Visibility.Collapsed;
            if (correct == 2)
                SequenceIMG2.Visibility = Visibility.Collapsed;
            if (correct == 3)
                SequenceIMG3.Visibility = Visibility.Collapsed;
            if (correct == 4)
                SequenceIMG4.Visibility = Visibility.Collapsed;
            if (correct == 5)
                SequenceIMG5.Visibility = Visibility.Collapsed;
            if (correct == 6)
                SequenceIMG6.Visibility = Visibility.Collapsed;
            if (correct == 7)
                SequenceIMG7.Visibility = Visibility.Collapsed;
            if (correct == 8)
                SequenceIMG8.Visibility = Visibility.Collapsed;
            if (correct == 9)
                SequenceIMG9.Visibility = Visibility.Collapsed;
            if (correct == 10)
                SequenceIMG10.Visibility = Visibility.Collapsed;
            if (correct == 11)
                SequenceIMG11.Visibility = Visibility.Collapsed;
            if (correct == 12)
                SequenceIMG12.Visibility = Visibility.Collapsed;
            if (correct == 13)
                SequenceIMG13.Visibility = Visibility.Collapsed;
            if (correct == 14)
                SequenceIMG14.Visibility = Visibility.Collapsed;
        }

        private void ResetSequence()
        {
            if (maxIndex == 14)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = SequenceIMG13.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = SequenceIMG14.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 13)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = SequenceIMG13.Visibility =
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = 
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 12)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = 
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = 
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility =
                    SequenceIMG7.Visibility = SequenceIMG12.Visibility = Visibility.Visible;
            }
            if (maxIndex == 11)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = 
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility =
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = SequenceIMG11.Visibility = 
                    SequenceIMG7.Visibility  = Visibility.Visible;
            }
            if (maxIndex == 10)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = 
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = 
                    SequenceIMG6.Visibility = SequenceIMG10.Visibility = 
                    SequenceIMG7.Visibility  = Visibility.Visible;
            }
            if (maxIndex == 9)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = 
                    SequenceIMG5.Visibility = SequenceIMG9.Visibility = 
                    SequenceIMG6.Visibility = 
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 8)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = SequenceIMG8.Visibility = 
                    SequenceIMG5.Visibility = 
                    SequenceIMG6.Visibility = 
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 7)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = 
                    SequenceIMG5.Visibility = 
                    SequenceIMG6.Visibility = 
                    SequenceIMG7.Visibility = Visibility.Visible;
            }
            if (maxIndex == 6)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility = 
                    SequenceIMG5.Visibility =
                    SequenceIMG6.Visibility =Visibility.Visible;
            }
            if (maxIndex == 5)
            {
                SequenceIMG1.Visibility = SequenceIMG2.Visibility = SequenceIMG3.Visibility =
                    SequenceIMG4.Visibility =
                    SequenceIMG5.Visibility = Visibility.Visible;
            }
            

            FailCount++;
            failed.Text = "Times FAILED: " + FailCount;
            correct = 0;
            
        }

        private void ReturnFeedback()
        {
            FeedbackTEXT.Visibility = Visibility.Visible;
            FeedbackTEXT.Text = "Excellent!";
            feedstory.Begin();
        }

        #endregion
	}
}
