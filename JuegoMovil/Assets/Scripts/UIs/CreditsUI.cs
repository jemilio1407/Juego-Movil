using NaughtyAttributes;
using UnityEngine;

public class CreditsUI : UIWindow
{

    public void OnCloseClicked()
    {
        Hide();
    }

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
