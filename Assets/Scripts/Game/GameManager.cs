using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Playing,
        GoodVictory,
        EvilVictory,
        GoodDefeat,
        EvilDefeat
    }

    public string GoodLayer;

    public string EvilLayer;

    public string LayerForAlignment(Alignment alignment)
    { return alignment == Alignment.Good ? GoodLayer : EvilLayer; }

    public int GoodScore
    { get { return UnitManager.Instance.GoodScore; } }

    public int EvilScore
    { get { return UnitManager.Instance.EvilScore; } }

    public GameState State 
    { get; private set; }

    void Update()
    {
        if (PlayerController.Instance.IsDead)
            Defeat();
        else if (UnitManager.Instance.Evil == 0)
            GoodVictory();
        else if (UnitManager.Instance.Good == 0)
            EvilVictory();

    }

    void Defeat()
    {
        if (State != GameState.Playing)
            return;

        if (PlayerController.Instance.Alignment == Alignment.Good)
            State = GameState.GoodDefeat;
        else
            State = GameState.EvilDefeat;
    }

    void GoodVictory()
    {
        if (State != GameState.Playing)
            return;

        State = GameState.GoodVictory;
    }

    void EvilVictory()
    {
        if (State != GameState.Playing)
            return;

        State = GameState.EvilVictory;
    }

}
