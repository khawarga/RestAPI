using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataList : MonoBehaviour
{
    public class Nutritions
    {
        public float carbohydrates;
        public float protein;
        public float fat;
        public float calories;
        public float sugar;
    }

    [Serializable]
    public class Fruit
    {
        public string genus;
        public string name;
        public int id;
        public string family;
        public string order;
        public Nutritions nutritions;
    }

    [Serializable]
    public class FruitList
    {
        public List<Fruit> Items;
    }
}
