using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SquaresVersusCircles
{
    enum AIType { None, Rain, Turret, Wall, Runner}
    class Circle : GameObject
    {
        private AIType ai = AIType.None;
        public int Radius { get; set; }
        public static Texture2D CircleTexture { get; set; }

        public Circle(int x, int y, int r, Room room) : base(x, y, room)
        {
            Radius = r;
            Health = Radius / 2;
        }

        public Circle(Char c, int x, int y, Room room): base(x, y, room)
        {
            switch (c)
            {
                case 'R':
                    Radius = 16;
                    MyColor = Color.Gray;
                    Damagable = true;
                    IsMobile = true;
                    CanPush = false;
                    Health = 10;
                    Proj = false;
                    ai = AIType.Rain;
                    Acceleration.Y = 0;
                    break;
                case 'T':
                    Radius = 16;
                    MyColor = Color.OrangeRed;
                    Damagable = true;
                    IsMobile = true;
                    CanPush = false;
                    Health = 10;
                    Proj = false;
                    ai = AIType.Turret;
                    Acceleration.Y = 0;
                    break;
                case 'V':
                    Radius = 16;
                    MyColor = Color.Khaki;
                    Damagable = true;
                    IsMobile = true;
                    CanPush = false;
                    Health = 10;
                    Proj = false;
                    ai = AIType.Wall;
                    Acceleration.Y = 0;
                    break;
                case 'N':
                    Radius = 16;
                    MyColor = Color.LightBlue;
                    Damagable = true;
                    IsMobile = true;
                    CanPush = false;
                    Health = 10;
                    Proj = false;
                    ai = AIType.Runner;
                    Acceleration.Y = 0;
                    break;
            }
        }

        public Circle(Point pos, Point vel, Room r): base(pos.X, pos.Y, r)
        {
            Proj = true;
            Radius = 8;
            MyColor = Color.Purple;
            Damagable = true;
            IsMobile = true;
            CanPush = false;
            Health = 12;
            VelocityX = new Point(vel.X, 0);
            VelocityY = new Point(0, vel.Y);
            Acceleration.Y = 0;
        }

        public override void Update()
        {
            if(room.Player.Health > 0)
            switch (ai)
            {
                case AIType.Rain:
                    if (Position.X > room.Player.Position.X)
                        VelocityX.X = -1;
                    else if (Position.X < room.Player.Position.X)
                        VelocityX.X = 1;
                    else
                        VelocityX.X = 0;
                    if(ShotTimer == 0)
                    {
                        if (room.Player.Position.Y > Position.Y)
                            room.ProjectileCircle.Add(new Circle(Position, new Point(0, 15), room));
                        else
                            room.ProjectileCircle.Add(new Circle(Position, new Point(-0, -15), room));
                        ShotTimer = 20;
                    }
                    else
                    {
                        ShotTimer--;
                    }
                    break;
                case AIType.Turret:
                    if (ShotTimer == 0)
                    {
                        room.ProjectileCircle.Add(new Circle(Position, (room.Player.Position - Position).ChangeMag(10), room));
                        ShotTimer = 10;
                    }
                    else
                    {
                        ShotTimer--;
                    }
                    break;
                case AIType.Runner:
                    Point p = new Point(room.Player.Position.X - Position.X, room.Player.Position.Y - Position.Y);
                    p = p.ChangeMag(4);
                    VelocityX.X = -p.X;
                    VelocityY.Y = -p.Y;
                    if (ShotTimer == 0)
                    {
                        room.ProjectileCircle.Add(new Circle(Position, (room.Player.Position - Position).ChangeMag(15), room));
                        ShotTimer = 30;
                    }
                    else
                    {
                        ShotTimer--;
                    }
                    break;
                case AIType.Wall:
                    if (Position.Y > room.Player.Position.Y)
                        VelocityY.Y = -1;
                    else if (Position.Y < room.Player.Position.Y)
                        VelocityY.Y = 1;
                    else
                        VelocityY.Y = 0;
                    if (ShotTimer == 0)
                    {
                        if(room.Player.Position.X > Position.X)
                            room.ProjectileCircle.Add(new Circle(Position, new Point(15, 0), room));
                        else
                            room.ProjectileCircle.Add(new Circle(Position, new Point(-15, 0), room));
                        ShotTimer = 20;
                    }
                    else
                    {
                        ShotTimer--;
                    }
                    break;
            }
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Damagable)
            {
                sb.Draw(CircleTexture, new Rectangle(Position.X - Radius + Room.Screen.X, Position.Y - Radius + Room.Screen.Y, Radius * 2, Radius * 2), new Color(MyColor, Health * 25));
            }
            else
                sb.Draw(CircleTexture, new Rectangle(Position.X - Radius + Room.Screen.X, Position.Y - Radius + Room.Screen.Y, Radius * 2, Radius * 2), MyColor);
        }

        public override bool CheckCollision(Quad q)
        {
            if (CheckCollision(new Point(q.Position.X - q.Width / 2, q.Position.Y - q.Height / 2)))
                return true;
            if (CheckCollision(new Point(q.Position.X - q.Width / 2, q.Position.Y + q.Height / 2)))
                return true;
            if (CheckCollision(new Point(q.Position.X + q.Width / 2, q.Position.Y - q.Height / 2)))
                return true;
            if (CheckCollision(new Point(q.Position.X + q.Width / 2, q.Position.Y + q.Height / 2)))
                return true;
            if(q.Position.X > Position.X)
            {
                if (q.CheckCollision(new Point(Position.X + Radius, Position.Y)))
                    return true;
            }
            else
            {
                if (q.CheckCollision(new Point(Position.X - Radius, Position.Y)))
                    return true;
            }
            if (q.Position.Y > Position.Y)
            {
                if (q.CheckCollision(new Point(Position.X, Position.Y + Radius)))
                    return true;
            }
            else
            {
                if (q.CheckCollision(new Point(Position.X, Position.Y - Radius)))
                    return true;
            }
            return false;
        }

        public override bool CheckCollision(Quad q, Point vel)
        {
            if (CheckCollision(new Point(q.Position.X - q.Width / 2, q.Position.Y - q.Height / 2), vel))
            {
                return true;
            }
            if (CheckCollision(new Point(q.Position.X - q.Width / 2, q.Position.Y + q.Height / 2), vel))
            {
                return true;
            }
            if (CheckCollision(new Point(q.Position.X + q.Width / 2, q.Position.Y - q.Height / 2), vel))
            {
                return true;
            }
            if (CheckCollision(new Point(q.Position.X + q.Width / 2, q.Position.Y + q.Height / 2), vel))
            {
                return true;
            }
            if (q.Position.X > Position.X + vel.X)
            {
                if (q.CheckCollision(new Point(Position.X + Radius + vel.X, Position.Y)))
                {
                    return true;
                }
            }
            else
            {
                if (q.CheckCollision(new Point(Position.X - Radius + vel.X, Position.Y)))
                {
                    return true;
                }
            }
            if (q.Position.Y > Position.Y + vel.Y)
            {
                if (q.CheckCollision(new Point(Position.X, Position.Y + Radius + vel.Y)))
                {
                    return true;
                }
            }
            else
            {
                if (q.CheckCollision(new Point(Position.X, Position.Y - Radius + vel.Y)))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckCollision(Point p)
        {
            return (Position - p).DistanceSquared() < Radius * Radius; 
        }
        public override bool CheckCollision(Circle c, Point vel)
        {
            return (Position + vel - c.Position).DistanceSquared() < (Radius + c.Radius) * (Radius + c.Radius);
        }
        public override bool CheckCollision(Circle c)
        {
            return (Position - c.Position).DistanceSquared() < (Radius + c.Radius) * (Radius + c.Radius);
        }
        private bool CheckCollision(Point p, Point vel)
        {
            return (Position + vel - p).DistanceSquared() < Radius * Radius;
        }

        public override void Flush(Quad q, Point vel, int i)
        {
            if(i > 0)
                Push(new Point(vel.X/2, vel.Y/2), i - 1);
        }
    }
}
