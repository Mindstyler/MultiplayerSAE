using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
internal class ServerBrowserUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset _serverListElement;

    private VisualElement _root;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        ListView listView = _root.Q<ListView>();

        listView.makeItem = () => _serverListElement.CloneTree();

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
}
