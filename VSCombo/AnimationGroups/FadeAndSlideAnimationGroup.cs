using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VSCombo.AnimationGroup
{
    public sealed class FadeAndSlideAnimationGroup : IAnimationGroup
    {
        public double OffsetYMax { get; }
        public UIElement Target { get; }
        public TimeSpan Duration { get; }
        public event EventHandler Completed;

        private TranslateTransform translateTransform;
        private Storyboard storyboard;
        public FadeAndSlideAnimationGroup(double offsetYMax, int durationMilli, UIElement target)
        {
            OffsetYMax = offsetYMax;
            Target = target;
            Duration = TimeSpan.FromMilliseconds(durationMilli);
            translateTransform = target.RenderTransform as TranslateTransform;
            if (translateTransform == null)
                throw new Exception("no TranslateTransform binded to target");
        }

        public void Begin()
        {
            if (storyboard != null)
                return;

            storyboard = new Storyboard();
            {
                if (Target.Visibility != Visibility.Visible)
                    Target.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = Duration
                };
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
                storyboard.Children.Add(animation);
            }
            {
                translateTransform.Y = 0;
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = 0,
                    To = OffsetYMax,
                    Duration = Duration
                };
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));
                storyboard.Children.Add(animation);
            }
            storyboard.Completed += OnCompleted;
            storyboard.Begin();
        }
        public void Cancel()
        {
            if (storyboard == null)
                return;

            storyboard.Stop();
            storyboard = null;
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            Target.Visibility = Visibility.Collapsed;
            storyboard = null;
            Completed?.Invoke(this, null);
        }
    }

}
