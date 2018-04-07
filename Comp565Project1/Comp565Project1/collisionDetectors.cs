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
    class CollisionDetectors : MovableModel3D
    {
        BoundingSphere boundingSphere;
        NPAgent parent;
        Object3D temp;
        int rotate = 0;

        public CollisionDetectors(Stage theStage, string label, string meshFile, Vector3 position, NPAgent inParent)  
	    : base(theStage, label, meshFile) 
	    {
            parent = inParent;
            temp = addObject(position + new Vector3(800f, 0f, 0f), Vector3.Up, 0f);
            stage.Collidable.Add(temp);
        }

        public override void Update(GameTime gameTime)
        {
            move(parent);
        }

        public void move(NPAgent position)
        {
            foreach (Object3D obj in instance)
            {
                rotate++;
                obj.Yaw = rotate * .01f;
                obj.Translation = position.AgentObject.Translation + new Vector3(800f, 0f, 0f);
            }
        }
    }
}
