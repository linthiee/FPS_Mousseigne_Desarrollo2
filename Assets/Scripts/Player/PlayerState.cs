using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void UpdateState();
    void Exit();
}

public class PlayerIdleState : IPlayerState
{
    private PlayerMovement player;
    
    private readonly int idleHash = Animator.StringToHash("idle");

    public PlayerIdleState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.anim.CrossFade(idleHash, 0.2f);
    }

    public void UpdateState()
    {
        if (player.MovementInput.magnitude > 0.1f) 
        {
            player.ChangeState(new PlayerWalkState(player));
        }
    }
    
    public void Exit()
    {
    }
}

public class PlayerWalkState : IPlayerState
{
    private PlayerMovement player;
    
    private readonly int walkHash = Animator.StringToHash("pistol_walk");

    public PlayerWalkState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.anim.CrossFade(walkHash, 0.2f); 
    }

    public void UpdateState()
    {
        if (player.MovementInput.magnitude <= 0.1f)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    public void Exit()
    {
    }
}