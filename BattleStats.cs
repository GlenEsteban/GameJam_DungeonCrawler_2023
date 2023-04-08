using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStats : MonoBehaviour
{
    [SerializeField] int damageDealt = 5;
    [SerializeField] int recoveryAmount = 10;
    [SerializeField] int recoilDamage = 2;
    [SerializeField] int blocksTillRecovery = 5;
    public int GetRecoilDamage() {
        return recoilDamage;
    }
    public int GetDamageDealt() {
        return damageDealt;
    }
    public int GetRecoveryAmount() {
        return recoveryAmount;
    }
    public int GetBlocksTillRecovery() {
        return blocksTillRecovery;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
