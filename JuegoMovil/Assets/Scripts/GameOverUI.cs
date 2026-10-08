using NaughtyAttributes;
using UnityEngine;

public class GameOverUI : UIWindow
{
    #region Test Methods

    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }


    #endregion
}
