using System;
using UnityEngine;
using Zenject;
using TheSTAR.Data;
using TheSTAR.Sound;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameController : MonoBehaviour
{
    private DataController data;
    private GuiController gui;
    private SoundController sounds;
    private Player player;

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    public event Action BeforeAutoSaveGameEvent;

    [Inject]
    private void Construct(
        DataController data,
        SoundController sounds,
        GuiController gui,
        Player player)
    {
        this.data = data;
        this.sounds = sounds;
        this.gui = gui;
        this.player = player;
    }

    private void Start()
    {
        sounds.Play(MusicType.MainTheme);

        var loadScreen = gui.FindScreen<LoadScreen>();

        loadScreen.Init(() =>
        {
            if (gameConfig.Get.UseGDPR && !data.gameData.commonData.gdprAccepted)
            {
                var gdprScreen = gui.FindScreen<GDPRScreen>();
                gdprScreen.Init(() =>
                {
                    Debug.Log("On GDPR Accepted");
                    // MaxSdk.SetHasUserConsent(true);
                    data.gameData.commonData.gdprAccepted = true;
                    data.Save(DataSectionType.Common);

                    StartGame();
                });
                gui.Show(gdprScreen);
                return;
            }
            else StartGame();
        });

        gui.Show(loadScreen);
    }

    private void StartGame()
    {
        gui.ShowMainScreen();

        if (!data.gameData.commonData.gameStarted) data.gameData.commonData.gameStarted = true;
        else player.transform.position = data.gameData.commonData.playerPosition;
    }

    private void OnApplicationPause()
    {
        AutoSaveGame();
    }

    private void OnApplicationQuit()
    {
        AutoSaveGame();
    }

    private void AutoSaveGame()
    {
        Debug.Log("AutoSave");
        BeforeAutoSaveGameEvent?.Invoke();
        data.gameData.commonData.playerPosition = player.transform.position;
        data.SaveAll();
    }
}