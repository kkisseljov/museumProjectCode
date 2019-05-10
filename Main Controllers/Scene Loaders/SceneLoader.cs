using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneLoader
{
    string sceneName { get; }
    ILoaderData data { get; set; }
    List<LoadingProcces> processes { get; }
    IEnumerator StartLoadSequence();
}
