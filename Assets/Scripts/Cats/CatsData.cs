using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatsData", menuName = "Scriptable Objects/CatsData")]
public class CatsData : ScriptableObject
{
    public List<Cat> cats;
}
