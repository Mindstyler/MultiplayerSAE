using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
internal class ServerCreatorUI : MonoBehaviour
{
    private VisualElement _root;

    [SerializeField]
    private LocalizedString _serverNameInputText;
    [SerializeField]
    private LocalizedString _portInputText;
    [SerializeField]
    private LocalizedString _createServerButtonText;
    [SerializeField]
    private LocalizedString _maxPlayersSliderText;
    [SerializeField]
    private LocalizedString _returnToMenuButtonText;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        PropertyInfo fillerTextProperty = typeof(TextField).GetProperty("text");

        TextField portInput = _root.Q<TextField>("port-input-field");
        portInput.RegisterValueChangedCallback((keyPressed) =>
        {
            if (!ushort.TryParse(keyPressed.newValue, out _))
            {
                portInput.SetValueWithoutNotify(keyPressed.previousValue);
            }
        });
        _portInputText.StringChanged += localizedText => fillerTextProperty.SetValue(portInput, localizedText);

        TextField serverNameInput = _root.Q<TextField>("server-name-input-field");
        _serverNameInputText.StringChanged += localizedText => fillerTextProperty.SetValue(serverNameInput, localizedText);

        SliderInt maxPlayersSlider = _root.Q<SliderInt>();
        _maxPlayersSliderText.StringChanged += localizedText => maxPlayersSlider.label = localizedText;

        Button createServerButton = _root.Q<Button>("create-server-button");
        _createServerButtonText.StringChanged += localizedText => createServerButton.text = localizedText;
        createServerButton.clickable.clicked += () => ServerManager.Instance.HostNewServer(serverNameInput.value, ushort.Parse(portInput.value), (byte)maxPlayersSlider.value);

        Button returnToMenuButton = _root.Q<Button>("return-to-main-menu-button");
        _returnToMenuButtonText.StringChanged += localizedText => returnToMenuButton.text = localizedText;
        returnToMenuButton.clickable.clicked += static async () => await SceneLoader.LoadSceneAsync(0);
    }
}
