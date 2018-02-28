/*  
    Copyright (C) 2017 G. Michael Barnes
 
    The file Player.cs is part of AGMGSKv9 a port and update of AGXNASKv8 from
    MonoGames 3.5 to MonoGames 3.6  

    AGMGSKv9 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


#region Using Statements
using System;
using System.IO;  // needed for trace()'s fout
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AGMGSKv6;
#endregion

namespace AGMGSKv9
{

    /// <summary>
    /// Represents the user / player interacting with the stage. 
    /// The Update(Gametime) handles both user keyboard and gamepad controller input.
    /// If there is a gamepad attached the keyboard inputs are not processed.
    /// 
    /// removed game controller code from Update()
    /// 
    /// 2/8/2014 last changed
    /// </summary>

    public class Player : Agent
    {
        private KeyboardState oldKeyboardState;
        private int rotate;
        private float angle;
        private Matrix initialOrientation;

        private TreasureList treasures;
        private int treasureCount;

        public int treasure_count
        {
            get { return this.treasureCount; }
            set { this.treasureCount = value; }
        }

        public Player(Stage theStage, string label, Vector3 pos, Vector3 orientAxis,
        float radians, TreasureList tl, string meshFile)
        : base(theStage, label, pos, orientAxis, radians, meshFile)
        {  // change names for on-screen display of current camera
            first.Name = "First";
            follow.Name = "Follow";
            above.Name = "Above";
            IsCollidable = true;  // players test collision with Collidable set.
            stage.Collidable.Add(agentObject);  // player's agentObject can be collided with by others.
            rotate = 0;
            angle = 0.01f;
            initialOrientation = agentObject.Orientation;

            this.treasures = tl;
            this.treasureCount = 0;
        }

        /// <summary>
        /// Handle player input that affects the player.
        /// See Stage.Update(...) for handling user input that affects
        /// how the stage is rendered.
        /// First check if gamepad is connected, if true use gamepad
        /// otherwise assume and use keyboard.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.R) && !oldKeyboardState.IsKeyDown(Keys.R))
                agentObject.Orientation = initialOrientation;
            // allow more than one keyboardState to be pressed
            if (keyboardState.IsKeyDown(Keys.Up)) agentObject.Step++;
            if (keyboardState.IsKeyDown(Keys.Down)) agentObject.Step--;
            if (keyboardState.IsKeyDown(Keys.Left)) rotate++;
            if (keyboardState.IsKeyDown(Keys.Right)) rotate--;
            oldKeyboardState = keyboardState;    // Update saved state.
            agentObject.Yaw = rotate * angle;
            base.Update(gameTime);
            rotate = agentObject.Step = 0;

            //Treasure Collision: Have we come within 300 pixels of an untagged treasure?
            int thisPosX = (int)this.instance[0].Translation.X;
            int thisPosZ = (int)this.instance[0].Translation.Z;
            float distance;

            for (int i = 0; i < treasures.Instance.Count; i++)
            {
                int x, z;

                // If treasure is tagged ignore it and iterate
                if (this.treasures.getTreasureNode[i].isTagged)
                {
                    continue;
                }

                // Get Position from treasure and calculate distance
                x = (int)this.treasures.getTreasureNode[i].x * this.stage.Terrain.Spacing;
                z = (int)this.treasures.getTreasureNode[i].z * this.stage.Terrain.Spacing;
                distance = Vector2.Distance(new Vector2(x, z), new Vector2(thisPosX, thisPosZ));

                // Tag treasure if close enough
                if (distance < (this.stage.Terrain.Spacing * 2))
                { 
                    this.treasureCount += 1;
                    this.treasures.getTreasureNode[i].isTagged = true;
                }
            }
        }
    }
}
