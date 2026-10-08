using NaughtyAttributes;
using UnityEngine;

public class CreditsUI : UIWindow
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
