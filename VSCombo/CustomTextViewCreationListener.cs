using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCombo
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class CustomTextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("ComboAdormentLayer")]
        [Order(After = PredefinedAdornmentLayers.CurrentLineHighlighter)]
        public AdornmentLayerDefinition comboLayer;
        public void TextViewCreated(IWpfTextView textView)
        {
            new ComboAdorment(textView);
        }
    }
}
