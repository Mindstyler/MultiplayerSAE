using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

internal class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private LocalizedString _serverBrowserButtonText;
    [SerializeField]
    private LocalizedString _hostServerButtonText;

    private VisualElement _root;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        Button serverBrowserButton = _root.Q<Button>("server-browser-button");
        serverBrowserButton.clickable.clicked += static async () => await SceneLoader.LoadSceneAsync(1);
        _serverBrowserButtonText.StringChanged += localizedTest => serverBrowserButton.text = localizedTest;

        Button hostServerButton = _root.Q<Button>("host-server-button");
        hostServerButton.clickable.clicked += static async () => await SceneLoader.LoadSceneAsync(2);
        _hostServerButtonText.StringChanged += localizedText => hostServerButton.text = localizedText;
    }
}
