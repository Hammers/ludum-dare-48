using System;
using UnityEngine;

public abstract class Ability : ScriptableObject, IEquatable<Ability>
{
    public Ability previous;
    public string abilityNameBase;
    public string abilityName;
    public string abilityDesc;
    public string upgradeDesc;
    public int cost;
    public Sprite icon;

    public override bool Equals(object obj) => this.Equals(obj as Ability);

    public bool Equals(Ability p)
    {
        if (p is null)
        {
            return false;
        }

        // Optimization for a common success case.
        if (object.ReferenceEquals(this, p))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != p.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (abilityName == p.abilityName) && (abilityDesc == p.abilityDesc) && (cost == p.cost);
    }

    public override int GetHashCode() => (abilityName, abilityDesc, cost).GetHashCode();

    public static bool operator ==(Ability lhs, Ability rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

    public static bool operator !=(Ability lhs, Ability rhs) => !(lhs == rhs);
}

public static class AbilityHelper
{
    public static bool IsUpgradeOf(this Ability ability, Ability otherAbility){
        Ability prevAbility = ability.previous;

        while(prevAbility != null){
            if(prevAbility == otherAbility)
                return true;
            prevAbility = prevAbility.previous;
        }
        return false;
    }
}
