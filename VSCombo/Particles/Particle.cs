using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VSCombo.ParticleEffect
{
    public sealed class DrawingVisualElement : FrameworkElement
    {
        public DrawingVisual visual;

        public DrawingVisualElement() { visual = new DrawingVisual(); }

        protected override int VisualChildrenCount { get { return 1; } }

        protected override Visual GetVisualChild(int index) { return visual; }
    }
    public sealed class ParticleEmitter
    {
        public Point Center { get; set; }
        public List<Particle> particles = new List<Particle>();
        Random rand = new Random();
        public WriteableBitmap TargetBitmap;
        public WriteableBitmap ParticleBitmap;

        public void CreateParticle(int count, Color particleColor)
        {
            while (count > 0)
            {
                count--;
                var speed = rand.Next(20) + 120;
                var angle = 2 * Math.PI * rand.NextDouble();

                particles.Add(
                    new Particle()
                    {
                        Position = new Point(Center.X, Center.Y),
                        Velocity = new Point(
                            Math.Sin(angle) * speed,
                            Math.Cos(angle) * speed),
                        Color = particleColor,
                        Lifespan = 0.5 + rand.Next(200) / 1000d
                    });
            }
        }

        public void Update()
        {
            var updateInterval = .005;

            particles.RemoveAll(p =>
            {
                p.Update(updateInterval);
                return p.Color.A == 0;
            });
        }
    }

    public sealed class Particle
    {
        public Point Position;
        public Point Velocity;
        public Color Color;
        public double Lifespan;
        public double Elapsed;

        public void Update(double elapsedSeconds)
        {
            Elapsed += elapsedSeconds;
            if (Elapsed > Lifespan)
            {
                Color.A = 0;
                return;
            }
            Color.A = (byte)(255 - ((255 * Elapsed)) / Lifespan);
            Position.X += Velocity.X * elapsedSeconds;
            Position.Y += Velocity.Y * elapsedSeconds;
        }
    }
}
