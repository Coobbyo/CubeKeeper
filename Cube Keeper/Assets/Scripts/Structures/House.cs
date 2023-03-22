using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Structure
{
    public int Population { get; private set; }

    private void Start()
    {
        Population = 5;
    }
}
