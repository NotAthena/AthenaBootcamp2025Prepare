
using UnityEngine;


[CreateAssetMenu(fileName = "Sample", menuName = "ScriptableObjects/Sample", order = 0)]
public class SampleModel : ScriptableObject, ICloneable<SampleModel>
{
    public string SampleData = "DEFAULT";
}
