#region Using Statements
using System;
using System.Collections.Generic;
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
    public struct TreasureNode
    {
        public float x;
        public float z;
        public bool isTagged;

        public TreasureNode(float x, float z, bool isTagged)
        {
            this.x = x;
            this.z = z;
            this.isTagged = isTagged;
        }
    }

    /// <summary>
    /// This class holds information regarding a lsit of treasure nodes. A treasure
    /// will be characterized by its location, and whether it has been taken or not.
    /// Once a treasure has been taken, it is first tagged, and then a visual marker
    /// should be placed to indicate such.
    /// </summary>
    /// 
    public class TreasureList : Model3D
    {
        //Variables
        private TreasureNode[] treasureNode;

        public TreasureList(Stage theStage, string label, string meshFile, bool isCollidable = false)
            : base(theStage, label, meshFile)
        {
            this.isCollidable = isCollidable;
            Random random = new Random();


            //List of treausre locations
            int[,] treasure = {
                                  { random.Next(384,512),random.Next(384, 512) },
                                  { random.Next(384,512),random.Next(384, 512) },
                                  { random.Next(384,512),random.Next(384, 512) },
                                  {447,453},
                              };

            //Create a list of treasures
            this.treasureNode = new TreasureNode[treasure.GetLength(0)];
            int x, z;

            for (int i = 0; i < treasure.GetLength(0); i++)
            {
                //Positions
                x = treasure[i, 0];
                z = treasure[i, 1];

                //Keep a list of these locations
                this.treasureNode[i].x = x;
                this.treasureNode[i].z = z;
                this.treasureNode[i].isTagged = false;

                //Add the treasure
                addObject(new Vector3(x * stage.Spacing, stage.Terrain.surfaceHeight(x, z), z * stage.Spacing),
                          new Vector3(0, 1, 0),
                          0.0f,
                          new Vector3(1, 1, 1));

            }


        }

        public TreasureNode[] getTreasureNode
        {
            get { return this.treasureNode; }
        }


        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.instance.Count; i++)
            {

                this.instance[i].Step = 0;

                //If the treasure is not tagged, spin it
                if (!this.treasureNode[i].isTagged)
                {
                    this.instance[i].Yaw = .01f;
                    this.instance[i].updateMovableObject();
                }
                else
                {
                    this.instance[i].Yaw = 0.0f;
                    this.instance[i].updateMovableObject();
                }

            }

            base.Update(gameTime);
        }

    }



}