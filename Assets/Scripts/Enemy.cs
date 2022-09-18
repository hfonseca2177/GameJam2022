using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : TargetableObject
{

    [SerializeField] private GameObject spell;

    public void CastSpell()
    {
        Instantiate(spell, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
