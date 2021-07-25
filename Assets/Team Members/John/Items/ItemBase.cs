using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace John
{
    [CreateAssetMenu(fileName ="New Item", menuName = "Items")]
    public class ItemBase : ScriptableObject
    {
        public string name;
        public GameObject ItemPrefab;
        
    }
}

