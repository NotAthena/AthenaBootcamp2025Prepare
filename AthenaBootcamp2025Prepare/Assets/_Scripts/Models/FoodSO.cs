using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObjects/Food", order = 0)]
public class FoodSO : ScriptableObject
{
    [SerializeField] Food food;

    public Food Food { get => food;}
}