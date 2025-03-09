using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameElement : MonoBehaviour
{
    [SerializeField] private readonly SampleModel sampleModelSO;
    [SerializeField] private SampleModel sampleModel;

    private void OnEnable()
    {
        sampleModel = ((ICloneable<SampleModel>)sampleModelSO).CloneSelf();
    }
    public void SampleBehavior()
    {
        //TO DO: Any thing
    }
}
