using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isGhost = false;
    public float damage;
    public enum ParentType { Player, Enemy};
    public ParentType parentType;
}