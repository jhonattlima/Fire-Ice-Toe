﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Referência: Network Manager HUD Source Code
//             https://moodle.pucrs.br/mod/resource/view.php?id=1448787

public class OnlineGamesView : MonoBehaviour {

    // Template de botão para cada partida que foi encontrada.
    // Será instanciado na UI um para cada partida.
    public GameObject MatchButtonTemplate;

    // Objetos raiz das hierarquias de UI para cada um dos estados do processo de conexão.
    public GameObject ConnectRoot;
    public GameObject WaitingForPlayersRoot;
    public GameObject ConnectingRoot;
    public GameObject InGameRoot;

    // Período entre atualizações da lista de partidas.
    public float MatchmakingUpdatePeriod = 0.5f;

    // Variáveis privadas de controle.
    private string _matchName;
    private float _timeToUpdate;
    private bool _isConnected;

    // Número máximo de partidas suportadas.
    private const int _maxMatches = 10;

    // Botões de partidas que foram criados.
    private List<GameObject> _currentMatches = new List<GameObject>();

    /*
     * INICIALIZAÇÃO
     */

    private void Start()
    {
        // Reinicia o estado da UI para o inicial
        ConnectRoot.SetActive(true);
        WaitingForPlayersRoot.SetActive(false);
        InGameRoot.SetActive(false);
        ConnectingRoot.SetActive(false);

        // Cria um botão para cada uma das partidas suportadas.
        MatchButtonTemplate.SetActive(false);
        for (int i=0; i<_maxMatches; i++)
        {
            var buttonObject = Instantiate(MatchButtonTemplate, MatchButtonTemplate.transform.parent);
            _currentMatches.Add(buttonObject);

            var button = buttonObject.GetComponent<Button>();

            // Assina o evento de clique do botão, passando o índice correto para o handler.
            // Importante capturar o índice em uma variável separada para que o lambda faça
            // a captura do valor correto.
            int index = i;
            button.onClick.AddListener(() => OnMatchClicked(index));
        }

        // Assina o evento do NetworkManager que nos avisa sempre que
        // um client se conectar.
        MyNetworkManager.onServerConnect += OnServerConnect;
        MyNetworkManager.onClientConnect += OnClientConnect;

        // Inicializa o NetworkManager para utilizar o matchmaker.
        MyNetworkManager.singleton.StartMatchMaker();
    }

    private void OnClientConnect(NetworkConnection obj)
    {
        // Atualiza a UI para o estado "conectado".
        ConnectingRoot.SetActive(false);
        InGameRoot.SetActive(true);

        // Marca nosso estado interno como "conectados".
        _isConnected = true;
    }

    /*
     * CÓDIGO DO SERVIDOR
     */

    // Cria uma partida. Chamado pelo botão da UI respectivo.
    public void CreateMatch()
    {
        // Cria um nome aleatório para a nossa partida.
        _matchName = Random.Range(100000, 1000000).ToString();

        // Cria uma partida, passando o callback do NetworkManager.
        MyNetworkManager.Match.CreateMatch(_matchName, 2, true, "", "", "", 0, 0, 
            MyNetworkManager.singleton.OnMatchCreate);

        // Atualiza a UI para o estado "waiting for players".
        ConnectRoot.SetActive(false);
        WaitingForPlayersRoot.SetActive(true);
    }

    // Chamada pelo NetworkManager quando um cliente conecta, através da assinatura feita no Start().
    private void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Alguem conectou!");

        // Atualiza o estado da UI para "in game".
        WaitingForPlayersRoot.SetActive(false);
        InGameRoot.SetActive(true);

        // Neste momento seria enviado alguma mensagem de "iniciar partida",
        // carregamento de cena de gameplay etc.
    }

    /*
     * CÓDIGO DO CLIENTE
     */

    // Chamada quando um botão relacionado a uma partida é clicado.
    private void OnMatchClicked(int index)
    {
        // Acessa os dados correspondentes à partida mapeada pelo botão.
        var matchData = MyNetworkManager.singleton.matches[index];

        // Conecta com a partida, passando o callback do NetworkManager.
        MyNetworkManager.Match.JoinMatch(matchData.networkId, "", "", "", 0, 0, 
            MyNetworkManager.singleton.OnMatchJoined);

        // Atualiza a UI para o estado "conectando".
        ConnectRoot.SetActive(false);
        ConnectingRoot.SetActive(true);
    }

    private void Update()
    {
        if (!_isConnected)
        {
            // Atualiza o contador para fazer o Refresh das partidas atuais.
            _timeToUpdate -= Time.deltaTime;
            if (_timeToUpdate < 0)
            {
                // Chama o refresh de partidas.
                RefreshMatches();
                UpdateButtons();

                _timeToUpdate = MatchmakingUpdatePeriod;
            }
        }
    }

    // Atualiza as partidas conhecidas através do Network Match.
    private void RefreshMatches()
    {
        // Atualiza a lista interna de partidas, passando o callback do NetworkManager.
        MyNetworkManager.Match.ListMatches(0, _maxMatches, "", true, 0, 0, 
            MyNetworkManager.singleton.OnMatchList);
    }

    // Atualiza a lista de partidas nos botões, lendo da
    // lista interna de partidas do NetworkManager.
    private void UpdateButtons()
    {
        var matches = MyNetworkManager.singleton.matches;
        int i = 0;
        if (matches != null)
        {
            foreach (var match in matches)
            {
                if (i >= _maxMatches)
                {
                    break;
                }
                _currentMatches[i].SetActive(true);
                _currentMatches[i].GetComponentInChildren<Text>().text = "Join match: " + match.name;
                i++;
            }
        }
        for (; i < _currentMatches.Count; i++)
        {
            _currentMatches[i].SetActive(false);
        }
    }

}
