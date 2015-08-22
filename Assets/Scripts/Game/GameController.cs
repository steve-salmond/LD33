using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController>
{

    public string GoodLayer;

    public string EvilLayer;


    public string LayerForAlignment(Alignment alignment)
    {
        return alignment == Alignment.Good ? GoodLayer : EvilLayer;
    }
}
