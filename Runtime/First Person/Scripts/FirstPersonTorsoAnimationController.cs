using System;
using Chonker.Core;
using Chonker.Scripts.Core;
using Chonker.Taco_Simulator.Scripts.Player_Character.Torso_Control;
using UnityEngine;

public class FirstPersonTorsoAnimationController : AnimationLayerManager<TorsoAnimationStateId>
{
    private ModelManager modelManager;

    protected override TorsoAnimationStateId initialStateType => TorsoAnimationStateId.None;
    public override string animatorLayerName => "Torso Layer";

    protected override int getAnimationHash(TorsoAnimationStateId stateId) {
        switch (stateId) {
            case TorsoAnimationStateId.None:
                return Animator.StringToHash(animatorLayerName + ".None");
            case TorsoAnimationStateId.Sprint:
                return Animator.StringToHash(animatorLayerName + ".Sprint");
        }

        throw new NotImplementedException();
    }

    public void CrossfadeAnimationState(TorsoAnimationStateId TorsoAnimationStateId) {
        CrossFadeAnimationState(TorsoAnimationStateId);
    }
}
