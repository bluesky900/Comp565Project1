#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AGMGSKv9;
#if MONOGAMES //  true, build for MonoGames
   using Microsoft.Xna.Framework.Storage; 
#endif
#endregion

namespace AGMGSKv6
{
  public class Sensor : Model3D
  {

    Object3D parent; // Object for sensors to follow and turn
    Inspector inspector; // For output (deprecated)

    public Sensor(Stage theStage, string label, string meshFile, bool isCollidable = false, Object3D theParent = null, Inspector theInspector = null)
            : base(theStage, label, meshFile)
    {
      this.isCollidable = isCollidable;
      inspector = theInspector;
      float scale = 1.6f;
      //Spawn four sensor spheres
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      parent = theParent;
    }

    public override void Update(GameTime gameTime)
    {
      //For better readability;
      Object3D leftSphere = instance[0];
      Object3D rightSphere = instance[1];
      Object3D leftleftSphere = instance[2];
      Object3D rightrightSphere = instance[3];
      //Position to place left & right sphere
      Vector3 Position = parent.Translation + parent.Forward * 400;
      leftSphere.Translation = Position + parent.Left * 150f;
      rightSphere.Translation = Position + parent.Left * -150f;
      //Position to place leftleft & rightright
      leftleftSphere.Translation = parent.Translation + parent.Forward * 100f + parent.Left * 280f;
      rightrightSphere.Translation = parent.Translation + parent.Forward * 100f + parent.Left * -280f;
      //Reset parent turn
      parent.Yaw = 0;
      //If collision detected turn parent in opposite direction
      if (leftSphere.collided)
      {
        if (rightSphere.collided) parent.Yaw += 0.2f; // if both spheres are colliding turn this by default
        else parent.Yaw -= 0.2f;

      }
      else if (rightSphere.collided) parent.Yaw += 0.2f;
      if (leftleftSphere.collided)
      {
        if (rightrightSphere.collided) parent.Yaw += 0.05f;// if both spheres are colliding turn this by default
        parent.Yaw -= 0.05f;
      }
      else if (rightrightSphere.collided) parent.Yaw += 0.05f;

      base.Update(gameTime);
    }


  }
}
