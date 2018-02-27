/*  
    Copyright (C) 2017 G. Michael Barnes
 
    The file Wall.cs is part of AGMGSKv9 a port and update of AGXNASKv8 from
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
#endregion

namespace AGMGSKv9 {

    /// <summary>
    /// A collection of brick.x Models. 
    /// Used for path finding and obstacle avoidance algorithms.
    /// You set the brick positions in the brick[,] array in initWall(...).
    /// 
    /// 2/14/2016 last changed
    /// </summary>

    public class Treasure : Model3D {

        public Treasure(Stage theStage, string label, string meshFile, int nTreasures) : base(theStage, label, meshFile) {
            Random random = new Random();
            for(int i = 0; i < nTreasures; ++i) {
                int x = (96 + random.Next(256)*stage.Spacing);
                int z = (96 + random.Next(256)*stage.Spacing);
                if(i == 1) {
                    x = 67050;
                    z = 67950;
                }
                addObject(
                    new Vector3(x, stage.surfaceHeight(x, z), z),
                    new Vector3(0, 1, 0), 0.0f,
                    new Vector3(2.5f, 2.5f, 2.5f)
                    );
            }
        }
        /*
        public override void Update(GameTime gameTime) {
                
            base.Update(gameTime);
        }
        */
    }
}