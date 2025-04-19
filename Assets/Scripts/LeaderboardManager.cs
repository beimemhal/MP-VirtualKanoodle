using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardParent;
    [SerializeField] private Transform leaderboardContentParent;
    [SerializeField] private Transform leaderboardItemPrefab;

    private string leaderboardID = "VPG_leaderboard";

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, 0);

        leaderboardParent.SetActive(false); // TODO
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (leaderboardParent.activeInHierarchy)
            {
                leaderboardParent.SetActive(false); // TODO
            }
            else
            {
                leaderboardParent.SetActive(true); // TODO
                UpdateLeaderboard();

                try
                {
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, Timer.timeValue);
                }
                catch (LeaderboardsException e)
                {
                    Debug.Log(e.Reason);
                }
                // TODO playerScript.playerScore.Value = 0;

            }
        }
    }

    private async void UpdateLeaderboard()
    {
        while (Application.isPlaying && leaderboardParent.activeInHierarchy)
        {
            LeaderboardScoresPage leaderboardScoresPage = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID);

            foreach (Transform t in leaderboardContentParent)
            {
                Destroy(t.gameObject);
            }

            foreach (LeaderboardEntry entry in leaderboardScoresPage.Results) // entries written as name and score
            {
                Transform leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardContentParent);
                leaderboardItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.PlayerName;
                leaderboardItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.Score.ToString();
                
                // TODO difficulty
            }


            await Task.Delay(500);
        }
    }
}
