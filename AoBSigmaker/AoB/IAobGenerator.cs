using System.Collections.Generic;

namespace AoBSigmaker.AoB
{
    public interface IAobGenerator
    {
        string Make(IEnumerable<string> input);
    }
}
