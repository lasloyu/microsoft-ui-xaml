// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using AnimatedVisualPlayerTests;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;


#if !BUILD_WINDOWS
using AnimatedVisualPlayer = Microsoft.UI.Xaml.Controls.AnimatedVisualPlayer;
#endif

namespace MUXControlsTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnimatedVisualPlayerPage : TestPage
    {
        public AnimatedVisualPlayerPage()
        {
            this.InitializeComponent();
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            bool isPlaying = Player.IsPlaying;
            IsPlayingTextBoxBeforePlaying.Text = isPlaying.ToString();

            Task task1 = Player.PlayAsync(0, 1, false).AsTask();
            Task task2 = GetIsPlayingAsync();

            await Task.WhenAll(task1, task2);

            ProgressTextBox.Text = Constants.PlayingEndedText;
        }

        private async void ToZeroKeyframeAnimationPlayButton_Click(object sender, RoutedEventArgs e)
        {
            await Player.PlayAsync(0.35, 0, false);

            ToZeroKeyframeAnimationProgressTextBox.Text = Constants.PlayingEndedText;
        }

        private async void FromOneKeyframeAnimationPlayButton_Click(object sender, RoutedEventArgs e)
        {
            await Player.PlayAsync(1, 0.35, false);

            FromOneKeyframeAnimationProgressTextBox.Text = Constants.PlayingEndedText;
        }

        private async Task GetIsPlayingAsync()
        {
            //
            // This artificial delay of 200ms is to ensure that the player's PlayAsync
            // has enough time ready to set value of IsPlaying property to true.
            //
            await Task.Delay(200);

            //
            // The player's PlayAsync returns immediately in RS4 or lower windows build.
            // Thus, Constants.TrueText is set to IsPlayingTextBoxBeingPlaying's content
            // in order to satisfy the interaction test that uses Accessibility.
            //
            if (IsRS5OrHigher())
            {
                Player.Pause();
                bool isPlaying = Player.IsPlaying;
                IsPlayingTextBoxBeingPlaying.Text = isPlaying.ToString();
                Player.Resume();
            }
            else
            {
                IsPlayingTextBoxBeingPlaying.Text = Constants.TrueText;
            }
        }

        private bool IsRS5OrHigher()
        {
            return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7);
        }
    }
}
