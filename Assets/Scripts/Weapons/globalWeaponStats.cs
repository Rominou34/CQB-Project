using UnityEngine;
using System.Collections;

public class globalWeaponStats : MonoBehaviour {

    /*WEAPON NUMBERS
     * Hunting Shotgun = 0
     */

    //MAX AMMOS
    public static int[] weaponMaxAmmo = { 24 };

    //MAGAZINE SIZE
    public static int[] weaponMagSize = { 4 };

    //DAMAGE
    public static int[] weaponDamages = { 20 };

    //WEAPON NAME
    public static string[] weaponNames = { "Hunting Shotgun" };

    //SINGLE FIRE
    public static bool[] isSingleFire = { true };

    //RECOIL TIME
    public static float[] globalRecoilTime = { 1.5f };

    //RELOAD TIME
    public static float[] globalReloadTime = { 4f };
}
