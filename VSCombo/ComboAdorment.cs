using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCombo
{
    public sealed class ComboAdorment
    {
        private IWpfTextView textView;
        private IAdornmentLayer adormentLayer;
        private ComboGrid comboGrid;
        public ComboAdorment(IWpfTextView textView)
        {
            if (textView == null)
                throw new ArgumentNullException(nameof(textView));

            this.textView = textView;
            adormentLayer = textView.GetAdornmentLayer("ComboAdormentLayer");
            comboGrid = new ComboGrid();
            adormentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, comboGrid, null);
            textView.TextBuffer.Changed += OnTextBufferChanged;
            textView.ViewportWidthChanged += OnViewportWidthChanged;
            comboGrid.SetLeft(textView.ViewportWidth);
        }

        private void OnViewportWidthChanged(object sender, EventArgs e)
        {
            double viewportWidth = (sender as IWpfTextView).ViewportWidth;
            comboGrid.SetLeft(viewportWidth);
        }

        private void OnTextBufferChanged(object sender, Microsoft.VisualStudio.Text.TextContentChangedEventArgs e)
        {
            int sum = e.Changes.Sum(change => change.Delta);
            if (sum <= 0)
                comboGrid.NotifyDelay();
            else
                comboGrid.AddCount();
        }
    }
}
