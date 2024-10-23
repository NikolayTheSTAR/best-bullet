using System;
using UnityEngine;
using Zenject;
using TheSTAR.GUI;

/// <summary>
/// Биндим в контексте сцены Game
/// </summary>
public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private CameraController worldCameraPrefab;
    [SerializeField] private GameController gameControllerPrefab;
    [SerializeField] private Player playerPrefab;

    [Header("GUI")]
    [SerializeField] private GuiController guiControllerPrefab;
    [SerializeField] private GuiScreen[] screenPrefabs;
    [SerializeField] private GuiUniversalElement[] universalElementPrefabs;

    [Space]
    [SerializeField] private TutorialController tutorialControllerPrefab;
    [SerializeField] private FlyUIContainer flyUiContainerPrefab;

    private GuiController gui;
    private FlyUIContainer flyUI;

    public override void InstallBindings()
    {
        InstallGuiContainers();

        var player = Container.InstantiatePrefabForComponent<Player>(playerPrefab);
        Container.Bind<Player>().FromInstance(player).AsSingle();
        Container.Bind<ICameraFocusable>().FromInstance(player).AsSingle();

        var camera = Container.InstantiatePrefabForComponent<CameraController>(worldCameraPrefab);
        Container.Bind<CameraController>().FromInstance(camera).AsSingle();

        var gameController = Container.InstantiatePrefabForComponent<GameController>(gameControllerPrefab);
        Container.Bind<GameController>().FromInstance(gameController).AsSingle();

        // gui
        InstallGuiScreens();
    }

    private void InstallGuiContainers()
    {
        gui = Container.InstantiatePrefabForComponent<GuiController>(guiControllerPrefab, guiControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<GuiController>().FromInstance(gui).AsSingle();

        var tutor = Container.InstantiatePrefabForComponent<TutorialController>(tutorialControllerPrefab, tutorialControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<TutorialController>().FromInstance(tutor).AsSingle();

        flyUI = Container.InstantiatePrefabForComponent<FlyUIContainer>(flyUiContainerPrefab, tutorialControllerPrefab.transform.position, Quaternion.identity, null);
        Container.Bind<FlyUIContainer>().FromInstance(flyUI).AsSingle();
    }

    private void InstallGuiScreens()
    {
        GuiScreen[] createdScreens = new GuiScreen[screenPrefabs.Length];
        GuiScreen screen;
        for (int i = 0; i < screenPrefabs.Length; i++)
        {
            screen = Container.InstantiatePrefabForComponent<GuiScreen>(screenPrefabs[i], gui.ScreensContainer.position, Quaternion.identity, gui.ScreensContainer);
            createdScreens[i] = screen;
        }

        GuiUniversalElement[] createdUniversalElements = new GuiUniversalElement[universalElementPrefabs.Length];
        GuiUniversalElement ue;
        for (int i = 0; i < universalElementPrefabs.Length; i++)
        {
            ue = Container.InstantiatePrefabForComponent<GuiUniversalElement>(universalElementPrefabs[i], gui.UniversalElementsContainer.position, Quaternion.identity, gui.UniversalElementsContainer);
            createdUniversalElements[i] = ue;
        }

        gui.Set(createdScreens, createdUniversalElements);
    }

    [ContextMenu("Sort")]
    private void Sort()
    {
        Array.Sort(screenPrefabs);
        Array.Sort(universalElementPrefabs);
    }
}