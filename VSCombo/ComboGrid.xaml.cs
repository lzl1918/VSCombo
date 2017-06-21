#define NO_PARTICLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSCombo.AnimationGroup;
using VSCombo.Model;
using VSCombo.ParticleEffect;

namespace VSCombo
{
    /// <summary>
    /// Interaction logic for ComboGrid.xaml
    /// </summary>
    public partial class ComboGrid : UserControl
    {
        private int count = 0;
        private IAnimationGroup championAnimation;
        private IAnimationGroup beatAnimation;
        private IAnimationGroup shrinkAnimation;
        private ExpBoundCalculator fontsizeCalc = new ExpBoundCalculator(20, 26);
        private List<Champion> champions;
        private List<ColorCalculator> colorCalcs;
        private Color baseColor;
        private SolidColorBrush countTextForeground;

#if !NO_PARTICLE
        private ParticleEmitter particleEmitter;
#endif

        public ComboGrid()
        {
            InitializeComponent();

            championAnimation = new FadeAndSlideAnimationGroup(10, 500, championContainer);
            beatAnimation = new BeatAnimationGroup(1.4, 100, countContainer);
            shrinkAnimation = new ShrinkAnimationGroup(1500, comboTimerIndicator);
            shrinkAnimation.Completed += ShrinkCompleted;
            champions = Champion.GetPredefinedChampions().ToList();

            countTextForeground = countText.Foreground as SolidColorBrush;
            baseColor = countTextForeground.Color;

            Color from;
            int begin = 0;
            from = baseColor;
            colorCalcs = new List<ColorCalculator>();
            foreach (Champion champion in champions)
            {
                colorCalcs.Add(new ColorCalculator(from, champion.KeyFrameColor, champion.TriggerCombo - begin, begin));
                from = champion.KeyFrameColor;
                begin = champion.TriggerCombo;
            }
#if !NO_PARTICLE
            particleEmitter = new ParticleEmitter();
            Loaded += OnLoaded;
            CompositionTarget.Rendering += OnCompositionTargetRendering;
#endif
        }

#if !NO_PARTICLE
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            particleEmitter.Center = new Point(particleCanvas.ActualWidth - 30, particleCanvas.ActualHeight / 2);
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            particleCanvas.Children.Clear();
            particleEmitter.Update();
            using (var dc = particleDrawingElement.visual.RenderOpen())
            {
                particleEmitter.particles.ForEach(p =>
                {
                    var c = p.Color;
                    c.A /= 3;
                    dc.DrawEllipse(
                        new SolidColorBrush(c),
                        null, p.Position, 7, 7);
                    dc.DrawEllipse(
                        new SolidColorBrush(p.Color),
                        null, p.Position, 2, 2);
                });
            }
            particleCanvas.Children.Add(particleDrawingElement);
        }
#endif

        private void ShrinkCompleted(object sender, EventArgs e)
        {
            count = 0;
        }

        public void AddCount()
        {
            count++;
            countText.Text = count.ToString();
            countText.FontSize = fontsizeCalc.Calculate(count);
            beatAnimation.Begin();
            shrinkAnimation.Begin();

            {
                int i = 0;
                for (i = 0; i < champions.Count; i++)
                {
                    if (champions[i].TriggerCombo > count) break;
                }
                if (i < colorCalcs.Count)
                {
                    ColorCalculator calc = colorCalcs[i];
                    Color color = calc.Calculate(count - calc.Base);
                    countTextForeground.Color = color;
                }
                else
                {
                    
                }
            }
            {
                Champion champ = champions.FirstOrDefault(x => x.TriggerCombo == count);
                if (champ != null)
                {
                    (championText.Foreground as SolidColorBrush).Color = countTextForeground.Color;
                    championText.Text = champ.Name;
                    championAnimation.Begin();
#if !NO_PARTICLE
                    particleEmitter.CreateParticle(50, countTextForeground.Color);
#endif
                }
            }
#if !NO_PARTICLE
            {
                if (count % 5 == 0)
                    particleEmitter.CreateParticle(50, countTextForeground.Color);
            }
#endif
        }
        public void NotifyDelay()
        {
            if (count > 0)
                shrinkAnimation.Begin();
        }

        public void SetLeft(double viewportRight)
        {
            Canvas.SetLeft(adornmentRoot, viewportRight - adornmentRoot.Width - 10);
        }

    }
}
