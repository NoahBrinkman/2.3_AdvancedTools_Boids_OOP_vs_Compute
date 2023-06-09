using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOP
{
    public class Boid : MonoBehaviour
    {
        [HideInInspector] public int SwarmIndex;
        public float NoClumpingRadius;
        public float LocalAreaRadius;
        public float Speed;
        public float SteeringSpeed;

        
        [SerializeField, Range(0, 1)] private float seperationWeight = 1;
        [SerializeField, Range(0, 1)] private float alignmentWeight = 1;
        [SerializeField, Range(0, 1)] private float cohesionWeight = 1;
        public void SimulateMovement(List<Boid> other, float time)
        {
            //default vars
              var steering = Vector3.zero;

        var separationDirection = Vector3.zero;
        var separationCount = 0;
        var alignmentDirection = Vector3.zero;
        var alignmentCount = 0;
        var cohesionDirection = Vector3.zero;
        var cohesionCount = 0;

        var leaderBoid = other[0];
        var leaderAngle = 180f;

        foreach (Boid boid in other)
        {
            //skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour
            if (distance < NoClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

            //identify local neighbour
            if (distance < LocalAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boid.transform.position - transform.position;
                cohesionCount++;

                //identify leader
                var angle = Vector3.Angle(boid.transform.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        //flip
        separationDirection = -separationDirection;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        //get direction to center of mass
        cohesionDirection -= transform.position;

        //weighted rules
        steering += separationDirection.normalized;
        steering += alignmentDirection.normalized;
        steering += cohesionDirection.normalized;

        //local leader
        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized;

        //obstacle avoidance
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward *1.1f, out hitInfo, LocalAreaRadius, LayerMask.GetMask("Default")))
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;
        }
        
    }
}