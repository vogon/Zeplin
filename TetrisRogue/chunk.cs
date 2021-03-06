﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Microsoft.Xna.Framework;

namespace TetrisRogue
{
    class Chunk : GameObject
    {
        public Chunk()
        {
            _tiles = new DungeonTile[4, 4];
            OnDraw += Draw;
            OnUpdate += Update;
        }

        public void Draw(GameTime time)
        {
            foreach (DungeonTile d in _tiles)
                d.Draw(time);
        }

        public void Update(GameTime time)
        {
            //todo: put dirty flag here so we aren't doing 5million translations a second
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    this[x, y].Transformation.Position = this.Position + new Vector2(24 * x, -24 * y);
                }
            }
        }

        public Vector2 Position { get; set; }

        public DungeonTile this[int x, int y]
        {
            get
            {
                RotateCoordinates(ref x, ref y);
                return _tiles[x, y];
            }
            set 
            {
                RotateCoordinates(ref x, ref y);
                _tiles[x, y] = value;
            }
        }

        public void Rotate(Direction direction)
        {
            switch(direction)
            {
                case Direction.Clockwise:
                    this._rotation = (Rotation)(((int)_rotation + 1) % 4);
                    break;

                case Direction.Counterclockwise:
                    if (_rotation == Rotation.None) _rotation = Rotation.Cw270;
                    else _rotation = (Rotation)((int)_rotation - 1);
                    break;
            }
        }

        public void RotateCoordinates(ref int x, ref int y)
        {
            int newX, newY;
            //
            switch (_rotation)
            {
                case Rotation.Cw90:
                    newX = y;
                    newY = 3 - x;
                    break;

                case Rotation.Cw180:
                    newX = 3 - x;
                    newY = 3 - y;
                    break;

                case Rotation.Cw270:
                    newY = x;
                    newX = 3 - y;
                    break;
                    
                default:
                    newX = x;
                    newY = y;
                    break;
            }
            x = newX;
            y = newY;
        }

        public override string ToString()
        {
            String result = String.Empty;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    result += String.Format("{0},{1}\t", this[x, y].SubRect.X.ToString(), this[x, y].SubRect.Y.ToString());
                }
                result += "\n";
            }
            return result;
        }

        protected DungeonTile[,] _tiles;
        protected Rotation _rotation;
    }

    public enum Direction
    {
        Clockwise, Counterclockwise
    }

    public enum Rotation
    {
        None, Cw90, Cw180, Cw270
    }
}
