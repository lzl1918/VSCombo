using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VSCombo.AnimationGroup
{
    public sealed class ShrinkAnimationGroup : IAnimationGroup
    {
        public TimeSpan Duration { get; }
        public UIElement Target { get; }
        public event EventHandler Completed;

        private ScaleTransform scaleTransform;
        private Storyboard storyboard;
        public ShrinkAnimationGroup(int durationMilli, UIElement target)
        {
            Target = target;
            Duration = TimeSpan.FromMilliseconds(durationMilli);
            scaleTransform = target.RenderTransform as ScaleTransform;
            if (scaleTransform == null)
                throw new Exception("no ScaleTransform binded to target");
        }

        public void Begin()
        {
            if (storyboard != null)
                storyboard.Stop();

            storyboard = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = Duration
            };
            Storyboard.SetTarget(animation, Target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(ScaleTransform.ScaleX)"));
            storyboard.Children.Add(animation);

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
            storyboard = null;
            Completed?.Invoke(this, null);
        }
    }

}
