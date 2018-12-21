// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Markup;
using Windows.UI;
using System.Windows.Input;

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
            await LottiePlayer.PlayAsync(0, 1, false);
            ProgressTextBox.Text = "Lottie player ended";
        }
    }
}
