﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : BoltSingletonPrefab<GameState>
{
    public enum State
    {
        mainMenu,
        mainSettings,
        mainControls,
        playMenu,
        matchmaking,
        gameplay,
        pauseMenu,
        pauseSettings,
        pauseControls
    }

    State m_currentState;
    public State CurrentState
    {
        get
        {
            return m_currentState;
        }

        set
        {
            if (value == State.gameplay)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible   = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible   = true;
            }

            m_currentState = value;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandleStateExit();
    }

    public void HandleStateExit()
    {
        switch (CurrentState)
        {
            case State.mainSettings:
                TransitionToMainMenu();
                break;

            case State.mainControls:
                TransitionToMainMenu();
                break;

            case State.playMenu:
                TransitionToMainMenu();
                break;

            case State.matchmaking:
                TransitionToMainMenu();
                break;

            case State.gameplay:
                if (!GameUI.instance.IsScoreboardOpen())
                    TransitionToPauseMenu();
                break;

            case State.pauseMenu:
                TransitionToGameplay();
                break;

            case State.pauseSettings:
                TransitionToPauseMenu();
                break;

            case State.pauseControls:
                TransitionToPauseMenu();
                break;
        }
    }

    public void TransitionToMainMenu()
    {
        if (CurrentState == State.mainSettings || CurrentState == State.mainControls || CurrentState == State.playMenu)
        {
            MainMenu menu = FindObjectOfType<MainMenu>();
            menu.ShowMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");

            if (BoltNetwork.IsRunning)
                BoltNetwork.Shutdown();
        }

        CurrentState = State.mainMenu;
    }

    public void TransitionToPauseMenu()
    {
        if (CurrentState == State.pauseSettings)
            SettingsPopup.instance.Hide();
        else if (CurrentState == State.pauseControls)
            ControlsPopup.instance.Hide();
        else if (CurrentState == State.gameplay)
        {
            PlayerMainCamera playerCam = FindObjectOfType<PlayerMainCamera>();
            if (playerCam)
            {
                GameObject  playerObject = MyHelper.FindFirstParentWithComponent(playerCam.gameObject, typeof(PlayerMotor));
                PlayerMotor playerMotor  = playerObject.GetComponent<PlayerMotor>();

                playerMotor.IsInputDisabled = true;
            }
        }

        PausePopup.instance.Show();

        CurrentState = State.pauseMenu;
    }

    public void TransitionToGameplay()
    {
        if (CurrentState == State.pauseMenu)
        {
            PausePopup.instance.Hide();

            PlayerMainCamera playerCam = FindObjectOfType<PlayerMainCamera>();
            if (playerCam)
            {
                GameObject playerObject = MyHelper.FindFirstParentWithComponent(playerCam.gameObject, typeof(PlayerMotor));
                PlayerMotor playerMotor = playerObject.GetComponent<PlayerMotor>();

                playerMotor.IsInputDisabled = false;
            }
        }
        else if (CurrentState == State.pauseControls)
        {
            ControlsPopup.instance.Hide();
        }
        else if (CurrentState == State.pauseSettings)
        {
            PausePopup.instance.Hide();
        }

        CurrentState = State.gameplay;
    }
}