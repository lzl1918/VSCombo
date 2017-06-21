using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VSCombo.AnimationGroup
{
    public sealed class BeatAnimationGroup : IAnimationGroup
    {
        public double ScaleMax { get; }
        public UIElement Target { get; }
        public TimeSpan Duration { get; }
        public event EventHandler Completed;

        private ScaleTransform scaleTransform;
        private Storyboard storyboard;

        public BeatAnimationGroup(double scaleMax, int durationMilli, UIElement target)
        {
            ScaleMax = scaleMax;
            Target = target;
            Duration = TimeSpan.FromMilliseconds(durationMilli);
            scaleTransform = target.RenderTransform as ScaleTransform;
            if (scaleTransform == null)
                throw new Exception("no ScaleTransform binded to target");
        }

        public void Begin()
        {
            double from = 1;
            if (storyboard != null)
            {
                storyboard.Stop();
                double tf = scaleTransform.ScaleX - 0.3;
                if (tf >= 1)
                    from = tf;
            }
            storyboard = new Storyboard();
            {
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = from,
                    To = ScaleMax,
                    AutoReverse = true,
                    Duration = Duration
                };
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(animation);
            }
            {
                DoubleAnimation animation = new DoubleAnimation()
                {
                    From = from,
                    To = ScaleMax,
                    AutoReverse = true,
                    Duration = Duration
                };
                Storyboard.SetTarget(animation, Target);
                Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(ScaleTransform.ScaleY)"));
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
            storyboard = null;
            Completed?.Invoke(this, null);
        }
    }

}
