using UnityEngine;

public interface IAttacker
{

    bool Attacking { get; }

    Transform Target { get; }

}
