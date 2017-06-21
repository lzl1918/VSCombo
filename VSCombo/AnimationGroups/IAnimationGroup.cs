using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCombo.AnimationGroup
{
    public interface IAnimationGroup
    {
        event EventHandler Completed;
        void Begin();
        void Cancel();
    }
}
