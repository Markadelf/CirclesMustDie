using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresVersusCircles
{

    abstract class GameObject
    {
        //Link to the room the object is in
        protected Room room;
        //Number of times we attempt to flush objects together
        private static int flushTimes = 3;

        //Always at center of the gameobject
        public Point Position { get; set; }
        public Point VelocityX { get; set; }
        public Point VelocityY { get; set; }
        public Point Acceleration { get; set; }

        //Tint of object
        public Color MyColor { get; set; }
        //Can this object be damaged
        public Boolean Damagable { get; set; }
        //Does this object move
        public Boolean IsMobile { get; set; }
        //Can this object push other objects
        public bool CanPush { get; set; }
        //How much health does this object have
        public int Health { get; set; }
        //Does this object damage others upon impact
        public bool Proj { get; set; }
        //Is this object in a state where it could jump?
        public bool CanJump { get; set; }
        //How long does this object have to wait before shooting again?
        public int ShotTimer { get; set; }

        /// <summary>
        /// Creates a game object, and ties it to the room
        /// </summary>
        /// <param name="x">X Position of Center</param>
        /// <param name="y">Y Position of Center</param>
        /// <param name="r">Link to the room it is in</param>
        public GameObject(int x, int y, Room r)
        {
            Position = new Point(x, y);
            this.room = r;
            VelocityX = new Point(0, 0);
            VelocityY = new Point(0, 0);
            Acceleration = new Point(0, 4);
            MyColor = Color.White;
            Damagable = false;
            IsMobile = false;
            CanPush = false;
            Health = 0;
            Proj = false;
            CanJump = false;
            ShotTimer = 0;
        }

        /// <summary>
        /// Moves the object in space
        /// </summary>
        virtual public void Update()
        {
            //Is it a movable object
            if (IsMobile)
            {
                //Update the velocity vectors
                VelocityX.X += Acceleration.X;
                VelocityY.Y += Acceleration.Y;
                //If it isn't a projectile
                if (!Proj)
                {
                    //Limit its speed
                    if (VelocityX.X > 10)
                    {
                        VelocityX.X = 10;
                    }
                    if (VelocityX.X < -10)
                    {
                        VelocityX.X = -10;
                    }
                    if (VelocityY.Y > 20)
                    {
                        VelocityY.Y = 20;
                    }
                    if (VelocityY.Y < -20)
                    {
                        VelocityY.Y = -20;
                    }
                }
                //Check for collisions if we move on the x axis
                bool collideX = false;
                for(int i = 0; i < room.Immobile.Count && !collideX; i++)
                {
                    if (this.CheckCollision(room.Immobile[i], VelocityX))
                    {
                        collideX = true;

                        if (this == room.Player)
                        {
                            if (room.Player.Position.Y - room.Player.Height / 2 + VelocityY.Y > room.Immobile[i].Position.Y + room.Immobile[i].Height / 2
                                || room.Player.Position.Y + room.Player.Height / 2 + VelocityY.Y < room.Immobile[i].Position.Y - room.Immobile[i].Height / 2)
                            {
                                collideX = false;
                            }
                            else
                            {
                                Flush(room.Immobile[i], VelocityX, flushTimes);
                                VelocityX.X = 0;
                            }
                        }
                        else
                        {
                            Flush(room.Immobile[i], VelocityX, flushTimes);
                            VelocityX.X = 0;
                        }
                        
                    }
                }
                for (int i = 0; i < room.Mobile.Count && !collideX; i++)
                {
                    if (this.CheckCollision(room.Mobile[i], VelocityX))
                    {
                        if (room.Mobile[i] != this)
                        {
                            Flush(room.Mobile[i], VelocityX, flushTimes);
                            collideX = true;
                        }
                        if (this == room.Player && room.Player.Position.Y - room.Player.Height / 2 + VelocityY.Y > room.Mobile[i].Position.Y + room.Mobile[i].Height / 2)
                        {
                            collideX = false;
                        }
                        else if (CanPush && room.Mobile[i].Push(VelocityX, flushTimes))
                        {
                            collideX = false;
                        }
                        else
                        {
                            VelocityX.X = 0;
                        }
                    }
                }
                if (this is Quad && !Proj && this != room.Player)
                {
                    for (int i = 0; i < room.Entity.Count && !collideX; i++)
                    {
                        if (this.CheckCollision(room.Entity[i], VelocityX))
                        {
                            collideX = true;
                            if (room.Entity[i].Push(VelocityX, flushTimes))
                            {
                                collideX = false;
                            }
                            else
                            {
                                room.ProjectileCircle.Add(new Circle(room.Entity[i].Position, new Point(3, -15), room));
                                room.ProjectileCircle[room.ProjectileCircle.Count - 1].Acceleration.Y = 4;
                                room.ProjectileCircle.Add(new Circle(room.Entity[i].Position, new Point(0, -15), room));
                                room.ProjectileCircle[room.ProjectileCircle.Count - 1].Acceleration.Y = 4;
                                room.ProjectileCircle.Add(new Circle(room.Entity[i].Position, new Point(-3, -15), room));
                                room.ProjectileCircle[room.ProjectileCircle.Count - 1].Acceleration.Y = 4;
                                room.Entity.RemoveAt(i);
                            }
                        }

                    }
                }
                if(this is Circle && !Proj)
                {
                    for (int i = 0; i < room.Entity.Count && !collideX; i++)
                    {
                        if (this != room.Entity[i] && this.CheckCollision(room.Entity[i], VelocityX))
                        {
                            collideX = true;
                        }
                    }
                }
                if(this != room.Player && !Proj && this is Quad)
                {
                    if (this.CheckCollision(room.Player, VelocityX))
                    {
                        collideX = false;
                        VelocityX.X = 0;
                    }
                }
                //If there were no collisions, Move
                if (!collideX)
                    Position += VelocityX;
                //Check for collisions in Y
                bool collideY = false;
                for (int i = 0; i < room.Immobile.Count && !collideY; i++)
                {
                    if (this.CheckCollision(room.Immobile[i], VelocityY))
                    {
                        collideY = true;
                        Flush(room.Immobile[i], VelocityY, flushTimes);
                        if (!CanJump && room.Immobile[i].Position.Y > Position.Y)
                            CanJump = true;

                    }
                }
                for (int i = 0; i < room.Mobile.Count && !collideY; i++)
                {
                    if (this.CheckCollision(room.Mobile[i], VelocityY))
                    {
                        if(room.Mobile[i] != this)
                        {
                            Flush(room.Mobile[i], VelocityY, flushTimes);
                            collideY = true;
                        }
                        if (!CanJump && room.Mobile[i].Position.Y > Position.Y)
                            CanJump = true;
                    }
                }
                if (this is Quad && !Proj && this != room.Player)
                {
                    for (int i = 0; i < room.Entity.Count && !collideY; i++)
                    {
                        if (this.CheckCollision(room.Entity[i], VelocityY))
                        {
                            collideX = true;
                            if (room.Entity[i].Push(VelocityY, flushTimes))
                            {
                                collideY = false;
                            }
                            else
                            {
                                room.Entity.RemoveAt(i);
                            }
                        }

                    }
                }
                if (this is Circle && !Proj)
                {
                    for (int i = 0; i < room.Entity.Count && !collideY; i++)
                    {
                        if (this != room.Entity[i] && this.CheckCollision(room.Entity[i], VelocityX))
                        {
                            collideY = true;
                        }
                    }
                }
                if (this != room.Player && !Proj && this is Quad)
                {
                    if (this.CheckCollision(room.Player, VelocityY))
                    {
                        collideY = false;
                        VelocityY.Y = 0;
                    }
                }
                //If there were no collisions, Move
                if (!collideY)
                    Position += VelocityY;
                else if(CanJump)
                {
                    VelocityY.Y = 0;
                }

            }
                
        }

        /// <summary>
        /// Attempt to move an object to another spot.
        /// </summary>
        /// <param name="vel">Vector for movement</param>
        /// <param name="count">Number of times left to try to flush any collisions</param>
        /// <returns>True if we moved it</returns>
        public bool Push(Point vel, int count)
        {

            if (IsMobile)
            {
                for (int i = 0; i < room.Immobile.Count; i++)
                {
                    if (this.CheckCollision(room.Immobile[i], vel))
                    {
                        Flush(room.Immobile[i], vel, count);
                        return false;
                    }
                }
                for (int i = 0; i < room.Mobile.Count; i++)
                {
                    if (this.CheckCollision(room.Mobile[i], vel))
                    {
                        if (room.Mobile[i] != this)
                        {
                            Flush(room.Mobile[i], vel, count);
                            return false;
                        }
                    }
                }
                Position += vel;
                return true;
            }
            return false;
        }
        //Abstract collision checks
        public abstract bool CheckCollision(Quad q);
        public abstract bool CheckCollision(Quad q, Point vel);
        public abstract bool CheckCollision(Circle c);
        public abstract bool CheckCollision(Circle c, Point vel);

        //Abstract, flush with rectangle
        public abstract void Flush(Quad q, Point v, int i);

        //Abstract draw
        public abstract void Draw(SpriteBatch sb);
    }
}
