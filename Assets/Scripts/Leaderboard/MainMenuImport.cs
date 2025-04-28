using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenuImport : Panel
{

    [SerializeField] public TextMeshProUGUI nameText = null;
    [SerializeField] private Button logoutButton = null;
    [SerializeField] private Button leaderboardEasyButton = null;
    [SerializeField] private Button leaderboardMediumButton = null;
    [SerializeField] private Button leaderboardHardButton = null;
    [SerializeField] private Button leaderboardVeryHardButton = null;
    [SerializeField] private Button leaderboardExtremeButton = null;
    [SerializeField] private Button backToMainMenuButton = null;

    public static string leaderboardID = "leaderboardEasy";
    
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
        leaderboardExtremeButton.onClick.AddListener(LeaderboardExtreme);
        backToMainMenuButton.onClick.AddListener(BTMM);
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
        leaderboardID = "leaderboardEasy";
        PanelManager.Open("leaderboardEasy");
    }

    private void LeaderboardMedium()
    {
        leaderboardID = "leaderboardMedium";
        PanelManager.Open("leaderboardMedium");
    }

    private void LeaderboardHard()
    {
        leaderboardID = "leaderboardHard";
        PanelManager.Open("leaderboardHard");
    }

    private void LeaderboardVeryHard()
    {
        leaderboardID = "leaderboardVeryHard";
        PanelManager.Open("leaderboardVeryHard");
    } 

    private void LeaderboardExtreme()
    {
        leaderboardID = "leaderboardExtreme";
        PanelManager.Open("leaderboardExtreme");
    } 
    
    private void BTMM() // short: only scene change without game reset -> from leaderboard to main menu scene
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
