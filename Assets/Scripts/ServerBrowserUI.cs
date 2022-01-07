using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
internal class ServerBrowserUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset _serverListElement;
    [SerializeField]
    private LocalizedString _serverNameHeader;
    [SerializeField]
    private LocalizedString _playerCountHeader;
    [SerializeField]
    private LocalizedString _connectButtonHeader;
    [SerializeField]
    private LocalizedString _connectButton;

    private VisualElement _root;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        ListView listView = _root.Q<ListView>();

        LocalizeServerBrowserUI();

        listView.makeItem = () =>
        {
            TemplateContainer listElement = _serverListElement.CloneTree();
            _connectButton.StringChanged += localizedText => listElement.Q<Button>("connect-button").text = localizedText;

            return listElement;
        };

        listView.bindItem = (element, index) =>
        {
            ServerInfo server = ((ObservableCollection<ServerInfo>)listView.itemsSource)[index];

            element.Q<Label>("server-name").text = server.Name;
            element.Q<Label>("player-count").text = $"{server.CurrentPlayer} / {server.MaxPlayers}";

            // TODO this is not an optimal implementation. probably
            Button button = element.Q<Button>();
            button.userData = (Action)(() => ServerManager.Instance.ConnectToServer(index));
            button.clickable.clicked += (Action)button.userData;
        };

        listView.unbindItem = static (element, _) =>
        {
            // TODO this is not an optimal implementation. probably
            Button button = element.Q<Button>();
            button.clickable.clicked -= (Action)button.userData;
        };

        listView.itemsSource = ServerManager.Instance.ServerList;

        ServerManager.Instance.ServerList.CollectionChanged += (_, _) => listView.RefreshItems();
    }

    private void LocalizeServerBrowserUI()
    {
        _serverNameHeader.StringChanged += localizedText => _root.Q<Label>("server-name-header").text = localizedText;
        _playerCountHeader.StringChanged += localizedText => _root.Q<Label>("player-count-header").text = localizedText;
        _connectButtonHeader.StringChanged += localizedText => _root.Q<Label>("connect-button-header").text = localizedText;
    }
}
