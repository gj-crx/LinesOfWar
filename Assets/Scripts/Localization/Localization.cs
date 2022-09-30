using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public Vocabulary vocabulary;
    [HideInInspector]
    public static Localization Singleton;

    private void Awake()
    {
        Singleton = this;
    }
}
