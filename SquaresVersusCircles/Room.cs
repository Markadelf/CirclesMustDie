using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresVersusCircles
{
    class Room
    {
        public static Rectangle Screen { get; set; }
        public List<Quad> Mobile { get; set; }
        public List<Quad> Immobile { get; set; }
        public List<Circle> Entity { get; set; }
        public List<Quad> ProjectileSquare { get; set; }
        public List<Circle> ProjectileCircle { get; set; }
        public Quad Player { get; set; }
        public int RoomNumber { get; set; }
        private int delay;
        public bool IsPaused { get; set; }
        public bool CanPlay { get; set; }


        private bool heartbeat;

        public Room()
        {
            Initialize();
            RoomNumber = 1;
            IsPaused = true;
            CanPlay = true;
        }

        public void TestRoom()
        {
            Initialize();
            for (int i = 0; i < 15; i++)
                for(int j = 0; j < 10; j++)
                    Immobile.Add(new Quad('W', 32 * i + 16, 32 * 5, this));
            
            Immobile[16].MyColor = Color.YellowGreen;
            Immobile[17].MyColor = Color.YellowGreen;
            Immobile[18].MyColor = Color.YellowGreen;
            Immobile[31].MyColor = Color.YellowGreen;
            Immobile[46].MyColor = Color.YellowGreen;
            Immobile[47].MyColor = Color.YellowGreen;
            Immobile[48].MyColor = Color.YellowGreen;
            Immobile[61].MyColor = Color.YellowGreen;
            Immobile[76].MyColor = Color.YellowGreen;
            Immobile[77].MyColor = Color.YellowGreen;
            Immobile[78].MyColor = Color.YellowGreen;

            Player = new Quad('P', 0, 0, this);
            Player.Health = 0;

            Mobile.Add(new Quad('B', 32 * 3, 32, this));
        }

        public void Initialize()
        {
            Screen = new Rectangle(0, 0, 0, 0);
            heartbeat = true;
            Mobile = new List<Quad>();
            Immobile = new List<Quad>();
            ProjectileSquare = new List<Quad>();
            ProjectileCircle = new List<Circle>();
            Entity = new List<Circle>();
            Player = new Quad('P', 12, 12, this);
            delay = 100;
        }

        public void LoadRoom()
        {
            Initialize();
            StreamReader infile = null;
            try
            {
                if(File.Exists("Level" + RoomNumber + ".txt"))
                {
                    infile = new StreamReader("level" + RoomNumber + ".txt");
                    int x = 16;
                    int y = 16;
                    Rectangle r = new Rectangle(0, 0, 0, 0);
                    for(string s = infile.ReadLine(); s != "S"; s = infile.ReadLine())
                    {
                        for(int i = 0; i < s.Length; i++)
                        {
                            switch (s[i])
                            {
                                case 'P':
                                    Player = new Quad('P', x, y, this);
                                    break;
                                case 'W':
                                    Immobile.Add(new Quad('W', x, y, this));
                                    break;
                                case 'B':
                                    Mobile.Add(new Quad('B', x, y, this));
                                    break;
                                case 'R':
                                    Entity.Add(new Circle('R', x, y, this));
                                    break;
                                case 'T':
                                    Entity.Add(new Circle('T', x, y, this));
                                    break;
                                case 'V':
                                    Entity.Add(new Circle('V', x, y, this));
                                    break;
                                case 'N':
                                    Entity.Add(new Circle('N', x, y, this));
                                    break;
                            }
                            x += 32;
                            r.Width = x - 16;
                        }
                        x = 16;
                        y += 32;
                        r.Height = y - 16;
                    }
                    Screen = r;
                }
                else
                {
                    CanPlay = false;
                    IsPaused = true;
                    RoomNumber = -1;
                    Initialize();
                    Screen = new Rectangle(0, 0, 15 * 32, 10 * 32);
                }
            }
            catch(Exception e)
            {
                CanPlay = false;
                IsPaused = true;
                RoomNumber = -1;
                Initialize();
                Screen = new Rectangle(0, 0,15*32, 10*32);
            }
            finally
            {
                if(infile != null)
                {
                    infile.Close();
                }
            }
        }

        public void Update()
        {
            if (!IsPaused)
            {
                if (CheckVictory())
                {
                    delay--;
                    if(delay < 0)
                    {
                        Victory();
                    }
                }
                heartbeat = !heartbeat;
                for (int i = 0; i < Immobile.Count; i++)
                {
                    Immobile[i].Update();
                }
                for (int i = 0; i < Mobile.Count; i++)
                {
                    Mobile[i].Update();
                }
                for (int i = 0; i < ProjectileSquare.Count; i++)
                {
                    ProjectileSquare[i].Update();
                    for (int j = 0; j < Entity.Count; j++)
                    {
                        if (ProjectileSquare[i].CheckCollision(Entity[j]))
                        {
                            Entity[j].Health--;
                            ProjectileSquare[i].Health = 0;
                            if (Entity[j].Health <= 0)
                            {
                                ProjectileCircle.Add(new Circle(Entity[j].Position, new Point(3, -15), this));
                                ProjectileCircle[ProjectileCircle.Count - 1].Acceleration.Y = 4;
                                ProjectileCircle.Add(new Circle(Entity[j].Position, new Point(0, -15), this));
                                ProjectileCircle[ProjectileCircle.Count - 1].Acceleration.Y = 4;
                                ProjectileCircle.Add(new Circle(Entity[j].Position, new Point(-3, -15), this));
                                ProjectileCircle[ProjectileCircle.Count - 1].Acceleration.Y = 4;

                                Entity.RemoveAt(j);
                            }

                        }
                    }
                    if (heartbeat)
                        ProjectileSquare[i].Health--;
                    if (ProjectileSquare[i].Health <= 0)
                        ProjectileSquare.RemoveAt(i);              
                }
                for (int i = 0; i < ProjectileCircle.Count; i++)
                {
                    ProjectileCircle[i].Update();
                    if (ProjectileCircle[i].CheckCollision(Player))
                    {
                        Player.Health--;
                        ProjectileCircle[i].Health = 0;
                        if (Player.Health <= 0)
                        {
                            ProjectileSquare.Add(new Quad(Player.Position, new Point(3, -15), this));
                            ProjectileSquare[ProjectileSquare.Count - 1].Acceleration.Y = 4;
                            ProjectileSquare.Add(new Quad(Player.Position, new Point(0, -15), this));
                            ProjectileSquare[ProjectileSquare.Count - 1].Acceleration.Y = 4;
                            ProjectileSquare.Add(new Quad(Player.Position, new Point(-3, -15), this));
                            ProjectileSquare[ProjectileSquare.Count - 1].Acceleration.Y = 4;
                        }
                    }
                    if (heartbeat)
                        ProjectileCircle[i].Health--;
                    if (ProjectileCircle[i].Health <= 0)
                        ProjectileCircle.RemoveAt(i);                    
                    else
                        for (int j = 0; j < Mobile.Count; j++)
                        {
                            if (ProjectileCircle[i].CheckCollision(Mobile[j]))
                            {
                                ProjectileCircle.RemoveAt(i);
                                break;
                            }
                    }
                }
                for (int i = 0; i < Entity.Count; i++)
                {
                    Entity[i].Update();
                }
                if(Player.Health > 0)
                    Player.Update();
            }
        }
            

        public void Victory()
        {
            if (File.Exists("Level" + (RoomNumber + 1) + ".txt"))
                RoomNumber++;
            else
                IsPaused = true;
            LoadRoom();
        }

        public bool CheckVictory()
        {
            return Entity.Count == 0;
        }

    }
}
