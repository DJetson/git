using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationEditor.Controls
{
    /// <summary>
    /// Interaction logic for PlaybackControl.xaml
    /// </summary>
    public partial class PlaybackControl : UserControl
    {
        public PlaybackControl()
        {
            InitializeComponent();
        }

        private void PlayPauseButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            if (Animation.IsPlaying)
                Animation.Pause();
            else
                Animation.Play(LoopButton.IsChecked == true);
        }

        private void StopButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.Stop();            
        }

        private void RewindButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.CurrentFrameIndex = 0;
        }

        private void StepBackButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.StepBackward();
        }

        private void StepForwardButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.StepForward();
        }

        private void FastForwardButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.CurrentFrameIndex = Animation.Frames.Count - 1;
        }

        private void ReverseButton_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void LoopButton_Click_1(object sender, RoutedEventArgs e)
        {
            AnimatedClip Animation = DataContext as AnimatedClip;
            Animation.Play((sender as ToggleButton).IsChecked == true);
        }
    }
}
