#region Using Statements
using System;
using System.IO;  // needed for trace()'s fout
using System.Collections.Generic;
using System.Diagnostics; // For Debug
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AGMGSKv6;
#endregion

namespace AGMGSKv9
{
    class CollisionDetectors
    {
        BoundingSphere boundingSphere;

        public CollisionDetectors(Vector3 parentPosition)
        {
            boundingSphere = new BoundingSphere(parentPosition, 10f);
        }


    }
}
