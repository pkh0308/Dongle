using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UI_Base
{

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _initialized = true;
        return true;
    }

    public void ClosePopUp()
    {
        Managers.UI.ClosePopUp(this);
    }
}