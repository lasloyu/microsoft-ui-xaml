// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

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
                const string strEnded = "Lottie player ended";
                var textBox = FindElement.ByName<Edit>("ProgressTextBox");
                var playButton = FindElement.ByName<Button>("PlayButton");

                if (playButton != null && textBox != null)
                {
                    Log.Comment(textBox.Value);
                    using (var propertyChangedEventWaiter = new PropertyChangedEventWaiter(textBox, UIProperty.Get("Value.Value")))
                    {
                        // Not click the play button until an event waiter is started for race contention issue.
                        // Where test might be too slow to catch the event change moment from UI lottie player running.
                        playButton.Click();
                        Log.Comment("Wait until lottie player ending.");
                        propertyChangedEventWaiter.Wait();
                        Log.Comment("Text property change event fired");
                        Verify.AreEqual(strEnded, textBox.Value);
                    }
                }
                else
                {
                    Verify.Fail("Either PlayButton or ProgresstextBox is not found.");
                }
            }
        }
    }
}