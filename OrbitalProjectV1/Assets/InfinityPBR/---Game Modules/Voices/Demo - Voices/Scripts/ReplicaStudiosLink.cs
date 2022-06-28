using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used in the demo scene to provide a button link to Replica Studios with my referral code, so we can both get 30 minutes of free credit.
/// https://replicastudios.com/account/signup?referral_code=Xe0Nwvbx
/// </summary>

public class ReplicaStudiosLink : MonoBehaviour
{
    public void VisitReplicaStudios() => Application.OpenURL("https://replicastudios.com/account/signup?referral_code=Xe0Nwvbx");
}
