using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static Transform Player;
    void Awake()
    {
        Player = transform;
    }

}
