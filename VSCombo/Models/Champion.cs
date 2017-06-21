using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VSCombo.Model
{
    [DebuggerDisplay("{TriggerCombo},{Name}")]
    public sealed class Champion
    {
        public int TriggerCombo { get; }
        public string Name { get; }
        public Color KeyFrameColor { get; }

        public Champion(int triggerCombo, string name, Color keyFrameColor)
        {
            TriggerCombo = triggerCombo;
            Name = name;
            KeyFrameColor = keyFrameColor;
        }

        public static IEnumerable<Champion> GetPredefinedChampions()
        {
            return new List<Champion>()
            {
                new Champion(7, "Whoah", Color.FromRgb(51, 119, 119)),
                new Champion(14, "Great", Color.FromRgb(85, 34, 102)),
                new Champion(20, "Brilliant!", Color.FromRgb(102, 34, 34)),
                new Champion(27, "Impressive!", Color.FromRgb(156, 119, 34)),
                new Champion(34, "Unbelievable!", Color.FromRgb(34, 34, 34))
            };
        }
    }
}
