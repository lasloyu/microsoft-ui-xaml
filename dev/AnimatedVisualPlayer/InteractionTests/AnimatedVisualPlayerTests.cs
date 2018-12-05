// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

using AnimatedVisualPlayerTests;
using Common;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;

#if USING_TAEF
using WEX.TestExecution;
using WEX.TestExecution.Markup;
using WEX.Logging.Interop;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
#endif

#if BUILD_WINDOWS
using System.Windows.Automation;
using MS.Internal.Mita.Foundation;
using MS.Internal.Mita.Foundation.Controls;
using MS.Internal.Mita.Foundation.Waiters;
#else
using Microsoft.Windows.Apps.Test.Automation;
using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Microsoft.Windows.Apps.Test.Foundation.Patterns;
using Microsoft.Windows.Apps.Test.Foundation.Waiters;
#endif


// CatGates requires that test namespaces begin with "Windows.UI.Xaml.Tests",
// so we need to make sure that our test namespace begins with that to ensure that we get picked up.
namespace Windows.UI.Xaml.Tests.MUXControls.InteractionTests
{
    [TestClass]
    class AnimatedVisualPlayerTests
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "Integration")]
        [TestProperty("Platform", "Any")]
        [TestProperty("DEPControlsTestSuite", "SuiteB")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext);
        }

        public void TestCleanup()
        {
            TestCleanupHelper.Cleanup();
        }

        [TestMethod]
        public void AccessibilityTest()
        {
            using (var setup = new TestSetupHelper("AnimatedVisualPlayer Tests"))
            {
                var progressTextBox = FindElement.ByName<Edit>("ProgressTextBox");
                var isPlayingTextBoxBeforePlaying = FindElement.ByName<Edit>("IsPlayingTextBoxBeforePlaying");
                var isPlayingTextBoxBeingPlaying = FindElement.ByName<Edit>("IsPlayingTextBoxBeingPlaying");
                var playButton = FindElement.ByName<Button>("PlayButton");

                if (playButton != null &&
                    progressTextBox != null &&
                    isPlayingTextBoxBeforePlaying != null &&
                    isPlayingTextBoxBeingPlaying != null)
                {
                    using (var progressTextBoxWaiter = new PropertyChangedEventWaiter(progressTextBox, UIProperty.Get("Value.Value")))
                    {
                        playButton.Click();

                        Log.Comment("Waiting until AnimatedVisualPlayer playing ends.");
                        progressTextBoxWaiter.Wait();
                        Log.Comment("EventWaiter of progressTextBox is raised.");

                        Log.Comment("Value of isPlayingTextBoxBeforePlaying: \"{0}\".", isPlayingTextBoxBeforePlaying.Value);
                        Verify.AreEqual(Constants.FalseText, isPlayingTextBoxBeforePlaying.Value);

                        //
                        // isPlayingTextBoxBeingPlaying value is supposed to be updated
                        // inside the event handler function of Click for playButton in
                        // the UI test.
                        //
                        Log.Comment("Value of isPlayingTextBoxBeingPlaying: \"{0}\".", isPlayingTextBoxBeingPlaying.Value);
                        Verify.AreEqual(Constants.TrueText, isPlayingTextBoxBeingPlaying.Value);

                        Log.Comment("Value of progressTextBox: \"{0}\".", progressTextBox.Value);
                        Verify.AreEqual(Constants.PlayingEndedText, progressTextBox.Value);
                    }
                }
                else
                {
                    Verify.Fail("PlayButton or any other UIElement is not found.");
                }

                ToZeroKeyframeAnimationAccessibilityTest();
                FromOneKeyframeAnimationAccessibilityTest();
            }
        }

        private void ToZeroKeyframeAnimationAccessibilityTest()
        {
            var toZeroKeyframeAnimationProgressTextBox = FindElement.ByName<Edit>("ToZeroKeyframeAnimationProgressTextBox");
            var toZeroKeyframeAnimationPlayButton = FindElement.ByName<Button>("ToZeroKeyframeAnimationPlayButton");

            if (toZeroKeyframeAnimationPlayButton != null &&
                toZeroKeyframeAnimationProgressTextBox != null)
            {
                using (var toZeroAnimationTextBoxWaiter =
                    new PropertyChangedEventWaiter(toZeroKeyframeAnimationProgressTextBox, UIProperty.Get("Value.Value")))
                {
                    toZeroKeyframeAnimationPlayButton.Click();

                    Log.Comment("ToZeroKeyframeAnimationTextBoxWaiter: Waiting until AnimatedVisualPlayer playing ends.");
                    toZeroAnimationTextBoxWaiter.Wait();
                    Log.Comment("EventWaiter of toZeroKeyframeAnimationProgressTextBox is raised.");

                    Log.Comment("Value of toZeroKeyframeAnimationProgressTextBox: \"{0}\".", toZeroKeyframeAnimationProgressTextBox.Value);
                    Verify.AreEqual(Constants.PlayingEndedText, toZeroKeyframeAnimationProgressTextBox.Value);
                }
            }
            else
            {
                Verify.Fail("toZeroKeyframeAnimationPlayButton or any other UIElement is not found.");
            }
        }

        private void FromOneKeyframeAnimationAccessibilityTest()
        {
            var fromOneKeyframeAnimationProgressTextBox = FindElement.ByName<Edit>("FromOneKeyframeAnimationProgressTextBox");
            var fromOneKeyframeAnimationPlayButton = FindElement.ByName<Button>("FromOneKeyframeAnimationPlayButton");

            if (fromOneKeyframeAnimationPlayButton != null &&
                fromOneKeyframeAnimationProgressTextBox != null)
            {
                using (var fromOneAnimationTextBoxWaiter =
                    new PropertyChangedEventWaiter(fromOneKeyframeAnimationProgressTextBox, UIProperty.Get("Value.Value")))
                {
                    fromOneKeyframeAnimationPlayButton.Click();

                    Log.Comment("FromOneKeyframeAnimationTextBoxWaiter: Waiting until AnimatedVisualPlayer playing ends.");
                    fromOneAnimationTextBoxWaiter.Wait();
                    Log.Comment("EventWaiter of fromOneKeyframeAnimationProgressTextBox is raised.");

                    Log.Comment("Value of fromOneKeyframeAnimationProgressTextBox: \"{0}\".", fromOneKeyframeAnimationProgressTextBox.Value);
                    Verify.AreEqual(Constants.PlayingEndedText, fromOneKeyframeAnimationProgressTextBox.Value);
                }
            }
            else
            {
                Verify.Fail("fromOneKeyframeAnimationPlayButton or any other UIElement is not found.");
            }
        }
    }
}