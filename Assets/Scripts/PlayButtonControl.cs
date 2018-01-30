using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonControl : ShootableUI
{
    public override void ShotClick()
    {
        worldSpaceVideo.PlayPause();
    }
}
