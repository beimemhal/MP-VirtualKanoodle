using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Leaderboards;

public class LeaderboardsMenu : Panel
{

    [SerializeField] private int playersPerPage = 25;
    [SerializeField] private LeaderboardsPlayerItem playerItemPrefab = null;
    [SerializeField] private RectTransform playersContainer = null;
    [SerializeField] public TextMeshProUGUI pageText = null; // shows the current page and total page nr
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button prevButton = null;
    [SerializeField] private Button closeButton = null;
    // [SerializeField] private Button addScoreButton = null;

    public static string leaderboardID = "leaderboardEasy";


    private int currentPage = 1;
    private int totalPages = 0;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ClearPlayersList();
        closeButton.onClick.AddListener(ClosePanel);
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        // addScoreButton.onClick.AddListener(AddScore); // TODO delete and put in screen after win
        base.Initialize();
    }

    // new code:
    void Update()
    {
        if (ButtonFunct.updateLeaderboardEntry) // won and add entry button pressed
        {
            ButtonFunct.updateLeaderboardEntry = false;
            AddScoreAsync((int) (Timer.timeValue * 1000)); // TODO add score to leaderboard in format int (m)(m)(s)(s)(ms)(ms)(ms)
        }
    }
    
    public override void Open()
    {
        pageText.text = "-";
        nextButton.interactable = false;
        prevButton.interactable = false;
        base.Open();
        ClearPlayersList();
        currentPage = 1;
        totalPages = 0;
        LoadPlayers(1);
    }

    public async void AddScoreAsync(int score) // TODO call in screen after win with the achieved time
    {
        // ButtonFunct.updateLeaderboardEntry = false;
        Debug.Log("Score to be added: " + score);
        // addScoreButton.interactable = false;
        try
        {
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            LoadPlayers(currentPage);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        // addScoreButton.interactable = true;
    }

    private async void LoadPlayers(int page)
    {
        nextButton.interactable = false;
        prevButton.interactable = false;
        try
        {
            GetScoresOptions options = new GetScoresOptions();
            options.Offset = (page - 1) * playersPerPage;
            options.Limit = playersPerPage;
            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, options); // TODO error Leaderboard config could not be found -> leaderboardID not found
            ClearPlayersList();
            for (int i = 0; i < scores.Results.Count; i++)
            {
                LeaderboardsPlayerItem item = Instantiate(playerItemPrefab, playersContainer);
                item.Initialize(scores.Results[i]);
            }
            totalPages = Mathf.CeilToInt((float)scores.Total / (float)scores.Limit);
            currentPage = page;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        pageText.text = currentPage.ToString() + "/" + totalPages.ToString();
        nextButton.interactable = currentPage < totalPages && totalPages > 1;
        prevButton.interactable = currentPage > 1 && totalPages > 1;
    }

    private void NextPage()
    {
        if (currentPage + 1 > totalPages)
        {
            LoadPlayers(1);
        }
        else
        {
            LoadPlayers(currentPage + 1);
        }
    }

    private void PrevPage()
    {
        if (currentPage - 1 <= 0)
        {
            LoadPlayers(totalPages);
        }
        else
        {
            LoadPlayers(currentPage - 1);
        }
    }

    private void ClosePanel()
    {
        Close();
    }

    private void ClearPlayersList()
    {
        LeaderboardsPlayerItem[] items = playersContainer.GetComponentsInChildren<LeaderboardsPlayerItem>();
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
    }

}