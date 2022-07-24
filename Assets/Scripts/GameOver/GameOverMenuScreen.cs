using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuScreen : AMenu<GameOverMenuScreenType>
{
    void Start() { }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            menuGroup.ChangeCurrentMenu(GameOverMenuScreenType.MainMenu);
        }
    }
}
