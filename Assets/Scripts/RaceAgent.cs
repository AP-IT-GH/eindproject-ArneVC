using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgent : Agent
{
    public Transform Target;
    public Collider Target2;
    private bool touched = false;
    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is
        this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;

        // verplaats de target naar een nieuwe willekeurige locatie 
        Target.localPosition = new Vector3(Random.value * 8 - 4,0.5f,Random.value * 8 - 4);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target en Agent posities
        // sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
    }
    public float speedMultiplier = 0.1f;
    public float rotationMultiplier = 5;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        transform.Translate(controlSignal * speedMultiplier);

        transform.Rotate(0.0f, rotationMultiplier* actionBuffers.ContinuousActions[1], 0.0f);
        // Beloningen
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
    
        // target bereikt
        if (distanceToTarget < 1.42f)
        {
            AddReward(0.15f);
            touched = true;
            Target.localPosition = new Vector3(99,-99,99);
        }
        if (Target2.bounds.Contains(transform.position))
        {
            AddReward(0.1f);
            if (touched){AddReward(0.85f);};
            touched = false;
            EndEpisode();
        }
        // Van het platform gevallen?
        else if (this.transform.localPosition.y < 0)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
