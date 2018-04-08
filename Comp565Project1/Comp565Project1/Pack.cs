/*  
    Copyright (C) 2017 G. Michael Barnes
 
    The file Pack.cs is part of AGMGSKv9 a port and update of AGXNASKv8 from
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

using System.Diagnostics;
#endregion

namespace AGMGSKv9 {

/// <summary>
/// Pack represents a "flock" of MovableObject3D's Object3Ds.
/// Usually the "player" is the leader and is set in the Stage's LoadContent().
/// With no leader, determine a "virtual leader" from the flock's members.
/// Model3D's inherited List<Object3D> instance holds all members of the pack.
/// 
/// 2/1/2016 last changed
/// </summary>
public class Pack : MovableModel3D { 

    Object3D leader;
    KeyboardState currentKeyState;
    KeyboardState  prevKeyState;
    Inspector inspector;
    float leaderWeight = 0;


    /// <summary>
    /// Construct a pack with an Object3D leader
    /// </summary>
    /// <param name="theStage"> the scene </param>
    /// <param name="label"> name of pack</param>
    /// <param name="meshFile"> model of a pack instance</param>
    /// <param name="xPos, zPos">  approximate position of the pack </param>
    /// <param name="aLeader"> alpha dog can be used for flock center and alignment </param>
    public Pack(Stage theStage, string label, string meshFile, int nDogs, int xPos, int zPos, Object3D theLeader, Inspector theInspector)
      : base(theStage, label, meshFile) {
      isCollidable = true;
		random = new Random();
      leader = theLeader;
		int spacing = stage.Spacing;
      // initial vertex offset of dogs around (xPos, zPos)
      int [,] position = { {0, 0}, {32, -16}, {-28, -8}, {-28, 16}, {20, 8} };
		for( int i = 0; i < nDogs/*position.GetLength(0)*/; i++) {
        Debug.WriteLine(nDogs);
        int x = xPos + random.Next(-128, 128);

      int z = zPos + random.Next(-128, 128);

      float scale = (float)(0.5f + 0.5f * random.NextDouble());
			addObject( new Vector3(x * spacing, stage.surfaceHeight(x, z), z * spacing),
						  new Vector3(0, 1, 0), 0.0f,
						  new Vector3(scale, scale, scale));
			}

      for ( int i = 0; i < nDogs; i++ )
      { //randomize Dog positions
        Vector3 Rando = new Vector3((float)random.NextDouble() - 0.5f, 0, (float)random.NextDouble() - 0.5f);
        Rando.Normalize();
        instance[i].turnToFace(instance[i].Translation + Rando);
      }
      inspector = theInspector;

      currentKeyState = Keyboard.GetState();
      prevKeyState = currentKeyState;

      }

   /// <summary>
   /// Each pack member's orientation matrix will be updated.
   /// Distribution has pack of dogs moving randomly.  
   /// Supports leaderless and leader based "flocking" 
   /// </summary>      
   public override void Update(GameTime gameTime) {

      //Check buttons for changing leader weight and display in inspector
      prevKeyState = currentKeyState;
      currentKeyState = Keyboard.GetState();
      if ( currentKeyState.IsKeyDown(Keys.U) && !prevKeyState.IsKeyDown(Keys.U) )
      {
        if (leaderWeight >=1f) leaderWeight = 0f;
        else leaderWeight = Math.Min( 1f, leaderWeight + 0.33334f);
        Debug.WriteLine(leaderWeight);
      }
      inspector.setInfo(20, "Leader Weight "+ leaderWeight.ToString() );


      random = new Random();

      //For every member of the pack
      for (int i = 0; i < instance.Count; i++)
      {
        if (random.NextDouble() < 10f / 60) //every 10 frames do alter the direction
        {
          //Get current relevant positions and directions
          Vector3 Here = instance[i].Translation;
          Vector3 ToLeader = leader.Translation - Here;
          Vector3 Torwards = new Vector3(instance[i].Forward.X, instance[i].Forward.Y, instance[i].Forward.Z);
          //Normalize for correct weighting
          Torwards.Normalize();
          ToLeader.Normalize();

          int NearbyCount = 1;
          Vector3 AveragePosition = Here;
          Vector3 AverageAlignment = instance[i].Forward;

          for ( int j = 0; j < instance.Count; j++ )
          {
            //Find all members in radius, if they are add too average position and alignment
            Vector3 Other = instance[j].Translation;
            float Distance = Vector3.Distance(Here, Other);
            if ( Distance < 4000f )
            {
              AverageAlignment += instance[j].Forward;
              AveragePosition += Other;
              NearbyCount += 1;
            }      
          }
          //Check leader as well
          float DistanceLead = Vector3.Distance(Here, leader.Translation);
          if ( DistanceLead < 4000f )
          {
            AverageAlignment += leader.Forward;
            AveragePosition += leader.Translation;
            NearbyCount += 1;
          }

          AveragePosition /= (float)NearbyCount;
          AverageAlignment /= (float)NearbyCount;
          AverageAlignment.Normalize();

          Vector3 ToFlock = AveragePosition - Here;
          ToFlock.Normalize();
          Vector3 FromFlock = ToFlock * -1f;

          Vector3 Goal;
          //Goals
          //ToFlock is towards AvgPosition
          //AverageAlignment is self-evident
          //FromFlock is away from AvgPosition

          //Determine which direction to go
          float DistanceAvg = Vector3.Distance(Here, AveragePosition);
          if (NearbyCount > 2)
          {
            if (DistanceAvg < 1000f)
            {
              //Separation
              if ( DistanceAvg > 500f)
              {
                //Interpolate with Alignment
                float CloseWeight = (DistanceAvg - 500f) / 500f;
                Goal = (1f - CloseWeight) * FromFlock + (CloseWeight * AverageAlignment); 
              }
              else Goal = FromFlock;
            }
            if (DistanceAvg < 3000f)
            {
              //Allignment
              if ( DistanceAvg > 2000 )
              {
                //Interpolate with Cohesion
                float FarWeight = (DistanceAvg - 1000f) / 1000f;
                Goal = (1f - FarWeight) * AverageAlignment + (FarWeight * ToFlock);
              }
              else Goal = AverageAlignment;
            }
            //Cohesion
            else Goal = ToFlock;
          }
          //Keep going whatever direction
          else Goal = Torwards;

          //Leader Position weighted directly with goal
          Goal = Goal * (1.0f - leaderWeight) + ToLeader * leaderWeight;
          Goal.Normalize();

          Goal = Goal * 0.25f + instance[i].Forward * 0.75f;

          instance[i].turnToFace(Here +Goal);
        }

        instance[i].updateMovableObject();
        stage.setSurfaceHeight(instance[i]);
      }

        base.Update(gameTime);  // MovableMesh's Update(); 
      }


   public Object3D Leader {
      get { return leader; }
      set { leader = value; }}

   }
}
