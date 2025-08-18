using System;
using UnityEngine;

public class UpdateWrapper : MonoBehaviour
{
    public event Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
