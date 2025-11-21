using Projectiles;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IProjectileFactory
    {
        Projectile Get(Transform towerParent);
    }
}