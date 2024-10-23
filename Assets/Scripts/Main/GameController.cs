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

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    [Inject]
    private void Construct(
        DataController data,
        SoundController sounds,
        GuiController gui)
    {
        this.data = data;
        this.sounds = sounds;
        this.gui = gui;
    }

    private void Start()
    {
        sounds.Play(MusicType.MainTheme);

        var loadScreen = gui.FindScreen<LoadScreen>();

        loadScreen.Init(() =>
        {
            Action showStartGameScreens = () =>
            {
                gui.ShowMainScreen();
            };

            if (gameConfig.Get.UseGDPR && !data.gameData.commonData.gdprAccepted)
            {
                var gdprScreen = gui.FindScreen<GDPRScreen>();
                gdprScreen.Init(() =>
                {
                    Debug.Log("On GDPR Accepted");
                    // MaxSdk.SetHasUserConsent(true);
                    data.gameData.commonData.gdprAccepted = true;
                    data.Save(DataSectionType.Common);

                    showStartGameScreens();
                });
                gui.Show(gdprScreen);
                return;
            }
            else
            {
                showStartGameScreens();
            }
        });

        gui.Show(loadScreen);
    }
}