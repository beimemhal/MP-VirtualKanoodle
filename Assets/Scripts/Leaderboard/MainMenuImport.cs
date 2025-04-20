using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine.UI;

public class MainMenuImport : Panel
{

    [SerializeField] public TextMeshProUGUI nameText = null;
    [SerializeField] private Button logoutButton = null;
    [SerializeField] private Button leaderboardEasyButton = null;
    [SerializeField] private Button leaderboardMediumButton = null;
    [SerializeField] private Button leaderboardHardButton = null;
    [SerializeField] private Button leaderboardVeryHardButton = null;
    
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        logoutButton.onClick.AddListener(SignOut);
        leaderboardEasyButton.onClick.AddListener(LeaderboardEasy);
        leaderboardMediumButton.onClick.AddListener(LeaderboardMedium);
        leaderboardHardButton.onClick.AddListener(LeaderboardHard);
        leaderboardVeryHardButton.onClick.AddListener(LeaderboardVeryHard);
        base.Initialize();
    }
    
    public override void Open()
    {
        UpdatePlayerNameUI();
        base.Open();
    }
    
    private void SignOut()
    {
        MenuManager.Singleton.SignOut();
    }
    
    private void UpdatePlayerNameUI()
    {
        nameText.text = AuthenticationService.Instance.PlayerName;
    }
    
    private void LeaderboardEasy()
    {
        PanelManager.Open("leaderboardEasy");
    }

    private void LeaderboardMedium()
    {
        PanelManager.Open("leaderboardMedium");
    }

    private void LeaderboardHard()
    {
        PanelManager.Open("leaderboardHard");
    }

    private void LeaderboardVeryHard()
    {
        PanelManager.Open("leaderboardVeryHard");
    }

}