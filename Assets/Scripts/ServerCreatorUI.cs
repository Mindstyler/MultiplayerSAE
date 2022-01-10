using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
internal class ServerCreatorUI : MonoBehaviour
{
    private VisualElement _root;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        TextField portInput = _root.Q<TextField>("port-input-field");

        portInput.RegisterValueChangedCallback((keyPressed) =>
        {
            if (!ushort.TryParse(keyPressed.newValue, out _))
            {
                portInput.SetValueWithoutNotify(keyPressed.previousValue);
            }
        });

        _root.Q<Button>().clickable.clicked += () => ServerManager.Instance.HostNewServer(_root.Q<TextField>("server-name-input-field").value, ushort.Parse(portInput.value), (byte)_root.Q<SliderInt>().value);
    }
}
