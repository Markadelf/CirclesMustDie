using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SquaresVersusCircles
{


    class Quad : GameObject
    {
        public static Texture2D SquareTexture { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        
        public Quad(int x, int y, Room room): base(x, y, room)
        {

        }

        public Quad(int x, int y, int height, int width, Room room) : base(x, y, room)
        {
            Height = height;
            Width = width;
            Health = width / 2;
            IsMobile = false;
        }

        public Quad(Char c, int x, int y, Room room): base(x, y, room)
        {
            switch (c)
            {
                case 'P':
                    Width = 24;
                    Height = 24;
                    MyColor = Color.Blue;
                    Damagable = true;
                    IsMobile = true;
                    CanPush = true;
                    Health = 20;
                    Proj = false;
                    break;
                case 'W':
                    Width = 32;
                    Height = 32;
                    MyColor = Color.Brown;
                    Damagable = false;
                    IsMobile = false;
                    CanPush = false;
                    Health = 16;
                    Proj = false;
                    break;
                case 'B':
                    Width = 32;
                    Height = 32;
                    MyColor = Color.Yellow;
                    Damagable = false;
                    IsMobile = true;
                    CanPush = false;
                    Health = 16;
                    Proj = false;
                    break;
            }
        }

        public Quad(Point pos, Point vel, Room r): base(pos.X, pos.Y, r)
        {
            Proj = true;
            Width = 10;
            Height = 10;
            MyColor = Color.Green;
            Damagable = true;
            IsMobile = true;
            CanPush = true;
            Health = 10;
            VelocityX = new Point(vel.X, 0);
            VelocityY = new Point(0, vel.Y);
            Acceleration.Y = 0;
        }

        public override void Update()
        {
            base.Update();

        }

        public override void Draw(SpriteBatch sb)
        {
            if (Damagable)
            {
                sb.Draw(SquareTexture, new Rectangle(Position.X - Width / 2 + Room.Screen.X, Position.Y - Height / 2 + Room.Screen.Y, Width, Height), new Color(MyColor, Health * 10));
            }
            else
                sb.Draw(SquareTexture, new Rectangle(Position.X - Width / 2 + Room.Screen.X, Position.Y - Height / 2 + Room.Screen.Y, Width, Height), MyColor);
        }

        //Checks for collision
        public override bool CheckCollision(Quad q)
        {
            bool collideX = false;
            bool collideY = false;

            if (Position.X < q.Position.X)
            {
                if(Position.X + Width /2 > q.Position.X - q.Width /2)
                {
                    collideX = true;
                }
            }
            else if (Position.X - Width / 2 < q.Position.X + q.Width / 2)
            {
                collideX = true;
            }
            if (Position.Y < q.Position.Y)
            {
                if (Position.Y + Height / 2 > q.Position.Y - q.Height / 2)
                {
                    collideY = true;
                }
            }
            else if (Position.Y - Height / 2 < q.Position.Y + q.Height / 2)
            {
                collideY = true;
            }
            return collideX && collideY;
        }

        //Checks for collision after move
        public override bool CheckCollision(Quad q, Point vel)
        {
            bool collideX = false;
            bool collideY = false;

            if (Position.X + vel.X < q.Position.X )
            {
                if (Position.X + Width / 2  + vel.X > q.Position.X - q.Width / 2)
                {
                    collideX = true;
                }
            }
            else if (Position.X - Width / 2 + vel.X < q.Position.X + q.Width / 2)
            {
                collideX = true;
            }
            if (Position.Y + vel.Y < q.Position.Y)
            {
                if (Position.Y + Height / 2 + vel.Y > q.Position.Y - q.Height / 2)
                {
                    collideY = true;
                }
            }
            else if (Position.Y - Height / 2 + vel.Y < q.Position.Y + q.Height / 2)
            {
                collideY = true;
            }
            return collideX && collideY;
        }
        public bool CheckCollision(Point p)
        {
            bool collideX = false;
            bool collideY = false;

            if (Position.X < p.X)
            {
                if (Position.X + Width / 2 > p.X )
                {
                    collideX = true;
                }
            }
            else if (Position.X - Width / 2 < p.X)
            {
                collideX = true;
            }
            if (Position.Y < p.Y)
            {
                if (Position.Y + Height / 2 > p.Y)
                {
                    collideY = true;
                }
            }
            else if (Position.Y - Height / 2 < p.Y)
            {
                collideY = true;
            }
            return collideX && collideY;
        }

        public override void Flush(Quad q, Point vel, int i)
        {
            Point v = new Point(vel.X, vel.Y);
            if (v.X > 0)
            {
                v.X = (q.Position.X - q.Width/2) - (Position.X + Width / 2);
            }
            else if (v.X < 0)
            {
                v.X = (q.Position.X + q.Width / 2) - (Position.X - Width / 2);
            }
            if (v.Y > 0)
            {
                v.Y = (q.Position.Y - q.Height / 2) - (Position.Y + Height / 2);
            }
            else if (v.Y < 0)
            {
                v.Y = (q.Position.Y + q.Height / 2) - (Position.Y - Height / 2);
            }
            if(i > 0)
                Push(v, i - 1);
        }

        public override bool CheckCollision(Circle c, Point vel)
        {
            return c.CheckCollision(this, new Point(-vel.X, -vel.Y));
        }
        public override bool CheckCollision(Circle c)
        {
            return c.CheckCollision(this);
        }
    }
}
