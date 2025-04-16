using UnityEngine;

#region View
[System.Serializable]
public class DataView : DataViewBase
{

    public LayerMask occlusionlayers;

    [Header("InsideObject")]
    public bool InsideObject = false;

    [Header("InSight")]
    public bool InSight = false;
    Transform enemy;
    public DataView()
    { }

    public override bool IsInSight(Transform AimOffset)
    {
        this.InSight = false;
        enemy = null;
        if (AimOffset == null) return this.InSight;

        Vector3 origin = this.Owner.position;
        Vector3 dest = AimOffset.position;
        Vector3 direcction = dest - origin;

        if (direcction.magnitude > distance + 2)
            return InSight;



        if (dest.y < -(this.height + this.Owner.position.y) || dest.y > (this.height + this.Owner.position.y))
        {
            return this.InSight;
        }

        direcction.y = 0;
        float deltaAngle = Vector3.Angle(direcction.normalized, this.Owner.forward);

        if (deltaAngle > this.angle)
        {
            return this.InSight;
        }

        //if (this.IsDrawGizmo) 
        //  Debug.DrawLine(origin, dest, Color.red, 2f);

        if (this.InsideObject && Physics.Linecast(origin, dest, this.occlusionlayers))
        {
            return this.InSight;
        }


        enemy = AimOffset;

        this.InSight = true;
        return this.InSight;
    }
    public override void OnDrawGizmos()
    {
        if (!this.IsDrawGizmo) return;

        if (this.mesh != null && this.Owner != null)
        {
            if (this.InSight)
                Gizmos.color = this.meshSightIn;
            else
                Gizmos.color = this.meshSightOut;

            Gizmos.DrawMesh(this.mesh, this.Owner.position, this.Owner.rotation);

            Gizmos.DrawLine(this.Owner.position, this.Owner.position + this.Owner.forward * this.distance);
            Gizmos.DrawWireSphere(this.Owner.position + this.Owner.forward * this.distance, 1f);

        }
        //if (enemy != null)
        //{
        //    Gizmos.DrawLine(this.Owner.position, enemy.position);
        //}
    }
}
#endregion