// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#pragma once

#include "pch.h"
#include "common.h"

#include "AnimatedVisualPlayer.g.h"
#include <winrt\Windows.UI.Xaml.Controls.h>

// Derive from DeriveFromPanelHelper_base so that we get access to Children collection
// in Panel. The Children collection holds the fallback content.
struct AnimatedVisualPlayer final 
    : ReferenceTracker<AnimatedVisualPlayer, DeriveFromPanelHelper_base, winrt::AnimatedVisualPlayer>
{
    AnimatedVisualPlayer();
    ~AnimatedVisualPlayer();
    winrt::IInspectable Diagnostics();
    winrt::TimeSpan Duration();
    winrt::IAnimatedVisualSource Source();
    void Source(winrt::IAnimatedVisualSource const& value);
    winrt::DataTemplate FallbackContent();
    void FallbackContent(winrt::DataTemplate const& value);
    bool AutoPlay();
    void AutoPlay(bool value);
    bool IsAnimatedVisualLoaded();
    bool IsPlaying();
    double PlaybackRate();
    void PlaybackRate(double value);
    winrt::Composition::CompositionObject ProgressObject();
    ::winrt::Windows::UI::Xaml::Media::Stretch Stretch();
    void Stretch(::winrt::Windows::UI::Xaml::Media::Stretch const& value);
    void Pause();
    winrt::IAsyncAction PlayAsync(double fromProgress, double toProgress, bool looped);
    void Resume();
    void SetProgress(double progress);
    void Stop();

    // FrameworkElement overrides
    winrt::Size MeasureOverride(winrt::Size const& availableSize);
    winrt::Size ArrangeOverride(winrt::Size const& finalSize);

    // IUIElement / IUIElementOverridesHelper
    winrt::AutomationPeer OnCreateAutomationPeer();

    static void ClearProperties();
    static void EnsureProperties();

    // Dependency properties
    static winrt::DependencyProperty AutoPlayProperty() { return s_AutoPlayProperty; }
    static winrt::DependencyProperty DiagnosticsProperty() { return s_DiagnosticsProperty; }
    static winrt::DependencyProperty DurationProperty() { return s_DurationProperty; }
    static winrt::DependencyProperty FallbackContentProperty() { return s_FallbackContentProperty; }
    static winrt::DependencyProperty IsAnimatedVisualLoadedProperty() { return s_IsAnimatedVisualLoadedProperty; }
    static winrt::DependencyProperty IsPlayingProperty() { return s_IsPlayingProperty; }
    static winrt::DependencyProperty PlaybackRateProperty() { return s_PlaybackRateProperty; }
    static winrt::DependencyProperty SourceProperty() { return s_SourceProperty; }
    static winrt::DependencyProperty StretchProperty() { return s_StretchProperty; }

private:
    //
    // An awaitable object that is completed when an animation play is completed.
    //
    struct AnimationPlay final : public Awaitable
    {
        AnimationPlay(
            AnimatedVisualPlayer& owner,
            float fromProgress,
            float toProgress,
            bool looped);

        float FromProgress();

        bool IsCurrentPlay();

        // Sets the playback rate of the animation.
        void SetPlaybackRate(float value);

        void Start();

        void Pause();

        void Resume();

        void OnHiding();
        void OnUnhiding();

        // Called to indicate that the play has been completed. Unblocks awaiters.
        void Complete();

    private:
        AnimatedVisualPlayer& m_owner;
        const float m_fromProgress{};
        const float m_toProgress{};
        const bool m_looped{};
        winrt::TimeSpan m_playDuration{};

        winrt::Composition::AnimationController m_controller{ nullptr };
        bool m_isPaused{ false };
        bool m_isPausedBecauseHidden{ false };
        winrt::event_token m_batchCompletedToken{0};
        winrt::Composition::CompositionScopedBatch m_batch{ nullptr };
    };

    void CompleteCurrentPlay();

    void Diagnostics(winrt::IInspectable const& value);
    void Duration(winrt::TimeSpan const& value);
    void IsAnimatedVisualLoaded(bool value);
    void IsPlaying(bool value);

    static void OnAutoPlayPropertyChanged(winrt::DependencyObject const& sender, winrt::DependencyPropertyChangedEventArgs const& args);
    void OnAutoPlayPropertyChanged(bool newValue);

    static void OnFallbackContentPropertyChanged(winrt::DependencyObject const& sender, winrt::DependencyPropertyChangedEventArgs const& args);
    void OnFallbackContentPropertyChanged();

    static void OnPlaybackRatePropertyChanged(winrt::DependencyObject const& sender, winrt::DependencyPropertyChangedEventArgs const& args);
    void OnPlaybackRatePropertyChanged(winrt::DependencyPropertyChangedEventArgs const& args);

    static void OnSourcePropertyChanged(winrt::DependencyObject const& sender, winrt::DependencyPropertyChangedEventArgs const& args);
    void OnSourcePropertyChanged(winrt::IAnimatedVisualSource const& oldSource, winrt::IAnimatedVisualSource const& newSource);

    static void OnStretchPropertyChanged(winrt::DependencyObject const& sender, winrt::DependencyPropertyChangedEventArgs const& args);

    void UpdateContent();
    void UnloadContent();

    void LoadFallbackContent();
    void UnloadFallbackContent();

    void SetFallbackContent(winrt::UIElement const& uiElement);

    void OnLoaded(winrt::IInspectable const& sender, winrt::RoutedEventArgs const& args);
    void OnUnloaded(winrt::IInspectable const& sender, winrt::RoutedEventArgs const& args);
    void OnHiding();
    void OnUnhiding();

    static GlobalDependencyProperty s_AutoPlayProperty;
    static GlobalDependencyProperty s_DiagnosticsProperty;
    static GlobalDependencyProperty s_DurationProperty;
    static GlobalDependencyProperty s_FallbackContentProperty;
    static GlobalDependencyProperty s_FromProgressProperty;
    static GlobalDependencyProperty s_IsAnimatedVisualLoadedProperty;
    static GlobalDependencyProperty s_IsPlayingProperty;
    static GlobalDependencyProperty s_PlaybackRateProperty;
    static GlobalDependencyProperty s_SourceProperty;
    static GlobalDependencyProperty s_StretchProperty;
    static GlobalDependencyProperty s_ToProgressProperty;

    //
    // Initialized by the constructor.
    //
    // A Visual used for clipping and for parenting of m_animatedVisualRoot.
    winrt::Composition::SpriteVisual m_rootVisual{ nullptr };
    // The property set that contains the Progress property that will be used to
    // set the progress of the animated visual.
    winrt::Composition::CompositionPropertySet m_progressPropertySet{ nullptr };
    // Revokers for events that we are subscribed to.
    winrt::Application::Suspending_revoker m_suspendingRevoker{};
    winrt::Application::Resuming_revoker m_resumingRevoker{};
    winrt::CoreWindow::VisibilityChanged_revoker m_visibilityChangedRevoker{};
    winrt::event_token m_loadedRevoker{};
    winrt::event_token m_unloadedRevoker{};

    //
    // Player mutable state state.
    //
    tracker_ref<winrt::IAnimatedVisual> m_animatedVisual{ this };
    // The native size of the current animated visual. Only valid if m_animatedVisual is not nullptr.
    winrt::float2 m_animatedVisualSize;
    winrt::Composition::Visual m_animatedVisualRoot{ nullptr };
    int m_playAsyncVersion{ 0 };
    double m_currentPlayFromProgress{ 0 };
    // The play that will be stopped when Stop() is called.
    std::shared_ptr<AnimationPlay> m_nowPlaying{ nullptr };
    winrt::event_token m_dynamicAnimatedVisualInvalidatedToken;

    // Set true if an animated visual has failed to load and set false the next time an animated
    // visual loads with non-null content. When this is true the fallback content (if any) will
    // be displayed.
    bool m_isFallenBack{ false };

    // Set true when FrameworkElement::Unloaded is fired, then set false when FrameworkElement::Loaded is fired.
    // This is used to differentiate the first Loaded event (when the element has never been
    // unloaded) from later Loaded events.
    bool m_isUnloaded{ false };
};

CppWinRTActivatableClassWithDPFactory(AnimatedVisualPlayer);
