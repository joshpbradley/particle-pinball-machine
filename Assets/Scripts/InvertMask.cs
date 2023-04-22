using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class InvertMask : Image
{
    public override Material materialForRendering {
        get
        {
            Material m = new Material(base.materialForRendering);
            m.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return m;
        }
    }
}
