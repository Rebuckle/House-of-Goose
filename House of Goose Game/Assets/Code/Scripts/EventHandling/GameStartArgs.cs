using UnityEngine;
using System;
using System.Collections.Generic;

public class GameStartArgs : EventArgs
{
    public Player player;
    public Transform notebook;
    public Transform computer;
    public List<Transform> manillas;
    public Transform phone;
}
