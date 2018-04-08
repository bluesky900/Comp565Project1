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

    Object3D parent;
    Inspector inspector;
    Object3D leftSphere;
    Object3D rightSphere;

    public Sensor(Stage theStage, string label, string meshFile, bool isCollidable = false, Object3D theParent = null,Inspector theInspector=null)
            : base(theStage, label, meshFile)
    {
        this.isCollidable = isCollidable;
      inspector = theInspector;
      float scale = 1.6f;
        addObject(theParent.Translation, theParent.Forward, 0f, new Vector3( scale,scale,scale) );
        addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      addObject(theParent.Translation, theParent.Forward, 0f, new Vector3(scale, scale, scale));
      parent = theParent;
    }

    public override void Update(GameTime gameTime)
    {
      leftSphere = instance[0];
      rightSphere = instance[1];
      Object3D leftleftSphere = instance[2];
      Object3D rightrightSphere = instance[3];
        Vector3 Position = parent.Translation + parent.Forward*400;
        leftSphere.Translation = Position + parent.Left * 150f;
        rightSphere.Translation = Position + parent.Left * -150f;
      leftleftSphere.Translation = parent.Translation + parent.Forward * 100f + parent.Left* 280f;
      rightrightSphere.Translation = parent.Translation + parent.Forward * 100f + parent.Left * -280f;
      string output = "NOT COLLIDING";
        parent.Yaw = 0;
        if (leftSphere.collided)
        {
          if ( rightSphere.collided) parent.Yaw += 0.2f;
          else parent.Yaw -= 0.2f;

        }
        else if (rightSphere.collided) parent.Yaw += 0.2f;
      if (leftleftSphere.collided) parent.Yaw -= 0.05f;
      if (rightrightSphere.collided) parent.Yaw += 0.05f;
      inspector.setInfo(21, output); 

      base.Update(gameTime);
    }


  }
}
