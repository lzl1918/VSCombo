using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace VSCombo.AnimationGroup
{
    public sealed class FadeAnimationGroup : IAnimationGroup
    {
        private Storyboard storyboard;

        public UIElement Target { get; }
        public double From { get; }
        public double To { get; }
        public TimeSpan Duration { get; }
        public bool AutoHide { get; }
        public event EventHandler Completed;
        public FadeAnimationGroup(double from, double to, int durationMilli, UIElement target, bool autoHide)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            From = from;
            To = to;
            Duration = TimeSpan.FromMilliseconds(durationMilli);
            Target = target;
            AutoHide = autoHide;
        }

        public void Begin()
        {
            if (storyboard != null)
                return;

            storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = From,
                To = To,
                Duration = Duration
            };
            Storyboard.SetTarget(animation, Target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            storyboard.Children.Add(animation);
            storyboard.Completed += OnCompleted;
            if (Target.Visibility != Visibility.Visible)
                Target.Visibility = Visibility.Visible;
            storyboard.Begin();
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            if (AutoHide)
                Target.Visibility = Visibility.Collapsed;
            Completed?.Invoke(this, null);
        }

        public void Cancel()
        {
            if (storyboard == null)
                return;

            storyboard.Stop();
            storyboard = null;
        }
    }

}
