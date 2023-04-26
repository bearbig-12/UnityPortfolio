using System.Collections;
using System.Collections.Generic;
using StuartH;
using UnityEngine;

namespace StuartH
{
    /// <summary>
    ///CreditsUI - This class is responsible for displaying the credits screen
    /// </summary>
    public class CreditsUI : CanvasGroupBase
    {
        private void Awake() => Hide();
        public void Show() => ShowUI();
        public void Hide() => HideUI();

    }
}
