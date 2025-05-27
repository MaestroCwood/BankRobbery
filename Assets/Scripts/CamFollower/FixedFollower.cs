using UnityEngine;

public class FixedFollower : Follower
{

    private void FixedUpdate()
    {
        FollowMove(Time.fixedDeltaTime);
    }
}
